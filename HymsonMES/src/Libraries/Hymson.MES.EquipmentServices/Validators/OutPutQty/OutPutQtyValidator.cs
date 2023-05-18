using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.EquipmentServices.Dtos.OutPutQty;
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
    internal class OutPutQtyValidator : AbstractValidator<OutPutQtyDto>
    {
        public OutPutQtyValidator()
        {
            //RuleFor(x => x.EquipmentCode).NotEmpty().WithErrorCode(ErrorCode.MES19001);
            //RuleFor(x => x.ResourceCode).NotEmpty().WithErrorCode(ErrorCode.MES19002);
            RuleFor(x => x.SFC).NotEmpty().WithErrorCode(ErrorCode.MES19003);
        }
    }
}
