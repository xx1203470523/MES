/*
 *creator: Karl
 *
 *describe: 托盘信息    Dto | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-05-16 10:57:03
 */

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
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

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

       /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

       
    }


    /// <summary>
    /// 托盘信息新增Dto
    /// </summary>
    public record InteTrayCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

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

       /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

       
    }

    /// <summary>
    /// 托盘信息更新Dto
    /// </summary>
    public record InteTrayModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

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

       /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

       

    }

    /// <summary>
    /// 托盘信息分页Dto
    /// </summary>
    public class InteTrayPagedQueryDto : PagerInfo
    {
    }
}
