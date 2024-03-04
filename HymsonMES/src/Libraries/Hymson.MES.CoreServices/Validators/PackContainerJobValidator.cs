using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.CoreServices.Bos.Job;

namespace Hymson.MES.CoreServices.Validators
{
    /// <summary>
    /// 维修Job 验证(结束)
    /// </summary>
    internal class PackContainerJobValidator : AbstractValidator<ManuFacePlatePackBo>
    {
        /// <summary>
        ///  
        /// </summary>
        public PackContainerJobValidator()
        {
            RuleFor(x => x).NotEmpty().WithErrorCode(ErrorCode.MES10103);
            RuleFor(x => x.SiteId).NotEmpty().WithErrorCode(ErrorCode.MES10103);
            RuleFor(x => x.ProcedureId).NotEmpty().WithErrorCode(ErrorCode.MES10103);
            RuleFor(x => x.ResourceId).NotEmpty().WithErrorCode(ErrorCode.MES10103);
            RuleFor(x => x.SFCs).NotEmpty().WithErrorCode(ErrorCode.MES10103);
        }
    }
}
