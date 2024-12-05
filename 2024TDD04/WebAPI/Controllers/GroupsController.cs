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

        // GET: api/Groups/Name/{name}
        [HttpGet("Name/{name}")]
        public async Task<ActionResult<bool>> GroupNameExists(string name)
        {
            bool exists = await _context.Groups.AnyAsync(g => g.Name == name);
            return exists;
        }

        // GET: api/Groups/Acronym/{acronym}
        [HttpGet("Acronym/{acronym}")]
        public async Task<ActionResult<bool>> GroupAcronymExists(string acronym)
        {
            bool exists = await _context.Groups.AnyAsync(g => g.Acronym == acronym);
            return exists;
        }

        // PUT: api/Groups/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroup(int id, GroupDTO groupDTO)
        {
            if (id != groupDTO.Id)
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

            group.IsDeleted = true;
            _context.Groups.Update(group);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GroupExists(int id)
        {
            return _context.Groups.Any(e => e.Id == id);
        }
    }
}
