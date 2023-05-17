using FluentValidation;
using Hymson.MES.EquipmentServices.Dtos.InboundInContainer;
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
        private readonly AbstractValidator<InboundInContainerDto> _validationInboundInContainerDtoRules;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationInboundInContainerDtoRules"></param>
        /// <param name="currentEquipment"></param>
        public InboundInContainerService(AbstractValidator<InboundInContainerDto> validationInboundInContainerDtoRules, ICurrentEquipment currentEquipment)
        {
            _validationInboundInContainerDtoRules = validationInboundInContainerDtoRules;
            _currentEquipment = currentEquipment;
        }

        /// <summary>
        /// 进站-容器
        /// </summary>
        /// <param name="InboundInContainerDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task InboundInContainerAsync(InboundInContainerDto InboundInContainerDto)
        {
            await _validationInboundInContainerDtoRules.ValidateAndThrowAsync(InboundInContainerDto);
            throw new NotImplementedException();
        }
    }
}
