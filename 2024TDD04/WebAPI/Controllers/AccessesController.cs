using DAL;
using DAL.Models;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Mapper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccessesController : ControllerBase
    {
        private readonly RoomAccessContext _context;

        public AccessesController(RoomAccessContext context)
        {
            _context = context;
        }

        // GET: api/Accesses/HasAccessGroup/<roomId>/<groupId>
        [HttpGet("HasAccessGroup/{roomId}/{groupId}")]
        public async Task<ActionResult<bool>> HasAccessGroupAsync(int roomId, int groupId)
        {
            if (!_context.Rooms.Any(r => r.Id == roomId) || !_context.Groups.Any(g => g.Id == groupId))
            {
                return NotFound();
            }

            return await _context.Accesses.AnyAsync(a => a.RoomId == roomId && a.GroupId == groupId);
        }

        // GET: api/Accesses/HasAccessUser/<roomId>/<userId>
        [HttpGet("HasAccessUser/{roomId}/{userId}")]
        public async Task<ActionResult<bool>> HasAccessUserAsync(int roomId, int userId)
        {
            if (!_context.Rooms.Any(r => r.Id == roomId) || !_context.Users.Any(u => u.Id == userId))
            {
                return NotFound();
            }

            // Get all the groups of the user
            var userGroups = await _context.UserGroups.Where(ug => ug.UserId == userId).ToListAsync();

            // If the user is not in any group, he doesn't have access
            if (userGroups.Count == 0)
            {
                return false;
            }

            // Check all the groups of the user to see if he has access
            // Cycle through all the groups of the user to see if he has access
            foreach (var userGroup in userGroups)
            {
                if (await _context.Accesses.AnyAsync(a => a.RoomId == roomId && a.GroupId == userGroup.GroupId))
                {
                    return true;
                }
            }
            // If the user is not in any group that has access, return false
            return false;
        }

        // POST: api/GrantAccess
        [HttpPost("GrantAccess")]
        public async Task<ActionResult<Boolean>> GrantAccessAsync(AccessDTO accessDTO)
        {
            if (!_context.Rooms.Any(r => r.Id == accessDTO.RoomId) || !_context.Groups.Any(g => g.Id == accessDTO.GroupId))
            {
                return BadRequest();
            }

            // Check if the room is not deleted
            if (await _context.Rooms.AnyAsync(r => r.Id == accessDTO.RoomId && r.IsDeleted))
            {
                return NotFound();
            }

            // Check if the access already exists
            if (await _context.Accesses.AnyAsync(a => a.RoomId == accessDTO.RoomId && a.GroupId == accessDTO.GroupId))
            {
                return Conflict();
            }

            Access access = AccessMapper.toDAL(accessDTO);

            _context.Accesses.Add(access);
            await _context.SaveChangesAsync();

            return true;
        }

        // POST: api/RevokeAccess
        [HttpPost("RevokeAccess")]
        public async Task<ActionResult<Boolean>> RevokeAccessAsync(AccessDTO accessDTO)
        {
            if (!_context.Rooms.Any(r => r.Id == accessDTO.RoomId) || !_context.Groups.Any(g => g.Id == accessDTO.GroupId))
            {
                return NotFound();
            }

            // Check if the room is not deleted
            if (await _context.Rooms.AnyAsync(r => r.Id == accessDTO.RoomId && r.IsDeleted))
            {
                return NotFound();
            }

            // Get the access
            Access storedAccess = await _context.Accesses.FirstOrDefaultAsync(a => a.RoomId == accessDTO.RoomId && a.GroupId == accessDTO.GroupId);

            // If the access doesn't exist, return not found
            if (storedAccess == null)
            {
                return NotFound();
            }

            _context.Accesses.Remove(storedAccess);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
