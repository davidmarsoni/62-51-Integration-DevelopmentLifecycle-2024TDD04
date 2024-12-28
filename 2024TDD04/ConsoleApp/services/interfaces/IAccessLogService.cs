using DTO;

namespace MVC.Services.Interfaces
{
    public interface IAccessLogService
    {
        Task<IEnumerable<RoomAccessLogDTO>> GetAccessLog(int? logNumber, int? offset, string? order);
    }
}