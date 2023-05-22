using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.EquipmentServices.Dtos.BindSFC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Validators.BindSFC
{
    internal class UnBindSFCValidator : AbstractValidator<UnBindSFCDto>
    {
        /// <summary>
        /// 条码解绑验证
        /// </summary>
        public UnBindSFCValidator()
        {
            RuleFor(x => x.ResourceCode).NotEmpty().WithErrorCode(ErrorCode.MES19002);
            RuleFor(x => x.SFC).NotEmpty().WithErrorCode(ErrorCode.MES19003);
            RuleFor(x => x.BindSFCs).NotEmpty().Must(list => list.Length > 0).WithErrorCode(ErrorCode.MES19101);
            //每个条码都不允许为空
            RuleFor(x => x.BindSFCs).Must(list =>
                list.Where(c => !string.IsNullOrEmpty(c.Trim())).Any()).WithErrorCode(ErrorCode.MES19003);
            //条码不允许重复
            RuleFor(x => x.BindSFCs).Must(list => list.GroupBy(sfc => sfc.Trim()).Where(sfc => sfc.Count() < 2).Any()).WithErrorCode(ErrorCode.MES19007);
        }
    }
}
