using Azure;
using ConsoleApp.utils;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace MVC.Services.QuerySnippet
{
    /// <summary>
    /// Static class to hold all the query snippets for the application's services.
    /// Contains all the HTTP queries and JSON handling methods.
    /// </summary>
    public static class QuerySnippet
    {
        public static readonly String GETALL = "GETALL";
        public static readonly String GET = "GET";
        public static readonly String POST = "POST";
        public static readonly String PUT = "PUT";
        public static readonly String DELETE = "DELETE";

        public static bool Debug { get; set; } = false;

        #region HTTP Queries

        public static async Task<HttpResponseMessage?> GetOnURL(HttpClient httpClient, String url) {
            return await httpClient.GetAsync(url);
        }

        public static async Task<HttpResponseMessage?> PostOnUrl(HttpClient httpClient, String url, Object obj)
        {
            return await httpClient.PostAsJsonAsync(url, obj);
        }

        public static async Task<HttpResponseMessage?> PutOnUrl(HttpClient httpClient, String url, Object obj)
        {
            return await httpClient.PutAsJsonAsync(url, obj);
        }

        public static async Task<HttpResponseMessage?> DeleteOnUrl(HttpClient httpClient, String url)
        {
            return await httpClient.DeleteAsync(url);
        }

        /// <summary>
        /// Handles the HTTP response and deserializes the JSON content if the response is successful.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the JSON content into.</typeparam>
        /// <param name="httpResponse">The HTTP response message.</param>
        /// <param name="originalOperation">The original operation name.</param>
        /// <returns>The deserialized object if the response is successful; otherwise, the default value of the type.</returns>
        public static T? HttpResponseHandling<T>(HttpResponseMessage? httpResponse, string originalOperation)
        {
            if (isHttpResponseMessageSuccess(httpResponse, originalOperation))
            {
                String responseBody = httpResponse.Content.ReadAsStringAsync().Result;
                return JsonDeserialize<T>(responseBody);
            }
            return default;
        }

        /// <summary>
        /// Checks if the HTTP response message indicates a successful response.
        /// </summary>
        /// <param name="httpResponse">The HTTP response message.</param>
        /// <param name="originalOperation">The original operation name (logging).</param>
        /// <returns>True if the response is successful; otherwise, false.</returns>
        public static Boolean isHttpResponseMessageSuccess(HttpResponseMessage? httpResponse, string originalOperation)
        {
            try
            {
                httpResponse.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception e)
            {
                if (Debug)
                {
                    Console.WriteLine(Colors.Colorize($"{originalOperation} Request failed with message: {e.Message}", Colors.Magenta));
                }
                return false;
            }
        }

        #endregion

        #region JSON Handling

        public static T? JsonDeserialize<T>(string json)
        {
            try {
                return JsonSerializer.Deserialize<T>(json, JsonSerializerOpt); ;
            } catch (Exception e)
            {
                if (Debug)
                {
                    Console.WriteLine(Colors.Colorize($"Failed to deserialize JSON: {e.Message}", Colors.Magenta));
                }
                return default;
            }
            
        }

        public static JsonSerializerOptions JsonSerializerOpt = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        #endregion
    }
}
