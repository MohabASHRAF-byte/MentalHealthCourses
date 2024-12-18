namespace MentalHealthcare.Domain.Constants;

public class Global
{
    public const string Roles = "Roles";
    public const string PassCode = "TokenPassCode";
    public const string ProgramName = "MentalHealthcare";
    public const string TenantClaimType = "TenantClaimType";
    public const string ApplicationTenant = "MentalHealthcareApp";
    public const int UrlMaxLength = 250;
    public const int TitleMaxLength = 100;
    public const int MaxNameLength = 50;

    public const string AdminIdClaimType = "AdminId";
    public const string UserIdClaimType = "AdminId";
    public static string CourseThumbnailDirectory = "CoursesThumbnails";
    public static string ThumbnailFileExtension = ".jpeg";


    #region Terms and Conditions 
    public enum HelpCenterItems
    {
        TermsConditions = 1,
        PrivacyPolicy = 2,
        FaQ = 3,
    }
    public const int TermNameMaxLength = 100;
    public const int TermDescriptionMaxLength = 500;
    #endregion

    #region advertisement 
    public const string AdvertisementFolderName = "Advertisements";
    public const int AdvertisementNameMaxLength = 50;
    public const int AdvertisementDescriptionMaxLength = 500;
    public const int AdvertisementImgSize = 10;

    #endregion
    #region Article
    public const string ArticleFolderName = "Articles";
    public const int ArticleNameMaxLength = 50;
    public const int ArticleDescriptionMaxLength = 500;
    public const int ArticleImgSize = 10;
    #endregion
    #region Meditation
    public const string MeditationFolderName = "Meditation";
    public const int MeditationNameMaxLength = 50;
    public const int MeditationDescriptionMaxLength = 500;
    public const int MeditationImgSize = 10;
    #endregion
    #region Podcast
    public const string PodcastFolderName = "Podcast";
    public const int PodcastNameMaxLength = 50;
    public const int PodcastDescriptionMaxLength = 500; //Audio File
    public const int PodcastImgSize = 10;
    #endregion
    #region PodCaster
    public const string PodCasterFolderName = "PodCaster";
    public const int PodCasterNameMaxLength = 50;
    public const int PodCasterDescriptionMaxLength = 500;
    public const int PodCasterImgSize = 10;
    #endregion
    #region Instructor
    public const string InstructorFolderName = "Instructor";
    public const int InstructorNameMaxLength = 50;
    public const int InstructorDescriptionMaxLength = 500;
    public const int InstructorImgSize = 10;
    #endregion
    #region Author
    public const string AuthorFolderName = "Author";
    public const int AuthorNameMaxLength = 50;
    public const int AuthorDescriptionMaxLength = 500;
    public const int AuthorImgSize = 10;
    #endregion

    #region ContactUs
    public const int ContactUsMaxNameLength = 50;
    public const int ContactUsMaxEmailLength = 50;
    public const int ContactUsMaxPhoneLength = 20;
    public const int ContactUsMaxMsgLength = 500;
    #endregion


    #region Courses

    public const int CourseSectionNameMaxLength = 50;

    public const int CourseRecourseSize = 10;//Mb
    public const string CourseRecoursesPath = "CourseResourses";
    public const int CourseLessonPdfSize = 10;//Mb
    #endregion
}