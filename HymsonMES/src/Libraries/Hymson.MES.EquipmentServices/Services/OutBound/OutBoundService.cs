using FluentValidation;
using Hymson.MES.EquipmentServices.Request.InBound;
using Hymson.MES.EquipmentServices.Request.OutBound;
using Hymson.Web.Framework.WorkContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Services.OutBound
{
    /// <summary>
    /// 出站服务
    /// </summary>
    public class OutBoundService : IOutBoundService
    {
        private readonly ICurrentEquipment _currentEquipment;
        private readonly AbstractValidator<OutBoundRequest> _validationOutBoundRequestRules;
        private readonly AbstractValidator<OutBoundMoreRequest> _validationOutBoundMoreRequestRules;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationOutBoundRequestRules"></param>
        /// <param name="currentEquipment"></param>
        /// <param name="validationOutBoundMoreRequestRules"></param>
        public OutBoundService(AbstractValidator<OutBoundRequest> validationOutBoundRequestRules, ICurrentEquipment currentEquipment, AbstractValidator<OutBoundMoreRequest> validationOutBoundMoreRequestRules)
        {
            _validationOutBoundRequestRules = validationOutBoundRequestRules;
            _currentEquipment = currentEquipment;
            _validationOutBoundMoreRequestRules = validationOutBoundMoreRequestRules;
        }

        /// <summary>
        /// 出站
        /// </summary>
        /// <param name="outBoundRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task OutBound(OutBoundRequest outBoundRequest)
        {
            await _validationOutBoundRequestRules.ValidateAndThrowAsync(outBoundRequest);
            throw new NotImplementedException();
        }


        /// <summary>
        /// 出站（多个）
        /// </summary>
        /// <param name="outBoundMoreRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task OutBoundMore(OutBoundMoreRequest outBoundMoreRequest)
        {
            await _validationOutBoundMoreRequestRules.ValidateAndThrowAsync(outBoundMoreRequest);
            throw new NotImplementedException();
        }
    }
}
