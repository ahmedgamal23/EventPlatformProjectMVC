using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlatformProjectMVC.Domain
{
    public class Event
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string Location { get; set; } = string.Empty;
        public int AvailableSeats { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public DateTime StartDateTime { get; set; }
        [Required]
        public DateTime EndDateTime { get; set; }
        [Required]
        public string OrganizerId { get; set; } = string.Empty; // FK -> Users
        public virtual ApplicationUser? Organizer { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }

        public byte[]? Picture { get; set; }

        [NotMapped]
        public IFormFile? PictureFile { get; set; }
    }
}


