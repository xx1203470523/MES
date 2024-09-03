using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Helper
{
    /// <summary>
    /// nio帮助类
    /// </summary>
    public static class NioHelper
    {
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <param name="createTime"></param>
        /// <param name="updateTime"></param>
        /// <returns></returns>
        public static long GetTimestamp(DateTime date)
        {
            DateTime utcDateTime = ((DateTime)date).ToUniversalTime();
            // 然后计算UTC时间与Unix纪元（1970年1月1日UTC）之间的差值  
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan timeSpan = utcDateTime - epoch;
            return (long)timeSpan.TotalSeconds;

            //date = date.AddHours(8);
            //return (long)((DateTime)date - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local)).TotalSeconds;
        }

        /// <summary>
        /// 将Unix时间戳转换为DateTime  
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime UnixTimestampMillisToDateTime(long timestamp)
        {
            // 将Unix时间戳转换为UTC DateTime  
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime utcDateTime = epoch.AddSeconds(timestamp);
            // 然后将UTC DateTime转换为本地时间  
            DateTime localDateTime = utcDateTime.ToLocalTime();
            return localDateTime;
        }

        /// <summary>
        /// 获取JSON序列化方式
        /// </summary>
        /// <returns></returns>
        public static JsonSerializerSettings GetJsonSerializer()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            return settings;
        }


    }

}
