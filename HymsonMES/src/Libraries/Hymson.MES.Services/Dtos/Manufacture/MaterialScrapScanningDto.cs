using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Manufacture.ManuSFCScrap
{
    /// <summary>
    /// 物料条码
    /// </summary>
    public class MaterialScrapScanningDto
    {
      public string BarCode { set; get; }
    }

    public class MaterialScrapBarCodeDto
    {
        /// <summary>
        /// 条码信息
        /// </summary>
        public string BarCode { set; get; }

        ///// <summary>
        ///// 工单号
        ///// </summary>
        //public string WorkOrderCode { set; get; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string materialCode { set; get; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string materialName { set; get; }

        /// <summary>
        /// 产品信息数量
        /// </summary>
        public decimal Qty { set; get; }
    }
}
