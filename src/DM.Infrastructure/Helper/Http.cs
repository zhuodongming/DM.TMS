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

        ///// <summary>
        ///// Http Get请求
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="url">http/https地址</param>
        ///// <param name="encode">文本编码</param>
        ///// <param name="timeout">超时时间，单位毫秒</param>
        ///// <returns></returns>
        //public static T Get<T>(string url, Encoding encode, int? timeout = null)
        //{
        //    HttpWebRequest request = null;//设置Http请求基本信息
        //    if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))//是否HTTPS请求  
        //    {
        //        ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;//设置证书验证
        //        request = WebRequest.Create(url) as HttpWebRequest;
        //        request.ProtocolVersion = HttpVersion.Version10;//协议版本
        //    }
        //    else
        //    {
        //        request = WebRequest.Create(url) as HttpWebRequest;
        //    }

        //    request.Method = HttpMethod.Get.Method;//Http Get方法

        //    if (timeout.HasValue)//设置超时时间
        //    {
        //        request.Timeout = timeout.Value;
        //    }

        //    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())//发送请求
        //    {
        //        using (StreamReader srReader = new StreamReader(response.GetResponseStream(), encode))
        //        {
        //            string strResult = srReader.ReadToEnd();//获取响应信息
        //            return JsonHelper.ToObject<T>(strResult);//反序列化
        //        }
        //    }
        //}

        ///// <summary>
        ///// Http Post请求,报文体使用json
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="url">http/https地址</param>
        ///// <param name="postData">发送的报文体对象,</param>
        ///// <param name="encode">文本编码</param>
        ///// <param name="timeout">超时时间，单位毫秒</param>
        ///// <returns></returns>
        //public static T PostJson<T>(string url, object postData, Encoding encode, int? timeout = null)
        //{
        //    HttpWebRequest request = null;//设置Http请求基本信息
        //    if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))//如果是发送HTTPS请求  
        //    {
        //        ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
        //        request = WebRequest.Create(url) as HttpWebRequest;
        //        request.ProtocolVersion = HttpVersion.Version10;
        //    }
        //    else
        //    {
        //        request = WebRequest.Create(url) as HttpWebRequest;
        //    }

        //    request.Method = HttpMethod.Post.Method;//Http Post方法
        //    request.ContentType = "application/json";//请求报文体为json

        //    if (postData != null)//如果需要POST数据  
        //    {
        //        byte[] buffer = encode.GetBytes(JsonHelper.ToJson(postData));
        //        request.ContentLength = buffer.Length;
        //        using (Stream requestStream = request.GetRequestStream())
        //        {
        //            requestStream.Write(buffer, 0, buffer.Length);//填充报文体数据
        //        }
        //    }
        //    else
        //    {
        //        request.ContentLength = 0;
        //    }

        //    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())//发送请求
        //    {
        //        using (StreamReader srReader = new StreamReader(response.GetResponseStream(), encode))
        //        {
        //            string strResult = srReader.ReadToEnd();//获取响应信息
        //            return JsonHelper.ToObject<T>(strResult);//反序列化
        //        }
        //    }
        //}

        ///// <summary>
        ///// Http Post请求,报文体使用form表单
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="url">http/https地址</param>
        ///// <param name="postData">发送的报文体对象,</param>
        ///// <param name="encode">文本编码</param>
        ///// <param name="timeout">超时时间，单位毫秒</param>
        ///// <returns></returns>
        //public static T PostForm<T>(string url, string formData, Encoding encode, int? timeout = null)
        //{
        //    HttpWebRequest request = null;//设置Http请求基本信息
        //    if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))//如果是发送HTTPS请求  
        //    {
        //        ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
        //        request = WebRequest.Create(url) as HttpWebRequest;
        //        request.ProtocolVersion = HttpVersion.Version10;
        //    }
        //    else
        //    {
        //        request = WebRequest.Create(url) as HttpWebRequest;
        //    }

        //    request.Method = HttpMethod.Post.Method;//Http Post方法
        //    request.ContentType = "application/x-www-form-urlencoded";//请求报文体为form表单

        //    if (formData != null)//如果需要POST数据  
        //    {
        //        byte[] buffer = encode.GetBytes(formData);
        //        request.ContentLength = buffer.Length;
        //        using (Stream requestStream = request.GetRequestStream())
        //        {
        //            requestStream.Write(buffer, 0, buffer.Length);//填充报文体数据
        //        }
        //    }
        //    else
        //    {
        //        request.ContentLength = 0;
        //    }

        //    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())//发送请求
        //    {
        //        using (StreamReader srReader = new StreamReader(response.GetResponseStream(), encode))
        //        {
        //            string strResult = srReader.ReadToEnd();//获取响应信息
        //            return JsonHelper.ToObject<T>(strResult);//反序列化
        //        }
        //    }
        //}

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

        public async static Task<string> PostAsync(string url, string postData, Encoding encode = null)
        {
            if (encode == null)
            {
                encode = Encoding.UTF8;
            }

            StringContent content = new StringContent(postData, encode);
            HttpResponseMessage response = await client.PostAsync(url, content);
            string strResult = await response.Content.ReadAsStringAsync();
            return strResult;
        }

        public async static Task<T> PostAsync<T>(string url, string postData, Encoding encode = null)
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
    }
}
