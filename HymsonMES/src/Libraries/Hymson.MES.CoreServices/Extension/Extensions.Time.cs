namespace Hymson.MES.CoreServices.Extension
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExtensionsTime
    {
        /// <summary>
        /// 序列化为字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToDateTime(this DateTime time)
        {
            return time.ToString("yyyy-MM-dd HH:mm:ss");
        }

    }

}
