using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 数据实体（点检记录详情表）   
    /// equ_inspection_record_details
    /// @author User
    /// @date 2024-04-03 04:51:12
    /// </summary>
    public class EquInspectionRecordDetailsEntity : BaseEntity
    {
        /// <summary>
        /// 详情id
        /// </summary>
        public long InspectionRecordId { get; set; }

        /// <summary>
        /// 详情id
        /// </summary>
        public long InspectionTaskDetailSnapshootId { get; set; }

        /// <summary>
        /// 点检结果
        /// </summary>
        public string InspectionResult { get; set; }

        /// <summary>
        /// 是否合格
        /// </summary>
        public TrueOrFalseEnum? IsQualified { get; set; }

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
