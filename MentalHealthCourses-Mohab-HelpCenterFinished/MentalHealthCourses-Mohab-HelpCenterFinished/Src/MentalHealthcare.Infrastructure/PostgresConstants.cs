namespace MentalHealthcare.Infrastructure;

public class PostgresConstants
{
    public static string ForeignKeyViolation = "23503";
    public static string CourseInstructorFkConstrain = "FK_Courses_Instructors_InstructorId";
}