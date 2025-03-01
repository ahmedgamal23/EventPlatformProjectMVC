using EventPlatformProjectMVC.Application.Interfaces;
using EventPlatformProjectMVC.Domain;
using EventPlatformProjectMVC.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlatformProjectMVC.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IBaseRepository<ApplicationUser, string> ApplicationUsers { get; private set; }
        public IBaseRepository<Booking, int> Booking { get; private set; }
        public IBaseRepository<Event, int> Events { get; private set; }
        public IBaseRepository<Notification, int> Notifications { get; private set; }
        public IBaseRepository<Payment, int> Payments { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            ApplicationUsers = new BaseRepository<ApplicationUser, string>(_context);
            Booking = new BaseRepository<Booking, int>(_context);
            Events = new BaseRepository<Event, int>(_context);
            Notifications = new BaseRepository<Notification, int>(_context);
            Payments = new BaseRepository<Payment, int>(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
