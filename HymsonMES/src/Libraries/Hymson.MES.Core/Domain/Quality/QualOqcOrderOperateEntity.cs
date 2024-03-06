using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 数据实体（OQC检验单操作记录）   
    /// qual_oqc_order_operate
    /// @author xiaofei
    /// @date 2024-03-04 10:54:09
    /// </summary>
    public class QualOqcOrderOperateEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// OQC检验单Id
        /// </summary>
        public long OQCOrderId { get; set; }

        /// <summary>
        /// 操作类型(1-开始检验 2-完成检验 3-关闭检验)
        /// </summary>
        public OrderOperateTypeEnum OperateType { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string OperateBy { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperateOn { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
