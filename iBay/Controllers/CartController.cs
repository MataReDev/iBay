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
            
            if (cart is null) return BadRequest("Cart not found");

            var listOfProductCart = _context.ProductCart.Where( u => u.cart.Id == id).ToList();

            if(listOfProductCart is null) return Ok("No product in this cart");

            List<Product> listOfProduct = new List<Product>();

            foreach (ProductCart item in listOfProductCart)
            {
                var product = await _context.Product.FindAsync(item.productId);

                if (product is null) return BadRequest("Product not found");

                listOfProduct.Add(product);
            }

            return Ok(listOfProduct);
        }

        //POST
        [HttpPost]
        public async Task<IActionResult> Create(int cartId, int productId)
            
        {
            var cartGET = await _context.Cart.FindAsync(cartId);

            if (cartGET is null) return BadRequest("Cart not found");

            var productCart = new ProductCart()
            {
                cart = cartGET, productId = productId
            };

            _context.ProductCart.Add(productCart);
            _context.SaveChanges();
            return Ok(productCart);
        }

        [HttpPost]
        [Route("pay")]
        public async Task<IActionResult> Pay(int cartId, float moneyUser)
        {
            var cart = await _context.Cart.FindAsync(cartId);

            if (cart is null) return BadRequest("Cart not found");

            var listProduct = _context.ProductCart.Where(u => u.cart == cart).ToList();

            if (listProduct is null) return BadRequest("No list of product found");

            float total = 0;
            foreach (ProductCart item in listProduct)

            {
                var product = await _context.Product.FindAsync(item.productId);

                total += product.price;
            }

            if (moneyUser < total)
            {
                return Ok("402 - Not enough money");
            }

            PaymentHistory paymentHistory = new PaymentHistory()
            {
                UserId = cart.UserId,
                Amount = total
            };

            _context.PaymentHistory.Add(paymentHistory);
            _context.ProductCart.RemoveRange( _context.ProductCart.Where(u => u.cart == cart));
            _context.SaveChanges();

            return Ok(moneyUser - total);

        }

        //DELETE
        [HttpDelete]
        public async Task<IActionResult> Delete(int cartId, int productId)
        {

            var cartGET = await _context.Cart.FindAsync(cartId);

            if (cartGET is null) return BadRequest("Cart not found");

            var ProductCartGET = _context.ProductCart.Where(u => u.productId == productId && u.cart == cartGET).FirstOrDefault();

            if (ProductCartGET is null)  return BadRequest("This product is not in this cart");

            _context.ProductCart.Remove(ProductCartGET);
            _context.SaveChanges();

            return Ok("Product has been deleted succesfully");
        }
        
    }
}