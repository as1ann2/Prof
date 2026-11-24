using System;
using System.Collections.Generic;

namespace ToirProfAPI.Models;

public partial class Workshop
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<Equipment> Equipment { get; set; } = new List<Equipment>();

    public virtual ICollection<RepairCrew> RepairCrews { get; set; } = new List<RepairCrew>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
