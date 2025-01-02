using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL;
using DAL.Models;
using DTO;
using WebApi.Mapper;
using WebApi.Controllers.Interfaces;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase, IGroupsController
    {
        private readonly RoomAccessContext _context;

        public GroupsController(RoomAccessContext context)
        {
            _context = context;
        }

        private async Task<(Group? group, ActionResult? error)> ValidateGroupAsync(int id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null) return (null, NotFound());
            if (group.IsDeleted) return (null, Forbid());
            return (group, null);
        }

        private async Task<(bool? valid, ActionResult? error)> ValidateGroupnameAsync(string name, int? groupId)
        {
            bool exists = await _context.Groups.AnyAsync(group => group.Name == name && group.Id != groupId);
            if (exists) return (true, Conflict());
            return (false, null);
        }

        private async Task<(bool? valid, ActionResult? error)> ValidateGroupAcronymAsync(string acronym, int? groupId)
        {
            bool exists = await _context.Groups.AnyAsync(group => group.Acronym == acronym && group.Id != groupId);
            if (exists) return (true, Conflict());
            return (false, null);
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
            return GroupMapper.toDTO(group);
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
                var (existingGroup, error) = await ValidateGroupAsync(id);
                if (error != null) return error;

                var (validName, errorName) = await ValidateGroupnameAsync(groupDTO.Name, id);
                if (errorName != null) return errorName;

                var (validAcronym, errorAcronym) = await ValidateGroupAcronymAsync(groupDTO.Acronym, id);
                if (errorAcronym != null) return errorAcronym;

                Group group = GroupMapper.toDAL(groupDTO);
                _context.Entry(existingGroup).CurrentValues.SetValues(group);
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
            Group group = GroupMapper.toDAL(groupDTO);

            // check if group already exists
            if (_context.Groups.Any(group => group.Id == groupDTO.Id))
            {
                return Conflict("Group with the same ID already exists.");
            }

            var (_, errorName) = await ValidateGroupnameAsync(group.Name, null);
            if (errorName != null) return errorName;

            var (_, errorAcronym) = await ValidateGroupAcronymAsync(group.Acronym, null);
            if (errorAcronym != null) return errorAcronym;

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

        // GET: api/Groups/Name/{name}
        [HttpGet("Name/{name}")]
        public async Task<ActionResult<bool>> GroupNameExists(string name)
        {
            var (exists, _) = await ValidateGroupnameAsync(name, null);
            return exists;
        }

        // GET: api/Groups/Acronym/{acronym}
        [HttpGet("Acronym/{acronym}")]
        public async Task<ActionResult<bool>> GroupAcronymExists(string acronym)
        {
            var (exists, _) = await ValidateGroupAcronymAsync(acronym, null);
            return exists;
        }
    }
}
