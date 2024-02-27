/*
 *creator: Karl
 *
 *describe: 烘烤执行表    Dto | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-08-02 07:32:33
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
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

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
        /// 烘烤计划时长
        /// </summary>
        public int? BakingPlan { get; set; }

       /// <summary>
        /// 烘烤时长
        /// </summary>
        public int? BakingExecution { get; set; }

       /// <summary>
        /// 烘烤状态：0 烘烤中，1 正常结束，2终止
        /// </summary>
        public bool Status { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";

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
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

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
        /// 烘烤计划时长
        /// </summary>
        public int? BakingPlan { get; set; }

       /// <summary>
        /// 烘烤时长
        /// </summary>
        public int? BakingExecution { get; set; }

       /// <summary>
        /// 烘烤状态：0 烘烤中，1 正常结束，2终止
        /// </summary>
        public bool Status { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";

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
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

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
        /// 烘烤计划时长
        /// </summary>
        public int? BakingPlan { get; set; }

       /// <summary>
        /// 烘烤时长
        /// </summary>
        public int? BakingExecution { get; set; }

       /// <summary>
        /// 烘烤状态：0 烘烤中，1 正常结束，2终止
        /// </summary>
        public bool Status { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";

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
