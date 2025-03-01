using EventPlatformProjectMVC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlatformProjectMVC.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public IBaseRepository<ApplicationUser, string> ApplicationUsers { get; }
        public IBaseRepository<Booking, int> Booking { get; }
        public IBaseRepository<Event, int> Events { get; }
        public IBaseRepository<Notification, int> Notifications { get;}
        public IBaseRepository<Payment, int> Payments { get; }

        Task<int> SaveChangesAsync();
    }
}
