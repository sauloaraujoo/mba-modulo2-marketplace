using LojaVirtual.Api.Models;
using LojaVirtual.Core.Business.Entities;
using LojaVirtual.Core.Business.Extensions.IdentityUser;
using LojaVirtual.Core.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace LojaVirtual.Api.Controllers
{
    [AllowAnonymous]
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly IVendedorRepository _vendedorRepository;
        public AuthController(SignInManager<IdentityUser> signInManager,
                              UserManager<IdentityUser> userManager,
                              IOptions<JwtSettings> jwtSettings,
                              IVendedorRepository vendedorRepository,
                              INotifiable notifiable) : base(notifiable)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
            _vendedorRepository = vendedorRepository;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginUserViewModel loginUser)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);

            if (result.Succeeded)
            {
                return CustomResponse(HttpStatusCode.OK, await GerarJwt(loginUser.Email));
            }
            AdicionarErroProcessamento("E-mail ou senha inválidos.");
            return CustomResponse();
        }

        [HttpPost("register")]
        public async Task<ActionResult> Registrar(RegisterUserModel registerUser, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }
            var idUser = Guid.NewGuid();
            var userIden = await _userManager.FindByIdAsync(registerUser.Email);
            if (await _userManager.FindByEmailAsync(registerUser.Email) != null)
            {
                AdicionarErroProcessamento("E-mail já cadastrado.");
                return CustomResponse();
            }

            var user = new IdentityUser
            {
                Id = idUser.ToString(),
                UserName = registerUser.Email,
                Email = registerUser.Email,
                EmailConfirmed = true,
                NormalizedEmail = registerUser.Email.ToUpper(),
                AccessFailedCount = 0,
                NormalizedUserName = registerUser.Email.ToUpper(),
            };

            var result = await _userManager.CreateAsync(user, registerUser.Password);
            if (result.Succeeded)
            {
                var vendedor = new Vendedor(idUser, registerUser.Nome, registerUser.Email);
                
                await _vendedorRepository.Insert(vendedor, cancellationToken);
                await _vendedorRepository.SaveChanges(cancellationToken);
                await _signInManager.SignInAsync(user, false);
                return CustomResponse(HttpStatusCode.OK, await GerarJwt(user.Email));
            }
            AdicionarErroProcessamento("Falha no registro do usuário.");
            return CustomResponse();
        }
        private async Task<string> GerarJwt(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) return string.Empty;            

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id),
            };            

            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret!);

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var encodedToken = tokenHandler.WriteToken(token);

            return encodedToken;
        }
    }
}
