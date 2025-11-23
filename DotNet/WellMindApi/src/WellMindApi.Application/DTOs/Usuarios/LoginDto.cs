namespace WellMindApi.Application.DTOs.Usuarios;

/// <summary>
/// DTO para autenticação
/// </summary>
public record LoginDto(
    string Email,
    string Senha
);

/// <summary>
/// DTO de resposta de autenticação
/// </summary>
public record LoginResponseDto(
    string Token,
    DateTime ExpiresAt,
    UsuarioDto Usuario
);
