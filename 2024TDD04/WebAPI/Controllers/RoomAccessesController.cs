using DAL.Models;
using DAL;
using DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Mapper;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomAccessesController : ControllerBase
    {
        private readonly RoomAccessContext _context;
        public RoomAccessesController(RoomAccessContext context)
        {
            _context = context;
        }

        private async Task<(User? user, ActionResult? error)> ValidateUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return (null, NotFound());
            if (user.IsDeleted) return (null, Forbid());
            return (user, null);
        }

        // POST: api/RoomAccesses/Access/{roomId}/{userId}
        [HttpGet("Access/{roomId}/{userId}")]
        public async Task<ActionResult<RoomAccessDTO>> AccessRoom(int roomId, int userId)
        {
            // The goal of this method is to simulate the access to a room by a user
            // * The user is granted access if he is in a group that has access to the room
            // * The user is denied access if he is not in any group that has access to the room
            // * The user is denied access if he is not in any group
            // All is written in the logs, and the result is returned if the user has access or not

            // Check if the room exists and retrieve it
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null)
            {
                return NotFound();
            }

            var (user, error) = await ValidateUserAsync(userId);
            if (error != null) return error;

            // Get all the groups of the user
            var userGroups = await _context.UserGroups.Where(userGroup => userGroup.UserId == userId).ToListAsync();

            // If the user is not in any group, he doesn't have access
            if (userGroups.Count() == 0)
            {
                RoomAccessLog logNoGroup = new RoomAccessLog
                {
                    RoomId = roomId,
                    UserId = userId,
                    Timestamp = DateTime.Now,
                    Info = "User is not in any group, access denied.",
                };
                _context.RoomAccessLogs.Add(logNoGroup);
                _context.SaveChanges();
                return NoContent();
            }

            // Check if the user has access to the room
            bool? hasAccess = null;
            foreach (var userGroup in userGroups)
            {
                hasAccess = _context.Accesses.Any(access => access.RoomId == roomId && access.GroupId == userGroup.GroupId);
                if (hasAccess == true)
                {
                    break;
                }
            }

            RoomAccessLog log = new RoomAccessLog
            {
                RoomId = roomId,
                UserId = userId,
                Timestamp = DateTime.Now,
                Info = hasAccess != null ? "User was granted access." : "User was denied access, unsufficient rights.",
            };
            _context.RoomAccessLogs.Add(log);
            _context.SaveChanges();

            return hasAccess != null ? new RoomAccessDTO { RoomId = roomId, UserId = userId } : NoContent();
        }
    }
}
