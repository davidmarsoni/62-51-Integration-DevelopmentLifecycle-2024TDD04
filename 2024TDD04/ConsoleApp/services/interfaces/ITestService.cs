
using DTO;

namespace MVC.Services.Interfaces
{
    public interface ITestService
    {
        Task<RoomAccessDTO> TestAccessRoom(int roomId, int userId);
    }
}