using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.Services.Validators.Process
{
    /// <summary>
    /// 设备参数组详情 更新 验证
    /// </summary>
    internal class ProcEquipmentGroupParamDetailCreateValidator: AbstractValidator<ProcEquipmentGroupParamDetailCreateDto>
    {
        public ProcEquipmentGroupParamDetailCreateValidator()
        {
            RuleFor(x => x).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10100));
            RuleFor(x => x.ParamId).Must(it => it > 0).WithErrorCode(nameof(ErrorCode.MES18721));
            RuleFor(x => x.DecimalPlaces).Must(it => it==null||it >= 0&&it<10).WithErrorCode(nameof(ErrorCode.MES18722));
            RuleFor(x => x).Must(it => it.MaxValue == null || it.MinValue == null || (it.MaxValue >= it.MinValue)).WithErrorCode(nameof(ErrorCode.MES18723));
        }
    }

    
}
