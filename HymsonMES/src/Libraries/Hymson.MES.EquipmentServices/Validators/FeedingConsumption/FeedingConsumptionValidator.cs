using FluentValidation;
using Hymson.MES.EquipmentServices.Request.FeedingConsumption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Validators.FeedingConsumption
{
    /// <summary>
    ///上报物料消耗验证
    /// </summary>
    internal class FeedingConsumptionValidator : AbstractValidator<FeedingConsumptionRequest>
    {
        public FeedingConsumptionValidator()
        {

        }
    }
}
