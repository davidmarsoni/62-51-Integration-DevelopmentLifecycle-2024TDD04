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

        public async Task<bool> TestAccessAsync(RoomAccessDTO roomAccessDTO)
        {
            var result = await SQS.Post<RoomAccessDTO>(_client, $"{_baseUrl}/Access", roomAccessDTO);
            return result != null;
        }
    }
}
