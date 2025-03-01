using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlatformProjectMVC.Domain
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = string.Empty;

        public byte[]? ProfilePicture { get; set; }

        [Required]
        public DateTime CreateAt { get; set; }
    }
}
