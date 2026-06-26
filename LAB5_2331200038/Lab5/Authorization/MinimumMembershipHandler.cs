using Microsoft.AspNetCore.Authorization;
using Lab5.Data;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Lab5.Authorization;

public class MinimumMembershipHandler : AuthorizationHandler<MinimumMembershipRequirement>
{
    private readonly AppDbContext _context;

    public MinimumMembershipHandler(AppDbContext context)
    {
        _context = context;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumMembershipRequirement requirement)
    {
        var username = context.User.Identity?.Name;

        var user = _context.Users.FirstOrDefault(u => u.Email == username);

        if (user != null)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}