using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 数据实体（设备点检保养项目）   
    /// equ_inspection_item
    /// @author User
    /// @date 2024-04-03 04:49:48
    /// </summary>
    public class EquInspectionItemEntity : BaseEntity
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 操作方法
        /// </summary>
        public string OperationMethod { get; set; }

        /// <summary>
        /// 操作内容
        /// </summary>
        public string OperationContent { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; } = "";

        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }      
    }
}
