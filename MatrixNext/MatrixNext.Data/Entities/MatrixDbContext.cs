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
        }
    }
}
