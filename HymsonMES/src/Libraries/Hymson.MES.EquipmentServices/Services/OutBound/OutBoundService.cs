using FluentValidation;
using Hymson.MES.EquipmentServices.Dtos.InBound;
using Hymson.MES.EquipmentServices.Dtos.OutBound;
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
        private readonly AbstractValidator<OutBoundDto> _validationOutBoundDtoRules;
        private readonly AbstractValidator<OutBoundMoreDto> _validationOutBoundMoreDtoRules;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationOutBoundDtoRules"></param>
        /// <param name="currentEquipment"></param>
        /// <param name="validationOutBoundMoreDtoRules"></param>
        public OutBoundService(AbstractValidator<OutBoundDto> validationOutBoundDtoRules, ICurrentEquipment currentEquipment, AbstractValidator<OutBoundMoreDto> validationOutBoundMoreDtoRules)
        {
            _validationOutBoundDtoRules = validationOutBoundDtoRules;
            _currentEquipment = currentEquipment;
            _validationOutBoundMoreDtoRules = validationOutBoundMoreDtoRules;
        }

        /// <summary>
        /// 出站
        /// </summary>
        /// <param name="outBoundDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task OutBound(OutBoundDto outBoundDto)
        {
            await _validationOutBoundDtoRules.ValidateAndThrowAsync(outBoundDto);
            throw new NotImplementedException();
        }


        /// <summary>
        /// 出站（多个）
        /// </summary>
        /// <param name="outBoundMoreDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task OutBoundMore(OutBoundMoreDto outBoundMoreDto)
        {
            await _validationOutBoundMoreDtoRules.ValidateAndThrowAsync(outBoundMoreDto);
            throw new NotImplementedException();
        }
    }
}
