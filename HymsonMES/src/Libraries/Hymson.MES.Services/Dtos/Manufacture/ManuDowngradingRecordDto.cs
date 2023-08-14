/*
 *creator: Karl
 *
 *describe: 降级品录入记录    Dto | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-08-10 10:15:49
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Services.Dtos.Manufacture
{
    /// <summary>
    /// 降级品录入记录Dto
    /// </summary>
    public record ManuDowngradingRecordDto : BaseEntityDto
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
        /// 是否取消;0 否 1是
        /// </summary>
        public ManuDowngradingRecordTypeEnum? IsCancellation { get; set; }

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

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

    }

    /// <summary>
    /// 降级品录入记录分页Dto
    /// </summary>
    public class ManuDowngradingRecordPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string? SFC { get; set; }

        /// <summary>
        /// 品级
        /// </summary>
        public string? Grade { get; set; }
    }
}
