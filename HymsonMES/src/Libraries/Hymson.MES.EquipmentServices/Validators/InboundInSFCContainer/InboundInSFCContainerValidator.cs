using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.EquipmentServices.Dtos.InboundInSFCContainer;
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
    internal class InboundInSFCContainerValidator : AbstractValidator<InboundInSFCContainerDto>
    {
        public InboundInSFCContainerValidator()
        {
            //RuleFor(x => x.EquipmentCode).NotEmpty().WithErrorCode(ErrorCode.MES19001);
            //RuleFor(x => x.ResourceCode).NotEmpty().WithErrorCode(ErrorCode.MES19002);
            RuleFor(x => x.SFC).NotEmpty().WithErrorCode(ErrorCode.MES19003);
            RuleFor(x => x.ContainerCode).NotEmpty().WithErrorCode(ErrorCode.MES19111);
        }
    }
}
