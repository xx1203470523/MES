using FluentValidation;
using FluentValidation.Results;
using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcCirculation.Query;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using Hymson.Sequences;
using Hymson.Utils;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Hymson.MES.CoreServices.Services.Common.ManuCommon
{
    /// <summary>
    /// 生产公共类
    /// </summary>
    public class ManuCommonService
    {
        /// <summary>
        /// 序列号服务
        /// </summary>
        private readonly ISequenceService _sequenceService;

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
        /// 仓储接口（物料库存）
        /// </summary>
        private readonly IWhMaterialInventoryRepository _whMaterialInventoryRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly ILocalizationService _localizationService;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sequenceService"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="manuSfcCirculationRepository"></param>
        /// <param name="manuContainerPackRepository"></param>
        /// <param name="procBomDetailRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="whMaterialInventoryRepository"></param>
        /// <param name="localizationService"></param>
        public ManuCommonService(
            ISequenceService sequenceService,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IManuSfcCirculationRepository manuSfcCirculationRepository,
            IManuContainerPackRepository manuContainerPackRepository,
            IProcBomDetailRepository procBomDetailRepository,
            IProcMaterialRepository procMaterialRepository,
            IWhMaterialInventoryRepository whMaterialInventoryRepository,
            ILocalizationService localizationService)
        {
            _sequenceService = sequenceService;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuSfcCirculationRepository = manuSfcCirculationRepository;
            _manuContainerPackRepository = manuContainerPackRepository;
            _procBomDetailRepository = procBomDetailRepository;
            _procMaterialRepository = procMaterialRepository;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _localizationService = localizationService;
        }


        /// <summary>
        /// 获取生产条码信息
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public async Task<(ManuSfcProduceEntity, ManuSfcProduceBusinessEntity)> GetProduceSFCAsync(SingleSFCBo bo)
        {
            if (string.IsNullOrWhiteSpace(bo.SFC) == true
                || bo.SFC.Contains(' ') == true) throw new CustomerValidationException(nameof(ErrorCode.MES16305));

            // 条码在制表
            var sfcProduceEntity = await _manuSfcProduceRepository.GetBySFCAsync(new ManuSfcProduceBySfcQuery
            {
                SiteId = bo.SiteId,
                Sfc = bo.SFC
            });

            // 不存在在制表的话，就去库存查找
            if (sfcProduceEntity == null)
            {
                var whMaterialInventoryEntity = await _whMaterialInventoryRepository.GetByBarCodeAsync(new WhMaterialInventoryBarCodeQuery
                {
                    SiteId = bo.SiteId,
                    BarCode = bo.SFC
                });
                if (whMaterialInventoryEntity != null) throw new CustomerValidationException(nameof(ErrorCode.MES16318));

                throw new CustomerValidationException(nameof(ErrorCode.MES16306));
            }

            // 获取锁状态
            var sfcProduceBusinessEntity = await _manuSfcProduceRepository.GetSfcProduceBusinessBySFCAsync(new SfcProduceBusinessQuery
            {
                SiteId = bo.SiteId,
                Sfc = sfcProduceEntity.SFC,
                BusinessType = ManuSfcProduceBusinessType.Lock
            });

            return (sfcProduceEntity, sfcProduceBusinessEntity);
        }

        /// <summary>
        /// 批量验证条码是否锁定
        /// </summary>
        /// <param name="sfcs"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public async Task VerifySfcsLockAsync(IEnumerable<string> sfcs, long siteId)
        {
            var sfcProduceBusinesss = await _manuSfcProduceRepository.GetSfcProduceBusinessListBySFCAsync(new SfcListProduceBusinessQuery
            {
                SiteId = siteId,
                Sfcs = sfcs,
                BusinessType = ManuSfcProduceBusinessType.Lock
            });

            if (sfcProduceBusinesss == null || sfcProduceBusinesss.Any() == false) return;

            List<ValidationFailure> validationFailures = new();
            foreach (var item in sfcProduceBusinesss)
            {
                var sfcProduceLockBo = JsonSerializer.Deserialize<SfcProduceLockBo>(item.BusinessContent);
                if (sfcProduceLockBo == null) continue;

                if (sfcProduceLockBo.Lock != QualityLockEnum.InstantLock
                    && sfcProduceLockBo.Lock != QualityLockEnum.FutureLock) continue;

                validationFailures.Add(new ValidationFailure
                {
                    FormattedMessagePlaceholderValues = new Dictionary<string, object> { { "CollectionIndex", item.Sfc } },
                    ErrorCode = nameof(ErrorCode.MES18010)
                });
            }

            // 是否存在错误
            if (validationFailures.Any() == true)
            {
                throw new ValidationException(_localizationService.GetResource("SFCError"), validationFailures);
            }
        }

        /// <summary>
        /// 批量验证条码是否锁定
        /// </summary>
        /// <param name="sfcs"></param>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        public async Task VerifySfcsLockAsync(string[] sfcs, long procedureId, long siteId)
        {
            var sfcProduceBusinesss = await _manuSfcProduceRepository.GetSfcProduceBusinessListBySFCAsync(new SfcListProduceBusinessQuery { SiteId = siteId, Sfcs = sfcs, BusinessType = ManuSfcProduceBusinessType.Lock });
            if (sfcProduceBusinesss != null && sfcProduceBusinesss.Any())
            {
                var validationFailures = new List<ValidationFailure>();

                foreach (var item in sfcProduceBusinesss)
                {
                    var sfcProduceLockBo = JsonSerializer.Deserialize<SfcProduceLockBo>(item.BusinessContent);
                    if (sfcProduceLockBo == null) continue;

                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", item.Sfc}
                        };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", item.Sfc);
                    }

                    if (sfcProduceLockBo.Lock == QualityLockEnum.InstantLock)
                    {
                        validationFailure.ErrorCode = nameof(ErrorCode.MES18010);
                        validationFailures.Add(validationFailure);
                    }

                    if (sfcProduceLockBo.Lock == QualityLockEnum.FutureLock && sfcProduceLockBo.LockProductionId == procedureId)
                    {
                        validationFailure.ErrorCode = nameof(ErrorCode.MES18010);
                        validationFailures.Add(validationFailure);
                    }
                }

                //是否存在错误
                if (validationFailures.Any())
                {
                    throw new ValidationException(_localizationService.GetResource("SFCError"), validationFailures);
                }
            }
        }

        /// <summary>
        /// 批量验证条码是否被容器包装
        /// </summary>
        /// <param name="sfcs"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public async Task VerifyContainerAsync(string[] sfcs, long siteId)
        {
            var manuContainerPackEntities = await _manuContainerPackRepository.GetByLadeBarCodesAsync(new ManuContainerPackQuery
            {
                LadeBarCodes = sfcs,
                SiteId = siteId
            });

            if (manuContainerPackEntities != null && manuContainerPackEntities.Any() == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18015)).WithData("SFCs", string.Join(",", sfcs));
                //throw new CustomerValidationException(nameof(ErrorCode.MES18019)).WithData("SFC", string.Join(",", sfcs));
            }
        }

        /// <summary>
        /// 验证条码BOM清单用量
        /// </summary>
        /// <param name="bomId"></param>
        /// <param name="procedureId"></param>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public async Task VerifyBomQtyAsync(long bomId, long procedureId, string sfc, long siteId)
        {
            var procBomDetailEntities = await _procBomDetailRepository.GetByBomIdAsync(bomId);
            if (procBomDetailEntities == null) return;

            // 过滤出当前工序对应的物料（数据收集方式为内部和外部）
            procBomDetailEntities = procBomDetailEntities.Where(w => w.ProcedureId == procedureId && w.DataCollectionWay != MaterialSerialNumberEnum.Batch);
            if (procBomDetailEntities == null) return;

            // 流转信息
            var sfcCirculationEntities = await GetBarCodeCirculationListAsync(procedureId, sfc, siteId);
            if (sfcCirculationEntities == null) return;

            // 根据物料分组
            var procBomDetailDictionary = procBomDetailEntities.ToLookup(w => w.MaterialId).ToDictionary(d => d.Key, d => d);
            foreach (var item in procBomDetailDictionary)
            {
                // 检查每个物料是否已经满足BOM用量要求
                var currentQty = sfcCirculationEntities.Where(w => w.CirculationMainProductId == item.Key)
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
        /// 获取挂载的活动组件信息
        /// </summary>
        /// <param name="procedureId"></param>
        /// <param name="sfc"></param>
        /// <returns></returns>
        private async Task<IEnumerable<ManuSfcCirculationEntity>> GetBarCodeCirculationListAsync(long procedureId, string sfc, long siteId)
        {
            return await _manuSfcCirculationRepository.GetSfcMoudulesAsync(new ManuSfcCirculationQuery
            {
                Sfc = sfc,
                SiteId = siteId,
                CirculationTypes = new SfcCirculationTypeEnum[] {
                    SfcCirculationTypeEnum.Consume ,
                    SfcCirculationTypeEnum.ModuleAdd,
                    SfcCirculationTypeEnum.ModuleReplace
                },
                ProcedureId = procedureId,
                IsDisassemble = TrueOrFalseEnum.No
            });
        }

    }

    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class ManuSfcProduceExtensions
    {
        /// <summary>
        /// 条码资源关联校验
        /// </summary>
        /// <param name="sfcProduceEntity"></param>
        /// <param name="resourceId"></param>
        public static ManuSfcProduceEntity VerifyResource(this ManuSfcProduceEntity sfcProduceEntity, long resourceId)
        {
            // 当前资源是否对于的上
            if (sfcProduceEntity.ResourceId.HasValue == true && sfcProduceEntity.ResourceId != resourceId)
                throw new CustomerValidationException(nameof(ErrorCode.MES16316)).WithData("SFC", sfcProduceEntity.SFC);

            return sfcProduceEntity;
        }

        /// <summary>
        /// 检查条码状态是否合法
        /// </summary>
        /// <param name="sfcProduceEntity"></param>
        /// <param name="produceStatus"></param>
        public static ManuSfcProduceEntity VerifySFCStatus(this ManuSfcProduceEntity sfcProduceEntity, SfcProduceStatusEnum produceStatus)
        {
            // 当前条码是否是被锁定
            if (sfcProduceEntity.Status == SfcProduceStatusEnum.Locked) throw new CustomerValidationException(nameof(ErrorCode.MES16314)).WithData("SFC", sfcProduceEntity.SFC);

            // 当前条码是否是已报废
            if (sfcProduceEntity.IsScrap == TrueOrFalseEnum.Yes) throw new CustomerValidationException(nameof(ErrorCode.MES16322)).WithData("SFC", sfcProduceEntity.SFC);

            // 当前工序是否是指定状态
            if (sfcProduceEntity.Status != produceStatus) throw new CustomerValidationException(nameof(ErrorCode.MES16313)).WithData("Status", produceStatus.GetDescription());

            return sfcProduceEntity;
        }

        /// <summary>
        /// 工序活动状态校验
        /// </summary>
        /// <param name="sfcProduceEntity"></param>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        public static ManuSfcProduceEntity VerifyProcedure(this ManuSfcProduceEntity sfcProduceEntity, long procedureId)
        {
            // 产品编码是否和工序对应
            if (sfcProduceEntity.ProcedureId != procedureId) throw new CustomerValidationException(nameof(ErrorCode.MES16308));

            return sfcProduceEntity;
        }

        /// <summary>
        /// 工序锁校验
        /// </summary>
        /// <param name="sfcProduceBusinessEntity"></param>
        /// <param name="sfc"></param>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        public static ManuSfcProduceBusinessEntity? VerifyProcedureLock(this ManuSfcProduceBusinessEntity sfcProduceBusinessEntity, string sfc, long procedureId)
        {
            // 是否被锁定
            if (sfcProduceBusinessEntity == null) return sfcProduceBusinessEntity;
            if (sfcProduceBusinessEntity.BusinessType != ManuSfcProduceBusinessType.Lock) return sfcProduceBusinessEntity;

            var sfcProduceLockBo = JsonSerializer.Deserialize<SfcProduceLockBo>(sfcProduceBusinessEntity.BusinessContent);//sfcProduceBusinessEntity.BusinessContent
            if (sfcProduceLockBo == null) return sfcProduceBusinessEntity;

            if (sfcProduceLockBo.Lock == QualityLockEnum.InstantLock)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16314)).WithData("SFC", sfc);
            }

            if (sfcProduceLockBo.Lock == QualityLockEnum.FutureLock && sfcProduceLockBo.LockProductionId == procedureId)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16314)).WithData("SFC", sfc);
            }

            return sfcProduceBusinessEntity;
        }

        /// <summary>
        /// 验证条码
        /// </summary>
        /// <param name="barCode"></param>
        /// <param name="maskCodeRules"></param>
        /// <returns></returns>
        public static bool VerifyBarCode(this string barCode, IEnumerable<ProcMaskCodeRuleEntity> maskCodeRules)
        {
            // 对掩码规则进行校验
            foreach (var ruleEntity in maskCodeRules)
            {
                var rule = Regex.Replace(ruleEntity.Rule, "[?？]", ".");
                var pattern = $"{rule}";

                switch (ruleEntity.MatchWay)
                {
                    case MatchModeEnum.Start:
                        pattern = $"{rule}.+";
                        break;
                    case MatchModeEnum.Middle:
                        pattern = $".+{rule}.+";
                        break;
                    case MatchModeEnum.End:
                        pattern = $".+{rule}";
                        break;
                    case MatchModeEnum.Whole:
                        pattern = $"^{pattern}$";
                        break;
                    default:
                        break;
                }

                if (Regex.IsMatch(barCode, pattern) == false) return false;
            }

            return true;
        }
    }
}
