using FluentValidation;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.EquipmentServices.Dtos.InBound;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Web.Framework.WorkContext;

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
        private readonly IManuSfcStepRepository _manuSfcStepRepository;
        private readonly IProcResourceRepository _procResourceRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationInBoundDtoRules"></param>
        /// <param name="currentEquipment"></param>
        /// <param name="validationInBoundMoreDtoRules"></param>
        /// <param name="procResourceRepository"></param>
        /// <param name="manuSfcStepRepository"></param>
        public InBoundService(AbstractValidator<InBoundDto> validationInBoundDtoRules,
            ICurrentEquipment currentEquipment,
            AbstractValidator<InBoundMoreDto> validationInBoundMoreDtoRules,
            IProcResourceRepository procResourceRepository,
            IManuSfcStepRepository manuSfcStepRepository)
        {
            _validationInBoundDtoRules = validationInBoundDtoRules;
            _currentEquipment = currentEquipment;
            _validationInBoundMoreDtoRules = validationInBoundMoreDtoRules;
            _procResourceRepository = procResourceRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
        }

        /// <summary>
        /// 进站
        /// </summary>
        /// <param name="inBoundDto"></param>
        /// <returns></returns>
        public async Task InBound(InBoundDto inBoundDto)
        {
            await _validationInBoundDtoRules.ValidateAndThrowAsync(inBoundDto);
            if (inBoundDto == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }
            //已经验证过资源是否存在直接使用
            var procResource = await _procResourceRepository.GetByCodeAsync(new EntityByCodeQuery { Site = _currentEquipment.SiteId, Code = inBoundDto.ResourceCode });
            ManuSfcStepEntity manuSfcStepEntity = PrepareSetpEntity(procResource.Id, inBoundDto.SFC);
            await _manuSfcStepRepository.InsertAsync(manuSfcStepEntity);
        }

        /// <summary>
        /// 进站（多个）
        /// </summary>
        /// <param name="inBoundMoreDto"></param>
        /// <returns></returns>
        public async Task InBoundMore(InBoundMoreDto inBoundMoreDto)
        {
            await _validationInBoundMoreDtoRules.ValidateAndThrowAsync(inBoundMoreDto);
            if (inBoundMoreDto == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }
            if (inBoundMoreDto.SFCs.Length <= 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19101));
            }
            //已经验证过资源是否存在直接使用
            var procResource = await _procResourceRepository.GetByCodeAsync(new EntityByCodeQuery { Site = _currentEquipment.SiteId, Code = inBoundMoreDto.ResourceCode });
            List<ManuSfcStepEntity> sfcStepList = new();
            foreach (var sfc in inBoundMoreDto.SFCs)
            {
                ManuSfcStepEntity manuSfcStepEntity = PrepareSetpEntity(procResource.Id, sfc);
                sfcStepList.Add(manuSfcStepEntity);
            }
            await _manuSfcStepRepository.InsertRangeAsync(sfcStepList);
        }

        /// <summary>
        /// 组装SfcStepEntity数据
        /// </summary>
        /// <param name="procResourceId"></param>
        /// <param name="sfc"></param>
        /// <returns></returns>
        private ManuSfcStepEntity PrepareSetpEntity(long procResourceId, string sfc)
        {
            return new ManuSfcStepEntity()
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = _currentEquipment.SiteId,
                SFC = sfc,
                Qty = 1,//进站数量
                EquipmentId = _currentEquipment.Id,
                ResourceId = procResourceId,
                CurrentStatus = SfcProduceStatusEnum.Activity,
                Operatetype = ManuSfcStepTypeEnum.InStock,
                CreatedBy = _currentEquipment.Name,
                CreatedOn = HymsonClock.Now(),
                UpdatedBy = _currentEquipment.Name,
                UpdatedOn = HymsonClock.Now()
            };
        }
    }
}
