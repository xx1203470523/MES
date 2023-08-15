using System.Collections.Concurrent;

namespace Hymson.MES.SystemServicesTests
{
    /// <summary>
    /// 当前系统信息
    /// 用于模拟当前系统信息
    /// 必须拥有Id，Name，FactoryId，SiteId，SiteName
    /// </summary>
    public static class CurrentSystemInfo
    {
        /// <summary>
        /// 设备信息
        /// </summary>
        private static Lazy<ConcurrentDictionary<string, object>> systemDic = new(() => new());

        /// <summary>
        /// 设备信息
        /// </summary>
        public static Lazy<ConcurrentDictionary<string, object>> SystemDic
        {
            get { return systemDic; }
            set { systemDic = value; }
        }

        /// <summary>
        /// 批量添加或更新值
        /// </summary>
        /// <param name="dic"></param>
        public static void AddUpdate(Dictionary<string, object> dic)
        {
            foreach (var item in dic)
            {
                SystemDic.Value.AddOrUpdate(item.Key, item.Value, (_key, _value) => item.Value);
            }
        }
    }
}
