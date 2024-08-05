using FluentValidation;
using Hymson.MES.Services.Dtos.NioPushCollection;

namespace Hymson.MES.Services.Validators.NioPushCollection
{
    /// <summary>
    /// NIO推送参数 验证
    /// </summary>
    internal class NioPushCollectionSaveValidator: AbstractValidator<NioPushCollectionSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public NioPushCollectionSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
