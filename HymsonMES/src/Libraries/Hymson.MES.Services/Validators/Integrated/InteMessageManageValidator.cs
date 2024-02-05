using FluentValidation;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Validators.Integrated
{
    /// <summary>
    /// 消息管理 验证
    /// </summary>
    internal class InteMessageManageSaveValidator: AbstractValidator<InteMessageManageTriggerSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public InteMessageManageSaveValidator()
        {
            
        }
    }

}
