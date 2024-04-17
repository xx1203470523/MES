/*
 *creator: Karl
 *
 *describe: 生产过站面板    Dto | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-04-01 02:44:26
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Process;

namespace Hymson.MES.Services.Dtos.Manufacture
{
    /// <summary>
    /// 生产过站面板Dto
    /// </summary>
    public record ManuFacePlateProductionDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 面板Id
        /// </summary>
        public long FacePlateId { get; set; }

        /// <summary>
        /// 资源id
        /// </summary>
        public long ResourceId { get; set; }
        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode { get; set; }
        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 是否修改资源
        /// </summary>
        public bool IsResourceEdit { get; set; }

        /// <summary>
        /// 扫描JOBId 多个使用,号分割
        /// </summary>
        public string ScanJobId { get; set; }
        /// <summary>
        /// 扫描JOBId对应的Code 多个使用,号分割
        /// </summary>
        public string ScanJobCode { get; set; }

        /// <summary>
        /// 工序id
        /// </summary>
        public string ProcedureId { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcedureCode { get; set; }
        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcedureName { get; set; }

        /// <summary>
        /// 是否修改工序
        /// </summary>
        public bool IsProcedureEdit { get; set; }

        /// <summary>
        /// 是否有成功提示音
        /// </summary>
        public bool? IsSuccessBeep { get; set; }

        /// <summary>
        /// 成功提示音地址
        /// </summary>
        public string SuccessBeepUrl { get; set; }

        /// <summary>
        /// 成功提示音时间（ms）
        /// </summary>
        public decimal SuccessBeepTime { get; set; }

        /// <summary>
        /// 是否有错误提示音
        /// </summary>
        public bool? IsErrorBeep { get; set; }

        /// <summary>
        /// 错误提示音地址
        /// </summary>
        public string ErrorBeepUrl { get; set; }

        /// <summary>
        /// 错误提示音时间（ms）
        /// </summary>
        public decimal? ErrorBeepTime { get; set; }

        /// <summary>
        /// 是否绑定工单
        /// </summary>
        public bool IsShowBindWorkOrder { get; set; }

        /// <summary>
        /// 是否显示合格数量
        /// </summary>
        public bool IsShowQualifiedQty { get; set; }
        /// <summary>
        /// 显示合格颜色
        /// </summary>
        public bool IsShowQualifiedColour { get; set; }
        /// <summary>
        /// 合格颜色
        /// </summary>
        public string QualifiedColour { get; set; }

        /// <summary>
        /// 是否显示不合格数量
        /// </summary>
        public bool IsShowUnqualifiedQty { get; set; }

        /// <summary>
        /// 显示不合格颜色
        /// </summary>
        public bool IsShowUnqualifiedColour { get; set; }
        /// <summary>
        /// 报警颜色
        /// </summary>
        public string UnqualifiedColour { get; set; }

        /// <summary>
        /// 是否显示日志
        /// </summary>
        public bool IsShowLog { get; set; }

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
        /// 类型;0、产品序列码   1 载具编码
        /// </summary>
        public ManuFacePlateBarcodeTypeEnum BarcodeType { get; set; }

        /// <summary>
        /// 是否显示活动中条码
        /// </summary>
        public bool? IsShowActivityList { get; set; }
    }


    /// <summary>
    /// 生产过站面板新增Dto
    /// </summary>
    public record ManuFacePlateProductionCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 资源id
        /// </summary>
        public long? ResourceId { get; set; }

        /// <summary>
        /// 是否修改资源
        /// </summary>
        public bool? IsResourceEdit { get; set; }
        /// <summary>
        /// 扫描JOBId 多个使用,号分割
        /// </summary>
        public string? ScanJobId { get; set; }

        /// <summary>
        /// 工序id
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 是否修改工序
        /// </summary>
        public bool? IsProcedureEdit { get; set; }

        /// <summary>
        /// 是否有成功提示音
        /// </summary>
        public bool? IsSuccessBeep { get; set; }

        /// <summary>
        /// 成功提示音地址
        /// </summary>
        public string? SuccessBeepUrl { get; set; }

        /// <summary>
        /// 成功提示音时间（ms）
        /// </summary>
        public decimal? SuccessBeepTime { get; set; }

        /// <summary>
        /// 是否有错误提示音
        /// </summary>
        public bool? IsErrorBeep { get; set; }

        /// <summary>
        /// 错误提示音地址
        /// </summary>
        public string? ErrorBeepUrl { get; set; }

        /// <summary>
        /// 错误提示音时间（ms）
        /// </summary>
        public decimal? ErrorBeepTime { get; set; }

        /// <summary>
        /// 是否绑定工单
        /// </summary>
        public bool? IsShowBindWorkOrder { get; set; }

        /// <summary>
        /// 是否显示合格数量
        /// </summary>
        public bool? IsShowQualifiedQty { get; set; }

        /// <summary>
        /// 合格颜色
        /// </summary>
        public string? QualifiedColour { get; set; }

        /// <summary>
        /// 是否显示不合格数量
        /// </summary>
        public bool? IsShowUnqualifiedQty { get; set; }

        /// <summary>
        /// 报警颜色
        /// </summary>
        public string? UnqualifiedColour { get; set; }

        /// <summary>
        /// 是否显示日志
        /// </summary>
        public bool? IsShowLog { get; set; }

        /// <summary>
        /// 类型;0、产品序列码   1 载具编码
        /// </summary>
        public ManuFacePlateBarcodeTypeEnum BarcodeType { get; set; }

        /// <summary>
        /// 是否显示活动中条码
        /// </summary>
        public bool? IsShowActivityList { get; set; }
    }

    /// <summary>
    /// 生产过站面板更新Dto
    /// </summary>
    public record ManuFacePlateProductionModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 面板Id
        /// </summary>
        public long FacePlateId { get; set; }

        /// <summary>
        /// 资源id
        /// </summary>
        public long? ResourceId { get; set; }

        /// <summary>
        /// 是否修改资源
        /// </summary>
        public bool? IsResourceEdit { get; set; }
        /// <summary>
        /// 扫描JOBId 多个使用,号分割
        /// </summary>
        public string? ScanJobId { get; set; }

        /// <summary>
        /// 工序id
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 是否修改工序
        /// </summary>
        public bool? IsProcedureEdit { get; set; }

        /// <summary>
        /// 是否有成功提示音
        /// </summary>
        public bool? IsSuccessBeep { get; set; }

        /// <summary>
        /// 成功提示音地址
        /// </summary>
        public string? SuccessBeepUrl { get; set; }

        /// <summary>
        /// 成功提示音时间（ms）
        /// </summary>
        public decimal SuccessBeepTime { get; set; }

        /// <summary>
        /// 是否有错误提示音
        /// </summary>
        public bool? IsErrorBeep { get; set; }

        /// <summary>
        /// 错误提示音地址
        /// </summary>
        public string? ErrorBeepUrl { get; set; }

        /// <summary>
        /// 错误提示音时间（ms）
        /// </summary>
        public decimal? ErrorBeepTime { get; set; }

        /// <summary>
        /// 是否绑定工单
        /// </summary>
        public bool? IsShowBindWorkOrder { get; set; }

        /// <summary>
        /// 是否显示合格数量
        /// </summary>
        public bool? IsShowQualifiedQty { get; set; }

        /// <summary>
        /// 合格颜色
        /// </summary>
        public string? QualifiedColour { get; set; }

        /// <summary>
        /// 是否显示不合格数量
        /// </summary>
        public bool? IsShowUnqualifiedQty { get; set; }

        /// <summary>
        /// 报警颜色
        /// </summary>
        public string? UnqualifiedColour { get; set; }

        /// <summary>
        /// 是否显示日志
        /// </summary>
        public bool? IsShowLog { get; set; }

        /// <summary>
        /// 类型;0、产品序列码   1 载具编码
        /// </summary>
        public ManuFacePlateBarcodeTypeEnum BarcodeType { get; set; }

        /// <summary>
        /// 是否显示活动中条码
        /// </summary>
        public bool? IsShowActivityList { get; set; }
    }

    /// <summary>
    /// 生产过站面板分页Dto
    /// </summary>
    public class ManuFacePlateProductionPagedQueryDto : PagerInfo
    {
    }


    public class ManuFacePlateProductionPackageQueryDto
    {
        /// <summary>
        /// SFC 产品条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public long? ResourceId { get; set; }
    }

    /// <summary>
    /// 生产过站面板 组装界面信息
    /// </summary>
    public class ManuFacePlateProductionPackageDto
    {
        /// <summary>
        /// 主物料ID
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 主物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 主物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 主物料版本
        /// </summary>
        public string MaterialVersion { get; set; }

        /// <summary>
        /// 数据采集方式
        /// </summary>
        public MaterialSerialNumberEnum? SerialNumber { get; set; }

        /// <summary>
        /// 物料需求数量
        /// </summary>
        public decimal Usages { get; set; }

        /// <summary>
        /// 已经装配数量
        /// </summary>
        public decimal HasAssembleNum { get; set; }

        /// <summary>
        /// bom对应的主物料数量
        /// </summary>
        public int BomMainMaterialNum { get; set; }

        /// <summary>
        /// 已经组装到第几个主物料
        /// </summary>
        public int CurrentMainMaterialIndex { get; set; }

        /// <summary>
        /// 当前bom详情Id (bom下主物料对应的bomdetailId)(不是主物料ID)(表proc_bom_detail对应的ID)
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 替代物料
        /// </summary>
        public List<MainReplaceMaterial> MainReplaceMaterials { get; set; }
    }

    public class MainReplaceMaterial
    {
        public long MaterialId { get; set; }

        public string MaterialCode { get; set; }

        public string MaterialName { get; set; }

        public string MaterialVersion { get; set; }

        /// <summary>
        /// 数据采集方式
        /// </summary>
        public MaterialSerialNumberEnum? SerialNumber { get; set; }
    }

    public class ManuFacePlateProductionPackageAddDto
    {
        /// <summary>
        /// SFC 产品条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public long? ResourceId { get; set; }

        /// <summary>
        /// 主物料ID
        /// </summary>
        public long CirculationMainProductId { get; set; }

        /// <summary>
        /// 替代物料ID   有值表示使用的是替代物料
        /// </summary>
        public long? CirculationProductId { get; set; }

        /// <summary>
        /// 物料 条码 
        /// </summary>
        public string CirculationBarCode { get; set; }


        public long BomDetailId { get; set; }
    }
}
