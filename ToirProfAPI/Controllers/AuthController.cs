using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToirProfAPI.Dtos;
using ToirProfAPI.Models;
using ToirProfAPI.Services;

namespace ToirProfAPI.Controllers
{
    [ApiController]
    [Route("auth")]
    
    public class AuthController : ControllerBase
    {
        private readonly ToirProfContext _context;
        private readonly ITokenService _tokenService;
        
        public  AuthController(ToirProfContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            // var user = new User();
            StringBuilder sb = new StringBuilder();
            using (MD5 md5 = MD5.Create())
            {
                // Преобразуем строку пароля в байты
                var passwordBytes = Encoding.UTF8.GetBytes(dto.Password); 

                // Вычисляем хеш
                var hashBytes = md5.ComputeHash(passwordBytes); 

                // Преобразуем байты хеша в строку (каждая пара байтов — это две шестнадцатеричные цифры)
                
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null || sb.ToString() != user.PasswordHash)
                return Unauthorized(ApiResponce<object>.Fail("Неверный email или пароль"));
            if (user.Status != "активен")
                return Unauthorized(ApiResponce<object>.Fail("Пользователь заблокирован"));
            return Ok(ApiResponce<object>.Ok(new { token = _tokenService.GenerateJwtToken(user) }));
        }
    }
}
