using OnlineDataExchange.Contracts;
using System;
using System.Configuration;
using System.Text.RegularExpressions;

namespace OnlineDataExchange
{
    public static class ValidationUtils
    {
        private static readonly HashSet<string> AllowedPrefixes = new HashSet<string>
        {
            "03", "70", "71", "76", "78", "79", "81"
        };

        public static string ValidateMSISDN(string msisdn)
        {
            if (msisdn == null || msisdn.Length != 8)
            {
                return "01"; // Response Code
            }

            string firstTwoChars = msisdn.Substring(0, 2);

            if (!AllowedPrefixes.Contains(firstTwoChars))
            {
                return "01"; // Response Code
            }

            return "00"; // Success
        }

        public static string ValidatePassword(string password) {
            var regexPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!""#$%&'()*+,\-./:;<=>?@[\\\]^_`{|}~])[a-zA-Z\d!""#$%&'()*+,\-./:;<=>?@[\\\]^_`{|}~]{15,}$";
            var regex = new Regex(regexPattern);
            if (regex.IsMatch(password))
            {
                return "00";
            }
            else
            {
                return "01";
            }
        }
    }

    public static class ResponseCodes
    {
        public static readonly Dictionary<string, string> ResponsesDictionary = new Dictionary<string, string>
        {
            { "00", "Success" },
            { "01", "Wrong or Missing Input Parameters" },
            { "02", "Authentication error" },
            { "03", "Customer not allowed to pay invoice at POS" },
            { "04", "Duplicate UniqueReferenceNumber" },
            { "05", "Wrong Balance amount" },
            { "06", "Invalid PaymentDate format" },
            { "07", "Missing PIN Parameter" },
            { "08", "Invalid Pin" },
            { "09", "Not Enough Balance to be paid" },
            { "10", "Internal Error" },
            { "11", "Maximum PIN request attempts is reached" },
            { "12", "Account is locked" }
        };
    }

    public class Authentication
    {
        private readonly IConfiguration _configuration;

        public Authentication(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string AuthenticateUser(string u, string p)
        {
            /*string passwordFromConfig = ConfigurationManager.

            if (passwordFromConfig == null)
            {
                return false;
            }

            return password == passwordFromConfig;*/

            // Check username and password
            string password = _configuration["AppSettings:" + u];

            if (string.IsNullOrEmpty(password)) return "02";
            else if (password != p) return "02";
            else return "00";
        }

    }
}
