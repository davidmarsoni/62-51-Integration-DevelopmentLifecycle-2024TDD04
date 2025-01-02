using Microsoft.AspNetCore.Mvc;
using DTO;
using DAL.Models;
using DAL;
using Microsoft.EntityFrameworkCore;
using WebAPI.Mapper;
using WebApi.Mapper;
using Microsoft.IdentityModel.Tokens;
using WebApi.Controllers.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase, IRoomsController
    {
        private readonly RoomAccessContext _context;

        public RoomsController(RoomAccessContext context)
        {
            _context = context;
        }

        private async Task<(Room? room, ActionResult? error)> ValidateRoomAsync(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return (null, NotFound());
            if (room.IsDeleted) return (null, Forbid());
            return (room, null);
        }

        private async Task<(bool exists, ActionResult? errorResponse)> ValidateRoomNameAsync(string name, int? roomId)
        {
            bool exists = await _context.Rooms.AnyAsync(room => room.Name == name && room.Id != roomId);
            if (exists) return (true, Conflict());
            return (false, null);
        }

        private async Task<(bool exists, ActionResult? errorResponse)> ValidateRoomAbreviationAsync(string abreviation, int? roomId)
        {
            if (abreviation.IsNullOrEmpty()) return (false, null);
            bool exists = await _context.Rooms.AnyAsync(room => room.RoomAbreviation == abreviation && room.Id != roomId);
            if (exists) return (true, Conflict());
            return (false, null);
        }

        // GET: api/Rooms
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

        // GET: api/Rooms/Active
        [HttpGet("Active")]
        public async Task<ActionResult<IEnumerable<RoomDTO>>> GetRoomsActive()
        {
            // get all rooms that are active
            IEnumerable<Room> rooms = await _context.Rooms.Where(user => !user.IsDeleted).ToListAsync();
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

        // GET: api/Rooms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RoomDTO>> GetRoom(int id)
        {
            var (room, error) = await ValidateRoomAsync(id);
            if (error != null) return error;
            return RoomMapper.toDTO(room);
        }

        // PUT: api/Rooms/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoom(int id, RoomDTO roomDTO)
        {
            if (id != roomDTO.Id)
            {
                return BadRequest();
            }

            var (existingRoom, error) = await ValidateRoomAsync(id);
            if (error != null) return error;

            var (_, errorName) = await ValidateRoomNameAsync(roomDTO.Name, id);
            if (errorName != null) return errorName;

            var (_, errorAbreviation) = await ValidateRoomAbreviationAsync(roomDTO.RoomAbreviation, id);
            if (errorAbreviation != null) return errorAbreviation;

            try
            {
                Room room = RoomMapper.toDAL(roomDTO);
                _context.Entry(existingRoom).CurrentValues.SetValues(room);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest("Invalid group data : " + ex.Message);
            }
        }

        // POST: api/Rooms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RoomDTO>> PostRoom(RoomDTO roomDTO)
        {
            // check if the room already exists
            if (await _context.Rooms.AnyAsync(room => room.Id == roomDTO.Id))
            {
                return Conflict("Room already exists.");
            }

            var (_, errorName) = await ValidateRoomNameAsync(roomDTO.Name, null);
            if (errorName != null) return errorName;

            var (_, errorAbreviation) = await ValidateRoomAbreviationAsync(roomDTO.RoomAbreviation, null);
            if (errorAbreviation != null) return errorAbreviation;

            Room room = RoomMapper.toDAL(roomDTO);

            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetRoom", new { id = room.Id }, room);
        }

        // DELETE: api/Rooms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var (room, error) = await ValidateRoomAsync(id);
            if (error != null) return error;

            room.IsDeleted = true;
            _context.Update(room);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Rooms/Name/{name}
        [HttpGet("Name/{name}")]
        public async Task<ActionResult<bool>> RoomNameExists(string name)
        {
            var (exists, _) = await ValidateRoomNameAsync(name, null);
            return exists;
        }

        // GET: api/Rooms/Abreviation/{abreviation}
        [HttpGet("Abreviation/{abreviation}")]
        public async Task<ActionResult<bool>> RoomAbreviationExists(string abreviation)
        {
            var (exists, _) = await ValidateRoomAbreviationAsync(abreviation, null);
            return exists;
        }
    }
}
