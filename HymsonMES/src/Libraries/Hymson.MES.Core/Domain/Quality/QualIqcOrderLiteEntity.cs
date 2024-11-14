using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 数据实体（iqc检验单（新））   
    /// qual_iqc_order_lite
    /// @author Czhipu
    /// @date 2024-07-16 10:24:28
    /// </summary>
    public class QualIqcOrderLiteEntity : BaseEntity
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
        /// 收货单ID;wh_material_receipt的id
        /// </summary>
        public long MaterialReceiptId { get; set; }

        /// <summary>
        /// 通知单号
        /// </summary>
        public string? InformCode { get; set; }

        /// <summary>
        /// 同步单号
        /// </summary>
        public string? SyncCode { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string? WarehouseName { get; set; }


        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string? MaterialName { get; set; }

        /// <summary>
        /// 供应商Id
        /// </summary>
        public long? SupplierId { get; set; }

        /// <summary>
        /// 检验状态;1、待检验2、检验中3、已检验4、取消检验
        /// </summary>
        public IQCLiteStatusEnum Status { get; set; }

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
