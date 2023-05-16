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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationBindContainerRules"></param>
        /// <param name="currentEquipment"></param>
        public BindContainerService(AbstractValidator<BindContainerRequest> validationBindContainerRules, ICurrentEquipment currentEquipment)
        {
            _validationBindContainerRules = validationBindContainerRules;
            _currentEquipment = currentEquipment;
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
    }
}
