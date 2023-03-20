/*
 *creator: Karl
 *
 *describe: 条码生产信息（物理删除）    验证规则 | 代码由框架生成  
 *builder:  zhaoqing
 *build datetime: 2023-03-18 05:37:27
 */
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
    /// 条码生产信息（物理删除） 更新 验证
    /// </summary>
    internal class ManuSfcProduceCreateValidator: AbstractValidator<ManuSfcProduceCreateDto>
    {
        public ManuSfcProduceCreateValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11").WithMessage("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111").WithMessage("111");
        }
    }

    /// <summary>
    /// 条码生产信息（物理删除） 修改 验证
    /// </summary>
    internal class ManuSfcProduceModifyValidator : AbstractValidator<ManuSfcProduceModifyDto>
    {
        public ManuSfcProduceModifyValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11").WithMessage("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111").WithMessage("111");
        }
    }
}
