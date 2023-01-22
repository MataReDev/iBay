using Microsoft.AspNetCore.Mvc;
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

            var listOfProductCart = _context.ProductCart.Where( u => u.CartId == id).ToList();
            if(listOfProductCart is null) return Ok("No product in this cart");

            List<Product> listOfProduct = new List<Product>();

            foreach (ProductCart item in listOfProductCart)
            {
                var product = await _context.Product.FindAsync(item.ProductId);
                if (product is null) return BadRequest("Product not found");

                listOfProduct.Add(product);
            }

            return Ok(listOfProduct);
        }

        //POST
        [HttpPost]
        public IActionResult Create(int cartId, int productId)
        {
            var productCart = new ProductCart()
            {
                CartId = cartId,
                ProductId = productId
            };

            _context.ProductCart.Add(productCart);
            _context.SaveChanges();
            return Ok(productCart);
        }

        //DELETE
        [HttpDelete]
        public async Task<IActionResult> Delete(int cartId, int productId)
        {
            var cartGET = await _context.Cart.FindAsync(cartId);
            if (cartGET is null) return BadRequest("Cart not found");

            var ProductCartGET = _context.ProductCart.Where(u => u.ProductId == productId && u.CartId == cartId).FirstOrDefault();
            if (ProductCartGET is null)  return BadRequest("This product is not in this cart");

            _context.ProductCart.Remove(ProductCartGET);
            _context.SaveChanges();

            return Ok("Product has been deleted succesfully");
        }
        
    }
}