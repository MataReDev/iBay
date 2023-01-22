using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClassLibrary;

namespace iBay.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayementController : ControllerBase
    {
        private readonly Context _context;

        public PayementController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetPayementByUser(int userId)
        {
            var payementHistoryGET = _context.PaymentHistory.Where(u => u.UserId == userId).ToList();
            if (payementHistoryGET is null) return Ok("No product in this cart");

            return Ok(payementHistoryGET);
        }


        [HttpPost]
        [Route("pay")]
        public async Task<IActionResult> Pay(int cartId, float moneyUser)
        {
            var cart = await _context.Cart.FindAsync(cartId);
            if (cart is null) return BadRequest("Cart not found");

            var listProduct = _context.ProductCart.Where(u => u.CartId == cartId).ToList();
            if (listProduct is null) return BadRequest("No list of product found");

            float total = 0;
            foreach (ProductCart item in listProduct)
            {
                var product = await _context.Product.FindAsync(item.ProductId);
                if (product is null) return BadRequest("The product is the list doesn't exist");

                total += product.Price;
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
            _context.ProductCart.RemoveRange(_context.ProductCart.Where(u => u.CartId == cartId));
            _context.SaveChanges();

            return Ok($"Argent rendu : {moneyUser - total}");
        }
    }
}
