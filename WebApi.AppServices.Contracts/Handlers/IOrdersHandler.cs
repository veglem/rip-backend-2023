using WebApi.AppServices.Contracts.Models.Request;
using WebApi.AppServices.Contracts.Models.Responce;

namespace WebApi.AppServices.Contracts.Handlers;

public interface IOrdersHandler
{
    public Task<List<GetUnitResult>> AddUnitToOrder(int unitId, string username,
        CancellationToken cancellationToken);

    public Task<List<GetOrderResult>> GetUserOrders(string username,
        CancellationToken cancellationToken);

    public Task<GetOrderResult> GetOrderById(string username, int orderId,
        CancellationToken cancellationToken);

    public Task<GetOrderResult> UpdateOrder(int orderId, string username, UpdateOrderRequest request,
        CancellationToken cancellationToken);
    
    public Task<GetOrderResult> UpdateStatusUser(int orderId, string username, string status,
        CancellationToken cancellationToken);

    public Task<GetOrderResult> DeleteUnitFromOrder(
        int orderId,
        int unitId,
        string username,
        CancellationToken cancellationToken);

    public Task UpdateSignature(int orderId, string signature);
}
