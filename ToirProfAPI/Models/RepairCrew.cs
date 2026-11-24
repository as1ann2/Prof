using System;
using System.Collections.Generic;

namespace ToirProfAPI.Models;

public partial class RepairCrew
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Specialization { get; set; } = null!;

    public int? WorkshopId { get; set; }

    public virtual ICollection<PlanSchedule> PlanSchedules { get; set; } = new List<PlanSchedule>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public virtual ICollection<WorkOrder> WorkOrders { get; set; } = new List<WorkOrder>();

    public virtual Workshop? Workshop { get; set; }
}
