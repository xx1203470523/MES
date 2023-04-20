/*
 *creator: Karl
 *
 *describe: 条码信息表 查询类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-21 04:00:29
 */

using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Query
{
    /// <summary>
    /// 条码信息表 查询参数
    /// </summary>
    public class ManuSfcStatusQuery
    {
        /// <summary>
        /// 条码列表
        /// </summary>
        public string[] Sfcs { get; set; }

        /// <summary>
        /// 条码状态
        /// </summary>
        public SfcStatusEnum?[] Statuss { get; set; } 
    }
}
