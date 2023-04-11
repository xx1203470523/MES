using FluentValidation;
using Hymson.MES.Services.Dtos.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Validators.Manufacture
{
    /// <summary>
    /// 新增操作面板验证
    /// </summary>
    internal class ManuFacePlateContainerPackCreateValidator : AbstractValidator<ManuFacePlateContainerPackCreateDto>
    {
        public ManuFacePlateContainerPackCreateValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

    /// <summary>
    /// 新增操作面板验证
    /// </summary>
    internal class ManuFacePlateContainerPackModifyValidator : AbstractValidator<ManuFacePlateContainerPackModifyDto>
    {
        public ManuFacePlateContainerPackModifyValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }
}
