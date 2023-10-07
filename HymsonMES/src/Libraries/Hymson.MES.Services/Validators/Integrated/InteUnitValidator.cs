using FluentValidation;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Validators.Integrated
{
    /// <summary>
    /// 单位维护 验证
    /// </summary>
    internal class InteUnitSaveValidator: AbstractValidator<InteUnitSaveDto>
    {
        public InteUnitSaveValidator()
        {
        }
    }

}
