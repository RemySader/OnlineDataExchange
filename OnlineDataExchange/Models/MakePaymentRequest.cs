using System.Runtime.Serialization;

namespace OnlineDataExchange
{
    [DataContract]
    public class MakePaymentRequest
    {
        [DataMember(Order = 1, IsRequired = true)]
        public string TransactionID { get; set; }
        /*[DataMember(Order = 2, IsRequired = true)]
        public string Username { get; set; }
        [DataMember(Order = 3, IsRequired = true)]
        public string Password { get; set; }*/
        [DataMember(Order = 4, IsRequired = true)]
        public string Currency { get; set; }
        [DataMember(Order = 5, IsRequired = true)]
        public bool Retry { get; set; } = false;
        [DataMember(Order = 6, IsRequired = true)]
        public decimal Balance { get; set; }
        [DataMember(Order = 7, IsRequired = true)]
        public string ReasonCode { get; set; } = "Invoice settlement";
        [DataMember(Order = 8, IsRequired = true)]
        public string UniqueReferenceNumber { get; set; }
        /*[DataMember(Order = 9, IsRequired = true)]
        public DateTime PaymentDate { get; set; } = DateTime.Now;*/
        [DataMember(Order = 10, IsRequired = true)]
        public string MSISDN { get; set; }
        [DataMember(Order = 11, IsRequired = false)]
        public string PaymentLocation { get; set; }
    }
}
