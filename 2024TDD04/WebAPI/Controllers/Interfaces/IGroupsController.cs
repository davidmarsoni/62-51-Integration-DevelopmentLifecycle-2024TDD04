using Microsoft.AspNetCore.Mvc;
using DTO;

namespace WebApi.Controllers.Interfaces
{
    public interface IGroupsController
    {
        Task<ActionResult<IEnumerable<GroupDTO>>> GetGroups();
        Task<ActionResult<List<GroupDTO>>> GetUsersActive();
        Task<ActionResult<GroupDTO>> GetGroup(int id);
        Task<ActionResult<List<GroupDTO>>> GetGroupsByUserId(int userId);
        Task<ActionResult<bool>> GroupNameExists(string name);
        Task<ActionResult<bool>> GroupAcronymExists(string acronym);
        Task<IActionResult> PutGroup(int id, GroupDTO groupDTO);
        Task<ActionResult<GroupDTO>> PostGroup(GroupDTO groupDTO);
        Task<IActionResult> DeleteGroup(int id);
    }
}
