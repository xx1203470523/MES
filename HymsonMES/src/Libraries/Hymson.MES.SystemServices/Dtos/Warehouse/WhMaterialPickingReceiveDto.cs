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
        public string MaterialBarCode { set; get; } = "";

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { set; get; }

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string SupplierCode { get; set; } = "";

        /// <summary>
        /// 物料的有效期（过期时间)
        /// </summary>
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string Version { get; set; } = "";

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; } = "";

        /// <summary>
        /// 物料批次
        /// </summary>
        public string Batch { get; set; } = "";
    }
}
