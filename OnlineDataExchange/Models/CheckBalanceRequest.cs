using System.Runtime.Serialization;

namespace OnlineDataExchange
{
    [DataContract]
    public class CheckBalanceRequest
    {
        [DataMember(Order = 1, IsRequired = true)]
        public string TransactionID { get; set; }
        /*[DataMember(Order = 2, IsRequired = true)]
        public string Username { get; set; }

        [DataMember(Order = 3, IsRequired = true)]
        public string Password { get; set; }*/
        [DataMember(Order = 4, IsRequired = true)]
        public string MSISDN { get; set; }
        [DataMember(Order = 5, IsRequired = false)]
        public string PIN { get; set; }
    }
}
