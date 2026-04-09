using Microsoft.AspNetCore.Http;
using System;

namespace MES_MVC
{
    public class CookieHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CookieHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetCookie(string key, string value, int expireDays = 1)
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null) return;

            var options = new CookieOptions
            {
                HttpOnly = false, // Cho phép JavaScript truy cập
                Expires = DateTimeOffset.UtcNow.AddDays(expireDays),
                IsEssential = true,
                SameSite = SameSiteMode.Lax
            };
            context.Response.Cookies.Append(key, value ?? string.Empty, options);
        }

        public string? GetCookie(string key)
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null) return null;

            if (context.Request.Cookies.TryGetValue(key, out var value))
            {
                return value;
            }
            return null;
        }

        public void DeleteCookie(string key)
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null) return;

            context.Response.Cookies.Delete(key);
        }
    }
}
