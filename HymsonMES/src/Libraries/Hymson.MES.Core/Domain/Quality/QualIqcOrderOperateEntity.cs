using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 数据实体（iqc检验单操作表）   
    /// qual_iqc_order_operate
    /// @author Czhipu
    /// @date 2024-03-06 02:26:24
    /// </summary>
    public class QualIqcOrderOperateEntity : BaseEntity
    {
        /// <summary>
        /// IQC检验单Id
        /// </summary>
        public long IQCOrderId { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string OperateBy { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime? OperateOn { get; set; }

        /// <summary>
        /// 操作类型;1、开始检验2、完成检验3、关闭检验
        /// </summary>
        public OrderOperateTypeEnum OperationType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }


    }
}
