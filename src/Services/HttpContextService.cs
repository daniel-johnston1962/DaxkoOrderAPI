using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace DaxkoOrderAPI.Services
{
    public class HttpContextService : IHttpContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetLoggedInUserId()
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userId))
                userId = _httpContextAccessor.HttpContext.User.FindFirst("sub")?.Value;

            return userId;
        }

        public string GetLoggedInUserId(string defaultValue)
        {
            var userId = GetLoggedInUserId();

            if (string.IsNullOrWhiteSpace(userId))
                userId = defaultValue;

            return userId;
        }

        public string GetSessionId()
        {
            return _httpContextAccessor.HttpContext.Session.Id;
        }
    }

    public interface IHttpContextService
    {
        string GetLoggedInUserId();
        string GetLoggedInUserId(string defaultValue);
        string GetSessionId();
    }
}
