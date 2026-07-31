using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Domain.Common
{
    public interface IDomainEvent : INotification
    {
    }

}
