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
    public class PlanSceduleController : ControllerBase
    {
        private readonly ToirProfContext _context;

        public PlanSceduleController(ToirProfContext context)
        {
            _context = context;
        }

        // GET: api/PlanScedule
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlanSchedule>>> GetPlanSchedules()
        {
            return await _context.PlanSchedules.ToListAsync();
        }

        // GET: api/PlanScedule/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PlanSchedule>> GetPlanSchedule(int id)
        {
            var planSchedule = await _context.PlanSchedules.FindAsync(id);

            if (planSchedule == null)
            {
                return NotFound();
            }

            return planSchedule;
        }

        // PUT: api/PlanScedule/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlanSchedule(int id, PlanSchedule planSchedule)
        {
            if (id != planSchedule.Id)
            {
                return BadRequest();
            }

            _context.Entry(planSchedule).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlanScheduleExists(id))
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

        // POST: api/PlanScedule
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PlanSchedule>> PostPlanSchedule(PlanSchedule planSchedule)
        {
            _context.PlanSchedules.Add(planSchedule);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlanSchedule", new { id = planSchedule.Id }, planSchedule);
        }

        // DELETE: api/PlanScedule/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlanSchedule(int id)
        {
            var planSchedule = await _context.PlanSchedules.FindAsync(id);
            if (planSchedule == null)
            {
                return NotFound();
            }

            _context.PlanSchedules.Remove(planSchedule);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlanScheduleExists(int id)
        {
            return _context.PlanSchedules.Any(e => e.Id == id);
        }
    }
}
