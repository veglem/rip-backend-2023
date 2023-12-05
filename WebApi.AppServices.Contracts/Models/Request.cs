using System;
using System.Collections.Generic;
using WebApi.AppServices.Contracts.Models;

namespace WebApi.DataAccess;

public class Request
{
    public int Id { get; set; }

    public int UnitId { get; set; }

    public int OrderId { get; set; }

    public RectorOrder Order { get; set; } = null!;

    public UnivesityUnit Unit { get; set; } = null!;
}
