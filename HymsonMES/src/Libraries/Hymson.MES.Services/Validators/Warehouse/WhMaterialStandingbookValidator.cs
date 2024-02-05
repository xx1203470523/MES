/*
 *creator: Karl
 *
 *describe: 物料台账    验证规则 | 代码由框架生成  
 *builder:  pengxin
 *build datetime: 2023-03-13 10:03:29
 */
using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Services.Dtos.Warehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Validators.Warehouse
{
    /// <summary>
    /// 物料台账 更新 验证
    /// </summary>
    internal class WhMaterialStandingbookCreateValidator : AbstractValidator<WhMaterialStandingbookCreateDto>
    {
        public WhMaterialStandingbookCreateValidator()
        {
            RuleFor(x => x.MaterialCode).NotEmpty().WithErrorCode(nameof(ErrorCode.MES15201));
            RuleFor(x => x.MaterialVersion).NotEmpty().WithErrorCode(nameof(ErrorCode.MES15202));
            RuleFor(x => x.MaterialBarCode).NotEmpty().WithErrorCode(nameof(ErrorCode.MES15203));
            RuleFor(x => x.Batch).NotEmpty().WithErrorCode(nameof(ErrorCode.MES15204));
            RuleFor(x => x.Quantity).NotEmpty().WithErrorCode(nameof(ErrorCode.MES15205));
            RuleFor(x => x.Type).NotEmpty().WithErrorCode(nameof(ErrorCode.MES15206));
            RuleFor(x => x.Source).NotEmpty().WithErrorCode(nameof(ErrorCode.MES15208));
        }
    }

    /// <summary>
    /// 物料台账 修改 验证
    /// </summary>
    internal class WhMaterialStandingbookModifyValidator : AbstractValidator<WhMaterialStandingbookModifyDto>
    {
        public WhMaterialStandingbookModifyValidator()
        {

        }
    }
}
