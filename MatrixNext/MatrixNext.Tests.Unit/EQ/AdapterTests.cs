using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using MatrixNext.Web.Models.EQ;
using MatrixNext.Web.Services.EQ.Adapters;
using MatrixNext.Web.Areas.EQ.Models;

namespace MatrixNext.Tests.Unit.EQ
{
    /// <summary>
    /// Tests para adaptador QuoteHeaderToViewModelAdapter
    /// Valida conversión de entidades EF a ViewModels para cálculos
    /// </summary>
    public class QuoteHeaderToViewModelAdapterTests
    {
        /// <summary>
        /// Test 1: Adapter convierte EqQuoteHeader con todos sus includes a ViewModel
        /// </summary>
        [Fact]
        public void Adapter_ConvertirCompleto_Exitosamente()
        {
            // Arrange
            var quoteHeader = new EqQuoteHeader
            {
                Id = 1,
                PropuestaNombre = "Test Proposal",
                GrupoObjetivo = "Test Target",
                Cliente = "Test Client",
                SL = "L5",
                MetodologiaSL = "F2F",
                RecordDetail = "Standard",
                CategoriaProducto = "Test",
                Notas = ""
            };

            var questionnaire = new EqQuestionnaire
            {
                Id = 1,
                QuoteHeaderId = 1,
                DuracionMinutos = 20,
                PenetracionLabel = "MAS82",
                TopLine = true
            };
            quoteHeader.Questionnaires = new List<EqQuestionnaire> { questionnaire };

            var methodology = new EqMethodology
            {
                Id = 1,
                QuoteHeaderId = 1,
                MetodologiaRecoleccion = "Hogares"
            };
            quoteHeader.Methodologies = new List<EqMethodology> { methodology };

            var city = new EqSampleCity
            {
                Id = 1,
                QuoteHeaderId = 1,
                Ciudad = "Bogotá",
                Activa = true,
                MuestraTotal = 400
            };
            quoteHeader.SampleCities = new List<EqSampleCity> { city };
            quoteHeader.Mysteries = new List<EqMystery>();
            quoteHeader.StaffSL = new List<EqStaffSL>();

            // Act
            var vm = new QuoteHeaderToViewModelAdapter().ToViewModel(quoteHeader);

            // Assert
            Assert.NotNull(vm);
            Assert.Equal(quoteHeader.PropuestaNombre, vm.Header.Nombre);
            Assert.Equal(quoteHeader.Cliente, vm.Header.Cliente);
            Assert.NotNull(vm.Questionnaire);
            Assert.Equal(20, vm.Questionnaire.DuracionMin);
            Assert.NotNull(vm.SampleCities);
            Assert.Single(vm.SampleCities);
            Assert.Equal("Bogotá", vm.SampleCities.First().Ciudad);
        }

        /// <summary>
        /// Test 2: Adapter maneja nulls correctamente
        /// </summary>
        [Fact]
        public void Adapter_ConNull_NoThrow()
        {
            // Act
            var vm = new QuoteHeaderToViewModelAdapter().ToViewModel(null);

            // Assert
            Assert.NotNull(vm);
            Assert.NotNull(vm.Header);
            Assert.NotNull(vm.Questionnaire);
            Assert.NotNull(vm.SampleCities);
            Assert.Empty(vm.SampleCities);
        }

        /// <summary>
        /// Test 3: Adapter mapea Mystery visits correctamente
        /// </summary>
        [Fact]
        public void Adapter_MysteryVisits_MappingCorrecto()
        {
            // Arrange
            var quoteHeader = new EqQuoteHeader
            {
                Id = 1,
                PropuestaNombre = "Test",
                GrupoObjetivo = "Test",
                Cliente = "Test",
                SL = "SL",
                MetodologiaSL = "F2F",
                RecordDetail = "Test",
                CategoriaProducto = "Test",
                Notas = ""
            };

            var mystery = new EqMystery
            {
                Id = 1,
                QuoteHeaderId = 1,
                TipoVisita = 1,
                Complejidad = "Alta",
                NumOlas = 2,
                Desplazamientos = 50000m,
                Tanques = 25000m,
                EdicionVideo = 100000m
            };

            quoteHeader.Mysteries = new List<EqMystery> { mystery };
            quoteHeader.Questionnaires = new List<EqQuestionnaire>();
            quoteHeader.Methodologies = new List<EqMethodology>();
            quoteHeader.SampleCities = new List<EqSampleCity>();
            quoteHeader.StaffSL = new List<EqStaffSL>();

            // Act
            var vm = new QuoteHeaderToViewModelAdapter().ToViewModel(quoteHeader);

            // Assert
            Assert.NotNull(vm.MysteryVisits);
            Assert.Single(vm.MysteryVisits);
            
            var mappedMystery = vm.MysteryVisits.First();
            Assert.Equal("1", mappedMystery.TipoVisita);
            Assert.Equal("Alta", mappedMystery.Complejidad);
            Assert.Equal(2, mappedMystery.NumOlas);
            Assert.Equal(50000m, mappedMystery.Desplazamientos);
            Assert.Equal(25000m, mappedMystery.Tanqueos);
            Assert.Equal(100000m, mappedMystery.Edicion);
        }

        /// <summary>
        /// Test 4: Adapter mapea Staff SL correctamente
        /// </summary>
        [Fact]
        public void Adapter_StaffSL_MappingCorrecto()
        {
            // Arrange
            var quoteHeader = new EqQuoteHeader
            {
                Id = 1,
                PropuestaNombre = "Test",
                GrupoObjetivo = "Test",
                Cliente = "Test",
                SL = "SL",
                MetodologiaSL = "F2F",
                RecordDetail = "Test",
                CategoriaProducto = "Test",
                Notas = ""
            };

            var staff = new EqStaffSL
            {
                Id = 1,
                QuoteHeaderId = 1,
                Nivel = "L6",
                HorasMinimas = 10m,
                HorasPresupuestadas = 20m,
                TarifaNivel = 75000m
            };

            quoteHeader.StaffSL = new List<EqStaffSL> { staff };
            quoteHeader.Questionnaires = new List<EqQuestionnaire>();
            quoteHeader.Methodologies = new List<EqMethodology>();
            quoteHeader.SampleCities = new List<EqSampleCity>();
            quoteHeader.Mysteries = new List<EqMystery>();

            // Act
            var vm = new QuoteHeaderToViewModelAdapter().ToViewModel(quoteHeader);

            // Assert
            Assert.NotNull(vm.StaffSL);
            Assert.Single(vm.StaffSL);
            
            var mappedStaff = vm.StaffSL.First();
            Assert.Equal("L6", mappedStaff.Nivel);
            Assert.Equal(10m, mappedStaff.HorasMinimas);
            Assert.Equal(20m, mappedStaff.HorasPresup);
            Assert.Equal(75000m, mappedStaff.Tarifa);
        }

        /// <summary>
        /// Test 5: Adapter mapea Sample Cities correctamente
        /// </summary>
        [Fact]
        public void Adapter_SampleCities_MappingCorrecto()
        {
            // Arrange
            var quoteHeader = new EqQuoteHeader
            {
                Id = 1,
                PropuestaNombre = "Test",
                GrupoObjetivo = "Test",
                Cliente = "Test",
                SL = "SL",
                MetodologiaSL = "F2F",
                RecordDetail = "Test",
                CategoriaProducto = "Test",
                Notas = ""
            };

            var city = new EqSampleCity
            {
                Id = 1,
                QuoteHeaderId = 1,
                Ciudad = "Medellín",
                Activa = true,
                MuestraTotal = 300,
                NSE1 = 30,
                NSE2 = 60,
                NSE3 = 80,
                NSE4 = 80,
                NSE5 = 40,
                NSE6 = 10
            };

            quoteHeader.SampleCities = new List<EqSampleCity> { city };
            quoteHeader.Questionnaires = new List<EqQuestionnaire>();
            quoteHeader.Methodologies = new List<EqMethodology>();
            quoteHeader.Mysteries = new List<EqMystery>();
            quoteHeader.StaffSL = new List<EqStaffSL>();

            // Act
            var vm = new QuoteHeaderToViewModelAdapter().ToViewModel(quoteHeader);

            // Assert
            Assert.NotNull(vm.SampleCities);
            Assert.Single(vm.SampleCities);
            
            var mappedCity = vm.SampleCities.First();
            Assert.Equal("Medellín", mappedCity.Ciudad);
            Assert.True(mappedCity.Activa);
            Assert.Equal(300m, mappedCity.MuestraTotal);
            Assert.Equal(30m, mappedCity.NSE1);
            Assert.Equal(60m, mappedCity.NSE2);
            Assert.Equal(80m, mappedCity.NSE3);
            Assert.Equal(80m, mappedCity.NSE4);
            Assert.Equal(40m, mappedCity.NSE5);
            Assert.Equal(10m, mappedCity.NSE6);
        }
    }
}
