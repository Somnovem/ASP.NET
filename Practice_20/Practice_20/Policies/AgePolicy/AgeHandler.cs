using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Practice_20
{
    public class AgeHandler : AuthorizationHandler<AgeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            AgeRequirement requirement)
        {
            if (context.User.HasClaim(c => c.Type == ClaimTypes.DateOfBirth))
            {
                DateTime birthday = DateTime.Now;
                if (DateTime.TryParse(context.User.FindFirst(c => c.Type == ClaimTypes.DateOfBirth).Value, out birthday))
                {
                    if ((DateTime.Now.Year - birthday.Year) >= requirement.Age)
                    {
                        context.Succeed(requirement);
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}
