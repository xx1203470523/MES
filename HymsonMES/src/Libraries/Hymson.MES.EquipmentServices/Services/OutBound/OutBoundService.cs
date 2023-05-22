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
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }
            //已经验证过资源是否存在直接使用
            var procResource = await _procResourceRepository.GetByCodeAsync(new EntityByCodeQuery { Site = _currentEquipment.SiteId, Code = outBoundDto.ResourceCode });
            //使用批量出站Dto
            OutBoundMoreDto outBoundMoreDto = new()
            {
                EquipmentCode = _currentEquipment.Code,
                ResourceCode = outBoundDto.ResourceCode,
                LocalTime = outBoundDto.LocalTime,
                SFCs = new OutBoundDto[] { outBoundDto }
            };
            //条码步骤
            List<ManuSfcStepEntity> manuSfcStepEntitys = PrepareSetpEntity(outBoundMoreDto, procResource.Id);
            //标准参数
            List<ManuProductParameterEntity> manuProductParameterEntities = await PrepareProductParameterEntity(outBoundMoreDto, procResource.Id);
            //NG
            List<ManuSfcStepNgEntity> manuProductNGEntities = await PrepareProductNgEntity(outBoundMoreDto, manuSfcStepEntitys);

            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                await _manuSfcStepRepository.InsertRangeAsync(manuSfcStepEntitys);
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
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }
            if (outBoundMoreDto.SFCs.Length <= 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19101));
            }
            //已经验证过资源是否存在直接使用
            var procResource = await _procResourceRepository.GetByCodeAsync(new EntityByCodeQuery { Site = _currentEquipment.SiteId, Code = outBoundMoreDto.ResourceCode });
            //条码步骤
            List<ManuSfcStepEntity> manuSfcStepEntitys = PrepareSetpEntity(outBoundMoreDto, procResource.Id);
            //标准参数
            List<ManuProductParameterEntity> manuProductParameterEntities = await PrepareProductParameterEntity(outBoundMoreDto, procResource.Id);
            //NG
            List<ManuSfcStepNgEntity> manuProductNGEntities = await PrepareProductNgEntity(outBoundMoreDto, manuSfcStepEntitys);

            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                await _manuSfcStepRepository.InsertRangeAsync(manuSfcStepEntitys);
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
        /// 组装SetpEntity
        /// </summary>
        /// <param name="outBoundMoreDto"></param>
        /// <param name="procResourceId"></param>
        /// <returns></returns>
        private List<ManuSfcStepEntity> PrepareSetpEntity(OutBoundMoreDto outBoundMoreDto, long procResourceId)
        {
            List<ManuSfcStepEntity> manuSfcStepEntities = new();
            foreach (var item in outBoundMoreDto.SFCs)
            {
                manuSfcStepEntities.Add(new ManuSfcStepEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentEquipment.SiteId,
                    SFC = item.SFC,
                    Qty = 1,//出站数量
                    Passed = item.Passed,
                    EquipmentId = _currentEquipment.Id,
                    ResourceId = procResourceId,
                    CurrentStatus = SfcProduceStatusEnum.Activity,
                    Operatetype = ManuSfcStepTypeEnum.OutStock,
                    CreatedBy = _currentEquipment.Name,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentEquipment.Name,
                    UpdatedOn = HymsonClock.Now()
                });
            }
            return manuSfcStepEntities;
        }

        /// <summary>
        /// 组装参数信息
        /// </summary>
        /// <param name="outBoundMoreDto"></param>
        /// <param name="procResourceId"></param>
        /// <returns></returns>
        private async Task<List<ManuProductParameterEntity>> PrepareProductParameterEntity(OutBoundMoreDto outBoundMoreDto, long procResourceId)
        {
            List<ManuProductParameterEntity> manuProductParameterEntities = new();
            //所有参数
            List<string> paramCodeList = new();
            foreach (var item in outBoundMoreDto.SFCs)
            {
                if (item.ParamList != null)
                {
                    var paramCodes = item.ParamList.Select(c => c.ParamCode);
                    paramCodeList.AddRange(paramCodes);
                }
            }
            //如果所有参数都为空
            if (paramCodeList.Count <= 0)
            {
                return manuProductParameterEntities;
            }

            var codesQuery = new EntityByCodesQuery
            {
                Site = _currentEquipment.SiteId,
                Codes = paramCodeList.ToArray()
            };
            var procParameter = await _procParameterRepository.GetByCodesAsync(codesQuery);
            //如果有不存在的参数编码就提示
            var noIncludeCodes = paramCodeList.Where(w => procParameter.Select(s => s.ParameterCode).Contains(w) == false);
            if (noIncludeCodes.Any() == true)
                throw new CustomerValidationException(nameof(ErrorCode.MES19108)).WithData("Code", string.Join(',', noIncludeCodes));

            foreach (var outBoundDto in outBoundMoreDto.SFCs)
            {
                if (outBoundDto.ParamList != null)
                {
                    var paramList = outBoundDto.ParamList.Select(s =>
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
                     );
                    manuProductParameterEntities.AddRange(paramList);
                }
            }
            return manuProductParameterEntities;
        }


        /// <summary>
        /// 组装NG信息
        /// </summary>
        /// <param name="outBoundMoreDto"></param>
        /// <param name="manuSfcStepEntities"></param>
        /// <returns></returns>
        private async Task<List<ManuSfcStepNgEntity>> PrepareProductNgEntity(OutBoundMoreDto outBoundMoreDto, List<ManuSfcStepEntity> manuSfcStepEntities)
        {

            List<ManuSfcStepNgEntity> manuSfcStepNgEntities = new();
            //所有NG编码
            List<string> ngCodeList = new();
            foreach (var item in outBoundMoreDto.SFCs)
            {
                if (item.NG != null)
                {
                    var ngCodes = item.NG.Select(c => c.NGCode);
                    ngCodeList.AddRange(ngCodes);
                }
            }
            //如果所有NG都为空
            if (ngCodeList.Count <= 0)
            {
                return manuSfcStepNgEntities;
            }

            var codesQuery = new QualUnqualifiedCodeByCodesQuery
            {
                Site = _currentEquipment.SiteId,
                Codes = ngCodeList.ToArray()
            };
            var qualUnqualifiedCodes = await _qualUnqualifiedCodeRepository.GetByCodesAsync(codesQuery);
            //如果有不存在的参数编码就提示
            var noIncludeCodes = ngCodeList.Where(w => qualUnqualifiedCodes.Select(s => s.UnqualifiedCode).Contains(w) == false);
            if (noIncludeCodes.Any() == true)
                throw new CustomerValidationException(nameof(ErrorCode.MES19114)).WithData("Code", string.Join(',', noIncludeCodes));

            foreach (var outBoundDto in outBoundMoreDto.SFCs)
            {
                var stepId = manuSfcStepEntities.Where(c => c.SFC == outBoundDto.SFC).First().Id;
                if (outBoundDto.NG != null)
                {
                    var ngList = outBoundDto.NG.Select(s =>
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
                     });
                    manuSfcStepNgEntities.AddRange(ngList);
                }
            }
            return manuSfcStepNgEntities;
        }
    }
}
