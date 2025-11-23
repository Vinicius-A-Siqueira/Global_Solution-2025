using Microsoft.ML;
using Microsoft.ML.Data;
using WellMindApi.Infrastructure.External.ML.Models;

namespace WellMindApi.Infrastructure.External.ML;

/// <summary>
/// Preditor de burnout usando ML.NET
/// </summary>
public class BurnoutPredictor
{
    private readonly MLContext _mlContext;
    private ITransformer? _model;
    private readonly string _modelPath = "MLModels/burnout_model.zip";

    public BurnoutPredictor()
    {
        _mlContext = new MLContext(seed: 0);
    }

    /// <summary>
    /// Treina o modelo com dados fornecidos
    /// </summary>
    public void TreinarModelo(IEnumerable<BurnoutModelInput> dadosTreinamento)
    {
        var dataView = _mlContext.Data.LoadFromEnumerable(dadosTreinamento);

        // Pipeline de transformação e treinamento
        var pipeline = _mlContext.Transforms.Concatenate(
                "Features",
                nameof(BurnoutModelInput.NivelHumor),
                nameof(BurnoutModelInput.NivelEstresse),
                nameof(BurnoutModelInput.NivelEnergia),
                nameof(BurnoutModelInput.HorasSono),
                nameof(BurnoutModelInput.QualidadeSono),
                nameof(BurnoutModelInput.DiasSemDescanso),
                nameof(BurnoutModelInput.MediaHorasTrabalho))
            .Append(_mlContext.BinaryClassification.Trainers.FastTree(
                labelColumnName: "Label",
                featureColumnName: "Features",
                numberOfLeaves: 20,
                numberOfTrees: 100,
                minimumExampleCountPerLeaf: 10,
                learningRate: 0.2));

        // Treinar modelo
        _model = pipeline.Fit(dataView);

        // Salvar modelo
        _mlContext.Model.Save(_model, dataView.Schema, _modelPath);
    }

    /// <summary>
    /// Carrega modelo pré-treinado
    /// </summary>
    public void CarregarModelo()
    {
        if (File.Exists(_modelPath))
        {
            _model = _mlContext.Model.Load(_modelPath, out _);
        }
    }

    /// <summary>
    /// Prediz probabilidade de burnout
    /// </summary>
    public BurnoutModelOutput Predizer(BurnoutModelInput input)
    {
        if (_model == null)
        {
            // Se modelo não existe, usar lógica baseada em regras
            return PredizirComRegras(input);
        }

        var predictionEngine = _mlContext.Model.CreatePredictionEngine<BurnoutModelInput, BurnoutModelOutput>(_model);
        return predictionEngine.Predict(input);
    }

    /// <summary>
    /// Predição baseada em regras (fallback se modelo não existir)
    /// </summary>
    private static BurnoutModelOutput PredizirComRegras(BurnoutModelInput input)
    {
        // Lógica baseada em regras (usada anteriormente)
        var score = 0f;

        // Humor baixo
        if (input.NivelHumor <= 4) score += 2f;
        else if (input.NivelHumor <= 6) score += 1f;

        // Estresse alto
        if (input.NivelEstresse >= 8) score += 3f;
        else if (input.NivelEstresse >= 6) score += 1.5f;

        // Energia baixa
        if (input.NivelEnergia <= 3) score += 2.5f;
        else if (input.NivelEnergia <= 5) score += 1f;

        // Sono insuficiente
        if (input.HorasSono < 5) score += 2f;
        else if (input.HorasSono < 6) score += 1f;

        // Qualidade sono ruim
        if (input.QualidadeSono <= 4) score += 1.5f;

        // Normalizar score para probabilidade (0-1)
        var probabilidade = Math.Min(score / 10f, 1f);

        return new BurnoutModelOutput
        {
            PossuiBurnout = probabilidade >= 0.7f,
            Probabilidade = probabilidade,
            Score = score
        };
    }
}
