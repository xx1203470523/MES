using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 数据实体对象（设备故障原因关联设备故障现象）
    /// @author Czhipu
    /// @date 2022-11-14
    /// </summary>
    public class EquFaultReasonPhenomenonEntity : BaseEntity
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 设备故障原因id
        /// </summary>
        public long FaultReasonId { get; set; }

        /// <summary>
        /// 设备故障现象id
        /// </summary>
        public long FaultPhenomenonId { get; set; }
    }
}
