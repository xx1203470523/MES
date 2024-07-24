using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 数据视图对象（容器维护）
    /// inte_container
    /// @author Czhipu
    /// @date 2023-03-08 09:21:27
    /// </summary>
    public class InteContainerView : BaseEntity
    {
        /// <summary>
        /// 容器名称
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// 容器编码
        /// </summary>
        public string Code { get; set; } = "";

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; } = "";

        /// <summary>
        /// 物料id
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 物料组Id
        /// </summary>
        public long MaterialGroupId { get; set; }

        /// <summary>
        /// 定义方式;0-物料，1-物料组
        /// </summary>
        public DefinitionMethodEnum DefinitionMethod { get; set; }

        /// <summary>
        /// 包装等级（分为一级/二级/三级）
        /// </summary>
        public LevelEnum Level { get; set; }

        /// <summary>
        /// 状态;0-新建 1-启用 2-保留3-废弃
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

        /// <summary>
        /// 最大值
        /// </summary>
        public decimal Maximum { get; set; }

        /// <summary>
        /// 最小值
        /// </summary>
        public decimal Minimum { get; set; }

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
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";
    }

    /// <summary>
    /// 容器表Dto
    /// </summary>
    public record InteContainerInfoViewDto : BaseEntityDto
    {
        /// <summary>
        /// 容器名称
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// 容器编码
        /// </summary>
        public string Code { get; set; } = "";

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; } = "";

        /// <summary>
        /// 品检状态
        /// </summary>
        public ProductReceiptQualifiedStatusEnum Type { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

       /// <summary>
       /// 数量
       /// </summary>
        public decimal Qty {  get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        public string Batch { get; set; }

        /// <summary>
        /// 仓库
        /// </summary>
        public string WarehouseCode {  get; set; }
    }
}
