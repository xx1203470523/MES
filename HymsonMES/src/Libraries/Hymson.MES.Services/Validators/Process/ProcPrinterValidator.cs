/*
 *creator: Karl
 *
 *describe: 资源配置打印机表    验证规则 | 代码由框架生成  
 *builder:  wxk
 *build datetime: 2023-04-12 08:10:40
 */
using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.Services.Validators.Process
{
    /// <summary>
    /// 资源配置打印机表 更新 验证
    /// </summary>
    internal class ProcPrinterCreateValidator: AbstractValidator<ProcPrinterDto>
    {
        public ProcPrinterCreateValidator()
        {
            RuleFor(x => x.PrintName).NotEmpty().WithErrorCode("打印机名称不能为空");
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

    /// <summary>
    /// 资源配置打印机表 修改 验证
    /// </summary>
    internal class ProcPrinterModifyValidator : AbstractValidator<ProcPrinterDto>
    {
        public ProcPrinterModifyValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }
}
