using DTO;
using MVC.Services.Interfaces;
using SQS = MVC.Services.QuerySnippet.StandardQuerySet;

namespace MVC.Services
{
    public class GroupService : IGroupService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;
        

        public GroupService(HttpClient client, String baseURL, bool debug)
        {
            _client = client;
            _baseUrl = baseURL + "/groups";
            SQS.Debug = debug;
        }

        public async Task<GroupDTO?> GetGroupById(int id)
        {
            return await SQS.Get<GroupDTO>(_client, $"{_baseUrl}/{id}");
        }

        public async Task<IEnumerable<GroupDTO>?> GetAllGroups()
        {
            return await SQS.GetAll<GroupDTO>(_client, $"{_baseUrl}/Active");
        }

        public async Task<GroupDTO?> CreateGroup(GroupDTO groupDTO)
        {
            return await SQS.Post<GroupDTO?>(_client, _baseUrl, groupDTO);
        }

        public async Task<Boolean> UpdateGroup(GroupDTO groupDTO)
        {
            return await SQS.PutNoReturn(_client, $"{_baseUrl}/{groupDTO.Id}", groupDTO);
        }

        public async Task<Boolean> DeleteGroup(int id)
        {
            return await SQS.Delete(_client, $"{_baseUrl}/{id}");
        }

        public async Task<bool> GroupNameExists(string name)
        {
            return await SQS.Get<bool>(_client, $"{_baseUrl}/Name/{name}");
        }

        public async Task<bool> GroupAcronymExists(string acronym)
        {
            return await SQS.Get<bool>(_client, $"{_baseUrl}/Acronym/{acronym}");
        }
    }
}