using Hymson.MES.Core.Enums;

namespace Hymson.MES.CoreServices.Bos.Integrated
{
    /// <summary>
    /// 
    /// </summary>
    public record EventTypeUpgradeBo
    {
        /// <summary>
        /// 升级Id
        /// </summary>
        public long EventTypeUpgradeId { get; set; }

        /// <summary>
        /// 级别;1、第一等级2、第二等级3、第三等级
        /// </summary>
        public UpgradeLevelEnum Level { get; set; }
    }
}
