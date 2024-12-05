// Written By Marcelino , Reviewed by Mohab
// Reviewed No Edits

namespace MentalHealthcare.Domain.Entities;

public class Meditation : MaterialBe
{
    public int MeditationId { get; set; }

    //todo implement it with url to avoid heavy db searches
    public string Content { get; set; } = default!;
}