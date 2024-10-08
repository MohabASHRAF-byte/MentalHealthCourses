using MentalHealthcare.Domain.Constants;

namespace MentalHealthcare.Application.Common;

public static class Roles
{
    public static bool HasRole(long userRoles, UserRoles role)
    {
        return (userRoles & (1L << (int)role)) != 0;
    }

    public static bool HasAllRoles(long userRoles, List<UserRoles> roles)
    {
        return roles.All(role => HasRole(userRoles, role));
    }

    public static bool HasAnyRole(long userRoles, List<UserRoles> roles)
    {
        return roles.Any(role => HasRole(userRoles, role));
    }

    public static long AddRole(long userRoles, UserRoles role)
    {
        userRoles |= (1L << (int)role);
        return userRoles;
    }

    public static long AddRoles( long userRoles, List<UserRoles> roles)
    {
        foreach (var role in roles)
        {
            userRoles = AddRole(userRoles, role);
        }
        return userRoles;
    }

    public static void RemoveRole(ref long userRoles, UserRoles role)
    {
        userRoles &= ~(1L << (int)role);
    }

    public static void RemoveRoles(ref long userRoles, List<UserRoles> roles)
    {
        foreach (var role in roles)
        {
            RemoveRole(ref userRoles, role);
        }
    }
}