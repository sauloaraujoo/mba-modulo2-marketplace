using LojaVirtual.Api.Controllers;
using LojaVirtual.Api.Models;
using LojaVirtual.Business.Entities;
using LojaVirtual.Business.Extensions.IdentityUser;
using LojaVirtual.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace LojaVirtual.Api.V1.Controllers
{
    [AllowAnonymous]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/auth")]
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly IClienteRepository _clienteRepository;
        public AuthController(SignInManager<IdentityUser> signInManager,
                              UserManager<IdentityUser> userManager,
                              IOptions<JwtSettings> jwtSettings,
                              IClienteRepository clienteRepository,
                              INotificavel notificavel) : base(notificavel)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
            _clienteRepository = clienteRepository;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginUserViewModel usuarioLogin)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var resultado = await _signInManager.PasswordSignInAsync(usuarioLogin.Email, usuarioLogin.Senha, false, true);

            if (resultado.Succeeded)
            {
                return CustomResponse(HttpStatusCode.OK, await GerarJwt(usuarioLogin.Email));
            }
            AdicionarErroProcessamento("E-mail ou senha inválidos.");
            return CustomResponse();
        }

        [HttpPost("registrar")]
        public async Task<ActionResult> Registrar(RegisterUserModel usuarioRegistro, CancellationToken tokenDeCancelamento)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }
            var idUsuarioNovo = Guid.NewGuid();
            var usuarioPreexistente = await _userManager.FindByIdAsync(usuarioRegistro.Email);
            if (await _userManager.FindByEmailAsync(usuarioRegistro.Email) != null)
            {
                AdicionarErroProcessamento("E-mail já cadastrado.");
                return CustomResponse();
            }

            var usuarioNovo = new IdentityUser
            {
                Id = idUsuarioNovo.ToString(),
                UserName = usuarioRegistro.Email,
                Email = usuarioRegistro.Email,
                EmailConfirmed = true,
                NormalizedEmail = usuarioRegistro.Email.ToUpper(),
                AccessFailedCount = 0,
                NormalizedUserName = usuarioRegistro.Email.ToUpper(),
            };

            var resultado = await _userManager.CreateAsync(usuarioNovo, usuarioRegistro.Senha);
            if (resultado.Succeeded)
            {
                var cliente = new Cliente(idUsuarioNovo, usuarioRegistro.Nome, usuarioRegistro.Email);

                await _clienteRepository.Inserir(cliente, tokenDeCancelamento);
                await _clienteRepository.SalvarMudancas(tokenDeCancelamento);
                await _signInManager.SignInAsync(usuarioNovo, false);
                return CustomResponse(HttpStatusCode.OK, await GerarJwt(usuarioNovo.Email));
            }
            foreach (var item in resultado.Errors)
            {
                AdicionarErroProcessamento(item.Description);
            }
            
            return CustomResponse();
        }
        private async Task<LoginResponseViewModel> GerarJwt(string email)
        {
            var usuario = await _userManager.FindByEmailAsync(email);
            var declaracoes = await _userManager.GetClaimsAsync(usuario);
            var papeisDoUsuario = await _userManager.GetRolesAsync(usuario);

            declaracoes.Add(new Claim(JwtRegisteredClaimNames.Sub, usuario.Id));
            declaracoes.Add(new Claim(JwtRegisteredClaimNames.Email, usuario.Email));
            declaracoes.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            declaracoes.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            declaracoes.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));
            foreach (var papelDoUsuario in papeisDoUsuario)
            {
                declaracoes.Add(new Claim("role", papelDoUsuario));
            }

            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret!);

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(declaracoes),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var encodedToken = tokenHandler.WriteToken(token);

            var response = new LoginResponseViewModel
            {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(_jwtSettings.ExpiracaoHoras).TotalSeconds,
                UserToken = new UserTokenViewModel
                {
                    Id = usuario.Id,
                    Email = usuario.Email,
                    Claims = declaracoes.Select(c => new ClaimViewModel { Type = c.Type, Value = c.Value })
                }
            };

            return response;
        }
        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}
