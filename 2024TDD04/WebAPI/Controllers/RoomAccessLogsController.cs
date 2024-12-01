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
    public class RoomAccessLogsController : ControllerBase
    {
        private readonly RoomAccessContext _context;

        public RoomAccessLogsController(RoomAccessContext context)
        {
            _context = context;
        }

        // GET: api/RoomAccessLogs
        [HttpGet]
        public async Task<IEnumerable<RoomAccessLogDTO>> GetRoomAccessLogsAsync()
        {
            IEnumerable<RoomAccessLog> roomAccessLogs = await _context.RoomAccessLogs.ToListAsync();
            List<RoomAccessLogDTO> result = new List<RoomAccessLogDTO>();
            if (roomAccessLogs != null && roomAccessLogs.Any())
            {
                foreach (RoomAccessLog roomAccessLog in roomAccessLogs)
                {
                    result.Add(RoomAccessLogMapper.toDTO(roomAccessLog));
                }
            }
            return result;
        }

        // GET api/RoomAccessLogs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RoomAccessLogDTO>> GetRoomAccessLog(int id)
        {
            var roomAccessLog = await _context.RoomAccessLogs.FindAsync(id);

            if (roomAccessLog == null)
            {
                return NotFound();
            }

            RoomAccessLogDTO roomAccessLogDTO = RoomAccessLogMapper.toDTO(roomAccessLog);

            return roomAccessLogDTO;
        }

        // POST: api/RoomAccessLogs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RoomAccessLogDTO>> PostRoomAccessLogs(RoomAccessLogDTO roomAccessLogDTO) 
        {
            RoomAccessLog roomAccessLog;

            if (roomAccessLogDTO == null)
            {
                return BadRequest();
            }

            //check if the room exists
            if (!_context.Rooms.Any(u => u.Id == roomAccessLogDTO.RoomId))
            {
                return BadRequest();
            }
            //check if the group exists
            if (!_context.Groups.Any(g => g.Id == roomAccessLogDTO.GroupId))
            {
                return BadRequest();
            }
            //check if the user exists
            if (!_context.Users.Any(u => u.Id == roomAccessLogDTO.UserId))
            {
                return BadRequest();
            }
            //check if the room access log is already in the group
            if (_context.Accesses.Any(ug => ug.RoomId == roomAccessLogDTO.RoomId && ug.GroupId == roomAccessLogDTO.GroupId))
            {
                return Conflict();
            }

            try
            {
                roomAccessLog = RoomAccessLogMapper.toDAL(roomAccessLogDTO);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }

            _context.RoomAccessLogs.Add(roomAccessLog);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAccess", new { id = roomAccessLog.Id }, roomAccessLog);
        }

        // PUT: api/RoomAccessLog/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoomAccessLog(int id, RoomAccessLogDTO roomAccessLogDTO)
        {
            if (id != roomAccessLogDTO.Id)
            {
                return BadRequest();
            }

            RoomAccessLog roomAccessLog;

            try
            {
                roomAccessLog = RoomAccessLogMapper.toDAL(roomAccessLogDTO);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }

            _context.Entry(roomAccessLog).State = EntityState.Modified;

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

        // DELETE: api/RoomAccessLog/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoomAccessLog(int id)
        {
            var roomAccessLog = await _context.RoomAccessLogs.FindAsync(id);
            if (roomAccessLog == null)
            {
                return NotFound();
            }

            _context.RoomAccessLogs.Remove(roomAccessLog);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AccessExists(int id)
        {
            return _context.Groups.Any(e => e.Id == id);
        }
    }
}
