using DTO;
using MVC.Services.Interfaces;
using SQS = MVC.Services.QuerySnippet.StandardQuerySet;

namespace MVC.Services
{
    public class AccessService : IAccessService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;
        public AccessService(HttpClient client, string baseURL, bool debug)
        {
            _client = client;
            _baseUrl = baseURL + "/accesses";
            SQS.Debug = debug;
        }

        public async Task<bool> GrantAccessAsync(AccessDTO accessDTO)
        {
            return await SQS.PostNoReturn(_client, $"{_baseUrl}/GrantAccess", accessDTO);
        }

        public async Task<bool> RevokeAccessAsync(int roomId, int groupId)
        {
            return await SQS.Delete(_client, $"{_baseUrl}/RevokeAccess/{roomId}/{groupId}");
        }

        public async Task<bool> HasAccessGroupAsync(int roomId, int groupId)
        {
            return await SQS.Get<bool>(_client, $"{_baseUrl}/HasAccessGroup/{roomId}/{groupId}");
        }

        public async Task<bool> HasAccessUserAsync(int roomId, int userId)
        {
            return await SQS.Get<bool>(_client, $"{_baseUrl}/HasAccessUser/{roomId}/{userId}");
        }

        public async Task<IEnumerable<RoomDTO>?> GetAccessesByUserId(int userId)
        {
            return await SQS.Get<IEnumerable<RoomDTO>>(_client, $"{_baseUrl}/GetAccessesByUserId/{userId}");
        }

        public async Task<IEnumerable<RoomDTO>?> GetAccessesByGroupId(int groupId)
        {
            return await SQS.Get<IEnumerable<RoomDTO>>(_client, $"{_baseUrl}/GetAccessesByGroupId/{groupId}");
        }
    }
}