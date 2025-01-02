using Microsoft.AspNetCore.Mvc;
using DTO;

namespace WebApi.Controllers.Interfaces
{
    public interface IGroupsController
    {
        Task<ActionResult<IEnumerable<GroupDTO>>> GetGroups();
        Task<ActionResult<IEnumerable<GroupDTO>>> GetGroupsActive();
        Task<ActionResult<GroupDTO>> GetGroup(int id);
        Task<IActionResult> PutGroup(int id, GroupDTO groupDTO);
        Task<ActionResult<GroupDTO>> PostGroup(GroupDTO groupDTO);
        Task<IActionResult> DeleteGroup(int id);
        Task<ActionResult<bool>> GroupNameExists(string name);
        Task<ActionResult<bool>> GroupAcronymExists(string acronym);
    }
}
