using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace EventPlatformProjectMVC.Application.Interfaces
{
    public interface IBaseRepository<T, Ttype> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(
            int pageNumber=1,
            int pageSize=5,
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T> , IQueryable<T>>? include = null);
        Task<T?> GetByIdAsync(Ttype id, Func<IQueryable<T>, IQueryable<T>>? include = null);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
