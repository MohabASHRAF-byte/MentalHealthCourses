using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Domain.Entities
{
    public class Notification
    {

        public int NotificationId { get; set; }

        public string ContentOfNotification { get; set; }

        public string UserName { get; set; }

        // Navigation property for the users who receive the notification
        public ICollection<HumanBe> Users { get; set; } = new List<HumanBe>();
    }
}
