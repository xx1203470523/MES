using Hymson.MES.Core.Domain.Manufacture;

namespace Hymson.MES.Services.Dtos.Manufacture.WhMaterialPicking
{
    /// <summary>
    /// 
    /// </summary>
    public class PickMaterialDto
    {
        /// <summary>
        /// 派工单Id
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// 退仓类型1：实仓，2：虚仓
        /// </summary>
        public ManuRequistionTypeEnum Type { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string? WarehouseCode { get; set; }

        /// <summary>
        /// 工单编码（虚仓领料时，需要指定从哪个工单领虚退的物料）
        /// </summary>
        public string? WorkOrderCode { get; set; }

        /// <summary>
        /// 领料数量
        /// </summary>
        public List<PickBomDetailDto> Details { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PickBomDetailDto
    {
        /// <summary>
        /// 物料Id
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 最小批次数量
        /// </summary>
        public decimal Batch { get; set; }

        /// <summary>
        /// BomID
        /// </summary>
        public long? BomId { get; set; }
    }

}
