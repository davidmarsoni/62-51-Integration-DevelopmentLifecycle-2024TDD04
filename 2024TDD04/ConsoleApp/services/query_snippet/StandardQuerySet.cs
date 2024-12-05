using System.Net;
using QS = MVC.Services.QuerySnippet.QuerySnippet;

namespace MVC.Services.QuerySnippet
{
    /// <summary>
    /// Method Library used to handle the standard CRUD operations for a resource.
    /// </summary>
    public class StandardQuerySet
    {
        /// <summary>
        /// Retrieves all resources of type 'T' from the specified URL using the provided HttpClient.
        /// </summary>
        /// <typeparam name="T">The type of the resources to retrieve.</typeparam>
        /// <param name="httpClient">The HttpClient instance to use for the request.</param>
        /// <param name="url">The URL of the resources to retrieve.</param>
        /// <returns>Returns all resources of type 'T' if the request is successful, otherwise null.</returns>
        public static async Task<IEnumerable<T>?> GetAll<T>(HttpClient httpClient, String url)
        {
            return QS.HttpResponseHandling<IEnumerable<T>>(await QS.GetOnURL(httpClient, url), QS.GETALL);
        }

        /// <summary>
        /// Sends a GET request to retrieve a resource from the specified URL using the provided HttpClient.
        /// </summary>
        /// <typeparam name="T">The type of the resource to retrieve.</typeparam>
        /// <param name="httpClient">The HttpClient instance to use for the request.</param>
        /// <param name="url">The URL of the resource to retrieve. (include id please)</param>
        /// <returns>Returns the retrieved resource of type 'T' if the request is successful, otherwise null.</returns>
        public static async Task<T?> Get<T>(HttpClient httpClient, String url)
        {
            return QS.HttpResponseHandling<T>(await QS.GetOnURL(httpClient, url), QS.GET);
        }

        /// <summary>
        /// Sends a POST request to create a new resource at the specified URL using the provided HttpClient.
        /// </summary>
        /// <typeparam name="T">The type of the resource to create.</typeparam>
        /// <param name="httpClient">The HttpClient instance to use for the request.</param>
        /// <param name="url">The URL where the resource will be created.</param>
        /// <param name="obj">The object containing the resource data to be created.</param>
        /// <returns>Returns the created resource of type 'T' if the request is successful, otherwise null.</returns>
        public static async Task<T?> Post<T>(HttpClient httpClient, String url, T obj)
        {
            return QS.HttpResponseHandling<T>(await QS.PostOnUrl(httpClient, url, obj), QS.POST);
        }

        /// <summary>
        /// Sends a PUT request to update a resource at the specified URL using the provided HttpClient.
        /// </summary>
        /// <typeparam name="T">The type of the resource to create.</typeparam>
        /// <param name="httpClient">The HttpClient instance to use for the request.</param>
        /// <param name="url">The URL of the resource to update. (include id please)</param>
        /// <param name="obj">The object containing the updated resource data.</param>
        /// <returns>Returns if the resource was successfully created, otherwise false.</returns>
        public static async Task<Boolean> PostNoReturn<T>(HttpClient httpClient, String url, T obj)
        {
            HttpResponseMessage? httpResponseMessage = await QS.PostOnUrl(httpClient, url, obj);
            return QS.isHttpResponseMessageSuccess(httpResponseMessage, QS.POST);
        }

        /// <summary>
        /// Sends a PUT request to update a resource at the specified URL using the provided HttpClient.
        /// </summary>
        /// <typeparam name="T">The type of the resource to update.</typeparam>
        /// <param name="httpClient">The HttpClient instance to use for the request.</param>
        /// <param name="url">The URL of the resource to update. (include id please)</param>
        /// <param name="obj">The object containing the updated resource data.</param>
        /// <returns>Returns true if the resource was successfully updated, otherwise false. Does not return any objects type 'T'</returns>
        public static async Task<Boolean> PutNoReturn<T>(HttpClient httpClient, String url, T obj)
        {
            HttpResponseMessage? httpResponseMessage = await QS.PutOnUrl(httpClient, url, obj);
            return QS.isHttpResponseMessageSuccess(httpResponseMessage, QS.PUT);
        }

       

        /// <summary>
        /// Deletes a resource from the specified URL using the provided HttpClient.
        /// </summary>
        /// <param name="httpClient">The HttpClient instance to use for the request.</param>
        /// <param name="url">The URL of the resource to delete. (include id please)</param>
        /// <returns>Returns true if the resource was successfully deleted, otherwise false.</returns>
        public static async Task<Boolean> Delete(HttpClient httpClient, String url)
        {
            HttpResponseMessage? httpResponseMessage = await QS.DeleteOnUrl(httpClient, url);
            return QS.isHttpResponseMessageSuccess(httpResponseMessage, QS.DELETE);
        }
    }
}
