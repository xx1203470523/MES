/*
 *creator: Karl
 *
 *describe: CCD文件 查询类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-05-17 11:09:19
 */

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// CCD文件 查询参数
    /// </summary>
    public class ManuCcdFileQuery
    {
        /// <summary>
        /// 站点
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public IEnumerable<string> Sfcs { get; set; }
    }
}
