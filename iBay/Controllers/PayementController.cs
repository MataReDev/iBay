﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClassLibrary;
using Microsoft.AspNetCore.Authorization;
using iBay.Tools;

namespace iBay.Controllers
{
    /// <summary>
    /// Payment controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PayementController : ControllerBase
    {
        private readonly Context _context;

        /// <summary>
        /// Constructeur Payment
        /// </summary>
        /// <param name="context"></param>
        public PayementController(Context context)
        {
            _context = context;
        }

        /// <summary>
        /// List all the payement made by the user precised
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Return the list of payement</returns>
        /// <response code="400">User not found</response>
        /// <response code="200">No product in this cart</response>
        /// <response code="200">Returns the historic of payment of the user</response>
        [HttpGet]
        public async Task<IActionResult> GetPayementByUser(int userId, [FromHeader] string authorization)
        {
            var user = await _context.User.FindAsync(userId);
            if (user is null) return BadRequest("User not found");

            string emailToken = JwtTokenTools.GetEmailFromToken(authorization);
            if (emailToken != user.Email)
            {
                return Unauthorized("You can't delete other user");
            }

            var payementHistoryGET = _context.PaymentHistory.Where(u => u.UserId == userId).ToList();
            if (payementHistoryGET is null) return Ok("No product in this cart");

            return Ok(payementHistoryGET);
        }

        /// <summary>
        /// Pay a cart by the id of the cart and the money you give
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="moneyUser"></param>
        /// <returns>Return a string with money giving back</returns>
        /// <response code="400">Cart not found</response>
        /// <response code="400">No list of product found</response>
        /// <response code="400">The product is the list doesn't exist</response>
        /// <response code="200">402 - Not enough money</response>
        /// <response code="200">Money back : (float) </response>
        [HttpPost]
        [Route("pay")]
        [Authorize]
        public async Task<IActionResult> Pay(int cartId, float moneyUser, [FromHeader] string authorization)
        {
            var cart = await _context.Cart.FindAsync(cartId);
            if (cart is null) return BadRequest("Cart not found");

            string idToken = JwtTokenTools.GetIdFromToken(authorization);
            if (idToken != cart.UserId.ToString())
            return Unauthorized("Vous n'avez pas le droit");

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

            return Ok($"Money back : {moneyUser - total}");
        }
    }
}
