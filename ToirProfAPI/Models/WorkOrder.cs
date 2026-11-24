using System;
using System.Collections.Generic;

namespace ToirProfAPI.Models;

public partial class WorkOrder
{
    public int Id { get; set; }

    public string Number { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string? Priority { get; set; }

    public string Description { get; set; } = null!;

    public string ReasonRefusal { get; set; } = null!;

    public int EquipmentId { get; set; }

    public int? CreatedBy { get; set; }

    public int? RepairCrewsId { get; set; }

    public int? PlanScheduleId { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? DateCreate { get; set; }

    public DateTime? PlannedDateStart { get; set; }

    public DateTime? FactDateStart { get; set; }

    public DateTime? PlannedDateEnd { get; set; }

    public DateTime? FactDateEnd { get; set; }

    public string? Report { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual Equipment Equipment { get; set; } = null!;

    public virtual PlanSchedule? PlanSchedule { get; set; }

    public virtual RepairCrew? RepairCrews { get; set; }
}
