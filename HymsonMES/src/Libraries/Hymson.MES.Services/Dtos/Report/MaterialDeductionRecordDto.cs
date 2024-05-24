using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Report
{

    public class MaterialDeductionRecordResultDto
    {
        /// <summary>
        /// 产品序列码
        /// </summary>
        public string Sfc { get; set; }

        /// <summary>
        /// 批次条码
        /// </summary>
        public string CirculationBarCode { get; set; }

        /// <summary>
        /// 扣量数量
        /// </summary>
        public decimal CirculationQty { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        public string WorkOrder { get; set; }

        /// <summary>
        /// 物料编码/产品
        /// </summary>
        public string MaterialRemark { get; set; }

        /// <summary>
        /// 需求用量
        /// </summary>
        public decimal BomUsages { get; set; }

        /// <summary>
        /// 扣料工序
        /// </summary>
        public string ProcedureName { get; set; }

        /// <summary>
        /// 扣料资源
        /// </summary>
        public string ResourceCode { get; set; }

        /// <summary>
        /// 扣料名称
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string CreateBy { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime CreateOn { get; set; }
    }
}