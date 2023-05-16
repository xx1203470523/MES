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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationBindRequestRules"></param>
        /// <param name="currentEquipment"></param>
        public BindSFCService(AbstractValidator<BindSFCRequest> validationBindRequestRules, ICurrentEquipment currentEquipment)
        {
            _validationBindRequestRules = validationBindRequestRules;
            _currentEquipment = currentEquipment;
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
    }
}
