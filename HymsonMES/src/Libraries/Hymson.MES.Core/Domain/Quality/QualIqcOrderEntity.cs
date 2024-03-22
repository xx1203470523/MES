using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 数据实体（iqc检验单）   
    /// qual_iqc_order
    /// @author Czhipu
    /// @date 2024-03-06 02:26:10
    /// </summary>
    public class QualIqcOrderEntity : BaseEntity
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 检验单号
        /// </summary>
        public string InspectionOrder { get; set; }

        /// <summary>
        /// 物料id
        /// </summary>
        public long? MaterialId { get; set; }

        /// <summary>
        /// 供应商Id
        /// </summary>
        public long? SupplierId { get; set; }

        /// <summary>
        /// qual_iqc_inspection_item_snapshot 的Id
        /// </summary>
        public long IqcInspectionItemSnapshotId { get; set; }

        /// <summary>
        /// 收货单详情Id
        /// </summary>
        public long MaterialReceiptDetailId { get; set; }

        /// <summary>
        /// 检验等级(1-正常 2-加严 3-放宽)
        /// </summary>
        public InspectionGradeEnum InspectionGrade { get; set; }

        /// <summary>
        /// 是否免检
        /// </summary>
        public TrueOrFalseEnum IsExemptInspection { get; set; } = TrueOrFalseEnum.No;

        /// <summary>
        /// 接收水准
        /// </summary>
        public int AcceptanceLevel { get; set; }

        /// <summary>
        /// 状态;1、待检验2、检验中3、已检验4、已关闭
        /// </summary>
        public InspectionStatusEnum Status { get; set; }

        /// <summary>
        /// 是否合格;0、不合格 1、合格
        /// </summary>
        public TrueOrFalseEnum? IsQualified { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }


    }
}
