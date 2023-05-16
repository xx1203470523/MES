﻿using FluentValidation;
using Hymson.MES.EquipmentServices.Request.InboundInContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Validators.InboundInContainer
{
    /// <summary>
    /// 进站-容器验证
    /// </summary>
    internal class InboundInContainerValidator : AbstractValidator<InboundInContainerRequest>
    {
        public InboundInContainerValidator()
        {

        }
    }
}
