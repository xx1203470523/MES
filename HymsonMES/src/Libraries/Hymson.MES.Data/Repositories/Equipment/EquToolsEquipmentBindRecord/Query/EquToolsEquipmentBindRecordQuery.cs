using Hymson.MES.Core.Enums.Equipment;

namespace Hymson.MES.Data.Repositories.Equipment.Query
{
    /// <summary>
    /// 工具绑定设备操作记录表 查询参数
    /// </summary>
    public class EquToolsEquipmentBindRecordQuery
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 设备Id
        /// </summary>
        public long? EquipmentId { get; set; }

        /// <summary>
        /// 工具Id
        /// </summary>
        public long? ToolId { get; set; }

        /// <summary>
        /// 位置号
        /// </summary>
        public string? Position { get; set; }

        /// <summary>
        /// 操作类型1、绑定 2、卸载
        /// </summary>
        public BindOperationTypeEnum? OperationType { get; set; }
    }
}
