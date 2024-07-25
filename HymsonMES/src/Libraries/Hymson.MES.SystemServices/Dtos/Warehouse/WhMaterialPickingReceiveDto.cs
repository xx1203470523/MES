using Hymson.MES.Core.Enums.Warehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.SystemServices.Dtos.Warehouse
{
    /// <summary>
    /// 领料单收料实体
    /// </summary>
    public  class WhMaterialPickingReceiveDto
    { 
        /// <summary>
      /// 领料单号
      /// </summary>
        public string RequistionOrderCode { set; get; }

        /// <summary>
        /// 领料单结果
        /// </summary>
        public WhMaterialPickingReceiveResultEnum ReceiptResult { set; get; }

        /// <summary>
        /// 退料单仓库收料详情
        /// </summary>
        public IEnumerable<WhMaterialPickingDetailDto>? Details { set; get; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { set; get; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string OperateBy { set; get; }
    }

    /// <summary>
    /// 领料单料
    /// </summary>
    public class WhMaterialPickingDetailDto
    {
        /// <summary>
        /// 物料条码
        /// </summary>
        public string MaterialBarCode { set; get; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { set; get; }
    }
}
