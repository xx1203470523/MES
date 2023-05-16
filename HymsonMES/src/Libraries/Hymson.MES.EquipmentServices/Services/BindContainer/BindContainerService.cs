using FluentValidation;
using Hymson.MES.EquipmentServices.Request.BindContainer;
using Hymson.Web.Framework.WorkContext;

namespace Hymson.MES.EquipmentServices.Services.BindContainer
{
    /// <summary>
    /// 容器绑定服务
    /// </summary>
    public class BindContainerService : IBindContainerService
    {
        private readonly ICurrentEquipment _currentEquipment;
        private readonly AbstractValidator<BindContainerRequest> _validationBindContainerRules;
        private readonly AbstractValidator<UnBindContainerRequest> _validationUnBindContainerRules;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationBindContainerRules"></param>
        /// <param name="currentEquipment"></param>
        public BindContainerService(AbstractValidator<BindContainerRequest> validationBindContainerRules, ICurrentEquipment currentEquipment, AbstractValidator<UnBindContainerRequest> validationUnBindContainerRules)
        {
            _validationBindContainerRules = validationBindContainerRules;
            _currentEquipment = currentEquipment;
            _validationUnBindContainerRules = validationUnBindContainerRules;
        }
        /// <summary>
        /// 绑定
        /// </summary>
        /// <param name="bindContainerRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task BindContainerAsync(BindContainerRequest bindContainerRequest)
        {
            await _validationBindContainerRules.ValidateAndThrowAsync(bindContainerRequest);
            throw new NotImplementedException();
        }

        /// <summary>
        /// 解绑
        /// </summary>
        /// <param name="unBindContainerRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task UnBindContainerAsync(UnBindContainerRequest unBindContainerRequest)
        {
            await _validationUnBindContainerRules.ValidateAndThrowAsync(unBindContainerRequest);
            throw new NotImplementedException();
        }
    }
}
