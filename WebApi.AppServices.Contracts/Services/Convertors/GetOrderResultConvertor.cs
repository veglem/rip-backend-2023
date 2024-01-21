using WebApi.AppServices.Contracts.Models.Responce;
using WebApi.DataAccess;

namespace WebApi.AppServices.Contracts.Services.Convertors;

public class GetOrderResultConvertor
{
    public static GetOrderResult FromDomainModel(RectorOrder order)
    {
        return new GetOrderResult()
        {
            Id = order.Id,
            Name = order.Name,
            OrderBody = order.OrderBody,
            CreationDate = order.CreationDate,
            FormationDate = order.FormationDate,
            EndDate = order.EndDate,
            Creator = order.Creator.Username,
            Moderator = order.Moderator?.Username,
            Status = order.Status.Name,
            Units = order.Requests.Select(request =>
                request.UnitId).ToList()
        };
    }
}
