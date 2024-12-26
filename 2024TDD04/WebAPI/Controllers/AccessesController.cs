using DAL;
using DAL.Models;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Mapper;

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
            if (!_context.Rooms.Any(room => room.Id == roomId) || !_context.Groups.Any(group => group.Id == groupId))
            {
                return NotFound();
            }

            return await _context.Accesses.AnyAsync(access => access.RoomId == roomId && access.GroupId == groupId);
        }

        // GET: api/Accesses/HasAccessUser/<roomId>/<userId>
        [HttpGet("HasAccessUser/{roomId}/{userId}")]
        public async Task<ActionResult<bool>> HasAccessUserAsync(int roomId, int userId)
        {
            if (!_context.Rooms.Any(room => room.Id == roomId) || !_context.Users.Any(user => user.Id == userId))
            {
                return NotFound();
            }

            // Get all the groups of the user
            var userGroups = await _context.UserGroups.Where(userGroup => userGroup.UserId == userId).ToListAsync();

            // If the user is not in any group, he doesn't have access
            if (userGroups.Count == 0)
            {
                return false;
            }

            // Check all the groups of the user to see if he has access
            // Cycle through all the groups of the user to see if he has access
            foreach (var userGroup in userGroups)
            {
                if (await _context.Accesses.AnyAsync(access => access.RoomId == roomId && access.GroupId == userGroup.GroupId))
                {
                    return true;
                }
            }
            // If the user is not in any group that has access, return false
            return false;
        }

        // GET: api/Accesses/GetAccessesByUserId/<userId>
        [HttpGet("GetAccessesByUserId/{userId}")]
        public async Task<ActionResult<IEnumerable<RoomDTO>>> GetAccessesByUserId(int userId)
        {
            if (!_context.Users.Any(user => user.Id == userId))
            {
                return NotFound();
            }

            // Get all the groups of the user
            var userGroups = await _context.UserGroups.Where(userGroup => userGroup.UserId == userId).ToListAsync();

            // If the user is not in any group, he doesn't have access
            if (userGroups.Count == 0)
            {
                return NoContent();
            }

            List<RoomDTO> result = new List<RoomDTO>();
            // Cycle through all the groups of the user to see if he has access
            foreach (var userGroup in userGroups)
            {
                var accesses = await _context.Accesses.Where(access => access.GroupId == userGroup.GroupId).ToListAsync();
                if (accesses != null && accesses.Count() > 0)
                {
                    foreach (Access access in accesses)
                    {
                        Room room = await _context.Rooms.FindAsync(access.RoomId);
                        if (room != null)
                        {
                            result.Add(RoomMapper.toDTO(room));
                        }
                    }
                }
            }
            return result;
        }

        // GET: api/Accesses/GetAccessesByGroupId/<groupId>
        [HttpGet("GetAccessesByGroupId/{groupId}")]
        public async Task<ActionResult<IEnumerable<RoomDTO>>> GetAccessesByGroupId(int groupId)
        {
            if (!_context.Groups.Any(group => group.Id == groupId))
            {
                return NotFound();
            }

            List<RoomDTO> result = new List<RoomDTO>();
            var accesses = await _context.Accesses.Where(access => access.GroupId == groupId).ToListAsync();
            if (accesses != null && accesses.Count() > 0)
            {
                foreach (Access access in accesses)
                {
                    Room room = await _context.Rooms.FindAsync(access.RoomId);
                    if (room != null)
                    {
                        result.Add(RoomMapper.toDTO(room));
                    }
                }
            }
            return result;
        }

        // POST: api/GrantAccess
        [HttpPost("GrantAccess")]
        public async Task<ActionResult<Boolean>> GrantAccessAsync(AccessDTO accessDTO)
        {
            if (!_context.Rooms.Any(room => room.Id == accessDTO.RoomId) || !_context.Groups.Any(group => group.Id == accessDTO.GroupId))
            {
                return BadRequest();
            }

            // Check if the room is not deleted
            if (await _context.Rooms.AnyAsync(room => room.Id == accessDTO.RoomId && room.IsDeleted))
            {
                return NotFound();
            }

            // Check if the access already exists
            if (await _context.Accesses.AnyAsync(access => access.RoomId == accessDTO.RoomId && access.GroupId == accessDTO.GroupId))
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
            if (!_context.Rooms.Any(room => room.Id == accessDTO.RoomId) || !_context.Groups.Any(group => group.Id == accessDTO.GroupId))
            {
                return NotFound();
            }

            // Check if the room is not deleted
            if (await _context.Rooms.AnyAsync(room => room.Id == accessDTO.RoomId && room.IsDeleted))
            {
                return NotFound();
            }

            // Get the access
            Access storedAccess = await _context.Accesses.FirstOrDefaultAsync(access => access.RoomId == accessDTO.RoomId && access.GroupId == accessDTO.GroupId);

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
