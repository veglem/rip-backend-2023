using WebApi.AppServices.Models;

namespace WebApi.AppServices.Contracts.Handlers;

public interface IOrdersHandler
{
    public Task<List<GetUnitResult>> AddUnitToOrder(int unitId, string username,
        CancellationToken cancellationToken);
}
