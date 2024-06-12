using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums.QualUnqualifiedCode;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    public class ManuProductBadRecordReportView
    {
        /// <summary>
        /// 不合格代码Id
        /// </summary>
        public long UnqualifiedId { get; set; }

        /// <summary>
        /// 汇总数量
        /// </summary>
        //public decimal Num { get; set; }
        public int Num { get; set; }
    }

    public class ManuProductBadRecordLogReportView : BaseEntity
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 工单编码
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 实际NG工序
        /// </summary>
        public string FactProcdedureCode { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResCode { get; set; }

        /// <summary>
        /// 不合格代码Id
        /// </summary>
        public long UnqualifiedId { get; set; }

        /// <summary>
        /// 不合格代码
        /// </summary>
        public string UnqualifiedCode { get; set; }

        /// <summary>
        /// 不合格类型
        /// </summary>
        public QualUnqualifiedCodeTypeEnum UnqualifiedType { get; set; }

        /// <summary>
        /// 不良状态
        /// </summary>
        public ProductBadRecordStatusEnum BadRecordStatus { get; set; }
        
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get;set; }
    }
}
