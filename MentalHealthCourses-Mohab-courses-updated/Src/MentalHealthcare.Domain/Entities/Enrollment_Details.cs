using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MentalHealthcare.Domain.Entities;

// Written By Marcelino , Reviewed by Mohab
// Reviewed
/* Review
 * ==========
   - Convert progress to string (i will explain the implementation  details later)
   - Naming conventions
   - add max length to the progress
 */
public class EnrollmentDetails
{
    public int EnrollmentId { get; set; }
    public DateTime Date { get; set; }
    public string Progress { get; set; } = default!;
    //
    public int SystemUserId { get; set; }
    public SystemUser SystemUser { get; set; } = default!;
    public int CourseId { get; set; }
    public Course Course { get; set; } = default!;
}