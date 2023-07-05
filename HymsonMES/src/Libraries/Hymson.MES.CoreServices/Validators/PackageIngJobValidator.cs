using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.CoreServices.Bos.Job;

namespace Hymson.MES.Services.Validators.Equipment
{
    /// <summary>
    /// 维修Job 验证(结束)
    /// </summary>
    internal class PackageIngJobValidator : AbstractValidator<PackageIngRequestBo>
    {
        /// <summary>
        ///  
        /// </summary>
        public PackageIngJobValidator()
        {
            RuleFor(x => x).NotEmpty().WithErrorCode(ErrorCode.MES16332);
            RuleFor(x => x.SiteId).NotEmpty().WithErrorCode(ErrorCode.MES10112);
            RuleFor(x => x.ProcedureId).NotEmpty().WithErrorCode(ErrorCode.MES16335);
            RuleFor(x => x.ResourceId).NotEmpty().WithErrorCode(ErrorCode.MES16334);
        }
    }
}