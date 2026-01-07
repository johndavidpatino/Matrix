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
        public DbSet<AsignacionProyecto> AsignacionesProyectos { get; set; }

        // ===== PY: TRABAJOS CUALITATIVOS (SPRINT 4) =====
        public DbSet<TrabajosCuali> TrabajosCuali { get; set; }
        public DbSet<SegmentosCuali> SegmentosCuali { get; set; }
        public DbSet<SesionesCuali> SesionesCuali { get; set; }
        public DbSet<MuestrasCuali> MuestrasCuali { get; set; }
        public DbSet<EntrevistadorasCuali> EntrevistadorasCuali { get; set; }
        public DbSet<ParticipantesSesion> ParticipantesSesion { get; set; }

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

            // ===== CONFIGURACIÓN PY: TRABAJOS CUALITATIVOS (SPRINT 4) =====
            modelBuilder.Entity<TrabajosCuali>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(2000);

                entity.Property(e => e.JobBook)
                    .HasMaxLength(50);

                entity.Property(e => e.Estado)
                    .HasMaxLength(50)
                    .HasDefaultValue("Creado");

                entity.Property(e => e.TipoEstudio)
                    .HasMaxLength(100);

                entity.Property(e => e.Ubicacion)
                    .HasMaxLength(500);

                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETUTCDATE()");

                // Índices
                entity.HasIndex(e => e.IdProyecto).HasDatabaseName("IX_TrabajosCuali_IdProyecto");
                entity.HasIndex(e => e.Estado).HasDatabaseName("IX_TrabajosCuali_Estado");
                entity.HasIndex(e => e.Activo).HasDatabaseName("IX_TrabajosCuali_Activo");

                // Relaciones
                entity.HasMany(e => e.Segmentos)
                    .WithOne(e => e.TrabajoCuali)
                    .HasForeignKey(e => e.IdTrabajoCuali)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.Sesiones)
                    .WithOne(e => e.TrabajoCuali)
                    .HasForeignKey(e => e.IdTrabajoCuali)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.Muestras)
                    .WithOne(e => e.TrabajoCuali)
                    .HasForeignKey(e => e.IdTrabajoCuali)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ===== CONFIGURACIÓN PY: SEGMENTOS CUALI =====
            modelBuilder.Entity<SegmentosCuali>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(2000);

                entity.Property(e => e.CriteriosInclusion)
                    .HasMaxLength(1000);

                entity.Property(e => e.CriteriosExclusion)
                    .HasMaxLength(1000);

                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETUTCDATE()");

                // Índices
                entity.HasIndex(e => e.IdTrabajoCuali).HasDatabaseName("IX_SegmentosCuali_IdTrabajoCuali");
                entity.HasIndex(e => e.Activo).HasDatabaseName("IX_SegmentosCuali_Activo");

                // Relaciones
                entity.HasMany(e => e.Muestras)
                    .WithOne(e => e.Segmento)
                    .HasForeignKey(e => e.IdSegmento)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasMany(e => e.Entrevistadores)
                    .WithOne(e => e.Segmento)
                    .HasForeignKey(e => e.IdSegmento)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // ===== CONFIGURACIÓN PY: SESIONES CUALI =====
            modelBuilder.Entity<SesionesCuali>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Tipo)
                    .HasMaxLength(100);

                entity.Property(e => e.HoraInicio)
                    .HasMaxLength(5);

                entity.Property(e => e.HoraFin)
                    .HasMaxLength(5);

                entity.Property(e => e.Ubicacion)
                    .HasMaxLength(500);

                entity.Property(e => e.Moderador)
                    .HasMaxLength(250);

                entity.Property(e => e.Estado)
                    .HasMaxLength(50)
                    .HasDefaultValue("Planeada");

                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETUTCDATE()");

                // Índices
                entity.HasIndex(e => e.IdTrabajoCuali).HasDatabaseName("IX_SesionesCuali_IdTrabajoCuali");
                entity.HasIndex(e => e.FechaProgramada).HasDatabaseName("IX_SesionesCuali_FechaProgramada");
                entity.HasIndex(e => e.Estado).HasDatabaseName("IX_SesionesCuali_Estado");

                // Relaciones
                entity.HasMany(e => e.Participantes)
                    .WithOne(e => e.Sesion)
                    .HasForeignKey(e => e.IdSesion)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ===== CONFIGURACIÓN PY: MUESTRAS CUALI =====
            modelBuilder.Entity<MuestrasCuali>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.NumeroMuestra)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.NombreParticipante)
                    .HasMaxLength(500);

                entity.Property(e => e.Telefono)
                    .HasMaxLength(20);

                entity.Property(e => e.Email)
                    .HasMaxLength(250);

                entity.Property(e => e.Direccion)
                    .HasMaxLength(1000);

                entity.Property(e => e.Genero)
                    .HasMaxLength(50);

                entity.Property(e => e.Ocupacion)
                    .HasMaxLength(250);

                entity.Property(e => e.Estado)
                    .HasMaxLength(50)
                    .HasDefaultValue("Planeada");

                entity.Property(e => e.CalidadDatos)
                    .HasMaxLength(50);

                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETUTCDATE()");

                // Índices
                entity.HasIndex(e => e.IdTrabajoCuali).HasDatabaseName("IX_MuestrasCuali_IdTrabajoCuali");
                entity.HasIndex(e => e.NumeroMuestra).HasDatabaseName("IX_MuestrasCuali_NumeroMuestra");
                entity.HasIndex(e => e.Estado).HasDatabaseName("IX_MuestrasCuali_Estado");
                entity.HasIndex(e => e.IdEntrevistador).HasDatabaseName("IX_MuestrasCuali_IdEntrevistador");
            });

            // ===== CONFIGURACIÓN PY: ENTREVISTADORES CUALI =====
            modelBuilder.Entity<EntrevistadorasCuali>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.NombreCompleto)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Telefono)
                    .HasMaxLength(20);

                entity.Property(e => e.Email)
                    .HasMaxLength(250);

                entity.Property(e => e.Especialidad)
                    .HasMaxLength(100);

                entity.Property(e => e.Estado)
                    .HasMaxLength(50)
                    .HasDefaultValue("Asignado");

                entity.Property(e => e.NivelExperiencia)
                    .HasMaxLength(50);

                entity.Property(e => e.Disponibilidad)
                    .HasMaxLength(50)
                    .HasDefaultValue("Disponible");

                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETUTCDATE()");

                // Índices
                entity.HasIndex(e => e.IdTrabajoCuali).HasDatabaseName("IX_EntrevistadorasCuali_IdTrabajoCuali");
                entity.HasIndex(e => e.IdUsuario).HasDatabaseName("IX_EntrevistadorasCuali_IdUsuario");
                entity.HasIndex(e => e.Estado).HasDatabaseName("IX_EntrevistadorasCuali_Estado");

                // Relaciones
                entity.HasMany(e => e.Muestras)
                    .WithOne(e => e.Entrevistador)
                    .HasForeignKey(e => e.IdEntrevistador)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // ===== CONFIGURACIÓN PY: PARTICIPANTES SESIÓN =====
            modelBuilder.Entity<ParticipantesSesion>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Asistencia)
                    .HasMaxLength(50);

                entity.Property(e => e.CalidadRespuestas)
                    .HasMaxLength(50);

                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETUTCDATE()");

                // Índices
                entity.HasIndex(e => e.IdSesion).HasDatabaseName("IX_ParticipantesSesion_IdSesion");
                entity.HasIndex(e => e.IdMuestra).HasDatabaseName("IX_ParticipantesSesion_IdMuestra");
                entity.HasIndex(e => e.Asistencia).HasDatabaseName("IX_ParticipantesSesion_Asistencia");
            });
        }
    }
}
