using FluentValidation;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Validators.Equipment
{
    /// <summary>
    /// 点检记录表 验证
    /// </summary>
    internal class EquInspectionRecordSaveValidator: AbstractValidator<EquInspectionRecordSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public EquInspectionRecordSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
