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

    #endregion
}