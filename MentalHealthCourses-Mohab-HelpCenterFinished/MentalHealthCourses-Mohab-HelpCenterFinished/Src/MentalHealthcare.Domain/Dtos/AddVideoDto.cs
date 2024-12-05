namespace MentalHealthcare.Domain.Dtos;

public class AddVideoDto
{
    public int CourseId { get; set; }
    public string? Description { set; get; }
    public string Title { set; get; }=default!;
}