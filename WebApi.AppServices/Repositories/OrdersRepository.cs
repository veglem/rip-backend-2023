using WebApi.AppServices.Contracts.Repositories;
using WebApi.DataAccess;

namespace WebApi.AppServices.Repositories;

public class OrdersRepository : IOrdersRepository
{
    private RectorOrdersDatabaseContext _context;

    public OrdersRepository(RectorOrdersDatabaseContext context)
    {
        _context = context;
    }
    
}
