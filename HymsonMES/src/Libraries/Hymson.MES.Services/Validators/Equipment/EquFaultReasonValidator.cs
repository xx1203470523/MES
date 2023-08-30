using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Validators.Equipment
{
    /// <summary>
    /// 设备故障原因表 更新 验证
    /// </summary>
    internal class EquFaultReasonCreateValidator : AbstractValidator<EquFaultReasonSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public EquFaultReasonCreateValidator()
        {
            RuleFor(x => x.FaultReasonCode).NotEmpty().WithErrorCode(ErrorCode.MES13009);
            RuleFor(x => x.FaultReasonCode).MaximumLength(50).WithErrorCode(ErrorCode.MES10109);
            RuleFor(x => x.FaultReasonName).NotEmpty().WithErrorCode(ErrorCode.MES13010);
            RuleFor(x => x.FaultReasonName).MaximumLength(50).WithErrorCode(ErrorCode.MES10110);
            //RuleFor(x => x.UseStatus).NotEmpty().WithErrorCode(ErrorCode.MES13008);
            //RuleFor(x => x.UseStatus).Must(it => Enum.IsDefined(typeof(SysDataStatusEnum), it ?? 0)).WithErrorCode(ErrorCode.MES13012);
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111").WithMessage("111");
        }
    }
}
