using FluentValidation;
using FluentValidation.Results;
using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Services.Common.ManuExtension;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcCirculation.Query;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.MaskCode;
using System.Data;
using System.Text.Json;

namespace Hymson.MES.CoreServices.Services.Common.ManuCommon
{
    /// <summary>
    /// 生产公共类
    /// </summary>
    public class ManuCommonService : IManuCommonService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ILocalizationService _localizationService;

        /// <summary>
        /// 仓储接口（条码生产信息）
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 仓储接口（条码流转信息）
        /// </summary>
        private readonly IManuSfcCirculationRepository _manuSfcCirculationRepository;

        /// <summary>
        /// 仓储接口（容器包装）
        /// </summary>
        private readonly IManuContainerPackRepository _manuContainerPackRepository;

        /// <summary>
        /// 仓储接口（BOM明细）
        /// </summary>
        private readonly IProcBomDetailRepository _procBomDetailRepository;

        /// <summary>
        /// 仓储接口（物料维护）
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 仓储接口（掩码规则维护）
        /// </summary>
        private readonly IProcMaskCodeRuleRepository _procMaskCodeRuleRepository;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sequenceService"></param>
        /// <param name="localizationService"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="manuSfcCirculationRepository"></param>
        /// <param name="manuContainerPackRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="planWorkOrderActivationRepository"></param>
        /// <param name="procResourceRepository"></param>
        /// <param name="procBomDetailRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="procProcessRouteDetailLinkRepository"></param>
        /// <param name="procProcessRouteDetailNodeRepository"></param>
        /// <param name="whMaterialInventoryRepository"></param>
        public ManuCommonService(ILocalizationService localizationService,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IManuSfcCirculationRepository manuSfcCirculationRepository,
            IManuContainerPackRepository manuContainerPackRepository,
            IProcBomDetailRepository procBomDetailRepository,
            IProcMaterialRepository procMaterialRepository,
            IProcMaskCodeRuleRepository procMaskCodeRuleRepository)
        {
            _localizationService = localizationService;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuSfcCirculationRepository = manuSfcCirculationRepository;
            _manuContainerPackRepository = manuContainerPackRepository;
            _procBomDetailRepository = procBomDetailRepository;
            _procMaterialRepository = procMaterialRepository;
            _procMaskCodeRuleRepository = procMaskCodeRuleRepository;
        }

        /// <summary>
        /// 批量验证条码是否锁定
        /// </summary>
        /// <param name="procedureBo"></param>
        /// <returns></returns>
        public async Task VerifySfcsLockAsync(ManuProcedureBo procedureBo)
        {
            var sfcProduceBusinesss = await _manuSfcProduceRepository.GetSfcProduceBusinessListBySFCAsync(new SfcListProduceBusinessQuery
            {
                SiteId = procedureBo.SiteId,
                Sfcs = procedureBo.SFCs,
                BusinessType = ManuSfcProduceBusinessType.Lock
            });

            if (sfcProduceBusinesss == null || !sfcProduceBusinesss.Any()) return;

            List<ValidationFailure> validationFailures = new();
            foreach (var item in sfcProduceBusinesss)
            {
                var sfcProduceLockBo = JsonSerializer.Deserialize<SfcProduceLockBo>(item.BusinessContent);
                if (sfcProduceLockBo == null) continue;

                switch (sfcProduceLockBo.Lock)
                {
                    case QualityLockEnum.InstantLock:
                        break;
                    case QualityLockEnum.FutureLock:
                        // 如果锁的不是目标工序，就跳过
                        if (procedureBo.ProcedureId.HasValue && sfcProduceLockBo.LockProductionId != procedureBo.ProcedureId) continue;
                        break;
                    case QualityLockEnum.Unlock:
                    default:
                        continue;
                }

                validationFailures.Add(new ValidationFailure
                {
                    FormattedMessagePlaceholderValues = new Dictionary<string, object> { { "CollectionIndex", item.Sfc } },
                    ErrorCode = nameof(ErrorCode.MES18010)
                });
            }

            // 是否存在错误
            if (validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("SFCError"), validationFailures);
            }
        }

        /// <summary>
        /// 批量验证条码是否被容器包装
        /// </summary>
        /// <param name="sfcBos"></param>
        /// <returns></returns>
        public async Task VerifyContainerAsync(MultiSFCBo sfcBos)
        {
            var manuContainerPackEntities = await _manuContainerPackRepository.GetByLadeBarCodesAsync(new ManuContainerPackQuery
            {
                SiteId = sfcBos.SiteId,
                LadeBarCodes = sfcBos.SFCs.ToArray(),
            });

            if (manuContainerPackEntities != null && manuContainerPackEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18015)).WithData("SFC", string.Join(",", sfcBos.SFCs));
                //throw new CustomerValidationException(nameof(ErrorCode.MES18019)).WithData("SFC", string.Join(",", sfcs));
            }
        }

        /// <summary>
        /// 验证条码BOM清单用量
        /// </summary>
        /// <param name="procedureBomBo"></param>
        /// <returns></returns>
        public async Task VerifyBomQtyAsync(ManuProcedureBomBo procedureBomBo)
        {
            var procBomDetailEntities = await _procBomDetailRepository.GetByBomIdAsync(procedureBomBo.BomId);
            if (procBomDetailEntities == null) return;

            // 过滤出当前工序对应的物料（数据收集方式为内部和外部）
            procBomDetailEntities = procBomDetailEntities.Where(w => w.ProcedureId == procedureBomBo.ProcedureId && w.DataCollectionWay != MaterialSerialNumberEnum.Batch);
            if (procBomDetailEntities == null || procBomDetailEntities.Any() == false) return;

            // 流转信息（多条码）
            var sfcCirculationEntities = await _manuSfcCirculationRepository.GetSfcMoudulesAsync(new ManuSfcCirculationBySfcsQuery
            {
                SiteId = procedureBomBo.SiteId,
                Sfc = procedureBomBo.SFCs,
                CirculationTypes = new SfcCirculationTypeEnum[] {
                    SfcCirculationTypeEnum.Consume ,
                    SfcCirculationTypeEnum.ModuleAdd,
                    SfcCirculationTypeEnum.ModuleReplace
                },
                ProcedureId = procedureBomBo.ProcedureId,
                IsDisassemble = TrueOrFalseEnum.No
            });

            if (sfcCirculationEntities == null || sfcCirculationEntities.Any() == false)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16323));
            }

            // 根据物料分组
            var procBomDetailDictionary = procBomDetailEntities.ToLookup(w => w.MaterialId).ToDictionary(d => d.Key, d => d);
            foreach (var item in procBomDetailDictionary)
            {
                // 检查每个物料是否已经满足BOM用量要求（这里可以优化下）
                var currentQty = sfcCirculationEntities?.Where(w => w.CirculationMainProductId == item.Key)
                    .Sum(s => s.CirculationQty);

                // 目标用量
                var targetQty = item.Value.Sum(s => s.Usages);

                if (currentQty < targetQty)
                {
                    var materialEntity = await _procMaterialRepository.GetByIdAsync(item.Key);
                    if (materialEntity == null) continue;

                    throw new CustomerValidationException(nameof(ErrorCode.MES16321)).WithData("Code", materialEntity.MaterialCode);
                }
            }
        }

        /// <summary>
        /// 验证条码掩码规则
        /// </summary>
        /// <param name="barCode"></param>
        /// <param name="materialId"></param>
        /// <returns></returns>
        public async Task<bool> CheckBarCodeByMaskCodeRuleAsync(string barCode, long materialId)
        {
            var material = await _procMaterialRepository.GetByIdAsync(materialId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10204));

            // 物料未设置掩码
            if (material.MaskCodeId.HasValue == false) throw new CustomerValidationException(nameof(ErrorCode.MES16616)).WithData("barCode", barCode);

            // 未设置规则
            var maskCodeRules = await _procMaskCodeRuleRepository.GetByMaskCodeIdAsync(material.MaskCodeId.Value);
            if (maskCodeRules == null || maskCodeRules.Any() == false) throw new CustomerValidationException(nameof(ErrorCode.MES16616)).WithData("barCode", barCode);

            return barCode.VerifyBarCode(maskCodeRules);
        }

        #region 内部方法

        #endregion
    }
}
