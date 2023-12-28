using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;

namespace Hymson.MES.Data.Repositories.Integrated.InteContainer.Query
{
    /// <summary>
    /// 容器维护 分页参数
    /// </summary>
    public class InteContainerPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点编码 
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 物料名称/物料组名称
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
        /// 定义方式;0-物料，1-物料组
        /// </summary>
        public DefinitionMethodEnum? DefinitionMethod { get; set; }

        /// <summary>
        /// 包装等级（分为一级/二级/三级）
        /// </summary>
        public LevelEnum? Level { get; set; }

        /// <summary>
        /// 状态;0-新建 1-启用 2-保留3-废弃
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }
    }
}
