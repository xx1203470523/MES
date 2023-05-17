using FluentValidation;
using Hymson.MES.EquipmentServices.Dtos.OutBound;

namespace Hymson.MES.EquipmentServices.Validators.OutBound
{
    /// <summary>
    /// 出站验证
    /// </summary>
    internal class OutBoundValidator : AbstractValidator<OutBoundDto>
    {
        public OutBoundValidator() { 
        
        }
    }
}
