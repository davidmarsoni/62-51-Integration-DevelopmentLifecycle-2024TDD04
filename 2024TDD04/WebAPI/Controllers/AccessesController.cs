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
    
        private async Task<(bool isValid, ActionResult? errorResponse)> ValidateGroupExists(int groupId)
        {
            var groupExists = await _context.Groups.AnyAsync(group => group.Id == groupId);
            if (!groupExists)
                return (false, NotFound());
            return (true, null);
        }
    
        private async Task<(bool isValid, ActionResult? errorResponse)> ValidateRoomExists(int roomId)
        {
            var roomExists = await _context.Rooms.AnyAsync(room => room.Id == roomId && !room.IsDeleted);
            if (!roomExists)
                return (false, NotFound());
            return (true, null);
        }

        private async Task<(bool isValid, ActionResult? errorResponse)> ValidateUserExists(int userId)
        {
            var userExists = await _context.Users.AnyAsync(user => user.Id == userId);
            if (!userExists)
                return (false, NotFound());
            return (true, null);
        }
    
        private async Task<IQueryable<Access>> GetUserAccessesQuery(int userId)
        {
            var userGroupIds = await _context.UserGroups
                .Where(userGroup => userGroup.UserId == userId)
                .Select(userGroup => userGroup.GroupId)
                .ToListAsync();
    
            return _context.Accesses.Where(access => userGroupIds.Contains(access.GroupId));
        }
    
        private async Task<List<RoomDTO>> GetRoomsByAccessesQuery(IQueryable<Access> accessQuery)
        {
            return await accessQuery
                .Join(_context.Rooms,
                    access => access.RoomId,
                    room => room.Id,
                    (access, room) => RoomMapper.toDTO(room))
                .ToListAsync();
        }
    
        [HttpGet("HasAccessGroup/{roomId}/{groupId}")]
        public async Task<ActionResult<bool>> HasAccessGroupAsync(int roomId, int groupId)
        {
            var (isValid, errorResponse) = await ValidateGroupExists(groupId);
            if (!isValid) return errorResponse!;

            var (isValidRoom, errorResponseRoom) = await ValidateRoomExists(roomId);
            if (!isValidRoom) return errorResponseRoom!;
    
            return await _context.Accesses.AnyAsync(access => 
                access.RoomId == roomId && access.GroupId == groupId);
        }
    
        [HttpGet("HasAccessUser/{roomId}/{userId}")]
        public async Task<ActionResult<bool>> HasAccessUserAsync(int roomId, int userId)
        {
            var (isValid, errorResponse) = await ValidateUserExists(userId);
            if (!isValid) return errorResponse!;

            var (isValidRoom, errorResponseRoom) = await ValidateRoomExists(roomId);
            if (!isValidRoom) return errorResponseRoom!;
    
            var accessQuery = await GetUserAccessesQuery(userId);
            return await accessQuery.AnyAsync(access => access.RoomId == roomId);
        }
    
        [HttpGet("GetAccessesByUserId/{userId}")]
        public async Task<ActionResult<IEnumerable<RoomDTO>>> GetAccessesByUserId(int userId)
        {
            var (isValid, errorResponse) = await ValidateUserExists(userId);
            if (!isValid) return errorResponse!;
    
            var accessQuery = await GetUserAccessesQuery(userId);
            var rooms = await GetRoomsByAccessesQuery(accessQuery);
            
            return rooms.Any() ? rooms : NoContent();
        }
    
        [HttpGet("GetAccessesByGroupId/{groupId}")]
        public async Task<ActionResult<IEnumerable<RoomDTO>>> GetAccessesByGroupId(int groupId)
        {
            if (!await _context.Groups.AnyAsync(group => group.Id == groupId))
                return NotFound();
    
            var accessQuery = _context.Accesses.Where(access => access.GroupId == groupId);
            return await GetRoomsByAccessesQuery(accessQuery);
        }
    
        [HttpPost("GrantAccess")]
        public async Task<ActionResult<bool>> GrantAccessAsync(AccessDTO accessDTO)
        {
            var (isValid, errorResponse) = await ValidateGroupExists(accessDTO.GroupId);
            if (!isValid) return errorResponse!;

            var (isValidRoom, errorResponseRoom) = await ValidateRoomExists(accessDTO.RoomId);
            if (!isValidRoom) return errorResponseRoom!;
    
            if (await _context.Accesses.AnyAsync(access => 
                access.RoomId == accessDTO.RoomId && access.GroupId == accessDTO.GroupId))
                return Conflict();
    
            _context.Accesses.Add(AccessMapper.toDAL(accessDTO));
            await _context.SaveChangesAsync();
            return true;
        }
    
        [HttpPost("RevokeAccess")]
        public async Task<ActionResult<bool>> RevokeAccessAsync(AccessDTO accessDTO)
        {
            var (isValid, errorResponse) = await ValidateGroupExists(accessDTO.GroupId);
            if (!isValid) return errorResponse!;
    
            var access = await _context.Accesses.FirstOrDefaultAsync(access => 
                access.RoomId == accessDTO.RoomId && access.GroupId == accessDTO.GroupId);
            
            if (access == null) return NotFound();
    
            _context.Accesses.Remove(access);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}