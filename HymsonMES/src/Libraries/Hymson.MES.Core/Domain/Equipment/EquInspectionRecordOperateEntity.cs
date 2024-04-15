using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 数据实体（点检操作表）   
    /// equ_inspection_record_operate
    /// @author User
    /// @date 2024-04-03 04:51:22
    /// </summary>
    public class EquInspectionRecordOperateEntity : BaseEntity
    {
        /// <summary>
        /// equ_inspection_record 的Id
        /// </summary>
        public long? InspectionRecordId { get; set; }

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

        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

        
    }
}
