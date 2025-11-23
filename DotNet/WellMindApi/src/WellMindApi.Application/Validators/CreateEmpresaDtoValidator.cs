using FluentValidation;
using WellMindApi.Application.DTOs.Empresas;

namespace WellMindApi.Application.Validators;

/// <summary>
/// Validador para CreateEmpresaDto
/// </summary>
public class CreateEmpresaDtoValidator : AbstractValidator<CreateEmpresaDto>
{
    public CreateEmpresaDtoValidator()
    {
        RuleFor(x => x.NomeEmpresa)
            .NotEmpty().WithMessage("Nome da empresa é obrigatório")
            .MaximumLength(200).WithMessage("Nome da empresa deve ter no máximo 200 caracteres");

        RuleFor(x => x.CNPJ)
            .NotEmpty().WithMessage("CNPJ é obrigatório")
            .Must(BeValidCNPJ).WithMessage("CNPJ inválido");

        RuleFor(x => x.EmailContato)
            .EmailAddress().WithMessage("Email de contato inválido")
            .When(x => !string.IsNullOrEmpty(x.EmailContato));

        RuleFor(x => x.Endereco)
            .MaximumLength(300).WithMessage("Endereço deve ter no máximo 300 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Endereco));

        RuleFor(x => x.Telefone)
            .MaximumLength(20).WithMessage("Telefone deve ter no máximo 20 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Telefone));
    }

    private bool BeValidCNPJ(string cnpj)
    {
        var cnpjLimpo = new string(cnpj.Where(char.IsDigit).ToArray());

        if (cnpjLimpo.Length != 14)
            return false;

        if (cnpjLimpo.Distinct().Count() == 1)
            return false;

        // Validar primeiro dígito
        int[] multiplicador1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int soma = 0;

        for (int i = 0; i < 12; i++)
            soma += int.Parse(cnpjLimpo[i].ToString()) * multiplicador1[i];

        int resto = soma % 11;
        int digito1 = resto < 2 ? 0 : 11 - resto;

        if (int.Parse(cnpjLimpo[12].ToString()) != digito1)
            return false;

        // Validar segundo dígito
        int[] multiplicador2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        soma = 0;

        for (int i = 0; i < 13; i++)
            soma += int.Parse(cnpjLimpo[i].ToString()) * multiplicador2[i];

        resto = soma % 11;
        int digito2 = resto < 2 ? 0 : 11 - resto;

        return int.Parse(cnpjLimpo[13].ToString()) == digito2;
    }
}
