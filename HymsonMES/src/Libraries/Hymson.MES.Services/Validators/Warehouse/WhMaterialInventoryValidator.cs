/*
 *creator: Karl
 *
 *describe: 物料库存    验证规则 | 代码由框架生成  
 *builder:  pengxin
 *build datetime: 2023-03-06 03:27:59
 */
using FluentValidation;
using Hymson.MES.Services.Dtos.Warehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Validators.Warehouse
{
    /// <summary>
    /// 物料库存 更新 验证
    /// </summary>
    internal class WhMaterialInventoryCreateValidator: AbstractValidator<WhMaterialInventoryCreateDto>
    {
        public WhMaterialInventoryCreateValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11").WithMessage("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111").WithMessage("111");
        }
    }

    /// <summary>
    /// 物料库存 修改 验证
    /// </summary>
    internal class WhMaterialInventoryModifyValidator : AbstractValidator<WhMaterialInventoryModifyDto>
    {
        public WhMaterialInventoryModifyValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11").WithMessage("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111").WithMessage("111");
        }
    }
}
