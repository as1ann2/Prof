using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ToirProfAPI.Models;

public partial class ToirProfContext : DbContext
{
    public ToirProfContext(DbContextOptions<ToirProfContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Equipment> Equipments { get; set; }

    public virtual DbSet<HistoryEquipment> HistoryEquipments { get; set; }

    public virtual DbSet<Material> Materials { get; set; }

    public virtual DbSet<OperationLog> OperationLogs { get; set; }

    public virtual DbSet<PlanSchedule> PlanSchedules { get; set; }

    public virtual DbSet<RepairCrew> RepairCrews { get; set; }

    public virtual DbSet<TypeMaintenance> TypeMaintenances { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<WorkOrder> WorkOrders { get; set; }

    public virtual DbSet<Workshop> Workshops { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Equipment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("equipments_pkey");

            entity.ToTable("equipments");

            entity.HasIndex(e => e.InventoryNumber, "equipments_inventory_number_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AtWorkDate).HasColumnName("at_work_date");
            entity.Property(e => e.CommisioningDate).HasColumnName("commisioning_date");
            entity.Property(e => e.CreateDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("create_date");
            entity.Property(e => e.DecommisioningDate).HasColumnName("decommisioning_date");
            entity.Property(e => e.InventoryNumber)
                .HasMaxLength(50)
                .HasColumnName("inventory_number");
            entity.Property(e => e.LastUpdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("last_update");
            entity.Property(e => e.Manufacturer).HasColumnName("manufacturer");
            entity.Property(e => e.Model)
                .HasMaxLength(100)
                .HasColumnName("model");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.SerialNumber)
                .HasMaxLength(100)
                .HasColumnName("serial_number");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.WorkshopId).HasColumnName("workshop_id");

            entity.HasOne(d => d.Workshop).WithMany(p => p.Equipment)
                .HasForeignKey(d => d.WorkshopId)
                .HasConstraintName("equipments_workshop_id_fkey");
        });

        modelBuilder.Entity<HistoryEquipment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("history_equipments_pkey");

            entity.ToTable("history_equipments");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateChange)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_change");
            entity.Property(e => e.EquipmentId).HasColumnName("equipment_id");
            entity.Property(e => e.NewStatus)
                .HasMaxLength(20)
                .HasColumnName("new_status");
            entity.Property(e => e.OldStatus)
                .HasMaxLength(20)
                .HasColumnName("old_status");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Equipment).WithMany(p => p.HistoryEquipments)
                .HasForeignKey(d => d.EquipmentId)
                .HasConstraintName("history_equipments_equipment_id_fkey");
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("materials_pkey");

            entity.ToTable("materials");

            entity.HasIndex(e => e.Article, "materials_article_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Article)
                .HasMaxLength(50)
                .HasColumnName("article");
            entity.Property(e => e.MinStock).HasColumnName("min_stock");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasPrecision(12, 2)
                .HasColumnName("price");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Stock).HasColumnName("stock");
            entity.Property(e => e.StorageLocation)
                .HasMaxLength(200)
                .HasColumnName("storage_location");
            entity.Property(e => e.Unit)
                .HasMaxLength(30)
                .HasColumnName("unit");
        });

        modelBuilder.Entity<OperationLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("operation_log_pkey");

            entity.ToTable("operation_log");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Action)
                .HasMaxLength(30)
                .HasColumnName("action");
            entity.Property(e => e.DateOperation)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_operation");
            entity.Property(e => e.EntityId).HasColumnName("entity_id");
            entity.Property(e => e.NewValue).HasColumnName("new_value");
            entity.Property(e => e.OldValue).HasColumnName("old_value");
            entity.Property(e => e.TypeEntity)
                .HasMaxLength(50)
                .HasColumnName("type_entity");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.OperationLogs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("operation_log_user_id_fkey");
        });

        modelBuilder.Entity<PlanSchedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("plan_schedules_pkey");

            entity.ToTable("plan_schedules");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EquipmentId).HasColumnName("equipment_id");
            entity.Property(e => e.FactCompletionDate).HasColumnName("fact_completion_date");
            entity.Property(e => e.PlanDate).HasColumnName("plan_date");
            entity.Property(e => e.RepairCrewId).HasColumnName("repair_crew_id");
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasColumnName("status");
            entity.Property(e => e.TypeMaintenanceId).HasColumnName("type_maintenance_id");

            entity.HasOne(d => d.Equipment).WithMany(p => p.PlanSchedules)
                .HasForeignKey(d => d.EquipmentId)
                .HasConstraintName("plan_schedules_equipment_id_fkey");

            entity.HasOne(d => d.RepairCrew).WithMany(p => p.PlanSchedules)
                .HasForeignKey(d => d.RepairCrewId)
                .HasConstraintName("plan_schedules_repair_crew_id_fkey");

            entity.HasOne(d => d.TypeMaintenance).WithMany(p => p.PlanSchedules)
                .HasForeignKey(d => d.TypeMaintenanceId)
                .HasConstraintName("plan_schedules_type_maintenance_id_fkey");
        });

        modelBuilder.Entity<RepairCrew>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("repair_crews_pkey");

            entity.ToTable("repair_crews");

            entity.HasIndex(e => e.Name, "repair_crews_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Specialization).HasColumnName("specialization");
            entity.Property(e => e.WorkshopId).HasColumnName("workshop_id");

            entity.HasOne(d => d.Workshop).WithMany(p => p.RepairCrews)
                .HasForeignKey(d => d.WorkshopId)
                .HasConstraintName("repair_crews_workshop_id_fkey");
        });

        modelBuilder.Entity<TypeMaintenance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("type_maintenance_pkey");

            entity.ToTable("type_maintenance");

            entity.HasIndex(e => e.Code, "type_maintenance_code_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.PeriodDay).HasColumnName("period_day");
            entity.Property(e => e.PlanTimeInHours).HasColumnName("plan_time_in_hours");
            entity.Property(e => e.StopEquiment)
                .HasDefaultValue(false)
                .HasColumnName("stop_equiment");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.HasIndex(e => e.Phone, "users_phone_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(100)
                .HasColumnName("middle_name");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(300)
                .HasColumnName("password_hash");
            entity.Property(e => e.Phone)
                .HasMaxLength(25)
                .HasColumnName("phone");
            entity.Property(e => e.RegisterDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("register_date");
            entity.Property(e => e.RepairCrewId).HasColumnName("repair_crew_id");
            entity.Property(e => e.Role)
                .HasMaxLength(30)
                .HasColumnName("role");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'активен'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.WorkshopId).HasColumnName("workshop_id");

            entity.HasOne(d => d.RepairCrew).WithMany(p => p.Users)
                .HasForeignKey(d => d.RepairCrewId)
                .HasConstraintName("users_repair_crew_id_fkey");

            entity.HasOne(d => d.Workshop).WithMany(p => p.Users)
                .HasForeignKey(d => d.WorkshopId)
                .HasConstraintName("users_workshop_id_fkey");
        });

        modelBuilder.Entity<WorkOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("work_orders_pkey");

            entity.ToTable("work_orders");

            entity.HasIndex(e => e.Number, "work_orders_number_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DateCreate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_create");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.EquipmentId).HasColumnName("equipment_id");
            entity.Property(e => e.FactDateEnd)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fact_date_end");
            entity.Property(e => e.FactDateStart)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fact_date_start");
            entity.Property(e => e.Number)
                .HasMaxLength(30)
                .HasColumnName("number");
            entity.Property(e => e.PlanScheduleId).HasColumnName("plan_schedule_id");
            entity.Property(e => e.PlannedDateEnd)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("planned_date_end");
            entity.Property(e => e.PlannedDateStart)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("planned_date_start");
            entity.Property(e => e.Priority)
                .HasMaxLength(30)
                .HasColumnName("priority");
            entity.Property(e => e.ReasonRefusal).HasColumnName("reason_refusal");
            entity.Property(e => e.RepairCrewsId).HasColumnName("repair_crews_id");
            entity.Property(e => e.Report).HasColumnName("report");
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasColumnName("status");
            entity.Property(e => e.Type)
                .HasMaxLength(100)
                .HasColumnName("type");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.WorkOrders)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("work_orders_created_by_fkey");

            entity.HasOne(d => d.Equipment).WithMany(p => p.WorkOrders)
                .HasForeignKey(d => d.EquipmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("work_orders_equipment_id_fkey");

            entity.HasOne(d => d.PlanSchedule).WithMany(p => p.WorkOrders)
                .HasForeignKey(d => d.PlanScheduleId)
                .HasConstraintName("work_orders_plan_schedule_id_fkey");

            entity.HasOne(d => d.RepairCrews).WithMany(p => p.WorkOrders)
                .HasForeignKey(d => d.RepairCrewsId)
                .HasConstraintName("work_orders_repair_crews_id_fkey");
        });

        modelBuilder.Entity<Workshop>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("workshops_pkey");

            entity.ToTable("workshops");

            entity.HasIndex(e => e.Code, "workshops_code_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .HasColumnName("code");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
