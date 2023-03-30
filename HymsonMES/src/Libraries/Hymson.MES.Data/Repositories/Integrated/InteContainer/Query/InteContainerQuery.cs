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
    }
}
