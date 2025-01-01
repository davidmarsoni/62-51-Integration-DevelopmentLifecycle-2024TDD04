using DTO;
using MVC.Services.Interfaces;
using SQS = MVC.Services.QuerySnippet.StandardQuerySet;

namespace MVC.Services
{
    public class TestService : ITestService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        public TestService(HttpClient client, string baseURL, bool debug)
        {
            _client = client;
            _baseUrl = $"{baseURL}/RoomAccesses";
            SQS.Debug = debug;
        }

        public async Task<RoomAccessDTO?> TestAccessRoom(int roomId, int userId)
        {
            return await SQS.Get<RoomAccessDTO>(_client, $"{_baseUrl}/Access/{roomId}/{userId}");
        }
    }
}
