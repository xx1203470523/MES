using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 数据实体（产品检验参数组）   
    /// proc_product_parameter_group
    /// @author Czhipu
    /// @date 2023-07-25 01:58:43
    /// </summary>
    public class ProcProductParameterGroupEntity : BaseEntity
    {
        /// <summary>
        /// 参数集编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 参数集名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

       /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

       /// <summary>
        /// 物料id
        /// </summary>
        public long MaterialId { get; set; }

       /// <summary>
        /// 工序id
        /// </summary>
        public long ProcedureId { get; set; }

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
