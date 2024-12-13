using System.ComponentModel.DataAnnotations;
using MentalHealthcare.Domain.Constants;

namespace MentalHealthcare.Domain.Entities;
// Written By Marcelino , Reviewed by Mohab
// Reviewed
/* Review
  ==========
   - add string content
   - img url
   - add title
 */

public class Article : MaterialBe
{
    public int ArticleId { get; set; }

    public int AuthorId { get; set; }    // Foreign Key
    public Author Author { get; set; } = default!;
    
    public string Content { get; set; } = default!;
    
    [MaxLength(Global.UrlMaxLength)] 
    public string PhotoUrl { get; set; } = default!;
}
