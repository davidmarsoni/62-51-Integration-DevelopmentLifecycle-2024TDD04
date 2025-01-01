using DTO;

namespace MVC.Services.Interfaces
{
    public interface IGroupService
    {
        Task<GroupDTO?> GetGroupById(int id);
        Task<IEnumerable<GroupDTO>?> GetAllGroups();
        Task<GroupDTO?> CreateGroup(GroupDTO groupDTO);
        Task<bool> UpdateGroup(GroupDTO groupDTO);
        Task<bool> DeleteGroup(int id);
        Task<bool> GroupNameExists(string name);
        Task<bool> GroupAcronymExists(string acronym);
        Task<bool> AddUserToGroup(UserGroupDTO userGroupDTO);
        Task<bool> RemoveUserFromGroup(int groupId, int userId);
    }
}