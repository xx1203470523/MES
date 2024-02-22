using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Services.Dtos.Manufacture
{
    /// <summary>
    /// Marking录入校验Dto
    /// </summary>
    public class ManuMarkingCheckDto
    {
        /// <summary>
        /// 发现不良工序Id
        /// </summary>
        public string? FoundBadOperationId { get; set; }

        /// <summary>
        /// 发现不良工序编码
        /// </summary>
        public string? FoundBadOperationCode { get; set; }

        /// <summary>
        /// 不合格代码Id
        /// </summary>
        public string? UnqualifiedId { get; set; }

        /// <summary>
        /// 不合格代码
        /// </summary>
        public string? UnqualifiedCode { get; set; }

        /// <summary>
        /// 拦截工序Id
        /// </summary>
        public string? InterceptProcedureId { get; set; }

        /// <summary>
        /// 拦截工序Code
        /// </summary>
        public string? InterceptProcedureCode { get; set; }

        /// <summary>
        /// 产品序列码
        /// </summary>
        public string? Sfc { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }

    /// <summary>
    /// Marking录入展示Dto
    /// </summary>
    public class MarkingEnterViewDto { 

        /// <summary>
        /// 产品序列码
        /// </summary>
        public string? Sfc { get; set; }

        /// <summary>
        /// 产品序列码状态
        /// </summary>
        public SfcStatusEnum? Status{ get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string? ProcedureCode { get; set; }

        /// <summary>
        /// 工单编码
        /// </summary>
        public string? WorkOrderCode { get; set; }

        /// <summary>
        /// 物料编码/版本
        /// </summary>
        public string? MaterialCodeVersion { get; set; }

        /// <summary>
        /// 不合格代码
        /// </summary>
        public string? UnqualifiedCode { get; set; }

        /// <summary>
        /// 不合格代码名称
        /// </summary>
        public string? UnqualifiedName { get; set; }

        /// <summary>
        /// 不良发现工序编码
        /// </summary>
        public string? FoundBadOperationCode { get; set; }

        /// <summary>
        /// 拦截工序编码
        /// </summary>
        public string? InterceptProcedureCode { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }

    /// <summary>
    /// marking关闭展示Dto
    /// </summary>
    public class MarkingCloseViewDto {
        /// <summary>
        /// Id
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 不合格代码
        /// </summary>
        public string? UnqualifiedCode { get; set; }

        /// <summary>
        /// 不合格代码名称
        /// </summary>
        public string? UnqualifiedName { get; set; }

        /// <summary>
        /// 拦截工序编码
        /// </summary>
        public string? InterceptProcedureCode { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 不合格记录开关;1、开启  2、关闭
        /// </summary>
        public bool? Status { get; set; }

        /// <summary>
        /// 资源代码
        /// </summary>
        public string? ResourceCode { get; set; }
    }

    /// <summary>
    /// Marking关闭确认提交Dto
    /// </summary>
    public class MarkingCloseConfirmDto {
        /// <summary>
        /// 不良录入修改内容
        /// </summary>
        public IEnumerable<MarkingCloseConfirmBadRecordDto>? MarkingCloseConfirmBadRecords { get; set; }

        /// <summary>
        /// 产品序列码
        /// </summary>
        public string? Sfc { get; set; }

        /// <summary>
        /// 条码步骤备注
        /// </summary>
        public string? SfcStepRemark { get; set; }
    }

    /// <summary>
    /// Marking关闭不良录入Dto
    /// </summary>
    public class MarkingCloseConfirmBadRecordDto {
        /// <summary>
        /// 产品不良录入Id
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 不合格记录开关;1、开启  2、关闭
        /// </summary>
        public ProductBadRecordStatusEnum? Status { get; set; }

        /// <summary>
        /// 产品不良录入备注
        /// </summary>
        public string? Remark { get; set; }
    }
}
