using DevMarketAPI.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;

namespace DevMarketAPI.Authorization.Handlers
{
    public class StudioAccessHandler : AuthorizationHandler<StudioAccessRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StudioAccessHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, StudioAccessRequirement requirement)
        {
            var studioIdClaim = context.User.FindFirst("studioId")?.Value;
            if (studioIdClaim == null)
                return Task.CompletedTask;

            // Get studioId from route
            var routeData = _httpContextAccessor.HttpContext?.GetRouteData();
            var studioIdFromRoute = routeData?.Values["id"]?.ToString();

            if (studioIdFromRoute == null)
                return Task.CompletedTask;

            if (studioIdFromRoute == studioIdClaim)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

}
