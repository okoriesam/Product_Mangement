using Microsoft.EntityFrameworkCore;
using Product_Management.Context;
using Product_Management.Services.IRepository;
using System.Linq.Expressions;

namespace Product_Management.Services.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {

        protected readonly ApplicationDbContext _context;
        internal DbSet<T> dbSet;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
            this.dbSet = context.Set<T>();
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public void AddRange(IEnumerable<T> entity)
        {
            dbSet.AddRange(entity);
        }

        public T Get(int id)
        {
            return dbSet.Find(id);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includePropperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            //includePropperties will be coma seperated
            if (includePropperties != null)
            {
                foreach (var includePropperty in includePropperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includePropperty);
                }
            }
            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter = null, string includePropperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            //includePropperties will be coma seperated
            if (includePropperties != null)
            {
                foreach (var includePropperty in includePropperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includePropperty);
                }
            }

            return query.FirstOrDefault();
        }

        public void Delete(int id)
        {
            T entityToRemove = dbSet.Find(id);
            Delete(entityToRemove);
        }

        public void Delete(T entity)
        {
            dbSet.Remove(entity);
            
        }

        public void Update(T entity)
        {
            dbSet.Update(entity);
        }

      /*  public void Save()
        {
            _context.SaveChanges(); 
        }*/
    }
}
