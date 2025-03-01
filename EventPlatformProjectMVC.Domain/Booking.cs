using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlatformProjectMVC.Domain
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserID { get; set; } = string.Empty; // FK -> Users
        public virtual ApplicationUser? User { get; set; }
        [Required]
        public int TicketCount { get; set; }
        [Required]
        public double TotalPrice { get; set; }
        [Required]
        public DateTime BookingDate { get; set; }
        [Required]
        public string PaymentStatus { get; set; } = string.Empty; // Paid, Pending, Failed
        [Required]
        public DateTime CreatedAt { get; set; }
    }
}

