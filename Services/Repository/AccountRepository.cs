using Microsoft.EntityFrameworkCore;
using Product_Management.Context;
using Product_Management.Models;
using Product_Management.Services.IRepository;

namespace Product_Management.Services.Repository
{
    public class AccountRepository : BaseRepository<Users>, IAccountRepository
    {
        public readonly ApplicationDbContext _context;
        public AccountRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void CreateAdmins(int id)
        {
           
        }

        public void DeleteAdmins(int id)
        {
            var deleteAdmin = _context.Users.Where(x => x.Id == id).FirstOrDefault();
            if (deleteAdmin != null)

                _context.Entry(deleteAdmin).State = EntityState.Deleted;
            _context.SaveChanges();
        }
    }
}
