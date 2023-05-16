using FluentValidation;
using Hymson.MES.EquipmentServices.Request.InboundInSFCContainer;
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
        private readonly AbstractValidator<InboundInSFCContainerRequest> _validationInboundInSFCContainerRequestRules;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationInboundInSFCContainerRequestRules"></param>
        /// <param name="currentEquipment"></param>
        public InboundInSFCContainerService(AbstractValidator<InboundInSFCContainerRequest> validationInboundInSFCContainerRequestRules, ICurrentEquipment currentEquipment)
        {
            _validationInboundInSFCContainerRequestRules = validationInboundInSFCContainerRequestRules;
            _currentEquipment = currentEquipment;
        }

        /// <summary>
        /// 进站-电芯和托盘-装盘2
        /// </summary>
        /// <param name="inboundInSFCContainerRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task InboundInSFCContainerAsync(InboundInSFCContainerRequest inboundInSFCContainerRequest)
        {
            await _validationInboundInSFCContainerRequestRules.ValidateAndThrowAsync(inboundInSFCContainerRequest);
            throw new NotImplementedException();
        }
    }
}
