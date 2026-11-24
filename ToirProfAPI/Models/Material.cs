using System;
using System.Collections.Generic;

namespace ToirProfAPI.Models;

public partial class Material
{
    public int Id { get; set; }

    public string? Article { get; set; }

    public string Name { get; set; } = null!;

    public string Unit { get; set; } = null!;

    public int Stock { get; set; }

    public int MinStock { get; set; }

    public string? StorageLocation { get; set; }

    public decimal Price { get; set; }

    public int Quantity { get; set; }
}
