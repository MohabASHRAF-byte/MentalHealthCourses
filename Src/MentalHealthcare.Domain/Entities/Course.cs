namespace MentalHealthcare.Domain.Entities;

public class Course
{
    public int CourseId { set; get; }
    public string Name { set; get; }=default!;
    public string Title { get; set; }
    public byte[]? Thumbnail { get; set; }
    public decimal Price { get; set; }
    public int? Ratings { get; set; }
    public string description { get; set; }

    public List<CourseMateriel> CourseMateriels { set; get; } = new();

    public Instructor instructors { get; set; }

    //public List<Courses_Category> courses_Categories { get; set; } = new();

    public List<Pdf>? pdfs { set; get; } = new();

    public List<Video>? Videos { get; set; }



    public ICollection<Category>? categories { get; set; }
    = new HashSet<Category>();


    public ICollection<User> Users_Fav_course { get; set; }
= new HashSet<User>();


    public ICollection<User> Users_Rates { get; set; }
= new HashSet<User>();


}