using FluentValidation;
using WellMindApi.Application.DTOs.Alertas;

namespace WellMindApi.Application.Validators;

/// <summary>
/// Validador para CreateAlertaDto
/// </summary>
public class CreateAlertaDtoValidator : AbstractValidator<CreateAlertaDto>
{
    private static readonly string[] NiveisGravidadeValidos = { "BAIXO", "MEDIO", "ALTO", "CRITICO" };

    public CreateAlertaDtoValidator()
    {
        RuleFor(x => x.IdUsuario)
            .GreaterThan(0).WithMessage("ID do usuário deve ser maior que zero");

        RuleFor(x => x.TipoAlerta)
            .NotEmpty().WithMessage("Tipo de alerta é obrigatório")
            .MaximumLength(50).WithMessage("Tipo de alerta deve ter no máximo 50 caracteres");

        RuleFor(x => x.NivelGravidade)
            .NotEmpty().WithMessage("Nível de gravidade é obrigatório")
            .Must(BeValidNivelGravidade)
            .WithMessage("Nível de gravidade inválido. Use: BAIXO, MEDIO, ALTO ou CRITICO");

        RuleFor(x => x.Descricao)
            .MaximumLength(500).WithMessage("Descrição deve ter no máximo 500 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Descricao));
    }

    private bool BeValidNivelGravidade(string nivel)
    {
        return NiveisGravidadeValidos.Contains(nivel.ToUpper());
    }
}
