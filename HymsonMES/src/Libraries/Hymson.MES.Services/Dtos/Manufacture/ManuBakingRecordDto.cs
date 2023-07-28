/*
 *creator: Karl
 *
 *describe: 烘烤执行表    Dto | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-07-28 05:42:41
 */

using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Manufacture
{
    /// <summary>
    /// 烘烤执行表Dto
    /// </summary>
    public record ManuBakingRecordDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 烘烤Id
        /// </summary>
        public long BakingId { get; set; }

       /// <summary>
        /// 位置
        /// </summary>
        public string Location { get; set; }

       /// <summary>
        /// 烘烤开始时间
        /// </summary>
        public DateTime BakingStart { get; set; }

       /// <summary>
        /// 烘烤结束时间
        /// </summary>
        public DateTime? BakingEnd { get; set; }

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
        public DateTime? UpdatedOn { get; set; }

       /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

       
    }


    /// <summary>
    /// 烘烤执行表新增Dto
    /// </summary>
    public record ManuBakingRecordCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 烘烤Id
        /// </summary>
        public long BakingId { get; set; }

       /// <summary>
        /// 位置
        /// </summary>
        public string Location { get; set; }

       /// <summary>
        /// 烘烤开始时间
        /// </summary>
        public DateTime BakingStart { get; set; }

       /// <summary>
        /// 烘烤结束时间
        /// </summary>
        public DateTime? BakingEnd { get; set; }

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
        public DateTime? UpdatedOn { get; set; }

       /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

       
    }

    /// <summary>
    /// 烘烤执行表更新Dto
    /// </summary>
    public record ManuBakingRecordModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 烘烤Id
        /// </summary>
        public long BakingId { get; set; }

       /// <summary>
        /// 位置
        /// </summary>
        public string Location { get; set; }

       /// <summary>
        /// 烘烤开始时间
        /// </summary>
        public DateTime BakingStart { get; set; }

       /// <summary>
        /// 烘烤结束时间
        /// </summary>
        public DateTime? BakingEnd { get; set; }

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
        public DateTime? UpdatedOn { get; set; }

       /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

       

    }

    /// <summary>
    /// 烘烤执行表分页Dto
    /// </summary>
    public class ManuBakingRecordPagedQueryDto : PagerInfo
    {
    }
}
