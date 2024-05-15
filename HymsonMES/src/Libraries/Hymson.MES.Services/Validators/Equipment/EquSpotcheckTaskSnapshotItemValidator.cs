using FluentValidation;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Validators.Equipment
{
    /// <summary>
    /// 设备点检快照任务项目 验证
    /// </summary>
    internal class EquSpotcheckTaskSnapshotItemSaveValidator: AbstractValidator<EquSpotcheckTaskSnapshotItemSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public EquSpotcheckTaskSnapshotItemSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
