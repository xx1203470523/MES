/*
 *creator: Karl
 *
 *describe: 烘烤执行表    验证规则 | 代码由框架生成  
 *builder:  wxk
 *build datetime: 2023-07-28 05:42:41
 */
using FluentValidation;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Validators.Manufacture
{
    /// <summary>
    /// 烘烤执行表 更新 验证
    /// </summary>
    internal class ManuBakingRecordCreateValidator: AbstractValidator<ManuBakingRecordCreateDto>
    {
        public ManuBakingRecordCreateValidator()
        {

        }
    }

    /// <summary>
    /// 烘烤执行表 修改 验证
    /// </summary>
    internal class ManuBakingRecordModifyValidator : AbstractValidator<ManuBakingRecordModifyDto>
    {
        public ManuBakingRecordModifyValidator()
        {

        }
    }
}
