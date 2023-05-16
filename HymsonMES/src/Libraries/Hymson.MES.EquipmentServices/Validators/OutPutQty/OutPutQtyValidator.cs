using FluentValidation;
using Hymson.MES.EquipmentServices.Request.OutPutQty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Validators.OutPutQty
{
    /// <summary>
    ///产出上报数量
    /// </summary>
    internal class OutPutQtyValidator : AbstractValidator<OutPutQtyRequest>
    {
        public OutPutQtyValidator()
        {

        }
    }
}
