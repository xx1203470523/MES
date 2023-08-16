/*
 *creator: Karl
 *
 *describe: CCS盖板NG记录 查询类 | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-08-15 05:15:40
 */

using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// CCS盖板NG记录 查询参数
    /// </summary>
    public class ManuSfcCcsNgRecordQuery
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }
        /// <summary>
        /// 模组条码
        /// </summary>
        public string SFC { get; set; }
        /// <summary>
        /// NG位置;A,B,C,D等
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// CCSNG状态
        /// </summary>
        public ManuSfcCcsNgRecordStatusEnum? Status { get; set; }
    }
}
