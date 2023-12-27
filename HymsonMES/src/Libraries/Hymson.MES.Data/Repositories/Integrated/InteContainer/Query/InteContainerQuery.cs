using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;

namespace Hymson.MES.Data.Repositories.Integrated.InteContainer.Query
{
    /// <summary>
    /// 容器维护 查询参数
    /// </summary>
    public class InteContainerQuery
    {
        /// <summary>
        /// 定义方式;0-物料，1-物料组
        /// </summary>
        public DefinitionMethodEnum DefinitionMethod { get; set; }

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
        public LevelEnum Level { get; set; }

        /// <summary>
        /// 状态;0-新建 1-启用 2-保留3-废弃
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }
    }

    /// <summary>
    /// 查询实体
    /// </summary>
    public class EntityByContainerIdQuery
    {
        /// <summary>
        /// 工厂
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 容器Id
        /// </summary>
        public long? ContainerId { get; set; }

    }
}
