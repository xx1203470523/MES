/*
 *creator: Karl
 *
 *describe: 产品不良录入    验证规则 | 代码由框架生成  
 *builder:  zhaoqing
 *build datetime: 2023-03-27 03:49:17
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
    /// 产品不良录入 更新 验证
    /// </summary>
    internal class ManuProductBadRecordCreateValidator: AbstractValidator<ManuProductBadRecordCreateDto>
    {
        public ManuProductBadRecordCreateValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11").WithMessage("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111").WithMessage("111");
        }
    }

    /// <summary>
    /// 产品不良录入 修改 验证
    /// </summary>
    internal class ManuProductBadRecordModifyValidator : AbstractValidator<ManuProductBadRecordModifyDto>
    {
        public ManuProductBadRecordModifyValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11").WithMessage("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111").WithMessage("111");
        }
    }
}
