using System.ComponentModel.DataAnnotations.Schema;

namespace MentalHealthcare.Domain.Entities;

// Written By Marcelino , Reviewed by Mohab
// Reviewed
/* Review
 * ==========
   - nammmmming
 */
public class Logs
{
    public int Id { get; set; }
    public string Description { get; set; }
    public int UserId { get; set; }
    [ForeignKey(nameof(User))]
    public User user { get; set; }
}