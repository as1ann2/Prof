using System;
using System.Collections.Generic;

namespace ToirProfAPI.Models;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string PasswordHash { get; set; } = null!;

    public string Role { get; set; } = null!;

    public DateTime RegisterDate { get; set; }

    public string Status { get; set; } = null!;

    public int? WorkshopId { get; set; }

    public int? RepairCrewId { get; set; }

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public virtual ICollection<OperationLog> OperationLogs { get; set; } = new List<OperationLog>();

    public virtual RepairCrew? RepairCrew { get; set; }

    public virtual ICollection<WorkOrder> WorkOrders { get; set; } = new List<WorkOrder>();

    public virtual Workshop? Workshop { get; set; }
}
