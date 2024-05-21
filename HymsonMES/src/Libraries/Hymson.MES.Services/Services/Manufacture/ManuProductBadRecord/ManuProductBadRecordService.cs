using FluentValidation;
using FluentValidation.Results;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Excel.Abstractions;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Constants.Manufacture;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums.QualUnqualifiedCode;
using Hymson.MES.Core.Enums.Warehouse;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuCommon;
using Hymson.Minio;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 产品不良录入 服务
    /// </summary>
    public class ManuProductBadRecordService : IManuProductBadRecordService
    {
        /// <summary>
        /// 当前对象（登录用户）
        /// </summary>
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 当前对象（站点）
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 仓储（在制品业务）
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 条码表 仓储
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        /// <summary>
        /// 条码步骤表仓储 仓储
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

        /// <summary>
        /// 产品不良录入 仓储
        /// </summary>
        private readonly IManuProductBadRecordRepository _manuProductBadRecordRepository;

        /// <summary>
        /// 不合格代码仓储
        /// </summary>
        private readonly IQualUnqualifiedCodeRepository _qualUnqualifiedCodeRepository;

        /// <summary>
        /// 生产公共服务
        /// </summary>
        private readonly IManuCommonOldService _manuCommonOldService;


        /// <summary>
        /// 条码信息表  仓储
        /// </summary>
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly ILocalizationService _localizationService;

        /// <summary>
        /// 工单激活 仓储
        /// </summary>
        private readonly IPlanWorkOrderActivationRepository _planWorkOrderActivationRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly AbstractValidator<ManuProductBadRecordModifyDto> _validationModifyRules;

        /// <summary>
        /// 工单信息表 仓储
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        /// 库存信息表仓储
        /// </summary>
        private readonly IWhMaterialInventoryRepository _whMaterialInventoryRepository;

        /// <summary>
        /// 报废表 仓储
        /// </summary>
        private readonly IManuSfcScrapRepository _manuSfcScrapRepository;

        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 仓储接口（载具注册）
        /// </summary>
        private readonly IInteVehicleRepository _inteVehicleRepository;

        /// <summary>
        /// 仓储接口（二维载具条码明细）
        /// </summary>
        private readonly IInteVehiceFreightStackRepository _inteVehiceFreightStackRepository;

        private readonly IProcProcedureRepository _procProcedureRepository;

        private readonly IExcelService _excelService;
        private readonly IMinioService _minioService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ManuProductBadRecordService(ICurrentUser currentUser, ICurrentSite currentSite,
                IManuSfcProduceRepository manuSfcProduceRepository,
                IManuSfcRepository manuSfcRepository,
                IManuSfcStepRepository manuSfcStepRepository,
                IManuProductBadRecordRepository manuProductBadRecordRepository,
                IQualUnqualifiedCodeRepository qualUnqualifiedCodeRepository,
                IManuSfcInfoRepository manuSfcInfoRepository,
                ILocalizationService localizationService,
                IManuCommonOldService manuCommonOldService,
                IPlanWorkOrderActivationRepository planWorkOrderActivationRepository,
                IPlanWorkOrderRepository planWorkOrderRepository,
                IWhMaterialInventoryRepository whMaterialInventoryRepository,
                IManuSfcScrapRepository manuSfcScrapRepository,
                IProcMaterialRepository procMaterialRepository,
                AbstractValidator<ManuProductBadRecordModifyDto> validationModifyRules,
                IInteVehicleRepository inteVehicleRepository,
                IInteVehiceFreightStackRepository inteVehiceFreightStackRepository, IProcProcedureRepository procProcedureRepository, IExcelService excelService, IMinioService minioService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _manuProductBadRecordRepository = manuProductBadRecordRepository;
            _qualUnqualifiedCodeRepository = qualUnqualifiedCodeRepository;
            _manuCommonOldService = manuCommonOldService;
            _validationModifyRules = validationModifyRules;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _localizationService = localizationService;
            _planWorkOrderRepository = planWorkOrderRepository;
            _planWorkOrderActivationRepository = planWorkOrderActivationRepository;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _procMaterialRepository = procMaterialRepository;
            _manuSfcScrapRepository = manuSfcScrapRepository;
            _inteVehicleRepository = inteVehicleRepository;
            _inteVehiceFreightStackRepository = inteVehiceFreightStackRepository;
            _procProcedureRepository = procProcedureRepository;
            _excelService = excelService;
            _minioService = minioService;
        }

        /// <summary>
        /// 产品不良录入
        /// </summary>
        /// <param name="manuProductBadRecordCreateDto"></param>
        /// <returns></returns>
        public async Task CreateManuProductBadRecordAsync(ManuProductBadRecordCreateDto manuProductBadRecordCreateDto)
        {
            // 验证DTO
            if (manuProductBadRecordCreateDto.Sfcs == null || manuProductBadRecordCreateDto.Sfcs.Length < 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15400));
            }
            //条码表
            var sfcEntities = await _manuSfcRepository.GetListAsync(new ManuSfcQuery
            {
                SFCs = manuProductBadRecordCreateDto.Sfcs,
                SiteId = _currentSite.SiteId ?? 0,
                Type = SfcTypeEnum.Produce
            });
            //条码信息表
            var sfcInfoEntities = await _manuSfcInfoRepository.GetBySFCIdsWithIsUseAsync(sfcEntities.Select(x => x.Id));
            var manuSfcProducePagedQuery = new ManuSfcProduceQuery { Sfcs = manuProductBadRecordCreateDto.Sfcs, SiteId = _currentSite.SiteId ?? 00 };
            //在制品信息
            var manuSfcProduces = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(manuSfcProducePagedQuery);

            // 获取不合格代码列表
            var qualUnqualifiedCodes = await _qualUnqualifiedCodeRepository.GetByIdsAsync(manuProductBadRecordCreateDto.UnqualifiedIds);


            //工单激活信息
            var orderIds = sfcInfoEntities.Select(x => x.WorkOrderId ?? 0).Distinct().ToArray();
            var activeOrders = await _planWorkOrderActivationRepository.GetByWorkOrderIdsAsync(orderIds);
            //工单信息
            var planWorkOrders = await _planWorkOrderRepository.GetByIdsAsync(orderIds);
            //库存信息
            var whMaterialInventoryEntities = await _whMaterialInventoryRepository.GetByBarCodesOfHasQtyAsync(
                new WhMaterialInventoryBarCodesQuery()
                {
                    SiteId = _currentSite.SiteId ?? 0,
                    BarCodes = manuProductBadRecordCreateDto.Sfcs
                }
                );

            var validationFailures = new List<ValidationFailure>();
            var manuSfcUpdateStatusByIdCommands = new List<ManuSfcUpdateStatusByIdCommand>();
            var sfcStepList = new List<ManuSfcStepEntity>();
            var manuProductBadRecords = new List<ManuProductBadRecordEntity>();
            var manuSfcProduceBusinessList = new List<ManuSfcProduceBusinessEntity>();
            var manuSfcUpdateRouteByIdCommands = new List<ManuSfcUpdateRouteByIdCommand>();
            var updateWhMaterialInventoryEmptyByIdCommands = new List<UpdateWhMaterialInventoryEmptyByIdCommand>();
            var manuSfcScrapEntities = new List<ManuSfcScrapEntity>();
            var manuSfcProduceList = new List<ManuSfcProduceEntity>();
            var scrapByIdCommands = new List<ScrapManuSfcByIdCommand>();
            var sfcList = new List<string>();
            var updateStatusByBarCodeCommands = new List<UpdateStatusByBarCodeCommand>();
            var updateManuSfcProduceStatusByIdCommands = new List<UpdateManuSfcProduceStatusByIdCommand>();
            bool isScrap = qualUnqualifiedCodes.Any(x => x.UnqualifiedCode.ToUpperInvariant() == ManuProductBadRecord.ScrapCode);
            Dtos.Manufacture.ManuMainstreamProcessDto.ManuCommonDto.ProcessRouteProcedureDto processRouteProcedure = new();
            if (qualUnqualifiedCodes.Any(x => x.Type == QualUnqualifiedCodeTypeEnum.Defect))
            {
                //首工序
                processRouteProcedure = await _manuCommonOldService.GetFirstProcedureAsync(manuProductBadRecordCreateDto.BadProcessRouteId ?? 0);
            }
            foreach (var sfc in manuProductBadRecordCreateDto.Sfcs)
            {
                var sfcEntity = sfcEntities.FirstOrDefault(x => x.SFC == sfc);

                if (sfcEntity == null)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", sfc}
                        };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", sfc);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15415);
                    validationFailures.Add(validationFailure);
                    continue;
                }

                //报废验证
                if (sfcEntity.Status == SfcStatusEnum.Scrapping)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", sfc}
                        };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", sfc);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15401);
                    validationFailures.Add(validationFailure);
                    continue;
                }

                //锁定验证
                if (sfcEntity.Status == SfcStatusEnum.Locked)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", sfc}
                        };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", sfc);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15416);
                    validationFailures.Add(validationFailure);
                    continue;
                }

                var sfcInfoEntity = sfcInfoEntities.FirstOrDefault(x => x.SfcId == sfcEntity.Id);

                if (sfcInfoEntity == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES15401));
                }

                var manuSfcProduceInfoEntity = manuSfcProduces.FirstOrDefault(x => x.SFC == sfc);

                //步骤表-不良录入
                var manuSfcStepEntity = new ManuSfcStepEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SFC = sfc,
                    ProductId = sfcInfoEntity.ProductId,
                    WorkOrderId = sfcInfoEntity.WorkOrderId ?? 0,
                    WorkCenterId = manuSfcProduceInfoEntity?.WorkCenterId,
                    ProductBOMId = manuSfcProduceInfoEntity?.ProductBOMId,
                    ProcessRouteId = manuSfcProduceInfoEntity?.ProcessRouteId,
                    Qty = sfcEntity.Qty,
                    EquipmentId = manuSfcProduceInfoEntity?.EquipmentId,
                    ResourceId = manuSfcProduceInfoEntity?.ResourceId,
                    ProcedureId = manuSfcProduceInfoEntity?.ProcedureId,
                    Operatetype = ManuSfcStepTypeEnum.BadEntry,
                    CurrentStatus = sfcEntity.Status,
                    SiteId = _currentSite.SiteId ?? 0,
                    CreatedOn = HymsonClock.Now(),
                    CreatedBy = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentUser.UserName,
                };
                sfcStepList.Add(manuSfcStepEntity);

                foreach (var unqualified in qualUnqualifiedCodes)
                {
                    //报废的不需要记录不良，不需要关闭和展示
                    if (unqualified.UnqualifiedCode.ToUpperInvariant() == ManuProductBadRecord.ScrapCode)
                    {
                        continue;
                    }

                    var manuProductBadRecordEntity = new ManuProductBadRecordEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = _currentSite.SiteId ?? 0,
                        FoundBadOperationId = manuProductBadRecordCreateDto.FoundBadOperationId,
                        OutflowOperationId = manuProductBadRecordCreateDto.OutflowOperationId,
                        UnqualifiedId = unqualified.Id,
                        SfcStepId = manuSfcStepEntity.Id,
                        SFC = sfc,
                        SfcInfoId = sfcInfoEntity.Id,
                        Qty = sfcEntity.Qty,
                        Source = ProductBadRecordSourceEnum.BadManualEntry,
                        Remark = manuProductBadRecordCreateDto.Remark ?? "",
                        CreatedOn = HymsonClock.Now(),
                        CreatedBy = _currentUser.UserName,
                        UpdatedOn = HymsonClock.Now(),
                        UpdatedBy = _currentUser.UserName,
                    };

                    if (isScrap)
                    {
                        manuProductBadRecordEntity.CloseOn = HymsonClock.Now();
                        manuProductBadRecordEntity.CloseBy = _currentUser.UserName;
                        manuProductBadRecordEntity.Status = ProductBadRecordStatusEnum.Close;
                        manuProductBadRecordEntity.DisposalResult = ProductBadDisposalResultEnum.Scrap;
                    }
                    else
                    {
                        manuProductBadRecordEntity.Status = ProductBadRecordStatusEnum.Open;
                        if (unqualified.Type == QualUnqualifiedCodeTypeEnum.Defect)
                        {
                            manuProductBadRecordEntity.DisposalResult = ProductBadDisposalResultEnum.Repair;
                        }
                    }

                    manuProductBadRecords.Add(manuProductBadRecordEntity);
                }

                //报废流程
                if (isScrap)
                {
                    var manuSfcScrapEntity = new ManuSfcScrapEntity()
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SFC = sfc,
                        SfcinfoId = sfcInfoEntity?.Id ?? 0,
                        SfcStepId = manuSfcStepEntity.Id,
                        ProcedureId = manuProductBadRecordCreateDto.FoundBadOperationId,
                        ScrapQty = sfcEntity.Qty,
                        IsCancel = false,
                        SiteId = _currentSite.SiteId ?? 0,
                        CreatedOn = HymsonClock.Now(),
                        CreatedBy = _currentUser.UserName,
                        UpdatedOn = HymsonClock.Now(),
                        UpdatedBy = _currentUser.UserName,
                    };
                    manuSfcScrapEntities.Add(manuSfcScrapEntity);

                    //在制品处理 修改为报废
                    if (manuSfcProduceInfoEntity != null)
                    {
                        updateManuSfcProduceStatusByIdCommands.Add(new UpdateManuSfcProduceStatusByIdCommand
                        {
                            Id = manuSfcProduceInfoEntity.Id,
                            Status = SfcStatusEnum.Scrapping,
                            CurrentStatus = manuSfcProduceInfoEntity.Status,
                            UpdatedOn = HymsonClock.Now(),
                            UpdatedBy = _currentSite.Name
                        });
                    }

                    //条码表 修改为报废
                    scrapByIdCommands.Add(new ScrapManuSfcByIdCommand
                    {
                        Id = sfcEntity.Id,
                        Status = SfcStatusEnum.Scrapping,
                        CurrentStatus = sfcEntity.Status,
                        SfcScrapId = manuSfcScrapEntity.Id
                    });

                    if (sfcEntity.Status == SfcStatusEnum.Complete)
                    {
                        //库存验证
                        var whMaterialInventoryEntity = whMaterialInventoryEntities.FirstOrDefault(x => x.MaterialBarCode == sfc);
                        if (whMaterialInventoryEntity == null)
                        {
                            var validationFailure = new ValidationFailure();
                            if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                            {
                                validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", sfc}
                             };
                            }
                            else
                            {
                                validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", sfc);
                            }
                            validationFailure.ErrorCode = nameof(ErrorCode.MES15421);
                            validationFailures.Add(validationFailure);
                            continue;
                        }
                        else
                        {
                            //库存锁定验证
                            if (whMaterialInventoryEntity.Status == WhMaterialInventoryStatusEnum.Locked)
                            {
                                var validationFailure = new ValidationFailure();
                                if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                                {
                                    validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", sfc}
                             };
                                }
                                else
                                {
                                    validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", sfc);
                                }
                                validationFailure.ErrorCode = nameof(ErrorCode.MES15422);
                                validationFailures.Add(validationFailure);
                                continue;
                            }
                            //库存报废
                            updateStatusByBarCodeCommands.Add(new UpdateStatusByBarCodeCommand
                            {
                                BarCode = sfc,
                                Quantity = whMaterialInventoryEntity.QuantityResidue,
                                Status = WhMaterialInventoryStatusEnum.Scrap,
                                UpdatedBy = _currentUser.UserName,
                                UpdatedOn = HymsonClock.Now()
                            });
                        }
                    }

                }
                else if (qualUnqualifiedCodes.Any(x => x.Type == QualUnqualifiedCodeTypeEnum.Defect))//缺陷处理流程
                {
                    //返修 更新条码
                    manuSfcUpdateStatusByIdCommands.Add(new ManuSfcUpdateStatusByIdCommand
                    {
                        Id = sfcEntity.Id,
                        Status = SfcStatusEnum.lineUp,
                        CurrentStatus = sfcEntity.Status,
                        UpdatedBy = _currentUser.UserName,
                        UpdatedOn = HymsonClock.Now()
                    });

                    //在制品处理
                    if (manuSfcProduceInfoEntity != null)
                    {
                        if ((manuSfcProduceInfoEntity.IsRepair ?? TrueOrFalseEnum.No) == TrueOrFalseEnum.Yes) continue;
                        //返工工艺路线与条码工艺路线不能一致
                        if (manuSfcProduceInfoEntity.ProcessRouteId == manuProductBadRecordCreateDto.BadProcessRouteId)
                        {
                            var validationFailure = new ValidationFailure();
                            if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                            {
                                validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", sfc}
                        };
                            }
                            else
                            {
                                validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", sfc);
                            }
                            validationFailure.ErrorCode = nameof(ErrorCode.MES15420);
                            validationFailures.Add(validationFailure);
                            continue;
                        }

                        // 添加维修在制品业务
                        var manuSfcProduceBusinessEntity = new ManuSfcProduceBusinessEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            SfcProduceId = manuSfcProduceInfoEntity.Id,
                            BusinessType = ManuSfcProduceBusinessType.Repair,
                            BusinessContent = JsonConvert.SerializeObject(new SfcProduceRepairBo
                            {
                                ProcessRouteId = manuSfcProduceInfoEntity.ProcessRouteId, //createDto.BadProcessRouteId ?? 0,
                                ProcedureId = manuSfcProduceInfoEntity.ProcedureId //processRouteProcedure.ProcedureId
                            }),
                            SiteId = _currentSite.SiteId ?? 0,
                            CreatedBy = _currentUser.UserName,
                            UpdatedBy = _currentUser.UserName
                        };

                        manuSfcProduceBusinessList.Add(manuSfcProduceBusinessEntity);

                        //更新工艺路线 为返工工艺路线  工序:返工工艺路线首工序排队  状态:排队  
                        manuSfcUpdateRouteByIdCommands.Add(new ManuSfcUpdateRouteByIdCommand
                        {
                            ProcessRouteId = manuProductBadRecordCreateDto.BadProcessRouteId ?? 0,
                            ProcedureId = processRouteProcedure.ProcedureId,
                            UpdatedBy = _currentUser.UserName,
                            UpdatedOn = HymsonClock.Now(),
                            Status = SfcStatusEnum.lineUp,
                            IsRepair = TrueOrFalseEnum.Yes,
                            Id = manuSfcProduceInfoEntity.Id
                        });
                    }
                    else
                    {
                        //已完成的改变 状态:在制  工序:返工工艺路线排队  
                        if (!activeOrders.Any(x => x.WorkOrderId == sfcInfoEntity.WorkOrderId))
                        {
                            var validationFailure = new ValidationFailure();
                            if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                            {
                                validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", sfc}
                             };
                            }
                            else
                            {
                                validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", sfc);
                            }
                            validationFailure.ErrorCode = nameof(ErrorCode.MES15418);
                            validationFailures.Add(validationFailure);
                            continue;
                        }

                        //库存验证
                        var whMaterialInventoryEntity = whMaterialInventoryEntities.FirstOrDefault(x => x.MaterialBarCode == sfc);
                        if (whMaterialInventoryEntity == null)
                        {
                            var validationFailure = new ValidationFailure();
                            if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                            {
                                validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", sfc}
                             };
                            }
                            else
                            {
                                validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", sfc);
                            }
                            validationFailure.ErrorCode = nameof(ErrorCode.MES15421);
                            validationFailures.Add(validationFailure);
                            continue;
                        }
                        else
                        {
                            //库存锁定
                            if (whMaterialInventoryEntity.Status == WhMaterialInventoryStatusEnum.Locked)
                            {
                                var validationFailure = new ValidationFailure();
                                if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                                {
                                    validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", sfc}
                             };
                                }
                                else
                                {
                                    validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", sfc);
                                }
                                validationFailure.ErrorCode = nameof(ErrorCode.MES15422);
                                validationFailures.Add(validationFailure);
                                continue;
                            }

                            updateWhMaterialInventoryEmptyByIdCommands.Add(new UpdateWhMaterialInventoryEmptyByIdCommand
                            {
                                Id = whMaterialInventoryEntity.Id,
                                UpdatedBy = _currentUser.UserName,
                                UpdatedOn = HymsonClock.Now()
                            });
                        }

                        var planWorkOrderEntity = planWorkOrders.FirstOrDefault();
                        if (planWorkOrderEntity != null)
                        {
                            var manuSfcProduceEntity = new ManuSfcProduceEntity
                            {
                                Id = IdGenProvider.Instance.CreateId(),
                                SiteId = _currentSite.SiteId ?? 0,
                                SFC = sfc,
                                SFCId = sfcEntity.Id,
                                ProductId = planWorkOrderEntity.ProductId,
                                WorkOrderId = planWorkOrderEntity.Id,
                                BarCodeInfoId = sfcInfoEntity.Id,
                                ProcessRouteId = manuProductBadRecordCreateDto.BadProcessRouteId ?? 0,
                                WorkCenterId = planWorkOrderEntity.WorkCenterId ?? 0,
                                ProductBOMId = planWorkOrderEntity.ProductBOMId,
                                Qty = whMaterialInventoryEntity.QuantityResidue,
                                ProcedureId = processRouteProcedure.ProcedureId,
                                Status = SfcStatusEnum.lineUp,
                                RepeatedCount = 0,
                                IsScrap = TrueOrFalseEnum.No,
                                CreatedOn = HymsonClock.Now(),
                                CreatedBy = _currentUser.UserName,
                                UpdatedOn = HymsonClock.Now(),
                                UpdatedBy = _currentUser.UserName,
                            };
                            manuSfcProduceList.Add(manuSfcProduceEntity);

                            // 添加维修在制品业务
                            var manuSfcProduceBusinessEntity = new ManuSfcProduceBusinessEntity
                            {
                                Id = IdGenProvider.Instance.CreateId(),
                                SfcProduceId = manuSfcProduceEntity.Id,
                                BusinessType = ManuSfcProduceBusinessType.Repair,
                                BusinessContent = JsonConvert.SerializeObject(new SfcProduceRepairBo
                                {
                                    ProcessRouteId = planWorkOrderEntity.ProcessRouteId,
                                    //ProcedureId = manuSfcProduceInfoEntity.ProcedureId 
                                }),
                                SiteId = _currentSite.SiteId ?? 0,
                                CreatedBy = _currentUser.UserName,
                                UpdatedBy = _currentUser.UserName
                            };
                            manuSfcProduceBusinessList.Add(manuSfcProduceBusinessEntity);
                        }
                    }
                }
            }

            if (validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("SFCError"), validationFailures);
            }

            using (var trans = TransactionHelper.GetTransactionScope())
            {
                //插入不合格记录
                if (manuProductBadRecords != null && manuProductBadRecords.Any())
                {
                    await _manuProductBadRecordRepository.InsertRangeAsync(manuProductBadRecords);
                }
                //插入步骤表
                if (sfcStepList != null && sfcStepList.Any())
                {
                    await _manuSfcStepRepository.InsertRangeAsync(sfcStepList);
                }

                if (qualUnqualifiedCodes.Any(x => x.UnqualifiedCode.ToUpperInvariant() == ManuProductBadRecord.ScrapCode))
                {
                    //插入报废信息
                    if (manuSfcScrapEntities != null && manuSfcScrapEntities.Any())
                    {
                        await _manuSfcScrapRepository.InsertRangeAsync(manuSfcScrapEntities);
                    }

                    //更新在制品状态
                    if (updateManuSfcProduceStatusByIdCommands != null && updateManuSfcProduceStatusByIdCommands.Any())
                    {
                        await _manuSfcProduceRepository.UpdateStatusByIdRangeAsync(updateManuSfcProduceStatusByIdCommands);
                    }

                    //修改条码状态
                    if (scrapByIdCommands != null && scrapByIdCommands.Any())
                    {
                        var row = await _manuSfcRepository.ManuSfcScrapByIdsAsync(scrapByIdCommands);
                        if (row != scrapByIdCommands.Count)
                        {
                            throw new CustomerValidationException(nameof(ErrorCode.MES15426));
                        }
                    }

                    //修改库存状态
                    if (updateStatusByBarCodeCommands != null && updateStatusByBarCodeCommands.Any())
                    {
                        await _whMaterialInventoryRepository.UpdatePointByBarCodeRangeAsync(updateStatusByBarCodeCommands);
                    }
                }
                else if (qualUnqualifiedCodes.Any(x => x.Type == QualUnqualifiedCodeTypeEnum.Defect))//缺陷处理流程
                {
                    //插入在制品表
                    if (manuSfcProduceList != null && manuSfcProduceList.Any())
                    {
                        await _manuSfcProduceRepository.InsertRangeAsync(manuSfcProduceList);
                    }

                    //更新条码状态
                    if (manuSfcUpdateStatusByIdCommands != null && manuSfcUpdateStatusByIdCommands.Any())
                    {

                        var row = await _manuSfcRepository.ManuSfcUpdateStatuByIdRangeAsync(manuSfcUpdateStatusByIdCommands);
                        if (row != manuSfcUpdateStatusByIdCommands.Count)
                        {
                            throw new CustomerValidationException(nameof(ErrorCode.MES15426));
                        }
                    }

                    //插入在制品业务表
                    if (manuSfcProduceBusinessList != null && manuSfcProduceBusinessList.Any())
                    {
                        await _manuSfcProduceRepository.InsertSfcProduceBusinessRangeAsync(manuSfcProduceBusinessList);
                    }

                    //更新在制品表
                    if (manuSfcUpdateRouteByIdCommands != null && manuSfcUpdateRouteByIdCommands.Any())
                    {
                        await _manuSfcProduceRepository.UpdateRouteByIdRangeAsync(manuSfcUpdateRouteByIdCommands);
                    }

                    //更新仓库
                    if (updateWhMaterialInventoryEmptyByIdCommands != null && updateWhMaterialInventoryEmptyByIdCommands.Any())
                    {
                        await _whMaterialInventoryRepository.UpdateWhMaterialInventoryEmptyByIdRangeAync(updateWhMaterialInventoryEmptyByIdCommands);
                    }
                }
                trans.Complete();
            }
        }

        /// <summary>
        /// 新增 (有条码类型)
        /// 给面板 特殊业务使用：多个载具编码
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        public async Task CreateManuProductBadRecordAboutCodeTypeAsync(FacePlateManuProductBadRecordCreateDto createDto)
        {
            //如果是 条码的情况还是走原来的 不良录入
            if (createDto.BarcodeType == ManuFacePlateBarcodeTypeEnum.Product)
            {
                await CreateManuProductBadRecordAsync(createDto);
                return;
            }

            //检验载具数据
            if (createDto.Sfcs == null || createDto.Sfcs.Length < 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15430));
            }

            #region 查询载具对应的条码
            // 读取载具关联的条码
            var vehicleEntities = await _inteVehicleRepository.GetByCodesAsync(new EntityByCodesQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                Codes = createDto.Sfcs
            });

            // 不在系统中的载具代码
            var notInSystem = createDto.Sfcs.Except(vehicleEntities.Select(s => s.Code));
            if (notInSystem.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18624))
                    .WithData("Code", string.Join(',', notInSystem));
            }

            // 查询载具关联的条码明细
            var vehicleFreightStackEntities = await _inteVehiceFreightStackRepository.GetEntitiesAsync(new EntityByParentIdsQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                ParentIds = vehicleEntities.Select(s => s.Id)
            });

            var sfcs = vehicleFreightStackEntities.Select(s => s.BarCode).ToArray();
            #endregion

            createDto.Sfcs = sfcs;
            //调取之前的不良录入方法
            await CreateManuProductBadRecordAsync(createDto);
        }

        /// <summary>
        /// 查询条码的不合格代码信息
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuProductBadRecordViewDto>> GetBadRecordsBySfcAsync(ManuProductBadRecordQueryDto queryDto)
        {
            var query = new ManuProductBadRecordQuery
            {
                SFC = queryDto.SFC,
                Status = queryDto.Status,
                Type = queryDto.Type,
                SiteId = _currentSite.SiteId ?? 0
            };
            var manuProductBads = await _manuProductBadRecordRepository.GetBadRecordsBySfcAsync(query);

            // 实体到DTO转换 装载数据
            var manuProductBadRecordDtos = new List<ManuProductBadRecordViewDto>();
            foreach (var manuProductBad in manuProductBads)
            {
                manuProductBadRecordDtos.Add(new ManuProductBadRecordViewDto
                {
                    UnqualifiedId = manuProductBad.UnqualifiedId,
                    UnqualifiedCode = manuProductBad.UnqualifiedCode,
                    UnqualifiedCodeName = manuProductBad.UnqualifiedCodeName,
                    ResCode = manuProductBad.ResCode,
                    ResName = manuProductBad.ResName,
                    ProcessRouteId = manuProductBad.ProcessRouteId,
                    Remark = ""
                });
            }

            // 根据条码和不合格代码和资源去重显示
            manuProductBadRecordDtos = manuProductBadRecordDtos.DistinctBy(x => x.UnqualifiedId).ToList();
            return manuProductBadRecordDtos;
        }

        /// <summary>
        /// 不良复判
        /// </summary>
        /// <param name="badReJudgmentDto"></param>
        /// <returns></returns>
        public async Task BadReJudgmentAsync(BadReJudgmentDto badReJudgmentDto)
        {
            if (string.IsNullOrWhiteSpace(badReJudgmentDto.Sfc)) throw new CustomerValidationException(nameof(ErrorCode.MES15400));
            //if (!badReJudgmentDto.UnqualifiedLists.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES15405));

            var manuSfcEntity = await _manuSfcRepository.GetSingleAsync(new ManuSfcQuery
            {
                SFC = badReJudgmentDto.Sfc,
                SiteId = _currentSite.SiteId ?? 0,
                Type = SfcTypeEnum.Produce
            }
            );

            var manuSfcProduceEntity = await _manuSfcProduceRepository.GetBySFCAsync(new ManuSfcProduceBySfcQuery
            {
                Sfc = badReJudgmentDto.Sfc,
                SiteId = _currentSite.SiteId ?? 0
            });

            //返修中无需复判
            if (manuSfcProduceEntity != null && (manuSfcProduceEntity.IsRepair ?? TrueOrFalseEnum.No) == TrueOrFalseEnum.Yes)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15424));
            }

            var badRecordList = await _manuProductBadRecordRepository.GetBadRecordsBySfcAsync(new ManuProductBadRecordQuery
            {
                SFC = badReJudgmentDto.Sfc,
                Status = ProductBadRecordStatusEnum.Open,
                Type = QualUnqualifiedCodeTypeEnum.Defect,
                SiteId = _currentSite.SiteId ?? 0
            });

            if (badRecordList == null || !badRecordList.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15423));
            }

            if (badRecordList.Any(o => !badReJudgmentDto.UnqualifiedLists.Select(x => x.UnqualifiedId).Contains(o.UnqualifiedId)))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15428));
            }

            //报废验证
            if (manuSfcEntity.Status == SfcStatusEnum.Scrapping)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15417));
            }

            //锁定验证
            if (manuSfcEntity.Status == SfcStatusEnum.Locked)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15416));
            }

            var manuSfcInfoEntity = await _manuSfcInfoRepository.GetBySFCIdWithIsUseAsync(manuSfcEntity.Id);

            //步骤表-复判
            var manuSfcStepEntity = new ManuSfcStepEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SFC = badReJudgmentDto.Sfc,
                ProductId = manuSfcInfoEntity.ProductId,
                WorkOrderId = manuSfcInfoEntity.WorkOrderId ?? 0,
                WorkCenterId = manuSfcProduceEntity?.WorkCenterId,
                ProductBOMId = manuSfcProduceEntity?.ProductBOMId,
                ProcessRouteId = manuSfcProduceEntity?.ProcessRouteId,
                Qty = manuSfcEntity.Qty,
                EquipmentId = manuSfcProduceEntity?.EquipmentId,
                ResourceId = manuSfcProduceEntity?.ResourceId,
                ProcedureId = manuSfcProduceEntity?.ProcedureId,
                Operatetype = ManuSfcStepTypeEnum.BadRejudgment,
                CurrentStatus = manuSfcEntity.Status,
                SiteId = _currentSite.SiteId ?? 0,
                CreatedOn = HymsonClock.Now(),
                CreatedBy = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now(),
                Remark = badReJudgmentDto.Remark,
                UpdatedBy = _currentUser.UserName,
            };

            //判断是否关闭所有不合格信息
            var updateCommandList = new List<ManuProductBadRecordCommand>();

            foreach (var unqualified in badReJudgmentDto.UnqualifiedLists)
            {
                var manuProductBadRecordCommand = new ManuProductBadRecordCommand
                {
                    SiteId = _currentSite.SiteId ?? 0,
                    Sfc = badReJudgmentDto.Sfc,
                    UnqualifiedId = unqualified.UnqualifiedId,
                    ReJudgmentSfcStepId = manuSfcStepEntity.Id,
                    CurrentStatus = ProductBadRecordStatusEnum.Open,
                    Remark = badReJudgmentDto.Remark ?? "",
                    UserId = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now(),
                    ReJudgmentBy = _currentUser.UserName,
                    ReJudgmentRemark = unqualified.Remark,
                    ReJudgmentOn = HymsonClock.Now()
                };

                if (unqualified.IsClosed ?? false)
                {
                    manuProductBadRecordCommand.ReJudgmentSfcStepId = manuSfcStepEntity.Id;
                    manuProductBadRecordCommand.Status = ProductBadRecordStatusEnum.Close;
                    manuProductBadRecordCommand.CloseBy = _currentUser.UserName;
                    manuProductBadRecordCommand.CloseOn = HymsonClock.Now();
                    manuProductBadRecordCommand.ReJudgmentResult = ProductBadDisposalResultEnum.ReJudgmentClosed;
                    manuProductBadRecordCommand.ReJudgmentBy = _currentUser.UserName;
                    manuProductBadRecordCommand.ReJudgmentOn = HymsonClock.Now();
                }
                else
                {
                    manuProductBadRecordCommand.Status = ProductBadRecordStatusEnum.Open;
                    manuProductBadRecordCommand.ReJudgmentResult = ProductBadDisposalResultEnum.ReJudgmentRepair;
                }
                updateCommandList.Add(manuProductBadRecordCommand);
            }

            if (badReJudgmentDto.UnqualifiedLists.Any(x => !(x.IsClosed ?? false)))//存在未关闭的不合格代码
            {
                var processRouteProcedure = await _manuCommonOldService.GetFirstProcedureAsync(badReJudgmentDto.BadProcessRouteId ?? 0);

                if (manuSfcProduceEntity == null)//完成品返修
                {
                    //获取激活工单
                    var activeOrder = await _planWorkOrderActivationRepository.GetByWorkOrderIdAsync(manuSfcInfoEntity.WorkOrderId ?? 0);
                    if (activeOrder == null)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES15418));
                    }

                    //库存信息
                    var whMaterialInventoryEntity = await _whMaterialInventoryRepository.GetByBarCodeAsync(
                        new WhMaterialInventoryBarCodeQuery()
                        {
                            SiteId = _currentSite.SiteId ?? 0,
                            BarCode = badReJudgmentDto.Sfc
                        });

                    if (whMaterialInventoryEntity == null)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES15421));
                    }

                    //工单信息
                    var planWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(activeOrder.WorkOrderId);
                    //插入在制品表
                    manuSfcProduceEntity = new ManuSfcProduceEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = _currentSite.SiteId ?? 0,
                        SFC = badReJudgmentDto.Sfc,
                        SFCId = manuSfcEntity.Id,
                        ProductId = planWorkOrderEntity.ProductId,
                        WorkOrderId = planWorkOrderEntity.Id,
                        BarCodeInfoId = manuSfcInfoEntity.Id,
                        ProcessRouteId = badReJudgmentDto.BadProcessRouteId ?? 0,
                        WorkCenterId = planWorkOrderEntity.WorkCenterId ?? 0,
                        ProductBOMId = planWorkOrderEntity.ProductBOMId,
                        Qty = whMaterialInventoryEntity.QuantityResidue,
                        ProcedureId = processRouteProcedure.ProcedureId,
                        Status = SfcStatusEnum.lineUp,
                        RepeatedCount = 0,
                        IsScrap = TrueOrFalseEnum.Yes,
                        CreatedOn = HymsonClock.Now(),
                        CreatedBy = _currentUser.UserName,
                        UpdatedOn = HymsonClock.Now(),
                        UpdatedBy = _currentUser.UserName,
                    };

                    //插入在制品业务
                    var manuSfcProduceBusinessEntity = new ManuSfcProduceBusinessEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SfcProduceId = manuSfcProduceEntity.Id,
                        BusinessType = ManuSfcProduceBusinessType.Repair,
                        BusinessContent = JsonConvert.SerializeObject(new SfcProduceRepairBo
                        {
                            ProcessRouteId = manuSfcProduceEntity.ProcessRouteId, //badReJudgmentDto.BadProcessRouteId ?? 0,
                            ProcedureId = manuSfcProduceEntity.ProcedureId,//processRouteProcedure.ProcedureId
                        }),
                        SiteId = _currentSite.SiteId ?? 0,
                        CreatedOn = HymsonClock.Now(),
                        CreatedBy = _currentUser.UserName,
                        UpdatedOn = HymsonClock.Now(),
                        UpdatedBy = _currentUser.UserName,
                    };

                    //修改条码状态 -排队 
                    var manuSfcUpdateStatusByIdCommand = new ManuSfcUpdateStatusByIdCommand
                    {
                        Status = SfcStatusEnum.lineUp,
                        CurrentStatus = manuSfcEntity.Status,
                        UpdatedOn = HymsonClock.Now(),
                        UpdatedBy = _currentUser.UserName,
                    };

                    //扣除库存数量
                    var whMaterialInventoryEmptyByIdCommand = new UpdateWhMaterialInventoryEmptyByIdCommand
                    {
                        Id = whMaterialInventoryEntity.Id
                    };

                    using (var trans = TransactionHelper.GetTransactionScope())
                    {
                        var row = await _manuProductBadRecordRepository.UpdateStatusRangeAsync(updateCommandList);
                        if (row != updateCommandList.Count)
                        {
                            throw new CustomerValidationException(nameof(ErrorCode.MES15425));
                        }
                        row = await _manuSfcRepository.ManuSfcUpdateStatuByIdAsync(manuSfcUpdateStatusByIdCommand);
                        if (row != 1)
                        {
                            throw new CustomerValidationException(nameof(ErrorCode.MES15426));
                        }
                        await _manuSfcProduceRepository.InsertAsync(manuSfcProduceEntity);
                        await _manuSfcProduceRepository.InsertSfcProduceBusinessAsync(manuSfcProduceBusinessEntity);
                        await _whMaterialInventoryRepository.UpdateWhMaterialInventoryEmptyByIdAync(whMaterialInventoryEmptyByIdCommand);
                        await _manuSfcStepRepository.InsertAsync(manuSfcStepEntity);
                    }
                }
                else
                {
                    // 在制品业务
                    var manuSfcProduceBusinessEntity = new ManuSfcProduceBusinessEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SfcProduceId = manuSfcProduceEntity.Id,
                        BusinessType = ManuSfcProduceBusinessType.Repair,
                        BusinessContent = JsonConvert.SerializeObject(new SfcProduceRepairBo
                        {
                            ProcessRouteId = manuSfcProduceEntity.ProcessRouteId, //badReJudgmentDto.BadProcessRouteId ?? 0,
                            ProcedureId = manuSfcProduceEntity.ProcedureId,//processRouteProcedure.ProcedureId
                        }),
                        SiteId = _currentSite.SiteId ?? 0,
                        CreatedOn = HymsonClock.Now(),
                        CreatedBy = _currentUser.UserName,
                        UpdatedOn = HymsonClock.Now(),
                        UpdatedBy = _currentUser.UserName,
                    };

                    //修改条码状态 -排队 
                    var manuSfcUpdateStatusByIdCommand = new ManuSfcUpdateStatusByIdCommand
                    {
                        Id = manuSfcEntity.Id,
                        Status = SfcStatusEnum.lineUp,
                        CurrentStatus = manuSfcEntity.Status,
                        UpdatedOn = HymsonClock.Now(),
                        UpdatedBy = _currentSite.Name
                    };

                    //修改在制品条码状态
                    var manuSfcUpdateRouteByIdCommand = new ManuSfcUpdateRouteByIdCommand
                    {
                        Id = manuSfcProduceEntity.Id,
                        Status = SfcStatusEnum.lineUp,
                        ProcessRouteId = badReJudgmentDto.BadProcessRouteId ?? 0,
                        ProcedureId = processRouteProcedure.ProcedureId,
                        IsRepair = TrueOrFalseEnum.Yes,
                    };

                    using (var trans = TransactionHelper.GetTransactionScope())
                    {
                        //处理不合格信息
                        var row = await _manuProductBadRecordRepository.UpdateStatusRangeAsync(updateCommandList);
                        if (row != updateCommandList.Count)
                        {
                            throw new CustomerValidationException(nameof(ErrorCode.MES15425));
                        }
                        row = await _manuSfcRepository.ManuSfcUpdateStatuByIdAsync(manuSfcUpdateStatusByIdCommand);
                        if (row != 1)
                        {
                            throw new CustomerValidationException(nameof(ErrorCode.MES15426));
                        }
                        await _manuSfcStepRepository.InsertAsync(manuSfcStepEntity);
                        await _manuSfcProduceRepository.InsertSfcProduceBusinessAsync(manuSfcProduceBusinessEntity);
                        await _manuSfcProduceRepository.UpdateRouteByIdAsync(manuSfcUpdateRouteByIdCommand);
                        trans.Complete();
                    }
                }
            }
            else
            {
                if (manuSfcProduceEntity == null)
                {
                    using (var trans = TransactionHelper.GetTransactionScope())
                    {
                        //处理不合格信息
                        var row = await _manuProductBadRecordRepository.UpdateStatusRangeAsync(updateCommandList);
                        if (row != updateCommandList.Count)
                        {
                            throw new CustomerValidationException(nameof(ErrorCode.MES15425));
                        }
                        await _manuSfcStepRepository.InsertAsync(manuSfcStepEntity);
                        trans.Complete();
                    }
                }
                else
                {
                    //var isLast = await IsLastProcedureIdAsync(manuSfcProduceEntity.ProcessRouteId, manuSfcProduceEntity.ProcedureId);
                    // 末尾工序在制完成
                    if (manuSfcProduceEntity.Status == SfcStatusEnum.InProductionComplete)
                    {
                        var manuSfcUpdateStatusByIdCommand = new ManuSfcUpdateStatusByIdCommand
                        {
                            Id = manuSfcProduceEntity.SFCId,
                            UpdatedBy = _currentUser.UserName,
                            UpdatedOn = HymsonClock.Now(),
                            Status = SfcStatusEnum.Complete,
                            CurrentStatus = manuSfcProduceEntity.Status
                        };

                        var procMaterialEntity = await _procMaterialRepository.GetByIdAsync(manuSfcProduceEntity.ProductId);
                        var planWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(manuSfcProduceEntity.WorkOrderId);
                        var whMaterialInventoryEntity = new WhMaterialInventoryEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            SupplierId = 0,//自制品 没有
                            MaterialId = manuSfcProduceEntity.ProductId,
                            MaterialBarCode = manuSfcProduceEntity.SFC,
                            //Batch = "",//自制品 没有
                            MaterialType = planWorkOrderEntity.ProductId == procMaterialEntity.Id ? MaterialInventoryMaterialTypeEnum.FinishedParts : MaterialInventoryMaterialTypeEnum.SelfMadeParts,
                            QuantityResidue = manuSfcProduceEntity.Qty,
                            Status = WhMaterialInventoryStatusEnum.ToBeUsed,
                            Source = MaterialInventorySourceEnum.ManuComplete,
                            SiteId = manuSfcProduceEntity.SiteId,
                            CreatedBy = _currentUser.UserName,
                            CreatedOn = HymsonClock.Now(),
                            UpdatedBy = _currentUser.UserName,
                            UpdatedOn = HymsonClock.Now(),
                        };
                        using (var trans = TransactionHelper.GetTransactionScope())
                        {
                            //处理不合格信息
                            var row = await _manuProductBadRecordRepository.UpdateStatusRangeAsync(updateCommandList);
                            if (row != updateCommandList.Count)
                            {
                                throw new CustomerValidationException(nameof(ErrorCode.MES15425));
                            }
                            //条码修改为已完成状态
                            row = await _manuSfcRepository.ManuSfcUpdateStatuByIdAsync(manuSfcUpdateStatusByIdCommand);
                            if (row != 1)
                            {
                                throw new CustomerValidationException(nameof(ErrorCode.MES15426));
                            }
                            //删除在制品信息
                            await _manuSfcProduceRepository.DeleteAsync(manuSfcProduceEntity.Id);

                            //记录step信息
                            await _manuSfcStepRepository.InsertAsync(manuSfcStepEntity);

                            await _whMaterialInventoryRepository.InsertAsync(whMaterialInventoryEntity);
                            trans.Complete();
                        }
                    }
                    else
                    {
                        var CleanRepeatedCountCommand = new CleanRepeatedCountCommand
                        {
                            Id = manuSfcProduceEntity.Id,
                            UpdatedBy = _currentUser.UserName,
                            UpdatedOn = HymsonClock.Now(),
                        };
                        using (var trans = TransactionHelper.GetTransactionScope())
                        {
                            //处理不合格信息
                            var row = await _manuProductBadRecordRepository.UpdateStatusRangeAsync(updateCommandList);
                            if (row != updateCommandList.Count)
                            {
                                throw new CustomerValidationException(nameof(ErrorCode.MES15425));
                            }
                            await _manuSfcProduceRepository.CleanRepeatedCountById(CleanRepeatedCountCommand);
                            await _manuSfcStepRepository.InsertAsync(manuSfcStepEntity);
                            trans.Complete();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 取消标识
        /// </summary>
        /// <param name="cancelDto"></param>
        /// <returns></returns>
        public async Task CancelSfcIdentificationAsync(CancelSfcIdentificationDto cancelDto)
        {
            if (cancelDto == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }

            if (!cancelDto.UnqualifiedLists.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15400));
            }

            var sfcs = cancelDto.UnqualifiedLists.Select(x => x.Sfc).Distinct().ToArray();
            var manuSfcProducePagedQuery = new ManuSfcProduceQuery { Sfcs = sfcs, SiteId = _currentSite.SiteId ?? 0 };
            // 获取条码列表
            var manuSfcs = await _manuSfcProduceRepository.GetManuSfcProduceInfoEntitiesAsync(manuSfcProducePagedQuery);
            if (!manuSfcs.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15402));
            }
            sfcs = manuSfcs.Select(x => x.SFC).ToArray();
            var scrapSfcs = manuSfcs.Where(x => x.IsScrap == TrueOrFalseEnum.Yes).Select(x => x.SFC).ToArray();
            //类型为报废时判断条码是否已经报废,若已经报废提示:存在已报废的条码，不可再次报废
            if (scrapSfcs.Any())
            {
                var strs = string.Join("','", scrapSfcs);
                throw new CustomerValidationException(nameof(ErrorCode.MES15411)).WithData("sfcs", strs);
            }
            await VerifySfcsLockAsync(manuSfcs.ToArray());

            #region  组装数据
            var sfcStepList = new List<ManuSfcStepEntity>();
            if (manuSfcs.Any())
            {
                sfcStepList.Add(CreateSFCStepEntity(manuSfcs.ToList()[0], ManuSfcStepTypeEnum.CloseIdentification, cancelDto.Remark ?? ""));
            }

            var updateCommandList = new List<ManuProductBadRecordCommand>();
            foreach (var unqualified in cancelDto.UnqualifiedLists)
            {
                updateCommandList.Add(new ManuProductBadRecordCommand
                {
                    SiteId = _currentSite.SiteId ?? 0,
                    Sfc = unqualified.Sfc,
                    UnqualifiedId = unqualified.UnqualifiedId,
                    Remark = unqualified.Remark ?? "",
                    Status = ProductBadRecordStatusEnum.Close,
                    UserId = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now(),
                    CurrentStatus = ProductBadRecordStatusEnum.Open
                });
            }
            #endregion

            //入库
            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                //1.修改状态为关闭
                rows += await _manuProductBadRecordRepository.UpdateStatusRangeAsync(updateCommandList);
                if (rows < updateCommandList.Count)
                {
                    //报错
                    throw new CustomerValidationException(nameof(ErrorCode.MES15414)).WithData("sfcs", string.Join("','", sfcs));
                }

                //2.记录数据
                rows += await _manuSfcStepRepository.InsertRangeAsync(sfcStepList);

                trans.Complete();
            }
        }

        /// <summary>
        /// 创建条码步骤数据
        /// </summary>
        /// <param name="sfc"></param>
        /// <param name="type"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        private ManuSfcStepEntity CreateSFCStepEntity(ManuSfcProduceEntity sfc, ManuSfcStepTypeEnum type, string remark = "")
        {
            return new ManuSfcStepEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SFC = sfc.SFC,
                ProductId = sfc.ProductId,
                WorkOrderId = sfc.WorkOrderId,
                WorkCenterId = sfc.WorkCenterId,
                ProductBOMId = sfc.ProductBOMId,
                ProcessRouteId = sfc?.ProcessRouteId,
                Qty = sfc.Qty,
                EquipmentId = sfc.EquipmentId,
                ResourceId = sfc.ResourceId,
                ProcedureId = sfc.ProcedureId,
                Operatetype = type,
                CurrentStatus = sfc.Status,
                Remark = remark,
                SiteId = _currentSite.SiteId ?? 0,
                CreatedBy = sfc.CreatedBy,
                UpdatedBy = sfc.UpdatedBy
            };
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesManuProductBadRecordAsync(long[] idsArr)
        {
            if (idsArr.Length < 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10102));
            }
            var command = new DeleteCommand
            {
                UserId = _currentUser.UserName,
                DeleteOn = HymsonClock.Now(),
                Ids = idsArr
            };
            return await _manuProductBadRecordRepository.DeleteRangeAsync(command);
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="manuProductBadRecordPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuProductBadRecordDto>> GetPageListAsync(ManuProductBadRecordPagedQueryDto manuProductBadRecordPagedQueryDto)
        {
            var manuProductBadRecordPagedQuery = manuProductBadRecordPagedQueryDto.ToQuery<ManuProductBadRecordPagedQuery>();
            manuProductBadRecordPagedQuery.SiteId = _currentSite.SiteId;
            var pagedInfo = await _manuProductBadRecordRepository.GetPagedInfoAsync(manuProductBadRecordPagedQuery);

            //实体到DTO转换 装载数据
            List<ManuProductBadRecordDto> manuProductBadRecordDtos = PrepareManuProductBadRecordDtos(pagedInfo);
            return new PagedInfo<ManuProductBadRecordDto>(manuProductBadRecordDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 实体转换
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ManuProductBadRecordDto> PrepareManuProductBadRecordDtos(PagedInfo<ManuProductBadRecordEntity> pagedInfo)
        {
            var manuProductBadRecordDtos = new List<ManuProductBadRecordDto>();
            foreach (var manuProductBadRecordEntity in pagedInfo.Data)
            {
                var manuProductBadRecordDto = manuProductBadRecordEntity.ToModel<ManuProductBadRecordDto>();
                manuProductBadRecordDtos.Add(manuProductBadRecordDto);
            }

            return manuProductBadRecordDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuProductBadRecordModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyManuProductBadRecordAsync(ManuProductBadRecordModifyDto manuProductBadRecordModifyDto)
        {
            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(manuProductBadRecordModifyDto);

            //DTO转换实体
            var manuProductBadRecordEntity = manuProductBadRecordModifyDto.ToEntity<ManuProductBadRecordEntity>();
            manuProductBadRecordEntity.UpdatedBy = _currentUser.UserName;

            await _manuProductBadRecordRepository.UpdateAsync(manuProductBadRecordEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuProductBadRecordDto> QueryManuProductBadRecordByIdAsync(long id)
        {
            var manuProductBadRecordEntity = await _manuProductBadRecordRepository.GetByIdAsync(id);
            if (manuProductBadRecordEntity != null)
            {
                return manuProductBadRecordEntity.ToModel<ManuProductBadRecordDto>();
            }
            return new ManuProductBadRecordDto();
        }

        /// <summary>
        /// 验证sfc是否锁定
        /// </summary>
        /// <param name="manuSfcs"></param>
        /// <returns></returns>
        private async Task VerifySfcsLockAsync(ManuSfcProduceInfoView[] manuSfcs)
        {
            var sfcs = manuSfcs.Select(x => x.SFC).ToArray();
            var sfcProduceBusinesss = await _manuSfcProduceRepository.GetSfcProduceBusinessListBySFCAsync(new SfcListProduceBusinessQuery { SiteId = _currentSite.SiteId ?? 0, Sfcs = sfcs, BusinessType = ManuSfcProduceBusinessType.Lock });
            if (sfcProduceBusinesss != null && sfcProduceBusinesss.Any())
            {
                var sfcProduceBusinesssList = sfcProduceBusinesss.ToList();
                var instantLockSfcs = new List<string>();
                foreach (var business in sfcProduceBusinesssList)
                {
                    var manuSfc = manuSfcs.FirstOrDefault(x => x.Id == business.SfcProduceId);
                    if (manuSfc == null)
                    {
                        continue;
                    }
                    var sfcProduceLockBo = System.Text.Json.JsonSerializer.Deserialize<SfcProduceLockBo>(business.BusinessContent);
                    if (sfcProduceLockBo == null)
                    {
                        continue;
                    }
                    if (sfcProduceLockBo.Lock == QualityLockEnum.InstantLock)
                    {
                        instantLockSfcs.Add(manuSfc.SFC);
                    }
                    if (sfcProduceLockBo.Lock == QualityLockEnum.FutureLock && sfcProduceLockBo.LockProductionId == manuSfc.ProcedureId)
                    {
                        instantLockSfcs.Add(manuSfc.SFC);
                    }
                }

                if (instantLockSfcs.Any())
                {
                    var strs = string.Join(",", instantLockSfcs.Distinct().ToArray());
                    throw new CustomerValidationException(nameof(ErrorCode.MES15407)).WithData("sfcs", strs);
                }
            }
        }

        /// <summary>
        /// 验证条码是否锁定和是否有返修任务
        /// </summary>
        /// <param name="sfc"></param>
        /// <param name="procedureId"></param>
        /// <param name="sfcInfoId"></param>
        /// <returns></returns>
        private async Task VerifyLockOrRepairAsync(string sfc, long procedureId, long sfcInfoId)
        {
            IEnumerable<long> sfcInfoIds = new[] { sfcInfoId };
            var sfcProduceBusinessEntities = await _manuSfcProduceRepository.GetSfcProduceBusinessBySFCIdsAsync(sfcInfoIds);
            if (sfcProduceBusinessEntities != null && sfcProduceBusinessEntities.Any())
            {
                //锁定的
                var lockEntity = sfcProduceBusinessEntities.FirstOrDefault(x => x.BusinessType == ManuSfcProduceBusinessType.Lock);
                if (lockEntity != null)
                {
                    lockEntity.VerifyProcedureLock(sfc, procedureId);
                }

                //有缺陷的返修业务
                var repairEntity = sfcProduceBusinessEntities.FirstOrDefault(x => x.BusinessType == ManuSfcProduceBusinessType.Repair);
                if (repairEntity != null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16319)).WithData("SFC", sfc);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="processRouteId"></param>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        private async Task<bool> IsLastProcedureIdAsync(long processRouteId, long procedureId)
        {
            var processRouteNodes = await _manuCommonOldService.GetProcessRouteAsync(processRouteId);
            var isLast = false;
            if (processRouteNodes.Any())
            {
                var id = processRouteNodes?.FirstOrDefault()?.ProcedureIds.Last() ?? 0;
                //判断是否末尾工序，末尾工序把条码改成已完成状态
                if (id == procedureId)
                {
                    isLast = true;
                }
            }
            return isLast;
        }

        #region 录入标识

        /// <summary>
        /// 录入标识
        /// </summary>
        /// <param name="productBadRecordMarkSaveDtos"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        public async Task SaveBadRecordMarkEntryAsync(List<ManuProductBadRecordMarkSaveDto> productBadRecordMarkSaveDtos)
        {
            #region 校验数据
            if (productBadRecordMarkSaveDtos == null || !productBadRecordMarkSaveDtos.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES15431));

            foreach (var item in productBadRecordMarkSaveDtos)
            {
                if (string.IsNullOrEmpty(item.SFC)) throw new CustomerValidationException(nameof(ErrorCode.MES15432));
                if (item.FoundBadOperationId <= 0) throw new CustomerValidationException(nameof(ErrorCode.MES15433));
                if (item.UnqualifiedId <= 0) throw new CustomerValidationException(nameof(ErrorCode.MES15434));
            }

            var needHandleSfcs = productBadRecordMarkSaveDtos.Select(x => x.SFC).Distinct();

            //检测是否与数据库中的重复
            var reaps = productBadRecordMarkSaveDtos.GroupBy(x => new { x.SFC, x.FoundBadOperationId, x.UnqualifiedId, x.InterceptProcedureId }).Where(group => group.Count() > 1);
            if (reaps != null && reaps.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES15435));

            var sfcsBadRecords = await _manuProductBadRecordRepository.GetManuProductBadRecordEntitiesBySFCAsync(new ManuProductBadRecordBySfcQuery { SiteId = _currentSite.SiteId ?? 0, SFCs = needHandleSfcs, Status = ProductBadRecordStatusEnum.Open });

            var addGroupKeys = productBadRecordMarkSaveDtos.GroupBy(x => new { x.SFC, x.FoundBadOperationId, x.UnqualifiedId, x.InterceptProcedureId });
            var groups = sfcsBadRecords.GroupBy(x => new { x.SFC, x.FoundBadOperationId, x.UnqualifiedId, x.InterceptOperationId });

            //查询工序信息
            var procProcedureEntities = await _procProcedureRepository.GetProcProcedureEntitiesAsync(new ProcProcedureQuery { SiteId = _currentSite.SiteId ?? 0 });

            foreach (var item in groups)
            {
                if (addGroupKeys.Any(x => x.Key.SFC == item.Key.SFC && x.Key.FoundBadOperationId == item.Key.FoundBadOperationId && x.Key.UnqualifiedId == item.Key.UnqualifiedId && x.Key.InterceptProcedureId == item.Key.InterceptOperationId))
                {
                    //查询工序
                    //var procedure = await _procProcedureRepository.GetByIdAsync(item.Key.FoundBadOperationId);
                    //发现不良工序
                    var foundBadprocedure = procProcedureEntities.FirstOrDefault(a => a.Id == item.Key.FoundBadOperationId);
                    //拦截工序
                    var interceptprocedure = procProcedureEntities.FirstOrDefault(a => a.Id == item.Key.InterceptOperationId);

                    //查询不合格代码
                    var unqualifiedCode = await _qualUnqualifiedCodeRepository.GetByIdAsync(item.Key.UnqualifiedId);

                    throw new CustomerValidationException(nameof(ErrorCode.MES15436)).WithData("sfc", item.Key.SFC).WithData("foundBadOperationCode", foundBadprocedure?.Code ?? "").WithData("unqualifiedCode", unqualifiedCode?.UnqualifiedCode ?? "").WithData("InterceptOperationCode", interceptprocedure?.Code ?? "");
                }
            }

            //检测条码是否是符合要求的
            var sfcEntities = await _manuSfcRepository.GetListAsync(new ManuSfcQuery
            {
                SFCs = needHandleSfcs,
                SiteId = _currentSite.SiteId ?? 0,
                Type = SfcTypeEnum.Produce
            });

            //检测条码状态
            var notAllowStatus = new[] { SfcStatusEnum.Locked, SfcStatusEnum.Delete, SfcStatusEnum.Invalid };
            var notAllowSfcs = sfcEntities.Where(x => notAllowStatus.Contains(x.Status)).Select(x => x.SFC);
            if (notAllowSfcs != null && notAllowSfcs.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15438)).WithData("sfc", string.Join(",", notAllowSfcs.ToArray()));
            }
            //检测条码是否存在
            var notExistSfcs = needHandleSfcs.Except(sfcEntities.Select(x => x.SFC).Distinct());
            if (notAllowSfcs != null && notAllowSfcs.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES15439)).WithData("sfc", string.Join(",", notExistSfcs));

            //检测不合格代码
            // 获取不合格代码列表
            var qualUnqualifiedCodes = await _qualUnqualifiedCodeRepository.GetByIdsAsync(productBadRecordMarkSaveDtos.Select(x => x.UnqualifiedId));

            if (qualUnqualifiedCodes.Any(x => x.Type != QualUnqualifiedCodeTypeEnum.Mark)) throw new CustomerValidationException(nameof(ErrorCode.MES15437));

            #endregion

            //条码信息表
            var sfcInfoEntities = await _manuSfcInfoRepository.GetBySFCIdsWithIsUseAsync(sfcEntities.Select(x => x.Id));
            var manuSfcProducePagedQuery = new ManuSfcProduceQuery { Sfcs = needHandleSfcs, SiteId = _currentSite.SiteId ?? 00 };
            //在制品信息
            var manuSfcProduces = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(manuSfcProducePagedQuery);


            var sfcStepList = new List<ManuSfcStepEntity>();
            var manuProductBadRecords = new List<ManuProductBadRecordEntity>();

            foreach (var item in productBadRecordMarkSaveDtos)
            {
                var sfcEntity = sfcEntities.Where(x => x.SFC == item.SFC).FirstOrDefault();
                var sfcInfoEntity = sfcInfoEntities.Where(x => x.SfcId == sfcEntity?.Id).FirstOrDefault();

                var manuSfcProduceInfoEntity = manuSfcProduces.FirstOrDefault(x => x.SFC == item.SFC);

                //步骤表-不良录入
                var manuSfcStepEntity = new ManuSfcStepEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SFC = item.SFC,
                    ProductId = sfcInfoEntity?.ProductId ?? 0,
                    WorkOrderId = sfcInfoEntity?.WorkOrderId ?? 0,
                    WorkCenterId = manuSfcProduceInfoEntity?.WorkCenterId,
                    ProductBOMId = manuSfcProduceInfoEntity?.ProductBOMId,
                    ProcessRouteId = manuSfcProduceInfoEntity?.ProcessRouteId,
                    Qty = sfcEntity?.Qty ?? 0,
                    EquipmentId = manuSfcProduceInfoEntity?.EquipmentId,
                    ResourceId = manuSfcProduceInfoEntity?.ResourceId,
                    ProcedureId = manuSfcProduceInfoEntity?.ProcedureId,
                    Operatetype = ManuSfcStepTypeEnum.Marking,
                    CurrentStatus = sfcEntity!.Status,
                    SiteId = _currentSite.SiteId ?? 0,
                    CreatedOn = HymsonClock.Now(),
                    CreatedBy = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentUser.UserName,
                };
                sfcStepList.Add(manuSfcStepEntity);

                var manuProductBadRecordEntity = new ManuProductBadRecordEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    FoundBadOperationId = item.FoundBadOperationId,
                    //OutflowOperationId = manuProductBadRecordCreateDto.OutflowOperationId,
                    UnqualifiedId = item.UnqualifiedId,
                    SfcStepId = manuSfcStepEntity.Id,
                    SFC = item.SFC,
                    SfcInfoId = sfcInfoEntity?.Id,
                    Qty = sfcEntity.Qty,
                    Source = ProductBadRecordSourceEnum.BadManualEntry,
                    Remark = item.Remark ?? "",
                    CreatedOn = HymsonClock.Now(),
                    CreatedBy = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentUser.UserName,

                    Status = ProductBadRecordStatusEnum.Open,
                    InterceptOperationId = item.InterceptProcedureId.GetValueOrDefault()
                };

                manuProductBadRecords.Add(manuProductBadRecordEntity);
            }

            using (var trans = TransactionHelper.GetTransactionScope())
            {
                //插入不合格记录
                if (manuProductBadRecords != null && manuProductBadRecords.Any())
                {
                    //await _manuProductBadRecordRepository.InsertRangeAsync(manuProductBadRecords);
                    var insertResult = await _manuProductBadRecordRepository.InsertIgnoreRangeAsync(manuProductBadRecords);
                    if (insertResult != manuProductBadRecords.Count)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES19710));
                    }
                }
                //插入步骤表
                if (sfcStepList != null && sfcStepList.Any())
                {
                    await _manuSfcStepRepository.InsertRangeAsync(sfcStepList);
                }

                trans.Complete();
            }

        }

        /// <summary>
        /// 录入标识导出模板
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public async Task DownloadImportTemplateAsync(Stream stream)
        {
            var excelTemplateDtos = new List<ManuProductBadRecordMarkEntryImportDto>();
            await _excelService.ExportAsync(excelTemplateDtos, stream, "录入标识导入模板");
        }

        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        public async Task ImportBadRecordMarkEntryAsync(IFormFile formFile)
        {
            using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream).ConfigureAwait(false);
            var excelImportDtos = _excelService.Import<ManuProductBadRecordMarkEntryImportDto>(memoryStream);

            /*
            // 备份用户上传的文件，可选
            var stream = formFile.OpenReadStream();
            var uploadResult = await _minioService.PutObjectAsync(formFile.FileName, stream, formFile.ContentType);
            */

            if (excelImportDtos == null || !excelImportDtos.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES14908));
            }

            #region 验证数据
            foreach (var item in excelImportDtos)
            {
                if (string.IsNullOrEmpty(item.SFC)) throw new CustomerValidationException(nameof(ErrorCode.MES15432));
                if (string.IsNullOrEmpty(item.FoundBadOperationCode)) throw new CustomerValidationException(nameof(ErrorCode.MES15433));
                if (string.IsNullOrEmpty(item.UnqualifiedCode)) throw new CustomerValidationException(nameof(ErrorCode.MES15434));
            }

            var needHandleSfcs = excelImportDtos.Select(x => x.SFC).Distinct();

            //检测 传入数据中内部是否重复
            var reaps = excelImportDtos.GroupBy(x => new { x.SFC, x.FoundBadOperationCode, x.UnqualifiedCode, x.InterceptProcedureCode }).Where(group => group.Count() > 1);
            if (reaps != null && reaps.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES15435));

            //检测 传入的数据 是否正确
            //检测条码是否是符合要求的
            var sfcEntities = await _manuSfcRepository.GetListAsync(new ManuSfcQuery { SFCs = needHandleSfcs, SiteId = _currentSite.SiteId ?? 0, Type = SfcTypeEnum.Produce });

            var notAllowStatus = new[] { SfcStatusEnum.Locked, SfcStatusEnum.Delete, SfcStatusEnum.Invalid };//检测条码状态
            var notAllowSfcs = sfcEntities.Where(x => notAllowStatus.Contains(x.Status)).Select(x => x.SFC);
            if (notAllowSfcs != null && notAllowSfcs.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15438)).WithData("sfc", string.Join(",", notAllowSfcs.ToArray()));
            }

            var notExistSfcs = needHandleSfcs.Except(sfcEntities.Select(x => x.SFC).Distinct());//检测条码是否存在
            if (notExistSfcs != null && notExistSfcs.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES15439)).WithData("sfc", string.Join(",", notExistSfcs));

            //检测 不合格代码
            // 获取不合格代码列表
            var unqualifiedCodes = excelImportDtos.Select(x => x.UnqualifiedCode).Distinct();
            var qualUnqualifiedCodes = await _qualUnqualifiedCodeRepository.GetByCodesAsync(new QualUnqualifiedCodeByCodesQuery { Codes = unqualifiedCodes, SiteId = _currentSite.SiteId ?? 0 });

            var notExistUnquaCodes = excelImportDtos.Select(x => x.UnqualifiedCode).Distinct().Except(qualUnqualifiedCodes.Select(x => x.UnqualifiedCode).Distinct());
            if (notExistUnquaCodes != null && notExistUnquaCodes.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES15441)).WithData("unqualifiedCode", string.Join(",", notExistUnquaCodes)); //检测不合格代码是否存在

            var notMarkUnqualifiedCodes = qualUnqualifiedCodes.Where(x => x.Type != QualUnqualifiedCodeTypeEnum.Mark);//获取不是标识的不合格代码
            if (notMarkUnqualifiedCodes != null && notMarkUnqualifiedCodes.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES15437)).WithData("unqualifiedCode", string.Join(",", notMarkUnqualifiedCodes));

            //检测 发现工序
            var procedures = await _procProcedureRepository.GetProcProcedureEntitiesAsync(new ProcProcedureQuery() { SiteId = _currentSite.SiteId ?? 0, Codes = excelImportDtos.Select(x => x.FoundBadOperationCode).ToArray() });

            var notExistProcedureCodes = excelImportDtos.Select(x => x.FoundBadOperationCode).Distinct().Except(procedures.Select(x => x.Code).Distinct());
            if (notExistProcedureCodes != null && notExistProcedureCodes.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES15443)).WithData("foundBadOperationCode", string.Join(",", notExistProcedureCodes)); //检测工序是否存在

            //检测 拦截工序
            var interceptProcedureCodes = excelImportDtos.Where(x => !string.IsNullOrWhiteSpace(x.InterceptProcedureCode))?.Select(x => x.InterceptProcedureCode ?? "")?.Distinct();

            var interceptProcedures = interceptProcedureCodes != null && interceptProcedureCodes.Any() ? await _procProcedureRepository.GetProcProcedureEntitiesAsync(new ProcProcedureQuery() { SiteId = _currentSite.SiteId ?? 0, Codes = interceptProcedureCodes.ToArray() }) : null;

            if (interceptProcedureCodes != null && interceptProcedureCodes.Any())
            {
                var notExistInterceptProcedureCodes = interceptProcedureCodes!.Except(interceptProcedures != null && interceptProcedures.Any() ? interceptProcedures.Select(x => x.Code).Distinct() : new List<string>());
                if (notExistInterceptProcedureCodes != null && notExistInterceptProcedureCodes.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES15444)).WithData("interceptProcedureCode", string.Join(",", notExistInterceptProcedureCodes)); //检测拦截工序是否存在
            }

            //检测是否与数据库中的重复
            var sfcsBadRecords = await _manuProductBadRecordRepository.GetManuProductBadRecordEntitiesBySFCAsync(new ManuProductBadRecordBySfcQuery { SiteId = _currentSite.SiteId ?? 0, SFCs = needHandleSfcs, Status = ProductBadRecordStatusEnum.Open });



            var handleExcelDtos = excelImportDtos.Select(x => new
            {
                x.SFC,

                FoundBadOperationId = procedures.FirstOrDefault(y => y.Code == x.FoundBadOperationCode)?.Id,
                x.FoundBadOperationCode,

                UnqualifiedId = qualUnqualifiedCodes.FirstOrDefault(y => y.UnqualifiedCode == x.UnqualifiedCode)?.Id,
                x.UnqualifiedCode,

                InterceptProcedureId = interceptProcedures?.FirstOrDefault(y => y.Code == x.InterceptProcedureCode)?.Id,
                x.InterceptProcedureCode,
                x.Remark
            });
            var addGroupKeys = handleExcelDtos.GroupBy(x => new { x.SFC, x.FoundBadOperationId, x.UnqualifiedId, x.InterceptProcedureId });

            var groups = sfcsBadRecords.GroupBy(x => new { x.SFC, x.FoundBadOperationId, x.UnqualifiedId, x.InterceptOperationId });

            //查询工序信息
            var procProcedureEntities = await _procProcedureRepository.GetProcProcedureEntitiesAsync(new ProcProcedureQuery { SiteId = _currentSite.SiteId ?? 0 });

            foreach (var item in groups)
            {
                if (addGroupKeys.Any(x => x.Key.SFC == item.Key.SFC && x.Key.FoundBadOperationId == item.Key.FoundBadOperationId && x.Key.UnqualifiedId == item.Key.UnqualifiedId && x.Key.InterceptProcedureId == item.Key.InterceptOperationId))
                {
                    //查询工序
                    //var procedure = procedures.FirstOrDefault(x=>x.Id== item.Key.FoundBadOperationId);

                    //发现不良工序
                    var foundBadprocedure = procProcedureEntities.FirstOrDefault(a => a.Id == item.Key.FoundBadOperationId);
                    //拦截工序
                    var interceptprocedure = procProcedureEntities.FirstOrDefault(a => a.Id == item.Key.InterceptOperationId);

                    //查询不合格代码
                    var unqualifiedCode = qualUnqualifiedCodes.FirstOrDefault(x => x.Id == item.Key.UnqualifiedId);

                    throw new CustomerValidationException(nameof(ErrorCode.MES15436)).WithData("sfc", item.Key.SFC).WithData("foundBadOperationCode", foundBadprocedure?.Code ?? "").WithData("unqualifiedCode", unqualifiedCode?.UnqualifiedCode ?? "").WithData("InterceptOperationCode", interceptprocedure?.Code ?? "");
                }
            }
            #endregion

            //条码信息表
            var sfcInfoEntities = await _manuSfcInfoRepository.GetBySFCIdsWithIsUseAsync(sfcEntities.Select(x => x.Id));
            var manuSfcProducePagedQuery = new ManuSfcProduceQuery { Sfcs = needHandleSfcs, SiteId = _currentSite.SiteId ?? 00 };
            //在制品信息
            var manuSfcProduces = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(manuSfcProducePagedQuery);


            var sfcStepList = new List<ManuSfcStepEntity>();
            var manuProductBadRecords = new List<ManuProductBadRecordEntity>();

            foreach (var item in handleExcelDtos)
            {
                var sfcEntity = sfcEntities.Where(x => x.SFC == item.SFC).FirstOrDefault();
                var sfcInfoEntity = sfcInfoEntities.Where(x => x.SfcId == sfcEntity?.Id).FirstOrDefault();

                var manuSfcProduceInfoEntity = manuSfcProduces.FirstOrDefault(x => x.SFC == item.SFC);

                //步骤表-不良录入
                var manuSfcStepEntity = new ManuSfcStepEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SFC = item.SFC,
                    ProductId = sfcInfoEntity?.ProductId ?? 0,
                    WorkOrderId = sfcInfoEntity?.WorkOrderId ?? 0,
                    WorkCenterId = manuSfcProduceInfoEntity?.WorkCenterId,
                    ProductBOMId = manuSfcProduceInfoEntity?.ProductBOMId,
                    ProcessRouteId = manuSfcProduceInfoEntity.ProcessRouteId,
                    Qty = sfcEntity?.Qty ?? 0,
                    EquipmentId = manuSfcProduceInfoEntity?.EquipmentId,
                    ResourceId = manuSfcProduceInfoEntity?.ResourceId,
                    ProcedureId = manuSfcProduceInfoEntity?.ProcedureId,
                    Operatetype = ManuSfcStepTypeEnum.BadEntry,
                    CurrentStatus = sfcEntity!.Status,
                    SiteId = _currentSite.SiteId ?? 0,
                    CreatedOn = HymsonClock.Now(),
                    CreatedBy = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentUser.UserName,
                };
                sfcStepList.Add(manuSfcStepEntity);

                var manuProductBadRecordEntity = new ManuProductBadRecordEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    FoundBadOperationId = item.FoundBadOperationId ?? 0,
                    //OutflowOperationId = manuProductBadRecordCreateDto.OutflowOperationId,
                    UnqualifiedId = item.UnqualifiedId ?? 0,
                    SfcStepId = manuSfcStepEntity.Id,
                    SFC = item.SFC,
                    SfcInfoId = sfcInfoEntity?.Id,
                    Qty = sfcEntity.Qty,
                    Source = ProductBadRecordSourceEnum.BadManualEntry,
                    Remark = item.Remark ?? "",
                    CreatedOn = HymsonClock.Now(),
                    CreatedBy = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentUser.UserName,

                    Status = ProductBadRecordStatusEnum.Open,
                    InterceptOperationId = item.InterceptProcedureId.GetValueOrDefault(),
                };
                manuProductBadRecords.Add(manuProductBadRecordEntity);
            }

            using (var trans = TransactionHelper.GetTransactionScope())
            {
                //插入不合格记录
                if (manuProductBadRecords != null && manuProductBadRecords.Any())
                {
                    //await _manuProductBadRecordRepository.InsertRangeAsync(manuProductBadRecords);
                    var insertResult = await _manuProductBadRecordRepository.InsertIgnoreRangeAsync(manuProductBadRecords);
                    if (insertResult != manuProductBadRecords.Count)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES19710));
                    }
                }
                //插入步骤表
                if (sfcStepList != null && sfcStepList.Any())
                {
                    await _manuSfcStepRepository.InsertRangeAsync(sfcStepList);
                }

                trans.Complete();
            }
        }

        #endregion
    }
}
