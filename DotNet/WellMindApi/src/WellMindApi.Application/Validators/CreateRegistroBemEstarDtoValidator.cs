using FluentValidation;
using WellMindApi.Application.DTOs.Registros;

namespace WellMindApi.Application.Validators;

/// <summary>
/// Validador para CreateRegistroBemEstarDto
/// </summary>
public class CreateRegistroBemEstarDtoValidator : AbstractValidator<CreateRegistroBemEstarDto>
{
    public CreateRegistroBemEstarDtoValidator()
    {
        RuleFor(x => x.IdUsuario)
            .GreaterThan(0).WithMessage("ID do usuário deve ser maior que zero");

        RuleFor(x => x.NivelHumor)
            .InclusiveBetween(1, 10).WithMessage("Nível de humor deve estar entre 1 e 10");

        RuleFor(x => x.NivelEstresse)
            .InclusiveBetween(1, 10).WithMessage("Nível de estresse deve estar entre 1 e 10");

        RuleFor(x => x.NivelEnergia)
            .InclusiveBetween(1, 10).WithMessage("Nível de energia deve estar entre 1 e 10");

        RuleFor(x => x.HorasSono)
            .InclusiveBetween(0, 24).WithMessage("Horas de sono deve estar entre 0 e 24");

        RuleFor(x => x.QualidadeSono)
            .InclusiveBetween(1, 10).WithMessage("Qualidade do sono deve estar entre 1 e 10");

        RuleFor(x => x.Observacoes)
            .MaximumLength(500).WithMessage("Observações deve ter no máximo 500 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Observacoes));
    }
}
