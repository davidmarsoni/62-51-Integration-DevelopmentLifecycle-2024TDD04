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
        private readonly RoomAccessContext _context;

        public GroupsController(RoomAccessContext context)
        {
            _context = context;
        }

        // GET: api/Groups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupDTO>>> GetGroups()
        {
            IEnumerable<Group> groups = await _context.Groups.ToListAsync();
            List<GroupDTO> result = new List<GroupDTO>();
            if (groups != null && groups.Any())
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
        public async Task<ActionResult<IEnumerable<GroupDTO>>> GetGroupsActive()
        {
            // get all groups that are active
            IEnumerable<Group> groups = await _context.Groups.Where(user => !user.IsDeleted).ToListAsync();
            List<GroupDTO> result = new List<GroupDTO>();
            if (groups != null && groups.Any())
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
            var (group, error) = await ValidateGroupAsync(id);
            if (error != null) return error;
            if (group == null)
            {
                return NotFound();
            }
            return GroupMapper.toDTO(group);
        }

        // GET: api/Groups/User/5
        [HttpGet("User/{userId}")]
        public async Task<ActionResult<IEnumerable<GroupDTO>>> GetGroupsByUserId(int userId)
        {
            // get all groups that the user is in
            IEnumerable<Group> groups = await _context.Groups.Where(group => group.Users.Any(user => user.Id == userId)).ToListAsync();
            List<GroupDTO> result = new List<GroupDTO>();
            if (groups != null && groups.Any())
            {
                foreach (Group group in groups)
                {
                    result.Add(GroupMapper.toDTO(group));
                }
            }
            return result;
        }

        // GET: api/Groups/Name/{name}
        [HttpGet("Name/{name}")]
        public async Task<ActionResult<bool>> GroupNameExists(string name)
        {
            bool exists = await _context.Groups.AnyAsync(group => group.Name == name);
            return exists;
        }

        // GET: api/Groups/Acronym/{acronym}
        [HttpGet("Acronym/{acronym}")]
        public async Task<ActionResult<bool>> GroupAcronymExists(string acronym)
        {
            bool exists = await _context.Groups.AnyAsync(group => group.Acronym == acronym);
            return exists;
        }

        // PUT: api/Groups/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroup(int id, GroupDTO groupDTO)
        {
            if (groupDTO == null || id != groupDTO.Id)
            {
                return BadRequest("Invalid group data or ID mismatch");
            }
        
            try
            {
                var existingGroup = await _context.Groups
                    .AsNoTracking()
                    .FirstOrDefaultAsync(group => group.Id == id);
        
                if (existingGroup == null)
                {
                    return NotFound($"Group with ID {id} not found");
                }
        
                var updatedGroup = GroupMapper.toDAL(groupDTO);
                _context.Groups.Update(updatedGroup);
                await _context.SaveChangesAsync();
        
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest("Invalid group data : " + ex.Message);
            }
        }

        // POST: api/Groups
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
            var (group, error) = await ValidateGroupAsync(id);
            if (error != null) return error;

            group.IsDeleted = true;
            _context.Groups.Update(group);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<(Group? group, ActionResult? error)> ValidateGroupAsync(int id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null) return (null, NotFound());
            if (group.IsDeleted) return (null, Forbid());
            return (group, null);
        }
    }
}
