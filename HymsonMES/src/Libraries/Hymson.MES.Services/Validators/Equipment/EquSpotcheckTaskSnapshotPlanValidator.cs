using FluentValidation;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Validators.Equipment
{
    /// <summary>
    /// 设备点检快照任务计划 验证
    /// </summary>
    internal class EquSpotcheckTaskSnapshotPlanSaveValidator: AbstractValidator<EquSpotcheckTaskSnapshotPlanSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public EquSpotcheckTaskSnapshotPlanSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
