using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Data.Repositories.Common.Query;

namespace Hymson.MES.Data.Repositories.Integrated.InteWorkCenter.Query
{
    /// <summary>
    /// 工作中心表分页参数
    /// @author admin
    /// @date 2023-02-22
    /// </summary>
    public class EntityByTypeQuery : EntityBySiteIdQuery
    {
        /// <summary>
        /// 类型(工厂/车间/产线) 
        /// </summary>
        public WorkCenterTypeEnum Type { get; set; }


    }
}