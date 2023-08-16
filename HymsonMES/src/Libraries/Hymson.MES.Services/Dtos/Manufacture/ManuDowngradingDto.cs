/*
 *creator: Karl
 *
 *describe: 降级录入    Dto | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-08-10 10:15:26
 */

using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Manufacture
{
    /// <summary>
    /// 降级录入Dto
    /// </summary>
    public record ManuDowngradingDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

       /// <summary>
        /// 品级
        /// </summary>
        public string Grade { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

    }

    /// <summary>
    /// 保存降级录入
    /// </summary>
    public record ManuDowngradingSaveDto
    {
       /// <summary>
        /// 条码
        /// </summary>
        public string[] Sfcs { get; set; }

       /// <summary>
        /// 品级
        /// </summary>
        public string Grade { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

    }

    /// <summary>
    /// 降级录入分页Dto
    /// </summary>
    public class ManuDowngradingPagedQueryDto : PagerInfo
    {
    }

    public class ManuDowngradingQuerySfcsDto 
    {
        public string[] Sfcs { get; set; }
    }

    /// <summary>
    /// 保存降级移除
    /// </summary>
    public record ManuDowngradingSaveRemoveDto
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string[] Sfcs { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

    }
}
