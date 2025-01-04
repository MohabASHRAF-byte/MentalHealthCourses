using System.ComponentModel.DataAnnotations;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities.Courses;

namespace MentalHealthcare.Domain.Entities;
// Written By Marcelino , Reviewed by Mohab
// Reviewed No Edits
public class Category
{
    public int CategoryId { get; set; }

    [Required]
    [MaxLength(Global.MaxNameLength)]
    public string Name { get; set; } = default!;
    public string? Description { get; set; }


    public ICollection<Course> Courses { get; set; }
        = new HashSet<Course>();
}