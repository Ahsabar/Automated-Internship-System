using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace api.Authorization.Handlers
{
    public class UserTypeHandler : AuthorizationHandler<UserTypeRequirement>
	{
		protected override Task HandleRequirementAsync(
			AuthorizationHandlerContext context,
			UserTypeRequirement requirement)
		{
			var userTypeClaim = context.User.FindFirst(claim => 
				claim.Type == "UserType");

			if (userTypeClaim != null && 
				userTypeClaim.Value == requirement.RequiredUserType)
			{
				context.Succeed(requirement);
			}

			return Task.CompletedTask;
		}
	}
}