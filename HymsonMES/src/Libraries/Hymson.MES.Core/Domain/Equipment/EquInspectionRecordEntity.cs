using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 数据实体（点检记录表）   
    /// equ_inspection_record
    /// @author User
    /// @date 2024-04-03 04:50:07
    /// </summary>
    public class EquInspectionRecordEntity : BaseEntity
    {
        /// <summary>
        /// 点检单号
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 点检任务Id
        /// </summary>
        public long InspectionTaskSnapshootId { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartExecuTime { get; set; }

        /// <summary>
        /// 1、待检验2、检验中3、已完成
        /// </summary>
        public EquInspectionRecordStatusEnum Status { get; set; }

        /// <summary>
        /// 是否合格
        /// </summary>
        public YesOrNoEnum? IsQualified { get; set; }

        /// <summary>
        /// 是否通知维修
        /// </summary>
        public YesOrNoEnum? IsNoticeRepair { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }       
    }
}
