using WebApi.AppServices.Contracts.Handlers;
using WebApi.AppServices.Contracts.Repositories;
using WebApi.DataAccess;

namespace WebApi.AppServices.Handlers;

public class UserHandler : IUserHandler
{
    private IUserRepository _userRepository;

    public UserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> GetUserByLoginAndPass(string username, string password,
        CancellationToken cancellationToken)
    {
        return await _userRepository.GetUserByUsernameAndPassword(username,
            password, cancellationToken);
    }

    public async Task<User> AddUser(string username, string password,
        CancellationToken cancellationToken)
    {
        return await _userRepository.AddUser(username, password, cancellationToken);
    }
}
