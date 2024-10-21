using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MentalHealthcare.Domain.Entities;

public class User : IdentityUser
{
    public long Roles { get; set; }
    [MaxLength(20)]
    public string Tenant { get; set; } = default!;
}