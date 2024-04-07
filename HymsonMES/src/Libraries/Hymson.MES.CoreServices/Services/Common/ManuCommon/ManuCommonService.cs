using FluentValidation;
using FluentValidation.Results;
using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums.Process;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Process.Resource;
using Hymson.Utils;
using System.Data;
using System.Text.Json;

namespace Hymson.MES.CoreServices.Services.Common
{
    /// <summary>
    /// 生产公共类
    /// </summary>
    public partial class ManuCommonService : IManuCommonService
    {
        /// <summary>
        /// 服务接口（主数据）
        /// </summary>
        private readonly IMasterDataService _masterDataService;

        /// <summary>
        /// 多语言
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
        /// 仓储接口（载具注册）
        /// </summary>
        private readonly IInteVehicleRepository _inteVehicleRepository;

        /// <summary>
        /// 仓储接口（二维载具条码明细）
        /// </summary>
        private readonly IInteVehiceFreightStackRepository _inteVehiceFreightStackRepository;

        /// <summary>
        /// 仓储接口（载具类型）
        /// </summary>
        private readonly IInteVehicleTypeRepository _inteVehicleTypeRepository;

        /// <summary>
        /// 条码仓储
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        /// <summary>
        /// 条码信息仓储
        /// </summary>
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="masterDataService"></param>
        /// <param name="localizationService"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="manuSfcCirculationRepository"></param>
        /// <param name="manuContainerPackRepository"></param>
        /// <param name="inteVehicleRepository"></param>
        /// <param name="inteVehiceFreightStackRepository"></param>
        /// <param name="inteVehicleTypeRepository"></param>
        /// <param name="manuSfcInfoRepository"></param>
        /// <param name="manuSfcRepository"></param>
        public ManuCommonService(IMasterDataService masterDataService,
            ILocalizationService localizationService,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IManuSfcCirculationRepository manuSfcCirculationRepository,
            IManuContainerPackRepository manuContainerPackRepository,
            IInteVehicleRepository inteVehicleRepository,
            IInteVehiceFreightStackRepository inteVehiceFreightStackRepository,
            IInteVehicleTypeRepository inteVehicleTypeRepository,
            IManuSfcInfoRepository manuSfcInfoRepository,
            IManuSfcRepository manuSfcRepository)
        {
            _masterDataService = masterDataService;
            _localizationService = localizationService;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuSfcCirculationRepository = manuSfcCirculationRepository;
            _manuContainerPackRepository = manuContainerPackRepository;
            _inteVehicleRepository = inteVehicleRepository;
            _inteVehiceFreightStackRepository = inteVehiceFreightStackRepository;
            _inteVehicleTypeRepository = inteVehicleTypeRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuSfcRepository = manuSfcRepository;
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
            }
        }

        /// <summary>
        /// 验证条码BOM清单用量
        /// </summary>
        /// <param name="procedureBomBo"></param>
        /// <returns></returns>
        public async Task VerifyBomQtyAsync(ManuProcedureBomBo procedureBomBo)
        {
            var procBomDetailEntities = await _masterDataService.GetBomDetailEntitiesByBomIdAsync(procedureBomBo.BomId);
            if (procBomDetailEntities == null) return;

            // 过滤出当前工序对应的物料（数据收集方式为内部和外部）
            procBomDetailEntities = procBomDetailEntities.Where(w => w.ProcedureId == procedureBomBo.ProcedureId && w.DataCollectionWay != MaterialSerialNumberEnum.Batch);
            if (procBomDetailEntities == null || !procBomDetailEntities.Any()) return;
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

            if (sfcCirculationEntities == null || !sfcCirculationEntities.Any())
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
                    var materialEntity = await _masterDataService.GetMaterialEntityByIdAsync(item.Key);
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
        /// <exception cref="CustomerValidationException"></exception>
        public async Task<bool> CheckBarCodeByMaskCodeRuleAsync(string barCode, long materialId)
        {
            var material = await _masterDataService.GetMaterialEntityByIdAsync(materialId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10204));

            // 物料未设置掩码
            if (!material.MaskCodeId.HasValue) throw new CustomerValidationException(nameof(ErrorCode.MES16616)).WithData("barCode", barCode);

            // 未设置规则
            var maskCodeRules = await _masterDataService.GetMaskCodeRuleEntitiesByMaskCodeIdAsync(material.MaskCodeId.Value);
            if (maskCodeRules == null || !maskCodeRules.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES16616)).WithData("barCode", barCode);

            return barCode.VerifyBarCode(maskCodeRules);
        }

        /// <summary>
        /// 获取载具里面的条码（带验证）
        /// </summary>
        /// <param name="requestBo"></param>
        /// <returns></returns>
        public async Task<IEnumerable<VehicleSFCResponseBo>> GetSFCsByVehicleCodesAsync(VehicleSFCRequestBo requestBo)
        {
            if (requestBo.VehicleCodes == null || !requestBo.VehicleCodes.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18623)).WithData("Code", "");
            }

            // 读取载具信息
            var vehicleEntities = await _inteVehicleRepository.GetByCodesAsync(new EntityByCodesQuery
            {
                SiteId = requestBo.SiteId,
                Codes = requestBo.VehicleCodes
            });

            // 不在系统中的载具代码
            var notInSystem = requestBo.VehicleCodes.Except(vehicleEntities.Select(s => s.Code));
            if (notInSystem.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18624))
                    .WithData("Code", string.Join(',', notInSystem));
            }

            // 检查是否是"禁用"状态的载具
            var disabledVehicles = vehicleEntities.Where(w => w.Status == DisableOrEnableEnum.Disable);
            if (disabledVehicles.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18625))
                    .WithData("Code", string.Join(',', disabledVehicles.Select(s => s.Code)));
            }

            // 检查是否是"禁用"状态的载具类型
            var vehicleTypeEntities = await _inteVehicleTypeRepository.GetByIdsAsync(vehicleEntities.Select(s => s.VehicleTypeId));
            var disabledVehicleTypes = vehicleTypeEntities.Where(w => w.Status == DisableOrEnableEnum.Disable);
            if (disabledVehicleTypes.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18627))
                    .WithData("Code", string.Join(',', disabledVehicleTypes.Select(s => s.Code)));
            }

            // 查询载具关联的条码明细
            var vehicleFreightStackEntities = await _inteVehiceFreightStackRepository.GetEntitiesAsync(new EntityByParentIdsQuery
            {
                SiteId = requestBo.SiteId,
                ParentIds = vehicleEntities.Select(s => s.Id)
            });

            if (!vehicleFreightStackEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18626))
                    .WithData("Code", string.Join(',', requestBo.VehicleCodes));
            }

            // 查询载具里面所有的条码
            var allProduceEntities = await _manuSfcProduceRepository.GetListBySfcsAsync(new ManuSfcProduceBySfcsQuery
            {
                SiteId = requestBo.SiteId,
                Sfcs = vehicleFreightStackEntities.Select(s => s.BarCode)
            });

            List<VehicleSFCResponseBo> list = new();
            var validationFailures = new List<ValidationFailure>();
            var vehicleFreightStackDic = vehicleFreightStackEntities.ToLookup(w => w.VehicleId).ToDictionary(d => d.Key, d => d);
            foreach (var item in vehicleFreightStackDic)
            {
                var vehicleEntity = vehicleEntities.FirstOrDefault(f => f.Id == item.Key);
                if (vehicleEntity == null) continue;

                // 验证产品序列码的编码/版本是否一致
                var sfcProduceEntities = allProduceEntities.Where(w => item.Value.Select(s => s.BarCode).Contains(w.SFC));
                var productIds = sfcProduceEntities.Select(s => s.ProductId).Distinct();
                if (productIds.Count() > 1)
                {
                    var validationFailure = new ValidationFailure() { FormattedMessagePlaceholderValues = new() };
                    validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", vehicleEntity.Code);
                    validationFailure.FormattedMessagePlaceholderValues.Add("Code", vehicleEntity.Code);
                    validationFailure.ErrorCode = nameof(ErrorCode.MES18628);
                    validationFailures.Add(validationFailure);
                    continue;
                }
                else
                {
                    list.AddRange(item.Value.Select(s => new VehicleSFCResponseBo { SFC = s.BarCode, VehicleCode = vehicleEntity.Code }));
                }
            }

            if (validationFailures.Any())
            {
                throw new ValidationException("", validationFailures);
            }

            return list;
        }

        /// <summary>
        /// 获取当前生产对象
        /// </summary>
        /// <param name="requestBo"></param>
        /// <returns></returns>
        public async Task<ManufactureResponseBo> GetManufactureBoAsync(ManufactureRequestBo requestBo)
        {
            // 查询资源
            var resourceEntity = await _masterDataService.GetResourceEntityByCodeAsync(new EntityByCodeQuery
            {
                Site = requestBo.SiteId,
                Code = requestBo.ResourceCode
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES19919)).WithData("ResCode", requestBo.ResourceCode);

            // 根据设备
            var equipmentEntity = await _masterDataService.GetEquipmentEntityByCodeAsync(new EntityByCodeQuery
            {
                Site = requestBo.SiteId,
                Code = requestBo.EquipmentCode
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES19005)).WithData("Code", requestBo.EquipmentCode);

            // 读取设备绑定的资源
            var resourceBindEntities = await _masterDataService.GetResourceEntitiesByEquipmentCodeAsync(new ProcResourceQuery
            {
                SiteId = requestBo.SiteId,
                EquipmentCode = requestBo.EquipmentCode
            });
            if (resourceBindEntities == null || !resourceBindEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19131))
                    .WithData("ResCode", requestBo.ResourceCode)
                    .WithData("EquCode", requestBo.EquipmentCode);
            }

            // 读取资源对应的工序（只查询启用状态）
            var procProcedureEntities = await _masterDataService.GetProcedureEntitiesByResourceIdAsync(new EntityByLinkIdQuery
            {
                SiteId = requestBo.SiteId,
                LinkId = resourceEntity.Id
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES19913)).WithData("ResCode", requestBo.ResourceCode);

            var procProcedureEntity = procProcedureEntities.FirstOrDefault(f => f.Status == SysDataStatusEnum.Enable || f.Status == SysDataStatusEnum.Retain)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES19935)).WithData("ResCode", requestBo.ResourceCode);

            return new ManufactureResponseBo
            {
                ResourceId = resourceEntity.Id,
                ProcedureId = procProcedureEntity.Id,
                EquipmentId = equipmentEntity.Id
            };
        }

        /// <summary>
        /// 校验工序是否合法
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task VerifyProcedureAsync(JobRequestBo commonBo)
        {
            if (commonBo == null) return;
            if (commonBo.InStationRequestBos == null || !commonBo.InStationRequestBos.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16370));
            }

            // 判断条码是否为空
            if (commonBo.InStationRequestBos.Any(a => a.SFC == null))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16382));
            }

            // 进站工序信息
            var procedureEntity = await _masterDataService.GetProcedureEntityByIdAsync(commonBo.ProcedureId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES16358)).WithData("Procedure", commonBo.ProcedureId);

            // 读取工序关联的资源
            var resourceIds = await commonBo.Proxy!.GetValueAsync(_masterDataService.GetProcResourceIdByProcedureIdAsync, commonBo.ProcedureId);
            if (resourceIds == null || !resourceIds.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16355)).WithData("ProcedureCode", procedureEntity.Code);
            }

            // 校验工序和资源是否对应
            if (!resourceIds.Any(a => a == commonBo.ResourceId))
            {
                //_logger.LogWarning($"工序{commonBo.ProcedureId}和资源{commonBo.ResourceId}不对应！");
                throw new CustomerValidationException(nameof(ErrorCode.MES16317));
            }

            // 临时中转变量
            var multiSFCBo = new MultiSFCBo { SiteId = commonBo.SiteId, SFCs = commonBo.InStationRequestBos.Select(s => s.SFC) };

            // 获取生产条码信息
            var sfcProduceEntities = await commonBo.Proxy.GetDataBaseValueAsync(_masterDataService.GetProduceEntitiesBySFCsAsync, multiSFCBo);
            if (sfcProduceEntities == null || !sfcProduceEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17415)).WithData("SFC", string.Join(',', multiSFCBo.SFCs));
            }

            // 是否有不属于在制品表的条码
            var notIncludeSFCs = multiSFCBo.SFCs.Except(sfcProduceEntities.Select(s => s.SFC));
            if (notIncludeSFCs.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17415)).WithData("SFC", string.Join(',', notIncludeSFCs));
            }

            // 判断条码锁状态
            var sfcProduceBusinessEntities = await commonBo.Proxy.GetValueAsync(_masterDataService.GetProduceBusinessEntitiesBySFCsAsync, multiSFCBo);

            // 合法性校验
            sfcProduceEntities.VerifySFCStatus(SfcStatusEnum.lineUp, _localizationService);
            sfcProduceBusinessEntities?.VerifyProcedureLock(sfcProduceEntities, procedureEntity);

            // 验证条码是否被容器包装
            await VerifyContainerAsync(multiSFCBo);

            // 获取生产工单（附带工单状态校验）
            var planWorkOrderEntities = await commonBo.Proxy.GetValueAsync(_masterDataService.GetProduceWorkOrderByIdsAsync, new WorkOrderIdsBo
            {
                WorkOrderIds = sfcProduceEntities!.Select(s => s.WorkOrderId)
            });
            if (planWorkOrderEntities!.Any(a => a.Status == PlanWorkOrderStatusEnum.Finish))
            {
                // 完工的工单，不允许再投入（不管哪个工序都不允许再投入，之前逻辑是会读取工艺路线，只对首工序进行校验）
                throw new CustomerValidationException(nameof(ErrorCode.MES16350));
            }

            // 如果工序对应不上
            var sfcProduceEntitiesOfNoMatchProcedure = sfcProduceEntities!.Where(a => a.ProcedureId != commonBo.ProcedureId);
            if (sfcProduceEntitiesOfNoMatchProcedure != null && sfcProduceEntitiesOfNoMatchProcedure.Any())
            {
                var query = new EntityBySiteIdQuery { SiteId = commonBo.SiteId };
                var allProcessRouteDetailLinks = await _masterDataService.GetProcessRouteLinkEntitiesAsync(query);
                var allProcessRouteDetailNodes = await _masterDataService.GetProcessRouteNodeEntitiesAsync(query);

                var validationFailures = new List<ValidationFailure>();
                foreach (var sfcProduce in sfcProduceEntitiesOfNoMatchProcedure)
                {
                    var sfcProcedureEntity = await _masterDataService.GetProcedureEntityByIdAsync(sfcProduce.ProcedureId)
                        ?? throw new CustomerValidationException(nameof(ErrorCode.MES16369))
                        .WithData("SFC", sfcProduce.SFC)
                        .WithData("Procedure", sfcProduce.ProcedureId);

                    // 如果存在工序不一致，且复投次数大于0时，抛出异常
                    if (sfcProduce.RepeatedCount > 0)
                    {
                        var validationFailure = new ValidationFailure() { FormattedMessagePlaceholderValues = new() };
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", sfcProduce.SFC);
                        validationFailure.FormattedMessagePlaceholderValues.Add("SFC", sfcProduce.SFC);
                        validationFailure.FormattedMessagePlaceholderValues.Add("Procedure", sfcProcedureEntity.Code);
                        validationFailure.FormattedMessagePlaceholderValues.Add("Cycle", sfcProduce.RepeatedCount);
                        validationFailure.ErrorCode = nameof(ErrorCode.MES16368);
                        validationFailures.Add(validationFailure);
                        continue;
                    }

                    // 如果有性能问题，可以考虑将这个两个集合先分组，然后再进行判断
                    var processRouteDetailLinks = allProcessRouteDetailLinks.Where(w => w.ProcessRouteId == sfcProduce.ProcessRouteId)
                        ?? throw new CustomerValidationException(nameof(ErrorCode.MES18213));

                    var processRouteDetailNodes = allProcessRouteDetailNodes.Where(w => w.ProcessRouteId == sfcProduce.ProcessRouteId)
                        ?? throw new CustomerValidationException(nameof(ErrorCode.MES18208));

                    // 判断条码应进站工序和实际进站工序之间是否全部都是随机工序（因为随机工序可以跳过）
                    var beginNode = processRouteDetailNodes.FirstOrDefault(f => f.ProcedureId == sfcProduce.ProcedureId);
                    var endNode = processRouteDetailNodes.FirstOrDefault(f => f.ProcedureId == commonBo.ProcedureId);

                    if (beginNode == null)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES18228))
                            .WithData("SFC", sfcProduce.SFC)
                            .WithData("Procedure", sfcProcedureEntity.Code);
                    }
                    if (endNode == null)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES18229))
                            .WithData("SFC", sfcProduce.SFC)
                            .WithData("Current", procedureEntity.Code);
                    }

                    var nodesOfOrdered = processRouteDetailNodes.OrderBy(o => o.SerialNo)
                        .Where(w => w.SerialNo.ParseToInt() >= beginNode.SerialNo.ParseToInt() && w.SerialNo.ParseToInt() < endNode.SerialNo.ParseToInt());

                    // 两个工序之间没有工序，即表示当前实际进站的工序，处于条码记录的应进站工序前面
                    if (!nodesOfOrdered.Any())
                    {
                        // 当前工序
                        var currentEntity = await _masterDataService.GetProcedureEntityByIdAsync(endNode.ProcedureId);

                        var validationFailure = new ValidationFailure() { FormattedMessagePlaceholderValues = new() };
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", sfcProduce.SFC);
                        validationFailure.FormattedMessagePlaceholderValues.Add("SFC", sfcProduce.SFC);
                        validationFailure.FormattedMessagePlaceholderValues.Add("Current", procedureEntity.Code);
                        validationFailure.FormattedMessagePlaceholderValues.Add("Procedure", sfcProcedureEntity.Code);
                        validationFailure.ErrorCode = nameof(ErrorCode.MES16354);
                        validationFailures.Add(validationFailure);

                        //_logger.LogWarning($"工艺路线工序节点数据异常，工艺路线ID：{sfcProduce.ProcessRouteId}，条码工序ID：{beginNode.ProcedureId}，进站工序ID：{endNode.ProcedureId}");
                        continue;
                    }

                    // 如果中间的工序存在不是随机工序的话，就返回false
                    if (nodesOfOrdered.Any(a => a.CheckType != ProcessRouteInspectTypeEnum.RandomInspection))
                    {
                        var validationFailure = new ValidationFailure() { FormattedMessagePlaceholderValues = new() };
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", sfcProduce.SFC);
                        validationFailure.FormattedMessagePlaceholderValues.Add("SFC", sfcProduce.SFC);
                        validationFailure.FormattedMessagePlaceholderValues.Add("Current", procedureEntity.Code);
                        validationFailure.FormattedMessagePlaceholderValues.Add("Procedure", sfcProcedureEntity.Code);
                        validationFailure.ErrorCode = nameof(ErrorCode.MES16357);
                        validationFailures.Add(validationFailure);
                        continue;
                    }
                }

                if (validationFailures.Any())
                {
                    throw new ValidationException("", validationFailures);
                }
            }

            // 循环次数验证（复投次数）
            sfcProduceEntities?.VerifySFCRepeatedCount(procedureEntity.Cycle ?? 1, _localizationService);
        }

    }
}
