using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 数据实体（条码报废表）   
    /// manu_sfc_scrap
    /// @author wangkeming
    /// @date 2023-09-12 01:54:49
    /// </summary>
    public class ManuSfcScrapEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 步骤表id
        /// </summary>
        public long SfcStepId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

       /// <summary>
        /// 条码信息表
        /// </summary>
        public long SfcinfoId { get; set; }

       /// <summary>
        /// 工序表id
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 流出工序id
        /// </summary>
        public long? OutFlowProcedureId { get; set; }

        /// <summary>
        /// 报废不合格代码id
        /// </summary>
        public long? UnqualifiedId { get; set; }

        /// <summary>
        /// 报废数量
        /// </summary>
        public decimal? ScrapQty { get; set; }

       /// <summary>
        /// 是否取消  0否  1 是
        /// </summary>
        public bool IsCancel { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 取消步骤表id
        /// </summary>
        public long CancelSfcStepId { get; set; }
    }
}
