﻿using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.EquipmentServices.Dtos.BindContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Validators.BindContainer
{
    internal class UnBindContainerValidator : AbstractValidator<UnBindContainerDto>
    {
        /// <summary>
        /// 容器绑定验证
        /// </summary>
        public UnBindContainerValidator()
        {
            RuleFor(x => x.ResourceCode).NotEmpty().WithErrorCode(ErrorCode.MES19002);
            RuleFor(x => x.ContainerCode).NotEmpty().WithErrorCode(ErrorCode.MES19102);
            //每个条码都不允许为空
            RuleFor(x => x.ContainerSFCs).Must(list =>
                list.Where(c => !string.IsNullOrEmpty(c.Trim())).Any()).WithErrorCode(ErrorCode.MES19003);
            //容器条码列表不能为空
            RuleFor(x => x.ContainerSFCs).NotEmpty().Must(list => list.Length > 0).WithErrorCode(ErrorCode.MES19103);
            //条码不允许重复
            RuleFor(x => x.ContainerSFCs).Must(list => list.GroupBy(sfc => sfc.Trim()).Where(sfc => sfc.Count() < 2).Any()).WithErrorCode(ErrorCode.MES19007);
        }
    }
}
