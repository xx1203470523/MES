/*
 *creator: Karl
 *
 *describe: 开机参数采集表    验证规则 | 代码由框架生成  
 *builder:  wxk
 *build datetime: 2023-07-12 04:58:46
 */
using FluentValidation;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.Services.Validators.Process
{
    /// <summary>
    /// 开机参数采集表 更新 验证
    /// </summary>
    internal class ProcBootupparamrecordCreateValidator: AbstractValidator<ProcBootupparamrecordCreateDto>
    {
        public ProcBootupparamrecordCreateValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

    /// <summary>
    /// 开机参数采集表 修改 验证
    /// </summary>
    internal class ProcBootupparamrecordModifyValidator : AbstractValidator<ProcBootupparamrecordModifyDto>
    {
        public ProcBootupparamrecordModifyValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }
}
