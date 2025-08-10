using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Domain.Dtos.Notification
{
    public class MarkReadRequest
    {

        public List<int> NotificationIds { get; set; }
        public bool MarkAll { get; set; } = false;

    }
}
