using WebApi.AppServices.Contracts.Models;
using WebApi.AppServices.Contracts.Models.Responce;

namespace WebApi.AppServices.Contracts.Services.Convertors;

public class GetUserInfoConverter
{
    public static GetUserInfo FromDomainModel(User user)
    {
        return new GetUserInfo()
        {
            Username = user.Username,
            IsModerator = user.IsModerator,
            ImageUrl = user.ImageUrl,
            Fio = user.Fio
        };
    }
}
