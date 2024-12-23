using DTO;

namespace MVC.Services.Interfaces
{
    public interface IAccessLogService
    {
        Task<bool> LogAccessAsync(RoomAccessLogDTO roomAccessLogDTO);
        Task<IEnumerable<RoomAccessLogDTO>> GetAccessLog(int? logNumber, int? offset, string? order);
    }
}