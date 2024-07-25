using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace OnlineDataExchange
{
    public class SoapHeaderMiddleware
    {
        private readonly RequestDelegate _next;

        public SoapHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.ContentType != null && context.Request.ContentType.Contains("text/xml"))
            {
                context.Request.EnableBuffering();
                var requestBodyStream = new MemoryStream();
                await context.Request.Body.CopyToAsync(requestBodyStream);
                requestBodyStream.Seek(0, SeekOrigin.Begin);

                var xmlDoc = new XmlDocument();
                requestBodyStream.Seek(0, SeekOrigin.Begin);

                // Load the XML document with the necessary settings to handle namespaces
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.DtdProcessing = DtdProcessing.Ignore;

                using (XmlReader reader = XmlReader.Create(requestBodyStream, settings))
                {
                    xmlDoc.Load(reader);

                    // Create namespace manager and add required namespaces
                    var namespaceManager = new XmlNamespaceManager(xmlDoc.NameTable);
                    namespaceManager.AddNamespace("soapenv", "http://schemas.xmlsoap.org/soap/envelope/");
                    namespaceManager.AddNamespace("wsse", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
                    namespaceManager.AddNamespace("tem", "http://tempuri.org/");
                    namespaceManager.AddNamespace("us", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#UsernameToken");
                    namespaceManager.AddNamespace("onl", "http://schemas.datacontract.org/2004/07/OnlineDataExchange");

                    // Example XPath to retrieve username and password
                    var usernameNode = xmlDoc.SelectSingleNode("//us:Username", namespaceManager);
                    var passwordNode = xmlDoc.SelectSingleNode("//us:Password", namespaceManager);

                    if (usernameNode != null && passwordNode != null)
                    {
                        context.Items["Username"] = usernameNode.InnerText;
                        context.Items["Password"] = passwordNode.InnerText;
                    }

                    requestBodyStream.Seek(0, SeekOrigin.Begin);
                    context.Request.Body = requestBodyStream;
                }
            }

            await _next(context);
        }
    }
}
