using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundServices.NIO.Utils
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
            return (long)((DateTime)date - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local)).TotalSeconds;
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
