using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Integrated
{
    /// <summary>
    /// Dto（容器维护）
    /// </summary>
    public record InteContainerDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 定义方式;0-物料，1-物料组
        /// </summary>
        public bool DefinitionMethod { get; set; }

        /// <summary>
        /// 物料id
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 物料组Id
        /// </summary>
        public long MaterialGroupId { get; set; }

        /// <summary>
        /// 状态;0-新建 1-启用 2-保留3-废弃
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 最大值
        /// </summary>
        public decimal Maximum { get; set; }

        /// <summary>
        /// 最小值
        /// </summary>
        public decimal MInimun { get; set; }

        /// <summary>
        /// 高度(mm)
        /// </summary>
        public decimal? Height { get; set; }

        /// <summary>
        /// 长度(mm)
        /// </summary>
        public decimal? Length { get; set; }

        /// <summary>
        /// 宽度(mm)
        /// </summary>
        public decimal? Width { get; set; }

        /// <summary>
        /// 最大填充重量(KG)
        /// </summary>
        public decimal? MaxFillWeight { get; set; }

        /// <summary>
        /// 重量(KG)
        /// </summary>
        public decimal? Weight { get; set; }

        /// <summary>
        /// 创建人;创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间;创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 更新人;更新人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间;更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }


    }

    /// <summary>
    /// 新增Dto（容器维护）
    /// </summary>
    public record InteContainerSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 定义方式;0-物料，1-物料组
        /// </summary>
        public bool DefinitionMethod { get; set; }

        /// <summary>
        /// 物料id
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 物料组Id
        /// </summary>
        public long MaterialGroupId { get; set; }

        /// <summary>
        /// 状态;0-新建 1-启用 2-保留3-废弃
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 最大值
        /// </summary>
        public decimal Maximum { get; set; }

        /// <summary>
        /// 最小值
        /// </summary>
        public decimal MInimun { get; set; }

        /// <summary>
        /// 高度(mm)
        /// </summary>
        public decimal? Height { get; set; }

        /// <summary>
        /// 长度(mm)
        /// </summary>
        public decimal? Length { get; set; }

        /// <summary>
        /// 宽度(mm)
        /// </summary>
        public decimal? Width { get; set; }

        /// <summary>
        /// 最大填充重量(KG)
        /// </summary>
        public decimal? MaxFillWeight { get; set; }

        /// <summary>
        /// 重量(KG)
        /// </summary>
        public decimal? Weight { get; set; }

        /// <summary>
        /// 创建人;创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间;创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 更新人;更新人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间;更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }


    }

    /// <summary>
    /// 分页Dto（容器维护）
    /// </summary>
    public class InteContainerPagedQueryDto : PagerInfo
    {
        ///// <summary>
        ///// 描述 :站点编码 
        ///// 空值 : false  
        ///// </summary>
        //public string SiteCode { get; set; }
    }
}
