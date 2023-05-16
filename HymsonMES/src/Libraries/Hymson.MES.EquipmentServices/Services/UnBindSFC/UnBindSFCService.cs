using FluentValidation;
using Hymson.MES.EquipmentServices.Request.UnBindSFC;
using Hymson.Web.Framework.WorkContext;

namespace Hymson.MES.EquipmentServices.Services.BindSFC
{
    /// <summary>
    /// 条码解绑服务
    /// </summary>
    public class UnBindSFCService : IUnBindSFCService
    {
        private readonly ICurrentEquipment _currentEquipment;
        private readonly AbstractValidator<UnBindSFCRequest> _validationUnBindRequestRules;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationUnBindRequestRules"></param>
        /// <param name="currentEquipment"></param>
        public UnBindSFCService(AbstractValidator<UnBindSFCRequest> validationUnBindRequestRules, ICurrentEquipment currentEquipment)
        {
            _validationUnBindRequestRules = validationUnBindRequestRules;
            _currentEquipment = currentEquipment;
        }
        /// <summary>
        /// 解绑
        /// </summary>
        /// <param name="unBindSFCRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task UnBindSFCAsync(UnBindSFCRequest unBindSFCRequest)
        {
            //验证参数
            await _validationUnBindRequestRules.ValidateAndThrowAsync(unBindSFCRequest);
            throw new NotImplementedException();
        }
    }
}
