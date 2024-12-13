namespace MentalHealthcare.Domain.Entities;

// Written By Marcelino , Reviewed by Mohab
// Reviewed No Edits
public class Author : ContentCreatorBe
{
    public int AuthorId { get; set; }    // Primary Key

    public List<Article> Articles { get; set; } = new();
}