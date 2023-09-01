/*
 *creator: Karl
 *
 *describe: 烘烤工序    验证规则 | 代码由框架生成  
 *builder:  wxk
 *build datetime: 2023-07-28 05:41:13
 */
using FluentValidation;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Validators.Manufacture
{
    /// <summary>
    /// 烘烤工序 更新 验证
    /// </summary>
    internal class ManuBakingCreateValidator: AbstractValidator<ManuBakingCreateDto>
    {
        public ManuBakingCreateValidator()
        {

        }
    }

    /// <summary>
    /// 烘烤工序 修改 验证
    /// </summary>
    internal class ManuBakingModifyValidator : AbstractValidator<ManuBakingModifyDto>
    {
        public ManuBakingModifyValidator()
        {
 
        }
    }
}
