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
        public async Task<ActionResult<bool>> HasAccessAsync(int roomId, int groupId)
        {
            if (!_context.Rooms.Any(r => r.Id == roomId) || !_context.Groups.Any(g => g.Id == groupId))
            {
                return BadRequest();
            }

            return await _context.Accesses.AnyAsync(a => a.RoomId == roomId && a.GroupId == groupId);;
        }

        // GET: api/Accesses/HasAccessUser/<roomId>/<userId>
        [HttpGet("HasAccessUser/{roomId}/{userId}")]
        public async Task<ActionResult<bool>> HasAccessUserAsync(int roomId, int userId)
        {
            if (!_context.Rooms.Any(r => r.Id == roomId) || !_context.Users.Any(u => u.Id == userId))
            {
                return BadRequest();
            }

            // Get all the groups of the user
            var userGroups = await _context.UserGroups.Where(ug => ug.UserId == userId).ToListAsync();

            // If the user is not in any group, he doesn't have access
            if (userGroups.Count == 0)
            {
                return false;
            }

            // Check if the user has access to the room
            return await _context.Accesses.AnyAsync(a => a.RoomId == roomId && userGroups.Any(ug => ug.GroupId == a.GroupId));;
        }

        // GET: api/Accesses/GetRoomAccessedByGroup/<groupId>
        [HttpGet("GetRoomAccessedByGroup/{groupId}")]
        public async Task<ActionResult<RoomDTO>> GetRoomAccessedByGroupAsync(int groupId)
        {
            if (!_context.Groups.Any(g => g.Id == groupId))
            {
                return BadRequest();
            }

            var room = await _context.Rooms.FirstOrDefaultAsync(r => _context.Accesses.Any(a => a.RoomId == r.Id && a.GroupId == groupId));

            if (room == null)
            {
                return NotFound();
            }

            RoomDTO roomDTO = RoomMapper.toDTO(room);

            return roomDTO;
        }

        // GET: api/Accesses/GetRoomAccessedByUser/<userId>
        [HttpGet("GetRoomAccessedByUser/{userId}")]
        public async Task<ActionResult<RoomDTO>> GetRoomAccessedByUserAsync(int userId)
        {
            if (!_context.Users.Any(u => u.Id == userId))
            {
                return BadRequest();
            }

            // Get all the groups of the user
            var userGroups = await _context.UserGroups.Where(ug => ug.UserId == userId).ToListAsync();

            // If the user is not in any group, he doesn't have access
            if (userGroups.Count == 0)
            {
                return NotFound();
            }

            var room = await _context.Rooms.FirstOrDefaultAsync(r => _context.Accesses.Any(a => a.RoomId == r.Id && userGroups.Any(ug => ug.GroupId == a.GroupId)));

            if (room == null)
            {
                return NotFound();
            }

            RoomDTO roomDTO = RoomMapper.toDTO(room);

            return roomDTO;
        }

        // POST: api/GrantAccess
        [HttpPost("GrantAccess")]
        public async Task<ActionResult<Boolean>> GrantAccessAsync(AccessDTO accessDTO)
        {
            if (!_context.Rooms.Any(r => r.Id == accessDTO.RoomId) || !_context.Groups.Any(g => g.Id == accessDTO.GroupId))
            {
                return BadRequest();
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
                return BadRequest();
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
