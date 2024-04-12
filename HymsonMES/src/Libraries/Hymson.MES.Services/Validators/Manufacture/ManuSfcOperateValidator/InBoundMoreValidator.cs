using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Manufacture.ManuSfcOperate;

namespace Hymson.MES.EquipmentServices.Validators.Manufacture
{
    /// <summary>
    /// 进站验证(多个)
    /// </summary>
    internal class InBoundMoreValidator : AbstractValidator<InBoundMoreDto>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public InBoundMoreValidator()
        {
            // 条码列表不允许为空
            RuleFor(x => x.SFCs).NotEmpty().Must(list => list.Any()).WithErrorCode(ErrorCode.MES19101);

            // 每个条码都不允许为空
            RuleFor(x => x.SFCs).Must(list => list.Any(a => !string.IsNullOrEmpty(a.SFC.Trim()))).WithErrorCode(ErrorCode.MES19003);

            // 条码不允许重复
            RuleFor(x => x.SFCs).Must(list => list.GroupBy(a => a.SFC.Trim()).Any(c => c.Count() < 2)).WithErrorCode(ErrorCode.MES19007);

        }
    }
}
