using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Resource;
using Hymson.MES.EquipmentServices.Dtos.InBound;
using Hymson.Web.Framework.WorkContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Validators.InBound
{
    /// <summary>
    /// 进站验证
    /// </summary>
    internal class InBoundValidator : AbstractValidator<InBoundDto>
    {
        private readonly IProcResourceRepository _procResourceRepository;
        private readonly ICurrentEquipment _currentEquipment;
        public InBoundValidator(IProcResourceRepository procResourceRepository, ICurrentEquipment currentEquipment)
        {
            _procResourceRepository = procResourceRepository;
            _currentEquipment = currentEquipment;

            RuleFor(x => x.SFC).NotEmpty().WithErrorCode(nameof(ErrorCode.MES19003));
            RuleFor(x => x).MustAsync(async (inBoundDto, cancellation) =>
            {
                var query = new ProcResourceQuery
                {
                    SiteId = _currentEquipment.SiteId,
                    ResCode = inBoundDto.ResourceCode.Trim()
                };
                return await _procResourceRepository.IsExistsActiveAsync(query);
            }).WithErrorCode(nameof(ErrorCode.MES19006));
        }
    }
}
