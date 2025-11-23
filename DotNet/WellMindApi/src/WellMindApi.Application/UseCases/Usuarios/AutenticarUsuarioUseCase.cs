using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WellMindApi.Application.DTOs.Usuarios;
using WellMindApi.Domain.Entities;
using WellMindApi.Domain.Interfaces.Repositories;

namespace WellMindApi.Application.UseCases.Usuarios;

/// <summary>
/// Caso de Uso: Autenticar usuário e gerar JWT
/// </summary>
public class AutenticarUsuarioUseCase
{
    private readonly IUsuarioRepository _repository;
    private readonly IConfiguration _configuration;

    public AutenticarUsuarioUseCase(
        IUsuarioRepository repository,
        IConfiguration configuration)
    {
        _repository = repository;
        _configuration = configuration;
    }

    public async Task<LoginResponseDto?> ExecutarAsync(LoginDto dto)
    {
        // Buscar usuário por email
        var usuario = await _repository.ObterPorEmailAsync(dto.Email);

        if (usuario == null || !usuario.Ativo)
            return null;

        // Validar senha
        if (!usuario.ValidarSenha(dto.Senha))
            return null;

        // Gerar token JWT
        var token = GerarToken(usuario);
        var expiresAt = DateTime.Now.AddHours(1);

        return new LoginResponseDto(
            token,
            expiresAt,
            MapearParaDto(usuario)
        );
    }

    private string GerarToken(Usuario usuario)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey não configurada");
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.IdUsuario.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
            new Claim(JwtRegisteredClaimNames.Name, usuario.Nome),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static UsuarioDto MapearParaDto(Usuario usuario)
    {
        return new UsuarioDto
        {
            IdUsuario = usuario.IdUsuario,
            Nome = usuario.Nome,
            Email = usuario.Email,
            DataNascimento = usuario.DataNascimento,
            Idade = usuario.ObterIdade(),
            Genero = usuario.Genero,
            Telefone = usuario.Telefone,
            DataCadastro = usuario.DataCadastro,
            Ativo = usuario.Ativo
        };
    }
}
