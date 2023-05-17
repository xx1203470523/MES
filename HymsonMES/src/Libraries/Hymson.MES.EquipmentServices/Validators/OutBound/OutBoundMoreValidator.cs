﻿using FluentValidation;
using Hymson.MES.EquipmentServices.Dtos.OutBound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Validators.OutBound
{
    /// <summary>
    /// 出站验证(多个)
    /// </summary>
    internal class OutBoundMoreValidator : AbstractValidator<OutBoundMoreDto>
    {
        public OutBoundMoreValidator()
        {

        }
    }
}
