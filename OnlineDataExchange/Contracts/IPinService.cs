using System.ServiceModel;
using System.Threading.Tasks;

namespace OnlineDataExchange.Contracts
{
    [ServiceContract]
    public interface IPinService
    {
        [OperationContract]
        Task<SendPinResponse> SendPin(SendPinRequest request);
    }
}