using Microsoft.EntityFrameworkCore;
using MatrixNext.Web.Models;
using MatrixNext.Web.Models.PY;
using MatrixNext.Web.Models.CORE;

namespace MatrixNext.Web.Infrastructure.Data
{
    /// <summary>
    /// DbContext principal para Matrix
    /// Ref: PLAN_IMPLEMENTACION_SPRINTS.md § T0.1
    /// Mapea entidades PY, CORE, OP (catálogos básicos)
    /// </summary>
    public class MatrixDbContext : DbContext
    {
        public MatrixDbContext(DbContextOptions<MatrixDbContext> options) : base(options)
        {
        }

        // ===== PY: PROYECTOS Y TRABAJOS =====
        public DbSet<Proyecto> Proyectos { get; set; }
        public DbSet<Trabajo> Trabajos { get; set; }
        public DbSet<VariableControl> VariablesControl { get; set; }

        // ===== CORE: WORKFLOW Y TAREAS =====
        public DbSet<Tarea> Tareas { get; set; }
        public DbSet<WorkFlow> WorkFlows { get; set; }
        public DbSet<TareaPrevía> TareasPrevias { get; set; }
        public DbSet<WorkFlowUsuarioAsignado> WorkFlowUsuariosAsignados { get; set; }
        public DbSet<ObservacionTarea> ObservacionesTareas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ===== CONFIGURACIÓN PY: PROYECTOS =====
            modelBuilder.Entity<Proyecto>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(2000);

                entity.Property(e => e.JobBook)
                    .HasMaxLength(50);

                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.FechaModificacion).HasDefaultValueSql("GETUTCDATE()");

                // Índices
                entity.HasIndex(e => e.IdGerenteProyectos).HasDatabaseName("IX_Proyecto_IdGerenteProyectos");
                entity.HasIndex(e => e.IdUnidad).HasDatabaseName("IX_Proyecto_IdUnidad");
                entity.HasIndex(e => e.Activo).HasDatabaseName("IX_Proyecto_Activo");

                // Relaciones
                entity.HasMany(e => e.Trabajos)
                    .WithOne(e => e.Proyecto)
                    .HasForeignKey(e => e.IdProyecto)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ===== CONFIGURACIÓN PY: TRABAJOS =====
            modelBuilder.Entity<Trabajo>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(2000);

                entity.Property(e => e.JobBook)
                    .HasMaxLength(50);

                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.FechaModificacion).HasDefaultValueSql("GETUTCDATE()");

                // Índices (ref: VALIDACION_BASE_DATOS.md § 5)
                entity.HasIndex(e => e.IdProyecto).HasDatabaseName("IX_Trabajo_IdProyecto");
                entity.HasIndex(e => e.Estado).HasDatabaseName("IX_Trabajo_Estado");
                entity.HasIndex(e => e.Activo).HasDatabaseName("IX_Trabajo_Activo");

                // Relaciones
                entity.HasMany(e => e.VariablesControl)
                    .WithOne(e => e.Trabajo)
                    .HasForeignKey(e => e.IdTrabajo)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ===== CONFIGURACIÓN PY: VARIABLES CONTROL =====
            modelBuilder.Entity<VariableControl>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Valor)
                    .HasMaxLength(1000);

                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETUTCDATE()");
            });

            // ===== CONFIGURACIÓN CORE: TAREAS (CATÁLOGO) =====
            modelBuilder.Entity<Tarea>(entity =>
            {
                entity.ToTable("CORE_Tareas");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.FechaModificacion).HasDefaultValueSql("GETUTCDATE()");

                // Índices
                entity.HasIndex(e => e.Nombre);
                entity.HasIndex(e => e.Visible);
            });

            // ===== CONFIGURACIÓN CORE: WORKFLOW =====
            modelBuilder.Entity<WorkFlow>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Estado)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValue("Creada");

                entity.Property(e => e.Observaciones)
                    .HasMaxLength(2000);

                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETUTCDATE()");

                // Índices (ref: VALIDACION_BASE_DATOS.md § 5)
                entity.HasIndex(e => e.IdTrabajo).HasDatabaseName("IX_WorkFlow_IdTrabajo");
                entity.HasIndex(e => e.Estado).HasDatabaseName("IX_WorkFlow_Estado");

                // Relaciones
                entity.HasMany(e => e.UsuariosAsignados)
                    .WithOne(e => e.WorkFlow)
                    .HasForeignKey(e => e.IdWorkFlow)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.Observaciones_Log)
                    .WithOne(e => e.WorkFlow)
                    .HasForeignKey(e => e.IdWorkFlow)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.TareasPrevias)
                    .WithOne(e => e.Tarea)
                    .HasForeignKey(e => e.IdTarea)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ===== CONFIGURACIÓN CORE: TAREAS PREVIAS =====
            modelBuilder.Entity<TareaPrevía>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETUTCDATE()");

                // Índice para búsquedas de precedencias
                entity.HasIndex(e => e.IdTareaPreviaRequerida)
                    .HasDatabaseName("IX_TareaPrevía_IdTareaPreviaRequerida");

                // Relación: Una tarea previa requiere otra
                entity.HasOne(e => e.TareaPreviaRequerida)
                    .WithMany()
                    .HasForeignKey(e => e.IdTareaPreviaRequerida)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ===== CONFIGURACIÓN CORE: USUARIOS ASIGNADOS =====
            modelBuilder.Entity<WorkFlowUsuarioAsignado>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Rol)
                    .HasMaxLength(100)
                    .HasDefaultValue("Responsable");

                entity.Property(e => e.FechaAsignacion).HasDefaultValueSql("GETUTCDATE()");

                // Índice para búsquedas por usuario
                entity.HasIndex(e => e.IdUsuario)
                    .HasDatabaseName("IX_WorkFlowUsuarioAsignado_IdUsuario");
            });

            // ===== CONFIGURACIÓN CORE: OBSERVACIONES TAREAS =====
            modelBuilder.Entity<ObservacionTarea>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Observacion)
                    .HasMaxLength(2000);

                entity.Property(e => e.TipoOperacion)
                    .HasMaxLength(50);

                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETUTCDATE()");

                // Índice para auditoría
                entity.HasIndex(e => new { e.IdWorkFlow, e.FechaCreacion })
                    .HasDatabaseName("IX_ObservacionTarea_WorkFlowFecha");
            });
        }
    }
}
