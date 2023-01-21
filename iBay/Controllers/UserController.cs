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
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly Context _context;


        public UsersController(Context context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public ActionResult<List<User>>GetUsers()
        {
            return  Ok(_context.User);
        }

        // GET: api/Users/5
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        //POST Create
        [HttpPost]
        public IActionResult Create(User item)
        {
            
            if (item == null)
            {
                return BadRequest();
            }

            var user = new User();
            user.Email = item.Email;
            user.Pseudo = item.Pseudo;
            user.Role = item.Role;
            user.Password = Password.hashPassword(item.Password);

            var cart = new Cart();
            cart.User = user;

            var token = "";

            _context.Cart.Add(cart);
            _context.User.Add(user);
            _context.SaveChanges();
            return Ok(user);
        }

        //POST LOGIN
        [HttpPost]
        [Route("login")]
        public IActionResult Login(string email, string password)
        {
            String hashPassword = Password.hashPassword(password);
            var dbUser = _context.User.Where( u => u.Email == email && u.Password == hashPassword).FirstOrDefault();

            if (dbUser == null)
            {
                return BadRequest("Email or Password is incorrect");
            }

            var token = "token";
            return Ok(token);
        }


        //PUT: api/Users/5
        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> Update(int id, User user)
        {
            //if (id != user.Id)
            //    return BadRequest();
            var userExist = await _context.User.FindAsync(id);
            if (userExist is null) return NotFound(id);

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

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }

        // DELETE: api/Users/5
        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user =  _context.User.Where(c => c.Id == id).FirstOrDefault();
            if (user == null)
            {
                return NotFound(id);
            }

            try
            {
                _context.User.Remove(user);
                _context.SaveChanges();
                return Ok(user);
            }
            catch (Exception ex)
            {
                var test = ex;
                return BadRequest();
            }
        }
    }
}
