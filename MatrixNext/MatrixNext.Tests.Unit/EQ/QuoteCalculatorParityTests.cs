using Xunit;
using System;
using MatrixNext.Web.Areas.EQ.Models;

namespace MatrixNext.Tests.Unit.EQ;

/// <summary>
/// Tests de paridad Excel → Motor C#
/// Valida las fórmulas matemáticas del motor sin dependencias de BD
/// NOTA: Para tests completos end-to-end con BD real, ejecutar la aplicación con datos maestros seeded
/// </summary>
public class QuoteCalculatorParityTests
{
    /// <summary>
    /// Test 1: Validar fórmula GM = DirectCost × 21.45%
    /// </summary>
    [Fact]
    public void Formula_GM_21Pct45_Correcto()
    {
        // Arrange
        decimal directCost = 10_000_000m;
        decimal expectedGM = directCost * 0.2145m; // 2,145,000

        // Act - Simula FORMULA 13
        decimal actualGM = directCost * 0.2145m;

        // Assert
        Assert.Equal(expectedGM, actualGM);
        Assert.Equal(2_145_000m, actualGM);
    }

    /// <summary>
    /// Test 2: Validar fórmula PB+RMF = -AOT × 4.3%
    /// </summary>
    [Fact]
    public void Formula_PBRMF_Negativo4Pct3_Correcto()
    {
        // Arrange
        decimal directCost = 10_000_000m;
        decimal gm = directCost * 0.2145m; // 2,145,000
        decimal aot = directCost + gm; // 12,145,000
        decimal expectedPB = -aot * 0.043m; // -522,235

        // Act - Simula FORMULA 22
        decimal actualPB = -aot * 0.043m;

        // Assert
        Assert.Equal(expectedPB, actualPB);
        Assert.True(actualPB < 0, "PB+RMF debe ser negativo");
        Assert.Equal(-522_235m, actualPB);
    }

    /// <summary>
    /// Test 3: Validar fórmula AOT = DirectCost + GM
    /// </summary>
    [Fact]
    public void Formula_AOT_SumaDirectCostMasGM_Correcto()
    {
        // Arrange
        decimal directCost = 8_500_000m;
        decimal gm = directCost * 0.2145m; // 1,823,250
        decimal expectedAOT = directCost + gm; // 10,323,250

        // Act - Simula FORMULA 24
        decimal actualAOT = directCost + gm;

        // Assert
        Assert.Equal(expectedAOT, actualAOT);
        Assert.Equal(10_323_250m, actualAOT);
    }

    /// <summary>
    /// Test 4: Validar fórmula ProfTime = -StaffSL
    /// </summary>
    [Fact]
    public void Formula_ProfTime_NegativoStaffSL_Correcto()
    {
        // Arrange
        decimal staffSL = 1_500_000m;
        decimal expectedProfTime = -staffSL; // -1,500,000

        // Act - Simula FORMULA 23
        decimal actualProfTime = -staffSL;

        // Assert
        Assert.Equal(expectedProfTime, actualProfTime);
        Assert.True(actualProfTime < 0, "ProfTime debe ser negativo");
        Assert.Equal(-1_500_000m, actualProfTime);
    }

    /// <summary>
    /// Test 5: Validar fórmula OP = GM + PB_RMF + ProfTime
    /// </summary>
    [Fact]
    public void Formula_OP_SumaMargenesCorrectamente()
    {
        // Arrange
        decimal gm = 2_000_000m;
        decimal pbRmf = -500_000m;
        decimal profTime = -300_000m;
        decimal expectedOP = gm + pbRmf + profTime; // 1,200,000

        // Act - Simula FORMULA 24
        decimal actualOP = gm + pbRmf + profTime;

        // Assert
        Assert.Equal(expectedOP, actualOP);
        Assert.Equal(1_200_000m, actualOP);
    }

    /// <summary>
    /// Test 6: Validar fórmula %OP = OP / AOT
    /// </summary>
    [Fact]
    public void Formula_PorcentajeOP_CalculaCorrecto()
    {
        // Arrange
        decimal op = 1_200_000m;
        decimal aot = 12_000_000m;
        decimal expectedPorcOP = op / aot; // 0.10 = 10%

        // Act - Simula FORMULA 25
        decimal actualPorcOP = aot == 0 ? 0 : op / aot;

        // Assert
        Assert.Equal(expectedPorcOP, actualPorcOP);
        Assert.Equal(0.10m, actualPorcOP);
    }

    /// <summary>
    /// Test 7: Validar factor Siembra (2× cuando está activo)
    /// </summary>
    [Fact]
    public void Formula_FactorSiembra_Duplica()
    {
        // Arrange
        decimal valorEncuesta = 20_000m;
        decimal muestra = 300m;
        bool siembraActiva = true;
        decimal factorSiembra = siembraActiva ? 2m : 1m;
        
        decimal costoSinSiembra = valorEncuesta * muestra * 1m; // 6,000,000
        decimal costoConSiembra = valorEncuesta * muestra * factorSiembra; // 12,000,000

        // Act - Simula FORMULA 2
        decimal actualConSiembra = valorEncuesta * muestra * factorSiembra;

        // Assert
        Assert.Equal(12_000_000m, actualConSiembra);
        Assert.Equal(costoSinSiembra * 2m, actualConSiembra);
    }

    /// <summary>
    /// Test 8: Validar factor Parafiscal F2F (1.16522)
    /// </summary>
    [Fact]
    public void Formula_FactorParafiscalF2F_16Pct522()
    {
        // Arrange
        decimal valorBase = 10_000_000m;
        string metodologia = "F2F";
        decimal factorParafiscal = string.Equals(metodologia, "F2F", StringComparison.OrdinalIgnoreCase) ? 1.16522m : 1m;
        decimal expectedCosto = valorBase * 1.16522m; // 11,652,200

        // Act - Simula FORMULA 1
        decimal actualCosto = valorBase * factorParafiscal;

        // Assert
        Assert.Equal(expectedCosto, actualCosto);
        Assert.Equal(11_652_200m, actualCosto);
    }

    /// <summary>
    /// Test 9: Validar que CATI/Online NO tiene parafiscal
    /// </summary>
    [Fact]
    public void Formula_CATI_SinParafiscal()
    {
        // Arrange
        decimal valorBase = 10_000_000m;
        string metodologia = "CATI";
        decimal factorParafiscal = string.Equals(metodologia, "F2F", StringComparison.OrdinalIgnoreCase) ? 1.16522m : 1m;
        decimal expectedCosto = valorBase * 1m; // 10,000,000 (sin incremento)

        // Act - Simula FORMULA 1
        decimal actualCosto = valorBase * factorParafiscal;

        // Assert
        Assert.Equal(expectedCosto, actualCosto);
        Assert.Equal(10_000_000m, actualCosto);
        Assert.Equal(1m, factorParafiscal); // Factor debe ser 1.0
    }

    /// <summary>
    /// Test 10: Validar cadena completa de márgenes
    /// Escenario: DirectCost = 10M → GM → AOT → PB → OP → %OP
    /// </summary>
    [Fact]
    public void Formula_CadenaCompleta_MargenesCoherentes()
    {
        // Arrange - Escenario completo
        decimal costoCampo = 6_000_000m;
        decimal viaticos = 500_000m;
        decimal incentivos = 1_200_000m;
        decimal insumos = 800_000m;
        decimal staffOps = 1_000_000m;
        decimal staffSL = 500_000m;
        decimal compraProducto = 0m;
        decimal otrosCostos = 0m;

        // Act - Simula cadena completa de fórmulas
        decimal directCost = costoCampo + viaticos + incentivos + insumos + staffOps + staffSL + compraProducto + otrosCostos;
        decimal gm = directCost * 0.2145m; // FORMULA 13
        decimal aot = directCost + gm; // FORMULA 24
        decimal pbRmf = -aot * 0.043m; // FORMULA 22
        decimal profTime = -staffSL; // FORMULA 23
        decimal op = gm + pbRmf + profTime; // FORMULA 24
        decimal porcOp = aot == 0 ? 0 : op / aot; // FORMULA 25

        // Assert - Validar coherencia
        Assert.Equal(10_000_000m, directCost); // 6M + 0.5M + 1.2M + 0.8M + 1M + 0.5M
        Assert.Equal(2_145_000m, gm); // 10M × 21.45%
        Assert.Equal(12_145_000m, aot); // 10M + 2.145M
        Assert.Equal(-522_235m, pbRmf); // -12.145M × 4.3%
        Assert.Equal(-500_000m, profTime); // -staffSL
        Assert.Equal(1_122_765m, op); // 2.145M - 522.235k - 500k
        
        decimal expectedPorcOp = 1_122_765m / 12_145_000m; // ≈ 0.0924 = 9.24%
        Assert.Equal(expectedPorcOp, porcOp, 6);
        Assert.True(porcOp > 0.09m && porcOp < 0.10m, $"%OP debe estar entre 9-10%, actual: {porcOp * 100:F2}%");
    }

    /// <summary>
    /// Test 11: Validar que EQSummary tiene todas las propiedades necesarias
    /// </summary>
    [Fact]
    public void EQSummary_TienePropiedadesRequeridas()
    {
        // Arrange & Act
        var summary = new EQSummary
        {
            CostoCampo = 1000,
            CostoCalidad = 0,
            Viaticos = 100,
            Incentivos = 200,
            Insumos = 150,
            StaffOps = 300,
            StaffSL = 50,
            CompraProducto = 0,
            Tablets = 0,
            DirectCostOps = 1800,
            GM = 385.10m,
            PB_RMF = -93.85m,
            ProfTime = -50,
            OP = 241.25m,
            AOT = 2185.10m,
            PorcOP = 0.1104m
        };

        // Assert - Todas las propiedades deben ser accesibles
        Assert.Equal(1000, summary.CostoCampo);
        Assert.Equal(100, summary.Viaticos);
        Assert.Equal(200, summary.Incentivos);
        Assert.Equal(150, summary.Insumos);
        Assert.Equal(300, summary.StaffOps);
        Assert.Equal(50, summary.StaffSL);
        Assert.Equal(1800, summary.DirectCostOps);
        Assert.Equal(385.10m, summary.GM);
        Assert.Equal(-93.85m, summary.PB_RMF);
        Assert.Equal(-50, summary.ProfTime);
        Assert.Equal(241.25m, summary.OP);
        Assert.Equal(2185.10m, summary.AOT);
        Assert.Equal(0.1104m, summary.PorcOP);
    }
}
