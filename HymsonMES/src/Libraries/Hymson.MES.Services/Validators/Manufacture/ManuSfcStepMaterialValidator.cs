/*
 *creator: Karl
 *
 *describe: 出站绑定的物料批次条码    验证规则 | 代码由框架生成  
 *builder:  chenjianxiong
 *build datetime: 2023-05-25 08:58:04
 */
using FluentValidation;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Validators.Manufacture
{
    /// <summary>
    /// 出站绑定的物料批次条码 更新 验证
    /// </summary>
    internal class ManuSfcStepMaterialCreateValidator: AbstractValidator<ManuSfcStepMaterialCreateDto>
    {
        public ManuSfcStepMaterialCreateValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

    /// <summary>
    /// 出站绑定的物料批次条码 修改 验证
    /// </summary>
    internal class ManuSfcStepMaterialModifyValidator : AbstractValidator<ManuSfcStepMaterialModifyDto>
    {
        public ManuSfcStepMaterialModifyValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }
}
