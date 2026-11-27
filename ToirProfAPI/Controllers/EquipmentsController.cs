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
    
    public class EquipmentsController : ControllerBase
    {
        private readonly ToirProfContext _context;

        public EquipmentsController(ToirProfContext context)
        {
            _context = context;
        }

        // GET: api/Equipments
        
        [HttpGet]
        public async Task<ActionResult<ApiResponce<List<EquipmnetDto>>>> GetEquipments(string? status, string? search, int? workshopId, string? model)
        {
            var query = _context.Equipments.AsQueryable();

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(e => e.Status == status);
            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(e => EF.Functions.Like(e.Name, $"%{search}%"));
            if (workshopId.HasValue)
                query = query.Where(e => e.WorkshopId == workshopId.Value);
            if (!string.IsNullOrWhiteSpace(model))
                query = query.Where(e => e.Model == model);
            
            
            
            var equipments = await query
                .Select(e=> new EquipmnetDto
                {
                    Id = e.Id,
                    InventoreNumber = e.InventoryNumber,
                    Name = e.Name,
                    Model = e.Model,
                    Status = e.Status,
                    WorkshopName = e.Workshop !=null ? e.Workshop.Name : null,
                })
                .ToListAsync();
            return Ok(ApiResponce<List<EquipmnetDto>>.Ok(equipments));
        }

        // GET: api/Equipments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Equipment>> GetEquipment(int id)
        {
            var equipment = await _context.Equipments.FindAsync(id);

            if (equipment == null)
            {
                return NotFound();
            }

            return equipment;
        }

        // PUT: api/Equipments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEquipment(int id, Equipment equipment)
        {
            if (id != equipment.Id)
            {
                return BadRequest();
            }

            _context.Entry(equipment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EquipmentExists(id))
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

        // POST: api/Equipments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Equipment>> PostEquipment(Equipment equipment)
        {
            _context.Equipments.Add(equipment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEquipment", new { id = equipment.Id }, equipment);
        }

        // DELETE: api/Equipments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEquipment(int id)
        {
            var equipment = await _context.Equipments.FindAsync(id);
            if (equipment == null)
            {
                return NotFound();
            }

            _context.Equipments.Remove(equipment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EquipmentExists(int id)
        {
            return _context.Equipments.Any(e => e.Id == id);
        }
    }
}
