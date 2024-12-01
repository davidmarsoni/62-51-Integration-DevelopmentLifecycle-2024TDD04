using DAL;
using DAL.Models;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Mapper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

        // GET: api/Accesses
        [HttpGet]
        public async Task<IEnumerable<AccessDTO>> GetAccessesAsync()
        {
            IEnumerable<Access> accesses = await _context.Accesses.ToListAsync();
            List<AccessDTO> result = new List<AccessDTO>();
            if (accesses != null && accesses.Any())
            {
                foreach (Access access in accesses)
                {
                    result.Add(AccessMapper.toDTO(access));
                }
            }
            return result;
        }

        // GET api/Accesses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AccessDTO>> GetAsync(int id)
        {
            var access = await _context.Accesses.FindAsync(id);

            if (access == null)
            {
                return NotFound();
            }

            AccessDTO accessDTO = AccessMapper.toDTO(access);

            return accessDTO;
        }

        // POST: api/Accesses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AccessDTO>> PostAccesses(AccessDTO accessDTO) 
        {
            Access access;

            if (accessDTO == null)
            {
                return BadRequest();
            }

            //check if the room exists
            if (!_context.Rooms.Any(u => u.Id == accessDTO.RoomId))
            {
                return BadRequest();
            }
            //check if the group exists
            if (!_context.Groups.Any(g => g.Id == accessDTO.GroupId))
            {
                return BadRequest();
            }
            //check if the access is already in the group
            if (_context.Accesses.Any(ug => ug.RoomId == accessDTO.RoomId && ug.GroupId == accessDTO.GroupId))
            {
                return Conflict();
            }

            try
            {
                access = AccessMapper.toDAL(accessDTO);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }

            _context.Accesses.Add(access);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAccess", new { id = access.Id }, access);
        }

        // PUT: api/Access/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccess(int id, AccessDTO accessDTO)
        {
            if (id != accessDTO.Id)
            {
                return BadRequest();
            }

            Access access;

            try
            {
                access = AccessMapper.toDAL(accessDTO);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }

            _context.Entry(access).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccessExists(id))
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

        // DELETE: api/Access/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccess(int id)
        {
            var access = await _context.Accesses.FindAsync(id);
            if (access == null)
            {
                return NotFound();
            }

            _context.Accesses.Remove(access);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AccessExists(int id)
        {
            return _context.Groups.Any(e => e.Id == id);
        }
    }
}
