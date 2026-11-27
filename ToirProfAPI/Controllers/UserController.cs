using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToirProfAPI.Dtos;
using ToirProfAPI.Models;
using System.Text;
using System.Security.Cryptography;
using ToirProfAPI.Services;

namespace ToirProfAPI.Controllers
{
    [Authorize(Roles = "администратор")]
    [ApiController]
    [Route("api/[controller]")]

    public class UserController : ControllerBase
    {

        private readonly ToirProfContext _context;
        private readonly ITokenService _tokenService;

        public UserController(ToirProfContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto dto)
        {


            if (string.IsNullOrEmpty(dto.Password) || dto.Password.Length < 8)
                return BadRequest(ApiResponce<object>.Fail("Пароль должен быть от 8 символов", "VALIDATION_ERROR"));
            var roles = new[] { "администратор", "инженер", "мастер", "член бригады", "руководитель" };
            if (!roles.Contains(dto.Role))
                return BadRequest(ApiResponce<object>.Fail("Недопустимая роль", "VALIDATION_ERROR"));
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                return Conflict(ApiResponce<object>.Fail("Пользователь с таким email уже существует", "CONFLICT"));

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


            var user = new User
            {
                Email = dto.Email,
                Phone = dto.Phone,
                LastName = dto.LastName,
                FirstName = dto.FirstName,
                MiddleName = dto.MiddleName,
                Role = dto.Role,
                WorkshopId = dto.WorkshopId,
                RepairCrewId = dto.RepairCrewId,
                PasswordHash = sb.ToString(),
                RegisterDate = DateTime.Now,
                Status = "активен"
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            var result = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Phone = user.Phone,
                LastName = user.LastName,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                Role = user.Role,
                Status = user.Status,
                RegisterDate = DateTime.Now
            };
            return CreatedAtAction("GetUser", new { id = user.Id }, ApiResponce<UserDto>.Ok(result));
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(ApiResponce<object>.Fail("Пользователь не найден", "NOT_FOUND"));
            }

            if (user.Status == "заблокирован")
            {
                return Conflict(ApiResponce<object>.Fail("Пользователь уже деактивирован", "CONFLICT"));
            }
            
            user.Status = "заблокирован";
            await _context.SaveChangesAsync();
            return Ok(ApiResponce<object>.Ok(null!, "Пользователь деактивирован"));

        }
    

    private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
