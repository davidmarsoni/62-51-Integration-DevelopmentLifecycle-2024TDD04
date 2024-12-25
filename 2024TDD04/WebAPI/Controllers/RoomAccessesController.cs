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

        // POST: api/RoomAccesses/Access
        [HttpPost("Access")]
        public async Task<ActionResult<RoomAccessDTO>> AccessAsync(RoomAccessDTO roomAccessDTO)
        {
            // The goal of this method is to simulate the access to a room by a user
            // * The user is granted access if he is in a group that has access to the room
            // * The user is denied access if he is not in any group that has access to the room
            // * The user is denied access if he is not in any group
            // All is written in the logs, and the result is returned if the user has access or not

            // Check if the room exists and retrieve it
            var room = await _context.Rooms.FindAsync(roomAccessDTO.RoomId);
            if (room == null)
            {
                return NotFound();
            }

            // Check if the user is not deleted
            var user = await _context.Users.FindAsync(roomAccessDTO.UserId);
            // If the user is deleted, return a 403 Forbidden, or a 404 Not Found if the user does not exist
            if (user == null || user.IsDeleted)
            {
                return user == null ? NotFound() : Forbid();
            }

            // Get all the groups of the user
            var userGroups = await _context.UserGroups.Where(ug => ug.UserId == roomAccessDTO.UserId).ToListAsync();

            // If the user is not in any group, he doesn't have access
            if (userGroups.Count() == 0)
            {
                RoomAccessLog logNoGroup = new RoomAccessLog
                {
                    RoomId = roomAccessDTO.RoomId,
                    Room = room.Name,
                    UserId = roomAccessDTO.UserId,
                    Timestamp = DateTime.Now,
                    Info = "User is not in any group, access denied.",
                };
                _context.RoomAccessLogs.Add(logNoGroup);
                return NoContent();
            }

            // Check if the user has access to the room
            bool? hasAccess = null;
            foreach (var userGroup in userGroups)
            {
                hasAccess = _context.Accesses.Any(a => a.RoomId == roomAccessDTO.RoomId && a.GroupId == userGroup.GroupId);
                if (hasAccess == true)
                {
                    break;
                }
            }

            RoomAccessLog log = new RoomAccessLog
            {
                RoomId = roomAccessDTO.RoomId,
                Room = room.Name,
                UserId = roomAccessDTO.UserId,
                Timestamp = DateTime.Now,
                Info = hasAccess != null ? "User was granted access." : "User was denied access, unsufficient rights.",
            };
            _context.RoomAccessLogs.Add(log);

            return hasAccess != null ? roomAccessDTO : NoContent();
        }
    }
}
