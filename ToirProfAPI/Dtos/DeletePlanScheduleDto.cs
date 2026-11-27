using System.ComponentModel.DataAnnotations;

namespace ToirProfAPI.Dtos;

public class DeletePlanScheduleDto
{
    [Required(ErrorMessage = "Укажите причину удаления")]
    [StringLength(500, ErrorMessage = "Причина не должна превышать 500 символов")]
    public string Reason { get; set; } = null!;
}