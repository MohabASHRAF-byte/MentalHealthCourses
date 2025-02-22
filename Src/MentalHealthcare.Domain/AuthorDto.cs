namespace MentalHealthcare.Domain;

public class AuthorDto
{
      public int AuthorId { get; set; }
        public string Name { get; set; } = default!;
        public string? About { get; set; }
        public string? ImageUrl { get; set; }
        
}