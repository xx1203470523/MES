/*
 *creator: Karl
 *
 *describe: 条码表 查询类 | 代码由框架生成
 *builder:  wangkeming
 *build datetime: 2023-04-10 04:55:42
 */

namespace Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Query
{
    /// <summary>
    /// 条码表 查询参数
    /// </summary>
    public class ManuSfcQuery
    {
    }


    /// <summary>
    /// 根据SFC查询条码
    /// </summary>
    public class GetBySfcQuery
    {
        /// <summary>
        /// 站点编码 
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string? SFC { get; set; }
    }
}
