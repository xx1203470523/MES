using FluentValidation;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Quality.IQualityRepository;
using Hymson.MES.Data.Repositories.Quality.QualUnqualifiedCode.Query;
using Hymson.MES.EquipmentServices.Dtos.OutBound;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Hymson.Web.Framework.WorkContext;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Transactions;

namespace Hymson.MES.EquipmentServices.Services.OutBound
{
    /// <summary>
    /// 出站服务
    /// </summary>
    public class OutBoundService : IOutBoundService
    {
        private readonly ICurrentEquipment _currentEquipment;
        private readonly AbstractValidator<OutBoundDto> _validationOutBoundDtoRules;
        private readonly AbstractValidator<OutBoundMoreDto> _validationOutBoundMoreDtoRules;
        private readonly IManuSfcStepRepository _manuSfcStepRepository;
        private readonly IProcResourceRepository _procResourceRepository;
        private readonly IProcParameterRepository _procParameterRepository;
        private readonly IManuProductParameterRepository _manuProductParameterRepository;
        private readonly IQualUnqualifiedCodeRepository _qualUnqualifiedCodeRepository;
        private readonly IManuSfcStepNgRepository _manuSfcStepNgRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationOutBoundDtoRules"></param>
        /// <param name="currentEquipment"></param>
        /// <param name="validationOutBoundMoreDtoRules"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="procResourceRepository"></param>
        /// <param name="procParameterRepository"></param>
        /// <param name="manuProductParameterRepository"></param>
        /// <param name="qualUnqualifiedCodeRepository"></param>
        /// <param name="manuSfcStepNgRepository"></param>
        public OutBoundService(AbstractValidator<OutBoundDto> validationOutBoundDtoRules,
            ICurrentEquipment currentEquipment,
            AbstractValidator<OutBoundMoreDto> validationOutBoundMoreDtoRules,
            IManuSfcStepRepository manuSfcStepRepository,
            IProcResourceRepository procResourceRepository,
            IProcParameterRepository procParameterRepository,
            IManuProductParameterRepository manuProductParameterRepository,
            IQualUnqualifiedCodeRepository qualUnqualifiedCodeRepository,
            IManuSfcStepNgRepository manuSfcStepNgRepository)
        {
            _validationOutBoundDtoRules = validationOutBoundDtoRules;
            _currentEquipment = currentEquipment;
            _validationOutBoundMoreDtoRules = validationOutBoundMoreDtoRules;
            _manuSfcStepRepository = manuSfcStepRepository;
            _procResourceRepository = procResourceRepository;
            _procParameterRepository = procParameterRepository;
            _manuProductParameterRepository = manuProductParameterRepository;
            _qualUnqualifiedCodeRepository = qualUnqualifiedCodeRepository;
            _manuSfcStepNgRepository = manuSfcStepNgRepository;
        }

        /// <summary>
        /// 出站
        /// </summary>
        /// <param name="outBoundDto"></param>
        /// <returns></returns>
        public async Task OutBound(OutBoundDto outBoundDto)
        {
            await _validationOutBoundDtoRules.ValidateAndThrowAsync(outBoundDto);
            if (outBoundDto == null)
            {
                throw new ValidationException(nameof(ErrorCode.MES10100));
            }
            List<ManuProductParameterEntity> manuProductParameterEntities = new();
            List<ManuSfcStepNgEntity> manuProductNGEntities = new();
            //已经验证过资源是否存在直接使用
            var procResource = await _procResourceRepository.GetByCodeAsync(new EntityByCodeQuery { Site = _currentEquipment.SiteId, Code = outBoundDto.ResourceCode });
            //条码步骤
            ManuSfcStepEntity manuSfcStepEntity = PrepareSetpEntity(outBoundDto, procResource.Id);
            //标准参数
            if (outBoundDto?.ParamList?.Length > 0)
            {
                manuProductParameterEntities = await PrepareProductParameterEntity(outBoundDto, procResource.Id);
            }
            //NG
            if (outBoundDto?.NG?.Length > 0)
            {
                manuProductNGEntities = await PrepareProductNgEntity(outBoundDto, manuSfcStepEntity.Id);
            }

            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                await _manuSfcStepRepository.InsertAsync(manuSfcStepEntity);
                if (manuProductParameterEntities.Any())
                {
                    await _manuProductParameterRepository.InsertsAsync(manuProductParameterEntities);
                }
                if (manuProductNGEntities.Any())
                {
                    await _manuSfcStepNgRepository.InsertsAsync(manuProductNGEntities);
                }
                ts.Complete();
            }
        }

        /// <summary>
        /// 出站（多个）
        /// </summary>
        /// <param name="outBoundMoreDto"></param>
        /// <returns></returns>
        public async Task OutBoundMore(OutBoundMoreDto outBoundMoreDto)
        {
            await _validationOutBoundMoreDtoRules.ValidateAndThrowAsync(outBoundMoreDto);
            if (outBoundMoreDto == null)
            {
                throw new ValidationException(nameof(ErrorCode.MES10100));
            }
            if (outBoundMoreDto.SFCs.Length <= 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES19101));
            }
            //已经验证过资源是否存在直接使用
            var procResource = await _procResourceRepository.GetByResourceCodeAsync(outBoundMoreDto.ResourceCode);
            long procResourceId = procResource.First().Id;
            List<ManuSfcStepEntity> sfcStepList = new();
            foreach (var item in outBoundMoreDto.SFCs)
            {
                ManuSfcStepEntity manuSfcStepEntity = PrepareSetpEntity(item, procResourceId);
                sfcStepList.Add(manuSfcStepEntity);
            }
            await _manuSfcStepRepository.InsertRangeAsync(sfcStepList);
        }

        /// <summary>
        /// 组装SetpEntity
        /// </summary>
        /// <param name="outBoundDto"></param>
        /// <param name="procResourceId"></param>
        /// <returns></returns>
        private ManuSfcStepEntity PrepareSetpEntity(OutBoundDto outBoundDto, long procResourceId)
        {
            return new ManuSfcStepEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = _currentEquipment.SiteId,
                SFC = outBoundDto.SFC,
                Qty = 1,//出站数量
                Passed = outBoundDto.Passed,
                EquipmentId = _currentEquipment.Id,
                ResourceId = procResourceId,
                CurrentStatus = SfcProduceStatusEnum.Activity,
                Operatetype = ManuSfcStepTypeEnum.OutStock,
                CreatedBy = _currentEquipment.Name,
                CreatedOn = HymsonClock.Now(),
                UpdatedBy = _currentEquipment.Name,
                UpdatedOn = HymsonClock.Now()
            };
        }

        /// <summary>
        /// 组装参数信息
        /// </summary>
        /// <param name="outBoundDto"></param>
        /// <param name="procResourceId"></param>
        /// <returns></returns>
        private async Task<List<ManuProductParameterEntity>> PrepareProductParameterEntity(OutBoundDto outBoundDto, long procResourceId)
        {
            var paramCodeArray = outBoundDto.ParamList.Select(c => c.ParamCode).ToArray();
            var codesQuery = new EntityByCodesQuery
            {
                Site = _currentEquipment.SiteId,
                Codes = paramCodeArray
            };
            var procParameter = await _procParameterRepository.GetByCodesAsync(codesQuery);
            //如果有不存在的参数编码就提示
            var noIncludeCodes = paramCodeArray.Where(w => procParameter.Select(s => s.ParameterCode).Contains(w) == false);
            if (noIncludeCodes.Any() == true)
                throw new CustomerValidationException(nameof(ErrorCode.MES19108)).WithData("Code", string.Join(',', noIncludeCodes));

            return outBoundDto.ParamList.Select(s =>
                new ManuProductParameterEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentEquipment.SiteId,
                    CreatedBy = _currentEquipment.Code,
                    UpdatedBy = _currentEquipment.Code,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now(),
                    EquipmentId = _currentEquipment.Id ?? 0,
                    LocalTime = outBoundDto.LocalTime,
                    SFC = outBoundDto.SFC,
                    ResourceId = procResourceId,
                    ParameterId = procParameter.Where(c => c.ParameterCode.Equals(s.ParamCode)).First().Id,
                    ParamValue = s.ParamValue,
                    Timestamp = s.Timestamp
                }
           ).ToList();
        }


        /// <summary>
        /// 组装NG信息
        /// </summary>
        /// <param name="outBoundDto"></param>
        /// <param name="stepId"></param>
        /// <returns></returns>
        private async Task<List<ManuSfcStepNgEntity>> PrepareProductNgEntity(OutBoundDto outBoundDto, long stepId)
        {
            var ngCodeArray = outBoundDto.NG.Select(c => c.NGCode).ToArray();
            var codesQuery = new QualUnqualifiedCodeByCodesQuery
            {
                Site = _currentEquipment.SiteId,
                Codes = ngCodeArray
            };
            var qualUnqualifiedCodes = await _qualUnqualifiedCodeRepository.GetByCodesAsync(codesQuery);
            //如果有不存在的参数编码就提示
            var noIncludeCodes = ngCodeArray.Where(w => qualUnqualifiedCodes.Select(s => s.UnqualifiedCode).Contains(w) == false);
            if (noIncludeCodes.Any() == true)
                throw new CustomerValidationException(nameof(ErrorCode.MES19114)).WithData("Code", string.Join(',', noIncludeCodes));

            return outBoundDto.NG.Select(s =>
                new ManuSfcStepNgEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentEquipment.SiteId,
                    BarCodeStepId = stepId,
                    UnqualifiedCode = s.NGCode,
                    CreatedBy = _currentEquipment.Code,
                    UpdatedBy = _currentEquipment.Code,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now()
                }
           ).ToList();
        }
    }
}
