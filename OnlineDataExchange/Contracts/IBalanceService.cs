using System.ServiceModel;

namespace OnlineDataExchange.Contracts
{
    [ServiceContract]
    public interface IBalanceService
    {
        [OperationContract]
        Task<CheckBalanceResponse> CheckBalance(CheckBalanceRequest request);
    }
}
