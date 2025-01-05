namespace MentalHealthcare.Application.Courses.Lessons.Commands.CreateVideo;

public class CreateVideoCommandResponse
{
    public string AuthorizationSignature { set; get; }=default!;
    public string AuthorizationExpire { set; get; }=default!;
    public string VideoId { set; get; }=default!;
    public string LibraryId { set; get; }=default!;
    public string CollectionId { set; get; }=default!;

}