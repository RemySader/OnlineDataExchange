using OnlineDataExchange.Contracts;
using SoapCore;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Http;
using System.Xml.Linq;
using OnlineDataExchange.Interface;
using OnlineDataExchange.Repository;

namespace OnlineDataExchange.Services
{
    public class PinService : IPinService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFailedLoginTracker _failedLoginTracker;

        public PinService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IFailedLoginTracker failedLoginTracker)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _failedLoginTracker = failedLoginTracker;
        }

        public async Task<SendPinResponse> SendPin(SendPinRequest request)
        {
            string resp_code = "00";

            var username = _httpContextAccessor.HttpContext.Items["Username"]?.ToString();
            var password = _httpContextAccessor.HttpContext.Items["Password"]?.ToString();

            // Authenticate using SOAP header credentials
            if (username == null)
            {
                resp_code = "01";
            }

            if (request == null) throw new ArgumentNullException(nameof(request));

            // Check if username is locked
            _failedLoginTracker.UnlockAccount(username);
            if (_failedLoginTracker.IsAccountLocked(username)) resp_code = "12";

            // Check MSISDN
            if (resp_code == "00") resp_code = ValidationUtils.ValidateMSISDN(request.MSISDN);

            // Check Password
            if (resp_code == "00") resp_code = ValidationUtils.ValidatePassword(password);

            // Check credentials
            Authentication au = new Authentication(_configuration);
            if (resp_code == "00")
            {
                resp_code = au.AuthenticateUser(username, password);
                if (resp_code != "00")
                {
                    _failedLoginTracker.IncrementFailedAttempts(username);
                    int failedAttempts = _failedLoginTracker.GetFailedAttempts(username);
                    if (failedAttempts == 7) { 
                        _failedLoginTracker.LockAccount(username);
                        _failedLoginTracker.ResetFailedAttempts(username);
                        resp_code = "12";
                    }
                } 
                else
                {
                    _failedLoginTracker.ResetFailedAttempts(username);
                }
            }

            return new SendPinResponse
            {
                TransactionID = request.TransactionID,
                ResponseCode = resp_code,
                ResponseText = ResponseCodes.ResponsesDictionary[resp_code],
                MSISDN = request.MSISDN
            };
        }



    }
}
