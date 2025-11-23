using WellMindApi.Application.DTOs.Usuarios;

namespace WellMindApi.Application.Interfaces.Services;

/// <summary>
/// Interface para serviço de autenticação
/// </summary>
public interface IAuthService
{
    Task<LoginResponseDto?> AutenticarAsync(LoginDto dto);
    Task<string> GerarTokenAsync(UsuarioDto usuario);
    Task<bool> ValidarTokenAsync(string token);
}
