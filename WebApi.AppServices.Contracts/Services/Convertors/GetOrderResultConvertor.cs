using System.Diagnostics;
using WebApi.AppServices.Contracts.Models.Responce;
using WebApi.DataAccess;

namespace WebApi.AppServices.Contracts.Services.Convertors;

public class GetOrderResultConvertor
{
    public static GetOrderResult FromDomainModel(RectorOrder order)
    {
        GetOrderResult result = new GetOrderResult()
        {
            Id = order.Id,
            Name = order.Name,
            OrderBody = order.OrderBody,
            CreationDate = order.CreationDate,
            FormationDate = order.FormationDate,
            EndDate = order.EndDate,
            Creator = order.Creator.Username,
            Moderator = order.Moderator?.Username,
            Units = order.Requests.Select(request =>
                request.UnitId).ToList()
        };

        switch (order.Status.Name)
        {
            case "deleted":
                result.Status = 5;
                break;
            case "draft":
                result.Status = 1;
                break;
            case "formed":
                result.Status = 2;
                break;
            case "completed":
                result.Status = 3;
                break;
            case "rejected":
                result.Status = 4;
                break;
        }
        
        return result;
    }
}
