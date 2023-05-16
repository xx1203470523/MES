using FluentValidation;
using Hymson.MES.EquipmentServices.Request.UnBindSFC;

namespace Hymson.MES.EquipmentServices.Validators.UnBindSFC
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
