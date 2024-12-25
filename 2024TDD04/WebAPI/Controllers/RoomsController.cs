using Microsoft.AspNetCore.Mvc;
using DTO;
using DAL.Models;
using DAL;
using Microsoft.EntityFrameworkCore;
using WebAPI.Mapper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly RoomAccessContext _context;

        public RoomsController(RoomAccessContext context)
        {
            _context = context;
        }

        // GET: api/Room
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomDTO>>> GetRooms()
        {
            IEnumerable<Room> rooms = await _context.Rooms.ToListAsync();
            List<RoomDTO> result = new List<RoomDTO>();
            if (rooms != null && rooms.Count() > 0)
            {
                foreach (Room room in rooms) {
                    result.Add(RoomMapper.toDTO(room));
                }
            }
            return result;
        }

        // GET: api/Room/Active
        [HttpGet("Active")]
        public async Task<ActionResult<IEnumerable<RoomDTO>>> GetRoomsActive()
        {
            // get all rooms that are active
            IEnumerable<Room> rooms = await _context.Rooms.Where(u => !u.IsDeleted).ToListAsync();
            List<RoomDTO> result = new List<RoomDTO>();
            if (rooms != null && rooms.Count() > 0)
            {
                foreach (Room room in rooms)
                {
                    result.Add(RoomMapper.toDTO(room));
                }
            }
            return result;
        }

        // GET: api/Room/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RoomDTO>> GetRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);

            if (room == null)
            {
                return NotFound();
            }

            RoomDTO roomDTO = RoomMapper.toDTO(room);

            return roomDTO;
        }

        // PUT: api/Room/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoom(int id, RoomDTO roomDTO)
        {
            if (id != roomDTO.Id)
            {
                return BadRequest();
            }

            Room room = RoomMapper.toDAL(roomDTO);

            var existingRoom = await _context.Rooms.FindAsync(id);
            if (existingRoom == null)
            {
                return NotFound();
            }
            _context.Entry(existingRoom).CurrentValues.SetValues(room);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomExists(id))
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

        // POST: api/Room
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RoomDTO>> PostRoom(RoomDTO roomDTO)
        {
            
            if (roomDTO == null)
            {
                return BadRequest();
            }

            //check if the room is already in the DB
            if (await _context.Rooms.AnyAsync(rm => rm.Name == roomDTO.Name || rm.RoomAbreviation == roomDTO.RoomAbreviation)){
                return Conflict();
            }

            Room room = RoomMapper.toDAL(roomDTO);

            _context.Rooms.Add(room);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRoom", new { id = room.Id }, room);
        }

        // DELETE: api/Room/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            room.IsDeleted = true;
            _context.Update(room);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Room/Name/{name}
        [HttpGet("Name/{name}")]
        public async Task<ActionResult<bool>> RoomNameExists(string name)
        {
            return await _context.Rooms.AnyAsync(rm => rm.Name == name);
        }

        // GET: api/Room/Abreviation/{abreviation}
        [HttpGet("Abreviation/{abreviation}")]
        public async Task<ActionResult<bool>> RoomAbreviationExists(string abreviation)
        {
            return await _context.Rooms.AnyAsync(rm => rm.RoomAbreviation == abreviation);
        }

        private bool RoomExists(int id)
        {
            return _context.Rooms.Any(e => e.Id == id);
        }
    }
}
