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

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly RoomRoom_Context _context;

        public GroupsController(RoomRoom_Context context)
        {
            _context = context;
        }

        // GET: api/Groups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupDTO>>> GetGroups()
        {
            IEnumerable<Group> groups = await _context.Groups.ToListAsync();
            List<GroupDTO> result = new List<GroupDTO>();
            if (groups != null && groups.Count() > 0)
            {
                foreach (Group group in groups)
                {
                    result.Add(GroupMapper.toDTO(group));
                }
            }
            return result;
        }

        // GET: api/Groups/Active
        [HttpGet("Active")]
        public async Task<ActionResult<List<GroupDTO>>> GetUsersActive()
        {
            // get all groups that are active
            IEnumerable<Group> groups = await _context.Groups.Where(u => !u.IsDeleted).ToListAsync();
            List<GroupDTO> result = new List<GroupDTO>();
            if (groups != null && groups.Count() > 0)
            {
                foreach (Group group in groups)
                {
                    result.Add(GroupMapper.toDTO(group));
                }
            }
            return result;
        }

        // GET: api/Groups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GroupDTO>> GetGroup(int id)
        {
            var group = await _context.Groups.FindAsync(id);

            if (group == null)
            {
                return NotFound();
            }

            var groupDTO = GroupMapper.toDTO(group);

            return groupDTO;
        }

        // GET: api/Groups/User/5
        [HttpGet("User/{userId}")]
        public async Task<ActionResult<List<GroupDTO>>> GetGroupsByUserId(int userId)
        {
            // get all groups that the user is in
            IEnumerable<Group> groups = await _context.Groups.Where(g => g.Users.Any(u => u.Id == userId)).ToListAsync();
            List<GroupDTO> result = new List<GroupDTO>();
            if (groups != null && groups.Count() > 0)
            {
                foreach (Group group in groups)
                {
                    result.Add(GroupMapper.toDTO(group));
                }
            }
            return result;
        }

        // PUT: api/Groups/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroup(int id, GroupDTO groupDTO)
        {
            if (id != groupDTO.GroupId)
            {
                return BadRequest();
            }

            Group group;

            try
            {
                group = GroupMapper.toDAL(groupDTO);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }

            _context.Entry(group).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Groups
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GroupDTO>> PostGroup(GroupDTO groupDTO)
        {
            Group group;

            try
            {
                group = GroupMapper.toDAL(groupDTO);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }

            _context.Groups.Add(group);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGroup", new { id = group.Id }, group);
        }

        // DELETE: api/Groups/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null)
            {
                return NotFound();
            }

            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GroupExists(int id)
        {
            return _context.Groups.Any(e => e.Id == id);
        }
    }
}
