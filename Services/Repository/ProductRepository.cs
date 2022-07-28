using Product_Management.Context;
using Product_Management.Models;
using Product_Management.Services.IRepository;

namespace Product_Management.Services.Repository
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public void DisableproductById(int id)
        {
            var product = _context.Products.FirstOrDefault(x => x.Id == id);

            if (product != null)
                product.IsDisabled = true;

            _context.SaveChanges();
        }

        public IEnumerable<Product> GetDisableProducts()
        {
          var disableProducts = _context.Products.Where(a => a.IsDisabled == true )
                                .OrderByDescending(x => x.DateCreated).ToList();

            if (disableProducts != null)
            {
               return disableProducts;
            }

            return null;
        }

        public double GetSumOfProductPriceSinceSevenDays()
        {
            DateTime end = DateTime.Today;
            DateTime start = end.AddDays(-7);
            var ProductPriceSinceSevenDay = _context.Products.Where(x => x.DateCreated >= start && x.DateCreated <= end).Sum(a => a.Price); 
                return ProductPriceSinceSevenDay;
        }
    }
}
