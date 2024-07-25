using System.Runtime.Serialization;

namespace OnlineDataExchange
{
    [DataContract]
    public class CheckBalanceResponse
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
        public string Currency { get; set; }
        [DataMember]
        public string Balance { get; set; }
        [DataMember]
        public string BalanceInLBP { get; set; }
        [DataMember]
        public string SayrafaRate { get; set; }
    }
}
