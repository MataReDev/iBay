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
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly Context _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UsersController(Context context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.User.ToListAsync();
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
            user.Id = item.Id;
            user.Email = item.Email;
            user.Pseudo = item.Pseudo;
            user.Role = item.Role;
            user.Password = Password.hashPassword(item.Password);

            var token = "";

            _context.User.Add(user);
            _context.SaveChanges();
            return Ok(token);
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

            var token = "";
            return Ok(token);
        }


        //PUT: api/Users/5
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }

            catch (DbUpdateException)
            {
                if (UserExists(id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Ok(user);
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }

        // DELETE: api/Users/5
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
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
                return NoContent();
            }
            catch (Exception ex)
            {
                var test = ex;
                return BadRequest();
            }
        }
    }
}
