using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Integrated
{
    /// <summary>
    /// 托盘信息Dto
    /// </summary>
    public record InteTrayDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 托盘编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 托盘名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 装载数量
        /// </summary>
        public decimal MaxLoadQty { get; set; }

        /// <summary>
        /// 最大序号
        /// </summary>
        public int? MaxSeq { get; set; }

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


    }

    /// <summary>
    /// 托盘信息更新Dto
    /// </summary>
    public record InteTraySaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 托盘编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 托盘名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 装载数量
        /// </summary>
        public decimal MaxLoadQty { get; set; }

        /// <summary>
        /// 最大序号
        /// </summary>
        public int? MaxSeq { get; set; }

    }

    /// <summary>
    /// 托盘信息分页Dto
    /// </summary>
    public class InteTrayPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 编码（托盘信息）
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 名称（托盘信息）
        /// </summary>
        public string? Name { get; set; }
    }
}
