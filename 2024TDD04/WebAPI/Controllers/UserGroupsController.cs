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

        // GET: api/UserGroup
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserGroupDTO>>> GetUserGroups()
        {
            IEnumerable<UserGroup> userGroups = await _context.UserGroups.ToListAsync();
            List<UserGroupDTO> result = new List<UserGroupDTO>();
            if (userGroups != null && userGroups.Count() > 0)
            {
                foreach (UserGroup userGroup in userGroups)
                {
                    // get the user associated with the userGroup
                    User user = await _context.Users.FindAsync(userGroup.UserId);
                    // get the group associated with the userGroup
                    Group group = await _context.Groups.FindAsync(userGroup.GroupId);
                    result.Add(UserGroupMapper.toDTO(userGroup, user, group));
                }
            }
            return result;
        }

        // GET: api/UserGroup/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserGroupDTO>> GetUserGroup(int id)
        {
            var userGroup = await _context.UserGroups.FindAsync(id);

            if (userGroup == null)
            {
                return NotFound();
            }
            User user = await _context.Users.FindAsync(userGroup.UserId);
            Group group = await _context.Groups.FindAsync(userGroup.GroupId);

            UserGroupDTO userGoupDTO = UserGroupMapper.toDTO(userGroup, user, group);

            return userGoupDTO;
        }

        

        // POST: api/UserGroup
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserGroup>> PostUserGroup(UserGroupDTO userGroupDTO)
        {
            UserGroup userGroup;
            if (userGroupDTO == null)
            {
                return BadRequest();
            }
            
            //check if the user exists
            if(!_context.Users.Any(u => u.Id == userGroupDTO.UserId))
            {
                return BadRequest();
            }
            //check if the group exists
            if(!_context.Groups.Any(g => g.Id == userGroupDTO.GroupId))
            {
                return BadRequest();
            }   
            //check if the user is already in the group
            if(_context.UserGroups.Any(ug => ug.UserId == userGroupDTO.UserId && ug.GroupId == userGroupDTO.GroupId))
            {
                return Conflict();
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

            return CreatedAtAction("GetUserGroup", new { id = userGroup.GroupId }, userGroup);
        }

        // DELETE: api/UserGroup/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserGroup(int id)
        {
            var userGroup = await _context.UserGroups.FindAsync(id);
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
