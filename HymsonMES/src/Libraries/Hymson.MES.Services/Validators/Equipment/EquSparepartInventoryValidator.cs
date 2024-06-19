/*
 *creator: Karl
 *
 *describe: 备件库存    验证规则 | 代码由框架生成  
 *builder:  pengxin
 *build datetime: 2024-06-12 10:15:26
 */
using FluentValidation;
using Hymson.MES.Services.Dtos.EquSparepartInventory;

namespace Hymson.MES.Services.Validators.EquSparepartInventory
{
    /// <summary>
    /// 备件库存 更新 验证
    /// </summary>
    internal class EquSparepartInventoryCreateValidator: AbstractValidator<EquSparepartInventoryCreateDto>
    {
        public EquSparepartInventoryCreateValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

    /// <summary>
    /// 备件库存 修改 验证
    /// </summary>
    internal class EquSparepartInventoryModifyValidator : AbstractValidator<EquSparepartInventoryModifyDto>
    {
        public EquSparepartInventoryModifyValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }
}
