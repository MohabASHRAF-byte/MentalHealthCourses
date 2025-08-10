//using MediatR;
//using MentalHealthcare.API.Hubs;
//using MentalHealthcare.Domain.Constants;
//using MentalHealthcare.Domain.Dtos;
//using MentalHealthcare.Domain.Dtos.Notification;
//using MentalHealthcare.Domain.Entities;
//using MentalHealthcare.Domain.Repositories;
//using MentalHealthcare.Infrastructure.Persistence;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.SignalR;
//using Microsoft.EntityFrameworkCore;
//using NuGet.Protocol.Plugins;
//using System.Security.Claims;

//namespace MentalHealthcare.API.Controllers
//{
//    [AllowAnonymous]

//    [ApiController]
//    [Route("[controller]")]
//    [Authorize(AuthenticationSchemes = "Bearer")]
//    [ApiExplorerSettings(GroupName = Global.DashboardVersion)]

//    public class NotificationController : ControllerBase
//    {


//        private readonly INotificationRepository _notificationRepository;
//        private readonly ILogger<NotificationController> _logger;

//        public NotificationController(INotificationRepository notificationRepository, ILogger<NotificationController> logger)
//        {
//            _notificationRepository = notificationRepository;
//            _logger = logger;
//        }



//        [HttpGet]
//        public async Task<IActionResult> GetUnreadNotifications()
//        {
//            var userId = User.FindFirstValue("sub"); // Or use a custom claim like "UserId" if it exists and is valid

//            if (string.IsNullOrWhiteSpace(userId))
//            {
//                return BadRequest("Invalid user ID.");
//            }

//            var notifications = await _notificationRepository.GetUnreadNotificationsAsync(userId);


//            var response = notifications.Select(n => new NotificationDto
//            {
//                Id = n.Id,
//                Title = n.Title,
//                Message = n.Message,
//                Type = n.Type,
//                CreatedAt = n.CreatedAt,

//            });

//            return Ok(response);
//        }





//        [HttpPut("mark-read")]
//        [Authorize]
//        public async Task<IActionResult> MarkNotificationsAsRead([FromBody] MarkReadRequest request)
//        {
//            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

//            if (!int.TryParse(userIdStr, out int userId))
//            {
//                return BadRequest("Invalid user ID.");
//            }

//            try
//            {
//                int affectedRecords;

//                if (request.MarkAll)
//                {
//                    affectedRecords = await _notificationRepository.MarkAllNotificationsAsReadAsync(userId);
//                }
//                else
//                {
//                    if (request.NotificationIds == null || !request.NotificationIds.Any())
//                    {
//                        return BadRequest("Notification IDs are required when MarkAll is false");
//                    }

//                    affectedRecords = await _notificationRepository.MarkNotificationsAsReadAsync(
//                        userId,
//                        request.NotificationIds);
//                }

//                return Ok(new
//                {
//                    Message = "Notifications marked as read",
//                    AffectedCount = affectedRecords
//                });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error marking notifications as read");
//                return StatusCode(500, "An error occurred");
//            }

//        }

//        [HttpPost("notify")]
//        public async Task<IActionResult> SendNotification(
//   [FromBody] NotificationRequest request,
//   [FromServices] INotificationRepository notificationService)
//        {
//            if (!int.TryParse(request.UserId, out int userId))
//            {
//                return BadRequest("Invalid UserId format.");
//            }

//            await notificationService.SendNotificationAsync(
//                userId,
//                request.Title,
//                request.Message);

//            return Ok();
//        }


//    }
//}