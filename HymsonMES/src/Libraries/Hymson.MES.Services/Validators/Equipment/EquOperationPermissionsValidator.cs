using FluentValidation;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Validators.Equipment
{
    /// <summary>
    /// 设备点检保养项目 验证
    /// </summary>
    internal class EquOperationPermissionsSaveValidator : AbstractValidator<EquOperationPermissionsSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public EquOperationPermissionsSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
