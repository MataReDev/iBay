using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClassLibrary;

namespace iBay.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly Context _context;

        public ProductController(Context context)
        {
            _context = context;
        }

        //GET
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct()
        {
            return await _context.Product.ToListAsync();
        }

        //GET id
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Product.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        //POST
        [HttpPost]
        public IActionResult Create(Product product)
        {
            if(product == null)
            {
                return BadRequest();
            }

            _context.Product.Add(product);
            _context.SaveChanges();
            return Ok();
        }

        //PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
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

            catch (DbUpdateException)
            {
                if (ProductExists(id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }

            }

            return Ok(product);
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }

        //DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = _context.Product.Where(c => c.Id == id).FirstOrDefault();
            if (product == null)
            {
                return NotFound(id);
            }

            try
            {
                _context.Product.Remove(product);
                _context.SaveChanges();
                return NoContent();
            }
            catch (Exception ex)
            {
                var test = ex;
                return BadRequest();
            }
        }
    }

}
