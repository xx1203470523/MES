/*
 *creator: Karl
 *
 *describe: 系统Token    Dto | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-06-15 02:09:57
 */

using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Integrated
{
    /// <summary>
    /// 系统TokenDto
    /// </summary>
    public record InteSystemTokenDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 系统编码
        /// </summary>
        public string SystemCode { get; set; }

       /// <summary>
        /// 设备名称
        /// </summary>
        public string SystemName { get; set; }

       /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }

       /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpirationTime { get; set; }

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
        /// 是否删除
        /// </summary>
        public long IsDeleted { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }


    /// <summary>
    /// 系统Token新增Dto
    /// </summary>
    public record InteSystemTokenCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 系统编码
        /// </summary>
        public string SystemCode { get; set; }

       /// <summary>
        /// 设备名称
        /// </summary>
        public string SystemName { get; set; }

       /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }

       /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpirationTime { get; set; }

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
        /// 是否删除
        /// </summary>
        public long IsDeleted { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }

    /// <summary>
    /// 系统Token更新Dto
    /// </summary>
    public record InteSystemTokenModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 系统编码
        /// </summary>
        public string SystemCode { get; set; }

       /// <summary>
        /// 设备名称
        /// </summary>
        public string SystemName { get; set; }

       /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }

       /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpirationTime { get; set; }

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
        /// 是否删除
        /// </summary>
        public long IsDeleted { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       

    }

    /// <summary>
    /// 系统Token分页Dto
    /// </summary>
    public class InteSystemTokenPagedQueryDto : PagerInfo
    {
    }
}
