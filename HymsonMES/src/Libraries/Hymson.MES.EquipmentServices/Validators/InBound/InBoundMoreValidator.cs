using FluentValidation;
using Hymson.MES.EquipmentServices.Dtos.InBound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Validators.InBound
{
    /// <summary>
    /// 进站验证(多个)
    /// </summary>
    internal class InBoundMoreValidator : AbstractValidator<InBoundMoreDto>
    {
        public InBoundMoreValidator()
        {

        }
    }
}
