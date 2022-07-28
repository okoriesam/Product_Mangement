using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product_Management.Context;
using Product_Management.Models;
using Product_Management.Services.IRepository;
using Product_Management.ViewModel;

namespace Product_Management.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
   
    public class ProductController : ControllerBase
    {
        public readonly IProductRepository _productRepository;
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context, IProductRepository productRepository)
        {
            _context = context;
            _productRepository = productRepository;
        }

      //  public static Product product = new Product();
        [Route("api/Product/CreateProduct")]
        [HttpPost]
        
        public async Task<ActionResult> CreateProduct(Product request)
        {
            var product = new Product();
            product.Price = request.Price;
            product.ProductName = request.ProductName;
            product.DateCreated = request.DateCreated;
            product.IsDisabled = request.IsDisabled;

            _productRepository.Add(product);
            await _context.SaveChangesAsync();
            return Ok();

        }

        [Route("api/Product/Products")]
        [HttpGet]
        public async Task<ActionResult> AllProduct()
        {
            var AllProducts = _productRepository.GetAll();
            return Ok(AllProducts);
        }

        [Route("api/Product/SumOfProducts")]
        [HttpGet]
        public async Task<ActionResult> TheSumOfTheProductCreatedLastWeek()
        {
            var AllProductCreatedSinceLastWeek =  _productRepository.GetSumOfProductPriceSinceSevenDays();
            return Ok(AllProductCreatedSinceLastWeek);      
        }


      //  [Authorize(Roles ="SuperAdmin")]
        [Route("api/Product/DisabledProducts")]
        [HttpGet]
        public async Task<ActionResult> AllDisableProduct()
        {
            var AllDisabledProducts = _productRepository.GetDisableProducts();
            return Ok(AllDisabledProducts);
        }



        [Route("api/Product/UpdateProduct")]
        [HttpPut]
        public async Task<ActionResult> UpdateProduct(Product request)
        {
            var update = _productRepository.GetFirstOrDefault(x => x.Id == request.Id);
            if (update != null)
            {

                update.ProductName = request.ProductName;
                update.Price = request.Price;
                update.IsDisabled = request.IsDisabled;
                update.DateCreated = request.DateCreated;

                _productRepository.Update(update);
                //  _productRepository.Entry(update).State = EntityState.Modified;
                await _context.SaveChangesAsync();

            }
            return Ok();
        }


       // [Authorize(Roles = "SuperAdmin")]
        [Route("api/Product/DeleteProduct")]
        [HttpDelete]
        public async Task<ActionResult> DeleteProduct(int Id)
        {
            var deleteProduct = _productRepository.GetFirstOrDefault(x => x.Id == Id);
            if (deleteProduct != null)
                _productRepository.Delete(Id);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
