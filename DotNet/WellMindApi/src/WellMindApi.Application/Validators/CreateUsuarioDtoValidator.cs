using FluentValidation;
using WellMindApi.Application.DTOs.Usuarios;

namespace WellMindApi.Application.Validators;

/// <summary>
/// Validador para CreateUsuarioDto
/// </summary>
public class CreateUsuarioDtoValidator : AbstractValidator<CreateUsuarioDto>
{
    public CreateUsuarioDtoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório")
            .MinimumLength(3).WithMessage("Nome deve ter no mínimo 3 caracteres")
            .MaximumLength(100).WithMessage("Nome deve ter no máximo 100 caracteres");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email é obrigatório")
            .EmailAddress().WithMessage("Email inválido")
            .MaximumLength(100).WithMessage("Email deve ter no máximo 100 caracteres");

        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("Senha é obrigatória")
            .MinimumLength(6).WithMessage("Senha deve ter no mínimo 6 caracteres")
            .MaximumLength(50).WithMessage("Senha deve ter no máximo 50 caracteres")
            .Matches(@"[A-Z]").WithMessage("Senha deve conter pelo menos uma letra maiúscula")
            .Matches(@"[a-z]").WithMessage("Senha deve conter pelo menos uma letra minúscula")
            .Matches(@"[0-9]").WithMessage("Senha deve conter pelo menos um número");

        RuleFor(x => x.DataNascimento)
            .NotEmpty().WithMessage("Data de nascimento é obrigatória")
            .Must(BeValidAge).WithMessage("Usuário deve ter entre 18 e 120 anos");

        RuleFor(x => x.Telefone)
            .MaximumLength(20).WithMessage("Telefone deve ter no máximo 20 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Telefone));

        RuleFor(x => x.Genero)
            .MaximumLength(20).WithMessage("Gênero deve ter no máximo 20 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Genero));
    }

    private bool BeValidAge(DateTime dataNascimento)
    {
        var idade = CalcularIdade(dataNascimento);
        return idade >= 18 && idade <= 120;
    }

    private int CalcularIdade(DateTime dataNascimento)
    {
        var hoje = DateTime.Today;
        var idade = hoje.Year - dataNascimento.Year;
        if (dataNascimento.Date > hoje.AddYears(-idade)) idade--;
        return idade;
    }
}
