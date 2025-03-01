using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlatformProjectMVC.Domain
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserID { get; set; } = string.Empty; // FK -> Users
        public virtual ApplicationUser? User { get; set; }
        [Required]
        public int EventID { get; set; } // FK -> Events
        public virtual Event? Event { get; set; }
        [Required]
        public string Message { get; set; } = string.Empty;
        [Required]
        public DateTime SentAt { get; set; }
    }
}
