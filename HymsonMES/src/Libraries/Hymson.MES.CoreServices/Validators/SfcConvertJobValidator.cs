using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.CoreServices.Bos.Job;

namespace Hymson.MES.CoreServices.Validators
{
    /// <summary>
    /// 条码转换 验证
    /// </summary>
    internal class SfcConvertJobValidator : AbstractValidator<SfcConvertRequestBo>
    {
        /// <summary>
        ///  
        /// </summary>
        public SfcConvertJobValidator()
        {
            RuleFor(x => x).NotEmpty().WithErrorCode(ErrorCode.MES16332);
            RuleFor(x => x.SFCs).NotEmpty().Must(it => it.Any(s => s.Trim() != "")).WithErrorCode(ErrorCode.MES16332);
            RuleFor(x => x.SiteId).NotEmpty().WithErrorCode(ErrorCode.MES10112);
            RuleFor(x => x.EquipmentId).NotEmpty().WithErrorCode(ErrorCode.MES16339);
            RuleFor(x => x.ProcedureId).NotEmpty().WithErrorCode(ErrorCode.MES16335);
            RuleFor(x => x.ResourceId).NotEmpty().WithErrorCode(ErrorCode.MES16334);
        }
    }
}
