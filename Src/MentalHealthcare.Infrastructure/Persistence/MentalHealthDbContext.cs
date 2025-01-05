using MentalHealthcare.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MentalHealthcare.Domain.Entities.Courses;
using MentalHealthcare.Domain.Entities.OrderProcessing;
using MentalHealthcare.Infrastructure.Configurations;

namespace MentalHealthcare.Infrastructure.Persistence;

public class MentalHealthDbContext : IdentityDbContext<User>
{
    public MentalHealthDbContext(DbContextOptions<MentalHealthDbContext> options)
        : base(options)
    {
    }

    #region DbSets of Tables

    public DbSet<Course> Courses { get; set; }
    public DbSet<CourseLessonResource> CourseLessonResources { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<SystemUser> SystemUsers { get; set; }
    public DbSet<Logs> Logs { get; set; }
    public DbSet<Article> Articles { get; set; }
    public DbSet<Meditation> Meditations { get; set; }
    public DbSet<Podcast> Podcasts { get; set; }
    public DbSet<Instructor> Instructors { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<PodCaster> PodCasters { get; set; }
    public DbSet<Payments> Payments { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<EnrollmentDetails> EnrollmentDetails { get; set; }
    public DbSet<SystemUserTokenCode> SystemUserTokenCodes { get; set; }
    public DbSet<PendingAdmins> PendingAdmins { get; set; }
    public DbSet<PendingVideoUpload> VideoUploads { get; set; }
    public DbSet<HelpCenterItem> HelpCenterItems { get; set; }

    public DbSet<Advertisement> Advertisements { get; set; }

    public DbSet<AdvertisementImageUrl> AdvertisementImageUrls { get; set; }
    public DbSet<ContactUsForm> ContactUses { get; set; }

    public DbSet<CourseSection> CourseSections { get; set; }
    public DbSet<CourseLesson> CourseLessons { get; set; }

    public DbSet<CoursePromoCode> CoursePromoCodes { get; set; }

    public DbSet<GeneralPromoCode> GeneralPromoCodes { get; set; }

    public DbSet<CoursesCart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }

    public DbSet<Invoice> Invoices { get; set; }

    public DbSet<CourseProgress> CourseProgresses { get; set; }

    public DbSet<FavouriteCourse> FavouriteCourses { get; set; }

    public DbSet<UserReview> UserReviews { get; set; }

    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ConfigureUserIdentity();
        modelBuilder.ConfigureMeditation();
        modelBuilder.ConfigureArticle();
        modelBuilder.ConfigurePodcast();
        modelBuilder.ConfigureAuthor();
        modelBuilder.ConfigurePodCaster();
        modelBuilder.ConfigureSystemUser();
        modelBuilder.ConfigureCourse();
        modelBuilder.ConfigureCourseLesson();
        modelBuilder.ConfigureAdvertisement();
        modelBuilder.ConfigureAdmin();
        modelBuilder.ConfigureCategory();
        modelBuilder.ConfigureInstructor();
        modelBuilder.ConfigureLogs();
        modelBuilder.ConfigurePayments();
        modelBuilder.ConfigureEnrollmentDetails();
        modelBuilder.CoursePromoCodeConfiguration();
        modelBuilder.ConfigureCart();
        modelBuilder.ConfigureInvoice();
        modelBuilder.ConfigureCourseProgress();
        modelBuilder.ConfigureCourseFavourite();
        modelBuilder.ConfigureCourseReview();
    }
}