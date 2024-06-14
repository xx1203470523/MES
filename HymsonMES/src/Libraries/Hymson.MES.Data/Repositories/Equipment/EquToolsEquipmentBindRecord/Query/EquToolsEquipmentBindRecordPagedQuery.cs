using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Equipment;

namespace Hymson.MES.Data.Repositories.Equipment.Query
{
    /// <summary>
    /// 工具绑定设备操作记录表 分页参数
    /// </summary>
    public class EquToolsEquipmentBindRecordPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 工具编码
        /// </summary>
        public string? ToolCode { get; set; }

        /// <summary>
        /// 工具名称
        /// </summary>
        public string? ToolName { get; set; }

        /// <summary>
        /// 工具类型
        /// </summary>
        public string? ToolType { get; set; }

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
