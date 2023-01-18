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

        //GET
        [HttpGet]
        public async Task<IActionResult> Get(int cartId)
        {
            var cart = await _context.Cart.FindAsync(cartId);

            if(cart == null)
            {
                return NotFound();
            }

            return Ok(cart.ListOfProducts);
        }

        //POST
        [HttpPost]
        public async Task<IActionResult> Create(Product product, int cartId)
            
        {
            if (product == null)
            {
                return BadRequest();
            }

            var cart = await _context.Cart.FindAsync(cartId);

            if (cart == null)
            {
                return BadRequest();
            }

            cart.ListOfProducts.Add(product);
            _context.Cart.Update(cart);
            _context.SaveChanges();
            return Ok();
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

            float total = 0;
            foreach (var item in cart.ListOfProducts)

            {
                total += item.price;
            }

            if (moneyUser < total)
            {
                return NoContent();
            }

            return Ok(moneyUser - total);

        }

        //DELETE
        [HttpDelete]
        public async Task<IActionResult> Delete(Product product, int cartId)
        {
            if(product== null)
            {
                return BadRequest();
            }

            var cart = await _context.Cart.FindAsync(cartId);

            if(cart == null) 
            { 
                return BadRequest(); 
            }

            cart.ListOfProducts.Remove(product);
            _context.Cart.Update(cart);
            _context.SaveChanges();
            return Ok();

        }
        
    }
}