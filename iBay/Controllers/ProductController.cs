using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClassLibrary;

namespace iBay.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly Context _context;

        public ProductController(Context context)
        {
            _context = context;
        }

        /// <summary>
        /// List all the products
        /// </summary>
        /// <returns>Return the whole list of products</returns>
        /// <response code="200">Return list of product</response>
        [HttpGet]
        public IActionResult GetProduct()
        {
            return Ok(_context.Product);
        }

        /// <summary>
        /// List a specific product by his id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Return the product specified</returns>
        /// <response code="400">Product not found</response>
        /// <response code="200">Return a product</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product == null) return BadRequest("Product not found");

            return Ok(product);
        }

        /// <summary>
        /// Create a product
        /// </summary>
        /// <param name="product"></param>
        /// <returns>Return the newly product</returns>
        /// <response code="400">Product not found</response>
        /// <response code="200">Return a newly product</response>
        [HttpPost]
        public IActionResult CreateProduct(Product product)
        {
            if(product is null) return BadRequest("Product not found");

            _context.Product.Add(product);
            _context.SaveChanges();
            return Ok(product);
        }

        /// <summary>
        /// Update the product specified by his id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <returns>Return the updately product</returns>
        /// <response code="400">Product not found</response>
        /// <response code="200">Return a newly product</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product item)
        {
            var product = await _context.Product.FindAsync(id);
            if (product is null) return BadRequest(id);

            try
            {
                product.Name = item.Name;
                product.Image = item.Image;
                product.Price = item.Price;
                product.Available = item.Available;
                product.Added_time = item.Added_time;
                _context.Product.Update(product);
                _context.SaveChanges();
                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }   
        }

        /// <summary>
        /// Remove the specified product
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Return the product removed</returns>
        /// <response code="400">Product not found</response>
        /// <response code="200">Return a newly product</response>
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = _context.Product.Where(c => c.Id == id).FirstOrDefault();
            if (product is null) return NotFound(id);

            try
            {
                _context.Product.Remove(product);
                _context.SaveChanges();
                return Ok(product);
            } catch (Exception ex) {
                var test = ex;
                return BadRequest();
            }
        }
    }

}
