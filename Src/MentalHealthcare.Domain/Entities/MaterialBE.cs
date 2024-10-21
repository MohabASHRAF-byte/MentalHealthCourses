namespace MentalHealthcare.Domain.Entities;

// Written By Marcelino , Reviewed by Mohab
// Reviewed
/*
  Review
  ========
   - upload by admin not string
   - remove admins
   - remove url to avoid nulls
   - remove unmapped property
 */
public class MaterialBE
{
    public string Title { get; set; } = default!;

    public int UploadedById { get; set; } // Foreign Key property
    public Admin UploadedBy { get; set; } = default!;

    public DateTime CreatedDate { get; set; }
}
