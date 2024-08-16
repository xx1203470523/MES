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

    /// <summary>
    /// 根据工序获取数量
    /// </summary>
    public class SfcStepProcedureQuery
    {
        /// <summary>
        /// 工序列表
        /// </summary>
        public List<string> ProcedureCodeList { get; set; } = new List<string>();

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndDate { get; set; }
    }

    /// <summary>
    /// 根据工单工序获取数量
    /// </summary>
    public class SfcStepOrderProcedureQuery
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// ERP订单
        /// </summary>
        public string WorkPlanCode { get; set; }

        /// <summary>
        /// MES工单
        /// </summary>
        public string? OrderCode { get; set; }

        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }
    }
}
