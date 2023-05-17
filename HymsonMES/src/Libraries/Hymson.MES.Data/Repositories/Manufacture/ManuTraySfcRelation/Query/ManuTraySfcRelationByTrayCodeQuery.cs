/*
 *creator: Karl
 *
 *describe: 托盘条码关系 查询类 | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-05-16 11:11:13
 */

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 根据托盘条码查询装载条码 查询参数
    /// </summary>
    public class ManuTraySfcRelationByTrayCodeQuery
    {
        /// <summary>
        /// 托盘码
        /// </summary>
        public string TrayCode { get; set; }

        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }
    }
}
