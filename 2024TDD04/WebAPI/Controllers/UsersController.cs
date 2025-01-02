using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL;
using DAL.Models;
using DTO;
using WebApi.Mapper;
using WebApi.Controllers.Interfaces;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase, IUsersController
    {
        private readonly RoomAccessContext _context;

        public UsersController(RoomAccessContext context)
        {
            _context = context;
        }

        private async Task<(User? user, ActionResult? error)> ValidateUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return (null, NotFound());
            if (user.IsDeleted) return (null, Forbid());
            return (user, null);
        }

        private async Task<(bool? valid, ActionResult? error)> ValidateUsernameAsync(string username, int? userId)
        {
            bool exists = await _context.Users.AnyAsync(user => user.Username == username && user.Id != userId);
            if (exists) return (true, Conflict());
            return (false, null);
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            IEnumerable<User> users = await _context.Users.ToListAsync();
            List<UserDTO> result = new List<UserDTO>();
            if (users != null && users.Count() > 0) {
                foreach ( User user in users )
                {
                    if (user != null)
                    {
                        result.Add(UserMapper.toDTO(user)); 
                    }
                }
            }
            return result;
        }

        // GET: api/Users/Active
        [HttpGet("Active")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsersActive()
        {
            // get all users that are active
            IEnumerable<User> users = await _context.Users.Where(user => !user.IsDeleted).ToListAsync();
            List<UserDTO> result = new List<UserDTO>();
            if (users != null && users.Count() > 0)
            {
                foreach (User user in users)
                {
                    result.Add(UserMapper.toDTO(user));
                }
            }
            return result;
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            var (user, error) = await ValidateUserAsync(id);
            if (error != null) return error;
            return UserMapper.toDTO(user);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserDTO userDTO)
        {
            if (userDTO == null || id != userDTO.Id)
            {
                return BadRequest("Invalid group data or ID mismatch");
            }

            try
            {
                var (existingUser, error) = await ValidateUserAsync(id);
                if (error != null) return error;

                var (validUsername, errorUsername) = await ValidateUsernameAsync(userDTO.Username, id);
                if (errorUsername != null) return errorUsername;

                User user = UserMapper.toDAL(userDTO);
                _context.Entry(existingUser).CurrentValues.SetValues(user);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest("Invalid group data : " + ex.Message);
            }
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostUser(UserDTO userDTO)
        {
            User user = UserMapper.toDAL(userDTO);

            // check if user already exists
            if (_context.Users.Any(user => user.Id == userDTO.Id))
            {
                return Conflict("User with the same ID already exists.");
            }

            var (validUsername, errorUsername) = await ValidateUsernameAsync(user.Username, null);
            if (errorUsername != null) return errorUsername;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var (user, error) = await ValidateUserAsync(id);
            if (error != null) return error;
            user.IsDeleted = true;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Users/Username
        [HttpGet("Username/{username}")]
        public async Task<ActionResult<Boolean>> UsernameExist(string username)
        {
            var (exists, _) = await ValidateUsernameAsync(username, null);
            return exists;
        }
    }
}
