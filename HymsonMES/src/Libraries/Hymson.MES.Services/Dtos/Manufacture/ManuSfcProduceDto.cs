/*
 *creator: Karl
 *
 *describe: 条码生产信息（物理删除）    Dto | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-03-18 05:37:27
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Services.Dtos.Manufacture
{
    /// <summary>
    /// 条码生产信息（物理删除）Dto
    /// </summary>
    public record ManuSfcProduceDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string Sfc { get; set; }

        /// <summary>
        /// 产品id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 工单id
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// 条码数据id
        /// </summary>
        public long BarCodeInfoId { get; set; }

        /// <summary>
        /// 工艺路线
        /// </summary>
        public long ProcessRouteId { get; set; }

        /// <summary>
        /// 工作中心
        /// </summary>
        public long WorkCenterId { get; set; }

        /// <summary>
        /// BOMId
        /// </summary>
        public long ProductBOMId { get; set; }

        /// <summary>
        /// 设备Id
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 资源id
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 工序id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 状态;1：排队；2：活动；
        /// </summary>
        public SfcProduceStatusEnum Status { get; set; }

        /// <summary>
        /// 锁;1：未锁定；2：即时锁；3：将来锁；
        /// </summary>
        public QualityLockEnum? Lock { get; set; }

        /// <summary>
        /// 未来锁工序id
        /// </summary>
        public long? LockProductionId { get; set; }

        /// <summary>
        /// 是否可疑
        /// </summary>
        public bool? IsSuspicious { get; set; }

        /// <summary>
        /// 复投次数;复投次数
        /// </summary>
        public int RepeatedCount { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }


    }

    /// <summary>
    /// 质量锁定操作实体
    /// </summary>
    public record ManuSfcProduceLockDto
    {
        /// <summary>
        /// 操作类型
        /// </summary>
        public QualityLockEnum OperationType { get; set; }

        /// <summary>
        /// 将来锁，锁定的工序id
        /// </summary>
        public long? LockProductionId { get; set; }

        /// <summary>
        /// 条码列表
        /// </summary>
        public string[] Sfcs { get; set; }
    }

    /// <summary>
    /// 将来锁
    /// </summary>
    public record FutureLockDto
    {
        /// <summary>
        /// 将来锁，锁定的工序id
        /// </summary>
        public long LockProductionId { get; set; }

        /// <summary>
        /// 条码列表
        /// </summary>
        public string[] Sfcs { get; set; }
    }

    /// <summary>
    /// 及时锁
    /// </summary>
    public record InstantLockDto
    {
        /// <summary>
        /// 条码列表
        /// </summary>
        public IEnumerable<string> Sfcs { get; set; }
    }

    /// <summary>
    /// 解除锁
    /// </summary>
    public record UnLockDto
    {
        /// <summary>
        /// 条码列表
        /// </summary>
        public IEnumerable<string> Sfcs { get; set; }
    }

    /// <summary>
    /// 报废/取消报废操作实体
    /// </summary>
    public record ManuSfScrapDto
    {
        /// <summary>
        /// 操作类型（报废/取消报废）
        /// </summary>
        public ScrapOperateTypeEnum OperationType { get; set; }

        /// <summary>
        /// 条码列表
        /// </summary>
        public string[] Sfcs { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }
    /// <summary>
    /// 条码生产信息（物理删除）新增Dto
    /// </summary>
    public record ManuSfcProduceCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string Sfc { get; set; }

        /// <summary>
        /// 产品id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 工单id
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// 条码数据id
        /// </summary>
        public long BarCodeInfoId { get; set; }

        /// <summary>
        /// 工艺路线
        /// </summary>
        public long ProcessRouteId { get; set; }

        /// <summary>
        /// 工作中心
        /// </summary>
        public long WorkCenterId { get; set; }

        /// <summary>
        /// BOMId
        /// </summary>
        public long ProductBOMId { get; set; }

        /// <summary>
        /// 设备Id
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 资源id
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 工序id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 状态;1：排队；2：活动；
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 锁;1：未锁定；2：即时锁；3：将来锁；
        /// </summary>
        public QualityLockEnum Lock { get; set; }

        /// <summary>
        /// 未来锁工序id
        /// </summary>
        public long? LockProductionId { get; set; }

        /// <summary>
        /// 是否可疑
        /// </summary>
        public bool? IsSuspicious { get; set; }

        /// <summary>
        /// 复投次数;复投次数
        /// </summary>
        public int RepeatedCount { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }


    }

    /// <summary>
    /// 条码生产信息（物理删除）更新Dto
    /// </summary>
    public record ManuSfcProduceModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string Sfc { get; set; }

        /// <summary>
        /// 产品id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 工单id
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// 条码数据id
        /// </summary>
        public long BarCodeInfoId { get; set; }

        /// <summary>
        /// 工艺路线
        /// </summary>
        public long ProcessRouteId { get; set; }

        /// <summary>
        /// 工作中心
        /// </summary>
        public long WorkCenterId { get; set; }

        /// <summary>
        /// BOMId
        /// </summary>
        public long ProductBOMId { get; set; }

        /// <summary>
        /// 设备Id
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 资源id
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 工序id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 状态;1：排队；2：活动；
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 锁;1：未锁定；2：即时锁；3：将来锁；
        /// </summary>
        public QualityLockEnum Lock { get; set; }

        /// <summary>
        /// 未来锁工序id
        /// </summary>
        public long? LockProductionId { get; set; }

        /// <summary>
        /// 是否可疑
        /// </summary>
        public bool? IsSuspicious { get; set; }

        /// <summary>
        /// 复投次数;复投次数
        /// </summary>
        public int RepeatedCount { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }
    }

    /// <summary>
    /// 条码生产信息（物理删除）分页Dto
    /// </summary>
    public class ManuSfcProducePagedQueryDto : PagerInfo
    {
        /// <summary>
        ///产品条码
        /// </summary>
        public string? Sfc { get; set; }

        /// <summary>
        /// 条码状态
        /// </summary>
        public SfcProduceStatusEnum? Status { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        public string? OrderCode { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 条码列表
        /// </summary>
        public string[]? Sfcs { get; set; }

        /// <summary>
        /// 资源id
        /// </summary>
        public long? ResourceId { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        public string? ResCode { get; set; }

        /// <summary>
        /// 锁定状态
        /// </summary>
        public int? Lock { get; set; }

        /// <summary>
        /// 查询锁定状态不为某个状态的sfc信息，即时锁定的不能操作不查
        /// </summary>
        public int? NoLock { get; set; }

        /// <summary>
        /// 是否报废
        /// </summary>
        public TrueOrFalseEnum? IsScrap { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string? Version { get; set; }
    }

    /// <summary>
    /// 条码生产信息查询 分页Dto
    /// </summary>
    public class ManuSfcProduceSelectPagedQueryDto : PagerInfo
    {
        /// <summary>
        ///产品条码
        /// </summary>
        public string? Sfc { get; set; }

        /// <summary>
        /// 条码状态
        /// </summary>
        public SfcProduceStatusEnum? Status { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public long? OrderId { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 条码列表
        /// </summary>
        public string[]? Sfcs { get; set; }

        /// <summary>
        /// 资源id
        /// </summary>
        public long? ResourceId { get; set; }

        /// <summary>
        /// 锁定状态
        /// </summary>
        public int? Lock { get; set; }

        /// <summary>
        /// 查询锁定状态不为某个状态的sfc信息，即时锁定的不能操作不查
        /// </summary>
        public int? NoLock { get; set; }

        ///// <summary>
        ///// 是否报废
        ///// </summary>
        //public TrueOrFalseEnum? IsScrap { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string? MaterialCode { get; set; }
    }


    /// <summary>
    /// 分页查询返回实体
    /// </summary>
    public class ManuSfcProduceViewDto
    {
        /// <summary>
        ///  唯一标识
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string Sfc { get; set; }

        /// <summary>
        /// 锁;1：未锁定；2：即时锁；3：将来锁；
        /// </summary>
        public QualityLockEnum? Lock { get; set; }

        /// <summary>
        /// 未来锁工序id
        /// </summary>
        public long? LockProductionId { get; set; }

        /// <summary>
        /// 工艺路线
        /// </summary>
        public long ProcessRouteId { get; set; }

        /// <summary>
        /// BOMId
        /// </summary>
        public long? ProductBOMId { get; set; }

        /// <summary>
        /// 当前工序
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 状态;1：排队；2：活动；
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 产品版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 是否报废
        /// </summary>
        public TrueOrFalseEnum? IsScrap { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResCode { get; set; }
    }

    /// <summary>
    /// 分页查询返回实体
    /// </summary>
    public class ManuSfcProduceSelectViewDto
    {
        /// <summary>
        ///  唯一标识
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string Sfc { get; set; }

        /// <summary>
        /// 锁;1：未锁定；2：即时锁；3：将来锁；
        /// </summary>
        public QualityLockEnum? Lock { get; set; }

        /// <summary>
        /// 未来锁工序id
        /// </summary>
        public long? LockProductionId { get; set; }

        /// <summary>
        /// BOMId
        /// </summary>
        public long? ProductBOMId { get; set; }

        /// <summary>
        /// 当前工序
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 状态;1：排队；2：活动；
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 产品版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResCode { get; set; }
    }


    /// <summary>
    /// 在制品步骤控制操作实体（操作条码）
    /// </summary>
    public record ManuSfcProduceStepSFCDto
    {
        /// <summary>
        /// 条码列表
        /// </summary>
        public string Sfc { get; set; }
    }

    /// <summary>
    /// 在制品步骤控制操作实体（操作条码）
    /// </summary>
    public record SaveManuSfcProduceStepDto
    {
        /// <summary>
        /// 工序ID
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public SfcProduceStatusEnum Type { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 条码列表
        /// </summary>
        public List<ManuSfcProduceStepSFCDto> Sfcs { get; set; }


    }


    /// <summary>
    /// 在制品步骤控制操作实体（步骤列表）
    /// </summary>
    public record ManuSfcProduceStepViewDto
    {
        /// <summary>
        /// 步骤
        /// </summary>
        public string Step { get; set; }
        /// <summary>
        /// 工序Id
        /// </summary>
        public long ProcedureId { get; set; }
        /// <summary>
        /// 工序Code
        /// </summary>
        public string ProcedureCode { get; set; }
        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcedureName { get; set; }


        /// <summary>
        /// 排队数量
        /// </summary>
        public int lineUpNumber { get; set; }
        /// <summary>
        /// 活动数量
        /// </summary>
        public int activityNumber { get; set; }
        /// <summary>
        /// 完工数量
        /// </summary>
        public int completeNumber { get; set; }
    }

    /// <summary>
    /// 在制品步骤控制操作实体（保存）
    /// </summary>
    public record ManuSfcProduceStepSaveDto
    {
        /// <summary>
        /// 操作类型
        /// </summary>
        public SfcProduceStatusEnum OperationType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 操作工序
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 条码列表
        /// </summary>
        public List<ManuSfcProduceStepSFCDto> Sfcs { get; set; }
    }



    /// <summary>
    /// 更改生产（列表）
    /// </summary>
    public record ManuUpdateViewDto
    {
        /// <summary>
        /// SFC
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SfcProduceStatusEnum Status { get; set; }

        /// <summary>
        /// 工序Code
        /// </summary>
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 物料与版本
        /// </summary>
        public string MaterialAndVersion { get; set; }

        /// <summary>
        /// 工艺路线与版本
        /// </summary>
        public string ProcessRouteAndVersion { get; set; }

        /// <summary>
        /// Bom与版本
        /// </summary>
        public string BomAndVersion { get; set; }

    }

    /// <summary>
    /// 更改生产（工序）
    /// </summary>
    public record ManuUpdateProcedureViewDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public long ProcedureId { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string ProcedureCode { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string ProcedureName { get; set; }

    }
    /// <summary>
    /// 在制品步骤控制操作实体（保存）
    /// </summary>
    public record ManuUpdateSaveDto
    {
        /// <summary>
        /// Sfcs
        /// </summary>
        public string[] Sfcs { get; set; }

        /// <summary>
        /// 工单ID
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }



    #region PDA

    /// <summary>
    /// 条码当前在制信息
    /// </summary>
    public record ManuSFCProdureInfoOutputDto
    {
        /// <summary>
        /// 在制工序Id
        /// </summary>
        public long? ProdureId { get; set; }

        /// <summary>
        /// 在制工序名称
        /// </summary>
        public string? ProcedureName { get; set; }

        /// <summary>
        /// 在制工序状态
        /// </summary>
        public SfcProduceStatusEnum? ProcessStatus { get; set; }

    }

    public record UpdateManuSFCProdureStatusDto
    {
        /// <summary>
        /// 更新的条码
        /// </summary>
        public string Sfc { get; set; }

        /// <summary>
        /// 更新的工序
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 更新的状态
        /// </summary>
        public SfcProduceStatusEnum ProcduceStatus { get; set; }

    }
    #endregion
}
