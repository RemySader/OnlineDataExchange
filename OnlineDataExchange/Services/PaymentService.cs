using OnlineDataExchange.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using OnlineDataExchange.Interface;
using OnlineDataExchange.Repository;

namespace OnlineDataExchange.Services
{
    public class PaymentService : IPaymentService
    {

        private readonly IBalanceService _balanceService;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFailedLoginTracker _failedLoginTracker;

        public PaymentService(IBalanceService balanceService, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IFailedLoginTracker failedLoginTracker)
        {
            _balanceService = balanceService;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _failedLoginTracker = failedLoginTracker;
        }
        public async Task<MakePaymentResponse> MakePayment(MakePaymentRequest request)
        {
            string resp_code = "00";
            var username = _httpContextAccessor.HttpContext.Items["Username"]?.ToString();
            var password = _httpContextAccessor.HttpContext.Items["Password"]?.ToString();

            // Authenticate using SOAP header credentials
            if (username == null)
            {
                resp_code = "01"; // Unauthorized
            }

            if (request == null) throw new ArgumentNullException(nameof(request));

            // Wrong Currency
            if (resp_code == "00")
            {
                if (request.Currency != "USD" && request.Currency != "LBP") resp_code = "01";
            }

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

            // Check Balance
            if (resp_code == "00")
            {
                CheckBalanceResponse balance_resp = null;
                try
                {
                    CheckBalanceRequest balance_req = new CheckBalanceRequest
                    {
                        TransactionID = request.TransactionID,
                        MSISDN = request.MSISDN,
                    };

                    balance_resp = await _balanceService.CheckBalance(balance_req);
                    decimal balance_decimal = decimal.Parse(balance_resp.Balance);
                    balance_decimal = request.Balance;
                    if (balance_decimal != request.Balance) resp_code = "09";
                }
                catch (Exception ex)
                {
                    resp_code = "10";
                    Console.WriteLine($"Exception occurred while checking balance: {ex.Message}");
                }
            }

            // Random PaymentID
            Random random = new Random();
            double paymentID = random.NextDouble() * 100; // Between 0 and 100 Random
            string paymentIDStr = paymentID.ToString("F0");

            return new MakePaymentResponse
            {
                TransactionID = request.TransactionID,
                ResponseCode = resp_code,
                ResponseText = ResponseCodes.ResponsesDictionary[resp_code],
                MSISDN = request.MSISDN,
                PaymentID = paymentIDStr,
                Currency = request.Currency
            };
        }
    }
}
