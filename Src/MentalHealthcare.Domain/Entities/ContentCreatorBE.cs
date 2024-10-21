using MentalHealthcare.Domain.Constants;
using System.ComponentModel.DataAnnotations;

// Written By Marcelino , Reviewed by Mohab
// Reviewed
/* Review
 * ==========
 * - make url nullable string
 * - add max length
 * - about is nullable
 */
namespace MentalHealthcare.Domain.Entities;

public class ContentCreatorBe
{
    [MaxLength(Global.MaxNameLength)]
    public string Name { get; set; } = default!;
    [MaxLength(Global.UrlMaxLength)]
    public string? ImageUrl { get; set; }
    public string? About { get; set; }

    public Admin AddedBy { get; set; } = default!;
}