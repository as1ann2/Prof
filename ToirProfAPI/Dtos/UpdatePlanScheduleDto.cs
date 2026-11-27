namespace ToirProfAPI.Dtos;

public class UpdatePlanScheduleDto
{
    public int TypeMaintenanceId { get; set; }
    public DateOnly PlanDate { get; set; }
}