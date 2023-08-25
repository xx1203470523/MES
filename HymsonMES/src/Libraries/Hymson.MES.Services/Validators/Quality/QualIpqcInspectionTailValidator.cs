using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Quality;

namespace Hymson.MES.Services.Validators.Quality
{
    /// <summary>
    /// 尾检检验单 验证
    /// </summary>
    internal class QualIpqcInspectionTailSaveValidator : AbstractValidator<QualIpqcInspectionTailSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public QualIpqcInspectionTailSaveValidator()
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
    internal class QualIpqcInspectionTailSampleAddValidator : AbstractValidator<List<QualIpqcInspectionTailSampleCreateDto>>
    {
        /// <summary>
        /// 
        /// </summary>
        public QualIpqcInspectionTailSampleAddValidator()
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
