using DTO;
using MVC.Services.Interfaces;
using SQS = MVC.Services.QuerySnippet.StandardQuerySet;

namespace MVC.Services
{
    public class RoomService : IRoomService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        public RoomService(HttpClient client, String baseURL, bool debug)
        {
            _client = client;
            _baseUrl = baseURL + "/rooms";
            SQS.Debug = debug;
        }

        public async Task<RoomDTO?> GetRoomById(int id)
        {
            return await SQS.Get<RoomDTO>(_client, $"{_baseUrl}/{id}");
        }

        public async Task<IEnumerable<RoomDTO>?> GetAllRooms()
        {
            return await SQS.GetAll<RoomDTO>(_client, $"{_baseUrl}/Active");
        }

        public async Task<RoomDTO?> CreateRoom(RoomDTO roomDTO)
        {
            return await SQS.Post<RoomDTO?>(_client, _baseUrl, roomDTO);
        }

        public async Task<bool> UpdateRoom(RoomDTO roomDTO)
        {
            return await SQS.PutNoReturn(_client, $"{_baseUrl}/{roomDTO.Id}", roomDTO);
        }

        public async Task<bool> DeleteRoom(int id)
        {
            return await SQS.Delete(_client, $"{_baseUrl}/{id}");
        }

        public async Task<bool> RoomNameExists(string name)
        {
            return await SQS.Get<bool>(_client, $"{_baseUrl}/Name/{name}");
        }

        public async Task<bool> RoomAbreviationExists(string roomAbreviation)
        {
            return await SQS.Get<bool>(_client, $"{_baseUrl}/Abreviation/{roomAbreviation}");
        }
    }
}