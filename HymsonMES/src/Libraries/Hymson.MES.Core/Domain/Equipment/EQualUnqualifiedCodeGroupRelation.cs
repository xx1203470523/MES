using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 设备故障类型关联故障现象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class EQualUnqualifiedCodeGroupRelation : BaseEntity
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 描述 :故障类型ID;equ_fault_type的Id
        /// 空值 : false  
        /// </summary>
        public long FaultTypeId { get; set; }

        /// <summary>
        /// 描述 :故障现象ID;equ_fault_phenomenon的Id
        /// 空值 : false  
        /// </summary>
        public long FaultPhenomenonId { get; set; }

        /// <summary>
        /// 描述 :说明 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; } = "";
    }
}