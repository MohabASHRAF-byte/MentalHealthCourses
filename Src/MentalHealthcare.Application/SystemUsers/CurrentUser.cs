using MentalHealthcare.Domain.Constants;

namespace MentalHealthcare.Application.SystemUsers;
/// <summary>
   /// Roles Explanation (Bitset Approach 😀)
   /// </summary>
   /// <remarks>
   /// <para><b>Overview:</b></para>
   /// <list type="bullet">
   /// <item><description>Define the roles in an enum called <c>UserRoles</c>.</description></item>
   /// <item><description>Each role represents a bit in a number, creating a bitmask for roles.</description></item>
   /// <item><description>Supports a maximum of 63 roles using a <c>long</c> (64-bit) number.</description></item>
   /// </list>
   ///
   /// <para><b>Pros:</b></para>
   /// <list type="bullet">
   /// <item><description>Efficient storage: Only one number is stored in the database to represent all the user's roles.</description></item>
   /// <item><description>Faster lookups and support for database scalability.</description></item>
   /// </list>
   ///
   /// <para><b>Cons:</b></para>
   /// <list type="bullet">
   /// <item><description>Implementation is more complex (using abstracted methods simplifies this).</description></item>
   /// <item><description>Limited to 63 roles with a <c>long</c> type (can be extended using <c>BigInteger</c>).</description></item>
   /// </list>
   ///
   /// <para><i>Note:</i> Bitwise operations may seem complex at first, but once set up, they are very efficient!</para>
   /// </remarks>
   

public record CurrentUser(
    string Id,
    string UserName,
    long Roles,
    string Tenant,
    int? SysUserId ,
    int? AdminId
    )
{
    // return true if the user have the Role you passed
    public bool HasRole(UserRoles role)
    {
        var roleVal = (int)role;
        return ((1L << roleVal) & Roles) != 0;
    }
    // return true if the user have all the passed roles 
    public bool HasRole(List<UserRoles> roles)
    {
        return roles
            .Select(role => (int)role)
            .All(roleVal => ((1L << roleVal) & Roles) != 0);
    }
    //return true if the user have any of the passed roles 
    public bool IsAuthorized(List<UserRoles> roles)
    {
        var requiredRoles = roles
            .Aggregate(0L, (current, role) =>
                current | (1L << (int)role));
        return (Roles & requiredRoles) != 0;
    }
}