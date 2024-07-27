using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Services.Dtos.Manufacture
{
    /// <summary>
    /// 转子装箱记录表新增/更新Dto
    /// </summary>
    public record ManuRotorPackListSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 箱体码
        /// </summary>
        public string BoxCode { get; set; }

       /// <summary>
        /// 成品码
        /// </summary>
        public string ProductCode { get; set; }

       /// <summary>
        /// 轴码
        /// </summary>
        public string ProductNo { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

       /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }

    /// <summary>
    /// 转子装箱记录表Dto
    /// </summary>
    public record ManuRotorPackListDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 箱体码
        /// </summary>
        public string BoxCode { get; set; }

       /// <summary>
        /// 成品码
        /// </summary>
        public string ProductCode { get; set; }

       /// <summary>
        /// 轴码
        /// </summary>
        public string ProductNo { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

       /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }

    public record ManuRotorPackViewDto
    {
        /// <summary>
        /// 箱码
        /// </summary>
        public string BoxCode { get; set; } = "";

        /// <summary>
        /// 成品码
        /// </summary>
        public string Sfc { get; set; } = "";

        /// <summary>
        /// 品检状态
        /// </summary>
        public ProductReceiptQualifiedStatusEnum Type { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        public string Batch { get; set; }

        /// <summary>
        /// 仓库
        /// </summary>
        public string WarehouseCode { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WhCode { get; set; }
    }

    /// <summary>
    /// 转子装箱记录表分页Dto
    /// </summary>
    public class ManuRotorPackListPagedQueryDto : PagerInfo { }

}
