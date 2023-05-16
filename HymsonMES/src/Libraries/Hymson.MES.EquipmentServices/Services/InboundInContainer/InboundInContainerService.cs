using FluentValidation;
using Hymson.MES.EquipmentServices.Request.InboundInContainer;
using Hymson.Web.Framework.WorkContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Services.InboundInContainer
{
    /// <summary>
    /// 进站-容器服务
    /// </summary>
    public class InboundInContainerService : IInboundInContainerService
    {
        private readonly ICurrentEquipment _currentEquipment;
        private readonly AbstractValidator<InboundInContainerRequest> _validationInboundInContainerRequestRules;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationInboundInContainerRequestRules"></param>
        /// <param name="currentEquipment"></param>
        public InboundInContainerService(AbstractValidator<InboundInContainerRequest> validationInboundInContainerRequestRules, ICurrentEquipment currentEquipment)
        {
            _validationInboundInContainerRequestRules = validationInboundInContainerRequestRules;
            _currentEquipment = currentEquipment;
        }

        /// <summary>
        /// 进站-容器
        /// </summary>
        /// <param name="InboundInContainerRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task InboundInContainerAsync(InboundInContainerRequest InboundInContainerRequest)
        {
            await _validationInboundInContainerRequestRules.ValidateAndThrowAsync(InboundInContainerRequest);
            throw new NotImplementedException();
        }
    }
}
