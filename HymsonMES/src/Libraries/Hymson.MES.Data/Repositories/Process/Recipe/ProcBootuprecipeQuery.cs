/*
 *creator: Karl
 *
 *describe: 开机配方表 查询类 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-07-05 04:11:36
 */

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 开机配方表 查询参数
    /// </summary>
    public class ProcBootuprecipeQuery
    {
       /// <summary>
       /// 设备组ID
       /// </summary>
        public long EquGroupId { get; set; }
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }
       
    }
}
