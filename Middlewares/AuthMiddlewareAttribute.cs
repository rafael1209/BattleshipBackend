﻿using System.Security.Claims;
using System.Security.Principal;
using BattleshipBackend.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BattleshipBackend.Middlewares;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthMiddlewareAttribute : Attribute, IFilterFactory
{
    public bool IsReusable => false;

    public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
    {
        var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
        var dbContext = serviceProvider.GetRequiredService<IUserRepository>();
        return new AuthMiddlewareFilter(httpContextAccessor, dbContext);
    }

    private class AuthMiddlewareFilter(IHttpContextAccessor httpContextAccessor, IUserRepository dbContext)
        : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                var request = httpContextAccessor.HttpContext!.Request;
                if (await IsUserAuthorizedByTokenFromHeaderAsync(request))
                {
                    var user = await dbContext.TryGetUserByAuthToken(request.Headers["Authorization"]);
                    httpContextAccessor.HttpContext.Items["@me"] = user;
                    request.HttpContext.User.AddIdentity(new ClaimsIdentity(new GenericIdentity(user?.Id.ToString())));

                    await next();
                }
                else
                {
                    context.Result = new UnauthorizedResult();
                }
            }
            catch (Exception)
            {
                context.Result = new UnauthorizedResult();
            }
        }

        private async Task<bool> IsUserAuthorizedByTokenFromHeaderAsync(HttpRequest request)
        {
            request.Headers.TryGetValue("Authorization", out var token);
            var apiKey = token.FirstOrDefault();

            if (string.IsNullOrEmpty(apiKey)) return false;

            var user = await dbContext.TryGetUserByAuthToken(apiKey);

            return user != null;
        }
    }
}