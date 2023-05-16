/*
 *creator: Karl
 *
 *describe: 托盘装载信息表    Dto | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-05-16 11:10:43
 */

using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Manufacture
{
    /// <summary>
    /// 托盘装载信息表Dto
    /// </summary>
    public record ManuTrayLoadDto : BaseEntityDto
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
        /// 托盘条码
        /// </summary>
        public string TrayCode { get; set; }

       /// <summary>
        /// 托盘id
        /// </summary>
        public long TrayId { get; set; }

       /// <summary>
        /// 装载数量
        /// </summary>
        public decimal LoadQty { get; set; }

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
        public bool IsDeleted { get; set; }

       
    }


    /// <summary>
    /// 托盘装载信息表新增Dto
    /// </summary>
    public record ManuTrayLoadCreateDto : BaseEntityDto
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
        /// 托盘条码
        /// </summary>
        public string TrayCode { get; set; }

       /// <summary>
        /// 托盘id
        /// </summary>
        public long TrayId { get; set; }

       /// <summary>
        /// 装载数量
        /// </summary>
        public decimal LoadQty { get; set; }

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
        public bool IsDeleted { get; set; }

       
    }

    /// <summary>
    /// 托盘装载信息表更新Dto
    /// </summary>
    public record ManuTrayLoadModifyDto : BaseEntityDto
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
        /// 托盘条码
        /// </summary>
        public string TrayCode { get; set; }

       /// <summary>
        /// 托盘id
        /// </summary>
        public long TrayId { get; set; }

       /// <summary>
        /// 装载数量
        /// </summary>
        public decimal LoadQty { get; set; }

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
        public bool IsDeleted { get; set; }

       

    }

    /// <summary>
    /// 托盘装载信息表分页Dto
    /// </summary>
    public class ManuTrayLoadPagedQueryDto : PagerInfo
    {
    }
}
