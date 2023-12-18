using WebApi.AppServices.Contracts.Models.Request;
using WebApi.AppServices.Contracts.Models.Responce;
using WebApi.DataAccess;

namespace WebApi.AppServices.Contracts.Repositories;

public interface IOrdersRepository
{
    public Task<List<RectorOrder>> GetOrdersByUser(string username,
        CancellationToken cancellationToken);

    public Task<RectorOrder> AddNewOrder(RectorOrder order,
        CancellationToken cancellationToken);
    
    public Task<List<GetUnitResult>> AddUnitToOrder(string username, int unitId,
        CancellationToken cancellationToken);

    public Task<GetOrderResult> GetOrderById(string username, int orderId,
        CancellationToken cancellationToken);

    public Task<GetOrderResult> UpdateOrder(int orderId, string username,
        UpdateOrderRequest request, CancellationToken cancellationToken);
    
    public Task<GetOrderResult> UpdateStatusUser(int orderId, string username,
        string status, CancellationToken cancellationToken);

    public Task<GetOrderResult> DeleteUnitFromOrder(
        int orderId,
        int unitId,
        string username,
        CancellationToken cancellationToken);
}
