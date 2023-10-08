/*
 *creator: Karl
 *
 *describe: 开机参数表 查询类 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-07-05 04:22:20
 */

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 开机参数表 查询参数
    /// </summary>
    public class ProcBootupparamQuery
    {
        /// <summary>
        /// 配方ID
        /// </summary>
        public long RecipeId { get; set; }
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }
    }
}
