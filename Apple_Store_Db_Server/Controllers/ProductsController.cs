using Apple_Store_Db_Server.Dto;
using Apple_Store_Db_Server.Models;
using Apple_Store_Db_Server.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Apple_Store_Db_Server.Services;

namespace Apple_Store_Db_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly IEmailService _sendEmail;

        public ProductsController(ApplicationContext context, IEmailService sendEmail)
        {
            _sendEmail = sendEmail;
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

        [Authorize]
        [HttpGet("getOrders")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders.Include(p => p.Products).ThenInclude(p => p.Product).ToListAsync();
        }

        [HttpGet("getProductById/{id}")]
        public async Task<ActionResult<Product>> GetProductById(Guid id)
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

        [Authorize]
        [HttpPut("updateProduct/{id}")]
        public async Task<IActionResult> PutProduct(Guid id, Product product)
        {
            if (ModelState.IsValid)
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
            else return BadRequest();
        }

        [Authorize]
        [HttpPost("addProduct")]
        public async Task<ActionResult<Product>> PostProduct(ProductDto prod)
        {
            if (ModelState.IsValid)
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
            else return BadRequest();
        }

        [Authorize]
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

        [Authorize]
        [HttpPut("generateRating/{id}/{rating}")]
        public async Task<ActionResult<decimal>> GenerateRating(Guid id, double rating)
        {
            if (ModelState.IsValid)
            {
                if (rating == null)
                {
                    return BadRequest();
                }
                else if (id == null)
                {
                    return BadRequest();
                }

                Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

                if (product == null)
                {
                    return BadRequest();
                }
                else
                {
                    product.SumOfRates += rating;
                    product.NumberOfRated += (int)1;
                    product.Rating = (double)(product.SumOfRates / product.NumberOfRated);
                }

                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                return Ok(product.Rating);
            }
            else return BadRequest();
        }

        [Authorize]
        [HttpPost("buy/{userId}")]
        public async Task<ActionResult<Order>> createOrder(List<OrderDto> prod, Guid userId)
        {
            if (ModelState.IsValid)
            {
                User user = await _context.Users.FirstOrDefaultAsync(p => p.Id == userId);

                Order order = new Order
                {
                    User = user,
                };

                foreach (var item in prod)
                {

                    BoughtProduct boughtProduct = new BoughtProduct
                    {
                        Amount = item.Amount,
                        Capacity = item.Capacity,
                        Product = item.Product,
                        Order = order
                    };

                    order.Cost = order.Cost + item.Product.Price;
                    order.Products.Add(boughtProduct);

                    item.Product.Amount = item.Product.Amount - item.Amount;
                    _context.Products.Update(item.Product);

                };

                _context.Orders.Add(order);
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                _sendEmail.SendEmail(user.Email, "Apple Store",
                    $"<h4>Congrats, {user.Name}, your order has been placed and will be delivered within 3-5 business days</h4>");
                return Ok(order);
            }
            else return BadRequest();
        }

        [NonAction]
        private bool ProductExists(Guid id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
