using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Warehouse;

namespace Hymson.MES.Services.Dtos.Manufacture
{
    /// <summary>
    /// 领料明细
    /// </summary>
    public record ManuRequistionOrderDetailDto
    {
        /// <summary>
        /// 领料单据号
        /// </summary>
        public string ReqOrderCode { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 物料条码
        /// </summary>
        public string MaterialBarCode { get; set; }

        /// <summary>
        /// 物料批次
        /// </summary>
        public string Batch { get; set; }

        /// <summary>
        /// 应领数量
        /// </summary>
        public decimal NeedQty { get; set; } = 0;

        /// <summary>
        /// 领料数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public WhMaterialPickingStatusEnum? Status { get; set; }

        /// <summary>
        /// 领料时间
        /// </summary>
        public DateTime PickTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; } = "";

    }
}
