using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Manufacture
{
    /// <summary>
    /// 容器包装Dto
    /// </summary>
    public record ManuFacePlateContainerPackDto : BaseEntityDto
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
        /// 容器ID
        /// </summary>
        public long? ContainerId { get; set; }

        /// <summary>
        /// 容器编码
        /// </summary>
        public string ContainerCode { get; set; }

        /// <summary>
        /// 包装等级
        /// </summary>
        public int? PackingLevel { get; set; }

        /// <summary>
        /// 是否修改工序
        /// </summary>
        public bool IsProcedureEdit { get; set; }

        /// <summary>
        /// JobId
        /// </summary>
        public string ScanJobId { get; set; }

        /// <summary>
        /// 扫码作业id
        /// </summary>
        public string ScanJobCode { get; set; }

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
        /// 是否允许不同版本物料
        /// </summary>
        public bool IsAllowDifferentMaterial { get; set; }

        /// <summary>
        /// 是否允许混工单
        /// </summary>
        public bool IsMixedWorkOrder { get; set; }

        /// <summary>
        /// 是否允许排队产品
        /// </summary>
        public bool IsAllowQueueProduct { get; set; }

        /// <summary>
        /// 是否允许完成产品
        /// </summary>
        public bool IsAllowCompleteProduct { get; set; }

        /// <summary>
        /// 是否允许活动产品
        /// </summary>
        public bool IsAllowActiveProduct { get; set; }

        /// <summary>
        /// 是否显示最小值
        /// </summary>
        public bool IsShowMinQty { get; set; }

        /// <summary>
        /// 是否显示最大值
        /// </summary>
        public bool IsShowMaxQty { get; set; }

        /// <summary>
        /// 是否显示当前值
        /// </summary>
        public bool IsShowCurrentQty { get; set; }

        /// <summary>
        /// 是否显示合格颜色
        /// </summary>
        public bool IsShowQualifiedColour { get; set; }

        /// <summary>
        /// 合格颜色
        /// </summary>
        public string QualifiedColour { get; set; }

        /// <summary>
        /// 显示不合格颜色
        /// </summary>
        public bool IsShowErrorsColour { get; set; }

        /// <summary>
        /// 报警颜色
        /// </summary>
        public string ErrorsColour { get; set; }

        /// <summary>
        /// 是否显示日志
        /// </summary>
        public bool IsShowLog { get; set; }

        
    }


    /// <summary>
    /// 容器包装面板新增Dto
    /// </summary>
    public record ManuFacePlateContainerPackCreateDto : BaseEntityDto
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
        /// 容器id
        /// </summary>
        public long? ContainerId { get; set; }

        /// <summary>
        /// 容器编码
        /// </summary>
        public string? ContainerCode { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        public string? ResourceCode { get; set; }
        /// <summary>
        /// 资源名称
        /// </summary>
        public string? ResourceName { get; set; }

        /// <summary>
        /// 是否修改资源
        /// </summary>
        public bool? IsResourceEdit { get; set; }

        /// <summary>
        /// 工序id
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string? ProcedureCode { get; set; }
        /// <summary>
        /// 工序名称
        /// </summary>
        public string? ProcedureName { get; set; }

        /// <summary>
        /// 是否修改工序
        /// </summary>
        public bool? IsProcedureEdit { get; set; }

        /// <summary>
        /// JobId
        /// </summary>
        public string? ScanJobId { get; set; }

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
        /// 是否允许不同版本物料
        /// </summary>
        public bool? IsAllowDifferentMaterial { get; set; }

        /// <summary>
        /// 是否允许混工单
        /// </summary>
        public bool? IsMixedWorkOrder { get; set; }
        /// <summary>
        /// 是否允许排队产品
        /// </summary>
        public bool? IsAllowQueueProduct { get; set; }
        /// <summary>
        /// 是否允许完成产品
        /// </summary>
        public bool? IsAllowCompleteProduct { get; set; }

        /// <summary>
        /// 是否允许活动产品
        /// </summary>
        public bool? IsAllowActiveProduct { get; set; }

        /// <summary>
        /// 是否显示最小值
        /// </summary>
        public bool? IsShowMinQty { get; set; }

        /// <summary>
        /// 是否显示最大值
        /// </summary>
        public bool? IsShowMaxQty { get; set; }
        /// <summary>
        /// 是否显示当前值
        /// </summary>
        public bool? IsShowCurrentQty { get; set; }

        /// <summary>
        /// 是否显示合格颜色
        /// </summary>
        public bool? IsShowQualifiedColour { get; set; }

        /// <summary>
        /// 合格颜色
        /// </summary>
        public string? QualifiedColour { get; set; }

        /// <summary>
        /// 显示不合格颜色
        /// </summary>
        public bool? IsShowErrorsColour { get; set; }
        /// <summary>
        /// 报警颜色
        /// </summary>
        public string? ErrorsColour { get; set; }

        /// <summary>
        /// 是否显示日志
        /// </summary>
        public bool? IsShowLog { get; set; }
    }

    /// <summary>
    /// 容器包装面板更新Dto
    /// </summary>
    public record ManuFacePlateContainerPackModifyDto : BaseEntityDto
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
        /// 容器id
        /// </summary>
        public long? ContainerId { get; set; }

        /// <summary>
        /// 容器编码
        /// </summary>
        public string? ContainerCode { get; set; }

        /// <summary>
        /// 资源id
        /// </summary>
        public long? ResourceId { get; set; }
        /// <summary>
        /// 资源编码
        /// </summary>
        public string? ResourceCode { get; set; }
        /// <summary>
        /// 资源名称
        /// </summary>
        public string? ResourceName { get; set; }

        /// <summary>
        /// 是否修改资源
        /// </summary>
        public bool? IsResourceEdit { get; set; }

        /// <summary>
        /// 工序id
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string? ProcedureCode { get; set; }
        /// <summary>
        /// 工序名称
        /// </summary>
        public string? ProcedureName { get; set; }

        /// <summary>
        /// 是否修改工序
        /// </summary>
        public bool? IsProcedureEdit { get; set; }

        /// <summary>
        /// JobId
        /// </summary>
        public string? ScanJobId { get; set; }

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
        /// 是否允许不同版本物料
        /// </summary>
        public bool? IsAllowDifferentMaterial { get; set; }

        /// <summary>
        /// 是否允许混工单
        /// </summary>
        public bool? IsMixedWorkOrder { get; set; }
        /// <summary>
        /// 是否允许排队产品
        /// </summary>
        public bool? IsAllowQueueProduct { get; set; }
        /// <summary>
        /// 是否允许完成产品
        /// </summary>
        public bool? IsAllowCompleteProduct { get; set; }

        /// <summary>
        /// 是否允许活动产品
        /// </summary>
        public bool? IsAllowActiveProduct { get; set; }

        /// <summary>
        /// 是否显示最小值
        /// </summary>
        public bool? IsShowMinQty { get; set; }

        /// <summary>
        /// 是否显示最大值
        /// </summary>
        public bool? IsShowMaxQty { get; set; }
        /// <summary>
        /// 是否显示当前值
        /// </summary>
        public bool? IsShowCurrentQty { get; set; }

        /// <summary>
        /// 是否显示合格颜色
        /// </summary>
        public bool? IsShowQualifiedColour { get; set; }

        /// <summary>
        /// 合格颜色
        /// </summary>
        public string? QualifiedColour { get; set; }

        /// <summary>
        /// 显示不合格颜色
        /// </summary>
        public bool? IsShowErrorsColour { get; set; }
        /// <summary>
        /// 报警颜色
        /// </summary>
        public string? ErrorsColour { get; set; }

        /// <summary>
        /// 是否显示日志
        /// </summary>
        public bool? IsShowLog { get; set; }
    }
}
