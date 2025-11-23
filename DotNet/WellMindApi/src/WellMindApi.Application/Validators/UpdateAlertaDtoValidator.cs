using FluentValidation;
using WellMindApi.Application.DTOs.Alertas;

namespace WellMindApi.Application.Validators;

/// <summary>
/// Validador para UpdateAlertaDto
/// </summary>
public class UpdateAlertaDtoValidator : AbstractValidator<UpdateAlertaDto>
{
    private static readonly string[] StatusValidos = { "PENDENTE", "EM_ANALISE", "RESOLVIDO", "CANCELADO" };

    public UpdateAlertaDtoValidator()
    {
        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status é obrigatório")
            .Must(BeValidStatus)
            .WithMessage("Status inválido. Use: PENDENTE, EM_ANALISE, RESOLVIDO ou CANCELADO");

        RuleFor(x => x.AcaoTomada)
            .MaximumLength(500).WithMessage("Ação tomada deve ter no máximo 500 caracteres")
            .When(x => !string.IsNullOrEmpty(x.AcaoTomada));
    }

    private bool BeValidStatus(string status)
    {
        return StatusValidos.Contains(status.ToUpper());
    }
}
