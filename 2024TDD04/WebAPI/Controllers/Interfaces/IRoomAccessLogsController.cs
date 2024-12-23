using Microsoft.AspNetCore.Mvc;
using DTO;

namespace WebApi.Controllers.Interfaces
{
    public interface IRoomAccessLogsController
    {
        Task<ActionResult<IEnumerable<RoomAccessLogDTO>>> GetRoomAccessLogsAsync(int? logNumber, int? offset, string? order);
    }
}
