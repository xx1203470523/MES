using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums.QualUnqualifiedCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    public class WorkshopJobControlReportView:BaseEntity
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 条码状态
        /// </summary>
        public SfcStatusEnum SFCStatus { get; set; }

        public SfcProduceStatusEnum SFCProduceStatus { get; set; }

        /// <summary>
        /// 物料编码/版本
        /// </summary>
        public string MaterialCodeVersion { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 工单编码
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 工单类型
        /// </summary>
        public PlanWorkOrderTypeEnum OrderType { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcedureName { get; set; }

        /// <summary>
        /// Bom编码/版本
        /// </summary>
        public string BomCodeVersion { get; set; }

        /// <summary>
        /// bom名称
        /// </summary>
        public string BomName { get; set; }

        /// <summary>
        /// 条码数量
        /// </summary>
        public decimal Qty { get; set; }
    }
}
