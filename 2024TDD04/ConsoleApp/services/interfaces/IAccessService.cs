
using DTO;

namespace MVC.Services.Interfaces
{
    public interface IAccessService
    {
        Task<bool> GrantAccessAsync(AccessDTO accessDTO);
        Task<bool> RevokeAccessAsync(AccessDTO accessDTO);
        Task<RoomDTO?> GetRoomAccessibleByGroup(int groupId);
        Task<RoomDTO?> GetRoomAccessibleByUser(int userId);
        Task<bool> HasAccessGroupAsync(int roomId, int groupId);
        Task<bool> HasAccessUserAsync(int roomId, int userId);
    }
}