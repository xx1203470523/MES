using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Manufacture.ManuSfcOperate;

namespace Hymson.MES.EquipmentServices.Validators.Manufacture
{
    /// <summary>
    /// 出站验证
    /// </summary>
    internal class OutBoundValidator : AbstractValidator<OutBoundDto>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public OutBoundValidator()
        {
            RuleFor(x => x.SFC).NotEmpty().WithErrorCode(nameof(ErrorCode.MES19003));

            // 每个条码都不允许为空
            RuleFor(x => x.SFC).Must(sfc => !string.IsNullOrEmpty(sfc.Trim())).WithErrorCode(ErrorCode.MES19003);
        }
    }
}
