using FluentValidation;
using Hymson.MES.Data.Repositories.Process.Resource;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.EquipmentServices.Dtos.OutBound;
using Hymson.Web.Framework.WorkContext;
using Hymson.MES.Core.Constants;

namespace Hymson.MES.EquipmentServices.Validators.OutBound
{
    /// <summary>
    /// 出站验证
    /// </summary>
    public class OutBoundValidator : AbstractValidator<OutBoundDto>
    {
        private readonly IProcResourceRepository _procResourceRepository;
        private readonly ICurrentEquipment _currentEquipment;
        /// <summary>
        /// 出站必要校验
        /// </summary>
        /// <param name="procResourceRepository"></param>
        /// <param name="currentEquipment"></param>
        public OutBoundValidator(IProcResourceRepository procResourceRepository, ICurrentEquipment currentEquipment)
        {
            _procResourceRepository = procResourceRepository;
            _currentEquipment = currentEquipment;

            RuleFor(x => x.SFC).NotEmpty().WithErrorCode(nameof(ErrorCode.MES19003));
            //每个条码都不允许为空
            RuleFor(x => x.SFC).Must(sfc => !string.IsNullOrEmpty(sfc.Trim())).WithErrorCode(ErrorCode.MES19003);
            RuleFor(x => x).MustAsync(async (outBoundDto, cancellation) =>
            {
                var query = new ProcResourceQuery
                {
                    SiteId = _currentEquipment.SiteId,
                    ResCode = outBoundDto.ResourceCode.Trim()
                };
                return await _procResourceRepository.IsExistsActiveAsync(query);
            }).WithErrorCode(nameof(ErrorCode.MES19006));
        }
    }
}
