using System;
using Microsoft.EntityFrameworkCore;

namespace MatrixNext.Data.Entities
{
    public class MatrixDbContext : DbContext
    {
        private readonly string? _connectionString;

        public MatrixDbContext(DbContextOptions<MatrixDbContext> options) : base(options)
        {
        }

        public MatrixDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<TH_SolicitudAusencia> SolicitudesAusencia => Set<TH_SolicitudAusencia>();
        public DbSet<TH_Ausencia_Incapacidades> AusenciaIncapacidades => Set<TH_Ausencia_Incapacidades>();

        public DbSet<CU_Brief> Briefs => Set<CU_Brief>();
        public DbSet<CU_Propuestas> Propuestas => Set<CU_Propuestas>();
        public DbSet<CU_Estudios> Estudios => Set<CU_Estudios>();
        public DbSet<CU_Estudios_Presupuestos> EstudiosPresupuestos => Set<CU_Estudios_Presupuestos>();
        public DbSet<CU_SeguimientoPropuestas> SeguimientoPropuestas => Set<CU_SeguimientoPropuestas>();
        public DbSet<CU_Presupuestos> Presupuestos => Set<CU_Presupuestos>();

        public DbSet<IQ_Parametros> IQParametros => Set<IQ_Parametros>();
        public DbSet<IQ_DatosGeneralesPresupuesto> IQDatosGenerales => Set<IQ_DatosGeneralesPresupuesto>();
        public DbSet<IQ_Muestra_1> IQMuestra => Set<IQ_Muestra_1>();
        public DbSet<IQ_Preguntas> IQPreguntas => Set<IQ_Preguntas>();
        public DbSet<IQ_ProcesosPresupuesto> IQProcesos => Set<IQ_ProcesosPresupuesto>();
        public DbSet<IQ_CostoActividades> IQCostoActividades => Set<IQ_CostoActividades>();
        public DbSet<IQ_ControlCostos> IQControlCostos => Set<IQ_ControlCostos>();
        public DbSet<IQ_OpcionesAplicadas> IQOpcionesAplicadas => Set<IQ_OpcionesAplicadas>();
        public DbSet<IQ_OpcionesTecnicas> IQOpcionesTecnicas => Set<IQ_OpcionesTecnicas>();
        public DbSet<IQ_TecnicaProcesos> IQTecnicaProcesos => Set<IQ_TecnicaProcesos>();
        public DbSet<IQ_Procesos> IQProcesosCatalogo => Set<IQ_Procesos>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                if (string.IsNullOrWhiteSpace(_connectionString))
                {
                    throw new InvalidOperationException("Connection string not provided for MatrixDbContext");
                }

                optionsBuilder.UseSqlServer(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TH_SolicitudAusencia>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");
                entity.Property(e => e.FiniCausacion).HasColumnName("FiniCausacion");
                entity.Property(e => e.FFinCausacion).HasColumnName("FFinCausacion");
                entity.Property(e => e.FInicio).HasColumnName("FInicio");
                entity.Property(e => e.FFin).HasColumnName("FFin");
                entity.Property(e => e.DiasCalendario).HasColumnName("DiasCalendario");
                entity.Property(e => e.DiasLaborales).HasColumnName("DiasLaborales");
                entity.Property(e => e.Tipo).HasColumnName("Tipo");
                entity.Property(e => e.Estado).HasColumnName("Estado");
                entity.Property(e => e.AprobadoPor).HasColumnName("AprobadoPor");
                entity.Property(e => e.FechaAprobacion).HasColumnName("FechaAprobacion");
                entity.Property(e => e.VoBo1).HasColumnName("VoBo1");
                entity.Property(e => e.FechaVoBo1).HasColumnName("FechaVoBo1");
                entity.Property(e => e.VoBo2).HasColumnName("VoBo2");
                entity.Property(e => e.FechaVoBo2).HasColumnName("FechaVoBo2");
                entity.Property(e => e.VoBo3).HasColumnName("VoBo3");
                entity.Property(e => e.FechaVoBo3).HasColumnName("FechaVoBo3");
                entity.Property(e => e.RegistradoPor).HasColumnName("RegistradoPor");
                entity.Property(e => e.FechaRegistro).HasColumnName("FechaRegistro");
                entity.Property(e => e.ObservacionesSolicitud).HasColumnName("ObservacionesSolicitud");
                entity.Property(e => e.ObservacionesAprobacion).HasColumnName("ObservacionesAprobacion");
            });

            modelBuilder.Entity<TH_Ausencia_Incapacidades>(entity =>
            {
                entity.HasKey(e => e.IdSolicitudAusencia);
                entity.Property(e => e.IdSolicitudAusencia).HasColumnName("idSolicitudAusencia");
                entity.Property(e => e.EntidadConsulta).HasColumnName("EntidadConsulta");
                entity.Property(e => e.IPS).HasColumnName("IPS");
                entity.Property(e => e.RegistroMedico).HasColumnName("RegistroMedico");
                entity.Property(e => e.TipoIncapacidad).HasColumnName("TipoIncapacidad");
                entity.Property(e => e.ClaseAusencia).HasColumnName("ClaseAusencia");
                entity.Property(e => e.SOAT).HasColumnName("SOAT");
                entity.Property(e => e.FechaAccidenteTrabajo).HasColumnName("FechaAccidenteTrabajo");
                entity.Property(e => e.Comentarios).HasColumnName("Comentarios");
                entity.Property(e => e.CIE).HasColumnName("CIE");
            });

            modelBuilder.Entity<CU_Brief>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("Id");
            });

            modelBuilder.Entity<CU_Propuestas>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("Id");
            });

            modelBuilder.Entity<CU_Estudios>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
            });

            modelBuilder.Entity<CU_Estudios_Presupuestos>(entity =>
            {
                entity.HasKey(e => new { e.EstudioId, e.PresupuestoId });
                entity.Property(e => e.EstudioId).HasColumnName("EstudioId");
                entity.Property(e => e.PresupuestoId).HasColumnName("PresupuestoId");
            });

            modelBuilder.Entity<CU_SeguimientoPropuestas>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("Id");
            });

            modelBuilder.Entity<CU_Presupuestos>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("Id");
                entity.Property(e => e.PropuestaId).HasColumnName("PropuestaId");
                entity.Property(e => e.Aprobado).HasColumnName("Aprobado");
                entity.Property(e => e.Alternativa).HasColumnName("Alternativa");
                entity.Property(e => e.EstadoId).HasColumnName("EstadoId");
                entity.Property(e => e.GrossMargin).HasColumnName("GrossMargin");
                entity.Property(e => e.JobBook).HasColumnName("JobBook");
                entity.Property(e => e.Muestra).HasColumnName("Muestra");
                entity.Property(e => e.Nombre).HasColumnName("Nombre");
                entity.Property(e => e.Nacional).HasColumnName("Nacional");
                entity.Property(e => e.ParaRevisar).HasColumnName("ParaRevisar");
                entity.Property(e => e.ProductoId).HasColumnName("ProductoId");
                entity.Property(e => e.UsadoPropuesta).HasColumnName("UsadoPropuesta");
                entity.Property(e => e.Valor).HasColumnName("Valor");
                entity.Property(e => e.Visible).HasColumnName("Visible");

                entity.HasOne(e => e.CU_Propuestas)
                    .WithMany(p => p.CU_Presupuestos)
                    .HasForeignKey(e => e.PropuestaId);
            });

            modelBuilder.Entity<IQ_Parametros>(entity =>
            {
                entity.HasKey(e => new { e.IdPropuesta, e.ParAlternativa, e.MetCodigo, e.ParNacional });
                entity.Property(e => e.ParNomPresupuesto).HasMaxLength(255);
                entity.Property(e => e.Pr_ProductCode).HasMaxLength(255);
                entity.Property(e => e.Pr_Offeringcode).HasMaxLength(255);
                entity.Property(e => e.ParNumJobBook).HasMaxLength(255);
                entity.Property(e => e.ParDicultadTargetCualitativo).HasMaxLength(255);
                entity.Property(e => e.ParObservaciones).HasColumnName("ParObservaciones");
                entity.Property(e => e.ParAnoSiguiente).HasColumnName("ParAñoSiguiente");
                entity.Property(e => e.NoIQuote).HasMaxLength(255);

                entity.HasMany(e => e.IQ_Muestra)
                    .WithOne(m => m.IQ_Parametros)
                    .HasForeignKey(m => new { m.IdPropuesta, m.ParAlternativa, m.MetCodigo, m.ParNacional });

                entity.HasMany(e => e.IQ_ProcesosPresupuesto)
                    .WithOne(p => p.IQ_Parametros)
                    .HasForeignKey(p => new { p.IdPropuesta, p.ParAlternativa, p.MetCodigo, p.ParNacional });

                entity.HasMany(e => e.IQ_CostoActividades)
                    .WithOne(c => c.IQ_Parametros)
                    .HasForeignKey(c => new { c.IdPropuesta, c.ParAlternativa, c.MetCodigo, c.ParNacional });

                entity.HasOne(e => e.IQ_Preguntas)
                    .WithOne(p => p.IQ_Parametros)
                    .HasForeignKey<IQ_Preguntas>(p => new { p.IdPropuesta, p.ParAlternativa, p.MetCodigo, p.ParNacional });

                entity.HasMany(e => e.IQ_OpcionesAplicadas)
                    .WithOne(o => o.IQ_Parametros)
                    .HasForeignKey(o => new { o.IdPropuesta, o.ParAlternativa, o.ParNacional, o.MetCodigo });
            });

            modelBuilder.Entity<IQ_DatosGeneralesPresupuesto>(entity =>
            {
                entity.HasKey(e => new { e.IdPropuesta, e.ParAlternativa });
            });

            modelBuilder.Entity<IQ_Muestra_1>(entity =>
            {
                entity.HasKey(e => new { e.IdPropuesta, e.ParAlternativa, e.MetCodigo, e.CiuCodigo, e.MuIdentificador, e.ParNacional });
            });

            modelBuilder.Entity<IQ_Preguntas>(entity =>
            {
                entity.HasKey(e => new { e.IdPropuesta, e.ParAlternativa, e.MetCodigo, e.ParNacional });
            });

            modelBuilder.Entity<IQ_ProcesosPresupuesto>(entity =>
            {
                entity.HasKey(e => new { e.IdPropuesta, e.ParAlternativa, e.MetCodigo, e.ProcCodigo, e.ParNacional });
            });

            modelBuilder.Entity<IQ_CostoActividades>(entity =>
            {
                entity.HasKey(e => new { e.IdPropuesta, e.ParAlternativa, e.MetCodigo, e.ActCodigo, e.ParNacional });
            });

            modelBuilder.Entity<IQ_ControlCostos>(entity =>
            {
                entity.HasKey(e => new { e.IdPropuesta, e.ParAlternativa, e.MetCodigo, e.ParNacional, e.ID });
            });

            modelBuilder.Entity<IQ_OpcionesAplicadas>(entity =>
            {
                entity.HasKey(e => new { e.IdPropuesta, e.ParAlternativa, e.ParNacional, e.MetCodigo, e.IdOpcion, e.TecCodigo });

                entity.HasOne(e => e.IQ_OpcionesTecnicas)
                    .WithMany(t => t.IQ_OpcionesAplicadas)
                    .HasForeignKey(e => new { e.TecCodigo, e.IdOpcion });
            });

            modelBuilder.Entity<IQ_OpcionesTecnicas>(entity =>
            {
                entity.HasKey(e => new { e.TecCodigo, e.IdOpcion });
            });

            modelBuilder.Entity<IQ_Procesos>(entity =>
            {
                entity.HasKey(e => e.ProcCodigo);
            });

            modelBuilder.Entity<IQ_TecnicaProcesos>(entity =>
            {
                entity.HasKey(e => new { e.ProcCodigo, e.TecCodigo });
            });
        }
    }
}
