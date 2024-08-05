using System.Data.SqlTypes;
using System.Text.Json;

namespace Hymson.MES.CoreServices.Extension
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class ExtensionsTime
    {
        /// <summary>
        /// 序列化为字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime ToTime(this object obj)
        {
            // 将obj转换为DateTime，先用DateTime.Try
            if (DateTime.TryParse(obj.ToString(), out DateTime time))
            {
                return time;
            }

            return SqlDateTime.MinValue.Value;
        }

    }
}
