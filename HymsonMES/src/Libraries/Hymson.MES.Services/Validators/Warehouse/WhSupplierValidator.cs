/*
 *creator: Karl
 *
 *describe: 供应商    验证规则 | 代码由框架生成  
 *builder:  pengxin
 *build datetime: 2023-03-03 01:51:43
 */
using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Dtos.Warehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Validators.Warehouse
{
    /// <summary>
    /// 供应商 更新 验证
    /// </summary>
    internal class WhSupplierCreateValidator : AbstractValidator<WhSupplierCreateDto>
    {
        public WhSupplierCreateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES15007));
            RuleFor(x => x.Name).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES15010));
            RuleFor(x => x.Code).NotEmpty().WithErrorCode(nameof(ErrorCode.MES15006));
            RuleFor(x => x.Code).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES15009));

        }
    }

    /// <summary>
    /// 供应商 修改 验证
    /// </summary>
    internal class WhSupplierModifyValidator : AbstractValidator<WhSupplierModifyDto>
    {
        public WhSupplierModifyValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES15007));
            RuleFor(x => x.Name).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES15010));

        }
    }

    internal class WhSupplierImportValidator : AbstractValidator<WhSupplierImportDto>
    {
        public WhSupplierImportValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES15007));
            RuleFor(x => x.Name).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES15010));
            RuleFor(x => x.Code).NotEmpty().WithErrorCode(nameof(ErrorCode.MES15006));
            RuleFor(x => x.Code).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES15009));
        }
    }
}
