using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.HttpClients.Requests.WMS
{
    /// <summary>
    /// IQC请求Dto
    /// </summary>
    public record IQCReturnResultDto : BaseEntityDto
    {
        /// <summary>
        /// 收货单号
        /// </summary>
        public string ReturnOrderCode { get; set; } = "";

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string WorkOrderCode { get; set; } = "";

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<IQCReturnMaterialResultDto>? Details { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public record IQCReturnMaterialResultDto : BaseEntityDto
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; } = "";

        /// <summary>
        /// 供应商生产批次
        /// </summary>
        public string SupplierBatch { get; set; } = "";

        /// <summary>
        /// 内部批次
        /// </summary>
        public string InternalBatch { get; set; } = "";

        /// <summary>
        /// 计划退货数量
        /// </summary>
        public decimal? PlanQty { get; set; }

        /// <summary>
        /// 实退数量
        /// </summary>
        public decimal? Qty { get; set; }

        /// <summary>
        /// 实际到货时间
        /// </summary>
        public DateTime? ActualTime { get; set; }

        /// <summary>
        /// 计划到货时间
        /// </summary>
        public DateTime? PlanTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 是否合格;0、不合格 1、合格
        /// </summary>
        public TrueOrFalseEnum IsQualified { get; set; }

    }

}
