using WellMindApi.Application.DTOs.Registros;
using WellMindApi.Domain.Interfaces.Repositories;

namespace WellMindApi.Application.UseCases.Registros;

/// <summary>
/// Caso de Uso: Analisar bem-estar completo de um usuário
/// </summary>
public class AnalisarBemEstarUsuarioUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IRegistroBemEstarRepository _registroRepository;

    public AnalisarBemEstarUsuarioUseCase(
        IUsuarioRepository usuarioRepository,
        IRegistroBemEstarRepository registroRepository)
    {
        _usuarioRepository = usuarioRepository;
        _registroRepository = registroRepository;
    }

    public async Task<AnaliseBemEstarDto?> ExecutarAsync(int idUsuario)
    {
        var usuario = await _usuarioRepository.ObterPorIdAsync(idUsuario);
        if (usuario == null)
            return null;

        var registros = await _registroRepository.ObterPorUsuarioAsync(idUsuario, 7);
        var registrosLista = registros.ToList();

        if (!registrosLista.Any())
            return null;

        // Calcular médias (com conversão explícita)
        var mediaHumor = (decimal)registrosLista.Average(r => r.NivelHumor);
        var mediaEstresse = (decimal)registrosLista.Average(r => r.NivelEstresse);
        var mediaEnergia = (decimal)registrosLista.Average(r => r.NivelEnergia);
        var mediaHorasSono = (decimal)registrosLista.Average(r => r.HorasSono);
        var mediaQualidadeSono = (decimal)registrosLista.Average(r => r.QualidadeSono);

        // Calcular índice geral
        var indiceBemEstarMedio = usuario.CalcularIndiceBemEstarMedio(7);

        // Determinar status
        var statusSaude = indiceBemEstarMedio switch
        {
            >= 8 => "EXCELENTE",
            >= 6 => "BOM",
            >= 4 => "REGULAR",
            >= 2 => "RUIM",
            _ => "CRÍTICO"
        };

        // Verificar risco de burnout
        var emRiscoBurnout = usuario.EmRiscoDeBurnout();

        // Identificar áreas de atenção
        var areasAtencao = new List<string>();
        if (mediaEstresse >= 7) areasAtencao.Add("ESTRESSE");
        if (mediaHorasSono < 6) areasAtencao.Add("SONO");
        if (mediaEnergia <= 4) areasAtencao.Add("ENERGIA");
        if (mediaHumor <= 4) areasAtencao.Add("HUMOR");

        // Gerar recomendações
        var recomendacoes = GerarRecomendacoes(areasAtencao, emRiscoBurnout);

        // Calcular tendências (últimos 3 vs primeiros 3 registros)
        var (tendenciaHumor, tendenciaEstresse, tendenciaEnergia) = CalcularTendencias(registrosLista);

        return new AnaliseBemEstarDto
        {
            IdUsuario = idUsuario,
            NomeUsuario = usuario.Nome,
            TotalRegistros = registrosLista.Count,
            IndiceBemEstarMedio = indiceBemEstarMedio,
            StatusGeralSaude = statusSaude,
            EmRiscoBurnout = emRiscoBurnout,
            MediaHumor = mediaHumor,
            MediaEstresse = mediaEstresse,
            MediaEnergia = mediaEnergia,
            MediaHorasSono = mediaHorasSono,
            MediaQualidadeSono = mediaQualidadeSono,
            TendenciaHumor = tendenciaHumor,
            TendenciaEstresse = tendenciaEstresse,
            TendenciaEnergia = tendenciaEnergia,
            AreasAtencao = areasAtencao,
            Recomendacoes = recomendacoes
        };
    }

    private static List<string> GerarRecomendacoes(List<string> areasAtencao, bool emRiscoBurnout)
    {
        var recomendacoes = new List<string>();

        if (emRiscoBurnout)
        {
            recomendacoes.Add("⚠️ ALERTA: Sinais críticos de burnout detectados. Busque apoio profissional imediatamente.");
        }

        foreach (var area in areasAtencao)
        {
            var recomendacao = area switch
            {
                "ESTRESSE" => "Pratique técnicas de respiração e meditação. Faça pausas regulares durante o trabalho.",
                "SONO" => "Estabeleça uma rotina de sono consistente. Evite telas 1 hora antes de dormir.",
                "ENERGIA" => "Mantenha uma alimentação balanceada e pratique exercícios físicos leves regularmente.",
                "HUMOR" => "Conecte-se com pessoas queridas. Considere agendar uma sessão com profissional de saúde mental.",
                _ => ""
            };

            if (!string.IsNullOrEmpty(recomendacao))
                recomendacoes.Add(recomendacao);
        }

        if (!recomendacoes.Any())
        {
            recomendacoes.Add("Continue mantendo seus bons hábitos de bem-estar!");
        }

        return recomendacoes;
    }

    private static (string humor, string estresse, string energia) CalcularTendencias(List<Domain.Entities.RegistroBemEstar> registros)
    {
        if (registros.Count < 3)
            return ("ESTAVEL", "ESTAVEL", "ESTAVEL");

        var ordenados = registros.OrderBy(r => r.DataRegistro).ToList();
        var metade = ordenados.Count / 2;

        var primeiraMetade = ordenados.Take(metade).ToList();
        var segundaMetade = ordenados.Skip(metade).ToList();

        var tendenciaHumor = CalcularTendencia(
            (decimal)primeiraMetade.Average(r => r.NivelHumor),
            (decimal)segundaMetade.Average(r => r.NivelHumor)
        );

        var tendenciaEstresse = CalcularTendencia(
            (decimal)segundaMetade.Average(r => r.NivelEstresse), // Invertido
            (decimal)primeiraMetade.Average(r => r.NivelEstresse)
        );

        var tendenciaEnergia = CalcularTendencia(
            (decimal)primeiraMetade.Average(r => r.NivelEnergia),
            (decimal)segundaMetade.Average(r => r.NivelEnergia)
        );

        return (tendenciaHumor, tendenciaEstresse, tendenciaEnergia);
    }

    private static string CalcularTendencia(decimal valorAntigo, decimal valorNovo)
    {
        var diferenca = valorNovo - valorAntigo;

        if (diferenca > 1) return "MELHORANDO";
        if (diferenca < -1) return "PIORANDO";
        return "ESTAVEL";
    }
}
