/*
 *creator: Karl
 *
 *describe: 烘烤工序    Dto | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-08-02 07:32:20
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Services.Dtos.Manufacture
{
    /// <summary>
    /// 烘烤工序Dto
    /// </summary>
    public record ManuBakingDto : BaseEntityDto
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
        /// 设备Id
        /// </summary>
        public long EquipmentId { get; set; }

       /// <summary>
        /// 极卷条码
        /// </summary>
        public string SFC { get; set; }

       /// <summary>
        /// 进站时间
        /// </summary>
        public DateTime BakingOn { get; set; }

       /// <summary>
        /// 出站时间
        /// </summary>
        public DateTime? BakingEnd { get; set; }

       /// <summary>
        /// 烘烤预计总时长
        /// </summary>
        public int? BakingPlan { get; set; }
        /// <summary>
        /// 烘烤总时长
        /// </summary>
        public int BakingTotalTimeSpan { get; set; }
        /// <summary>
        /// 烘烤状态
        /// </summary>
        public BakingStatusEnum BakingStatus { get; set; }

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
    /// 烘烤工序新增Dto
    /// </summary>
    public record ManuBakingCreateDto : BaseEntityDto
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
        /// 设备Id
        /// </summary>
        public long EquipmentId { get; set; }

       /// <summary>
        /// 极卷条码
        /// </summary>
        public string SFC { get; set; }

       /// <summary>
        /// 进站时间
        /// </summary>
        public DateTime BakingOn { get; set; }
       
    }

    /// <summary>
    /// 烘烤工序更新Dto
    /// </summary>
    public record ManuBakingModifyDto : BaseEntityDto
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
        /// 设备Id
        /// </summary>
        public long EquipmentId { get; set; }

       /// <summary>
        /// 极卷条码
        /// </summary>
        public string SFC { get; set; }

       /// <summary>
        /// 进站时间
        /// </summary>
        public DateTime BakingOn { get; set; }

       /// <summary>
        /// 出站时间
        /// </summary>
        public DateTime? BakingEnd { get; set; }

       /// <summary>
        /// 烘烤预计总时长
        /// </summary>
        public int? BakingPlan { get; set; }

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
    /// 烘烤工序分页Dto
    /// </summary>
    public class ManuBakingPagedQueryDto : PagerInfo
    {
    }
}
