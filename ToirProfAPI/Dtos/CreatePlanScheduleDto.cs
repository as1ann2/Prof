namespace ToirProfAPI.Dtos;

public class CreatePlanScheduleDto
{
    public int EquipmentId { get; set; }
    public int TypeMaintenanceId { get; set; }
    public DateOnly PlanDate { get; set; }
}