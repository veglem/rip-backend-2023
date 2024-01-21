using System;
using System.Collections.Generic;
using WebApi.AppServices.Contracts.Models;

namespace WebApi.DataAccess;

public class RectorOrder
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string OrderBody { get; set; } = null!;

    public int StatusId { get; set; }

    public DateTime? CreationDate { get; set; }

    public DateTime? FormationDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int? ModeratorId { get; set; }

    public int CreatorId { get; set; }

    public User Creator { get; set; } = null!;

    public User? Moderator { get; set; }

    public ICollection<Request> Requests { get; } = new List<Request>();

    public Status Status { get; set; } = null!;
}
