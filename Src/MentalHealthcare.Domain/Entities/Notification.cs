using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Domain.Entities
{
    public class Notification
    {
        public int Id { get; set; }

        public int UserId { get; set; } // FK

        public string Message { get; set; }
        public string? Title { get; set; }
        public string Type { get; set; }
        public int RelatedId { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey(nameof(UserId))] // FIXED
        public SystemUser User { get; set; }
    }
}

/*



 public int Id { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;

    

        // Receiver (Admin or SystemUser)
        public string? ReceiverId { get; set; }
        public User Receiver { get; set; }
        public bool IsDelivered { get; set; } = false;







*/