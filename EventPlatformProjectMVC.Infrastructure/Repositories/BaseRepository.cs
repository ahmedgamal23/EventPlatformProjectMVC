using EventPlatformProjectMVC.Application.Interfaces;
using EventPlatformProjectMVC.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EventPlatformProjectMVC.Infrastructure.Repositories
{
    public class BaseRepository<T, Ttype> : IBaseRepository<T, Ttype> where T : class
    {
        private readonly ApplicationDbContext _context;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAllAsync(int pageNumber=1,
            int pageSize=5,
            Expression<Func<T, bool>> filter = null , 
            Func<IQueryable<T>,IQueryable<T>>? include = null
            )
        {
            if(pageNumber < 0 || pageSize < 0)
            {
                throw new ArgumentException("Page number and page size must be greater than 0");
            }

            IQueryable<T> query = _context.Set<T>();
            if (include != null)
                query = include(query);

            if(filter != null)
                query = query.Where(filter);

            var result = await query.ToListAsync();
            return result
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public async Task<T?> GetByIdAsync(Ttype id, Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            IQueryable<T> query = _context.Set<T>();
            if (include != null)
                query = include(query);
            return await query.FirstOrDefaultAsync(entity => EF.Property<Ttype>(entity, "Id")!.Equals(id));
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public void Update(T entity)
        {
            _context.Update(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
    }
}

