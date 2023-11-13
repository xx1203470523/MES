using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.Services.Validators.Process
{
    /// <summary>
    /// 标准参数表 更新 验证
    /// </summary>
    internal class ProcParameterCreateValidator : AbstractValidator<ProcParameterCreateDto>
    {
        public ProcParameterCreateValidator()
        {
            RuleFor(x => x.ParameterCode).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10509));
            RuleFor(x => x.ParameterCode).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES10511));
            RuleFor(x => x.ParameterName).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10510));
            RuleFor(x => x.ParameterName).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES10515));
            RuleFor(x => x.ParameterUnit).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10508));
            RuleFor(x => x.ParameterUnit).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES10512));
            
            RuleFor(x => x.DataType).Must(it => Enum.IsDefined(typeof(DataTypeEnum), it)).WithErrorCode(ErrorCode.MES10527);
        }
    }

    /// <summary>
    /// 标准参数表 修改 验证
    /// </summary>
    internal class ProcParameterModifyValidator : AbstractValidator<ProcParameterModifyDto>
    {
        public ProcParameterModifyValidator()
        {

            RuleFor(x => x.ParameterUnit).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10508));
            RuleFor(x => x.ParameterUnit).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES10512));

            RuleFor(x => x.DataType).Must(it => Enum.IsDefined(typeof(DataTypeEnum), it)).WithErrorCode(ErrorCode.MES10527);
        }
    }

    internal class ProcParameterImportValidator : AbstractValidator<ProcParameterImportDto>
    {
        public ProcParameterImportValidator()
        {
            RuleFor(x => x.ParameterCode).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10509));
            RuleFor(x => x.ParameterCode).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES10511));
            RuleFor(x => x.ParameterName).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10510));
            RuleFor(x => x.ParameterName).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES10515));
            RuleFor(x => x.ParameterUnit).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10508));
            RuleFor(x => x.ParameterUnit).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES10512));

            RuleFor(x => x.DataType).Must(it => Enum.IsDefined(typeof(DataTypeEnum), it)).WithErrorCode(ErrorCode.MES10527);

        }
    }
}
