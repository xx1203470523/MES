using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.EquipmentServices.Request.BindSFC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Validators.BindSFC
{
    internal class UnBindSFCValidator : AbstractValidator<UnBindSFCRequest>
    {
        /// <summary>
        /// 条码解绑验证
        /// </summary>
        public UnBindSFCValidator()
        {
            RuleFor(x => x.ResourceCode).NotEmpty().WithErrorCode(ErrorCode.MES19002);
            RuleFor(x => x.SFC).NotEmpty().WithErrorCode(ErrorCode.MES19003);
            RuleFor(x => x.BindSFCs).NotEmpty().Must(list => list.Length <= 0).WithErrorCode(ErrorCode.MES19101);
        }
    }
}
