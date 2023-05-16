using FluentValidation;
using Hymson.MES.EquipmentServices.Request.OutBound;

namespace Hymson.MES.EquipmentServices.Validators.OutBound
{
    /// <summary>
    /// 出站验证
    /// </summary>
    internal class OutBoundValidator : AbstractValidator<OutBoundRequest>
    {
        public OutBoundValidator() { 
        
        }
    }
}
