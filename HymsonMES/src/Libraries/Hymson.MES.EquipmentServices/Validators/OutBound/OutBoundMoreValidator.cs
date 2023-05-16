using FluentValidation;
using Hymson.MES.EquipmentServices.Request.OutBound;

namespace Hymson.MES.EquipmentServices.Validators.OutBound
{
    /// <summary>
    /// 出站验证(多个)
    /// </summary>
    internal class OutBoundMoreValidator : AbstractValidator<OutBoundMoreRequest>
    {
        public OutBoundMoreValidator()
        {

        }
    }
}
