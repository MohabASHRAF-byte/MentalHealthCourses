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
    public User user { get; set; }
}