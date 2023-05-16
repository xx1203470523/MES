using FluentValidation;
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
        }
    }
}
