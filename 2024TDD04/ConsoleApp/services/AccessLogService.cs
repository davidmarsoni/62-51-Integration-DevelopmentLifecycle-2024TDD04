using DTO;
using MVC.Services.Interfaces;
using SQS = MVC.Services.QuerySnippet.StandardQuerySet;

namespace MVC.Services
{
    public class AccessLogService : IAccessLogService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;
        public AccessLogService(HttpClient client, string baseURL, bool debug)
        {
            _client = client;
            _baseUrl = baseURL + "/accesslogs";
            SQS.Debug = debug;
        }

        public async Task<bool> LogAccessAsync(RoomAccessLogDTO roomAccessLogDTO)
        {
            return await SQS.PostNoReturn(_client, $"{_baseUrl}/LogAccess", roomAccessLogDTO);
        }

        public async Task<IEnumerable<RoomAccessLogDTO>> GetAccessLog(int? logNumber, int? offset, string? order)
        {
            var queryParams = new List<string>();
            if (logNumber.HasValue) queryParams.Add($"logNumber={logNumber}");
            if (offset.HasValue) queryParams.Add($"offset={offset}");
            if (!string.IsNullOrEmpty(order)) queryParams.Add($"order={order}");

            var queryString = string.Join("&", queryParams);
            return await SQS.Get<IEnumerable<RoomAccessLogDTO>>(_client, $"{_baseUrl}?{queryString}");
        }
    }
}
