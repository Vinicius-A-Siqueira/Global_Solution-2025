using Microsoft.ML.Data;

namespace WellMindApi.Infrastructure.External.ML.Models;

/// <summary>
/// Modelo de entrada para predição de burnout usando ML.NET
/// </summary>
public class BurnoutModelInput
{
    [LoadColumn(0)]
    public float NivelHumor { get; set; }

    [LoadColumn(1)]
    public float NivelEstresse { get; set; }

    [LoadColumn(2)]
    public float NivelEnergia { get; set; }

    [LoadColumn(3)]
    public float HorasSono { get; set; }

    [LoadColumn(4)]
    public float QualidadeSono { get; set; }

    [LoadColumn(5)]
    public float DiasSemDescanso { get; set; }

    [LoadColumn(6)]
    public float MediaHorasTrabalho { get; set; }
}

/// <summary>
/// Modelo de saída para predição de burnout
/// </summary>
public class BurnoutModelOutput
{
    [ColumnName("PredictedLabel")]
    public bool PossuiBurnout { get; set; }

    [ColumnName("Probability")]
    public float Probabilidade { get; set; }

    [ColumnName("Score")]
    public float Score { get; set; }
}
