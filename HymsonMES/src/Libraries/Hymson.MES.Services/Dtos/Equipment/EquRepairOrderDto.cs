/*
 *creator: Karl
 *
 *describe: 设备维修记录    Dto | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-06-12 10:56:10
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Equipment;
using Hymson.MES.Services.Dtos.Integrated;
using Org.BouncyCastle.Asn1;

namespace Hymson.MES.Services.Dtos.EquRepairOrder
{
    #region 
    /// <summary>
    /// 报修Dot
    /// </summary>
    public record EquReportRepairDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 设备Code
        /// </summary>
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 故障时间
        /// </summary>
        public DateTime FaultTime { get; set; }

        /// <summary>
        /// 是否停机
        /// </summary>
        public TrueOrFalseEnum IsShutdown { get; set; }

        /// <summary>
        /// 状态 1、待维修 2、已维修3、已确认
        /// </summary>
        public EquRepairOrderStatusEnum Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; } = "";

        /// <summary>
        /// 故障现象 
        /// </summary>
        public IEnumerable<EquReportRepairFaultPhenomenonDto> FaultPhenomenonDto { get; set; }

    }

    /// <summary>
    /// 报修-故障现象-详情
    /// </summary>
    public record EquReportRepairFaultPhenomenonDto
    {
        /// <summary>
        /// 故障现象Id equ_fault_phenomenon id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 故障现象编码 
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 故障现象名称
        /// </summary>
        public string Name { get; set; }
    }


    /// <summary>
    /// 报修Dot
    /// </summary>
    public record EquMaintenanceDto
    {

        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 维修开始时间
        /// </summary>
        public DateTime RepairStartTime { get; set; }

        /// <summary>
        /// 维修结束时间
        /// </summary>
        public DateTime RepairEndTime { get; set; }

        /// <summary>
        /// 长期处理措施
        /// </summary>
        public string LongTermHandlingMeasures { get; set; }

        /// <summary>
        /// 临时处理措施
        /// </summary>
        public string TemporaryTermHandlingMeasures { get; set; }

        /// <summary>
        /// 故障原因
        /// </summary>
        public IEnumerable<EquReportRepairFaultReasonDto> FaultPhenomenonDto { get; set; }
    }

    /// <summary>
    /// 维修-故障原因-详情
    /// </summary> 
    public record EquReportRepairFaultReasonDto
    {
        /// <summary>
        ///  
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 故障原因
        /// </summary>
        public long? FaultReasonId { get; set; }

        /// <summary>
        /// 故障原因
        /// </summary>
        public string? FaultReason { get; set; }
    }

    /// <summary>
    /// 确认
    /// </summary>
    public record ConfirmDto
    {

        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 维修确认 1、维修完成2、重新维修
        /// </summary>
        public EquConfirmResultEnum ConfirmResult { get; set; }
    }


    /// <summary>
    /// 设备维修记录Dto
    /// </summary>
    public record EquRepairOrderFromDto : BaseEntityDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 设备编号
        /// </summary>
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName { get; set; }

        /// <summary>
        /// 报修单号
        /// </summary>
        public string RepairOrder { get; set; }

        /// <summary>
        /// 状态 1、待维修 2、已维修3、已确认
        /// </summary>
        public EquRepairOrderStatusEnum? Status { get; set; }

        /// <summary>
        /// 是否停机
        /// </summary>
        public TrueOrFalseEnum IsShutdown { get; set; }

        /// <summary>
        /// 报修人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 故障时间
        /// </summary>
        public DateTime FaultTime { get; set; }

        /// <summary>
        /// 报修时间 
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 维修开始时间
        /// </summary>
        public DateTime? RepairStartTime { get; set; }

        /// <summary>
        /// 维修结束时间
        /// </summary>
        public DateTime? RepairEndTime { get; set; }
    }


    /// <summary>
    /// 设备维修记录Dto
    /// </summary>
    public record EquRepairOrderFromDetailDto : BaseEntityDto
    {
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; } = "";

        /// <summary>
        /// 长期处理措施
        /// </summary>
        public string LongTermHandlingMeasures { get; set; }

        /// <summary>
        /// 临时处理措施
        /// </summary>
        public string TemporaryTermHandlingMeasures { get; set; }


        /// <summary>
        /// 维修人
        /// </summary>
        public string RepairPerson { get; set; }

        /// <summary>
        /// 确认人
        /// </summary>
        public string ConfirmBy { get; set; }

        /// <summary>
        /// 维修确认 1、维修完成2、重新维修
        /// </summary>
        public EquConfirmResultEnum? ConfirmResult { get; set; }

        /// <summary>
        /// 确认时间
        /// </summary>
        public DateTime? ConfirmOn { get; set; }

    }

    /// <summary>
    /// 故障现象-详情
    /// </summary>
    public record EquReportRepairFaultDto
    {
        /// <summary>
        /// 故障现象Id equ_fault_phenomenon id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 故障现象编码 
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 故障现象名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 故障原因id equ_fault_reasonId
        /// </summary>
        public long? FaultReasonId { get; set; }

        /// <summary>
        /// 故障原因
        /// </summary>
        public string FaultReason { get; set; }
    }


    /// <summary>
    /// 删除
    /// </summary>
    public record EquRepairOrderDeletesDto
    {
        /// <summary>
        /// 设备Id
        /// </summary>
        public IEnumerable<long> Ids { get; set; }
    }
    #endregion

    #region 附件
    /// <summary>
    /// 附件dto
    /// </summary>
    public record EquRepairOrderAttachmentDto
    {
        /// <summary>
        /// 单据id
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// 单据id 
        /// </summary>
        //public long OrderAttachmentId { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public EquRepairOrderAttachmentTypeEnum Type { get; set; }

    }
    /// <summary>
    /// 附件保存dto
    /// </summary>
    public record EquRepairOrderSaveAttachmentDto
    {
        /// <summary>
        /// 单据id
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public EquRepairOrderAttachmentTypeEnum Type { get; set; }

        /// <summary>
        /// 检验单（附件）
        /// </summary>
        public IEnumerable<InteAttachmentBaseDto> Attachments { get; set; }

    }

    #endregion

    /// <summary>
    /// 设备维修记录Dto
    /// </summary>
    public record EquRepairOrderDto : BaseEntityDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 设备编号
        /// </summary>
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName { get; set; }

        /// <summary>
        /// 报修单号
        /// </summary>
        public string RepairOrder { get; set; }

        /// <summary>
        /// 状态 1、待维修 2、已维修3、已确认
        /// </summary>
        public EquRepairOrderStatusEnum? Status { get; set; }


        /// <summary>
        /// 报修人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 维修人
        /// </summary>
        public string RepairPerson { get; set; }

        /// <summary>
        /// 确认人
        /// </summary>
        public string ConfirmBy { get; set; }

        /// <summary>
        /// 维修确认 1、维修完成2、重新维修
        /// </summary>
        public EquConfirmResultEnum? ConfirmResult { get; set; }

        /// <summary>
        /// 故障时间
        /// </summary>
        public DateTime FaultTime { get; set; }

        /// <summary>
        /// 报修时间 
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 维修开始时间
        /// </summary>
        public DateTime? RepairStartTime { get; set; }

        /// <summary>
        /// 维修结束时间
        /// </summary>
        public DateTime? RepairEndTime { get; set; }

        /// <summary>
        /// 确认时间
        /// </summary>
        public DateTime? ConfirmOn { get; set; }

    }


    /// <summary>
    /// 设备维修记录新增Dto
    /// </summary>
    public record EquRepairOrderCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 设备id equ_equipment的 id
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 故障时间
        /// </summary>
        public DateTime FaultTime { get; set; }

        /// <summary>
        /// 是否停机
        /// </summary>
        public TrueOrFalseEnum IsShutdown { get; set; }

        /// <summary>
        /// 状态 1、待维修 2、已维修3、已确认
        /// </summary>
        public EquRepairOrderStatusEnum Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }



    }

    /// <summary>
    /// 设备维修记录更新Dto
    /// </summary>
    public record EquRepairOrderModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 维修
        /// </summary>
        public string RepairOrder { get; set; }

        /// <summary>
        /// 设备id equ_equipment的 id
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 设备记录id equ_equipment_record
        /// </summary>
        public long EquipmentRecordId { get; set; }

        /// <summary>
        /// 故障时间
        /// </summary>
        public DateTime FaultTime { get; set; }

        /// <summary>
        /// 是否停机
        /// </summary>
        public TrueOrFalseEnum IsShutdown { get; set; }

        /// <summary>
        /// 状态 1、待维修 2、已维修3、已确认
        /// </summary>
        public EquRepairOrderStatusEnum Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }



    }

    /// <summary>
    /// 设备维修记录分页Dto
    /// </summary>
    public class EquRepairOrderPagedQueryDto : PagerInfo
    {

        /// <summary>
        /// 站点
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public string? EquipmentCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string? EquipmentName { get; set; }

        /// <summary>
        /// 报修单号
        /// </summary>
        public string? RepairOrder { get; set; }

        /// <summary>
        /// 状态 1、待维修 2、已维修3、已确认
        /// </summary>
        public EquRepairOrderStatusEnum? Status { get; set; }

        /// <summary>
        /// 报修人
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// 维修人
        /// </summary>
        public string? RepairPerson { get; set; }

        /// <summary>
        /// 确认人
        /// </summary>
        public string? ConfirmBy { get; set; }

        /// <summary>
        /// 维修确认 1、维修完成2、重新维修
        /// </summary>
        public EquConfirmResultEnum? ConfirmResult { get; set; }


        /// <summary>
        /// 报修时间
        /// </summary>
        public DateTime[]? CreatedOn { get; set; }

        /// <summary>
        /// 故障时间
        /// </summary>
        public DateTime[]? FaultTime { get; set; }

        /// <summary>
        /// 维修开始时间
        /// </summary>
        public DateTime[]? RepairStartTime { get; set; }

        /// <summary>
        /// 维修结束时间
        /// </summary>
        public DateTime[]? RepairEndTime { get; set; }

        /// <summary>
        /// 确认时间
        /// </summary>
        public DateTime[]? ConfirmOn { get; set; }
    }
}
