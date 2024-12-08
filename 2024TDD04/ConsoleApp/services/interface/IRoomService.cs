
using DTO;

namespace MVC.Services.Interfaces
{
    public interface IRoomService
    {
        Task<RoomDTO?> GetRoomById(int id);
        Task<IEnumerable<RoomDTO>?> GetAllRooms();
        Task<RoomDTO?> CreateRoom(RoomDTO roomDTO);
        Task<bool> UpdateRoom(RoomDTO roomDTO);
        Task<bool> DeleteRoom(int id);
        Task<bool> RoomNameExists(string name);
        Task<bool> RoomAbreviationExists(string roomAbreviation);
    }
}