using FluentValidation;
using Hymson.MES.EquipmentServices.Dtos.InboundInSFCContainer;
using Hymson.Web.Framework.WorkContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Services.InboundInSFCContainer
{
    /// <summary>
    /// 进站-电芯和托盘-装盘2服务
    /// </summary>
    public class InboundInSFCContainerService : IInboundInSFCContainerService
    {
        private readonly ICurrentEquipment _currentEquipment;
        private readonly AbstractValidator<InboundInSFCContainerDto> _validationInboundInSFCContainerDtoRules;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationInboundInSFCContainerDtoRules"></param>
        /// <param name="currentEquipment"></param>
        public InboundInSFCContainerService(AbstractValidator<InboundInSFCContainerDto> validationInboundInSFCContainerDtoRules, ICurrentEquipment currentEquipment)
        {
            _validationInboundInSFCContainerDtoRules = validationInboundInSFCContainerDtoRules;
            _currentEquipment = currentEquipment;
        }

        /// <summary>
        /// 进站-电芯和托盘-装盘2
        /// </summary>
        /// <param name="inboundInSFCContainerDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task InboundInSFCContainerAsync(InboundInSFCContainerDto inboundInSFCContainerDto)
        {
            await _validationInboundInSFCContainerDtoRules.ValidateAndThrowAsync(inboundInSFCContainerDto);
            throw new NotImplementedException();
        }
    }
}
