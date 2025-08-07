﻿using LojaVirtual.Business.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace LojaVirtual.Business.Extensions.IdentityUser
{
    public class AppIdentityUser : IAppIdentifyUser
    {
        private readonly IHttpContextAccessor _accessor;
        public AppIdentityUser(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string ObterUsuarioId()
        {
            if (!EAutenticado()) return string.Empty;

            var claim = _accessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return claim ?? string.Empty;
        }

        public bool EAutenticado()
        {
            return _accessor.HttpContext?.User.Identity is { IsAuthenticated: true };
        }
    }
}
