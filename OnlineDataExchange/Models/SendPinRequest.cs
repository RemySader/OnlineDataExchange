using System.Runtime.Serialization;

namespace OnlineDataExchange
{
    [DataContract]
    public class SendPinRequest
    {
        [DataMember(Order = 1, IsRequired = true)]
        public string TransactionID { get; set; }
        /*[DataMember(Order = 2, IsRequired = true)]
        public string Username { get; set; }

        [DataMember(Order = 3, IsRequired = true)]
        public string Password { get; set; }*/
        [DataMember(Order = 2, IsRequired = true)]
        public string MSISDN { get; set; }
    }
}
