using FluentValidation;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.EquipmentServices.Request.InboundInContainer;
using Hymson.MES.EquipmentServices.Request.QueryContainerBindSfc;
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
        private readonly AbstractValidator<InboundInContainerRequest> _validationInboundInContainerRequestRules;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationInboundInContainerRequestRules"></param>
        /// <param name="currentEquipment"></param>
        public InboundInContainerService(IManuTraySfcRelationRepository manuTraySfcRelationRepository, AbstractValidator<InboundInContainerRequest> validationInboundInContainerRequestRules, ICurrentEquipment currentEquipment)
        {
            _manuTraySfcRelationRepository = manuTraySfcRelationRepository;
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
            var manuTraySfcRelationEntits = await _manuTraySfcRelationRepository.GetManuTraySfcRelationByTrayCodeAsync(new ManuTraySfcRelationByTrayCodeQuery { TrayCode = InboundInContainerRequest.ContainerCode, SiteId = _currentEquipment.SiteId });
            List<string> list = new List<string>();
            foreach (var item in manuTraySfcRelationEntits)
            {
                list.Add(item.SFC);
            }
            //进攻   冲~
            throw new NotImplementedException();
        }
    }
}
