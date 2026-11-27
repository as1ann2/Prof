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

namespace ToirProfAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TypeMaintenanceController : ControllerBase
    {
        private readonly ToirProfContext _context;

        public TypeMaintenanceController(ToirProfContext context)
        {
            _context = context;
        }

        // GET: api/TypeMaintenance
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<ApiResponce<List<TypeMaintenanceDto>>>> GetTypeMaintenances(bool onlyActive = false)
        {
            var query = _context.TypeMaintenances.AsQueryable();
            if (onlyActive)
                query = query.Where(t=> t.IsActive == true);
            
            var types = await query.Select(t => new TypeMaintenanceDto
            {
                Id = t.Id,
                Code = t.Code,
                Name = t.Name,
                Description = t.Description,
                PeriodDay = t.PeriodDay,
                PlanTimeInHours = t.PlanTimeInHours,
                StopEquipment = t.StopEquiment ?? false,
                IsActive = t.IsActive ?? false,
            })
            .ToListAsync();
            return Ok(ApiResponce<List<TypeMaintenanceDto>>.Ok(types));
    
        }

        // GET: api/TypeMaintenance/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TypeMaintenance>> GetTypeMaintenance(int id)
        {
            var typeMaintenance = await _context.TypeMaintenances.FindAsync(id);

            if (typeMaintenance == null)
            {
                return NotFound();
            }

            return typeMaintenance;
        }

        // PUT: api/TypeMaintenance/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTypeMaintenance(int id, TypeMaintenance typeMaintenance)
        {
            if (id != typeMaintenance.Id)
            {
                return BadRequest();
            }

            _context.Entry(typeMaintenance).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TypeMaintenanceExists(id))
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

        // POST: api/TypeMaintenance
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "инженер, администратор")]
        [HttpPost]
        public async Task<ActionResult<CreateTypeMaintenanceDto>> CreateTypeMaintenance(CreateTypeMaintenanceDto dto)
        {
            
            if(string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest(ApiResponce<CreateTypeMaintenanceDto>.Fail("Недопустимое имя", "VALLIDATION_ERROR"));
            if (dto.PeriodDay <= 0 || dto.PlanTimeInHours <= 0)
                return BadRequest(ApiResponce<CreateTypeMaintenanceDto>.Fail(
                    "Периодичность и плановая длительность должны быть больше нуля", "VALLIDATION_ERROR"));
            if(!string.IsNullOrWhiteSpace(dto.Code) &&
                await _context.TypeMaintenances.AnyAsync(t => t.Code == dto.Code))
                return Conflict(ApiResponce<CreateTypeMaintenanceDto>.Fail("Код уже используется другим видом ТО", "CONFLICT"));

            var type = new TypeMaintenance()
            {
                Code = dto.Code,
                Name = dto.Name,
                Description = dto.Description,
                PeriodDay = dto.PeriodDay,
                PlanTimeInHours = dto.PlanTimeInHours,
                StopEquiment = dto.StopEquipment,
                IsActive = dto.IsActive 
            };
            
            _context.TypeMaintenances.Add(type);
            await _context.SaveChangesAsync();

            var result = new TypeMaintenanceDto()
            {
                Code = type.Code,
                Name = type.Name,
                Description = type.Description,
                PeriodDay = type.PeriodDay,
                PlanTimeInHours = type.PlanTimeInHours,
                StopEquipment = type.StopEquiment ?? false,
                IsActive = type.IsActive ?? false
            };
            
            return CreatedAtAction(nameof(GetTypeMaintenance), new { id = type.Id },
                ApiResponce<TypeMaintenanceDto>.Ok(result, "Вид ТО создан"));
            
        }

        // DELETE: api/TypeMaintenance/5
        [Authorize(Roles = "администратор, инженер")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTypeMaintenance(int id)
        {
            var typeMaintenance = await _context.TypeMaintenances.FindAsync(id);
            if (typeMaintenance == null)
            {
                return NotFound(ApiResponce<object>.Fail("ТО не найдено", "NOT_FOUND"));
            }

            if (typeMaintenance.IsActive == false)
            {
                return Conflict(ApiResponce<object>.Fail("ТО уже деактивировано", "CONFLICT"));
            }
            
            
            typeMaintenance.IsActive = false;
            await _context.SaveChangesAsync();

            return Ok(ApiResponce<object>.Ok(null!, "ТО деактивировано" ));
        }

        private bool TypeMaintenanceExists(int id)
        {
            return _context.TypeMaintenances.Any(e => e.Id == id);
        }
    }
}
