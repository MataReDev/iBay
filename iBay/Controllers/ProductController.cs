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
            return Ok(product);
        }

        //PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            //if (id != product.Id)
            //{
            //    return BadRequest();
            //}

            var productExist = await _context.Product.FindAsync(id);
            if (productExist is null) return NotFound(id);

            Console.WriteLine(productExist);

            try
            {
                productExist.Name = product.Name;
                productExist.Image = product.Image;
                productExist.Price = product.Price;
                productExist.Available = product.Available;
                productExist.Added_time = product.Added_time;
                _context.Product.Update(productExist);
                _context.SaveChanges();
                return Ok(productExist);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }      

        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }

        //DELETE
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
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
                return Ok(product);
            }
            catch (Exception ex)
            {
                var test = ex;
                return BadRequest();
            }
        }
    }

}
