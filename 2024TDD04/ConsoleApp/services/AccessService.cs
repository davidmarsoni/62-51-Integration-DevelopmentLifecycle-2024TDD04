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

        public async Task<bool> RevokeAccessAsync(AccessDTO accessDTO)
        {
            return await SQS.PostNoReturn(_client, $"{_baseUrl}/RevokeAccess", accessDTO);
        }

        public async Task<RoomDTO?> GetRoomAccessibleByGroup(int groupId)
        {
            return await SQS.Get<RoomDTO>(_client, $"{_baseUrl}/GetRoomAccessedByGroup/{groupId}");
        }

        public async Task<RoomDTO?> GetRoomAccessibleByUser(int userId)
        {
            return await SQS.Get<RoomDTO>(_client, $"{_baseUrl}/GetRoomAccessedByUser/{userId}");
        }

        public async Task<bool> HasAccessGroupAsync(int roomId, int groupId)
        {
            return await SQS.Get<bool>(_client, $"{_baseUrl}/HasAccessGroup/{roomId}/{groupId}");
        }

        public async Task<bool> HasAccessUserAsync(int roomId, int userId)
        {
            return await SQS.Get<bool>(_client, $"{_baseUrl}/HasAccessUser/{roomId}/{userId}");
        }
    }
}