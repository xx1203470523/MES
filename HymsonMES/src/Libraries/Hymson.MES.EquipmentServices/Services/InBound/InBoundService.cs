using FluentValidation;
using Hymson.MES.EquipmentServices.Dtos.InBound;
using Hymson.Web.Framework.WorkContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Services.InBound
{
    /// <summary>
    /// 进站服务
    /// </summary>
    public class InBoundService : IInBoundService
    {
        private readonly ICurrentEquipment _currentEquipment;
        private readonly AbstractValidator<InBoundDto> _validationInBoundDtoRules;
        private readonly AbstractValidator<InBoundMoreDto> _validationInBoundMoreDtoRules;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationInBoundDtoRules"></param>
        /// <param name="currentEquipment"></param>
        /// <param name="validationInBoundMoreDtoRules"></param>
        public InBoundService(AbstractValidator<InBoundDto> validationInBoundDtoRules, ICurrentEquipment currentEquipment, AbstractValidator<InBoundMoreDto> validationInBoundMoreDtoRules)
        {
            _validationInBoundDtoRules = validationInBoundDtoRules;
            _currentEquipment = currentEquipment;
            _validationInBoundMoreDtoRules = validationInBoundMoreDtoRules;
        }

        /// <summary>
        /// 进站
        /// </summary>
        /// <param name="inBoundDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task InBound(InBoundDto inBoundDto)
        {
            await _validationInBoundDtoRules.ValidateAndThrowAsync(inBoundDto);
            throw new NotImplementedException();
        }

        /// <summary>
        /// 进站（多个）
        /// </summary>
        /// <param name="inBoundMoreDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task InBoundMore(InBoundMoreDto inBoundMoreDto)
        {
            await _validationInBoundMoreDtoRules.ValidateAndThrowAsync(inBoundMoreDto);
            throw new NotImplementedException();
        }
    }
}
