using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace CMS_NetCore.Helpers
{
    public class AuthorizationHeader
    {
        private readonly RequestDelegate _next;

        public AuthorizationHeader(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var authenticationCookieName = "JWToken";
            var cookie = context.Request.Cookies[authenticationCookieName];
            string cookieValueFromContext = context.Request.Cookies["JWToken"];

            if (cookieValueFromContext != null)
            {

                if (!context.Request.Path.ToString().ToLower().Contains("/account/logout"))
                {
                    if (!string.IsNullOrEmpty(cookie))
                    {
                        //var token = JsonConvert.DeserializeObject<AccessToken>(cookie);
                        if (cookieValueFromContext != null)
                        {
                            var headerValue = "Bearer " + cookieValueFromContext;
                            if (context.Request.Headers.ContainsKey("Authorization"))
                            {
                                context.Request.Headers["Authorization"] = headerValue;
                            }
                            else
                            {
                                context.Request.Headers.Append("Authorization", headerValue);
                            }
                        }
                    }
                    await _next.Invoke(context);
                }
                else
                {
                    // this is a logout request, clear the cookie by making it expire now
                    context.Response.Cookies.Append(authenticationCookieName,
                                                    "",
                                                    new Microsoft.AspNetCore.Http.CookieOptions()
                                                    {
                                                        Path = "/",
                                                        HttpOnly = true,
                                                        Secure = false,
                                                        Expires = DateTime.UtcNow.AddHours(-1)
                                                    });
                    context.Response.Redirect("/");
                    return;
                }
            }
            else
            {
                await _next.Invoke(context);
            }
        }
    }
}