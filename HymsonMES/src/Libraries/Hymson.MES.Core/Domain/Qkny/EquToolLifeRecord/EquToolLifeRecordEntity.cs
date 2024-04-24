using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.EquToolLifeRecord
{
    /// <summary>
    /// 数据实体（设备夹具寿命）   
    /// equ_tool_life_record
    /// @author Yxx
    /// @date 2024-03-21 04:21:48
    /// </summary>
    public class EquToolLifeRecordEntity : BaseEntity
    {
        /// <summary>
        /// 设备id
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 夹具ID
        /// </summary>
        public long? ToolId { get; set; }

        /// <summary>
        /// 夹具编码
        /// </summary>
        public string ToolCode { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        public decimal ToolLife { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        
    }
}
