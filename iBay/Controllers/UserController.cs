using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClassLibrary;

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
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (id != User.Identity.Name)
            {
                return Unauthorized();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var deleteUserResult = await _userManager.DeleteAsync(user);

            if (!deleteUserResult.Succeeded)
            {
                return BadRequest(deleteUserResult.Errors);
            }

            return NoContent();
        }
    }
}
