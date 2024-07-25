using Microsoft.Extensions.Configuration;
using OnlineDataExchange.Contracts;
using OnlineDataExchange.Interface;
using OnlineDataExchange.Repository;

namespace OnlineDataExchange.Services
{
    public class BalanceService : IBalanceService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFailedLoginTracker _failedLoginTracker;

        public BalanceService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IFailedLoginTracker failedLoginTracker)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _failedLoginTracker = failedLoginTracker;
        }
        public async Task<CheckBalanceResponse> CheckBalance(CheckBalanceRequest request)
        {
            double balance = 0;
            string balanceStr = "";

            double sayrafaRate = 0;
            string sayrafaRateStr = "";

            double balanceInLBP = 0;
            string balanceInLBPStr = "";

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
                    if (failedAttempts == 7)
                    {
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

            if (resp_code == "00")
            {
                // Random Balance
                Random random = new Random();
                balance = random.NextDouble() * 100; // Between 0 and 100 Random
                balanceStr = balance.ToString("F2");

                // Sayrafa rate
                sayrafaRate = 90000;
                sayrafaRateStr = sayrafaRate.ToString();

                // Calculate BalanceInLBP
                balanceInLBP = balance * sayrafaRate;
                balanceInLBPStr = balanceInLBP.ToString("F2");
            }

            return new CheckBalanceResponse
            {
                TransactionID = request.TransactionID,
                ResponseCode = resp_code,
                ResponseText = ResponseCodes.ResponsesDictionary[resp_code],
                MSISDN = request.MSISDN,
                Currency = "USD",
                Balance = balanceStr,
                BalanceInLBP = balanceInLBPStr,
                SayrafaRate = sayrafaRateStr
            };
        }
    }
}