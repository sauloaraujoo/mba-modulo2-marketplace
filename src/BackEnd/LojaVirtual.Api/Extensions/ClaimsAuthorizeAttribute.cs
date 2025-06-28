using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LojaVirtual.Api.Extensions
{
    public class ClaimsAuthorizeAttribute : TypeFilterAttribute
    {
        public ClaimsAuthorizeAttribute(string claimName, string claimValue) : base(typeof(RequisitoClaimFilter))
        {
            Arguments = [new Claim(claimName, claimValue)];
        }
    }
}