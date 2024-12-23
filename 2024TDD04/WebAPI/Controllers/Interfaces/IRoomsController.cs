using Microsoft.AspNetCore.Mvc;
using DTO;

namespace WebApi.Controllers.Interfaces
{
    public interface IRoomsController
    {
        Task<ActionResult<IEnumerable<RoomDTO>>> GetRooms();
        Task<ActionResult<List<RoomDTO>>> GetRoomsActive();
        Task<ActionResult<RoomDTO>> GetRoom(int id);
        Task<IActionResult> PutRoom(int id, RoomDTO roomDTO);
        Task<ActionResult<RoomDTO>> PostRoom(RoomDTO roomDTO);
        Task<IActionResult> DeleteRoom(int id);
        Task<ActionResult<bool>> RoomNameExists(string name);
        Task<ActionResult<bool>> RoomAbreviationExists(string abreviation);
    }
}
