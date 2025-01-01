using DTO;

namespace MVC.Services.Interfaces
{
    public interface IAccessService
    {
        Task<bool> GrantAccessAsync(AccessDTO accessDTO);
        Task<bool> RevokeAccessAsync(int roomId, int groupId);
        Task<bool> HasAccessGroupAsync(int roomId, int groupId);
        Task<bool> HasAccessUserAsync(int roomId, int userId);
        Task<IEnumerable<RoomDTO>?> GetAccessesByUserId(int roomId);
        Task<IEnumerable<RoomDTO>?> GetAccessesByGroupId(int groupId);
    }
}