using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Quality;

namespace Hymson.MES.Services.Validators.Quality
{
    /// <summary>
    /// 巡检检验单 验证
    /// </summary>
    internal class QualIpqcInspectionPatrolSaveValidator : AbstractValidator<QualIpqcInspectionPatrolSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public QualIpqcInspectionPatrolSaveValidator()
        {
            RuleFor(x => x.TriggerCondition).IsInEnum().WithErrorCode(nameof(ErrorCode.MES13201));
            RuleFor(x => x.WorkOrderId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES13202));
            RuleFor(x => x.ProcedureId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES13203));
            RuleFor(x => x.ResourceId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES13204));
        }
    }

    /// <summary>
    /// 样品检验数据录入 验证
    /// </summary>
    internal class QualIpqcInspectionPatrolSampleAddValidator : AbstractValidator<List<QualIpqcInspectionPatrolSampleCreateDto>>
    {
        /// <summary>
        /// 
        /// </summary>
        public QualIpqcInspectionPatrolSampleAddValidator()
        {
            RuleFor(x => x).NotEmpty().WithErrorCode(nameof(ErrorCode.MES13201));

            When(x => x != null && x.Any(), () =>
            {
                RuleForEach(x => x).ChildRules(c =>
                {
                    c.RuleFor(x => x.IsQualified).NotEmpty().WithErrorCode(nameof(ErrorCode.MES13206));
                });
            });
        }
    }

}
