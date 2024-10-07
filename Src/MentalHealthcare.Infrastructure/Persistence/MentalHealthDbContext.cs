using MentalHealthcare.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MentalHealthcare.Infrastructure.Persistence;

public class MentalHealthDbContext(
    DbContextOptions<MentalHealthDbContext> options
) : DbContext(options)
{
    #region DbSets of Tables
    public DbSet<Course> Courses { get; set; }
    public DbSet<CourseMateriel> CourseMateriels { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<User> users { get; set; }
    public DbSet<Logs> logs { get; set; }
    //public DbSet<Courses_Category> courses_Categories { get; set; }
    public DbSet<Article> Articles { get; set; }
    public DbSet<Video> videos { get; set; }
    public DbSet<CourseMateriel> courseMateriels { get; set; }
    public DbSet<Course> courses { get; set; }
    public DbSet<Meditation> meditations { get; set; }
    public DbSet<Pdf> pdfs { get; set; }
    public DbSet<Podcast> podcasts { get; set; }
    public DbSet<Instructor>instructors  { get; set; }
    public DbSet<Author>Authors { get; set; }
    public DbSet<PodCaster> PodCasters { get; set; }
    public DbSet<Payments> payments { get; set; }
    public DbSet<Category> categories { get; set; }
    public DbSet<Enrollment_Details> enrollment_Details { get; set; }
    //public DbSet<User_Favourites> User_Favourites { get; set; } 
    #endregion


protected override void OnModelCreating(ModelBuilder modelBuilder)
    {       // modelBuilder.Entity<Admin>().HasKey(nameof(Admin.Gmail));

        modelBuilder.Entity<Admin>(A =>
        {
            A.HasKey(nameof(Admin.AdminId));
            A.Property(A =>A.AdminId)
              .UseIdentityColumn(1, 1);

             #region Additional_Data
            A.Property(A => A.FName).
              IsRequired(true);
            A.Property(A => A.LName).IsRequired(true);
            A.Property(A => A.Passwprd).IsRequired(true);
            A.Property(A => A.PhoneNumber).IsRequired(true); 
            #endregion

        });

        modelBuilder.Entity<User>(U =>
        {
            U.HasKey(nameof(User.UserId));
            U.Property(U => U.UserId)
            .UseIdentityColumn(1, 1);

            U.HasMany(U => U.Course_Rates)
            .WithMany(c => c.Users_Rates);



            U.HasMany(U => U.Fav_Courses)
            .WithMany(c => c.Users_Fav_course);


            #region Additional_Data


            U.Property(U => U.Gmail).IsRequired(true)
            .HasAnnotation("DataType", DataType.EmailAddress);

            U.Property(U => U.FName).IsRequired(true);
            U.Property(U => U.LName).IsRequired(true);
            U.Property(U => U.Passwprd).IsRequired(true).
            HasAnnotation("DataType", DataType.Password);

            U.Property(U => U.PhoneNumber).IsRequired(true); 
            #endregion
        });

        modelBuilder.Entity<Article>(Ar =>
        {
          

            Ar.HasOne(Ar => Ar.Admins)
                   .WithMany(A => A.articles)
                   .HasForeignKey(A => A.Admins.AdminId)
                   .OnDelete(DeleteBehavior.Cascade);


            Ar.HasOne(Ar => Ar.Authors)
           .WithMany(A => A.Articles)
           .HasForeignKey(A => A.Authors.AuthorId)
           .OnDelete(DeleteBehavior.Cascade);



            #region Additional_Data
            Ar.HasKey(nameof(Article))
               .HasAnnotation("DataType", DataType.Url);


            Ar.Property(Ar => Ar.Title)
      .IsRequired(true).HasAnnotation("DataType", DataType.Text);

            Ar.Property(Ar => Ar.UploadedBy).IsRequired(true);

            Ar.Property(Ar => Ar.UploadDate).IsRequired(true)
            .HasAnnotation("DataType", DataType.DateTime);

            Ar.Property(Ar => Ar.CreatedDate).HasComputedColumnSql("CreatedDate_Article");

            Ar.Property(Ar => Ar.ArticleId).UseIdentityColumn(10, 10);



            #endregion



        });

        modelBuilder.Entity<Podcast>(P =>
        {

            P.HasOne(p => p.Admins)
             .WithMany(A => A.podcasts)
             .HasForeignKey(A => A.Admins.AdminId)
             .OnDelete(DeleteBehavior.Cascade);

            P.HasOne(p => p.podCasters)
            .WithMany(A => A.Podcasts)
            .HasForeignKey(A => A.podCasters.PodCasterId)
            .OnDelete(DeleteBehavior.Cascade);


            #region Additional_Data

            P.HasAnnotation("DataType", DataType.Url);


            P.Property(P => P.Title)
              .IsRequired(true).HasAnnotation("DataType", DataType.Text);

            P.Property(P => P.UploadedBy).IsRequired(true)
            .HasAnnotation("DataType", DataType.Text);

            P.Property(P => P.UploadDate).IsRequired(true)
            .HasAnnotation("DataType", DataType.DateTime);

            P.Property(P => P.CreatedDate).HasComputedColumnSql("CreatedDate_Podcast");

            P.Property(P => P.PodcastId).UseIdentityColumn(100, 20);
            // Start From 100 and Increment +20 



            #endregion


        });

        modelBuilder.Entity<Meditation>(M =>
        {
          

            M.HasOne(M => M.Admins)
            .WithMany(A => A.Meditations)
            .HasForeignKey(A => A.Admins.AdminId);




            #region Additional_Data
  M.HasKey(nameof(Meditation.MeditationId)).
            HasAnnotation("DataType", DataType.Url);

            M.Property(M => M.Title)
               .IsRequired(true).HasAnnotation("DataType", DataType.Text);

            M.Property(M => M.UploadedBy).IsRequired(true)
            .HasAnnotation("DataType", DataType.Text);


            M.Property(M => M.Content).IsRequired(true)
            .HasAnnotation("DataType", DataType.Text);



            M.Property(M => M.UploadDate).IsRequired(true)
            .HasAnnotation("DataType", DataType.DateTime);

            M.Property(M => M.CreatedDate).HasComputedColumnSql("CreatedDate_Meditation");


            M.Property(M => M.MeditationId).UseIdentityColumn(110, 10); 


            #endregion


        });

        modelBuilder.Entity<Pdf>( PD => {
                PD.Property(PD => PD.PdfId)
                .UseIdentityColumn(1, 1);
            
            PD.HasOne(PD => PD.Admins)
            .WithMany(A => A.pdfs)
            .HasForeignKey(A => A.Admins.AdminId)
             .OnDelete(DeleteBehavior.Cascade);
            });

        modelBuilder.Entity<Video>(V =>
        {
            V.Property(V => V.VideoId)
            .UseIdentityColumn(5, 5);

            V.HasOne(V => V.Admins)
           .WithMany(A => A.videos)
          .HasForeignKey(A => A.Admins.AdminId)
           .HasForeignKey(c => c.Courses.CourseId)
        .OnDelete(DeleteBehavior.Cascade);

        });

        modelBuilder.Entity<Course>(c =>
                  {
                   c.HasMany(c => c.CourseMateriels)
                      .WithOne(cm => cm.Course)
                      .HasForeignKey(cm => cm.CourseId);

               c.HasOne(c => c.instructors)
                         .WithMany(Ins => Ins.courses)
                         .HasForeignKey(Ins => Ins.instructors.InstructorId)
              .OnDelete(DeleteBehavior.Cascade);


                      c.HasMany(c => c.categories)
                       .WithMany(cc => cc.Courses);



                      c.Property(c => c.CourseId)
                       .UseIdentityColumn(200, 1);


                  });

        modelBuilder.Entity<Category>(cc => { cc.HasMany(cc => cc.Courses).WithMany(c => c.categories); });

        modelBuilder.Entity<Author>(AU =>
        { AU.Property(Au => Au.AuthorId)
            .UseIdentityColumn(10, 2); AU.HasMany(AU => AU.Articles)   
            .WithOne(Ar => Ar.Authors)
            .HasForeignKey(Ar => Ar.ArticleId).OnDelete(DeleteBehavior.Cascade);

        });

        modelBuilder.Entity<PodCaster>(POD =>
        { 
        
        POD.Property(POD => POD.PodCasterId)    
            .UseIdentityColumn(100 , 1);   
        
            POD.HasMany(POD => POD.Podcasts)
           .WithOne(P => P.podCasters)
           .HasForeignKey(P => P.PodcastId)
        
           .OnDelete(DeleteBehavior.Cascade);

        
        });

        modelBuilder.Entity<Instructor>(Ins =>
        {
            Ins.Property(Ins => Ins.InstructorId)
            .UseIdentityColumn(100, 1);

            Ins.HasMany(Ins => Ins.courses)
                .WithOne(c => c.instructors)
                .OnDelete(DeleteBehavior.Cascade);
  } );

        modelBuilder.Entity<Logs>(L =>
        {
            L.Property(L => L.DescriptionLogs)
            .UseIdentityColumn(100, 1);

            L.HasOne(L => L.users)
                .WithMany(U => U.Logs)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Payments>(PA => {
        PA.HasOne(PA => PA.users)
            .WithMany(U => U.Payments)
            .HasForeignKey(U => U.users.UserId)
            .OnDelete(DeleteBehavior.Cascade);




        });

        modelBuilder.Entity<Enrollment_Details>()
           .HasKey(e => new { e.StudentID, e.CourseID }); // Composite key




























    }


}

