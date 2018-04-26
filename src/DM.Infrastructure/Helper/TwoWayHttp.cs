using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DM.Infrastructure.Helper
{
    /// <summary>
    /// Http Helper
    /// </summary>
    public sealed class TwoWayHttp
    {
        private static readonly HttpClient httpClient = null;
        static TwoWayHttp()
        {
            HttpClientHandler hander = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,//启用响应内容压缩
                ServerCertificateCustomValidationCallback = ServerCertificateCustomValidation,//设置访问https url
            };
            X509Certificate2 cert = new X509Certificate2(@"outgoing.CertwithKey.pkcs12", "IoM@1234");
            hander.ClientCertificates.Add(cert);

            httpClient = new HttpClient(hander);
            httpClient.Timeout = TimeSpan.FromSeconds(10);//超时设置
            //httpClient.DefaultRequestHeaders.Connection.Add("keep-alive");//保持链接
        }

        private static bool ServerCertificateCustomValidation(HttpRequestMessage sender, X509Certificate2 certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true;

        public async static Task<string> GetStringAsync(string url, IDictionary<string, string> headers = null)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                //添加http请求头
                headers?.ToList().ForEach(item =>
                {
                    request.Headers.Add(item.Key, item.Value);
                });

                using (HttpResponseMessage response = await httpClient.SendAsync(request))
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }

        public async static Task<Stream> GetStreamAsync(string url, IDictionary<string, string> headers = null)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                //添加http请求头
                headers?.ToList().ForEach(item =>
                {
                    request.Headers.Add(item.Key, item.Value);
                });

                using (HttpResponseMessage response = await httpClient.SendAsync(request))
                {
                    return await response.Content.ReadAsStreamAsync();
                }
            }
        }

        public async static Task<T> GetObjectAsync<T>(string url, IDictionary<string, string> headers = null)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                //添加http请求头
                headers?.ToList().ForEach(item =>
                {
                    request.Headers.Add(item.Key, item.Value);
                });

                using (HttpResponseMessage response = await httpClient.SendAsync(request))
                {
                    string strResult = await response.Content.ReadAsStringAsync();
                    return Json.ToObject<T>(strResult);
                }
            }
        }

        public async static Task<T> PostStringAsync<T>(string url, string postData, IDictionary<string, string> headers = null)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url))
            {
                request.Content = new StringContent(postData, Encoding.UTF8);

                //添加http请求头
                headers?.ToList().ForEach(item =>
                {
                    request.Headers.Add(item.Key, item.Value);
                });

                using (HttpResponseMessage response = await httpClient.SendAsync(request))
                {
                    string strResult = await response.Content.ReadAsStringAsync();
                    return Json.ToObject<T>(strResult);
                }
            }
        }

        public async static Task<T> PostFromAsync<T>(string url, IDictionary<string, string> dicForm, IDictionary<string, string> headers = null)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url))
            {
                request.Content = new FormUrlEncodedContent(dicForm);

                //添加http请求头
                headers?.ToList().ForEach(item =>
                {
                    request.Headers.Add(item.Key, item.Value);
                });

                using (HttpResponseMessage response = await httpClient.SendAsync(request))
                {
                    string strResult = await response.Content.ReadAsStringAsync();
                    return Json.ToObject<T>(strResult);
                }
            }
        }

        public async static Task<T> PostJsonAsync<T>(string url, object postModel, IDictionary<string, string> headers = null)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url))
            {
                request.Content = new StringContent(Json.ToJson(postModel), Encoding.UTF8, "application/json");

                //添加http请求头
                headers?.ToList().ForEach(item =>
                {
                    request.Headers.Add(item.Key, item.Value);
                });

                using (HttpResponseMessage response = await httpClient.SendAsync(request))
                {
                    string strResult = await response.Content.ReadAsStringAsync();
                    return Json.ToObject<T>(strResult);
                }
            }
        }

        public async static Task<T> PostMultipartFormDataAsync<T>(string url, IDictionary<string, FileInfo> dicFiles, IDictionary<string, string> dicForm = null, IDictionary<string, string> headers = null)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url))
            using (MultipartFormDataContent content = new MultipartFormDataContent())
            {
                //添加表单内容
                dicForm?.ToList().ForEach(item =>
                {
                    content.Add(new StringContent(item.Value), item.Key);
                });

                //添加多文件内容
                dicFiles.ToList().ForEach(item =>
                {
                    ByteArrayContent bc = new ByteArrayContent(File.ReadAllBytes(item.Value.FullName));
                    content.Add(bc, item.Key, item.Value.Name);
                });

                request.Content = content;

                //添加http请求头
                headers?.ToList().ForEach(item =>
                {
                    request.Headers.Add(item.Key, item.Value);
                });

                using (HttpResponseMessage response = await httpClient.SendAsync(request))
                {
                    string strResult = await response.Content.ReadAsStringAsync();
                    return Json.ToObject<T>(strResult);
                }
            }
        }

        public async static Task<string> DeleteAsync(string url, IDictionary<string, string> headers = null)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, url))
            {
                //添加http请求头
                headers?.ToList().ForEach(item =>
                {
                    request.Headers.Add(item.Key, item.Value);
                });

                using (HttpResponseMessage response = await httpClient.SendAsync(request))
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }

        public async static Task<T> PutJsonAsync<T>(string url, object postModel, IDictionary<string, string> headers = null)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, url))
            {
                request.Content = new StringContent(Json.ToJson(postModel), Encoding.UTF8, "application/json");

                //添加http请求头
                headers?.ToList().ForEach(item =>
                {
                    request.Headers.Add(item.Key, item.Value);
                });

                using (HttpResponseMessage response = await httpClient.SendAsync(request))
                {
                    string strResult = await response.Content.ReadAsStringAsync();
                    return Json.ToObject<T>(strResult);
                }
            }
        }
    }
}
