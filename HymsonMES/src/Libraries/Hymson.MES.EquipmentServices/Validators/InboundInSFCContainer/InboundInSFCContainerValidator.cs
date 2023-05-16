using FluentValidation;
using Hymson.MES.EquipmentServices.Request.InboundInSFCContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Validators.InboundInSFCContainer
{
    /// <summary>
    /// 进站-电芯和托盘-装盘2验证
    /// </summary>
    internal class InboundInSFCContainerValidator : AbstractValidator<InboundInSFCContainerRequest>
    {
        public InboundInSFCContainerValidator()
        {

        }
    }
}
