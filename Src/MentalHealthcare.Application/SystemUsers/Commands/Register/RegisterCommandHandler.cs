using AutoMapper;
using MediatR;
using MentalHealthcare.Application.Background_Services;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.SystemUsers.Commands.Register;

public class RegisterCommandHandler(
    ILogger<RegisterCommandHandler> logger,
    IUserRepository userRepository,
    IEmailSender emailSender,
    UserManager<User> userManager,
    OtpService _otpService,
    IMapper mapper
) : IRequestHandler<RegisterCommand, OperationResult<UserDto>>
{
    public async Task<OperationResult<UserDto>> Handle(RegisterCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Register systemUser with Email : {@user}", request.Email);

        if (string.IsNullOrEmpty(request.Tenant))
        {
            logger.LogInformation("Invalid Tenant");

            return OperationResult<UserDto>.Failure("Bad Request");
        }

        User user = mapper.Map<User>(request);
        user.Roles = Roles.AddRole(user.Roles, UserRoles.User);
        SystemUser systemUser = mapper.Map<SystemUser>(request);
        systemUser.User = user;
        systemUser.FName = request.FirstName;
        systemUser.LName = request.LastName;
        systemUser.CreatedDate = DateTime.UtcNow;
        systemUser.LastUpdatedDate = DateTime.UtcNow;
        systemUser.BirthDate = request.Birthday;

        logger.LogInformation("Inserting {@user} to the DB", request.UserName);
        var isInserted = await userRepository.RegisterUser(user, request.Password, systemUser);
        if (!isInserted.Succeeded)
        {
            logger.LogInformation("User {@user} already exists", request.UserName);
            var ret = OperationResult<UserDto>.Failure("Try Again", StateCode.BadRequest);
            ret.Errors.AddRange(isInserted.Errors);
            return ret;
        }

        logger.LogError("User {@user} Registered successfully", request.UserName);
        await SendConfirmation(user);
        var userDto = mapper.Map<UserDto>(request);
        logger.LogInformation("User {@user} Registered successfully", request.UserName);
        return OperationResult<UserDto>.SuccessResult(userDto, "User Created Successfully", StateCode.Created);
    }

    private async Task SendConfirmation(User user)
    {
        var otpCode = GenerateOtp(); // ✅ your custom OTP
        _otpService.AddOrUpdateOtp(user.Email!, "Default", otpCode); // ✅ store it

        await emailSender.SendEmailAsync(
     user.Email!,
     "تأكيد البريد الإلكتروني",
     "أهلاً بك في تطبيق اللجنة المجمعية للصحة النفسية ومكافحة الإدمان بالكنيسة القبطية الأرثوذكسية.\n" +
     "نحن سعداء بانضمامك إلينا، ونتمنى لك تجربة مثرية ومفيدة من خلال خدماتنا وبرامجنا.\n" +
     $"رمز التحقق الخاص بك هو: {otpCode}\n" +
     "يرجى استخدام هذا الرمز لتأكيد بريدك الإلكتروني. صلاحية الرمز 5 دقائق فقط.\n" +
     "إذا لم تطلب هذا الرمز، يمكنك تجاهل هذه الرسالة.\n\n" +
     "مع تحياتنا،\nفريق تطبيق اللجنة المجمعية للصحة النفسية\nالكنيسة القبطية الأرثوذكسية"
 );


    }


    private string GenerateOtp()
    {
        var random = new Random();
        return random.Next(100000, 999999).ToString(); // 6-digit OTP
    }

}