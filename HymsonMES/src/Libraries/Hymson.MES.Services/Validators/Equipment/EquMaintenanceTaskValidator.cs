using FluentValidation;
using Hymson.MES.Services.Dtos.Equipment.EquMaintenance;

namespace Hymson.MES.Services.Validators.Equipment
{
    /// <summary>
    /// 设备保养任务 验证
    /// </summary>
    internal class EquMaintenanceTaskSaveValidator: AbstractValidator<EquMaintenanceTaskSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public EquMaintenanceTaskSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
