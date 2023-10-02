using System;
using System.Collections.Generic;

namespace RIP_lab01;

public partial class RectorOrder
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? OrderBody { get; set; }

    public int? StatusId { get; set; }

    public DateTime? CreationDate { get; set; }

    public DateTime? FormationDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int? ModeratorId { get; set; }

    public virtual Moderator? Moderator { get; set; }

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

    public virtual Status? Status { get; set; }
}
