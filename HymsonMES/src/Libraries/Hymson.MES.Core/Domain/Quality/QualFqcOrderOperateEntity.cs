using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 数据实体（FQC检验单操作记录）   
    /// qual_fqc_order_operate
    /// @author User
    /// @date 2024-03-27 02:09:05
    /// </summary>
    public class QualFqcOrderOperateEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// FQC检验单Id
        /// </summary>
        public long FQCOrderId { get; set; }

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
