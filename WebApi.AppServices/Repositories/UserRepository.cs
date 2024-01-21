using Microsoft.EntityFrameworkCore;
using WebApi.AppServices.Contracts.Models;
using WebApi.AppServices.Contracts.Models.Request;
using WebApi.AppServices.Contracts.Models.Responce;
using WebApi.AppServices.Contracts.Repositories;
using WebApi.AppServices.Contracts.Services.Convertors;
using WebApi.AppServices.Exceptions;
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
        
        if (_context.Users
                .Count(user => user.Username == username) != 0)
        {
            throw new Exception("Такой логин уже занят");
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

    public async Task UpdateUserInfo(
        string username,
        UpdateUserInfo userInfo,
        CancellationToken cancellationToken)
    {
        User? user =
            await _context.Users.FirstOrDefaultAsync(
                u => u.Username == username, cancellationToken);

        if (user is null)
        {
            throw new ResultException(
                $"Пользователя с именем {username} не существует");
        }

        if (userInfo.Username is not null)
        {
            if (_context.Users.Count(u => u.Username == userInfo.Username) != 0)
            {
                throw new ResultException("Это имя пользователя уже занято");
            }

            user.Username = userInfo.Username;
        }
        
        if (userInfo.Fio is not null)
        {
            user.Fio = userInfo.Fio;
        }

        if (userInfo.NewPassword is null && userInfo.OldPassword is not null ||
            userInfo.NewPassword is not null && userInfo.OldPassword is null)
        {
            throw new ResultException(
                "Необходимо ввести и новый и старый пароль");
        }

        if (userInfo.NewPassword is not null &&
            userInfo.OldPassword is not null)
        {
            if (user.Passord == userInfo.OldPassword)
            {
                user.Passord = userInfo.NewPassword;
            }
            else
            {
                throw new ResultException("Старый пароль не совпадает");
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<GetUserInfo> GetUserInfo(string username, CancellationToken cancellationToken)
    {
        User? user =
            await _context.Users.FirstOrDefaultAsync(
                u => u.Username == username, cancellationToken);

        if (user is null)
        {
            throw new ResultException(
                $"Пользователя с именем {username} не существует");
        }

        return GetUserInfoConverter.FromDomainModel(user);
    }

    public async Task UpdateImage(string username, string imgPath,
        CancellationToken cancellationToken)
    {
        User? user =
            await _context.Users.FirstOrDefaultAsync(
                u => u.Username == username, cancellationToken);

        if (user is null)
        {
            throw new ResultException(
                $"Пользователя с именем {username} не существует");
        }

        user.ImageUrl = imgPath;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
