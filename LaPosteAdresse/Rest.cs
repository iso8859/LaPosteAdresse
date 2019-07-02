using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace ovh.api
{
    public class Rest
    {
        string _okapi_key;
        public Rest(string okapi_key)
        {
            _okapi_key = okapi_key;
        }

        public Dictionary<string, string> Headers = new Dictionary<string, string>();

        public HttpClient Client()
        {
            var result = new HttpClient();
            foreach (var h in Headers)
                result.DefaultRequestHeaders.Add(h.Key, h.Value);
            return result;
        }

        public async Task<HttpResponseMessage> SendAsync(HttpMethod method, string query, string body = null)
        {
            var request = new HttpRequestMessage(method, query);
            if (body != null)
                request.Content = new StringContent(body, Encoding.UTF8, "application/json; charset=utf-8");
            var client = Client();
            client.DefaultRequestHeaders.Add("X-Okapi-Key", _okapi_key);

            var response = await client.SendAsync(request).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode && response.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                var error = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                throw new Exception("Request failed:" + error);
            }
            return response;
        }

        public async Task<T> GetAsync<T>(string uri)
        {
            var response = await SendAsync(HttpMethod.Get, uri);
            response.EnsureSuccessStatusCode();
            string s = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(s);
        }

        public async Task<T> PostAsync<T>(string uri, object data = null)
        {
            var response = await SendAsync(HttpMethod.Post, uri, JsonConvert.SerializeObject(data));
            response.EnsureSuccessStatusCode();
            var s = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(s);
        }

        public async Task<T> PutAsync<T>(string uri, object data = null)
        {
            var response = await SendAsync(HttpMethod.Put, uri, JsonConvert.SerializeObject(data));
            response.EnsureSuccessStatusCode();
            var s = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(s);
        }
    }
}
