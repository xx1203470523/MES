using FluentValidation;
using Hymson.MES.EquipmentServices.Request.OutPutQty;
using Hymson.Web.Framework.WorkContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Services.OutPutQty
{
    /// <summary>
    /// 产出上报数量服务
    /// </summary>
    public class OutPutQtyService : IOutPutQtyService
    {
        private readonly ICurrentEquipment _currentEquipment;
        private readonly AbstractValidator<OutPutQtyRequest> _validationOutPutQtyRequestRules;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationOutPutQtyRequestRules"></param>
        /// <param name="currentEquipment"></param>
        public OutPutQtyService(AbstractValidator<OutPutQtyRequest> validationOutPutQtyRequestRules, ICurrentEquipment currentEquipment)
        {
            _validationOutPutQtyRequestRules = validationOutPutQtyRequestRules;
            _currentEquipment = currentEquipment;
        }

        /// <summary>
        /// 产出上报数量
        /// </summary>
        /// <param name="outPutQtyRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task OutPutQtyAsync(OutPutQtyRequest outPutQtyRequest)
        {
            await _validationOutPutQtyRequestRules.ValidateAndThrowAsync(outPutQtyRequest);
            throw new NotImplementedException();
        }
    }
}
