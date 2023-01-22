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

        /// <summary>
        /// DEBUG - List all the cart (use to know the id of a cart)
        /// </summary>
        /// <returns>Return the whole list of cart</returns>
        /// <response code="200">Returns the list of cart</response>
        [HttpGet]
        public ActionResult<List<Cart>> GetCart()
        {
            return Ok(_context.Cart);
        }

        /// <summary>
        /// List the products in the cart specified
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Return a list of products</returns>
        /// <response code="400">Cart not found</response>
        /// <response code="400">No product in this cart</response>
        /// <response code="400">Product not found</response>
        /// <response code="200">Returns the list of product in the cart</response>
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

        /// <summary>
        /// Add the specified product to the specified cart
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="productId"></param>
        /// <returns>Return the newly added product</returns>
        /// <response code="200">Returns a product</response>
        [HttpPost]
        public IActionResult AddProduct(int cartId, int productId)
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

        /// <summary>
        /// Remove the specified product to the specified cart
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        /// <response code="400">Cart not found</response>
        /// <response code="400">This product is not in this cart</response>
        /// <response code="200">Product has been deleted succesfully</response>
        [HttpDelete]
        public async Task<IActionResult> RemoveProduct(int cartId, int productId)
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