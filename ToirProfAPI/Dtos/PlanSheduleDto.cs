namespace ToirProfAPI.Dtos;

public class PlanScheduleDto
{
    public int Id { get; set; }

    public int? EquipmentId { get; set; }
    public string? EquipmentName { get; set; }

    public int? TypeMaintenanceId { get; set; }
    public string? TypeMaintenanceName { get; set; }

    public DateOnly PlanDate { get; set; }
    public DateOnly? FactCompletionDate { get; set; }

    public int? RepairCrewId { get; set; }
    public string Status { get; set; } = null!;
}