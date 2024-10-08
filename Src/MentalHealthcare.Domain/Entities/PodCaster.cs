namespace MentalHealthcare.Domain.Entities;
// Written By Marcelino , Reviewed by Mohab
// Reviewed
/* Review
 * ==========
    - make podcasts Nullable
 */
public class PodCaster : ContentCreatorBe
{
    public int PodCasterId { get; set; }

    public List<Podcast>? Podcasts { get; set; }
}