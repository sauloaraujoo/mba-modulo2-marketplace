namespace LojaVirtual.Core.Business.Extensions.IdentityUser
{
    public class JwtSettings
    {
        public string Secret { get; set; }

        public int ExpiracaoHoras { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }
    }
}
