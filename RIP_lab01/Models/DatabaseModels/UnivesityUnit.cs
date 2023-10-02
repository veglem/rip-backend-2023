using System;
using System.Collections.Generic;

namespace RIP_lab01;

public partial class UnivesityUnit
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string ImgUrl { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

    public virtual ICollection<UniversityEmployee> UniversityEmployees { get; set; } = new List<UniversityEmployee>();
}
