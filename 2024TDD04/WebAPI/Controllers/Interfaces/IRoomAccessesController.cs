using Microsoft.AspNetCore.Mvc;
using DTO;

namespace WebApi.Controllers.Interfaces
{
    public interface IRoomAccessesController
    {
        Task<ActionResult<RoomAccessDTO>> AccessRoom(int roomId, int userId);
    }
}
