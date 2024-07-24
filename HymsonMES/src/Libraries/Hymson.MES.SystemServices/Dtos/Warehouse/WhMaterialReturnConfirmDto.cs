using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Warehouse;
using Hymson.MES.CoreServices.Services.Job;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.SystemServices.Dtos.Warehouse
{
    public class WhMaterialReturnConfirmDto
    {
        /// <summary>
        /// 退料单号
        /// </summary>
        public string ReturnOrderCode { set; get; }

        /// <summary>
        /// 退料单结果
        /// </summary>
        public WhWarehouseRequistionResultEnum ReceiptResult { set; get; }

        /// <summary>
        /// 退料单仓库收料详情
        /// </summary>
        public IEnumerable<MaterialReturnReceiptDetails>? Details { set; get; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { set; get; }
    }

    public class MaterialReturnReceiptDetails
    {
        /// <summary>
        /// 物料条码
        /// </summary>
        public string MaterialBarcode { set; get; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { set; get; }
    }
}
