﻿using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Resource;
using Hymson.MES.EquipmentServices.Dtos.OutBound;
using Hymson.Web.Framework.WorkContext;

namespace Hymson.MES.EquipmentServices.Validators.OutBound
{
    /// <summary>
    /// 出站验证(多个)
    /// </summary>
    public class OutBoundMoreValidator : AbstractValidator<OutBoundMoreDto>
    {
        private readonly IProcResourceRepository _procResourceRepository;
        private readonly ICurrentEquipment _currentEquipment;
        /// <summary>
        /// 出站必要校验
        /// </summary>
        /// <param name="procResourceRepository"></param>
        /// <param name="currentEquipment"></param>
        public OutBoundMoreValidator(IProcResourceRepository procResourceRepository, ICurrentEquipment currentEquipment)
        {
            _procResourceRepository = procResourceRepository;
            _currentEquipment = currentEquipment;

            //条码列表不允许为空
            RuleFor(x => x.SFCs).NotEmpty().Must(list => list.Any()).WithErrorCode(ErrorCode.MES19101);
            //每个条码都不允许为空
            RuleFor(x => x.SFCs).Must(list =>
                list.Where(c => !string.IsNullOrEmpty(c.SFC.Trim())).Any()).WithErrorCode(ErrorCode.MES19003);
            //条码不允许重复
            RuleFor(x => x.SFCs).Must(list => list.GroupBy(c => c.SFC.Trim()).Where(c => c.Count() < 2).Any()).WithErrorCode(ErrorCode.MES19007);

            RuleFor(x => x).MustAsync(async (outBoundMoreDto, cancellation) =>
            {
                var query = new ProcResourceQuery
                {
                    SiteId = _currentEquipment.SiteId,
                    ResCode = outBoundMoreDto.ResourceCode.Trim()
                };
                return await _procResourceRepository.IsExistsActiveAsync(query);
            }).WithErrorCode(nameof(ErrorCode.MES19006));
        }
    }
}
