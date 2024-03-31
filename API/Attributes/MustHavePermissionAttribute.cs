using Common.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace API.Attributes
{
	public class MustHavePermissionAttribute : AuthorizeAttribute
	{
        public MustHavePermissionAttribute(string feature, string action)
            => Policy = AppPermission.NameFor(feature, action);
    }
}
