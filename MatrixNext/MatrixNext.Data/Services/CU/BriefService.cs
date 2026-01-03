using System;
using System.Collections.Generic;
using MatrixNext.Data.Adapters.CU;
using MatrixNext.Data.Entities;
using MatrixNext.Data.Modules.CU.Models;
using MatrixNext.Data.Modules.US.Usuarios.Adapters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Services.CU
{
    public class BriefService
    {
        private readonly BriefDataAdapter _adapter;
        private readonly IConfiguration _configuration;
        private readonly ILogger<BriefService> _logger;

        public BriefService(BriefDataAdapter adapter, IConfiguration configuration, ILogger<BriefService> logger)
        {
            _adapter = adapter ?? throw new ArgumentNullException(nameof(adapter));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public BriefFormViewModel PrepararFormulario(long? idBrief, long usuarioId)
        {
            var vm = new BriefFormViewModel
            {
                Unidades = ObtenerUnidadesUsuario(usuarioId)
            };

            if (idBrief.HasValue && idBrief.Value > 0)
            {
                var entidad = _adapter.ObtenerPorId(idBrief.Value);
                if (entidad != null)
                {
                    vm.Brief = MapEntidad(entidad);
                }
            }
            else
            {
                vm.Brief.Fecha = DateTime.Now.Date;
                vm.Brief.Viabilidad = true;
            }

            return vm;
        }

        public (bool success, string message, long id) Guardar(BriefViewModel model, long usuarioId)
        {
            try
            {
                var validation = Validar(model);
                if (!string.IsNullOrEmpty(validation))
                {
                    return (false, validation, 0);
                }

                var entidad = model.Id > 0
                    ? _adapter.ObtenerPorId(model.Id) ?? new CU_Brief()
                    : new CU_Brief();

                entidad.Fecha = model.Fecha;
                entidad.MarcaCategoria = model.MarcaCategoria;
                entidad.Cliente = model.Cliente;
                entidad.Contacto = model.Contacto;
                entidad.Titulo = model.Titulo;
                entidad.Antecedentes = model.Antecedentes;
                entidad.Objetivos = model.Objetivos;
                entidad.ActionStandars = model.ActionStandars;
                entidad.Metodologia = model.Metodologia;
                entidad.Unidad = model.Unidad;
                entidad.NewClient = model.NewClient;
                entidad.O1 = model.O1;
                entidad.O2 = model.O2;
                entidad.O3 = model.O3;
                entidad.O4 = model.O4;
                entidad.O5 = model.O5;
                entidad.O6 = model.O6;
                entidad.O7 = model.O7;
                entidad.D1 = model.D1;
                entidad.D2 = model.D2;
                entidad.D3 = model.D3;
                entidad.C1 = model.C1;
                entidad.C2 = model.C2;
                entidad.C3 = model.C3;
                entidad.C4 = model.C4;
                entidad.C5 = model.C5;
                entidad.M1 = model.M1;
                entidad.M2 = model.M2;
                entidad.M3 = model.M3;
                entidad.DI1 = model.DI1;
                entidad.DI2 = model.DI2;
                entidad.DI3 = model.DI3;
                entidad.DI4 = model.DI4;
                entidad.DI5 = model.DI5;
                entidad.DI6 = model.DI6;
                entidad.DI7 = model.DI7;
                entidad.DI8 = model.DI8;
                entidad.DI9 = model.DI9;
                entidad.DI10 = model.DI10;
                entidad.DI11 = model.DI11;
                entidad.DI12 = model.DI12;
                entidad.DI13 = model.DI13;
                entidad.DI14 = model.DI14;
                entidad.DI15 = model.DI15;
                entidad.DI16 = model.DI16;
                entidad.DI17 = model.DI17;
                entidad.DI18 = model.DI18;

                if (entidad.Id == 0)
                {
                    entidad.TipoBrief = 1;
                    entidad.GerenteCuentas = usuarioId;
                    entidad.FechaViabilidad = DateTime.Now;
                    entidad.Viabilidad = true;
                    entidad.MarcaViabilidad = usuarioId;
                }

                var id = _adapter.Guardar(entidad);
                return (true, "Brief guardado correctamente", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando brief");
                return (false, ex.Message, 0);
            }
        }

        public (bool success, string message) MarcarViabilidad(long idBrief, bool viable, long usuarioId)
        {
            try
            {
                var entidad = _adapter.ObtenerPorId(idBrief);
                if (entidad == null) return (false, "Brief no encontrado");

                entidad.Viabilidad = viable;
                entidad.FechaViabilidad = DateTime.Now;
                entidad.MarcaViabilidad = usuarioId;

                _adapter.Guardar(entidad);
                return (true, "Viabilidad actualizada");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando viabilidad");
                return (false, ex.Message);
            }
        }

        private string? Validar(BriefViewModel model)
        {
            if (model == null) return "Modelo vacio";
            if (model.Fecha == null) return "La fecha del brief es requerida";
            if (string.IsNullOrWhiteSpace(model.Cliente)) return "El nombre de la empresa es requerido";
            if (string.IsNullOrWhiteSpace(model.Contacto)) return "El solicitante es requerido";
            if (string.IsNullOrWhiteSpace(model.MarcaCategoria)) return "El producto/marca es requerido";
            if (string.IsNullOrWhiteSpace(model.Titulo)) return "El nombre del proyecto es requerido";
            if (model.Unidad == null || model.Unidad <= 0) return "Seleccione la unidad";
            return null;
        }

        private BriefViewModel MapEntidad(CU_Brief entidad)
        {
            return new BriefViewModel
            {
                Id = entidad.Id,
                Fecha = entidad.Fecha,
                MarcaCategoria = entidad.MarcaCategoria,
                Cliente = entidad.Cliente,
                Contacto = entidad.Contacto,
                Titulo = entidad.Titulo,
                Antecedentes = entidad.Antecedentes,
                Objetivos = entidad.Objetivos,
                ActionStandars = entidad.ActionStandars,
                Metodologia = entidad.Metodologia,
                Unidad = entidad.Unidad,
                Viabilidad = entidad.Viabilidad,
                FechaViabilidad = entidad.FechaViabilidad,
                NewClient = entidad.NewClient ?? false,
                O1 = entidad.O1,
                O2 = entidad.O2,
                O3 = entidad.O3,
                O4 = entidad.O4,
                O5 = entidad.O5,
                O6 = entidad.O6,
                O7 = entidad.O7,
                D1 = entidad.D1,
                D2 = entidad.D2,
                D3 = entidad.D3,
                C1 = entidad.C1,
                C2 = entidad.C2,
                C3 = entidad.C3,
                C4 = entidad.C4,
                C5 = entidad.C5,
                M1 = entidad.M1,
                M2 = entidad.M2,
                M3 = entidad.M3,
                DI1 = entidad.DI1,
                DI2 = entidad.DI2,
                DI3 = entidad.DI3,
                DI4 = entidad.DI4,
                DI5 = entidad.DI5,
                DI6 = entidad.DI6,
                DI7 = entidad.DI7,
                DI8 = entidad.DI8,
                DI9 = entidad.DI9,
                DI10 = entidad.DI10,
                DI11 = entidad.DI11,
                DI12 = entidad.DI12,
                DI13 = entidad.DI13,
                DI14 = entidad.DI14,
                DI15 = entidad.DI15,
                DI16 = entidad.DI16,
                DI17 = entidad.DI17,
                DI18 = entidad.DI18
            };
        }

        private IEnumerable<UnidadViewModel> ObtenerUnidadesUsuario(long usuarioId)
        {
            var unidades = new List<UnidadViewModel>();
            try
            {
                var adapter = new UsuarioDataAdapter(_configuration.GetConnectionString("MatrixDb")
                    ?? throw new InvalidOperationException("Connection string MatrixDb not found"));
                var data = adapter.ObtenerUnidadesUsuario((int)usuarioId);
                foreach (var u in data)
                {
                    unidades.Add(new UnidadViewModel { Id = u.Id, Nombre = u.Nombre });
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "No se pudieron obtener unidades del usuario");
            }
            return unidades;
        }
    }
}
