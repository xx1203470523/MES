using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Manufacture.ManuSfcOperate;

namespace Hymson.MES.EquipmentServices.Validators.Manufacture
{
    /// <summary>
    /// 出站验证(多个)
    /// </summary>
    internal class OutBoundMoreValidator : AbstractValidator<OutBoundMoreDto>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public OutBoundMoreValidator()
        {
            // 条码列表不允许为空
            RuleFor(x => x.SFCs).NotEmpty().Must(list => list.Any()).WithErrorCode(ErrorCode.MES19101);

            // 每个条码都不允许为空
            RuleFor(x => x.SFCs).Must(list => list.Any(c => !string.IsNullOrEmpty(c.SFC.Trim()))).WithErrorCode(ErrorCode.MES19003);

            // 条码不允许重复
            RuleFor(x => x.SFCs).Must(list => list.GroupBy(c => c.SFC.Trim()).Any(c => c.Count() < 2)).WithErrorCode(ErrorCode.MES19007);

        }
    }
}
