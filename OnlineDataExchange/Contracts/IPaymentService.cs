using System.ServiceModel;

namespace OnlineDataExchange.Contracts
{
    [ServiceContract]
    public interface IPaymentService
    {
        [OperationContract]
        Task<MakePaymentResponse> MakePayment(MakePaymentRequest request);
    }
}
