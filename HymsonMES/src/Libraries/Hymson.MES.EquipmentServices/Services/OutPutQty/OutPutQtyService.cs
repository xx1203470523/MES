using FluentValidation;
using Hymson.MES.EquipmentServices.Dtos.OutPutQty;
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
        private readonly AbstractValidator<OutPutQtyDto> _validationOutPutQtyDtoRules;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationOutPutQtyDtoRules"></param>
        /// <param name="currentEquipment"></param>
        public OutPutQtyService(AbstractValidator<OutPutQtyDto> validationOutPutQtyDtoRules, ICurrentEquipment currentEquipment)
        {
            _validationOutPutQtyDtoRules = validationOutPutQtyDtoRules;
            _currentEquipment = currentEquipment;
        }

        /// <summary>
        /// 产出上报数量
        /// </summary>
        /// <param name="outPutQtyDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task OutPutQtyAsync(OutPutQtyDto outPutQtyDto)
        {
            await _validationOutPutQtyDtoRules.ValidateAndThrowAsync(outPutQtyDto);
            throw new NotImplementedException();
        }
    }
}
