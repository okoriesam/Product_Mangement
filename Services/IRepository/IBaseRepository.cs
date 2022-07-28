using System.Linq.Expressions;

namespace Product_Management.Services.IRepository
{
    public interface IBaseRepository<T> where T : class
    {
        T Get(int id);

        IEnumerable<T> GetAll(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includePropperties = null
            );
        T GetFirstOrDefault(
             Expression<Func<T, bool>> filter = null,
              string includePropperties = null
            );
        void AddRange(IEnumerable<T> entity);

        void Add(T entity);

        void Update(T entity);
        void Delete(int id);

        void Delete(T entity);

       /* T Save();*/

    }
}
