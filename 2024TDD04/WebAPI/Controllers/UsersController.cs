﻿using Microsoft.AspNetCore.Mvc;
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

        // GET: api/Users/Username
        [HttpGet("Username/{username}")]
        public async Task<ActionResult<Boolean>> UsernameExist(string username)
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
        private async Task<(User? user, ActionResult? error)> ValidateUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return (null, NotFound());
            if (user.IsDeleted) return (null, Forbid());
            return (user, null);
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
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(user => user.Id == id);

                if (existingUser == null)
                {
                    return NotFound();
                }

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
            if (UserExists(user.Id))
            {
                return Conflict();
            }

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

        private bool UserExists(int id) { 
            return _context.Users.Any(user => user.Id == id); 
        }
    }
}
