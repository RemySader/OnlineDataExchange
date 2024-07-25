using System.Runtime.Serialization;

namespace OnlineDataExchange
{
    [DataContract]
    public class MakePaymentResponse
    {
        [DataMember]
        public string TransactionID { get; set; }
        [DataMember]
        public string ResponseCode { get; set; }

        [DataMember]
        public string ResponseText { get; set; }
        [DataMember]
        public string MSISDN { get; set; }
        [DataMember]
        public string PaymentID { get; set; }
        [DataMember]
        public string Currency { get; set; }
    }
}
