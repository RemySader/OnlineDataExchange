/*using Microsoft.AspNetCore.Http;
using SoapCore.Extensibility;
using System.IO;
using System.ServiceModel.Channels;
using System.Xml;

namespace OnlineDataExchange
{
    public class CustomMessageInspector : IMessageInspector
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomMessageInspector(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public object AfterReceiveRequest(ref Message message)
        {
            var headers = message.Headers;
            var securityHeaderIndex = headers.FindHeader("Security", "http://schemas.xmlsoap.org/ws/2002/07/secext");

            if (securityHeaderIndex >= 0)
            {
                var securityHeader = headers.GetReaderAtHeader(securityHeaderIndex);
                if (securityHeader.ReadToDescendant("UsernameToken", "http://schemas.xmlsoap.org/ws/2002/07/secext"))
                {
                    var username = securityHeader.ReadElementContentAsString("Username", "http://schemas.xmlsoap.org/ws/2002/07/secext");
                    var password = securityHeader.ReadElementContentAsString("Password", "http://schemas.xmlsoap.org/ws/2002/07/secext");

                    // Store the credentials in the HttpContext for later use
                    _httpContextAccessor.HttpContext.Items["Username"] = username;
                    _httpContextAccessor.HttpContext.Items["Password"] = password;
                }
            }

            return null;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
        }
    }
}
*/