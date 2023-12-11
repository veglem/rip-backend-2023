using WebApi.DataAccess;

namespace WebApi.AppServices.Contracts.Repositories;

public interface IUserRepository
{
    public Task<User?> GetUserByUsernameAndPassword(string username,
        string password, CancellationToken cancellationToken);
    
    public Task<User> AddUser(string username,
        string password, CancellationToken cancellationToken);
}
