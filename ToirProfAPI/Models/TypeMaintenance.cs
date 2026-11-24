using System;
using System.Collections.Generic;

namespace ToirProfAPI.Models;

public partial class TypeMaintenance
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int PeriodDay { get; set; }

    public int PlanTimeInHours { get; set; }

    public bool? StopEquiment { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<PlanSchedule> PlanSchedules { get; set; } = new List<PlanSchedule>();
}
