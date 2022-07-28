using Product_Management.Models;

namespace Product_Management.Services.IRepository
{
    public interface IAccountRepository : IBaseRepository<Users>
    {
        void CreateAdmins(int id);
        void DeleteAdmins(int id);
    }
}
