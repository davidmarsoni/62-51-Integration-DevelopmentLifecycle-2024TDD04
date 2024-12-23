using ConsoleApp.services.@interface;
using DTO;
using SQS = MVC.Services.QuerySnippet.StandardQuerySet;

namespace MVC.Services
{
    public class UserGroupService : IUserGroupService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        public UserGroupService(HttpClient client, String baseURL, bool debug)
        {
            _client = client;
            _baseUrl = baseURL + "/usergroups";
            SQS.Debug = debug;
        }

        public async Task<bool> AddUserToGroup(UserGroupDTO userGroupDTO)
        {
            return await SQS.PostNoReturn(_client, _baseUrl, userGroupDTO);
        }

        public async Task<bool> RemoveUserFromGroup(int groupId, int userId)
        {
            return await SQS.Delete(_client, $"{_baseUrl}/{groupId}/{userId}");
        }

        public async Task<IEnumerable<UserDTO>?> GetUsersInGroup(int groupId)
        {
            return await SQS.GetAll<UserDTO>(_client, $"{_baseUrl}/{groupId}/users");
        }
    }
}
