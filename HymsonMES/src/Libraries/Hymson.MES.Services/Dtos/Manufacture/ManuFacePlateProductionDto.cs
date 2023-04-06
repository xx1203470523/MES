/*
 *creator: Karl
 *
 *describe: 生产过站面板    Dto | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-04-01 02:44:26
 */

using Hymson.Infrastructure;

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
        /// 是否修改资源
        /// </summary>
        public bool IsResourceEdit { get; set; }

       /// <summary>
        /// 工序id
        /// </summary>
        public string ProcedureId { get; set; }

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
        /// 合格颜色
        /// </summary>
        public string QualifiedColour { get; set; }

       /// <summary>
        /// 是否显示不合格数量
        /// </summary>
        public bool IsShowUnqualifiedQty { get; set; }

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
        /// 面板Id
        /// </summary>
        public long FacePlateId { get; set; }

       /// <summary>
        /// 资源id
        /// </summary>
        public long ResourceId { get; set; }

       /// <summary>
        /// 是否修改资源
        /// </summary>
        public bool IsResourceEdit { get; set; }

       /// <summary>
        /// 工序id
        /// </summary>
        public string ProcedureId { get; set; }

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
        /// 合格颜色
        /// </summary>
        public string QualifiedColour { get; set; }

       /// <summary>
        /// 是否显示不合格数量
        /// </summary>
        public bool IsShowUnqualifiedQty { get; set; }

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
        public long ResourceId { get; set; }

       /// <summary>
        /// 是否修改资源
        /// </summary>
        public bool IsResourceEdit { get; set; }

       /// <summary>
        /// 工序id
        /// </summary>
        public string ProcedureId { get; set; }

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
        /// 合格颜色
        /// </summary>
        public string QualifiedColour { get; set; }

       /// <summary>
        /// 是否显示不合格数量
        /// </summary>
        public bool IsShowUnqualifiedQty { get; set; }

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

       

    }

    /// <summary>
    /// 生产过站面板分页Dto
    /// </summary>
    public class ManuFacePlateProductionPagedQueryDto : PagerInfo
    {
    }
}
