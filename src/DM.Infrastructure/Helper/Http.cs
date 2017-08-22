using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DM.Infrastructure.Helper
{
    /// <summary>
    /// Http Helper
    /// </summary>
    public static class Http
    {
        private static HttpClient client = new HttpClient();

        public async static Task<string> GetAsync(string url)
        {
            string strResult = await client.GetStringAsync(url);
            return strResult;
        }

        public async static Task<T> GetAsync<T>(string url)
        {
            string strResult = await client.GetStringAsync(url);
            return Json.ToObject<T>(strResult);
        }

        public async static Task<T> PostStringAsync<T>(string url, string postData, Encoding encode = null)
        {
            if (encode == null)
            {
                encode = Encoding.UTF8;
            }

            StringContent content = new StringContent(postData, encode);
            HttpResponseMessage response = await client.PostAsync(url, content);
            string strResult = await response.Content.ReadAsStringAsync();
            return Json.ToObject<T>(strResult);
        }

        public async static Task<T> PostFromAsync<T>(string url, IEnumerable<KeyValuePair<string, string>> nameValueCollection)
        {
            FormUrlEncodedContent content = new FormUrlEncodedContent(nameValueCollection);
            HttpResponseMessage response = await client.PostAsync(url, content);
            string strResult = await response.Content.ReadAsStringAsync();
            return Json.ToObject<T>(strResult);
        }
    }
}
