using EventPlatformProjectMVC.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlatformProjectMVC.Infrastructure.Data
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Notification>()
                .HasOne(x => x.Event)
                .WithMany()
                .HasForeignKey(x => x.EventID)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }


    }
}
