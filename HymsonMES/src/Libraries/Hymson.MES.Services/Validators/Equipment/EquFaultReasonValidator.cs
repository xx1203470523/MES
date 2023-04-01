using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Validators.Equipment
{
    /// <summary>
    /// 设备故障原因表 更新 验证
    /// </summary>
    internal class EquFaultReasonCreateValidator: AbstractValidator<EquFaultReasonSaveDto>
    {
        public EquFaultReasonCreateValidator()
        {
            RuleFor(x => x.FaultReasonCode).NotEmpty().WithErrorCode(ErrorCode.MES13009);
            RuleFor(x => x.FaultReasonName).NotEmpty().WithErrorCode(ErrorCode.MES13010);
            RuleFor(x => x.UseStatus).NotEmpty().WithErrorCode(ErrorCode.MES13008);
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111").WithMessage("111");
        }
    }
}
