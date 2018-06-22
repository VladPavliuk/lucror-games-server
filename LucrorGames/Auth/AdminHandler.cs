using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace mvc_auth.Auth
{
    public class AdminHandler : AuthorizationHandler<AdminRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminRequirement requirement)
        {
            if(context.User.IsInRole("Admin")) {
                context.Succeed(requirement);
            } else {
                context.Fail();
            }
            
            return Task.CompletedTask;
        }
    }
}