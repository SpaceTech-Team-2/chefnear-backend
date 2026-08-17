using AutoMapper;
using ChefNear.Application.Features.Notifications.DTOs;
using ChefNear.Domain.Entities;

namespace ChefNear.Application.Features.Notifications.Mappings;

public class NotificationProfile : Profile
{
    public NotificationProfile()
    {
        CreateMap<Notification, NotificationDto>();
    }
}
