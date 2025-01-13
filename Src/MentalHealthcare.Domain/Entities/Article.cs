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
    public List<ArticleImageUrl> ArticleImageUrls { get; set; } = [];

    public int AuthorId { get; set; } // Foreign Key property
    public Author Author { get; set; } = default!;
    
    public string Content { get; set; } = default!;
    
    public int LastUploadImgCnt { get; set; } = 0;
}
