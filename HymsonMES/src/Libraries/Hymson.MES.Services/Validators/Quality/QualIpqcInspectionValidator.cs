using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;
using Hymson.MES.Services.Dtos.Quality;

namespace Hymson.MES.Services.Validators.Quality
{
    /// <summary>
    /// IPQC检验项目 验证
    /// </summary>
    internal class QualIpqcInspectionSaveValidator : AbstractValidator<QualIpqcInspectionSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public QualIpqcInspectionSaveValidator()
        {
            RuleFor(x => x.ParameterGroupCode).NotEmpty().WithErrorCode(nameof(ErrorCode.MES13101));
            RuleFor(x => x.Type).IsInEnum().WithErrorCode(nameof(ErrorCode.MES13102));
            RuleFor(x => x.SampleQty).GreaterThan(0).WithErrorCode(nameof(ErrorCode.MES13103));
            RuleFor(x => x.GenerateCondition).GreaterThan(0).WithErrorCode(nameof(ErrorCode.MES13104));
            RuleFor(x => x.GenerateConditionUnit).IsInEnum().WithErrorCode(nameof(ErrorCode.MES13105));
            RuleFor(x => x.ControlTime).GreaterThan(0).When(x => x.ControlTime.HasValue && x.ControlTime != 0).WithErrorCode(nameof(ErrorCode.MES13106));
            RuleFor(x => x.ControlTimeUnit).IsInEnum().When(x => x.ControlTimeUnit.HasValue).WithErrorCode(nameof(ErrorCode.MES13107));
            RuleFor(x => x.Version).NotEmpty().WithErrorCode(nameof(ErrorCode.MES13108));
            RuleFor(x => x.Version).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES13109));
            //RuleFor(x => x.Status).IsInEnum().WithErrorCode(nameof(ErrorCode.MES13110));

            //参数项目校验
            When(x => x.Details != null && x.Details.Any(), () =>
            {
                //RuleForEach(x => x.Details).ChildRules(c =>
                //{
                //    c.RuleFor(x => x.ParameterId).NotEmpty();
                //});
                RuleFor(x => x.Details).Must((details) =>
                {
                    if (details == null) return true;
                    var groups = details.GroupBy(x => x.ParameterId).Select(x => new { x.Key, Count = x.Count() });
                    if (groups.Any(x => x.Count > 1))
                    {
                        return false;
                    }
                    return true;
                }).WithErrorCode(nameof(ErrorCode.MES13111));
            });

            //检验规则校验
            When(x => x.Rules != null && x.Rules.Any(), () =>
            {
                //RuleForEach(x => x.Rules).ChildRules(c =>
                //{
                //    c.RuleFor(x => x.Way).IsInEnum();
                //});
                RuleFor(x => x.Rules).Must((rules) =>
                {
                    if (rules == null) return true;
                    var groups = rules.GroupBy(x => x.Way).Select(x => new { x.Key, Count = x.Count() });
                    if (groups.Any(x => x.Count > 1))
                    {
                        return false;
                    }
                    return true;
                }).WithErrorCode(nameof(ErrorCode.MES13112));
            });

            ////状态为启用时校验关联表数据
            //When(x => x.Status == SysDataStatusEnum.Enable, () =>
            //{
            //    RuleFor(x => x.Details).NotEmpty().WithErrorCode(nameof(ErrorCode.MES13121));
            //});
            //When(x => x.Status == SysDataStatusEnum.Enable && x.Type == IPQCTypeEnum.FAI, () =>
            //{
            //    RuleFor(x => x.Rules).NotEmpty().WithErrorCode(nameof(ErrorCode.MES13122));
            //});
        }
    }

}
