using FluentValidation;
using Hymson.MES.Data.Repositories.Process.Resource;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.EquipmentServices.Dtos.InBound;
using Hymson.Web.Framework.WorkContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hymson.MES.Core.Constants;

namespace Hymson.MES.EquipmentServices.Validators.InBound
{
    /// <summary>
    /// 进站验证(多个)
    /// </summary>
    internal class InBoundMoreValidator : AbstractValidator<InBoundMoreDto>
    {
        private readonly IProcResourceRepository _procResourceRepository;
        private readonly ICurrentEquipment _currentEquipment;
        public InBoundMoreValidator(IProcResourceRepository procResourceRepository, ICurrentEquipment currentEquipment)
        {
            _procResourceRepository = procResourceRepository;
            _currentEquipment = currentEquipment;
            //条码列表不允许为空
            RuleFor(x => x.SFCs).NotEmpty().Must(list => list.Length <= 0).WithErrorCode(ErrorCode.MES19101);
            //每个条码都不允许为空
            RuleFor(x => x.SFCs).Must(list =>
                list.Where(sfc => string.IsNullOrEmpty(sfc.Trim())).Any()).WithErrorCode(ErrorCode.MES19003);
            //条码不允许重复
            RuleFor(x => x.SFCs).Must(list => list.GroupBy(sfc => sfc.Trim()).Where(c => c.Count() > 1).Any()).WithErrorCode(ErrorCode.MES19007);
            //资源编码校验
            RuleFor(x => x).MustAsync(async (inBoundMoreDto, cancellation) =>
            {
                var query = new ProcResourceQuery
                {
                    SiteId = _currentEquipment.SiteId,
                    ResCode = inBoundMoreDto.ResourceCode.Trim()
                };
                return await _procResourceRepository.IsExistsActiveAsync(query);
            }).WithErrorCode(nameof(ErrorCode.MES19006));
        }
    }
}
