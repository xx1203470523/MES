using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 容器维护，数据实体对象   
    /// inte_container
    /// @author Czhipu
    /// @date 2023-03-08 09:21:27
    /// </summary>
    public class InteContainerEntity : BaseEntity
    {
        /// <summary>
        /// 站点ID 
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 定义方式;0-物料，1-物料组
        /// </summary>
        public int DefinitionMethod { get; set; }

       /// <summary>
        /// 物料id
        /// </summary>
        public long MaterialId { get; set; }

       /// <summary>
        /// 物料组Id
        /// </summary>
        public long MaterialGroupId { get; set; }

        /// <summary>
        /// 包装等级（分为一级/二级/三级）
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 状态;0-新建 1-启用 2-保留3-废弃
        /// </summary>
        public int Status { get; set; }

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
}
