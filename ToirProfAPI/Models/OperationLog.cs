using System;
using System.Collections.Generic;

namespace ToirProfAPI.Models;

public partial class OperationLog
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string? TypeEntity { get; set; }

    public int EntityId { get; set; }

    public string Action { get; set; } = null!;

    public string? OldValue { get; set; }

    public string? NewValue { get; set; }

    public DateTime DateOperation { get; set; }

    public virtual User? User { get; set; }
}
