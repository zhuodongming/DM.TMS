using DM.Infrastructure.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace DM.TMS.Domain.Service.PKC
{
    /// <summary>
    /// SignSendHelper
    /// </summary>
    public static class SignSendService
    {
        public static R SendNoSignRequest<T, R>(string url, T requestModel) where T : RequestModel where R : ResponseModel
        {
            return Http.PostAsync<R>(url, Json.ToJson(requestModel)).Result;
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="requestModel"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static R SendRequest<T, R>(string url, T requestModel, string key) where T : RequestModel where R : ResponseModel
        {
            Dictionary<string, string> parameters = GetDic(requestModel);//获取属性字典
            SortedDictionary<string, string> sortedParams = FilterPara(parameters);//筛选,并且返回排序字典
            string prestr = CreateLinkString(sortedParams);//拼接请求参数
            prestr = prestr + key;//拼接key
            string sign = Crypto.GetSHA1(prestr);//获取SHA1签名
            requestModel.Sign = sign;//设置sign字段

            //sortedParams.Add("sign", sign);//添加sign字段
            //sortedParams = EncodingParamValue(sortedParams);//编码参数值UrlEncode
            //string formData = CreateLinkString(sortedParams);//拼接post报文体

            return Http.PostAsync<R>(url, Json.ToJson(requestModel)).Result;
        }

        /// <summary>
        /// 获取属性字典
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static Dictionary<string, string> GetDic(object obj)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            IEnumerable<PropertyInfo> properties = obj.GetType().GetRuntimeProperties();
            foreach (var property in properties)
            {
                string name = property.Name;
                object value = property.GetValue(obj);
                parameters.Add(name, value?.ToString());
            }

            return parameters;
        }

        /// <summary>
        /// 筛选,并且返回排序字典
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static SortedDictionary<string, string> FilterPara(IDictionary<string, string> parameters)
        {
            SortedDictionary<string, string> sortedParams = new SortedDictionary<string, string>(StringComparer.Ordinal);//区分大小写排序
            foreach (KeyValuePair<string, string> param in parameters)
            {
                if (!param.Key.ToLower().Equals("sign"))
                {
                    sortedParams.Add(param.Key, param.Value);
                }
            }

            return sortedParams;
        }

        /// <summary>
        /// 拼接请求参数:把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串
        /// </summary>
        /// <param name="paramList"></param>
        /// <returns></returns>
        private static string CreateLinkString(SortedDictionary<string, string> paramList)
        {
            var sPara = paramList.Select(p => p.Key + "=" + p.Value);//组合key=value
            string prestr = String.Join("&", sPara);//用&字符拼接各个键值对

            return prestr;
        }

        /// <summary>
        /// 编码参数值：发送请求前需要把参数值进行UrlEncode
        /// </summary>
        /// <param name="sortedParams"></param>
        private static SortedDictionary<string, string> EncodingParamValue(IDictionary<string, string> param)
        {
            SortedDictionary<string, string> sortedParams = new SortedDictionary<string, string>();
            foreach (var item in param)
            {
                sortedParams[item.Key] = WebUtility.UrlEncode(item.Value);
            }
            return sortedParams;
        }
    }
}
