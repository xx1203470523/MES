/*
 *creator: Karl
 *
 *describe: 设备维修记录    验证规则 | 代码由框架生成  
 *builder:  pengxin
 *build datetime: 2024-06-12 10:56:10
 */
using FluentValidation;
using Hymson.MES.Services.Dtos.EquRepairOrder;

namespace Hymson.MES.Services.Validators.EquRepairOrder
{
    /// <summary>
    /// 设备维修记录 更新 验证
    /// </summary>
    internal class EquRepairOrderCreateValidator: AbstractValidator<EquRepairOrderCreateDto>
    {
        public EquRepairOrderCreateValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

    /// <summary>
    /// 设备维修记录 修改 验证
    /// </summary>
    internal class EquRepairOrderModifyValidator : AbstractValidator<EquRepairOrderModifyDto>
    {
        public EquRepairOrderModifyValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }
}
