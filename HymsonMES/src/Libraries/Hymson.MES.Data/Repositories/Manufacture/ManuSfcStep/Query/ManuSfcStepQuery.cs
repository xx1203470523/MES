/*
 *creator: Karl
 *
 *describe: 条码步骤表 查询类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-03-22 05:17:57
 */

using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 条码步骤表 查询参数
    /// </summary>
    public class ManuSfcStepQuery
    {
        /// <summary>
        /// 步骤类型
        /// </summary>
        public ManuSfcStepTypeEnum? Operatetype { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        public IEnumerable<string>? SFCs { get; set; }
        /// <summary>
        /// 工序
        /// </summary>
        public long? ProcedureId { get; set; }

        public long SiteId { get; set; }
    }


    public class SfcInStepQuery
    {
        public long SiteId { get; set; }

        public string[] Sfcs { get; set; }

    }

    public class SfcMergeOrSplitAddStepQuery
    {
        public long SiteId { get; set; }

        public string Sfc { get; set; }

    }
}
