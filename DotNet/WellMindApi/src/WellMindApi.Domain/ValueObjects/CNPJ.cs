using WellMindApi.Domain.Entities;

namespace WellMindApi.Domain.ValueObjects;
public sealed class CNPJ : IEquatable<CNPJ>
{
    public string Numero { get; }

    private CNPJ(string numero)
    {
        Numero = numero;
    }

    /// <summary>
    /// Cria um novo objeto CNPJ com validação
    /// </summary>
    public static CNPJ Criar(string cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj))
            throw new DomainException("CNPJ não pode ser vazio");

        var cnpjLimpo = LimparCNPJ(cnpj);

        if (!Validar(cnpjLimpo))
            throw new DomainException($"CNPJ inválido: {cnpj}");

        return new CNPJ(cnpjLimpo);
    }

    /// <summary>
    /// Remove caracteres não numéricos
    /// </summary>
    private static string LimparCNPJ(string cnpj)
    {
        return new string(cnpj.Where(char.IsDigit).ToArray());
    }

    /// <summary>
    /// Valida CNPJ usando algoritmo oficial
    /// </summary>
    private static bool Validar(string cnpj)
    {
        // CNPJ deve ter exatamente 14 dígitos
        if (cnpj.Length != 14)
            return false;

        // Rejeitar CNPJs com todos os dígitos iguais
        if (cnpj.Distinct().Count() == 1)
            return false;

        // Validar primeiro dígito verificador
        int[] multiplicador1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int soma = 0;

        for (int i = 0; i < 12; i++)
            soma += int.Parse(cnpj[i].ToString()) * multiplicador1[i];

        int resto = soma % 11;
        int digito1 = resto < 2 ? 0 : 11 - resto;

        if (int.Parse(cnpj[12].ToString()) != digito1)
            return false;

        // Validar segundo dígito verificador
        int[] multiplicador2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        soma = 0;

        for (int i = 0; i < 13; i++)
            soma += int.Parse(cnpj[i].ToString()) * multiplicador2[i];

        resto = soma % 11;
        int digito2 = resto < 2 ? 0 : 11 - resto;

        return int.Parse(cnpj[13].ToString()) == digito2;
    }

    /// <summary>
    /// Formata CNPJ com máscara (XX.XXX.XXX/XXXX-XX)
    /// </summary>
    public string Formatado()
    {
        return $"{Numero.Substring(0, 2)}.{Numero.Substring(2, 3)}.{Numero.Substring(5, 3)}/{Numero.Substring(8, 4)}-{Numero.Substring(12, 2)}";
    }

    /// <summary>
    /// Obtém apenas a raiz do CNPJ (8 primeiros dígitos)
    /// Identifica o estabelecimento principal
    /// </summary>
    public string ObterRaiz()
    {
        return Numero.Substring(0, 8);
    }

    /// <summary>
    /// Obtém o número da filial (4 dígitos após a raiz)
    /// 0001 = Matriz, demais = Filiais
    /// </summary>
    public string ObterFilial()
    {
        return Numero.Substring(8, 4);
    }

    /// <summary>
    /// Verifica se é a matriz (0001)
    /// </summary>
    public bool EhMatriz()
    {
        return ObterFilial() == "0001";
    }

    /// <summary>
    /// Obtém os dígitos verificadores
    /// </summary>
    public string ObterDigitosVerificadores()
    {
        return Numero.Substring(12, 2);
    }

    // ========================================================================
    // CONVERSÕES IMPLÍCITAS
    // ========================================================================

    public static implicit operator string(CNPJ cnpj) => cnpj.Numero;

    public static explicit operator CNPJ(string cnpj) => Criar(cnpj);

    // ========================================================================
    // IGUALDADE E COMPARAÇÃO
    // ========================================================================

    public bool Equals(CNPJ? other)
    {
        if (other is null) return false;
        return Numero == other.Numero;
    }

    public override bool Equals(object? obj)
    {
        return obj is CNPJ cnpj && Equals(cnpj);
    }

    public override int GetHashCode()
    {
        return Numero.GetHashCode();
    }

    public override string ToString() => Formatado();

    public static bool operator ==(CNPJ? left, CNPJ? right)
    {
        if (left is null && right is null) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    public static bool operator !=(CNPJ? left, CNPJ? right)
    {
        return !(left == right);
    }
}
