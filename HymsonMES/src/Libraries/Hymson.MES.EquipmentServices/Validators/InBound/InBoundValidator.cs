using FluentValidation;
using Hymson.MES.EquipmentServices.Request.InBound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Validators.InBound
{
    /// <summary>
    /// 进站验证
    /// </summary>
    internal class InBoundValidator : AbstractValidator<InBoundRequest>
    {
        public InBoundValidator() { 
        
        }
    }

    /// <summary>
    /// 进站验证(多个)
    /// </summary>
    internal class InBoundMoreValidator : AbstractValidator<InBoundMoreRequest>
    {
        public InBoundMoreValidator()
        {

        }
    }
}
