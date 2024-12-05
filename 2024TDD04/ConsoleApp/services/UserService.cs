using DTO;
using MVC.Services.Interfaces;
using SQS = MVC.Services.QuerySnippet.StandardQuerySet;

namespace MVC.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        public UserService(HttpClient client, String baseURL)
        {
            _client = client;
            _baseUrl = baseURL + "/users";
        }

        public async Task<UserDTO?> GetUserById(int id)
        {
            return await SQS.Get<UserDTO>(_client, $"{_baseUrl}/{id}");
        }

        public async Task<Boolean> GetUsernameExist(string username) 
        {
            return await SQS.Get<Boolean>(_client, $"{_baseUrl}/Username/{username}");
        }

        public async Task<IEnumerable<UserDTO>?> GetAllUsers()
        {
            return await SQS.GetAll<UserDTO>(_client, _baseUrl);
        }

        public async Task<UserDTO?> CreateUser(UserDTO userDTO)
        {
            return await SQS.Post<UserDTO?>(_client, _baseUrl, userDTO);
        }

        public async Task<Boolean> UpdateUser(UserDTO userDTO)
        {
            return await SQS.PutNoReturn(_client, $"{_baseUrl}/{userDTO.Id}", userDTO);
        }

        public async Task<Boolean> DeleteUser(int id)
        {
            return await SQS.Delete(_client, $"{_baseUrl}/{id}");
        }

        public async Task<IEnumerable<UserDTO>?> GetUsersByGroupId(int groupId)
        {
            return await SQS.Get<IEnumerable<UserDTO>>(_client, $"{_baseUrl}/Group/{groupId}");
        }
    }
}
