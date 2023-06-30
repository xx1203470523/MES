using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.CoreServices.Bos.Job;

namespace Hymson.MES.Services.Validators.Equipment
{
    /// <summary>
    /// 维修Job 验证
    /// </summary>
    internal class RepairStartJobValidator : AbstractValidator<RepairStartRequestBo>
    {
        /// <summary>
        /// 
        /// </summary>
        public RepairStartJobValidator()
        {
            RuleFor(x => x).NotEmpty().WithErrorCode(ErrorCode.MES16332);
            RuleFor(x => x.SFCs).NotEmpty().Must(it => it.Any(s => s.Trim() != "")).WithErrorCode(ErrorCode.MES16332);
            RuleFor(x => x.SiteId).NotEmpty().WithErrorCode(ErrorCode.MES10112);
            RuleFor(x => x.ProcedureId).NotEmpty().WithErrorCode(ErrorCode.MES16335);
            RuleFor(x => x.ResourceId).NotEmpty().WithErrorCode(ErrorCode.MES16334);
            //RuleFor(x => x.UserName).NotEmpty().WithErrorCode(ErrorCode.);
        }
    }
}
