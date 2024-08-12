using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Validators.Manufacture
{
    /// <summary>
    /// Marking信息表 验证
    /// </summary>
    internal class ManuSfcMarkingSaveValidator : AbstractValidator<IEnumerable<ManuSfcMarkingSaveDto>>
    {
        /// <summary>
        /// 
        /// </summary>
        public ManuSfcMarkingSaveValidator()
        {
            RuleFor(x => x).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10100));

            When(x => x != null && x.Any(), () =>
            {
                RuleForEach(x => x).ChildRules(c =>
                {
                    c.RuleFor(x => x.FoundBadOperationId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES19701));
                    c.RuleFor(x => x.UnqualifiedId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES19702));
                    c.RuleFor(x => x.SFC).NotEmpty().WithErrorCode(nameof(ErrorCode.MES19703));
                });

                RuleFor(x => x).Must(list =>
                {
                    var group = list.GroupBy(x => new { x.UnqualifiedId, x.SFC });
                    if (group.Any(x => x.Count() > 1))
                    {
                        return false;
                    }
                    return true;
                }).WithErrorCode(nameof(ErrorCode.MES19714));
            });
        }
    }

}
