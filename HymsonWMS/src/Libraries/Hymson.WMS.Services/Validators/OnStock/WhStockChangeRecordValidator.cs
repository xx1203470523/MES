using FluentValidation;
using Hymson.MES.Services.Dtos.OnStock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Validators.OnStock
{
    internal class WhStockChangeRecordValidator: AbstractValidator<WhStockChangeRecordDto>
    {
        public WhStockChangeRecordValidator()
        {
            RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11").WithMessage("11");
            RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111").WithMessage("111");
        }
    }
}
