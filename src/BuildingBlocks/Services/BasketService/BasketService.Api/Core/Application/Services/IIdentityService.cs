﻿using System.Security.Claims;

namespace BasketService.Api.Core.Application.Services
{
    public interface IIdentityService
    {
        string GetUserName();
    }

    public class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public IdentityService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string GetUserName()
        {
            return _contextAccessor.HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
        }
    }
}
