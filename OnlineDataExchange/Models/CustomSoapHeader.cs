/*using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml;


namespace OnlineDataExchange
{
    [MessageContract]
    public class CustomSoapHeader : MessageHeader
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public override string Name => "Security";
        public override string Namespace => "http://schemas.xmlsoap.org/ws/2002/07/secext";

        protected override void OnWriteHeaderContents(XmlDictionaryWriter writer, MessageVersion messageVersion)
        {
            throw new NotImplementedException();
        }
    }
}*/