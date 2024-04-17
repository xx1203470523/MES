using FluentValidation;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Validators.Equipment
{
    /// <summary>
    /// 设备点检保养项目 验证
    /// </summary>
    internal class EquInspectionItemSaveValidator: AbstractValidator<EquInspectionItemSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public EquInspectionItemSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
