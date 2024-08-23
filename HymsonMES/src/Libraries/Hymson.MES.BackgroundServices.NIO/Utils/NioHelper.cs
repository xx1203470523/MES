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
            date = date.AddHours(-8);
            return (long)((DateTime)date - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local)).TotalSeconds;
        }
    }
}
