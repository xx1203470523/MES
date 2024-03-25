/*
 *creator: Karl
 *
 *describe: 烘烤工序    Dto | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-08-02 07:32:20
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Services.Dtos.Manufacture
{
    /// <summary>
    /// 条码数量调整
    /// </summary>
    public record ManuBarcodeQtyAdjustDto : BaseEntityDto
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string SFC {  get; set; }

        /// <summary>
        /// 调整后的数量
        /// </summary>
        public decimal Qty {  get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }

    /// <summary>
    /// 条码拆分
    /// </summary>
    public record ManuBarcodeSplitAdjustDto : BaseEntityDto
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 待拆分的数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }

    /// <summary>
    /// 条码合并
    /// </summary>
    public record ManuBarcodeMergeAdjust : BaseEntityDto 
    {
        /// <summary>
        /// 待合并的条码
        /// </summary>
        public IEnumerable<string> SFCs { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }

    public class ManuSfcAboutInfoPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 产品序列码
        /// </summary>
        public string? Sfc { get; set; }

        /// <summary>
        /// 产品序列码  硬查，不模糊查询
        /// </summary>
        public string? SfcHard { get; set; }


        /// <summary>
        /// 状态
        /// </summary>
        public SfcStatusEnum? Status { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public long? MaterialId { get; set; }

        /// <summary>
        /// 工单ID
        /// </summary>
        public long? WorkOrderId {  get; set; }


        /// <summary>
        /// 工序路线ID
        /// </summary>
        public long? ProcessRouteId { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public long? ResourceId { get; set; }

        /// <summary>
        /// 载具编码
        /// </summary>
        public long? VehicleId {  get; set; }
    }

    public record ManuSfcAboutInfoViewDto: BaseEntityDto
    {
        #region manusfc
        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 状态: 1：排队中 2：活动中 3：完成-在制 4：完成 5：锁定 6：报废 7：删除
        /// </summary>
        public SfcStatusEnum Status { get; set; }
        #endregion

        /// <summary>
        /// 工单ID
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 工单编码
        /// </summary>
        public string WorkOrderCode { get; set; }

        /// <summary>
        /// BomId
        /// </summary>
        public long ProductBomId { get; set; }

        /// <summary>
        /// 工艺路线Id
        /// </summary>
        public long ProcessRouteId { get; set; }

        /// <summary>
        /// 载具编码
        /// </summary>
        public string VehicleCode { get; set; }
        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcedureCode { get; set; }
        /// <summary>
        /// 物料编码/版本
        /// </summary>
        public string MaterialCodeVersion { get; set; }

        public string MaterialCode {  get; set; }
        public string MaterialVersion { get; set; }
        public string MaterialName {  get; set; }

        /// <summary>
        /// 物料单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 工艺路线/版本
        /// </summary>
        public string ProcessRouteCodeVersion { get; set; }
        /// <summary>
        /// bom编码/版本
        /// </summary>
        public string BomCodeVersion { get; set; }
    }

    public class ManuSfcsVerifyConditions 
    {
        /// <summary>
        /// 是否验证相同工单
        /// </summary>
        public bool IsVerifySameWorkOrder {  get; set; }=false;

        /// <summary>
        /// 是否验证相同物料
        /// </summary>
        public bool IsVerifySameMaterial {  get; set; }=false;

        /// <summary>
        /// 是否验证相同BOM
        /// </summary>
        public bool IsVerifySameBom {  get; set; }=false;

        /// <summary>
        /// 是否验证相同工艺路线
        /// </summary>
        public bool IsVerifySameProcessRoute { get; set; }=false;

        /// <summary>
        /// 是否验证相同条码状态
        /// </summary>
        public bool IsVerifySameSfcStatus { get; set; } = false;

        /// <summary>
        /// 是否验证绑定载具
        /// </summary>
        public bool IsVerifyBindVehicle {  get; set; }=false;

        /// <summary>
        /// 是否验证物料的数量限制
        /// </summary>
        public bool IsVerifyMaterialQuantityLimit { get; set; } = false;

        /// <summary>
        /// 是否绑定容器
        /// </summary>
        public bool IsVerifyBindContainer { get; set; }=false;

        /// <summary>
        /// 是否禁止工单状态
        /// </summary>
        public PlanWorkOrderStatusEnum[]? NotAllowWorkOrderStatus { get; set; }

        /// <summary>
        /// 是否禁止NG条码
        /// </summary>
        public bool IsBanNgSfc {  get; set; }=false;

        /// <summary>
        /// 不允许的条码状态
        /// </summary>
        public SfcStatusEnum[]? NotAllowStatus { get; set; }

    }
}
