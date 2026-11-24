using System;
using System.Collections.Generic;

namespace ToirProfAPI.Models;

public partial class Equipment
{
    public int Id { get; set; }

    public string InventoryNumber { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Model { get; set; } = null!;

    public string? SerialNumber { get; set; }

    public string Manufacturer { get; set; } = null!;

    public DateOnly CommisioningDate { get; set; }

    public DateOnly AtWorkDate { get; set; }

    public DateOnly? DecommisioningDate { get; set; }

    public int? WorkshopId { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public DateTime LastUpdate { get; set; }

    public virtual ICollection<HistoryEquipment> HistoryEquipments { get; set; } = new List<HistoryEquipment>();

    public virtual ICollection<PlanSchedule> PlanSchedules { get; set; } = new List<PlanSchedule>();

    public virtual ICollection<WorkOrder> WorkOrders { get; set; } = new List<WorkOrder>();

    public virtual Workshop? Workshop { get; set; }
}
