using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL;
using DAL.Models;
using DTO;
using WebApi.Mapper;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserGroupsController : ControllerBase
    {
        private readonly RoomAccessContext _context;

        public UserGroupsController(RoomAccessContext context)
        {
            _context = context;
        }

        private async Task<(User? user, ActionResult? error)> ValidateUserExists(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return (null, BadRequest("User does not exist."));
            if (user.IsDeleted) return (null, Forbid());
            return (user, null);
        }

        private async Task<(Group? group, ActionResult? error)> ValidateGroupExists(int groupId)
        {
            var group = await _context.Groups.FindAsync(groupId);
            if (group == null) return (null, BadRequest("Group does not exist."));
            if (group.IsDeleted) return (null, Forbid());
            return (group, null);
        }



        // GET: api/UserGroup

        [HttpGet("{id}")]
        public async Task<ActionResult<UserGroupDTO>> GetUserGroup(int id)
        {
            var userGroup = await _context.UserGroups.FindAsync(id);

            if (userGroup == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(userGroup.UserId);
            var group = await _context.Groups.FindAsync(userGroup.GroupId);

            UserGroupDTO userGroupDTO = UserGroupMapper.toDTO(userGroup, user, group);

            return userGroupDTO;
        }

        // GET: api/UserGroup/{groupId}/users
        [HttpGet("{groupId}/users")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsersInGroup(int groupId)
        {
            var userGroups = await _context.UserGroups
                .Where(ug => ug.GroupId == groupId)
                .ToListAsync();

            if (userGroups == null || userGroups.Count == 0)
            {
                return NotFound();
            }

            var users = new List<UserDTO>();
            foreach (var userGroup in userGroups)
            {
                var user = await _context.Users.FindAsync(userGroup.UserId);
                if (user != null)
                {
                    users.Add(UserMapper.toDTO(user));
                }
            }

            return users;
        }

        // POST: api/UserGroup
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserGroupDTO>> PostUserGroup(UserGroupDTO userGroupDTO)
        {
            if (userGroupDTO == null) return BadRequest();

            var (user, userError) = await ValidateUserExists(userGroupDTO.UserId);
            if (userError != null) return userError;

            var (group, groupError) = await ValidateGroupExists(userGroupDTO.GroupId);
            if (groupError != null) return groupError; 

            //check if the user is already in the group
            if(_context.UserGroups.Any(userGroup => userGroup.UserId == userGroupDTO.UserId && userGroup.GroupId == userGroupDTO.GroupId))
            {
                return Conflict("User is already in the group.");
            }

            UserGroup userGroup;
            try
            {
                userGroup = UserGroupMapper.toDAL(userGroupDTO);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }   
            
            _context.UserGroups.Add(userGroup);
            await _context.SaveChangesAsync();

            // Fetch user and group details for the response DTO
            UserGroupDTO resultDTO = UserGroupMapper.toDTO(userGroup, user, group);

            return CreatedAtAction("GetUserGroup", new { id = resultDTO.Id }, resultDTO);
        }

        // DELETE: api/UserGroup/{groupId}/{userId}
        [HttpDelete("{groupId}/{userId}")]
        public async Task<IActionResult> DeleteUserFromGroup(int userId, int groupId)
        {
            var userGroup = await _context.UserGroups
                .FirstOrDefaultAsync(userGroup => userGroup.UserId == userId && userGroup.GroupId == groupId);
            if (userGroup == null)
            {
                return NotFound();
            }

            _context.UserGroups.Remove(userGroup);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserGroupExists(int id)
        {
            return _context.UserGroups.Any(userGroup => userGroup.GroupId == id);
        }
    }
}
