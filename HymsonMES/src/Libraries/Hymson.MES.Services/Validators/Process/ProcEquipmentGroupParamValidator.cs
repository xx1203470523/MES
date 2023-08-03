/*
 *creator: Karl
 *
 *describe: 设备参数组    验证规则 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-08-02 01:48:35
 */
using FluentValidation;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.Services.Validators.Process
{
    /// <summary>
    /// 设备参数组 更新 验证
    /// </summary>
    internal class ProcEquipmentGroupParamCreateValidator: AbstractValidator<ProcEquipmentGroupParamCreateDto>
    {
        public ProcEquipmentGroupParamCreateValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

    /// <summary>
    /// 设备参数组 修改 验证
    /// </summary>
    internal class ProcEquipmentGroupParamModifyValidator : AbstractValidator<ProcEquipmentGroupParamModifyDto>
    {
        public ProcEquipmentGroupParamModifyValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }
}
