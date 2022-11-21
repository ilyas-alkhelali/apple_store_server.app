using Apple_Store_Db_Server.Dto;
using Apple_Store_Db_Server.Models;
using Apple_Store_Db_Server.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Apple_Store_Db_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(ApplicationContext context)
        {

            _context = context;
        }

       
        [HttpGet("getAllProducts")]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }

        [HttpGet("getProductsByTypeName/{typeName}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByTypeName(string ?typeName)
        {
            List<Product> products = new List<Product>();

            foreach(var product in _context.Products)
            {
                if(product.TypeName == typeName)
                {
                    products.Add(product);
                }

            }
            
            return products;
        }
        
        [HttpGet("getOrders")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders.Include(p => p.Products).ToListAsync();
        }

        [HttpGet("getProductById/{id}")]
        public async Task<ActionResult<Product>> GetProductById(Guid ?id)
        {
            if(id == null)
            {
                return Ok("null");
            }
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return Ok("hello");
            }

            return product;
        }

        [HttpPut("updateProduct/{id}")]
        public async Task<IActionResult> PutProduct(Guid id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost("addProduct")]
        public async Task<ActionResult<Product>> PostProduct(ProductDto prod)
        {
            if (prod == null)
            {
                return BadRequest();
            }

            Product product = new Product
            {
                TypeName = prod.TypeName,
                Version = prod.Version,
                Price = prod.Price,
                Amount = prod.Amount,
                About = prod.About
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Ok(product);
        }


        [HttpDelete("deleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(Guid id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        [HttpPut("generateRating/{id}/{rating}")]
        public async Task<ActionResult<decimal>> GenerateRating(Guid id, decimal rating)
        {
            if(rating == null)
            {
                return BadRequest();
            }
            else if(id == null)
            {
                return NotFound();
            }

            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if(product.SumOfRates == null)
            {
                product.SumOfRates = 0;
                product.SumOfRates += rating;
                product.NumberOfRated += (int)1;
                product.Rating = (decimal)product.SumOfRates / product.NumberOfRated;
            }
            else
            {
                product.SumOfRates += rating;
                product.NumberOfRated += (int)1;
                product.Rating = (decimal)product.SumOfRates / product.NumberOfRated;
            }

            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return Ok(product.Rating);
        }

        [HttpPost("buy")]
        public async Task<ActionResult<Order>> createOrder(List<OrderDto> prod, Guid userId)
        {
            User user = await _context.Users.FirstOrDefaultAsync(p => p.Id == userId);

            Order order = new Order
            {
                User = user,
                UserId = user.Id
            };

            foreach(var item in prod) { 
               
                order.Cost = order.Cost + item.Product.Price;
                order.Products.Add(item);

                item.Product.Amount = item.Product.Amount - item.Amount;
                _context.Products.Update(item.Product);

            };
           
            _context.Orders.Add(order);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return Ok(order);
        }
    }
}
