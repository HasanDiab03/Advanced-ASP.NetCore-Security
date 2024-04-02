using Application.Services.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.Identity
{
	public class CurrentUserRepository : ICurrentUserRepository
	{
		public string Id { get; }

        public CurrentUserRepository(IHttpContextAccessor httpContextAccessor)
        {
            Id = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);    
        }
    }
}
