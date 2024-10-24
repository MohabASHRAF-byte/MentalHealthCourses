using System.ComponentModel.DataAnnotations;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Domain.Dtos;

public class CategoryDto
{
    
    public int CategoryId { get; set; }
    [Required]
    [MaxLength(Global.MaxNameLength)]
    public string Name { get; set; } = default!;
    //todo add max length
    public string? Description { get; set; }
}