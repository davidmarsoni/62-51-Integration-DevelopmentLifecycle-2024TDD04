using Microsoft.AspNetCore.Mvc;
using DTO;
using DAL.Models;
using DAL;
using WebApi.Mapper;
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



        // POST: api/Room
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RoomDTO>> PostRoom(RoomDTO roomDTO)
        {
            Room room;
            if (roomDTO == null)
            {
                return BadRequest();
            }

            //check if the room is already in the DB
            if (_context.Rooms.Any(rm => rm.Name == roomDTO.Name || rm.RoomAbreviation == rm.RoomAbreviation)){
                return Conflict();
            }

            try
            {
                room = RoomMapper.toDAL(roomDTO);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }

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
    }
}
