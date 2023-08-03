/*
 *creator: Karl
 *
 *describe: 设备参数组详情    验证规则 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-08-02 02:08:48
 */
using FluentValidation;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.Services.Validators.Process
{
    /// <summary>
    /// 设备参数组详情 更新 验证
    /// </summary>
    internal class ProcEquipmentGroupParamDetailCreateValidator: AbstractValidator<ProcEquipmentGroupParamDetailCreateDto>
    {
        public ProcEquipmentGroupParamDetailCreateValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

    
}
