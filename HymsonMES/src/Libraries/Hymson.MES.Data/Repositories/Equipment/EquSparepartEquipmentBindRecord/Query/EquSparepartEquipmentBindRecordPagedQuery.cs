using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Equipment;

namespace Hymson.MES.Data.Repositories.Equipment.Query
{
    /// <summary>
    /// 工具绑定设备操作记录表 分页参数
    /// </summary>
    public class EquSparepartEquipmentBindRecordPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 备件编码
        /// </summary>
        public string? SparepartCode { get; set; }

        /// <summary>
        /// 备件名称
        /// </summary>
        public string? SparepartName { get; set; }

        /// <summary>
        /// 备件类型
        /// </summary>
        public string? SparePartType { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string? EquipmentCode { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        public string? Position { get; set; }

        /// <summary>
        /// 操作类型1、安装 2、卸载
        /// </summary>
        public BindOperationTypeEnum? OperationType { get; set; }

        /// <summary>
        /// 安装时间  时间范围  数组
        /// </summary>
        public DateTime[]? InstallTimeRange { get; set; }

        /// <summary>
        /// 卸载时间  时间范围  数组
        /// </summary>
        public DateTime[]? UninstallTimeRange { get; set; }
    }
}
