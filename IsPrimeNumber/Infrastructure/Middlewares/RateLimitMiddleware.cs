using IsPrimeNumber.Infrastructure.Persistence;
using IsPrimeNumber.Infrastructure.Persistence.Entities;
using IsPrimeNumber.Models;
using Microsoft.EntityFrameworkCore;

namespace IsPrimeNumber.Infrastructure.Middlewares;

public class RateLimitMiddleware
{
    private readonly RequestDelegate _next;

    public RateLimitMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext, ApplicationDbContext applicationDbContext)
    {
        var ipAddressProvided = httpContext.Request.Query.TryGetValue("IPAddress", out var ipAddress);
        var tokenProvided = httpContext.Request.Query.TryGetValue("Token", out var token);
        var dateTimeNow = DateTime.UtcNow;
        var blockedTill = dateTimeNow.AddHours(DefaultValues.BlockPeriodInHours);

        if (!ipAddressProvided || !tokenProvided)
        {
            await httpContext.Response.WriteAsJsonAsync(new IsPrimeNumberResponse
            {
                HasError = true,
                ErrorMessage = $"{nameof(IsPrimeNumberModel.IPAddress)} and {nameof(IsPrimeNumberModel.Token)} are required!"
            });
        }

        var user = await applicationDbContext.Users.FirstOrDefaultAsync(u => u.Token == token[0]);

        if (user == null)
        {
            user = new User
            {
                IPAddress = ipAddress[0]!,
                Token = token[0]!,
                Attemps = 1,
                IsBlocked = false,
                SessionStartTimeOn = dateTimeNow
            };

            await applicationDbContext.AddAsync(user);
            await applicationDbContext.SaveChangesAsync();

            await _next.Invoke(httpContext);
        }

        var remaninngTime = dateTimeNow.Subtract(user.BlockedTill ?? blockedTill).Minutes;

        if (user.IsBlocked && remaninngTime == 0) 
        { 
            user.IsBlocked = false;
            user.SessionStartTimeOn = dateTimeNow;
            user.Attemps = 1;

            applicationDbContext.Update(user);
            await applicationDbContext.SaveChangesAsync();

            await _next.Invoke(httpContext);
        }

        var errorBlockedResponse = new IsPrimeNumberResponse
        {
            HasError = true,
            ErrorMessage = $"You have exceeded your call limits. Remining time until restriction removed is {remaninngTime} minutes."
        };

        if (user.IsBlocked && remaninngTime > 0)
        {
            await httpContext.Response.WriteAsJsonAsync(errorBlockedResponse);
        }

        if (user.Attemps >= DefaultValues.CountOfAttempts)
        {
            user.IsBlocked = true;
            user.BlockedTill = dateTimeNow.AddHours(DefaultValues.BlockPeriodInHours);
            applicationDbContext.Update(user);
            await applicationDbContext.SaveChangesAsync();

            await httpContext.Response.WriteAsJsonAsync(errorBlockedResponse);
        }

        if (!user.IsBlocked)
        { 
            user.Attemps += 1;

            applicationDbContext.Update(user);
            await applicationDbContext.SaveChangesAsync();

            await _next.Invoke(httpContext);
        }
    }
}