using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DM.Infrastructure.Helper
{
    /// <summary>
    /// Json Helper
    /// </summary>
    public static class Json
    {
        /// <summary>
        /// 转换为json字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 转换为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns>T</returns>
        public static T ToObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
