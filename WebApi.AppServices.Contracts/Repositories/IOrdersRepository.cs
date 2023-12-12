using WebApi.AppServices.Models;
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
}
