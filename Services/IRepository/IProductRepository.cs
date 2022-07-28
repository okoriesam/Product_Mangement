using Product_Management.Models;

namespace Product_Management.Services.IRepository
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        void DisableproductById(int id);

        double GetSumOfProductPriceSinceSevenDays();

        IEnumerable<Product> GetDisableProducts();
    }
}
