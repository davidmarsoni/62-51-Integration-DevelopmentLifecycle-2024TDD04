using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL;
using DAL.Models;
using DTO;
using WebApi.Mapper;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly RoomAccessContext _context;

        public UsersController(RoomAccessContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<List<UserDTO>>> GetUsers()
        {
            IEnumerable<User> users = await _context.Users.ToListAsync();
            List<UserDTO> result = new List<UserDTO>();
            if (users != null && users.Count() > 0) {
                foreach ( User user in users )
                {
                    result.Add(UserMapper.toDTO(user));
                }
            }
            return result;
        }

        // GET: api/Users/Username
        [HttpGet("Username/{username}")]
        public async Task<ActionResult<Boolean>> GetUsernameExist(string username)
        {
            IEnumerable<User> users = await _context.Users.ToListAsync();
            List<UserDTO> result = new List<UserDTO>();
            Boolean usernameExist = false;
            if (users != null && users.Count() > 0)
            {
                foreach (User user in users)
                {
                    if (user.Username == username)
                    {
                        usernameExist = true;
                        break;
                    }
                }
            }
            return usernameExist;
        }

        // GET: api/Users/Active
        [HttpGet("Active")]
        public async Task<ActionResult<List<UserDTO>>> GetUsersActive()
        {
            // get all users that are active
            IEnumerable<User> users = await _context.Users.Where(u => !u.IsDeleted).ToListAsync();
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

        // GET: api/Users/Group/5
        [HttpGet("Group/{groupId}")]
        public async Task<ActionResult<List<UserDTO>>> GetUsersByGroupId(int groupId)
        {
            // get all active users that are in the group
            var users = await _context.UserGroups
                .Where(ug => ug.GroupId == groupId && !ug.User.IsDeleted)
                .Select(ug => ug.User)
                .ToListAsync();

            List<UserDTO> result = new List<UserDTO>();
            if (users != null && users.Count > 0)
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
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            UserDTO userDTO = UserMapper.toDTO(user);

            return userDTO;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserDTO userDTO)
        {
            if (id != userDTO.Id)
            {
                return BadRequest();
            }

            User user;

            try
            {
                user = UserMapper.toDAL(userDTO);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostUser(UserDTO userDTO)
        {
            User user;

            try
            {
                user = UserMapper.toDAL(userDTO);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
