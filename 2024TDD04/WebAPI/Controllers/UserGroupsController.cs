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
            UserGroup userGroup;
            if (userGroupDTO == null)
            {
                return BadRequest();
            }
            
            //check if the user exists
            var user = await _context.Users.FindAsync(userGroupDTO.UserId);
            if (user == null)
            {
                return BadRequest("User does not exist.");
            }
            //check if the group exists
            var group = await _context.Groups.FindAsync(userGroupDTO.GroupId);
            if (group == null)
            {
                return BadRequest("Group does not exist.");
            }   
            //check if the user is already in the group
            if(_context.UserGroups.Any(ug => ug.UserId == userGroupDTO.UserId && ug.GroupId == userGroupDTO.GroupId))
            {
                return Conflict("User is already in the group.");
            }

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

            return CreatedAtAction("GetUserGroup", new { id = userGroup.GroupId }, resultDTO);
        }

        // DELETE: api/UserGroup/{groupId}/{userId}
        [HttpDelete("{groupId}/{userId}")]
        public async Task<IActionResult> DeleteUserFromGroup(int userId, int groupId)
        {
            var userGroup = await _context.UserGroups
                .FirstOrDefaultAsync(ug => ug.UserId == userId && ug.GroupId == groupId);
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
            return _context.UserGroups.Any(e => e.GroupId == id);
        }
    }
}
