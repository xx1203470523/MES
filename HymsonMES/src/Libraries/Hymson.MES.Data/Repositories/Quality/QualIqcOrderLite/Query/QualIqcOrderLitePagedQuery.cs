using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// iqc检验单 分页参数
    /// </summary>
    public class QualIqcOrderLitePagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 检验单号
        /// </summary>
        public string? InspectionOrder { get; set; }

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
        /// 检验状态;1、待检验2、检验中3、已检验4、已关闭
        /// </summary>
        public InspectionStatusEnum? Status { get; set; }

        /// <summary>
        /// 是否免检
        /// </summary>
        public TrueOrFalseEnum? IsExemptInspection { get; set; }

        /// <summary>
        /// 是否合格;0、不合格 1、合格
        /// </summary>
        public TrueOrFalseEnum? IsQualified { get; set; }

        /// <summary>
        /// ID集合（物料）
        /// </summary>
        public IEnumerable<long>? MaterialIds { get; set; }

        /// <summary>
        /// ID集合（供应商）
        /// </summary>
        public IEnumerable<long>? SupplierIds { get; set; }

        /// <summary>
        /// 收货单Id
        /// </summary>
        public IEnumerable<long>? ReceiptIds { get; set; }

    }
}
