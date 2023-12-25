/*
 *creator: Karl
 *
 *describe: 发布记录表    验证规则 | 代码由框架生成  
 *builder:  pengxin
 *build datetime: 2023-12-19 10:03:09
 */
using FluentValidation;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Validators.Integrated
{
    /// <summary>
    /// 发布记录表 更新 验证
    /// </summary>
    internal class SysReleaseRecordCreateValidator: AbstractValidator<SysReleaseRecordCreateDto>
    {
        public SysReleaseRecordCreateValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

    /// <summary>
    /// 发布记录表 修改 验证
    /// </summary>
    internal class SysReleaseRecordModifyValidator : AbstractValidator<SysReleaseRecordModifyDto>
    {
        public SysReleaseRecordModifyValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }
}
