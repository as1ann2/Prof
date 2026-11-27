using Humanizer;
using ToirProfAPI.Models;

namespace ToirProfAPI.Dtos;

public class CreateTypeMaintenanceDto
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string? Description {get; set;}
    public int PeriodDay { get; set; }
    public int PlanTimeInHours { get; set; }
    
    public bool StopEquipment { get; set; }
    public bool IsActive { get; set; }

}