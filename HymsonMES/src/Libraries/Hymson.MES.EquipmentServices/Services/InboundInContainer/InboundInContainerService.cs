using FluentValidation;
using Hymson.MES.Data.Repositories.Manufacture;
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
        private readonly IManuTraySfcRelationRepository _manuTraySfcRelationRepository;

        private readonly AbstractValidator<InboundInContainerDto> _validationInboundInContainerDtoRules;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationInboundInContainerDtoRules"></param>
        /// <param name="currentEquipment"></param>
        public InboundInContainerService(IManuTraySfcRelationRepository manuTraySfcRelationRepository, AbstractValidator<InboundInContainerDto> validationInboundInContainerDtoRules, ICurrentEquipment currentEquipment)
        {
            _manuTraySfcRelationRepository = manuTraySfcRelationRepository;
            _validationInboundInContainerDtoRules = validationInboundInContainerDtoRules;
            _currentEquipment = currentEquipment;
        }

        /// <summary>
        /// 进站-容器
        /// </summary>
        /// <param name="inboundInContainerDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task InboundInContainerAsync(InboundInContainerDto inboundInContainerDto)
        {
            await _validationInboundInContainerDtoRules.ValidateAndThrowAsync(inboundInContainerDto);
            var manuTraySfcRelationEntits = await _manuTraySfcRelationRepository.GetManuTraySfcRelationByTrayCodeAsync(new ManuTraySfcRelationByTrayCodeQuery { TrayCode = inboundInContainerDto.ContainerCode, SiteId = _currentEquipment.SiteId });
            List<string> list = new List<string>();
            foreach (var item in manuTraySfcRelationEntits)
            {
                list.Add(item.SFC);
            }
            // 进站
        }
    }
}
