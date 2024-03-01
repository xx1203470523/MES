using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Plan;

namespace Hymson.MES.Services.Validators.Plan
{
    /// <summary>
    /// 班制 验证
    /// </summary>
    internal class PlanShiftSaveValidator: AbstractValidator<PlanShiftSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public PlanShiftSaveValidator()
        {
            RuleFor(x => x.Code).NotEmpty().WithErrorCode(nameof(ErrorCode.MES19501));
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES19502));
            
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }


    internal class PlanShiftDetailValidator : AbstractValidator<PlanShiftDetailModifyDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public PlanShiftDetailValidator()
        {
            RuleFor(x => x.ShiftType).IsInEnum().WithErrorCode(nameof(ErrorCode.MES19509));
            RuleFor(x => x.StartTime).NotEmpty().WithErrorCode(nameof(ErrorCode.MES19503));
            RuleFor(x => x.EndTime).NotEmpty().WithErrorCode(nameof(ErrorCode.MES19504));
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
