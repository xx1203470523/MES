/*
 *creator: Karl
 *
 *describe: 生产过站面板    Dto | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-04-01 02:44:26
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Services.Dtos.Manufacture
{
    //传入
    /// <summary>
    /// 在制品维修 获取初始信息
    /// </summary>
    public record ManuFacePlateRepairInitialDto
    {
        /// <summary>
        /// 面板Id
        /// </summary>
        public long FacePlateId { get; set; }

    }


    /// <summary>
    /// 在制品维修 执行作业
    /// </summary>
    public record ManuFacePlateRepairExJobDto
    {
        /// <summary>
        /// 面板ID
        /// </summary>
        public long FacePlateId { get; set; }

        /// <summary>
        /// 按钮ID
        /// </summary>
        public long FacePlateButtonId { get; set; }

        /// <summary>
        /// 工序id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 产品条码
        /// </summary>
        public string SFC { get; set; }

    }


    /// <summary>
    /// 在制品维修 开始维修
    /// </summary>
    public record ManuFacePlateRepairBeginRepairDto
    {
        /// <summary>
        /// 工序id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 产品条码
        /// </summary>
        public string SFC { get; set; }

    }

    /// <summary>
    /// 在制品维修确认提交Dto
    /// </summary>
    public record ManuFacePlateRepairConfirmSubmitDto
    {

        /// <summary>
        /// SFC
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 面板Id
        /// </summary>
        public long FacePlateId { get; set; }

        /// <summary>
        /// 资源id
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 工序id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 返回工序id
        /// </summary>
        public long ReturnProcedureId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 明细
        /// </summary>
        public IEnumerable<ManuFacePlateRepairConfirmSubmitDetailDto> confirmSubmitDetail { get; set; }
    }
    /// <summary>
    /// 在制品维修确认提交明细to
    /// </summary>
    public record ManuFacePlateRepairConfirmSubmitDetailDto
    {
        /// <summary>
        /// 不良录入Id
        /// </summary>
        public long BadRecordId { get; set; }
        /// <summary>
        /// 维修记录ID
        /// </summary>
        public long SfcRepairId { get; set; }

        /// <summary>
        /// 维修方法
        /// </summary>
        public string RepairMethod { get; set; }

        /// <summary>
        /// 原因分析
        /// </summary>
        public string CauseAnalyse { get; set; }

        /// <summary>
        /// 是否关闭
        /// </summary>
        public ProductBadRecordStatusEnum IsClose { get; set; }
    }


    //传出
    /// <summary>
    /// 在制品维修 获取初始信息
    /// </summary>
    public record ManuFacePlateRepairInitialInfoDto
    {
        /// <summary>
        /// 工序id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode { get; set; }
    }

    /// <summary>
    /// 在制维修展示数据
    /// </summary>
    public record ManuFacePlateRepairOpenInfoDto
    {
        /// <summary>
        /// 展示产品信息
        /// </summary>
        public ManuFacePlateRepairProductInfoDto productInfo { get; set; }

        /// <summary>
        /// 展示不合格信息
        /// </summary>
        public IEnumerable<ManuFacePlateRepairProductBadInfoDto> productBadInfo { get; set; }

        /// <summary>
        /// 返回工序
        /// </summary>
        public IEnumerable<ManuFacePlateRepairReturnProcedureDto> returnProcedureInfo { get; set; }

    }

    /// <summary>
    /// 展示产品信息
    /// </summary> 
    public record ManuFacePlateRepairProductInfoDto
    {
        /// <summary>
        /// 产品条码
        /// </summary>
        public string SFC { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcedureCode { get; set; }
        /// <summary>
        /// 工单号
        /// </summary>
        public string OrderCode { get; set; }
        /// <summary>
        /// 产品编码
        /// </summary>
        public string MaterialCode { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        public string? Version { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string MaterialName { get; set; }
        /// <summary>
        /// 展示不合格信息
        /// </summary>
        public IEnumerable<ManuFacePlateRepairProductBadInfoDto> badInfoDtos { get; set; }
    }
    /// <summary>
    /// 展示不合格信息
    /// </summary>
    public record ManuFacePlateRepairProductBadInfoDto
    {
        /// <summary>
        /// 不良录入Id
        /// </summary>
        public long BadRecordId { get; set; }
        /// <summary>
        /// 不合格代码ID
        /// </summary>
        public long UnqualifiedId { get; set; }
        /// <summary>
        /// 不合格编码
        /// </summary>
        public string UnqualifiedCode { get; set; }
        /// <summary>
        /// 不合格名称
        /// </summary>
        public string UnqualifiedCodeName { get; set; }
        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResCode { get; set; }

        /// <summary>
        /// 原因分析
        /// </summary>
        public string CauseAnalyse { get; set; }

        /// <summary>
        /// 维修方法
        /// </summary>
        public string RepairMethod { get; set; }

        /// <summary>
        /// 是否关闭
        /// </summary>
        public ProductBadRecordStatusEnum IsClose { get; set; }
    }

    /// <summary>
    /// 返回工序信息
    /// </summary>
    public record ManuFacePlateRepairReturnProcedureDto
    {
        /// <summary>
        /// 工序Id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcedureCode { get; set; }
    }




    /// <summary>
    /// 在制品维修面板Dto
    /// </summary>
    public record ManuFacePlateRepairDto : BaseEntityDto
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
        /// 工序id
        /// </summary>
        public long ProcedureId { get; set; }

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
        /// 是否显示产品列表
        /// </summary>
        public bool IsShowProductList { get; set; }

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


    }


    /// <summary>
    /// 在制品维修新增Dto
    /// </summary>
    public record ManuFacePlateRepairCreateDto : BaseEntityDto
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
        /// 工序id
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 是否修改工序
        /// </summary>
        public bool? IsProcedureEdit { get; set; }

        /// <summary>
        /// 是否显示产品列表
        /// </summary>
        public bool? IsShowProductList { get; set; }
    }

    /// <summary>
    /// 在制品维修更新Dto
    /// </summary>
    public record ManuFacePlateRepairModifyDto : BaseEntityDto
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
        /// 工序id
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 是否修改工序
        /// </summary>
        public bool? IsProcedureEdit { get; set; }

        /// <summary>
        /// 是否显示产品列表
        /// </summary>
        public bool? IsShowProductList { get; set; }
    }

    /// <summary>
    /// 在制品维修面板分页Dto
    /// </summary>
    public class ManuFacePlateRepairPagedQueryDto : PagerInfo
    {
    }
}
