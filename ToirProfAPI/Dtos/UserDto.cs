namespace ToirProfAPI.Dtos;

public class UserDto
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public string LastName { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string? MiddleName { get; set; }
    public string Role { get; set; } = null!;
    public string Status { get; set; }      
    public DateTime RegisterDate { get; set; }    
}