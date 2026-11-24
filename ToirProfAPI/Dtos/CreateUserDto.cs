namespace ToirProfAPI.Dtos;

public class CreateUserDto
{
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public string LastName { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string? MiddleName { get; set; }
    public string Password { get; set; } = null!;
    public string Role { get; set; } = null!;
    public int? WorkshopId { get; set; }
    public int? RepairCrewId { get; set; }   
}