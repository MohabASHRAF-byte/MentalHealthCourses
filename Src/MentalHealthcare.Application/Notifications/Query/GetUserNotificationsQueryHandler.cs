//using AutoMapper;
//using MediatR;
//using MentalHealthcare.Application.SystemUsers;
//using MentalHealthcare.Domain.Constants;
//using MentalHealthcare.Domain.Dtos;
//using MentalHealthcare.Domain.Repositories;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Threading;
//using System.Threading.Tasks;

//namespace MentalHealthcare.Application.Notifications.Query
//{
//    public class GetUserNotificationsQueryHandler : IRequestHandler<GetUserNotificationsQuery, List<NotificationDTO>>
//    {
//        private readonly INotificationRepository _notificationRepository;
//        private readonly ILogger<GetUserNotificationsQueryHandler> _logger;
//        private readonly IUserContext _userContext;
//        private readonly IMapper _mapper;

//        public GetUserNotificationsQueryHandler(
//            INotificationRepository notificationRepository,
//            ILogger<GetUserNotificationsQueryHandler> logger,
//            IUserContext userContext,
//            IMapper mapper)
//        {
//            _notificationRepository = notificationRepository;
//            _logger = logger;
//            _userContext = userContext;
//            _mapper = mapper;
//        }

//        public Task<List<NotificationDTO>> Handle(GetUserNotificationsQuery request, CancellationToken cancellationToken)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
