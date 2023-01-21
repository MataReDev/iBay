using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClassLibrary;

namespace iBay.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly Context _context;

        public CartController(Context context)
        {
            _context = context;
        }
        [HttpGet]
        public ActionResult<List<Cart>> GetCart()
        {
            return Ok(_context.Cart);
        }

        //GET
        [HttpGet]
        [Route ("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var cart = await _context.Cart.FindAsync(id);

            //if(cart == null)
            //{
            //    return NotFound();
            //}

            return Ok(cart);
        }

        //POST
        [HttpPost]
        public async Task<IActionResult> Create(int cartId, int productId)
            
        {
            var cartGET = await _context.Cart.FindAsync(cartId);

            var productGET = await _context.Product.FindAsync(productId);

            if (cartGET is null)
            {
                return BadRequest("Cart not found");
            }

            var productCart = new ProductCart()
            {
                cart = cartGET, product = productGET
            };

            _context.ProductCart.Add(productCart);
            _context.SaveChanges();
            return Ok(productCart);
        }

        [HttpPost]
        [Route("pay")]
        public async Task<IActionResult> Pay(float moneyUser, int cartId)
        {
            var cart = await _context.Cart.FindAsync(cartId);

            if (cart == null)

            {
                return BadRequest();
            }

            var listProduct = _context.ProductCart.Where(u => u.cart == cart);

            float total = 0;
            foreach (var item in listProduct)

            {
                total += item.product.price;
            }

            if (moneyUser < total)
            {
                return NoContent();
            }

            return Ok(moneyUser - total);

        }

        //DELETE
        [HttpDelete]
        public async Task<IActionResult> Delete(int cartId, int productId)
        {

            var cartGET = await _context.Cart.FindAsync(cartId);

            var productGET = await _context.Product.FindAsync(productId);

            if (cartGET is null)
            {
                return BadRequest("Cart not found");
            }

            var ProductCartGET = _context.ProductCart.Where(u => u.product == productGET && u.cart == cartGET).FirstOrDefault();

            _context.ProductCart.Remove(ProductCartGET);
            _context.SaveChanges();

            return Ok(ProductCartGET);
        }
        
    }
}