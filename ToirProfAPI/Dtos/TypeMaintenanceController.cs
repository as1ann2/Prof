namespace ToirProfAPI.Dtos;

public class TypeMaintenanceDto
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int PeriodDay { get; set; }
    public int PlanTimeInHours { get; set; }
    public bool StopEquipment { get; set; }
    public bool IsActive { get; set; }
}