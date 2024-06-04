/*
 *creator: Karl
 *
 *describe: 条码步骤ng信息记录表 查询类 | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-05-18 04:12:10
 */

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 条码步骤ng信息记录表 查询参数
    /// </summary>
    public class ManuSfcStepNgQuery
    {
        /// <summary>
        /// 起始日期
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 截止日期
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
}
