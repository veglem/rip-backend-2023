using Microsoft.EntityFrameworkCore;
using WebApi.AppServices.Contracts.Repositories;
using WebApi.DataAccess;

namespace WebApi.AppServices.Repositories;

public class UserRepository : IUserRepository
{
    private RectorOrdersDatabaseContext _context;

    public UserRepository(RectorOrdersDatabaseContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUserByUsernameAndPassword(string username,
        string password, CancellationToken cancellationToken)
    {
        return await _context.Users.FirstOrDefaultAsync(user =>
                user.Username == username && user.Passord == password,
            cancellationToken);
    }

    public async Task<User> AddUser(string username, string password,
        CancellationToken cancellationToken)
    {
        if (password.Length < 5)
        {
            throw new ArgumentException("пароль слишком короткий");
        }

        try
        {
            var user = await _context.Users.AddAsync(new User()
            {
                Username = username,
                Passord = password
            }, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return user.Entity;
        }
        catch(Exception ex)
        {
            throw new ArgumentException(ex.Message);
        }
    }
}
