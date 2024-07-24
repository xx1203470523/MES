using FluentValidation;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Validators.Manufacture
{
    /// <summary>
    /// 转子装箱记录表 验证
    /// </summary>
    internal class ManuRotorPackListSaveValidator: AbstractValidator<ManuRotorPackListSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public ManuRotorPackListSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
