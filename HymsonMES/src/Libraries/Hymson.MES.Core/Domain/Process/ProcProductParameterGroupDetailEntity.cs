using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 数据实体（产品工序参数项目表）   
    /// proc_product_parameter_group_detail
    /// @author Czhipu
    /// @date 2023-07-25 02:15:39
    /// </summary>
    public class ProcProductParameterGroupDetailEntity : BaseEntity
    {
        /// <summary>
        /// 产品检验参数id
        /// </summary>
        public long parameterGroupId { get; set; }

       /// <summary>
        /// 参数组id
        /// </summary>
        public long ProductParameterId { get; set; }

       /// <summary>
        /// 规格上限
        /// </summary>
        public decimal UpperLimit { get; set; }

       /// <summary>
        /// 中心值（均值）
        /// </summary>
        public decimal? CenterValue { get; set; }

       /// <summary>
        /// 规格下限
        /// </summary>
        public decimal LowerLimit { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }
       
    }
}
