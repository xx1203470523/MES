using FluentValidation;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.Services.Validators.Process
{
    /// <summary>
    /// 工艺设备组 验证
    /// </summary>
    internal class ProcProcessEquipmentGroupSaveValidator: AbstractValidator<ProcProcessEquipmentGroupSaveDto>
    {
        public ProcProcessEquipmentGroupSaveValidator()
        {
        }
    }

}
