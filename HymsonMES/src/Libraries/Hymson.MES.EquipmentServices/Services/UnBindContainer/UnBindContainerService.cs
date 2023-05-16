using FluentValidation;
using Hymson.MES.EquipmentServices.Request.BindContainer;
using Hymson.MES.EquipmentServices.Request.UnBindContainer;
using Hymson.Web.Framework.WorkContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Services.UnBindContainer
{
    /// <summary>
    /// 容器解绑服务
    /// </summary>
    public class UnBindContainerService : IUnBindContainerService
    {
        private readonly ICurrentEquipment _currentEquipment;
        private readonly AbstractValidator<UnBindContainerRequest> _validationUnBindContainerRules;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationUnBindContainerRules"></param>
        /// <param name="currentEquipment"></param>
        public UnBindContainerService(AbstractValidator<UnBindContainerRequest> validationUnBindContainerRules, ICurrentEquipment currentEquipment)
        {
            _validationUnBindContainerRules = validationUnBindContainerRules;
            _currentEquipment = currentEquipment;
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
