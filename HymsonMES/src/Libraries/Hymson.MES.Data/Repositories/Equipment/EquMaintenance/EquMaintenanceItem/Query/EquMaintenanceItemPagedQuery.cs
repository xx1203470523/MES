using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Equipment;
using Hymson.MES.Core.Enums.Equipment.EquMaintenance;

namespace Hymson.MES.Data.Repositories.Equipment.EquMaintenance.EquMaintenanceItem.Query
{
    /// <summary>
    /// 设备保养项目 分页参数
    /// </summary>
    public class EquMaintenanceItemPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }


        /// <summary>
        /// 数据类型
        /// </summary>
        public EquMaintenanceDataTypeEnum? DataType { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public DisableOrEnableEnum? Status { get; set; }

    }
}
