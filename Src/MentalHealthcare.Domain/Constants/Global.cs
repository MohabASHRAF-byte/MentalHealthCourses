namespace MentalHealthcare.Domain.Constants;

public class Global
{
    public const string MobileVersion = "MobileApp";
    public const string DashboardVersion = "Dashboard";
    public const string SharedVersion = "Shared";
    public const string DevelopmentVersion = "Development";
    public const string AllVersion = "All";
    public const string Roles = "Roles";
    public const string PassCode = "TokenPassCode";
    public const string ProgramName = "MentalHealthcare";
    public const string TenantClaimType = "TenantClaimType";
    public const string ApplicationTenant = "MentalHealthcareApp";
    public const int UrlMaxLength = 250;
    public const int TitleMaxLength = 100;
    public const int MaxNameLength = 50;
    public const string AdminIdClaimType = "AdminId";
    public const string UserIdClaimType = "UserId";
    public static string CourseThumbnailDirectory = "CoursesThumbnail";
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

    #region ContactUs

    public const int ContactUsMaxNameLength = 50;
    public const int ContactUsMaxEmailLength = 50;
    public const int ContactUsMaxPhoneLength = 20;
    public const int ContactUsMaxMsgLength = 500;

    #endregion

    #region Courses

    public const int CourseSectionNameMaxLength = 50;

    public const int CourseRecourseSize = 10; //Mb
    public const string CourseRecoursesPath = "CourseResourses";

    public const int CourseLessonPdfSize = 10; //Mb

    //todo: up to 95 %
    public const float CourseCompleteToReview = .0f;
    public const int UserReviewsLimit = 5;

    #endregion

    #region Cart

    public const int MaxCartItems = 20;

    #endregion
}