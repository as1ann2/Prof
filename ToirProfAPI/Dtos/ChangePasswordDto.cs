namespace ToirProfAPI.Dtos;

public class ChangePasswordDto
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}