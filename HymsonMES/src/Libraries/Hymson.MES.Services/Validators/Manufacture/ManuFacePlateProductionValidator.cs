using FluentValidation;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Validators.Manufacture
{
    internal class ManuFacePlateProductionValidator
    {
        /// <summary>
        /// 新增操作面板验证
        /// </summary>
        internal class ManuFacePlateProductionCreateValidator : AbstractValidator<ManuFacePlateProductionCreateDto>
        {
            public ManuFacePlateProductionCreateValidator()
            {
                //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
                //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
            }
        }

        /// <summary>
        /// 新增操作面板验证
        /// </summary>
        internal class ManuFacePlateProductionModifyValidator : AbstractValidator<ManuFacePlateProductionModifyDto>
        {
            public ManuFacePlateProductionModifyValidator()
            {
                //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
                //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
            }
        }
    }
}
