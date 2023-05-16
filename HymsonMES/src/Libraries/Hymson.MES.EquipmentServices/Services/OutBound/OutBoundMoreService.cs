using FluentValidation;
using Hymson.MES.EquipmentServices.Request.BindSFC;
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
    /// 出站服务（多个）
    /// </summary>
    public class OutBoundMoreService : IOutBoundMoreService
    {
        private readonly ICurrentEquipment _currentEquipment;
        private readonly AbstractValidator<OutBoundMoreRequest> _validationOutBoundMoreRequestRules;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationOutBoundMoreRequestRules"></param>
        /// <param name="currentEquipment"></param>
        public OutBoundMoreService(AbstractValidator<OutBoundMoreRequest> validationOutBoundMoreRequestRules, ICurrentEquipment currentEquipment)
        {
            _validationOutBoundMoreRequestRules = validationOutBoundMoreRequestRules;
            _currentEquipment = currentEquipment;
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
