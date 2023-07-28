using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Resource;
using Hymson.MES.EquipmentServices.Dtos.SfcCirculation;
using Hymson.Web.Framework.WorkContext;

namespace Hymson.MES.EquipmentServices.Validators.SfcCirculation
{
    internal class SfcCirculationUnBindValidator : AbstractValidator<SfcCirculationUnBindDto>
    {
        private readonly IProcResourceRepository _procResourceRepository;
        private readonly ICurrentEquipment _currentEquipment;
        /// <summary>
        /// 条码解绑验证
        /// </summary>
        public SfcCirculationUnBindValidator(IProcResourceRepository procResourceRepository, ICurrentEquipment currentEquipment)
        {
            _procResourceRepository = procResourceRepository;
            _currentEquipment = currentEquipment;

            RuleFor(x => x.ResourceCode).NotEmpty().WithErrorCode(ErrorCode.MES19002);
            //RuleFor(x => x.UnBindSFCs).NotEmpty().Must(list => list.Length > 0).WithErrorCode(ErrorCode.MES19101);
            //每个条码都不允许为空
            RuleFor(x => x.UnBindSFCs).Must(list =>
                list.Where(c => !string.IsNullOrEmpty(c.Trim())).Any()).WithErrorCode(ErrorCode.MES19120);
            //条码不允许重复
            RuleFor(x => x.UnBindSFCs).Must(list => list.GroupBy(sfc => sfc.Trim()).Where(sfc => sfc.Count() < 2).Any()).WithErrorCode(ErrorCode.MES19007);
            //资源编码校验
            RuleFor(x => x).MustAsync(async (sfcCirculationUnBindDto, cancellation) =>
            {
                var query = new ProcResourceQuery
                {
                    SiteId = _currentEquipment.SiteId,
                    ResCode = sfcCirculationUnBindDto.ResourceCode.Trim()
                };
                return await _procResourceRepository.IsExistsActiveAsync(query);
            }).WithErrorCode(nameof(ErrorCode.MES19006));
        }
    }
}
