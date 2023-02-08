using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 数据实体对象（设备故障解决措施关联设备故障原因）
    /// @author Czhipu
    /// @date 2022-11-14
    /// </summary>
    public class EquFaultSolutionReason : BaseEntity
    {
        /// <summary>SqlBuilderSqlBuilder
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public string SiteCode { get; set; }

        /// <summary>
        /// 设备故障原因id
        /// </summary>
        public long FaultReasonId { get; set; }

        /// <summary>
        /// 设备故障解决措施id
        /// </summary>
        public long FaultSolutionId { get; set; }
    }
}
