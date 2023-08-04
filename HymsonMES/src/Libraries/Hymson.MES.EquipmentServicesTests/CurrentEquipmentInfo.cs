using System.Collections.Concurrent;

namespace Hymson.MES.EquipmentServicesTests
{
    /// <summary>
    /// 当前设备信息
    /// 用于模拟当前操作设备信息
    /// 必须拥有Id，Code，Name，FactoryId，SiteId，SiteName
    /// </summary>
    public static class CurrentEquipmentInfo
    {
        /// <summary>
        /// 设备信息
        /// </summary>
        private static Lazy<ConcurrentDictionary<string, object>> equipmentInfoDic = new(() => new());

        /// <summary>
        /// 设备信息
        /// </summary>
        public static Lazy<ConcurrentDictionary<string, object>> EquipmentInfoDic
        {
            get { return equipmentInfoDic; }
            set { equipmentInfoDic = value; }
        }

        /// <summary>
        /// 批量添加或更新值
        /// </summary>
        /// <param name="dic"></param>
        public static void AddUpdate(Dictionary<string, object> dic)
        {
            foreach (var item in dic)
            {
                EquipmentInfoDic.Value.AddOrUpdate(item.Key, item.Value, (_key, _value) => item.Value);
            }
        }
    }
}
