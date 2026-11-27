using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToirProfAPI.Dtos;
using ToirProfAPI.Models;

namespace ToirProfAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class PlanSceduleController : ControllerBase
    {
        private readonly ToirProfContext _context;

        public PlanSceduleController(ToirProfContext context)
        {
            _context = context;
        }

        // GET: api/PlanScedule
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<ApiResponce<List<PlanScheduleDto>>>> GetPlanSchedules(int? equipmentId, int? workshopId, DateOnly? to, DateOnly? from, string? status)
        {
            var query =  _context.PlanSchedules.AsQueryable();
            if (equipmentId.HasValue)
                query = query.Where(e => e.EquipmentId == equipmentId.Value);
            if (workshopId.HasValue)
                query = query.Where(p => p.Equipment != null && p.Equipment.WorkshopId == workshopId.Value);
            if (from.HasValue)
                query = query.Where(p=> p.PlanDate >= from.Value);
            if (to.HasValue)
                query = query.Where(p=> p.PlanDate >= to.Value);
            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(p => p.Status == status);
            var schedules = await query.OrderBy(p => p.PlanDate)
                .Select(p => new PlanScheduleDto
                {
                    Id = p.Id,
                    EquipmentId = p.EquipmentId,
                    EquipmentName = p.Equipment != null ? p.Equipment.Name : null,
                    TypeMaintenanceId = p.TypeMaintenanceId,
                    TypeMaintenanceName = p.TypeMaintenance != null ? p.TypeMaintenance.Name : null,
                    PlanDate = p.PlanDate,
                    FactCompletionDate = p.FactCompletionDate,
                    RepairCrewId = p.RepairCrewId,
                    Status = p.Status,

                }).ToListAsync();
            return Ok(ApiResponce<List<PlanScheduleDto>>.Ok(schedules));
        }


        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponce<PlanScheduleDto>>> GetPlanSchedule(int id)
        {
            var schedule = await _context.PlanSchedules
                .Where(p=>p.Id == id)
                .Select(p => new PlanScheduleDto
                {
                    Id = p.Id,
                    EquipmentId = p.EquipmentId,
                    EquipmentName = p.Equipment != null ? p.Equipment.Name : null,
                    TypeMaintenanceId = p.TypeMaintenanceId,
                    TypeMaintenanceName = p.TypeMaintenance != null ? p.TypeMaintenance.Name : null,
                    PlanDate = p.PlanDate,
                    FactCompletionDate = p.FactCompletionDate,
                    RepairCrewId = p.RepairCrewId,
                    Status = p.Status,
                })
                .FirstOrDefaultAsync();

            if (schedule == null)
                return NotFound(ApiResponce<PlanScheduleDto>.Fail("Запись графика не найдена", "NOT_FOUND"));
            return Ok(ApiResponce<PlanScheduleDto>.Ok(schedule));
        }
        
        
        
        
        [Authorize]
        [HttpGet("upcoming")]
        public async Task<ActionResult<ApiResponce<List<PlanScheduleDto>>>> GetUpcommingPlanSchedules(int days = 7)
        {
            if (days <= 0)
                return BadRequest(ApiResponce<List<PlanScheduleDto>>.Fail("Количество дней должно быть больше нуля", "VALIDATION_ERROR"));
            var today = DateOnly.FromDateTime(DateTime.Today);
            var untill = today.AddDays(days);

            var schedules = await _context.PlanSchedules
                .Where(p => p.PlanDate >= today && p.PlanDate <= untill && p.Status != "выполнено")
                .OrderBy(p => p.PlanDate)
                .Select(p => new PlanScheduleDto
                    {
                        Id = p.Id,
                        EquipmentId = p.EquipmentId,
                        EquipmentName = p.Equipment != null ? p.Equipment.Name : null,
                        TypeMaintenanceId = p.TypeMaintenanceId,
                        TypeMaintenanceName = p.TypeMaintenance != null ? p.TypeMaintenance.Name : null,
                        PlanDate = p.PlanDate,
                        FactCompletionDate = p.FactCompletionDate,
                        RepairCrewId = p.RepairCrewId,
                        Status = p.Status,
                    }
                ).ToListAsync();
            return Ok(ApiResponce<List<PlanScheduleDto>>.Ok(schedules));
        }

        [Authorize]
        [HttpGet("overdue")]
        public async Task<ActionResult<ApiResponce<List<PlanScheduleDto>>>> GetOverdue()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var schedules = await _context.PlanSchedules
                .Where(p => p.PlanDate < today && p.Status != "выполнено")
                .OrderBy(p => p.PlanDate)
                .Select(p => new PlanScheduleDto
                    {
                        Id = p.Id,
                        EquipmentId = p.EquipmentId,
                        EquipmentName = p.Equipment != null ? p.Equipment.Name : null,
                        TypeMaintenanceId = p.TypeMaintenanceId,
                        TypeMaintenanceName = p.TypeMaintenance != null ? p.TypeMaintenance.Name : null,
                        PlanDate = p.PlanDate,
                        FactCompletionDate = p.FactCompletionDate,
                        RepairCrewId = p.RepairCrewId,
                        Status = p.Status,
                    }
                ).ToListAsync();
            return Ok(ApiResponce<List<PlanScheduleDto>>.Ok(schedules));
        }
        // PUT: api/PlanScedule/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "инженер, мастер, администратор")]
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponce<PlanScheduleDto>>> UpdatePlanSchedule(int id,
            UpdatePlanScheduleDto dto)
        {
            var schedule = await _context.PlanSchedules.FindAsync(id);
            if (schedule == null)
                return NotFound(ApiResponce<PlanScheduleDto>.Fail("Запись графика не найдена", "NOT_FOUND"));
            if (schedule.Status == "выполнено" || schedule.Status == "отменено")
                return Conflict(ApiResponce<PlanScheduleDto>.Fail("Запись нельзя изменить", "CONFLICT"));
            if (!await _context.TypeMaintenances.AnyAsync(t => t.Id == dto.TypeMaintenanceId && t.IsActive == true))
                return  BadRequest(ApiResponce<PlanScheduleDto>.Fail("Вид ТО не найден или неактивен", "VALIDATION_ERROR"));
            var today = DateOnly.FromDateTime(DateTime.Today);
            if(dto.PlanDate < today)
                return BadRequest(ApiResponce<PlanScheduleDto>.Fail("Плановая дата не может быть в прошлом", "VALIDATION_ERROR"));
            var exists = await _context.PlanSchedules
                .AnyAsync(p => p.EquipmentId == schedule.EquipmentId && p.TypeMaintenanceId == dto.TypeMaintenanceId && p.PlanDate == dto.PlanDate && p.Id != schedule.Id);
            if (exists)
                return Conflict(ApiResponce<PlanScheduleDto>.Fail("ТО с такой записью уже существует", "CONFLICT"));
            schedule.TypeMaintenanceId = dto.TypeMaintenanceId;
            schedule.PlanDate = dto.PlanDate;
            await _context.SaveChangesAsync();

            var result = new PlanScheduleDto
            {
                Id = schedule.Id,
                EquipmentId = schedule.EquipmentId,
                TypeMaintenanceId = schedule.TypeMaintenanceId,
                PlanDate = schedule.PlanDate,
                FactCompletionDate = schedule.FactCompletionDate,
                RepairCrewId = schedule.RepairCrewId,
                Status = schedule.Status,
            };
            return Ok(ApiResponce<PlanScheduleDto>.Ok(result, "Запись графика обновлена"));
        }


        [Authorize(Roles = "инженер, мастер, администратор")]
        [HttpPut("{id:int}/crew")]
        public async Task<ActionResult<ApiResponce<PlanScheduleDto>>> AssignCrew(int id, AssignCrewDto dto)
        {
            var schedule = await _context.PlanSchedules.FindAsync(id);
            if (schedule == null)
                return NotFound(ApiResponce<PlanScheduleDto>.Fail("ТО не найдено", "NOT_FOUND"));
            if (schedule.Status == "выполнено" || schedule.Status == "отменено")
                return Conflict(ApiResponce<PlanScheduleDto>.Fail("ТО уже выполнено", "CONFLICT"));
            var crew = await _context.RepairCrews.FindAsync(dto.RepairCrewId);
            if (crew == null)
                return BadRequest(ApiResponce<PlanScheduleDto>.Fail("Бригада не существует", "VALIDATION_ERROR"));
            schedule.RepairCrewId = dto.RepairCrewId;
            schedule.Status = "назначено";
            await _context.SaveChangesAsync();
            var result = new PlanScheduleDto
            {
                Id = schedule.Id,
                EquipmentId = schedule.EquipmentId,
                TypeMaintenanceId = schedule.TypeMaintenanceId,
                PlanDate = schedule.PlanDate,
                FactCompletionDate = schedule.FactCompletionDate,
                RepairCrewId = schedule.RepairCrewId,
                Status = schedule.Status,
            };
            return Ok(ApiResponce<PlanScheduleDto>.Ok(result, "Бригада назначена"));
        }


        // POST: api/PlanScedule
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "инженер, мастер, администратор")]
        [HttpPost]
        public async Task<ActionResult<ApiResponce<PlanScheduleDto>>> CreatePlanSchedule(CreatePlanScheduleDto dto)
        {
            if (!await _context.Equipments.AnyAsync(e => e.Id == dto.EquipmentId))
                return NotFound(ApiResponce<PlanScheduleDto>.Fail("Оборудование не найдено", "NOT_FOUND"));
            if (!await _context.TypeMaintenances.AnyAsync(t => t.Id == dto.TypeMaintenanceId && t.IsActive == true))
                return BadRequest(ApiResponce<PlanScheduleDto>.Fail("Вид ТО не найден или неактивен", "VALIDATION_ERROR"));
            var today = DateOnly.FromDateTime(DateTime.Today);
            if(dto.PlanDate < today)
                return BadRequest(ApiResponce<PlanScheduleDto>.Fail("Плановая дата не может быть в прошлом", "VALIDATION_ERROR"));
            var exists = await _context.PlanSchedules
                .AnyAsync(p => p.EquipmentId == dto.EquipmentId && p.TypeMaintenanceId == dto.TypeMaintenanceId && p.PlanDate == dto.PlanDate);
            if (exists)
                return Conflict(ApiResponce<PlanScheduleDto>.Fail("ТО с такой датой уже поставлена", "CONFLICT"));
            
            
            var schedule = new PlanSchedule
            {
                EquipmentId = dto.EquipmentId,
                TypeMaintenanceId = dto.TypeMaintenanceId,
                PlanDate = dto.PlanDate,
                Status = "запланировано"
            };
            
            _context.PlanSchedules.Add(schedule);
            
            await _context.SaveChangesAsync();

            var result = new PlanScheduleDto
            {
                Id = schedule.Id,
                EquipmentId = schedule.EquipmentId,
                TypeMaintenanceId = schedule.TypeMaintenanceId,
                PlanDate = schedule.PlanDate,
                FactCompletionDate = schedule.FactCompletionDate,
                RepairCrewId = schedule.RepairCrewId,
                Status = schedule.Status
            };

            return CreatedAtAction(nameof(GetPlanSchedule), new { id = schedule.Id },
                ApiResponce<PlanScheduleDto>.Ok(result, "Запись графика создана"));
        }

        // DELETE: api/PlanSchedule/5
        [Authorize(Roles = "инженер, мастер, администратор")]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ApiResponce<object>>> DeletePlanSchedule(int id, DeletePlanScheduleDto dto)
        {
            var schedule = await _context.PlanSchedules.FindAsync(id);
            if (schedule == null)
                return NotFound(ApiResponce<object>.Fail("Запись графика не найдена", "NOT_FOUND"));

            if (schedule.Status == "выполнено" || schedule.Status == "выполняется")
                return Conflict(ApiResponce<object>.Fail("Нельзя удалить выполненное или выполняемое ТО", "CONFLICT"));

            if (await _context.WorkOrders.AnyAsync(w => w.PlanScheduleId == id))
                return Conflict(ApiResponce<object>.Fail("На эту запись уже есть заявки, удаление невозможно", "CONFLICT"));

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int? userId = int.TryParse(userIdClaim, out var uid) ? uid : null;

            _context.OperationLogs.Add(new OperationLog
            {
                UserId = userId,
                TypeEntity = "график ТО",
                EntityId = schedule.Id,
                Action = "удаление",
                OldValue = $"оборудование {schedule.EquipmentId}, вид ТО {schedule.TypeMaintenanceId}, дата {schedule.PlanDate}, статус {schedule.Status}",
                NewValue = dto.Reason,
                DateOperation = DateTime.Now
            });

            _context.PlanSchedules.Remove(schedule);
            await _context.SaveChangesAsync();

            return Ok(ApiResponce<object>.Ok(null!, "Запись графика удалена"));
        }

        private bool PlanScheduleExists(int id)
        {
            return _context.PlanSchedules.Any(e => e.Id == id);
        }
    }
}
