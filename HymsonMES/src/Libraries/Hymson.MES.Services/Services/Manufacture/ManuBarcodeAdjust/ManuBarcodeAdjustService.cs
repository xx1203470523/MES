using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Constants.Manufacture;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.CoreServices.Services.Manufacture.ManuGenerateBarcode;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Integrated.InteCodeRule.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Transactions;
using ValidationFailure = FluentValidation.Results.ValidationFailure;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 条码调整 服务
    /// </summary>
    public class ManuBarcodeAdjustService : IManuBarcodeAdjustService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        private readonly IInteVehicleRepository _inteVehicleRepository;
        private readonly IInteVehiceFreightStackRepository _inteVehiceFreightStackRepository;
        private readonly IManuSfcRepository _manuSfcRepository;
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IProcProcessRouteRepository _procProcessRouteRepository;
        private readonly IProcBomRepository _procBomRepository;
        private readonly IProcProcedureRepository _procProcedureRepository;
        private readonly IProcResourceRepository _procResourceRepository;
        private readonly IManuContainerBarcodeRepository _manuContainerBarcodeRepository;
        private readonly ILocalizationService _localizationService;
        private readonly IManuProductBadRecordRepository _manuProductBadRecordRepository;
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly IWhMaterialInventoryRepository _whMaterialInventoryRepository;
        private readonly IManuSfcCirculationRepository _manuSfcCirculationRepository;
        private readonly IManuGenerateBarcodeService _manuGenerateBarcodeService;
        private readonly IInteCodeRulesRepository _inteCodeRulesRepository;
        private readonly IInteCodeRulesMakeRepository _inteCodeRulesMakeRepository;
        private readonly IManuSfcStepRepository _manuSfcStepRepository;
        private readonly IWhMaterialStandingbookRepository _whMaterialStandingbookRepository;
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;
        private readonly IManuContainerPackRepository _manuContainerPackRepository;

        /// <summary>
        /// 验证器
        /// </summary>
        private readonly AbstractValidator<ManuBarcodeSplitAdjustDto> _validationManuBarcodeSplitAdjustRules;

        public ManuBarcodeAdjustService(ICurrentUser currentUser, ICurrentSite currentSite,
            IManuSfcProduceRepository manuSfcProduceRepository, IInteVehiceFreightStackRepository inteVehiceFreightStackRepository,
            IManuSfcRepository manuSfcRepository, IProcMaterialRepository procMaterialRepository,
            IProcProcessRouteRepository procProcessRouteRepository, IProcBomRepository procBomRepository,
            IProcProcedureRepository procProcedureRepository, IProcResourceRepository procResourceRepository,
            IInteVehicleRepository inteVehicleRepository, IManuContainerBarcodeRepository manuContainerBarcodeRepository,
            ILocalizationService localizationService, IManuProductBadRecordRepository manuProductBadRecordRepository,
            IPlanWorkOrderRepository planWorkOrderRepository, IWhMaterialInventoryRepository whMaterialInventoryRepository,
            IManuSfcCirculationRepository manuSfcCirculationRepository, IManuGenerateBarcodeService manuGenerateBarcodeService,
            IInteCodeRulesRepository inteCodeRulesRepository, IInteCodeRulesMakeRepository inteCodeRulesMakeRepository,
            IManuSfcStepRepository manuSfcStepRepository, IWhMaterialStandingbookRepository whMaterialStandingbookRepository,
            IManuSfcInfoRepository manuSfcInfoRepository,
            IManuContainerPackRepository manuContainerPackRepository,
            AbstractValidator<ManuBarcodeSplitAdjustDto> validationManuBarcodeSplitAdjustRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _inteVehiceFreightStackRepository = inteVehiceFreightStackRepository;
            _manuSfcRepository = manuSfcRepository;
            _procMaterialRepository = procMaterialRepository;
            _procProcessRouteRepository = procProcessRouteRepository;
            _procBomRepository = procBomRepository;
            _procProcedureRepository = procProcedureRepository;
            _procResourceRepository = procResourceRepository;
            _inteVehicleRepository = inteVehicleRepository;
            _manuContainerBarcodeRepository = manuContainerBarcodeRepository;
            _localizationService = localizationService;
            _manuProductBadRecordRepository = manuProductBadRecordRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _manuSfcCirculationRepository = manuSfcCirculationRepository;

            _manuGenerateBarcodeService = manuGenerateBarcodeService;
            _inteCodeRulesRepository = inteCodeRulesRepository;
            _inteCodeRulesMakeRepository = inteCodeRulesMakeRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _whMaterialStandingbookRepository = whMaterialStandingbookRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuContainerPackRepository = manuContainerPackRepository;
            _validationManuBarcodeSplitAdjustRules = validationManuBarcodeSplitAdjustRules;
        }

        /// <summary>
        /// 分页获取到条码
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcAboutInfoViewDto>> GetBarcodePagedListAsync(ManuSfcAboutInfoPagedQueryDto queryDto)
        {

            var resultViews = new List<ManuSfcAboutInfoViewDto>();
            var result = new PagedInfo<ManuSfcAboutInfoViewDto>(resultViews, queryDto.PageIndex, queryDto.PageSize, 0);

            var needSelectSFC = new List<string>();

            //查询 工序与 设备 就是查条码在制表
            if (queryDto.ProcedureId.HasValue || queryDto.ResourceId.HasValue)
            {
                //查询条码在制表 对应了哪些条码
                var sfcProduces = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(new ManuSfcProduceQuery() { SiteId = _currentSite.SiteId ?? 0, ProcedureId = queryDto.ProcedureId, ResourceId = queryDto.ResourceId });
                if (sfcProduces.Any())
                {
                    needSelectSFC = needSelectSFC.Union(sfcProduces.Select(x => x.SFC).ToList()).ToList();
                }
                else return result;
            }

            //查询 载具 就是查载具里面有没有条码
            //IEnumerable<InteVehicleFreightStackEntity> vehicleStacks= new List<InteVehicleFreightStackEntity>();
            if (queryDto.VehicleId.HasValue)
            {
                var vehicleStacks = await _inteVehiceFreightStackRepository.GetInteVehiceFreightStackEntitiesAsync(new InteVehiceFreightStackQuery()
                {
                    SiteId = _currentSite.SiteId ?? 0,
                    VehicleId = queryDto.VehicleId.Value,
                });
                if (vehicleStacks.Any())
                {
                    needSelectSFC = needSelectSFC.Union(vehicleStacks.Select(x => x.BarCode).ToList()).ToList();
                }
                else return result;
            }

            var query = queryDto.ToQuery<ManuSfcAboutInfoPagedQuery>();
            query.SiteId = _currentSite.SiteId ?? 0;
            query.Sfcs = needSelectSFC;

            var pagedInfo = await _manuSfcRepository.GetManuSfcAboutInfoPagedAsync(query);

            if (!pagedInfo.Data.Any()) return result;

            //查询物料
            var materials = await _procMaterialRepository.GetByIdsAsync(pagedInfo.Data.Select(x => x.ProductId).ToArray());

            //查询工艺路线
            var procProcessRoutes = await _procProcessRouteRepository.GetByIdsAsync(pagedInfo.Data.Select(x => x.ProcessRouteId).ToArray());

            //查询bom
            var productBoms = await _procBomRepository.GetByIdsAsync(pagedInfo.Data.Select(x => x.ProductBomId).ToArray());

            //查询工序
            List<(string, ProcProcedureEntity?)> sfcProcedures = new List<(string, ProcProcedureEntity?)>();//sfc对应该工序组成的数据
            if (queryDto.ProcedureId.HasValue && queryDto.ProcedureId > 0)
            {
                var procedure = await _procProcedureRepository.GetByIdAsync(queryDto.ProcedureId.Value);
                if (procedure != null) pagedInfo.Data.ToList().ForEach(x => sfcProcedures.Add((x.SFC, procedure)));
            }
            else
            {
                ////查询条码在制表
                //var sfcProduces= await _manuSfcProduceRepository.GetListBySfcsAsync(new ManuSfcProduceBySfcsQuery() 
                //{ 
                //    SiteId = _currentSite.SiteId??0,
                //    Sfcs= pagedInfo.Data.Select(x => x.SFC).ToArray() 
                //});

                ////条码对应的工序ID 集合
                //var sfc_ProcedureIds= sfcProduces.Select(x => new { Sfc = x.SFC, ProcedureId = x.ProcedureId });

                ////查询所有的工序
                //var procedures = await _procProcedureRepository.GetByIdsAsync(sfc_ProcedureIds.Select(x=>x.ProcedureId));

                //if (procedures != null&& procedures.Any()) pagedInfo.Data.ToList().ForEach((x) => {
                //    //找到对应的条码对应的 工序ID
                //    var sfc_procedureId = sfc_ProcedureIds.FirstOrDefault(y => y.Sfc == x.SFC);

                //    //找到对应的工序
                //   var procedure= procedures.FirstOrDefault(y => y.Id == sfc_procedureId?.ProcedureId);

                //    sfcProcedures.Add((x.SFC, procedure));//添加 sfc对应工序
                //});

                //查询条码在制表
                var sfcProduces = await _manuSfcProduceRepository.GetListBySfcsAsync(new ManuSfcProduceBySfcsQuery()
                {
                    SiteId = _currentSite.SiteId ?? 0,
                    Sfcs = pagedInfo.Data.Select(x => x.SFC).ToArray()
                });

                //查询所有的工序
                var procedures = await _procProcedureRepository.GetByIdsAsync(sfcProduces.Select(x => x.ProcedureId));

                sfcProcedures = (from sfcAboutInfo in pagedInfo.Data
                                 join sfcProduce in sfcProduces on sfcAboutInfo.SFC equals sfcProduce.SFC
                                 join procedure in procedures on sfcProduce.ProcedureId equals procedure.Id
                                 select (sfcAboutInfo.SFC, procedure)).ToList();
            }

            //查询载具
            List<(string, InteVehicleEntity?)> sfcVehicles = new List<(string, InteVehicleEntity?)>();//sfc对应 载具组成的数据
            if (queryDto.VehicleId.HasValue && queryDto.VehicleId > 0)
            {
                var vehicle = await _inteVehicleRepository.GetByIdAsync(queryDto.VehicleId.Value);
                if (vehicle != null) pagedInfo.Data.ToList().ForEach(x => sfcVehicles.Add((x.SFC, vehicle)));
            }
            else
            {
                var vehicleStacks = await _inteVehiceFreightStackRepository.GetInteVehiceFreightStackEntitiesAsync(new InteVehiceFreightStackQuery()
                {
                    SiteId = _currentSite.SiteId ?? 0,
                    Sfcs = pagedInfo.Data.Select(x => x.SFC),
                });

                var vehicles = await _inteVehicleRepository.GetByIdsAsync(vehicleStacks.Select(x => x.VehicleId).ToArray());

                sfcVehicles = (from sfcAboutInfo in pagedInfo.Data
                               join vehiclesStack in vehicleStacks on sfcAboutInfo.SFC equals vehiclesStack.BarCode
                               join vehicle in vehicles on vehiclesStack.VehicleId equals vehicle.Id
                               select (sfcAboutInfo.SFC, vehicle)).ToList();
            }

            foreach (var item in pagedInfo.Data)
            {
                var material = materials.FirstOrDefault(x => x.Id == item.ProductId);//物料
                var procProcessRoute = procProcessRoutes.FirstOrDefault(x => x.Id == item.ProcessRouteId);//工艺路线
                var bom = productBoms.FirstOrDefault(x => x.Id == item.ProductBomId);
                var procedure = sfcProcedures.FirstOrDefault(x => x.Item1 == item.SFC).Item2;//工序
                var vehicle = sfcVehicles.FirstOrDefault(x => x.Item1 == item.SFC).Item2;//载具

                var itemDto = item.ToModel<ManuSfcAboutInfoViewDto>();
                itemDto.VehicleCode = vehicle?.Code ?? "";
                itemDto.ProcedureCode = procedure?.Code ?? "";
                itemDto.MaterialCodeVersion = material == null ? "" : $"{material.MaterialCode}/{material.Version}";
                itemDto.MaterialCode = material?.MaterialCode ?? "";
                itemDto.MaterialVersion = material?.Version ?? "";
                itemDto.MaterialName = material?.MaterialName ?? "";
                itemDto.ProcessRouteCodeVersion = procProcessRoute == null ? "" : $"{procProcessRoute.Code}/{procProcessRoute.Version}";
                itemDto.BomCodeVersion = bom == null ? "" : $"{bom.BomCode}/{bom.Version}";
                resultViews.Add(itemDto);
            }

            return new PagedInfo<ManuSfcAboutInfoViewDto>(resultViews, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        #region  验证 方法
        /// <summary>
        /// 调整条码数量的验证
        /// </summary>
        /// <param name="sfcs"></param>
        /// <returns></returns>
        public async Task<bool> QtyAdjustVerifySfcAsync(string[] sfcs)
        {
            return await VerifySfcsAsync(sfcs, new ManuSfcsVerifyConditions
            {
                NotAllowStatus = new SfcStatusEnum[] { SfcStatusEnum.Scrapping, SfcStatusEnum.Delete, SfcStatusEnum.Locked, SfcStatusEnum.Complete, SfcStatusEnum.Invalid },
                IsBanNgSfc = true,
                NotAllowWorkOrderStatus = new PlanWorkOrderStatusEnum[] { PlanWorkOrderStatusEnum.Pending, PlanWorkOrderStatusEnum.Closed },
                IsVerifyMaterialQuantityLimit = true
            });
        }

        /// <summary>
        /// 合并条码的验证
        /// </summary>
        /// <param name="sfcs"></param>
        /// <returns></returns>
        public async Task<bool> MergeAdjustVerifySfcAsync(string[] sfcs)
        {
            await VerifySfcsAsync(sfcs, new ManuSfcsVerifyConditions
            {
                NotAllowStatus = new SfcStatusEnum[] { SfcStatusEnum.Scrapping, SfcStatusEnum.Delete, SfcStatusEnum.Locked, SfcStatusEnum.Invalid },
                IsVerifyBindVehicle = true,
                IsBanNgSfc = true,
                IsVerifyBindContainer = true,
                NotAllowWorkOrderStatus = new PlanWorkOrderStatusEnum[] { PlanWorkOrderStatusEnum.Pending },
                IsVerifyMaterialQuantityLimit = true,

                IsVerifySameWorkOrder = true,
                IsVerifySameMaterial = true,
                IsVerifySameBom = true,
                IsVerifySameProcessRoute = true,
                IsVerifySameSfcStatus = true,
            });

            return true;
        }
        #endregion

        #region 根据条码获取数据 方法
        /// <summary>
        /// 合并的获取条码数据
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public async Task<ManuSfcAboutInfoViewDto?> GetSfcAboutInfoBySfcInMergeAsync(string sfc)
        {
            await MergeAdjustVerifySfcAsync(new string[] { sfc });

            return await GetSfcAboutInfoBySfcAsync(sfc);
        }

        /// <summary>
        /// 数量调整的获取条码数据
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public async Task<ManuSfcAboutInfoViewDto?> GetSfcAboutInfoBySfcInQtyAsync(string sfc)
        {
            await QtyAdjustVerifySfcAsync(new string[] { sfc });

            return await GetSfcAboutInfoBySfcAsync(sfc);
        }

        /// <summary>
        /// 获取条码数据(Marking拦截)
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public async Task<ManuSfcAboutInfoViewDto?> GetSfcAboutInfoByMarkingSfcAsync(string sfc)
        {
            return await GetSfcAboutInfoBySfcAsync(sfc);
        }
        #endregion

        /// <summary>
        /// 合并条码
        /// </summary>
        /// <param name="adjustDto"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        public async Task<string> BarcodeMergeAdjustAsync(ManuBarcodeMergeAdjust adjustDto)
        {
            if (adjustDto == null || adjustDto.SFCs == null || !adjustDto.SFCs.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES12801));
            if (adjustDto.SFCs.Count() == 1) throw new CustomerValidationException(nameof(ErrorCode.MES12814));
            if (!string.IsNullOrEmpty(adjustDto.Remark) && adjustDto.Remark.Length > 255) throw new CustomerValidationException(nameof(ErrorCode.MES12827));

            //验证数据
            await MergeAdjustVerifySfcAsync(adjustDto.SFCs.ToArray());

            var sfcs = adjustDto.SFCs.ToArray();
            var isProduce = false;//是否在制

            //IEnumerable<ManuSfcProduceEntity>? sfcProduces = null;
            #region 补充一些验证
            //判断是否有在制
            var sfcProduces = await _manuSfcProduceRepository.GetListBySfcsAsync(new ManuSfcProduceBySfcsQuery() { SiteId = _currentSite.SiteId ?? 0, Sfcs = sfcs });
            if (sfcProduces != null && sfcProduces.Any())
            {
                isProduce = true;

                //判断是否全部在制
                if (sfcProduces.Count() != sfcs.Count())
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES12815));
                }

                //判断当前的状态是啥
                var currentStatus = sfcProduces.FirstOrDefault()?.Status;
                if (currentStatus == null) throw new CustomerValidationException(nameof(ErrorCode.MES12816));
                if (currentStatus == SfcStatusEnum.lineUp || currentStatus == SfcStatusEnum.Activity)
                {
                    //验证是否相同工序
                    var sfcsGroupByProcedures = sfcProduces.GroupBy(x => x.ProcedureId);
                    if (sfcsGroupByProcedures.Count() > 1)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES12817));
                    }

                    if (currentStatus == SfcStatusEnum.Activity)
                    {
                        var sfcsGroupByResourceIds = sfcProduces.GroupBy(x => x.ResourceId);
                        if (sfcsGroupByResourceIds.Count() > 1)
                        {
                            throw new CustomerValidationException(nameof(ErrorCode.MES12818));
                        }
                    }
                }
            }
            #endregion

            //查询数据
            var manuSfcs = await _manuSfcRepository.GetManuSfcInfoEntitiesAsync(new ManuSfcStatusQuery()
            {
                SiteId = _currentSite.SiteId ?? 0,
                Sfcs = sfcs
            });
            if (manuSfcs == null || !manuSfcs.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES12801));
            var oneManuSfc = manuSfcs.FirstOrDefault();

            //查询 工单 
            var workOrders = await _planWorkOrderRepository.GetByIdsAsync(manuSfcs.Select(x => x.WorkOrderId).ToList());

            //查询物料
            var materials = await _procMaterialRepository.GetByIdsAsync(manuSfcs.Select(x => x.ProductId).ToList());

            //查找库存
            var whMaterialInventorys = await _whMaterialInventoryRepository.GetByBarCodesOfHasQtyAsync(new Data.Repositories.Warehouse.WhMaterialInventory.Query.WhMaterialInventoryBarCodesQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                BarCodes = sfcs
            });


            #region 生成子条码

            var productId = manuSfcs.FirstOrDefault()?.ProductId;
            if (productId == null || productId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES12819));
            var inteCodeRulesEntity = await _inteCodeRulesRepository.GetInteCodeRulesByProductIdAsync(new InteCodeRulesByProductQuery
            {
                ProductId = manuSfcs.FirstOrDefault()!.ProductId,
                CodeType = CodeRuleCodeTypeEnum.ProcessControlSeqCode
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES12820));

            var codeRulesMakeList = await _inteCodeRulesMakeRepository.GetInteCodeRulesMakeEntitiesAsync(new InteCodeRulesMakeQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                CodeRulesId = inteCodeRulesEntity.Id
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES12821));

            //生成子条码
            var barcodeList = await _manuGenerateBarcodeService.GenerateBarCodeSerialNumberReturnBarCodeInfosAsync(new BarCodeSerialNumberBo
            {
                IsTest = false,
                IsSimulation = false,
                CodeRulesMakeBos = codeRulesMakeList.Select(s => new CodeRulesMakeBo
                {
                    Seq = s.Seq,
                    ValueTakingType = s.ValueTakingType,
                    SegmentedValue = s.SegmentedValue,
                    CustomValue = s.CustomValue,
                }),

                CodeRuleKey = $"{inteCodeRulesEntity.Id}",
                Count = 1,
                Base = inteCodeRulesEntity.Base,
                Increment = inteCodeRulesEntity.Increment,
                IgnoreChar = inteCodeRulesEntity.IgnoreChar,
                OrderLength = inteCodeRulesEntity.OrderLength,
                ResetType = inteCodeRulesEntity.ResetType,
                StartNumber = inteCodeRulesEntity.StartNumber,
                CodeMode = inteCodeRulesEntity.CodeMode,
                SiteId = _currentSite.SiteId ?? 0
            });

            var newSfcs = new List<String>();
            foreach (var item in barcodeList)
            {
                foreach (var sfc in item.BarCodes)
                {
                    newSfcs.Add(sfc);
                }
            }
            if (newSfcs.Count != 1) throw new CustomerValidationException(nameof(ErrorCode.MES12822));

            var oneNewSfc = newSfcs.First();

            #endregion

            #region 组装数据
            List<ManuSfcCirculationEntity> manuSfcCirculationEntities = new();//流转记录
            List<ManuSfcStepEntity> manuSfcStepEntities = new();//步骤记录
            List<WhMaterialStandingbookEntity> whMaterialStandingbookEntitys = new();

            #region 父条码
            foreach (var item in manuSfcs)
            {
                manuSfcCirculationEntities.Add(new ManuSfcCirculationEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    ProcedureId = isProduce ? sfcProduces?.FirstOrDefault()?.ProcedureId ?? 0 : 0,
                    ResourceId = isProduce ? sfcProduces?.FirstOrDefault()?.ResourceId ?? 0 : 0,
                    SFC = item.SFC,
                    WorkOrderId = item.WorkOrderId,
                    ProductId = item.ProductId,
                    //EquipmentId = param.EquipmentId,
                    CirculationBarCode = oneNewSfc,   //子条码
                    CirculationWorkOrderId = item.WorkOrderId,
                    CirculationProductId = item.ProductId,//暂时使用原有产品ID
                    CirculationMainProductId = item.ProductId,
                    //Location = item.Location,
                    CirculationQty = item.Qty,
                    CirculationType = SfcCirculationTypeEnum.Merge,
                    CreatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now()
                });

                manuSfcStepEntities.Add(new ManuSfcStepEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SFC = item.SFC,
                    ProductId = item.ProductId,
                    WorkOrderId = item.WorkOrderId,
                    WorkCenterId = isProduce ? sfcProduces?.FirstOrDefault()?.WorkCenterId : null,
                    ProductBOMId = isProduce ? sfcProduces?.FirstOrDefault()?.ProductBOMId : workOrders?.FirstOrDefault(x => x.Id == item.WorkOrderId)?.ProductBOMId,
                    ProcedureId = isProduce ? sfcProduces?.FirstOrDefault()?.ProcedureId : null,
                    Qty = item.Qty,
                    EquipmentId = isProduce ? sfcProduces?.FirstOrDefault()?.EquipmentId : null,
                    ResourceId = isProduce ? sfcProduces?.FirstOrDefault()?.ResourceId : null,
                    CurrentStatus = SfcStatusEnum.Invalid,
                    Operatetype = ManuSfcStepTypeEnum.SfcMerge,
                    SiteId = _currentSite.SiteId ?? 0,
                    CreatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now(),

                    Remark = adjustDto.Remark
                });

                //库存修改记录
                if (!isProduce)
                {
                    var material = materials.FirstOrDefault(x => x.Id == item.ProductId);
                    var whMaterialInventory = whMaterialInventorys.FirstOrDefault(x => x.MaterialBarCode == item.SFC);

                    whMaterialStandingbookEntitys.Add(new WhMaterialStandingbookEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = _currentSite.SiteId ?? 0,
                        MaterialCode = material?.MaterialCode ?? "",
                        MaterialName = material?.MaterialName ?? "",
                        MaterialVersion = material?.Version ?? "",
                        MaterialBarCode = item.SFC,
                        Batch = whMaterialInventory.Batch,
                        Quantity = 0,//库存修改为0 
                        Unit = material?.Unit ?? "",
                        Type = WhMaterialInventoryTypeEnum.InventoryModify,
                        Source = whMaterialInventory?.Source ?? 0,
                        CreatedBy = _currentUser.UserName,
                        CreatedOn = HymsonClock.Now(),
                        UpdatedBy = _currentUser.UserName,
                        UpdatedOn = HymsonClock.Now()
                    });
                }

            }
            #endregion

            #region 子条码
            List<ManuSfcEntity> manuSfcList = new();
            List<ManuSfcInfoEntity> manuSfcInfoList = new();
            List<ManuSfcProduceEntity> manuSfcProduceList = new();
            List<WhMaterialInventoryEntity> whMaterialInventoryEntities = new();

            //子条码新增数据到manu_sfc , manu_sfc_info, 如果是在制则也新增到在制中   还有 库存           
            var sfcId = IdGenProvider.Instance.CreateId();
            var sfcInfoId = IdGenProvider.Instance.CreateId();

            var qty = manuSfcs.Select(x => x.Qty).Sum();
            manuSfcList.Add(new ManuSfcEntity
            {
                Id = sfcId,
                SiteId = _currentSite.SiteId ?? 0,
                SFC = oneNewSfc,
                Qty = qty,
                IsUsed = YesOrNoEnum.Yes,
                Status = oneManuSfc!.Status,
                CreatedBy = _currentUser.UserName,
                UpdatedBy = _currentUser.UserName
            });

            manuSfcInfoList.Add(new ManuSfcInfoEntity
            {
                Id = sfcInfoId,
                SiteId = _currentSite.SiteId ?? 0,
                SfcId = sfcId,
                WorkOrderId = oneManuSfc.WorkOrderId,
                ProductId = oneManuSfc.ProductId,
                ProcessRouteId = oneManuSfc.ProcessRouteId,
                ProductBOMId = oneManuSfc.ProductBOMId,
                IsUsed = true,
                CreatedBy = _currentUser.UserName,
                UpdatedBy = _currentUser.UserName
            });

            if (isProduce)
                manuSfcProduceList.Add(new ManuSfcProduceEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    SFC = oneNewSfc,
                    SFCId = sfcId,
                    ProductId = oneManuSfc.ProductId,
                    WorkOrderId = oneManuSfc.WorkOrderId,
                    BarCodeInfoId = sfcInfoId,
                    ProcessRouteId = sfcProduces?.FirstOrDefault()?.ProcessRouteId ?? 0,
                    WorkCenterId = sfcProduces?.FirstOrDefault()?.WorkCenterId ?? 0,
                    ProductBOMId = sfcProduces?.FirstOrDefault()?.ProductBOMId ?? 0,
                    Qty = qty,
                    ProcedureId = sfcProduces?.FirstOrDefault()?.ProcedureId ?? 0,
                    ResourceId = sfcProduces?.FirstOrDefault()?.ResourceId ?? 0,
                    Status = sfcProduces?.FirstOrDefault()?.Status ?? 0,
                    RepeatedCount = 0,
                    IsScrap = TrueOrFalseEnum.No,
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName
                });
            else
            {
                var material = materials.FirstOrDefault(x => x.Id == oneManuSfc.ProductId);

                var whMaterialInventoryFather = whMaterialInventorys.FirstOrDefault();//找到父组件的一个库存信息

                whMaterialInventoryEntities.Add(new WhMaterialInventoryEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SupplierId = whMaterialInventoryFather?.SupplierId ?? 0,
                    MaterialId = oneManuSfc.ProductId,
                    MaterialBarCode = oneNewSfc,
                    Batch = whMaterialInventoryFather.Batch,
                    MaterialType = whMaterialInventoryFather?.MaterialType ?? 0,
                    QuantityResidue = qty,
                    Status = WhMaterialInventoryStatusEnum.ToBeUsed,
                    Source = whMaterialInventoryFather?.Source ?? MaterialInventorySourceEnum.ManuComplete,
                    SiteId = _currentSite.SiteId ?? 0,
                    CreatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now()
                });
                whMaterialStandingbookEntitys.Add(new WhMaterialStandingbookEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    MaterialCode = material?.MaterialCode ?? "",
                    MaterialName = material?.MaterialName ?? "",
                    MaterialVersion = material?.Version ?? "",
                    MaterialBarCode = oneNewSfc,
                    Batch = whMaterialInventoryFather.Batch,
                    Quantity = qty,//库存修改为0 
                    Unit = material?.Unit ?? "",
                    Type = WhMaterialInventoryTypeEnum.Merge,
                    Source = whMaterialInventoryFather?.Source ?? MaterialInventorySourceEnum.ManuComplete,
                    CreatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now()
                });
            }

            manuSfcStepEntities.Add(new ManuSfcStepEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = _currentSite.SiteId ?? 0,
                SFC = oneNewSfc,
                ProductId = oneManuSfc.ProductId,
                WorkOrderId = oneManuSfc.WorkOrderId,
                ProductBOMId = isProduce ? sfcProduces?.FirstOrDefault()?.ProductBOMId : workOrders?.FirstOrDefault(x => x.Id == oneManuSfc.WorkOrderId)?.ProductBOMId,
                WorkCenterId = isProduce ? sfcProduces?.FirstOrDefault()?.WorkCenterId : null,
                Qty = qty,
                ProcedureId = isProduce ? sfcProduces?.FirstOrDefault()?.ProcedureId : null,
                Operatetype = ManuSfcStepTypeEnum.SfcMergeAdd,
                CurrentStatus = oneManuSfc.Status,
                CreatedBy = _currentUser.UserName,
                UpdatedBy = _currentUser.UserName,
                Remark = adjustDto.Remark
            });

            #endregion

            #endregion
            #region 操作数据库
            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                #region 父级相关操作
                //设置条码为无效，数量为0  manu_sfc与 manu_sfc_produce
                await _manuSfcRepository.UpdateStatusAndQtyBySfcsAsync(new UpdateStatusAndQtyBySfcsCommand()
                {
                    SiteId = _currentSite.SiteId,
                    SFCs = sfcs,
                    Status = SfcStatusEnum.Invalid,
                    Qty = 0,
                    UpdatedBy = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now()
                });
                if (isProduce)
                    await _manuSfcProduceRepository.UpdateStatusAndQtyBySfcsAsync(new UpdateStatusAndQtyBySfcsCommand
                    {
                        SiteId = _currentSite.SiteId ?? 0,
                        SFCs = sfcs,
                        Status = SfcStatusEnum.Invalid,
                        Qty = 0,
                        UpdatedBy = _currentUser.UserName,
                        UpdatedOn = HymsonClock.Now()
                    });
                else
                    //设置 条码库存为0
                    await _whMaterialInventoryRepository.UpdateQuantityResidueBySFCsAsync(new Data.Repositories.Warehouse.WhMaterialInventory.Command.UpdateQuantityResidueBySfcsCommand
                    {
                        SiteId = _currentSite.SiteId ?? 0,
                        Sfcs = sfcs,
                        QuantityResidue = 0,
                        UpdatedBy = _currentUser.UserName,
                    });

                #endregion

                #region 子
                await _manuSfcRepository.InsertRangeAsync(manuSfcList);
                await _manuSfcInfoRepository.InsertsAsync(manuSfcInfoList);
                if (manuSfcProduceList.Any()) await _manuSfcProduceRepository.InsertRangeAsync(manuSfcProduceList);
                if (whMaterialInventoryEntities.Any()) await _whMaterialInventoryRepository.InsertsAsync(whMaterialInventoryEntities);
                #endregion

                await _manuSfcCirculationRepository.InsertRangeAsync(manuSfcCirculationEntities);

                await _manuSfcStepRepository.InsertRangeAsync(manuSfcStepEntities);

                if (whMaterialStandingbookEntitys.Any()) await _whMaterialStandingbookRepository.InsertsAsync(whMaterialStandingbookEntitys);

                ts.Complete();
            }
            #endregion

            return oneNewSfc;
        }

        /// <summary>
        /// 条码拆分
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<string> BarcodeSplitAdjustAsync(ManuBarcodeSplitAdjustDto param)
        {
            await _validationManuBarcodeSplitAdjustRules.ValidateAndThrowAsync(param);

            #region 数据准备
            var manuSfcEntityTask = _manuSfcRepository.GetSingleAsync(new ManuSfcQuery
            {
                SFC = param.SFC,
                SiteId = _currentSite.SiteId ?? 0,
                Type = SfcTypeEnum.Produce
            });
            var manuSfcProduceEntityTask = _manuSfcProduceRepository.GetBySFCAsync(new ManuSfcProduceBySfcQuery { SiteId = _currentSite.SiteId ?? 0, Sfc = param.SFC });
            //包装
            var sfcPackEntityTask = _manuContainerPackRepository.GetByLadeBarCodeAsync(new ManuContainerPackQuery { LadeBarCode = param.SFC, SiteId = _currentSite.SiteId ?? 0 });
            // 仓库
            var whMaterialInventoryEntityTask = _whMaterialInventoryRepository.GetByBarCodeAsync(new WhMaterialInventoryBarCodeQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                BarCode = param.SFC
            });
            //载具
            var inteVehiceFreightStackEntityTask = _inteVehiceFreightStackRepository.GetBySFCAsync(new InteVehiceFreightStackBySfcQuery { SiteId = _currentSite.SiteId ?? 0, BarCode = param.SFC });
            //不合格
            var manuProductBadRecordEntitiesTask = _manuProductBadRecordRepository.GetManuProductBadRecordEntitiesBySFCAsync(new ManuProductBadRecordBySfcQuery
            {
                Status = ProductBadRecordStatusEnum.Open,
                SFC = param.SFC,
                SiteId = _currentSite.SiteId ?? 0,
            });

            var manuSfcEntity = await manuSfcEntityTask;
            var manuSfcProduceEntity = await manuSfcProduceEntityTask;
            var sfcPackEntity = await sfcPackEntityTask;
            var whMaterialInventoryEntity = await whMaterialInventoryEntityTask;
            var inteVehiceFreightStackEntity = await inteVehiceFreightStackEntityTask;
            var manuProductBadRecordEntities = await manuProductBadRecordEntitiesTask;

            var manuSfcInfoEntity = await _manuSfcInfoRepository.GetBySFCIdWithIsUseAsync(manuSfcEntity.Id);

            var planWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(manuSfcInfoEntity.WorkOrderId ?? 0);
            if (planWorkOrderEntity != null && planWorkOrderEntity.Status == PlanWorkOrderStatusEnum.Pending)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12835)).WithData("sfc", param.SFC).WithData("WorkOrder", planWorkOrderEntity.OrderCode);
            }
            #endregion

            #region 校验
            if (manuSfcEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12802)).WithData("sfc", param.SFC);
            }

            if (ManuSfcStatus.ForbidSfcStatuss.Contains(manuSfcEntity.Status))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16378)).WithData("Status", _localizationService.GetResource($"Hymson.MES.Core.Enums.manu.SfcStatusEnum.{SfcStatusEnum.GetName(manuSfcEntity.Status)}"));
            }

            if (sfcPackEntity != null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12804)).WithData("sfc", param.SFC);
            }

            if (inteVehiceFreightStackEntity != null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12803)).WithData("sfc", param.SFC);
            }

            if (manuSfcProduceEntity != null && manuSfcProduceEntity.IsRepair == TrueOrFalseEnum.Yes)
            {
                var procProcedureEntity = await _procProcedureRepository.GetByIdAsync(manuSfcProduceEntity.ProcedureId);
                throw new CustomerValidationException(nameof(ErrorCode.MES12834)).WithData("sfc", param.SFC).WithData("ProcedureCode", procProcedureEntity.Code);
            }

            if (manuProductBadRecordEntities != null && manuProductBadRecordEntities.Any())
            {
                if (manuProductBadRecordEntities.Any(x => x.DisposalResult == ProductBadDisposalResultEnum.WaitingJudge))
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES12838)).WithData("sfc", param.SFC);
                }

                throw new CustomerValidationException(nameof(ErrorCode.MES12839)).WithData("sfc", param.SFC);
            }

            var procMaterialEntitity = await _procMaterialRepository.GetByIdAsync(manuSfcInfoEntity?.ProductId ?? 0);
            if (procMaterialEntitity.QuantityLimit == MaterialQuantityLimitEnum.OnlyOne) throw new CustomerValidationException(nameof(ErrorCode.MES12836));

            if (param.Qty > manuSfcEntity.Qty)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12829)).WithData("sfc", param.SFC)
                    .WithData("Qty", manuSfcEntity.Qty)
                    .WithData("SplitQty", param.Qty);
            }


            #endregion

            var barCodeList = await GenerateBarcodeByproductId(manuSfcInfoEntity?.ProductId ?? 0);
            if (barCodeList == null || !barCodeList.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12830)).WithData("ProductCode", procMaterialEntitity.MaterialCode);
            }

            if (barCodeList.Count() > 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12831)).WithData("ProductCode", procMaterialEntitity.MaterialCode);
            }

            string? newSplitSFC = barCodeList.SelectMany(x => x.BarCodes.Select(x => x)).FirstOrDefault();
            if (newSplitSFC == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12837)).WithData("ProductCode", procMaterialEntitity.MaterialCode);
            }

            var sfcStepList = new List<ManuSfcStepEntity>();
            ManuSfcProduceEntity? addManuSfcProduceEntity = null;
            WhMaterialInventoryEntity? addWhMaterialInventoryEntity = null;
            UpdateQuantityRangeCommand? updateQuantityRangeCommand = null;
            WhMaterialStandingbookEntity? whMaterialStandingbookEntity = null;
            UpdateStatusAndQtyByIdCommand? updateStatusAndQtyByIdCommand = null;
            UpdateManuSfcQtyAndCurrentQtyVerifyByIdCommand? updateManuSfcQtyAndCurrentQtyVerifyByIdCommand = null;
            UpdateManuSfcProduceQtyByIdCommand? updateManuSfcProduceQtyByIdCommand = null;
            sfcStepList.Add(new ManuSfcStepEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = _currentSite.SiteId ?? 0,
                SFC = param.SFC,
                ProductId = manuSfcInfoEntity?.ProductId ?? 0,
                WorkOrderId = manuSfcInfoEntity?.WorkOrderId ?? 0,
                WorkCenterId = manuSfcProduceEntity?.WorkCenterId,
                ProductBOMId = manuSfcProduceEntity?.ProductBOMId,
                ProcedureId = manuSfcProduceEntity?.ProcedureId,
                Qty = manuSfcEntity.Qty,
                Operatetype = ManuSfcStepTypeEnum.Split,
                CurrentStatus = manuSfcEntity.Status,
                EquipmentId = manuSfcProduceEntity?.EquipmentId,
                ResourceId = manuSfcProduceEntity?.ResourceId,
                Remark = param.Remark,
                CreatedBy = _currentUser.UserName,
                CreatedOn = HymsonClock.Now(),
                UpdatedBy = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now(),
            });

            if (manuSfcEntity.Qty == param.Qty)
            {
                updateStatusAndQtyByIdCommand = new UpdateStatusAndQtyByIdCommand
                {
                    Id = manuSfcEntity.Id,
                    Status = SfcStatusEnum.Invalid,
                    CurrentStatus = manuSfcEntity.Status,
                    Qty = manuSfcEntity.Qty - param.Qty,
                    CurrentQty = manuSfcEntity.Qty,
                    UpdatedBy = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now()
                };
            }
            else
            {
                updateManuSfcQtyAndCurrentQtyVerifyByIdCommand = new UpdateManuSfcQtyAndCurrentQtyVerifyByIdCommand
                {
                    Id = manuSfcEntity.Id,
                    Qty = manuSfcEntity.Qty - param.Qty,
                    CurrentQty = manuSfcEntity.Qty,
                    CurrentStatus = manuSfcEntity.Status,
                    UpdatedBy = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now()
                };
                if (ManuSfcStatus.SfcStatusInProcess.Contains(manuSfcEntity.Status))
                {
                    updateManuSfcProduceQtyByIdCommand = new UpdateManuSfcProduceQtyByIdCommand
                    {
                        Id = manuSfcProduceEntity?.Id ?? 0,
                        Qty = manuSfcEntity.Qty - param.Qty,
                        UpdatedBy = _currentUser.UserName,
                        UpdatedOn = HymsonClock.Now()
                    };
                }
            }

            //生成条码
            //插入生产一套表
            var manuSfc = new ManuSfcEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SFC = newSplitSFC,
                IsUsed = YesOrNoEnum.No,
                Qty = param.Qty,
                SiteId = _currentSite.SiteId ?? 0,
                Status = manuSfcEntity.Status,
                CreatedBy = _currentUser.UserName,
                CreatedOn = HymsonClock.Now(),
                UpdatedBy = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now()
            };

            var manuSfcInfo = new ManuSfcInfoEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = manuSfcInfoEntity?.SiteId ?? 0,
                SfcId = manuSfc.Id,
                WorkOrderId = manuSfcInfoEntity?.WorkOrderId ?? 0,
                ProductId = manuSfcInfoEntity?.ProductId ?? 0,
                ProcessRouteId = planWorkOrderEntity?.ProcessRouteId,
                ProductBOMId = planWorkOrderEntity?.ProductBOMId,
                IsUsed = true,
                CreatedBy = _currentUser.UserName,
                UpdatedBy = _currentUser.UserName,
            };

            if (ManuSfcStatus.SfcStatusInProcess.Contains(manuSfcEntity.Status))
            {
                if (manuSfcProduceEntity != null)
                {
                    addManuSfcProduceEntity = new ManuSfcProduceEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = _currentSite.SiteId ?? 0,
                        SFC = newSplitSFC,
                        SFCId = manuSfc.Id,
                        ProductId = manuSfcProduceEntity.ProductId,
                        WorkOrderId = manuSfcProduceEntity.WorkOrderId,
                        BarCodeInfoId = manuSfcInfo.Id,
                        ProcessRouteId = manuSfcProduceEntity.ProcessRouteId,
                        WorkCenterId = manuSfcProduceEntity.WorkCenterId,
                        ProductBOMId = manuSfcProduceEntity.ProductBOMId,
                        EquipmentId = manuSfcProduceEntity.EquipmentId,
                        ResourceId = manuSfcProduceEntity.ResourceId,
                        Qty = param.Qty,
                        ProcedureId = manuSfcProduceEntity.ProcedureId,
                        Status = manuSfcProduceEntity.Status,
                        RepeatedCount = manuSfcProduceEntity.RepeatedCount,
                        IsScrap = manuSfcProduceEntity.IsScrap,
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName,
                    };
                }
            }

            sfcStepList.Add(new ManuSfcStepEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = _currentSite.SiteId ?? 0,
                SFC = newSplitSFC,
                ProductId = manuSfcInfo.ProductId,
                WorkOrderId = manuSfcInfo.WorkOrderId ?? 0,
                WorkCenterId = manuSfcProduceEntity?.WorkCenterId,
                ProductBOMId = manuSfcProduceEntity?.ProductBOMId,
                ProcedureId = manuSfcProduceEntity?.ProcedureId,
                Qty = param.Qty,
                Operatetype = ManuSfcStepTypeEnum.SplitCreate,
                CurrentStatus = manuSfc.Status,
                EquipmentId = manuSfcProduceEntity?.EquipmentId,
                ResourceId = manuSfcProduceEntity?.ResourceId,
                CreatedBy = _currentUser.UserName,
                CreatedOn = HymsonClock.Now(),
                UpdatedBy = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now(),
            });

            if (whMaterialInventoryEntity != null && manuSfcEntity.Status == SfcStatusEnum.Complete)
            {
                // 物料库存
                addWhMaterialInventoryEntity = new WhMaterialInventoryEntity
                {
                    SupplierId = whMaterialInventoryEntity.SupplierId,//自制品 没有
                    MaterialId = manuSfcInfo?.ProductId ?? 0,
                    MaterialBarCode = newSplitSFC,
                    //Batch = "",//自制品 没有
                    MaterialType = whMaterialInventoryEntity.MaterialType,
                    QuantityResidue = param.Qty,
                    Status = whMaterialInventoryEntity.Status,
                    Source = whMaterialInventoryEntity.Source,
                    SiteId = _currentSite.SiteId ?? 0,
                    Id = IdGenProvider.Instance.CreateId(),
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now()
                };
                whMaterialStandingbookEntity = new WhMaterialStandingbookEntity
                {
                    MaterialCode = procMaterialEntitity?.MaterialCode ?? "",
                    MaterialName = procMaterialEntitity?.MaterialName ?? "",
                    MaterialVersion = procMaterialEntitity?.Version ?? "",
                    MaterialBarCode = newSplitSFC,
                    //Batch = "",//自制品 没有
                    Quantity = param.Qty,
                    Unit = procMaterialEntitity?.Unit ?? "",
                    Type = WhMaterialInventoryTypeEnum.Split,
                    Source = MaterialInventorySourceEnum.ManuComplete,
                    SiteId = _currentSite.SiteId ?? 0,
                    Id = IdGenProvider.Instance.CreateId(),
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now()
                };

                updateQuantityRangeCommand = new UpdateQuantityRangeCommand
                {
                    Status = whMaterialInventoryEntity.Status,
                    BarCode = param.SFC,
                    QuantityResidue = param.Qty,
                    UpdatedBy = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now()
                };
            }

            //TODO 2023 12 08 暂无继承业务  放在后续优化 

            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                if (updateStatusAndQtyByIdCommand != null)
                {
                    var row = await _manuSfcRepository.UpdateStatusAndQtyByIdAsync(updateStatusAndQtyByIdCommand);
                    if (row != 1)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES12832));
                    }
                    if (manuSfcProduceEntity != null)
                    {
                        await _manuSfcProduceRepository.DeletePhysicalByIdSqlAsync(manuSfcProduceEntity.Id);
                    }
                }
                if (updateManuSfcQtyAndCurrentQtyVerifyByIdCommand != null)
                {

                    var row = await _manuSfcRepository.UpdateManuSfcQtyAndCurrentQtyVerifyByIdAsync(updateManuSfcQtyAndCurrentQtyVerifyByIdCommand);

                    if (row != 1)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES12832));
                    }

                    if (updateManuSfcProduceQtyByIdCommand != null)
                    {
                        await _manuSfcProduceRepository.UpdateQtyByIdAsync(updateManuSfcProduceQtyByIdCommand);
                    }
                }
                if (sfcStepList != null && sfcStepList.Any())
                {
                    await _manuSfcStepRepository.InsertRangeAsync(sfcStepList);
                }
                await _manuSfcRepository.InsertAsync(manuSfc);
                if (manuSfcInfo != null)
                {
                    await _manuSfcInfoRepository.InsertAsync(manuSfcInfo);
                }
                if (addManuSfcProduceEntity != null)
                {
                    await _manuSfcProduceRepository.InsertAsync(addManuSfcProduceEntity);
                }

                if (addWhMaterialInventoryEntity != null)
                {
                    await _whMaterialInventoryRepository.InsertAsync(addWhMaterialInventoryEntity);
                }

                if (whMaterialStandingbookEntity != null)
                {
                    await _whMaterialStandingbookRepository.InsertAsync(whMaterialStandingbookEntity);
                }

                if (updateQuantityRangeCommand != null)
                {
                    await _whMaterialInventoryRepository.UpdateReduceQuantityResidueAsync(updateQuantityRangeCommand);
                }

                ts.Complete();
            }
            return newSplitSFC;
        }

        /// <summary>
        /// 更改条码数量
        /// </summary>
        /// <param name="adjustDto"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        public async Task BarcodeQtyAdjustAsync(ManuBarcodeQtyAdjustDto adjustDto)
        {
            if (adjustDto == null || string.IsNullOrEmpty(adjustDto.SFC) || adjustDto.Qty <= 0) throw new CustomerValidationException(nameof(ErrorCode.MES12823));

            if (!string.IsNullOrEmpty(adjustDto.Remark) && adjustDto.Remark.Length > 255) throw new CustomerValidationException(nameof(ErrorCode.MES12827));

            await VerifySfcsAsync(new string[] { adjustDto.SFC }, new ManuSfcsVerifyConditions
            {
                NotAllowStatus = new SfcStatusEnum[] { SfcStatusEnum.Scrapping, SfcStatusEnum.Delete, SfcStatusEnum.Locked, SfcStatusEnum.Complete, SfcStatusEnum.Invalid },
                IsBanNgSfc = true,
                NotAllowWorkOrderStatus = new PlanWorkOrderStatusEnum[] { PlanWorkOrderStatusEnum.Pending, PlanWorkOrderStatusEnum.Closed },

                IsVerifyMaterialQuantityLimit = true
            });

            //查询数据
            //查询出当前条码信息
            var manuSfcAboutInfoView = await _manuSfcRepository.GetManSfcAboutInfoBySfcAsync(new ManuSfcAboutInfoBySfcQuery()
            {
                SiteId = _currentSite.SiteId ?? 0,
                Sfc = adjustDto.SFC
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES12802)).WithData("sfc", adjustDto.SFC);

            //变化数量
            var changeQty = adjustDto.Qty - manuSfcAboutInfoView.Qty;
            if (changeQty == 0) throw new CustomerValidationException(nameof(ErrorCode.MES12826));

            #region 查询当前工单的可下发数量
            //查询当前条码对应工单
            var workOrder = await _planWorkOrderRepository.GetByIdAsync(manuSfcAboutInfoView.WorkOrderId) ?? throw new CustomerValidationException(nameof(ErrorCode.MES12824)).WithData("sfc", adjustDto.SFC);

            // 应下达数量
            var residue = workOrder.Qty * (1 + workOrder.OverScale / 100);

            // 查询已下发数量
            var workOrderRecordEntity = await _planWorkOrderRepository.GetByWorkOrderIdAsync(workOrder.Id);
            if (workOrderRecordEntity != null && workOrderRecordEntity.PassDownQuantity.HasValue)
            {
                // 减掉已下达数量
                residue -= workOrderRecordEntity.PassDownQuantity.Value;
            }

            if (residue < 0) residue = 0;

            #endregion

            //判断是否是在制
            var isProduce = false;
            var sfcProduces = await _manuSfcProduceRepository.GetListBySfcsAsync(new ManuSfcProduceBySfcsQuery() { SiteId = _currentSite.SiteId ?? 0, Sfcs = new string[] { adjustDto.SFC } });
            if (sfcProduces != null && sfcProduces.Any())
            {
                isProduce = true;
            }

            #region 组装数据
            var updateManuSfcQtyByIdCommands = new List<UpdateManuSfcQtyByIdCommand>();
            UpdatePassDownQuantityCommand? updatePassDownQuantityCommand = null;
            var updateSfcProcedureQtyByIdCommands = new List<UpdateSfcProcedureQtyByIdCommand>();
            var manuSfcStepEntities = new List<ManuSfcStepEntity>();

            //判断是否需要修改可下单数据，如果物料ID一直则修改
            if (manuSfcAboutInfoView.ProductId == workOrder.ProductId)
            {
                //如果数量增加超过可下达数量
                if (changeQty > 0 && changeQty > residue)
                    throw new CustomerValidationException(nameof(ErrorCode.MES12825));

                //扣减/增可下达数量
                updatePassDownQuantityCommand = new UpdatePassDownQuantityCommand
                {
                    WorkOrderId = workOrder.Id,
                    PlanQuantity = workOrder.Qty * (1 + workOrder.OverScale / 100),
                    PassDownQuantity = changeQty,//再次下达的数量
                    UserName = _currentUser.UserName,
                    UpdateDate = HymsonClock.Now()
                };
            }

            //更新产品序列码数量
            updateManuSfcQtyByIdCommands.Add(new UpdateManuSfcQtyByIdCommand
            {
                Id = manuSfcAboutInfoView.Id,
                Qty = adjustDto.Qty,
            });

            //在制执行更改在制表信息： ps  基本上应该都是在制，上方验证条码状态就只有 排队与完成-在制
            if (isProduce)
            {
                updateSfcProcedureQtyByIdCommands.Add(new UpdateSfcProcedureQtyByIdCommand
                {
                    Id = sfcProduces?.FirstOrDefault(x => x.SFC == adjustDto.SFC)?.Id ?? 0,
                    Qty = adjustDto.Qty,

                    UpdatedBy = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now()
                });
            }

            manuSfcStepEntities.Add(new ManuSfcStepEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = _currentSite.SiteId ?? 0,
                SFC = adjustDto.SFC,
                ProductId = manuSfcAboutInfoView.ProductId,
                WorkOrderId = manuSfcAboutInfoView.WorkOrderId,
                ProductBOMId = isProduce ? sfcProduces?.FirstOrDefault()?.ProductBOMId : workOrder?.ProductBOMId,
                WorkCenterId = isProduce ? sfcProduces?.FirstOrDefault()?.WorkCenterId : null,
                Qty = adjustDto.Qty,
                ResourceId = isProduce ? sfcProduces?.FirstOrDefault()?.ResourceId : null,
                ProcedureId = isProduce ? sfcProduces?.FirstOrDefault()?.ProcedureId : null,
                Operatetype = ManuSfcStepTypeEnum.SfcQtyAdjust,
                CurrentStatus = manuSfcAboutInfoView.Status,
                CreatedBy = _currentUser.UserName,
                UpdatedBy = _currentUser.UserName,
                Remark = adjustDto.Remark
            }); ;

            #endregion

            #region 操作数据库
            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                //更新manu_sfc 条码的数量
                await _manuSfcRepository.UpdateManuSfcQtyByIdRangeAsync(updateManuSfcQtyByIdCommands);

                //更新在制的数量
                if (updateSfcProcedureQtyByIdCommands.Any()) await _manuSfcProduceRepository.UpdateQtyRangeAsync(updateSfcProcedureQtyByIdCommands);

                //更新可下达数量
                if (updatePassDownQuantityCommand != null) await _planWorkOrderRepository.UpdatePassDownQuantityByWorkOrderIdAsync(updatePassDownQuantityCommand);

                //步骤记录
                if (manuSfcStepEntities.Any()) await _manuSfcStepRepository.InsertRangeAsync(manuSfcStepEntities);

                ts.Complete();
            }
            #endregion
        }

        /// <summary>
        /// 条码验证
        /// </summary>
        /// <param name="sfcs"></param>
        /// <param name="verifyConditions"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        /// <exception cref="ValidationException"></exception>
        private async Task<bool> VerifySfcsAsync(string[] sfcs, ManuSfcsVerifyConditions? verifyConditions = null)
        {
            if (sfcs == null || sfcs.Length == 0) throw new CustomerValidationException(nameof(ErrorCode.MES12801));

            var distinctSfcs = sfcs.Distinct();//去重的条码

            //1. 在条码信息（manu_sfc）中查询是否有数据
            //var manuSfcs= await _manuSfcRepository.GetBySFCsAsync(distinctSfcs);
            var manuSfcs = await _manuSfcRepository.GetManuSfcInfoEntitiesAsync(new ManuSfcStatusQuery()
            {
                SiteId = _currentSite.SiteId ?? 0,
                Sfcs = distinctSfcs.ToArray()
            });
            if (manuSfcs.Count() != distinctSfcs.Count())
            {
                var lackSfcs = distinctSfcs.Except(manuSfcs.Select(x => x.SFC));

                throw new CustomerValidationException(nameof(ErrorCode.MES12802)).WithData("sfc", string.Join(",", lackSfcs));
            }

            //其他验证
            if (verifyConditions == null)
                return true;

            //是否验证不允许的条码状态
            if (verifyConditions.NotAllowStatus != null && verifyConditions.NotAllowStatus.Length > 0)
            {
                // 检查状态
                var notAllowSfcs = manuSfcs.Where(x => verifyConditions.NotAllowStatus.Any(y => y == x.Status));
                if (notAllowSfcs != null && notAllowSfcs.Any())
                {
                    var validationFailures = new List<ValidationFailure>();
                    foreach (var item in notAllowSfcs)
                    {
                        var validationFailure = new ValidationFailure() { FormattedMessagePlaceholderValues = new() };
                        //validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", item.SFC);
                        validationFailure.FormattedMessagePlaceholderValues.Add("SFC", item.SFC);
                        validationFailure.FormattedMessagePlaceholderValues.Add("Current", _localizationService.GetSFCStatusEnumDescription(item.Status));
                        validationFailure.ErrorCode = nameof(ErrorCode.MES12805);
                        validationFailures.Add(validationFailure);
                    }
                    if (validationFailures.Any())
                    {
                        throw new ValidationException("", validationFailures);
                    }
                }
            }

            //是否验证禁止ng的条码
            if (verifyConditions.IsBanNgSfc)
            {
                var sfcProduces = await _manuSfcProduceRepository.GetListBySfcsAsync(new ManuSfcProduceBySfcsQuery() { SiteId = _currentSite.SiteId ?? 0, Sfcs = distinctSfcs });

                var repairsSfcProduces = sfcProduces.Where(x => x.IsRepair == TrueOrFalseEnum.Yes);
                if (sfcProduces != null && repairsSfcProduces != null && repairsSfcProduces.Any())
                {
                    var procProcedureEntitys = await _procProcedureRepository.GetByIdsAsync(repairsSfcProduces.Select(x => x.ProcedureId));
                    throw new CustomerValidationException(nameof(ErrorCode.MES12834)).WithData("sfc", string.Join(",", repairsSfcProduces.Select(x => x.SFC))).WithData("ProcedureCode", string.Join(",", procProcedureEntitys.Select(x => x.Code)));
                }

                //查询是否ng
                var ngSfcs = await _manuProductBadRecordRepository.GetManuProductBadRecordEntitiesBySFCAsync(new ManuProductBadRecordBySfcQuery()
                {
                    SiteId = _currentSite.SiteId ?? 0,
                    Status = Core.Enums.Manufacture.ProductBadRecordStatusEnum.Open,
                    SFCs = distinctSfcs,
                });
                if (ngSfcs != null && ngSfcs.Any())
                {
                    var needJudgeNgs = ngSfcs.Where(x => x.DisposalResult == ProductBadDisposalResultEnum.WaitingJudge);

                    if (needJudgeNgs.Any())
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES12838)).WithData("sfc", string.Join(",", needJudgeNgs.Select(x => x.SFC).Distinct()));
                    }

                    throw new CustomerValidationException(nameof(ErrorCode.MES12839)).WithData("sfc", string.Join(",", ngSfcs.Select(x => x.SFC).Distinct()));
                }


            }

            //是否验证 不允许的工单状态
            if (verifyConditions.NotAllowWorkOrderStatus != null && verifyConditions.NotAllowWorkOrderStatus.Length > 0)
            {
                //查询 工单 
                var workOrders = await _planWorkOrderRepository.GetByIdsAsync(manuSfcs.Select(x => x.WorkOrderId).ToList());
                var notAllowWorkOrders = workOrders.Where(x => verifyConditions.NotAllowWorkOrderStatus.Any(y => y == x.Status));
                if (notAllowWorkOrders != null && notAllowWorkOrders.Any())
                {
                    var notAllowWorkOrderSfc = from notAllowWorkOrder in notAllowWorkOrders
                                               join manuSfc in manuSfcs on notAllowWorkOrder.Id equals manuSfc.WorkOrderId
                                               select new { SFC = manuSfc.SFC, WorkOrderCode = notAllowWorkOrder.OrderCode, WorkOrderStatus = notAllowWorkOrder.Status };

                    var validationFailures = new List<ValidationFailure>();
                    foreach (var item in notAllowWorkOrderSfc)
                    {
                        var validationFailure = new ValidationFailure() { FormattedMessagePlaceholderValues = new() };
                        //validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", item.SFC);
                        validationFailure.FormattedMessagePlaceholderValues.Add("SFC", item.SFC);
                        validationFailure.FormattedMessagePlaceholderValues.Add("WorkOrderCode", item.WorkOrderCode);
                        validationFailure.FormattedMessagePlaceholderValues.Add("WorkOrderStatus", _localizationService.GetResource($"{typeof(PlanWorkOrderStatusEnum).FullName}.{item.WorkOrderStatus.ToString()}"));
                        validationFailure.ErrorCode = nameof(ErrorCode.MES12807);
                        validationFailures.Add(validationFailure);
                    }
                    if (validationFailures.Any())
                    {
                        throw new ValidationException("", validationFailures);
                    }
                }
            }

            //是否验证 不允许绑定容器
            if (verifyConditions.IsVerifyBindContainer)
            {
                //查询是否装箱
                //var containerBarcodes = await _manuContainerBarcodeRepository.GetByCodesAsync(new ManuContainerBarcodeQuery
                //{
                //    SiteId = _currentSite.SiteId ?? 0,
                //    BarCodes = distinctSfcs.ToArray(),
                //});
                var containerPacks = await _manuContainerPackRepository.GetByLadeBarCodesAsync(new ManuContainerPackQuery { SiteId = _currentSite.SiteId ?? 0, LadeBarCodes = distinctSfcs });
                if (containerPacks != null && containerPacks.Any())
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES12804)).WithData("sfc", string.Join(",", containerPacks.Select(x => x.LadeBarCode)));
                }
            }

            //是否验证 不允许绑定载具
            if (verifyConditions.IsVerifyBindVehicle)
            {
                //查询是否被载具绑定
                var vehicleFreightStacks = await _inteVehiceFreightStackRepository.GetInteVehiceFreightStackEntitiesAsync(new InteVehiceFreightStackQuery
                {
                    SiteId = _currentSite.SiteId ?? 0,
                    Sfcs = distinctSfcs,
                });
                if (vehicleFreightStacks != null && vehicleFreightStacks.Any())
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES12803)).WithData("sfc", string.Join(",", vehicleFreightStacks.Select(x => x.BarCode)));
                }
            }

            //检查物料的 数量限制 是否是 仅1.0
            if (verifyConditions.IsVerifyMaterialQuantityLimit)
            {
                var materials = await _procMaterialRepository.GetByIdsAsync(manuSfcs.Select(x => x.ProductId).ToList());
                if (materials.Any(x => x.QuantityLimit == MaterialQuantityLimitEnum.OnlyOne)) throw new CustomerValidationException(nameof(ErrorCode.MES12836));
            }

            //是否验证 工单必相同
            if (sfcs.Length > 1 && verifyConditions.IsVerifySameWorkOrder)
            {
                var sfcsGroupByWorkOrders = manuSfcs.GroupBy(x => x.WorkOrderId);
                if (sfcsGroupByWorkOrders.Count() > 1)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES12808));
                }
            }

            //是否验证 物料必相同
            if (sfcs.Length > 1 && verifyConditions.IsVerifySameMaterial)
            {
                var sfcsGroupByProducts = manuSfcs.GroupBy(x => x.ProductId);
                if (sfcsGroupByProducts.Count() > 1)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES12809));
                }
            }

            //是否验证 物料清单必相同
            if (sfcs.Length > 1 && verifyConditions.IsVerifySameBom)
            {
                var sfcsGroupByBoms = manuSfcs.GroupBy(x => x.ProductBOMId);
                if (sfcsGroupByBoms.Count() > 1)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES12810));
                }
            }

            //是否验证 工艺路线必相同
            if (sfcs.Length > 1 && verifyConditions.IsVerifySameProcessRoute)
            {
                var sfcsGroupByProcessRoutes = manuSfcs.GroupBy(x => x.ProcessRouteId);
                if (sfcsGroupByProcessRoutes.Count() > 1)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES12811));
                }
            }

            //验证 条码状态相同
            if (sfcs.Length > 1 && verifyConditions.IsVerifySameSfcStatus)
            {
                var sfcsGroupByStatuss = manuSfcs.GroupBy(x => x.Status);
                if (sfcsGroupByStatuss.Count() > 1)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES12813));
                }
            }

            return true;
        }

        /// <summary>
        /// 根据条码获取数据
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        private async Task<ManuSfcAboutInfoViewDto?> GetSfcAboutInfoBySfcAsync(string sfc)
        {
            //1查询数据
            var manuSfcAboutInfoView = await _manuSfcRepository.GetManSfcAboutInfoBySfcAsync(new ManuSfcAboutInfoBySfcQuery()
            {
                SiteId = _currentSite.SiteId ?? 0,
                Sfc = sfc
            });

            if (manuSfcAboutInfoView == null) return null;

            //2查询物料
            var material = await _procMaterialRepository.GetByIdAsync(manuSfcAboutInfoView.ProductId);

            //3查询工艺路线
            var procProcessRoute = await _procProcessRouteRepository.GetByIdAsync(manuSfcAboutInfoView.ProcessRouteId);

            //查询bom
            var bom = await _procBomRepository.GetByIdAsync(manuSfcAboutInfoView.ProductBomId);

            //4查询工序
            //查询条码在制表
            var sfcProduce = await _manuSfcProduceRepository.GetBySFCAsync(new ManuSfcProduceBySfcQuery()
            {
                SiteId = _currentSite.SiteId ?? 0,
                Sfc = sfc
            });
            var procedure = sfcProduce != null ? await _procProcedureRepository.GetByIdAsync(sfcProduce.ProcedureId) : null;

            //5查询载具
            //查询载具装条码信息
            var vehicleStack = await _inteVehiceFreightStackRepository.GetBySFCAsync(new InteVehiceFreightStackBySfcQuery()
            {
                SiteId = _currentSite.SiteId ?? 0,
                BarCode = sfc
            });

            var vehicle = vehicleStack != null ? await _inteVehicleRepository.GetByIdAsync(vehicleStack.VehicleId) : null;

            //组装数据
            var manuSfcAboutInfoViewDto = manuSfcAboutInfoView.ToModel<ManuSfcAboutInfoViewDto>();
            manuSfcAboutInfoViewDto.VehicleCode = vehicle?.Code ?? "";
            manuSfcAboutInfoViewDto.ProcedureCode = procedure?.Code ?? "";
            manuSfcAboutInfoViewDto.MaterialCodeVersion = material == null ? "" : $"{material.MaterialCode}/{material.Version}";
            manuSfcAboutInfoViewDto.ProcessRouteCodeVersion = procProcessRoute == null ? "" : $"{procProcessRoute.Code}/{procProcessRoute.Version}";
            manuSfcAboutInfoViewDto.BomCodeVersion = bom == null ? "" : $"{bom.BomCode}/{bom.Version}";
            manuSfcAboutInfoViewDto.MaterialCode = material?.MaterialCode ?? "";
            manuSfcAboutInfoViewDto.MaterialName = material?.MaterialName ?? "";
            manuSfcAboutInfoViewDto.MaterialVersion = material?.Version ?? "";
            return manuSfcAboutInfoViewDto;
        }

        private async Task<IEnumerable<BarCodeInfo>> GenerateBarcodeByproductId(long productId)
        {
            var inteCodeRulesEntity = await _inteCodeRulesRepository.GetInteCodeRulesByProductIdAsync(new InteCodeRulesByProductQuery
            {
                ProductId = productId,
                CodeType = CodeRuleCodeTypeEnum.ProcessControlSeqCode
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES12820));

            var codeRulesMakeList = await _inteCodeRulesMakeRepository.GetInteCodeRulesMakeEntitiesAsync(new InteCodeRulesMakeQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                CodeRulesId = inteCodeRulesEntity.Id
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES12821));

            //生成子条码
            var barcodeList = await _manuGenerateBarcodeService.GenerateBarCodeSerialNumberReturnBarCodeInfosAsync(new BarCodeSerialNumberBo
            {
                IsTest = false,
                IsSimulation = false,
                CodeRulesMakeBos = codeRulesMakeList.Select(s => new CodeRulesMakeBo
                {
                    Seq = s.Seq,
                    ValueTakingType = s.ValueTakingType,
                    SegmentedValue = s.SegmentedValue,
                    CustomValue = s.CustomValue,
                }),

                CodeRuleKey = $"{inteCodeRulesEntity.Id}",
                Count = 1,
                Base = inteCodeRulesEntity.Base,
                Increment = inteCodeRulesEntity.Increment,
                IgnoreChar = inteCodeRulesEntity.IgnoreChar ?? string.Empty,
                OrderLength = inteCodeRulesEntity.OrderLength,
                ResetType = inteCodeRulesEntity.ResetType,
                StartNumber = inteCodeRulesEntity.StartNumber,
                CodeMode = inteCodeRulesEntity.CodeMode,
                SiteId = _currentSite.SiteId ?? 0
            });

            return barcodeList;
        }
    }
}
