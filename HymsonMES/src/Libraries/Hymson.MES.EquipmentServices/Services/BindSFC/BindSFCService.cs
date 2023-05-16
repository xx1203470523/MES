using FluentValidation;
using Hymson.MES.EquipmentServices.Request.BindSFC;
using Hymson.Web.Framework.WorkContext;

namespace Hymson.MES.EquipmentServices.Services.BindSFC
{
    /// <summary>
    /// 条码绑定服务
    /// </summary>
    public class BindSFCService : IBindSFCService
    {
        private readonly ICurrentEquipment _currentEquipment;
        private readonly AbstractValidator<BindSFCRequest> _validationBindRequestRules;
        private readonly AbstractValidator<UnBindSFCRequest> _validationUnBindRequestRules;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationBindRequestRules"></param>
        /// <param name="currentEquipment"></param>
        /// <param name="validationUnBindRequestRules"></param>
        public BindSFCService(AbstractValidator<BindSFCRequest> validationBindRequestRules, ICurrentEquipment currentEquipment, AbstractValidator<UnBindSFCRequest> validationUnBindRequestRules)
        {
            _validationBindRequestRules = validationBindRequestRules;
            _currentEquipment = currentEquipment;
            _validationUnBindRequestRules = validationUnBindRequestRules;
        }

        /// <summary>
        /// 绑定
        /// </summary>
        /// <param name="bindSFCRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task BindSFCAsync(BindSFCRequest bindSFCRequest)
        {
            //验证参数
            await _validationBindRequestRules.ValidateAndThrowAsync(bindSFCRequest);
            throw new NotImplementedException();
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
