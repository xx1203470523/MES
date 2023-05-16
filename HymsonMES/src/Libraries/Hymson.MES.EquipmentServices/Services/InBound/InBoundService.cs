using FluentValidation;
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
    /// 进站服务
    /// </summary>
    public class InBoundService : IInBoundService
    {
        private readonly ICurrentEquipment _currentEquipment;
        private readonly AbstractValidator<InBoundRequest> _validationInBoundRequestRules;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationInBoundRequestRules"></param>
        /// <param name="currentEquipment"></param>
        public InBoundService(AbstractValidator<InBoundRequest> validationInBoundRequestRules, ICurrentEquipment currentEquipment)
        {
            _validationInBoundRequestRules = validationInBoundRequestRules;
            _currentEquipment = currentEquipment;
        }

        /// <summary>
        /// 进站
        /// </summary>
        /// <param name="inBoundRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task InBound(InBoundRequest inBoundRequest)
        {
            await _validationInBoundRequestRules.ValidateAndThrowAsync(inBoundRequest);
            throw new NotImplementedException();
        }
    }
}
