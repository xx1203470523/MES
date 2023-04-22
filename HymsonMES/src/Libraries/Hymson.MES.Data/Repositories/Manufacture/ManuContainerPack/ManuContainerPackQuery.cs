/*
 *creator: Karl
 *
 *describe: 容器装载表（物理删除） 查询类 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-04-12 02:33:13
 */

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 容器装载表（物理删除） 查询参数
    /// </summary>
    public class ManuContainerPackQuery
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string? LadeBarCode { get; set; }

        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; }
    }
}
