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
            RuleFor(x => x.Code).NotEmpty().WithErrorCode(ErrorCode.MES13009);
            RuleFor(x => x.Code).MaximumLength(50).WithErrorCode(ErrorCode.MES10109);
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(ErrorCode.MES13010);
            RuleFor(x => x.Name).MaximumLength(50).WithErrorCode(ErrorCode.MES10110);
        }
    }
}
