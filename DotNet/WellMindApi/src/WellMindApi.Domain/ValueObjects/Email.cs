using WellMindApi.Domain.Entities;

namespace WellMindApi.Domain.ValueObjects;

public sealed class Email : IEquatable<Email>
{
    public string Endereco { get; }

    private Email(string endereco)
    {
        Endereco = endereco;
    }

    /// <summary>
    /// Cria um novo objeto Email com validação
    /// </summary>
    public static Email Criar(string endereco)
    {
        if (string.IsNullOrWhiteSpace(endereco))
            throw new DomainException("Email não pode ser vazio");

        endereco = endereco.Trim().ToLower();

        if (!Validar(endereco))
            throw new DomainException($"Email inválido: {endereco}");

        return new Email(endereco);
    }

    /// <summary>
    /// Valida formato do email
    /// </summary>
    private static bool Validar(string email)
    {
        if (email.Length < 5 || email.Length > 100)
            return false;

        // Deve conter @
        if (!email.Contains('@'))
            return false;

        // Deve ter texto antes e depois do @
        var partes = email.Split('@');
        if (partes.Length != 2)
            return false;

        var usuario = partes[0];
        var dominio = partes[1];

        // Validar parte do usuário
        if (string.IsNullOrWhiteSpace(usuario) || usuario.Length < 1)
            return false;

        // Validar domínio
        if (string.IsNullOrWhiteSpace(dominio) || !dominio.Contains('.'))
            return false;

        // Domínio deve ter pelo menos 4 caracteres (ex: a.co)
        if (dominio.Length < 4)
            return false;

        // Caracteres válidos básicos
        var caracteresInvalidos = new[] { ' ', ',', ';', ':', '<', '>', '[', ']', '(', ')' };
        if (email.Any(c => caracteresInvalidos.Contains(c)))
            return false;

        return true;
    }

    /// <summary>
    /// Verifica se é um email corporativo (não gratuito)
    /// </summary>
    public bool EhEmailCorporativo()
    {
        var dominiosGratuitos = new[]
        {
            "gmail.com", "hotmail.com", "outlook.com", "yahoo.com",
            "live.com", "icloud.com", "aol.com", "protonmail.com"
        };

        var dominio = Endereco.Split('@')[1].ToLower();
        return !dominiosGratuitos.Contains(dominio);
    }

    /// <summary>
    /// Obtém o domínio do email
    /// </summary>
    public string ObterDominio()
    {
        return Endereco.Split('@')[1];
    }

    /// <summary>
    /// Obtém o nome de usuário (parte antes do @)
    /// </summary>
    public string ObterUsuario()
    {
        return Endereco.Split('@')[0];
    }

    // ========================================================================
    // CONVERSÕES IMPLÍCITAS
    // ========================================================================

    public static implicit operator string(Email email) => email.Endereco;

    public static explicit operator Email(string endereco) => Criar(endereco);

    public bool Equals(Email? other)
    {
        if (other is null) return false;
        return Endereco.Equals(other.Endereco, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj)
    {
        return obj is Email email && Equals(email);
    }

    public override int GetHashCode()
    {
        return Endereco.GetHashCode(StringComparison.OrdinalIgnoreCase);
    }

    public override string ToString() => Endereco;

    public static bool operator ==(Email? left, Email? right)
    {
        if (left is null && right is null) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    public static bool operator !=(Email? left, Email? right)
    {
        return !(left == right);
    }
}
