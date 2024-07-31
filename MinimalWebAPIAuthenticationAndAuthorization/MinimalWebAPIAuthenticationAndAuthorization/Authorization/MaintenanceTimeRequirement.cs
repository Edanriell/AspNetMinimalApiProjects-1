using Microsoft.AspNetCore.Authorization;

namespace MinimalWebAPIAuthenticationAndAuthorization.Authorization;

public class MaintenanceTimeRequirement : IAuthorizationRequirement
{
    public TimeOnly StartTime { get; init; }

    public TimeOnly EndTime { get; init; }
}
