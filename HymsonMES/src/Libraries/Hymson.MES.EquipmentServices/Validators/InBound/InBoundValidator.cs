using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Resource;
using Hymson.MES.EquipmentServices.Dtos.InBound;
using Hymson.Web.Framework.WorkContext;

namespace Hymson.MES.EquipmentServices.Validators.InBound
{
    /// <summary>
    /// 进站验证
    /// </summary>
    public class InBoundValidator : AbstractValidator<InBoundDto>
    {
        private readonly IProcResourceRepository _procResourceRepository;
        private readonly ICurrentEquipment _currentEquipment;
        /// <summary>
        /// 进站必要校验
        /// </summary>
        /// <param name="procResourceRepository"></param>
        /// <param name="currentEquipment"></param>
        public InBoundValidator(IProcResourceRepository procResourceRepository, ICurrentEquipment currentEquipment)
        {
            _procResourceRepository = procResourceRepository;
            _currentEquipment = currentEquipment;

            RuleFor(x => x.SFC).NotEmpty().WithErrorCode(nameof(ErrorCode.MES19003));
            //每个条码都不允许为空
            RuleFor(x => x.SFC).Must(sfc => !string.IsNullOrEmpty(sfc.Trim())).WithErrorCode(ErrorCode.MES19003);
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
