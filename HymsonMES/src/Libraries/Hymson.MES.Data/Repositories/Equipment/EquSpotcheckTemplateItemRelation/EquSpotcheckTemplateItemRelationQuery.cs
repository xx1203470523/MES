/*
 *creator: Karl
 *
 *describe: 设备点检模板与项目关系 查询类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-05-13 03:22:39
 */

namespace Hymson.MES.Data.Repositories.EquSpotcheckTemplateItemRelation
{
    /// <summary>
    /// 设备点检模板与项目关系 查询参数
    /// </summary>
    public class EquSpotcheckTemplateItemRelationQuery
    {
        /// <summary>
        /// 站点
        /// </summary>
        public long? SiteId { get; set; } 
        
        /// <summary>
        /// 模板IDs
        /// </summary>
        public IEnumerable<long>? SpotCheckTemplateIds { get; set; }

    }
}
