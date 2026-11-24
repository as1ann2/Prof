using System;
using System.Collections.Generic;

namespace ToirProfAPI.Models;

public partial class HistoryEquipment
{
    public int Id { get; set; }

    public int? EquipmentId { get; set; }

    public string OldStatus { get; set; } = null!;

    public string NewStatus { get; set; } = null!;

    public DateTime DateChange { get; set; }

    public int UserId { get; set; }

    public virtual Equipment? Equipment { get; set; }
}
