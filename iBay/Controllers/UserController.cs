using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClassLibrary;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using iBay.Tools;

namespace iBay.Controllers
{
    /// <summary>
    /// Classe User
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly Context _context;

        /// <summary>
        /// Contructeur user
        /// </summary>
        /// <param name="context"></param>
        public UsersController(Context context)
        {
            _context = context;
        }
        /// <summary>
        /// DEBUG - List all the users
        /// </summary>
        /// <returns>Return the whole list of user</returns>
        /// <response code="200">Return the lists of users</response>
        [HttpGet]
        public ActionResult<List<User>>GetUsers()
        {
            return  Ok(_context.User);
        }

        /// <summary>
        /// List a specific user
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Return the user specified</returns>
        /// <response code="400">User not found</response>
        /// <response code="200">Return a user</response>
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null) return BadRequest("User not found");

            return Ok(user);
        }

        /// <summary>
        /// Create a user
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Return the new user</returns>
        /// <response code="400">A user already exists</response>
        /// <response code="400">No content send</response>
        /// <response code="200">Return a user</response>
        [HttpPost]
        public IActionResult Create(User item)
        {
            if (item == null) return BadRequest("No content send");

            if (UserExists(item.Email)) return BadRequest("A user already exists");

            var user = new User();
            user.Email = item.Email.ToLower();
            user.Pseudo = item.Pseudo;
            user.Role = item.Role;
            user.Password = Password.hashPassword(item.Password);

            var cart = new Cart();
            cart.UserId = user.Id;

            _context.Cart.Add(cart);
            _context.User.Add(user);
            _context.SaveChanges();

            return Ok(user);
        }


        /// <summary>
        /// Update a specific user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns>Return the user updated</returns>
        /// <response code="400">User not found</response>
        /// <response code="400">Exeption</response>
        /// <response code="200">Return the user updated</response>
        [HttpPut]
        [Route("{id}")]
        [Authorize]
        public async Task<ActionResult> Update(int id, User user)
        {
            //if (id != user.Id)
            //    return BadRequest();
            var userExist = await _context.User.FindAsync(id);
            if (userExist is null) return BadRequest("User not found");

            try
            {
                userExist.Email = user.Email;
                userExist.Pseudo = user.Pseudo;
                userExist.Role = user.Role;
                userExist.Password = Password.hashPassword(user.Password);
                _context.User.Update(userExist);
                _context.SaveChanges();
                return Ok(userExist);
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        private bool UserExists(string email)
        {
            return _context.User.Any(e => e.Email == email);
        }

        /// <summary>
        /// Delete a specific user
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Return </returns>
        /// <response code="400">User not found</response>
        /// <response code="400">Exeption</response>
        /// <response code="200">Return the user deleted</response>
        // DELETE: api/Users/5
        [HttpDelete]
        [Route("{id}")]
        [Authorize]
        public IActionResult DeleteUser(int id)
        {
            var user =  _context.User.Where(c => c.Id == id).FirstOrDefault();
            if (user == null) return BadRequest("User not found");

            try {
                _context.User.Remove(user);
                _context.SaveChanges();
                return Ok(user);
            } catch (Exception ex) {
                return BadRequest(ex);
            }
        }
    }
}
