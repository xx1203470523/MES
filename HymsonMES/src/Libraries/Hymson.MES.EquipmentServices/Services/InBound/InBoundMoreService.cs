using FluentValidation;
using Hymson.MES.EquipmentServices.Request.BindSFC;
using Hymson.MES.EquipmentServices.Request.InBound;
using Hymson.Web.Framework.WorkContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Services.InBound
{
    /// <summary>
    /// 进站服务（多个）
    /// </summary>
    public class InBoundMoreService : IInBoundMoreService
    {
        private readonly ICurrentEquipment _currentEquipment;
        private readonly AbstractValidator<InBoundMoreRequest> _validationInBoundMoreRequestRules;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationInBoundMoreRequestRules"></param>
        /// <param name="currentEquipment"></param>
        public InBoundMoreService(AbstractValidator<InBoundMoreRequest> validationInBoundMoreRequestRules, ICurrentEquipment currentEquipment)
        {
            _validationInBoundMoreRequestRules = validationInBoundMoreRequestRules;
            _currentEquipment = currentEquipment;
        }

        /// <summary>
        /// 进站（多个）
        /// </summary>
        /// <param name="inBoundMoreRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task InBoundMore(InBoundMoreRequest inBoundMoreRequest)
        {
            await _validationInBoundMoreRequestRules.ValidateAndThrowAsync(inBoundMoreRequest);
            throw new NotImplementedException();
        }
    }
}
