using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Validators.Integrated
{
    /// <summary>
    /// 托盘信息 更新 验证
    /// </summary>
    internal class InteTraySaveValidator: AbstractValidator<InteTraySaveDto>
    {
        public InteTraySaveValidator()
        {
            RuleFor(x => x.Code).NotEmpty().WithErrorCode(ErrorCode.MES10901);
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(ErrorCode.MES10902);
        }
    }

}
