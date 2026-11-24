using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToirProfAPI.Models;

namespace ToirProfAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeMaintenanceController : ControllerBase
    {
        private readonly ToirProfContext _context;

        public TypeMaintenanceController(ToirProfContext context)
        {
            _context = context;
        }

        // GET: api/TypeMaintenance
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypeMaintenance>>> GetTypeMaintenances()
        {
            return await _context.TypeMaintenances.ToListAsync();
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
        [HttpPost]
        public async Task<ActionResult<TypeMaintenance>> PostTypeMaintenance(TypeMaintenance typeMaintenance)
        {
            _context.TypeMaintenances.Add(typeMaintenance);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTypeMaintenance", new { id = typeMaintenance.Id }, typeMaintenance);
        }

        // DELETE: api/TypeMaintenance/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTypeMaintenance(int id)
        {
            var typeMaintenance = await _context.TypeMaintenances.FindAsync(id);
            if (typeMaintenance == null)
            {
                return NotFound();
            }

            _context.TypeMaintenances.Remove(typeMaintenance);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TypeMaintenanceExists(int id)
        {
            return _context.TypeMaintenances.Any(e => e.Id == id);
        }
    }
}
