using MediatR;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MentalHealthcare.Infrastructure.Hubs
{
    public class NotificationHub(MentalHealthDbContext dbContext) : Hub
    {
        ////TO All Users (Admins - End Users) 
        //public async Task SendNotificationToAll(HumanBe User,Notification notification)
        //{
        //    await dbContext.Notification.AddAsync(notification);
        //    await dbContext.SaveChangesAsync();
        //    await Clients.All.SendAsync(method: "ReceiveNotification", notification);
        //}

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        //TO All Users Excepted for Sender (Admins - End Users)
        [Authorize(Roles = "Admin, User")]
        [HubMethodName("SendNotification-ToAllClients")]
        public async Task SendNotificationToOthers(HumanBe User, Notification notification)
        {
            await dbContext.Notifications.AddAsync(notification);
            await dbContext.SaveChangesAsync();
            await Clients.Others.SendAsync(method: "ReceiveNotification", notification);
        }



        //TO specific User 
        [Authorize(Roles = "Admin, User")]
        [HubMethodName("SendNotification-ToSpecificUser")]

        public async Task SendNotification(HumanBe User, string userName, Notification notification)
        {
            await dbContext.Notifications.AddAsync(notification);
            await dbContext.SaveChangesAsync();
            //when user 
            await Clients.Client(Context.ConnectionId).SendAsync("ReceiveNotification", notification);
            await Clients.Client(Context.ConnectionId).SendAsync("ReceiveNotification", notification);


        }




        //Self Notification  (Admins - End Users)
        [Authorize(Roles = "Admin, User")]
        [HubMethodName("Self-Notification")]
        public async Task SendNotificationToMySelf(HumanBe User, Notification notification)
        {
            await dbContext.Notifications.AddAsync(notification);
            await dbContext.SaveChangesAsync();
            await Clients.Caller.SendAsync(method: "ReceiveNotification", notification);
        }




        // Send notification to a specific group
        [Authorize(Roles = "Admin")]
        [HubMethodName("SendNotification-ToSpecificGroup")]
        public async Task SendNotificationToGroup(string groupName, Notification notification)
        {
            await dbContext.Notifications.AddAsync(notification);
            await dbContext.SaveChangesAsync();
            //ex : add Section In Course
            await Clients.Group(groupName)
            .SendAsync("ReceiveNotification", notification);
        }


        // Send notification to multiple groups
        [Authorize(Roles = "Admin")]
        [HubMethodName("SendNotification-ToMultipleGroups")]
        public async Task SendNotificationToGroups(IEnumerable<string> groupNames, Notification notification)
        {     // Save the notification to the database
            await dbContext.Notifications.AddAsync(notification);
            await dbContext.SaveChangesAsync();

            // Send the notification to the specified group
            await Clients.Groups(groupNames).SendAsync("ReceiveNotification", notification);
        }

        // Add a user to a group
        [Authorize(Roles = "Admin , User")]
        [HubMethodName("AddUser-ToGroup")]
        public async Task AddToGroup(int ConnectionId, string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            //Notify only admins that a specific user joined the group
            var userName = Context.User.Identity.Name;

            // Ex : Notify Admins that specific User want to buy course
            await Clients.Group("AdminGroup")
               .SendAsync("ReceiveNotification", $"{userName}  joined the group {groupName}");
            await Clients.Caller.SendAsync("ReceiveNotification", "The request has been submitted.");

        }


        //Remove a user from a group
        [Authorize(Roles = "Admin")]
        [HubMethodName("RemoveUser-ToGroup")]
        public async Task RemoveFromGroup2(string groupName)
        { await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName); }












    }
}