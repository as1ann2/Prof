using System;
using System.Collections.Generic;

namespace ToirProfAPI.Models;

public partial class PlanSchedule
{
    public int Id { get; set; }

    public int? EquipmentId { get; set; }

    public int? TypeMaintenanceId { get; set; }

    public DateOnly PlanDate { get; set; }

    public DateOnly FactCompletionDate { get; set; }

    public int? RepairCrewId { get; set; }

    public string Status { get; set; } = null!;

    public virtual Equipment? Equipment { get; set; }

    public virtual RepairCrew? RepairCrew { get; set; }

    public virtual TypeMaintenance? TypeMaintenance { get; set; }

    public virtual ICollection<WorkOrder> WorkOrders { get; set; } = new List<WorkOrder>();
}
