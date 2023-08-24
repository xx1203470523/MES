using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums.Quality;
using Hymson.MES.Services.Dtos.Quality;

namespace Hymson.MES.Services.Validators.Quality
{
    /// <summary>
    /// 首检检验单 验证
    /// </summary>
    internal class QualIpqcInspectionHeadSaveValidator : AbstractValidator<QualIpqcInspectionHeadSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public QualIpqcInspectionHeadSaveValidator()
        {
            RuleFor(x => x.TriggerCondition).IsInEnum().WithErrorCode(nameof(ErrorCode.MES13201));

            When(x => x.TriggerCondition != TriggerConditionEnum.Shift, () =>
            {
                RuleFor(x => x.WorkOrderId).Must(x => x.HasValue && x.Value > 0).WithErrorCode(nameof(ErrorCode.MES13202));
                RuleFor(x => x.ProcedureId).Must(x => x.HasValue && x.Value > 0).WithErrorCode(nameof(ErrorCode.MES13203));
                RuleFor(x => x.ResourceId).Must(x => x.HasValue && x.Value > 0).WithErrorCode(nameof(ErrorCode.MES13204));
            });
        }
    }

    /// <summary>
    /// 首检样品录入 验证
    /// </summary>
    internal class QualIpqcInspectionHeadSampleAddValidator : AbstractValidator<List<QualIpqcInspectionHeadSampleCreateDto>>
    {
        /// <summary>
        /// 
        /// </summary>
        public QualIpqcInspectionHeadSampleAddValidator()
        {
            RuleFor(x => x).NotEmpty().WithErrorCode(nameof(ErrorCode.MES13201));

            When(x => x != null && x.Any(), () =>
            {
                RuleForEach(x => x).ChildRules(c =>
                {
                    //c.RuleFor(x => x.InspectionValue).NotEmpty().WithErrorCode(nameof(ErrorCode.MES13205));
                    c.RuleFor(x => x.IsQualified).NotEmpty().WithErrorCode(nameof(ErrorCode.MES13206));
                });
            });
        }
    }

}
