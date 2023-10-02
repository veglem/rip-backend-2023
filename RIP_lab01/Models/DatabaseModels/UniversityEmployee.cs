using System;
using System.Collections.Generic;

namespace RIP_lab01;

public partial class UniversityEmployee
{
    public int Id { get; set; }

    public string? Division { get; set; }

    public string? FullName { get; set; }

    public string? Position { get; set; }

    public string? Number { get; set; }

    public string? Email { get; set; }

    public string? Cabinet { get; set; }

    public string? Location { get; set; }

    public int? UnitId { get; set; }

    public virtual UnivesityUnit? Unit { get; set; }
}
