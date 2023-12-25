using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 数据实体（设备故障解决措施）   
    /// equ_fault_solution
    /// @author Czhipu
    /// @date 2023-12-19 07:11:01
    /// </summary>
    public class EquFaultSolutionEntity : BaseEntity
    {
        /// <summary>
        /// 故障解决措施代码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 故障解决措施代码
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述 :是否启用 0-禁用 1-启用 
        /// 空值 : false  
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

        /// <summary>
        /// 描述 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }
       
    }
}
