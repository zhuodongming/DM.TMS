using System;
using System.Collections.Generic;
using System.Text;

namespace DM.Infrastructure.Helper
{
    /// <summary>
    /// Time Helper
    /// </summary>
    public static class Time
    {
        private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// 获取秒级时间戳
        /// </summary>
        /// <returns></returns>
        public static int GetTimestamp()
        {
            return (int)(DateTime.UtcNow - Jan1st1970).TotalSeconds;
        }

        /// <summary>
        /// 获取毫秒级时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetTimestampByMS()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }

        /// <summary>
        /// DateTime转换为秒级时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int ToTimestamp(DateTime time)
        {
            return (int)(time.ToUniversalTime() - Jan1st1970).TotalSeconds;
        }

        /// <summary>
        /// DateTime转换为毫秒级时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long ToTimestampByMS(DateTime time)
        {
            return (int)(time.ToUniversalTime() - Jan1st1970).TotalMilliseconds;
        }

        /// <summary>
        /// 秒级时间戳转换为DateTime
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(long timestamp)
        {
            return Jan1st1970.AddSeconds(timestamp).ToLocalTime();
        }

        /// <summary>
        /// 毫秒级时间戳转换为DateTime
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime ToDateTimeByMS(long timestamp)
        {
            return Jan1st1970.AddMilliseconds(timestamp).ToLocalTime();
        }
    }
}
