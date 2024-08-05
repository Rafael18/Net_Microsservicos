using System.Text.Json;
using System.Net.Http.Headers;

namespace GeekShopping.Web.Utils
{
    public static class HttpClientExtensions
    {
        private static MediaTypeHeaderValue contentType = new MediaTypeHeaderValue("application/json");
        public static async Task<T> ReadContentAs<T>(this HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode) throw new ApplicationException($"Somethinf went wrong calling API:{response.ReasonPhrase}");

            var dataAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonSerializer.Deserialize<T>(dataAsString,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public static Task<HttpResponseMessage> PostAsJson<T>(this HttpClient httpClient,string url, T data)
        {
            var dataAsString = JsonSerializer.Serialize(data);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = contentType;
            return httpClient.PostAsync(url, content);
        }

        public static async Task<HttpResponseMessage> PutAsJason<T>(this HttpClient httpClient, string url, T data)
        {
            var dataAsString = JsonSerializer.Serialize(data);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = contentType;
            return await httpClient.PutAsync(url, content);
        }
    }
}
