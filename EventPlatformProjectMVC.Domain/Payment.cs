using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlatformProjectMVC.Domain
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int BookingID { get; set; } // FK -> Bookings
        public virtual Booking? Booking { get; set; }
        [Required]
        public string PaymentMethod { get; set; } = string.Empty;
        [Required]
        public DateTime PaymentDate { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public PaymentStatus PaymentStatus { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
    }
}

