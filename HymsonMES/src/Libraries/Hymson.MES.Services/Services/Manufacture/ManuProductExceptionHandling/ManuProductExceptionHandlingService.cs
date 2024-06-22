using FluentValidation;
using FluentValidation.Results;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Excel.Abstractions;
using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Constants.Manufacture;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums.Warehouse;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Linq;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 服务（生产异常处理）
    /// </summary>
    public class ManuProductExceptionHandlingService : IManuProductExceptionHandlingService
    {
        /// <summary>
        /// 日志对象
        /// </summary>
        private readonly ILogger<ManuProductExceptionHandlingService> _logger;

        /// <summary>
        /// 当前对象（登录用户）
        /// </summary>
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 当前对象（站点）
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 服务接口（Excel）
        /// </summary>
        private readonly IExcelService _excelService;

        /// <summary>
        /// 服务接口（多语言）
        /// </summary>
        private readonly ILocalizationService _localizationService;

        /// <summary>
        /// 服务接口（生产通用）
        /// </summary>
        private readonly IManuCommonService _manuCommonService;

        /// <summary>
        /// 服务接口（主数据）
        /// </summary>
        private readonly IMasterDataService _masterDataService;

        /// <summary>
        ///  仓储接口（条码）
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        /// <summary>
        ///  仓储接口（条码信息）
        /// </summary>
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;

        /// <summary>
        ///  仓储接口（条码在制）
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 仓储接口（条码步骤）
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

        /// <summary>
        /// 仓储接口（产品不良录入）
        /// </summary>
        private readonly IManuProductBadRecordRepository _manuProductBadRecordRepository;

        /// <summary>
        /// 仓储接口（产品NG记录）
        /// </summary>
        private readonly IManuProductNgRecordRepository _manuProductNgRecordRepository;

        /// <summary>
        ///  仓储接口（物料）
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 仓储接口（工序）
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;

        /// <summary>
        ///  仓储接口（工单）
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        /// 仓储接口（不合格代码）
        /// </summary>
        private readonly IQualUnqualifiedCodeRepository _qualUnqualifiedCodeRepository;

        /// <summary>
        ///  仓储接口（物料库存）
        /// </summary>
        private readonly IWhMaterialInventoryRepository _whMaterialInventoryRepository;

        /// <summary>
        ///  仓储接口（物料台账）
        /// </summary>
        private readonly IWhMaterialStandingbookRepository _whMaterialStandingbookRepository;

        /// <summary>
        /// 构造函数（生产异常处理）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="excelService"></param>
        /// <param name="localizationService"></param>
        /// <param name="manuCommonService"></param>
        /// <param name="masterDataService"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="manuSfcInfoRepository"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="manuProductBadRecordRepository"></param>
        /// <param name="manuProductNgRecordRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="qualUnqualifiedCodeRepository"></param>
        /// <param name="whMaterialInventoryRepository"></param>
        /// <param name="whMaterialStandingbookRepository"></param>
        public ManuProductExceptionHandlingService(ILogger<ManuProductExceptionHandlingService> logger,
            ICurrentUser currentUser, ICurrentSite currentSite, IExcelService excelService, ILocalizationService localizationService,
            IManuCommonService manuCommonService,
            IMasterDataService masterDataService,
            IManuSfcRepository manuSfcRepository,
            IManuSfcInfoRepository manuSfcInfoRepository,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IManuSfcStepRepository manuSfcStepRepository,
            IManuProductBadRecordRepository manuProductBadRecordRepository,
            IManuProductNgRecordRepository manuProductNgRecordRepository,
            IProcMaterialRepository procMaterialRepository,
            IProcProcedureRepository procProcedureRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IQualUnqualifiedCodeRepository qualUnqualifiedCodeRepository,
            IWhMaterialInventoryRepository whMaterialInventoryRepository,
            IWhMaterialStandingbookRepository whMaterialStandingbookRepository)
        {
            _logger = logger;
            _currentUser = currentUser;
            _currentSite = currentSite;
            _excelService = excelService;
            _localizationService = localizationService;
            _manuCommonService = manuCommonService;
            _masterDataService = masterDataService;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _manuProductBadRecordRepository = manuProductBadRecordRepository;
            _manuProductNgRecordRepository = manuProductNgRecordRepository;
            _procMaterialRepository = procMaterialRepository;
            _procProcedureRepository = procProcedureRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _qualUnqualifiedCodeRepository = qualUnqualifiedCodeRepository;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _whMaterialStandingbookRepository = whMaterialStandingbookRepository;
        }



        #region 让步接收
        /// <summary>
        /// 根据条码查询信息（让步接收）
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuCompromiseBarCodeDto>> GetCompromiseByBarCodeAsync(string barCode)
        {
            if (string.IsNullOrEmpty(barCode)) throw new CustomerValidationException(nameof(ErrorCode.MES15445));

            // 站点
            var siteId = _currentSite.SiteId ?? 0;
            var dtos = await GetInfoByBarCodesAsync(siteId, new List<string> { barCode });
            return dtos.Select(s => s.ToDto<ManuCompromiseBarCodeDto>());
        }

        /// <summary>
        /// 提交（让步接收）
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<int> SubmitCompromiseAsync(ManuCompromiseDto requestDto)
        {
            if (requestDto == null) return 0;
            if (requestDto.Compromises == null || !requestDto.Compromises.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES15445));

            // 初始化请求参数
            var dataBo = new CompromiseRequestBo
            {
                SiteId = _currentSite.SiteId ?? 0,
                UpdatedBy = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now(),
                Remark = requestDto.Remark ?? ""
            };

            // 传入的条码
            var barCodes = requestDto.Compromises.Select(s => s.BarCode);

            // 查询条码
            dataBo.SFCEntities = await _manuSfcRepository.GetListAsync(new ManuSfcQuery
            {
                SiteId = dataBo.SiteId,
                SFCs = barCodes
            });
            if (dataBo.SFCEntities == null || !dataBo.SFCEntities.Any()) return 0;

            // 状态校验
            var validationFailures = new List<ValidationFailure>();
            var noMatchSFCEntities = dataBo.SFCEntities.Where(w => ManuSfcStatus.ForbidSfcStatuss.Contains(w.Status));
            if (noMatchSFCEntities.Any())
            {
                foreach (var sfcEntity in noMatchSFCEntities)
                {
                    var validationFailure = new ValidationFailure() { FormattedMessagePlaceholderValues = new() };
                    validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", sfcEntity.SFC);
                    validationFailure.FormattedMessagePlaceholderValues.Add("barCode", sfcEntity.SFC);
                    validationFailure.FormattedMessagePlaceholderValues.Add("status", sfcEntity.Status.GetDescription());
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15447);
                    validationFailures.Add(validationFailure);
                }

                if (validationFailures.Any()) throw new ValidationException("", validationFailures);
            }

            // 查询条码信息
            dataBo.SFCInfoEntities = await _manuSfcInfoRepository.GetBySFCIdsAsync(dataBo.SFCEntities.Select(s => s.Id));

            // 读取在制品
            dataBo.SFCProduceEntities = await _manuSfcProduceRepository.GetBySFCIdsAsync(dataBo.SFCEntities.Where(w => w.Type == SfcTypeEnum.Produce).Select(s => s.Id));

            // 读取产品
            dataBo.ProductEntities = await _procMaterialRepository.GetByIdsAsync(dataBo.SFCInfoEntities.Select(s => s.ProductId));

            // 查询传入的不合格代码
            dataBo.UnqualifiedCodeEntities = await _qualUnqualifiedCodeRepository.GetByIdsAsync(requestDto.Compromises.Where(w => w.UnqualifiedCodeId.HasValue).Select(s => s.UnqualifiedCodeId!.Value));

            // 查询工序
            var procedureIds = requestDto.Compromises.Where(w => w.FoundProcedureId.HasValue).Select(s => s.FoundProcedureId!.Value);
            procedureIds = procedureIds.Union(requestDto.Compromises.Where(w => w.OutProcedureId.HasValue).Select(s => s.OutProcedureId!.Value));

            procedureIds = procedureIds.Distinct();
            dataBo.ProcedureEntities = await _procProcedureRepository.GetByIdsAsync(procedureIds);

            // 查询不合格记录（开启的状态）
            var allBadRecordEntities = await _manuProductBadRecordRepository.GetManuProductBadRecordEntitiesBySFCAsync(new ManuProductBadRecordBySfcQuery
            {
                SiteId = dataBo.SiteId,
                Status = ProductBadRecordStatusEnum.Open,
                SFCs = barCodes
            });
            //if (allBadRecordEntities == null || !allBadRecordEntities.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES15451)).WithData("barCode", string.Join(',', barCodes));
            dataBo.BadRecordEntitiesDict = allBadRecordEntities.ToLookup(x => x.SFC).ToDictionary(d => d.Key, d => d);

            // 遍历所有条码
            List<ManuSfcStepEntity> sfcStepEntities = new();
            List<ManuProductBadRecordUpdateCommand> badRecordUpdateCommands = new();
            List<WhMaterialInventoryEntity> materialInventoryEntities = new();
            List<WhMaterialStandingbookEntity> materialStandingbookEntities = new();
            List<ManuProductBadRecordEntity> productBadRecordEntities = new();
            List<ManuProductNgRecordEntity> productNgRecordEntities = new();
            List<PhysicalDeleteSFCProduceByIdsCommand> physicalDeleteSFCProduceByIdsCommands = new();
            PhysicalDeleteSFCProduceByIdsCommand physicalDeleteSFCProduceByIdsCommand = new() { SiteId = dataBo.SiteId };

            List<long> sfcProduceIds = new();
            foreach (var dto in requestDto.Compromises)
            {
                var validationFailure = new ValidationFailure() { FormattedMessagePlaceholderValues = new() };
                validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", dto.BarCode);

                if (!dto.UnqualifiedCodeId.HasValue)
                {
                    validationFailure.ErrorCode = nameof(ErrorCode.MES19702);
                    validationFailures.Add(validationFailure);
                    continue;
                }
                if (!dto.FoundProcedureId.HasValue)
                {
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15433);
                    validationFailures.Add(validationFailure);
                    continue;
                }
                if (!dto.OutProcedureId.HasValue)
                {
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15457);
                    validationFailures.Add(validationFailure);
                    continue;
                }

                // 条码
                var sfcEntity = dataBo.SFCEntities.FirstOrDefault(f => f.SFC == dto.BarCode);
                if (sfcEntity == null) continue;

                // 条码信息
                var sfcInfoEntity = dataBo.SFCInfoEntities.FirstOrDefault(f => f.SfcId == sfcEntity.Id);
                if (sfcInfoEntity == null) continue;

                // 不合格代码
                var unqualifiedCodeEntity = dataBo.UnqualifiedCodeEntities.FirstOrDefault(f => f.Id == dto.UnqualifiedCodeId);
                if (unqualifiedCodeEntity == null) continue;

                // 关闭不合格记录（如果有的话）
                if (dataBo.BadRecordEntitiesDict.TryGetValue(sfcEntity.SFC, out var badRecordEntities))
                {
                    badRecordUpdateCommands.AddRange(badRecordEntities.Select(s => new ManuProductBadRecordUpdateCommand
                    {
                        Id = s.Id,
                        Status = ProductBadRecordStatusEnum.Close,
                        DisposalResult = ProductBadDisposalResultEnum.Compromise,
                        UpdatedOn = dataBo.UpdatedOn,
                        UserId = dataBo.UpdatedBy,
                        Remark = dataBo.Remark
                    }));
                }

                var sfcStepId = 0L;
                if (sfcEntity.Type == SfcTypeEnum.Produce)
                {
                    // 在制品
                    var sfcProduceEntity = dataBo.SFCProduceEntities.FirstOrDefault(f => f.SFCId == sfcEntity.Id);
                    if (sfcProduceEntity == null) continue;

                    // 初始化步骤数据
                    sfcStepId = IdGenProvider.Instance.CreateId();
                    var stepEntity = new ManuSfcStepEntity
                    {
                        Id = sfcStepId,
                        Operatetype = ManuSfcStepTypeEnum.Compromise,
                        CurrentStatus = sfcProduceEntity.Status,
                        SFC = sfcProduceEntity.SFC,
                        ProductId = sfcProduceEntity.ProductId,
                        WorkOrderId = sfcProduceEntity.WorkOrderId,
                        WorkCenterId = sfcProduceEntity.WorkCenterId,
                        ProductBOMId = sfcProduceEntity.ProductBOMId,
                        ProcessRouteId = sfcProduceEntity.ProcessRouteId,
                        SFCInfoId = sfcProduceEntity.BarCodeInfoId,
                        Qty = sfcProduceEntity.Qty,
                        VehicleCode = "", // 这里要赋值？
                        ProcedureId = sfcProduceEntity.ProcedureId,
                        ResourceId = sfcProduceEntity.ResourceId,
                        EquipmentId = sfcProduceEntity.EquipmentId,
                        Remark = dataBo.Remark,
                        SiteId = dataBo.SiteId,
                        CreatedBy = dataBo.UpdatedBy,
                        CreatedOn = dataBo.UpdatedOn,
                        UpdatedBy = dataBo.UpdatedBy,
                        UpdatedOn = dataBo.UpdatedOn
                    };

                    // 如果是在制完成，改为完成（默认就处于合格工艺路线里面）
                    if (sfcProduceEntity.Status == SfcStatusEnum.InProductionComplete)
                    {
                        // 产品信息
                        var productEntity = dataBo.ProductEntities.FirstOrDefault(f => f.Id == sfcProduceEntity.ProductId);
                        if (productEntity == null) continue;

                        stepEntity.Operatetype = ManuSfcStepTypeEnum.OutStock;
                        stepEntity.AfterOperationStatus = SfcStatusEnum.Complete;

                        // 再次添加步骤
                        sfcStepEntities.Add(stepEntity);

                        // 删除在制
                        sfcProduceIds.Add(sfcProduceEntity.Id);

                        // 新增 wh_material_inventory
                        materialInventoryEntities.Add(new WhMaterialInventoryEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            SupplierId = 0,//自制品 没有
                            MaterialId = sfcProduceEntity.ProductId,
                            MaterialBarCode = sfcProduceEntity.SFC,
                            //Batch = "",//自制品 没有
                            MaterialType = MaterialInventoryMaterialTypeEnum.SelfMadeParts,
                            QuantityResidue = sfcProduceEntity.Qty,
                            Status = WhMaterialInventoryStatusEnum.ToBeUsed,
                            Source = MaterialInventorySourceEnum.ManuComplete,
                            SiteId = dataBo.SiteId,
                            CreatedBy = dataBo.UpdatedBy,
                            CreatedOn = dataBo.UpdatedOn,
                            UpdatedBy = dataBo.UpdatedBy,
                            UpdatedOn = dataBo.UpdatedOn
                        });

                        // 新增 wh_material_standingbook
                        materialStandingbookEntities.Add(new WhMaterialStandingbookEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            MaterialCode = productEntity.MaterialCode,
                            MaterialName = productEntity.MaterialName,
                            MaterialVersion = productEntity.Version ?? "",
                            MaterialBarCode = sfcProduceEntity.SFC,
                            //Batch = "",//自制品 没有
                            Quantity = sfcProduceEntity.Qty,
                            Unit = productEntity.Unit ?? "",
                            Type = WhMaterialInventoryTypeEnum.ManuComplete,
                            Source = MaterialInventorySourceEnum.ManuComplete,
                            SiteId = dataBo.SiteId,
                            CreatedBy = dataBo.UpdatedBy,
                            CreatedOn = dataBo.UpdatedOn,
                            UpdatedBy = dataBo.UpdatedBy,
                            UpdatedOn = dataBo.UpdatedOn
                        });
                    }

                    // 添加步骤
                    sfcStepEntities.Add(stepEntity);
                }
                else if (sfcEntity.Type == SfcTypeEnum.NoProduce)
                {
                    // 产品信息
                    var productEntity = dataBo.ProductEntities.FirstOrDefault(f => f.Id == sfcInfoEntity.ProductId);
                    if (productEntity == null) continue;

                    // 添加步骤
                    sfcStepId = IdGenProvider.Instance.CreateId();
                    sfcStepEntities.Add(new ManuSfcStepEntity
                    {
                        Id = sfcStepId,
                        Operatetype = ManuSfcStepTypeEnum.Compromise,
                        CurrentStatus = SfcStatusEnum.Complete,
                        SFC = sfcEntity.SFC,
                        ProductId = sfcInfoEntity.ProductId,
                        WorkOrderId = sfcInfoEntity.WorkOrderId ?? 0,
                        SFCInfoId = sfcInfoEntity.Id,
                        Qty = sfcEntity.Qty,
                        Remark = dataBo.Remark,
                        SiteId = dataBo.SiteId,
                        CreatedBy = dataBo.UpdatedBy,
                        CreatedOn = dataBo.UpdatedOn,
                        UpdatedBy = dataBo.UpdatedBy,
                        UpdatedOn = dataBo.UpdatedOn
                    });
                }

                // 是否有生成步骤ID
                if (sfcStepId == 0) continue;

                // 添加不良记录
                var badRecordId = IdGenProvider.Instance.CreateId();
                productBadRecordEntities.Add(new ManuProductBadRecordEntity
                {
                    Id = badRecordId,
                    FoundBadOperationId = dto.FoundProcedureId!.Value,
                    FoundBadResourceId = null,
                    OutflowOperationId = dto.OutProcedureId!.Value,
                    UnqualifiedId = unqualifiedCodeEntity.Id,
                    SfcStepId = sfcStepId,
                    SFC = sfcEntity.SFC,
                    SfcInfoId = sfcInfoEntity.Id,
                    Qty = sfcEntity.Qty,
                    Status = ProductBadRecordStatusEnum.Close,
                    Source = ProductBadRecordSourceEnum.BadManualEntry,
                    DisposalResult = ProductBadDisposalResultEnum.Rework,
                    Remark = dataBo.Remark,
                    SiteId = dataBo.SiteId,
                    CreatedBy = dataBo.UpdatedBy,
                    CreatedOn = dataBo.UpdatedOn,
                    UpdatedBy = dataBo.UpdatedBy,
                    UpdatedOn = dataBo.UpdatedOn
                });

                // 添加NG记录
                productNgRecordEntities.Add(new ManuProductNgRecordEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    BadRecordId = badRecordId,
                    UnqualifiedId = unqualifiedCodeEntity.Id,
                    NGCode = unqualifiedCodeEntity.UnqualifiedCode,
                    Remark = dataBo.Remark,
                    SiteId = dataBo.SiteId,
                    CreatedBy = dataBo.UpdatedBy,
                    CreatedOn = dataBo.UpdatedOn,
                    UpdatedBy = dataBo.UpdatedBy,
                    UpdatedOn = dataBo.UpdatedOn
                });
            }

            if (validationFailures.Any()) throw new ValidationException("", validationFailures);

            // 需要删除的条码ID
            physicalDeleteSFCProduceByIdsCommand.Ids = sfcProduceIds;

            using var trans = TransactionHelper.GetTransactionScope();
            List<Task<int>> tasks = new()
            {
                // 关闭不合格记录
               _manuProductBadRecordRepository.UpdateStatusByIdRangeAsync(badRecordUpdateCommands),

                // 插入 manu_sfc_step
                _manuSfcStepRepository.InsertRangeAsync(sfcStepEntities),

                // 删除在制
                _manuSfcProduceRepository.DeletePhysicalRangeByIdsAsync(physicalDeleteSFCProduceByIdsCommand),

                // 插入不良记录
                _manuProductBadRecordRepository.InsertRangeAsync(productBadRecordEntities),

                // 插入NG记录
                _manuProductNgRecordRepository.InsertRangeAsync(productNgRecordEntities)
            };

            var rowArray = await Task.WhenAll(tasks);
            trans.Complete();
            return rowArray.Sum();
        }

        /// <summary>
        /// 下载导入模板（让步接收）
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public async Task<string> DownloadCompromiseImportTemplateAsync(Stream stream)
        {
            var worksheetName = "产品异常处理-让步接收";
            await _excelService.ExportAsync(Array.Empty<ManuCompromiseExcelDto>(), stream, worksheetName);
            return worksheetName;
        }

        /// <summary>
        /// 导入（让步接收）
        /// </summary>
        /// <returns></returns>
        public async Task ImportCompromiseAsync(IFormFile formFile)
        {
            using MemoryStream memoryStream = new();
            await formFile.CopyToAsync(memoryStream).ConfigureAwait(false);
            var dtos = _excelService.Import<ManuCompromiseExcelDto>(memoryStream);
            if (dtos == null || !dtos.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES10133));

            /*
            // 备份用户上传的文件，可选
            var stream = formFile.OpenReadStream();
            var uploadResult = await _minioService.PutObjectAsync(formFile.FileName, stream, formFile.ContentType);
            */

            var validationFailures = new List<ValidationFailure>();
            var index = 0;
            foreach (var item in dtos)
            {
                index++;

                // 校验产品序列码
                if (string.IsNullOrEmpty(item.BarCode))
                {
                    var validationFailure = new ValidationFailure() { FormattedMessagePlaceholderValues = new() };
                    validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", index);
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15466);
                    validationFailures.Add(validationFailure);
                }
                // 校验发现工序
                if (string.IsNullOrEmpty(item.FoundProcedure))
                {
                    var validationFailure = new ValidationFailure() { FormattedMessagePlaceholderValues = new() };
                    validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", index);
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15463);
                    validationFailures.Add(validationFailure);
                }
                // 校验不合格代码
                if (string.IsNullOrEmpty(item.UnqualifiedCode))
                {
                    var validationFailure = new ValidationFailure() { FormattedMessagePlaceholderValues = new() };
                    validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", index);
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15464);
                    validationFailures.Add(validationFailure);
                }
                // 校验流出工序
                if (string.IsNullOrEmpty(item.OutProcedure))
                {
                    var validationFailure = new ValidationFailure() { FormattedMessagePlaceholderValues = new() };
                    validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", index);
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15457);
                    validationFailures.Add(validationFailure);
                }
            }

            if (validationFailures.Any()) throw new ValidationException(_localizationService.GetResource("ExcelRowError"), validationFailures);

            ManuCompromiseDto requestDto = await GetCompromiseDtosFromExcelAsync(dtos);
            if (requestDto == null || requestDto.Compromises == null || !requestDto.Compromises.Any()) return;

            await SubmitCompromiseAsync(requestDto);
        }
        #endregion


        #region 设备误判
        /// <summary>
        /// 根据条码查询信息（设备误判）
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuMisjudgmentBarCodeDto>> GetMisjudgmentByBarCodeAsync(string barCode)
        {
            if (string.IsNullOrEmpty(barCode)) throw new CustomerValidationException(nameof(ErrorCode.MES15445));

            // 站点
            var siteId = _currentSite.SiteId ?? 0;
            var dtos = await GetNgInfoByBarCodesAsync(siteId, new List<string> { barCode });
            return dtos.Select(s => s.ToDto<ManuMisjudgmentBarCodeDto>());
        }

        /// <summary>
        /// 提交（设备误判）
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<int> SubmitMisjudgmentAsync(ManuMisjudgmentDto requestDto)
        {
            if (requestDto == null) return 0;
            if (requestDto.BarCodes == null || !requestDto.BarCodes.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES15445));

            // 更新时间
            var siteId = _currentSite.SiteId ?? 0;
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // 查询条码
            var sfcEntities = await _manuSfcRepository.GetListAsync(new ManuSfcQuery
            {
                SiteId = siteId,
                SFCs = requestDto.BarCodes
            });
            if (sfcEntities == null || !sfcEntities.Any()) return 0;

            // 查询条码信息
            var sfcInfoEntities = await _manuSfcInfoRepository.GetBySFCIdsAsync(sfcEntities.Select(s => s.Id));

            // 读取在制品
            var sfcProduceEntities = await _manuSfcProduceRepository.GetBySFCIdsAsync(sfcEntities.Where(w => w.Type == SfcTypeEnum.Produce).Select(s => s.Id));

            // 读取产品
            var productEntities = await _procMaterialRepository.GetByIdsAsync(sfcInfoEntities.Select(s => s.ProductId));

            // 查询不合格记录（开启的状态）
            var allBadRecordEntities = await _manuProductBadRecordRepository.GetManuProductBadRecordEntitiesBySFCAsync(new ManuProductBadRecordBySfcQuery
            {
                SiteId = siteId,
                Status = ProductBadRecordStatusEnum.Open,
                SFCs = requestDto.BarCodes
            });
            if (allBadRecordEntities == null || !allBadRecordEntities.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES15451)).WithData("barCode", string.Join(',', requestDto.BarCodes));
            var allBadRecordEntitiesDict = allBadRecordEntities.ToLookup(x => x.SFC).ToDictionary(d => d.Key, d => d);

            // 遍历所有条码
            List<ManuSfcStepEntity> sfcStepEntities = new();
            List<ManuProductBadRecordUpdateCommand> badRecordUpdateCommands = new();
            List<WhMaterialInventoryEntity> materialInventoryEntities = new();
            List<WhMaterialStandingbookEntity> materialStandingbookEntities = new();
            List<PhysicalDeleteSFCProduceByIdsCommand> physicalDeleteSFCProduceByIdsCommands = new();
            PhysicalDeleteSFCProduceByIdsCommand physicalDeleteSFCProduceByIdsCommand = new() { SiteId = siteId };

            List<long> sfcProduceIds = new();
            foreach (var sfcEntity in sfcEntities)
            {
                // 每个条码的不合格记录
                if (!allBadRecordEntitiesDict.TryGetValue(sfcEntity.SFC, out var badRecordEntities)) continue;

                // 关闭不合格
                badRecordUpdateCommands.AddRange(badRecordEntities.Select(s => new ManuProductBadRecordUpdateCommand
                {
                    Id = s.Id,
                    Status = ProductBadRecordStatusEnum.Close,
                    DisposalResult = ProductBadDisposalResultEnum.Misjudgment,
                    UpdatedOn = updatedOn,
                    UserId = updatedBy,
                    Remark = requestDto.Remark ?? ""
                }));

                if (sfcEntity.Type == SfcTypeEnum.Produce)
                {
                    // 在制品
                    var sfcProduceEntity = sfcProduceEntities.FirstOrDefault(f => f.SFCId == sfcEntity.Id);
                    if (sfcProduceEntity == null) continue;

                    // 初始化步骤数据
                    var stepEntity = new ManuSfcStepEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        Operatetype = ManuSfcStepTypeEnum.Misjudgment,
                        CurrentStatus = sfcProduceEntity.Status,
                        SFC = sfcProduceEntity.SFC,
                        ProductId = sfcProduceEntity.ProductId,
                        WorkOrderId = sfcProduceEntity.WorkOrderId,
                        WorkCenterId = sfcProduceEntity.WorkCenterId,
                        ProductBOMId = sfcProduceEntity.ProductBOMId,
                        ProcessRouteId = sfcProduceEntity.ProcessRouteId,
                        SFCInfoId = sfcProduceEntity.BarCodeInfoId,
                        Qty = sfcProduceEntity.Qty,
                        VehicleCode = "", // 这里要赋值？
                        ProcedureId = sfcProduceEntity.ProcedureId,
                        ResourceId = sfcProduceEntity.ResourceId,
                        EquipmentId = sfcProduceEntity.EquipmentId,
                        Remark = requestDto.Remark,
                        SiteId = sfcEntity.SiteId,
                        CreatedBy = updatedBy,
                        CreatedOn = updatedOn,
                        UpdatedBy = updatedBy,
                        UpdatedOn = updatedOn
                    };
                    sfcStepEntities.Add(stepEntity);

                    // 如果是在制完成，改为完成（默认就处于合格工艺路线里面）
                    if (sfcProduceEntity.Status == SfcStatusEnum.InProductionComplete)
                    {
                        // 产品信息
                        var productEntity = productEntities.FirstOrDefault(f => f.Id == sfcProduceEntity.ProductId);
                        if (productEntity == null) continue;

                        stepEntity.Id = IdGenProvider.Instance.CreateId();
                        stepEntity.Operatetype = ManuSfcStepTypeEnum.OutStock;
                        stepEntity.AfterOperationStatus = SfcStatusEnum.Complete;

                        // 再次添加步骤
                        sfcStepEntities.Add(stepEntity);

                        // 删除在制
                        sfcProduceIds.Add(sfcProduceEntity.Id);

                        // 新增 wh_material_inventory
                        materialInventoryEntities.Add(new WhMaterialInventoryEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            SupplierId = 0,//自制品 没有
                            MaterialId = sfcProduceEntity.ProductId,
                            MaterialBarCode = sfcProduceEntity.SFC,
                            //Batch = "",//自制品 没有
                            MaterialType = MaterialInventoryMaterialTypeEnum.SelfMadeParts,
                            QuantityResidue = sfcProduceEntity.Qty,
                            Status = WhMaterialInventoryStatusEnum.ToBeUsed,
                            Source = MaterialInventorySourceEnum.ManuComplete,
                            SiteId = sfcEntity.SiteId,
                            CreatedBy = updatedBy,
                            CreatedOn = updatedOn,
                            UpdatedBy = updatedBy,
                            UpdatedOn = updatedOn
                        });

                        // 新增 wh_material_standingbook
                        materialStandingbookEntities.Add(new WhMaterialStandingbookEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            MaterialCode = productEntity.MaterialCode,
                            MaterialName = productEntity.MaterialName,
                            MaterialVersion = productEntity.Version ?? "",
                            MaterialBarCode = sfcProduceEntity.SFC,
                            //Batch = "",//自制品 没有
                            Quantity = sfcProduceEntity.Qty,
                            Unit = productEntity.Unit ?? "",
                            Type = WhMaterialInventoryTypeEnum.ManuComplete,
                            Source = MaterialInventorySourceEnum.ManuComplete,
                            SiteId = sfcEntity.SiteId,
                            CreatedBy = updatedBy,
                            CreatedOn = updatedOn,
                            UpdatedBy = updatedBy,
                            UpdatedOn = updatedOn
                        });
                    }
                }
                else if (sfcEntity.Type == SfcTypeEnum.NoProduce)
                {
                    // 条码信息
                    var sfcInfoEntity = sfcInfoEntities.FirstOrDefault(f => f.SfcId == sfcEntity.Id);
                    if (sfcInfoEntity == null) continue;

                    // 产品信息
                    var productEntity = productEntities.FirstOrDefault(f => f.Id == sfcInfoEntity.ProductId);
                    if (productEntity == null) continue;

                    // 添加步骤
                    sfcStepEntities.AddRange(badRecordEntities.Select(s => new ManuSfcStepEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        Operatetype = ManuSfcStepTypeEnum.Misjudgment,
                        CurrentStatus = SfcStatusEnum.Complete,
                        SFC = sfcEntity.SFC,
                        ProductId = sfcInfoEntity.ProductId,
                        WorkOrderId = sfcInfoEntity.WorkOrderId ?? 0,
                        SFCInfoId = sfcInfoEntity.Id,
                        Qty = sfcEntity.Qty,
                        Remark = requestDto.Remark,
                        SiteId = sfcEntity.SiteId,
                        CreatedBy = updatedBy,
                        CreatedOn = updatedOn,
                        UpdatedBy = updatedBy,
                        UpdatedOn = updatedOn
                    }));
                }
                else continue;
            }

            // 需要删除的条码ID
            physicalDeleteSFCProduceByIdsCommand.Ids = sfcProduceIds;

            using var trans = TransactionHelper.GetTransactionScope();
            List<Task<int>> tasks = new()
            {
                // 关闭不合格记录
               _manuProductBadRecordRepository.UpdateStatusByIdRangeAsync(badRecordUpdateCommands),

                // 插入 manu_sfc_step
                _manuSfcStepRepository.InsertRangeAsync(sfcStepEntities),

                // 删除在制
                _manuSfcProduceRepository.DeletePhysicalRangeByIdsAsync(physicalDeleteSFCProduceByIdsCommand)
            };

            var rowArray = await Task.WhenAll(tasks);
            trans.Complete();
            return rowArray.Sum();
        }
        #endregion


        #region 返工
        /// <summary>
        /// 根据条码查询信息（返工）
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuReworkBarCodeDto>> GetReworkByBarCodeAsync(string barCode)
        {
            if (string.IsNullOrEmpty(barCode)) throw new CustomerValidationException(nameof(ErrorCode.MES15445));

            // 站点
            var siteId = _currentSite.SiteId ?? 0;
            var dtos = await GetInfoByBarCodesAsync(siteId, new List<string> { barCode });
            return dtos.Select(s => s.ToDto<ManuReworkBarCodeDto>());
        }

        /// <summary>
        /// 根据托盘码条码查询信息（返工）
        /// </summary>
        /// <param name="palletCode"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuReworkBarCodeDto>> GetReworkByPalletCodeAsync(string palletCode)
        {
            if (string.IsNullOrEmpty(palletCode)) throw new CustomerValidationException(nameof(ErrorCode.MES19111));

            // 站点
            var siteId = _currentSite.SiteId ?? 0;

            // 根据载具代码获取载具里面的条码
            var vehicleSFCs = await _manuCommonService.GetSFCsByVehicleCodesAsync(new VehicleSFCRequestBo
            {
                SiteId = siteId,
                VehicleCodes = new List<string> { palletCode }
            });
            if (vehicleSFCs == null || !vehicleSFCs.Any()) return Array.Empty<ManuReworkBarCodeDto>();

            var dtos = await GetInfoByBarCodesAsync(siteId, vehicleSFCs.Select(s => s.SFC));
            return dtos.Select(s => s.ToDto<ManuReworkBarCodeDto>());
        }

        /// <summary>
        /// 提交（返工）
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<int> SubmitReworkAsync(ManuReworkDto requestDto)
        {
            if (requestDto == null) return 0;
            if (requestDto.Reworks == null || !requestDto.Reworks.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES15445));

            // 初始化请求参数
            var dataBo = new ReworkRequestBo
            {
                SiteId = _currentSite.SiteId ?? 0,
                UpdatedBy = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now(),
                Remark = requestDto.Remark ?? ""
            };

            // 传入的条码
            var barCodes = requestDto.Reworks.Select(s => s.BarCode);

            // 查询条码
            dataBo.SFCEntities = await _manuSfcRepository.GetListAsync(new ManuSfcQuery
            {
                SiteId = dataBo.SiteId,
                SFCs = barCodes
            });
            if (dataBo.SFCEntities == null || !dataBo.SFCEntities.Any()) return 0;

            // 状态校验
            var validationFailures = new List<ValidationFailure>();
            var noMatchSFCEntities = dataBo.SFCEntities.Where(w => ManuSfcStatus.ForbidSfcStatuss.Contains(w.Status));
            if (noMatchSFCEntities.Any())
            {
                foreach (var sfcEntity in noMatchSFCEntities)
                {
                    var validationFailure = new ValidationFailure() { FormattedMessagePlaceholderValues = new() };
                    validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", sfcEntity.SFC);
                    validationFailure.FormattedMessagePlaceholderValues.Add("barCode", sfcEntity.SFC);
                    validationFailure.FormattedMessagePlaceholderValues.Add("status", sfcEntity.Status.GetDescription());
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15447);
                    validationFailures.Add(validationFailure);
                }

                if (validationFailures.Any()) throw new ValidationException("", validationFailures);
            }

            // 查询条码信息
            dataBo.SFCInfoEntities = await _manuSfcInfoRepository.GetBySFCIdsAsync(dataBo.SFCEntities.Select(s => s.Id));

            // 查询工单信息
            var workOrderIds = dataBo.SFCInfoEntities.Where(w => w.WorkOrderId.HasValue).Select(s => s.WorkOrderId!.Value);
            workOrderIds = workOrderIds.Union(requestDto.Reworks.Where(w => w.ReworkWorkOrderId.HasValue).Select(s => s.ReworkWorkOrderId!.Value));

            workOrderIds = workOrderIds.Distinct();
            dataBo.WorkOrderEntities = await _planWorkOrderRepository.GetByIdsAsync(workOrderIds);

            // 读取在制品
            dataBo.SFCProduceEntities = await _manuSfcProduceRepository.GetBySFCIdsAsync(dataBo.SFCEntities.Where(w => w.Type == SfcTypeEnum.Produce).Select(s => s.Id));

            // 读取产品
            dataBo.ProductEntities = await _procMaterialRepository.GetByIdsAsync(dataBo.SFCInfoEntities.Select(s => s.ProductId));

            // 查询传入的不合格代码
            dataBo.UnqualifiedCodeEntities = await _qualUnqualifiedCodeRepository.GetByIdsAsync(requestDto.Reworks.Where(w => w.UnqualifiedCodeId.HasValue).Select(s => s.UnqualifiedCodeId!.Value));

            // 查询工序
            var procedureIds = requestDto.Reworks.Where(w => w.ReworkProcedureId.HasValue).Select(s => s.ReworkProcedureId!.Value);
            procedureIds = procedureIds.Union(requestDto.Reworks.Where(w => w.FoundProcedureId.HasValue).Select(s => s.FoundProcedureId!.Value));
            procedureIds = procedureIds.Union(requestDto.Reworks.Where(w => w.OutProcedureId.HasValue).Select(s => s.OutProcedureId!.Value));

            procedureIds = procedureIds.Distinct();
            dataBo.ProcedureEntities = await _procProcedureRepository.GetByIdsAsync(procedureIds);

            // 查询不合格记录（开启的状态）
            var badRecordEntities = await _manuProductBadRecordRepository.GetManuProductBadRecordEntitiesBySFCAsync(new ManuProductBadRecordBySfcQuery
            {
                SiteId = dataBo.SiteId,
                Status = ProductBadRecordStatusEnum.Open,
                SFCs = barCodes
            });

            // 不良记录分组
            dataBo.BadRecordEntitiesDict = badRecordEntities.ToLookup(x => x.SFC).ToDictionary(d => d.Key, d => d);

            // 将传入的数据按"返工类型"分组
            var reworkDict = requestDto.Reworks.ToLookup(x => x.Type).ToDictionary(d => d.Key, d => d);

            var responseBos = new List<ReworkResponseBo>();
            foreach (var item in reworkDict)
            {
                var responseBo = item.Key switch
                {
                    ManuReworkTypeEnum.OriginalOrder => await ReWorkForOriginalOrderAsync(item.Value, dataBo),
                    ManuReworkTypeEnum.NewOrder => await ReWorkForNewOrderAsync(item.Value, dataBo),
                    ManuReworkTypeEnum.NewOrderCell => await ReWorkForNewOrderCellAsync(item.Value, dataBo),
                    _ => null,
                };

                if (responseBo == null) continue;
                responseBos.Add(responseBo);
            }

            var rows = 0;
            if (responseBos == null || !responseBos.Any()) return rows;

            // 归集每个响应的结果
            var responseSummaryBo = new ReworkResponseSummaryBo
            {
                InsertSFCProduceEntities = responseBos.SelectMany(s => s.InsertSFCProduceEntities),
                UpdateSFCInfoEntities = responseBos.SelectMany(s => s.UpdateSFCInfoEntities),
                UpdateSFCProduceEntities = responseBos.SelectMany(s => s.UpdateSFCProduceEntities),
                SFCStepEntities = responseBos.SelectMany(s => s.SFCStepEntities),
                ProductBadRecordEntities = responseBos.SelectMany(s => s.ProductBadRecordEntities),
                ProductNgRecordEntities = responseBos.SelectMany(s => s.ProductNgRecordEntities),
                BadRecordUpdateCommands = responseBos.SelectMany(s => s.BadRecordUpdateCommands)
            };

            using var trans = TransactionHelper.GetTransactionScope();
            List<Task<int>> tasks = new()
            {
                // 关闭不合格记录
               _manuProductBadRecordRepository.UpdateStatusByIdRangeAsync(responseSummaryBo.BadRecordUpdateCommands),

                // 插入 manu_sfc_produce
                _manuSfcProduceRepository.InsertRangeAsync(responseSummaryBo.InsertSFCProduceEntities),
                
                // 更新 manu_sfc_info
                _manuSfcInfoRepository.UpdateRangeAsync(responseSummaryBo.UpdateSFCInfoEntities),

                // 更新 manu_sfc_produce
                _manuSfcProduceRepository.UpdateRangeAsync(responseSummaryBo.UpdateSFCProduceEntities),

                // 插入 manu_sfc_step
                _manuSfcStepRepository.InsertRangeAsync(responseSummaryBo.SFCStepEntities),

                // 插入不良记录
                _manuProductBadRecordRepository.InsertRangeAsync(responseSummaryBo.ProductBadRecordEntities),

                // 插入NG记录
                _manuProductNgRecordRepository.InsertRangeAsync(responseSummaryBo.ProductNgRecordEntities)
            };

            var rowArray = await Task.WhenAll(tasks);
            trans.Complete();
            return rowArray.Sum();
        }

        /// <summary>
        /// 下载导入模板（返工）
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public async Task<string> DownloadImportReworkTemplateAsync(Stream stream)
        {
            var worksheetName = "产品异常处理-返工";
            await _excelService.ExportAsync(Array.Empty<ManuReworkExcelDto>(), stream, worksheetName);
            return worksheetName;
        }

        /// <summary>
        /// 导入（返工）
        /// </summary>
        /// <returns></returns>
        public async Task ImportReworkAsync(IFormFile formFile)
        {
            using MemoryStream memoryStream = new();
            await formFile.CopyToAsync(memoryStream).ConfigureAwait(false);
            var dtos = _excelService.Import<ManuReworkExcelDto>(memoryStream);
            if (dtos == null || !dtos.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES10133));

            /*
            // 备份用户上传的文件，可选
            var stream = formFile.OpenReadStream();
            var uploadResult = await _minioService.PutObjectAsync(formFile.FileName, stream, formFile.ContentType);
            */

            var validationFailures = new List<ValidationFailure>();
            var index = 0;
            foreach (var item in dtos)
            {
                index++;

                // 校验产品序列码
                if (string.IsNullOrEmpty(item.BarCode))
                {
                    var validationFailure = new ValidationFailure() { FormattedMessagePlaceholderValues = new() };
                    validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", index);
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15466);
                    validationFailures.Add(validationFailure);
                }
                // 校验返工类型
                if (!Enum.IsDefined(typeof(ManuReworkTypeEnum), item.Type))
                {
                    var validationFailure = new ValidationFailure() { FormattedMessagePlaceholderValues = new() };
                    validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", index);
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15465);
                    validationFailures.Add(validationFailure);
                }
                // 校验返工工单
                if ((item.Type == ManuReworkTypeEnum.NewOrder || item.Type == ManuReworkTypeEnum.NewOrderCell)
                    && string.IsNullOrEmpty(item.ReworkWorkOrder))
                {
                    var validationFailure = new ValidationFailure() { FormattedMessagePlaceholderValues = new() };
                    validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", index);
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15456);
                    validationFailures.Add(validationFailure);
                }
                // 校验返工工序
                if (item.Type == ManuReworkTypeEnum.OriginalOrder && string.IsNullOrEmpty(item.ReworkProcedure))
                {
                    var validationFailure = new ValidationFailure() { FormattedMessagePlaceholderValues = new() };
                    validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", index);
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15458);
                    validationFailures.Add(validationFailure);
                }
                // 校验发现工序
                if ((item.Type == ManuReworkTypeEnum.OriginalOrder || item.Type == ManuReworkTypeEnum.NewOrder)
                    && string.IsNullOrEmpty(item.FoundProcedure))
                {
                    var validationFailure = new ValidationFailure() { FormattedMessagePlaceholderValues = new() };
                    validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", index);
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15463);
                    validationFailures.Add(validationFailure);
                }
                // 校验不合格代码
                if (string.IsNullOrEmpty(item.UnqualifiedCode))
                {
                    var validationFailure = new ValidationFailure() { FormattedMessagePlaceholderValues = new() };
                    validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", index);
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15464);
                    validationFailures.Add(validationFailure);
                }
                // 校验流出工序
                if ((item.Type == ManuReworkTypeEnum.OriginalOrder || item.Type == ManuReworkTypeEnum.NewOrder)
                    && string.IsNullOrEmpty(item.OutProcedure))
                {
                    var validationFailure = new ValidationFailure() { FormattedMessagePlaceholderValues = new() };
                    validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", index);
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15457);
                    validationFailures.Add(validationFailure);
                }
            }

            if (validationFailures.Any()) throw new ValidationException(_localizationService.GetResource("ExcelRowError"), validationFailures);

            ManuReworkDto requestDto = await GetReworkDtosFromExcelAsync(dtos);
            if (requestDto == null || requestDto.Reworks == null || !requestDto.Reworks.Any()) return;

            await SubmitReworkAsync(requestDto);
        }
        #endregion


        #region 离脱
        /// <summary>
        /// 根据条码查询信息（离脱）
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public async Task<ManuDetachmentBarCodeDto> GetDetachmentByBarCodeAsync(string barCode)
        {
            if (string.IsNullOrEmpty(barCode)) throw new CustomerValidationException(nameof(ErrorCode.MES15445));

            // 查询条码
            var sfcEntity = await _manuSfcRepository.GetSingleAsync(new ManuSfcQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                SFC = barCode
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES15446)).WithData("barCode", barCode);

            // 状态校验
            if (ManuSfcStatus.ForbidSfcStatuss.Contains(sfcEntity.Status)) throw new CustomerValidationException(nameof(ErrorCode.MES15447))
                    .WithData("barCode", barCode)
                    .WithData("status", sfcEntity.Status.GetDescription());

            // 初始化返回值
            var dto = new ManuDetachmentBarCodeDto
            {
                BarCode = sfcEntity.SFC,
                Qty = sfcEntity.Qty,
                Type = sfcEntity.Type
            };

            // 查询条码信息
            var sfcInfoEntity = await _manuSfcInfoRepository.GetBySFCIdAsync(sfcEntity.Id)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES15446)).WithData("barCode", barCode);

            // 填充工单
            if (sfcInfoEntity.WorkOrderId.HasValue)
            {
                var workOrderEntity = await _planWorkOrderRepository.GetByIdAsync(sfcInfoEntity.WorkOrderId.Value);
                if (workOrderEntity != null) dto.WorkOrderCode = workOrderEntity.OrderCode;
            }

            // 填充产品
            var productEntity = await _procMaterialRepository.GetByIdAsync(sfcInfoEntity.ProductId);
            if (productEntity != null)
            {
                dto.ProductCode = productEntity.MaterialCode;
                dto.ProductName = productEntity.MaterialName;
            }

            // 填充工序
            if (sfcEntity.Type == SfcTypeEnum.Produce)
            {
                var sfcProduceEntity = await _manuSfcProduceRepository.GetBySFCIdAsync(sfcEntity.Id);
                if (sfcProduceEntity != null)
                {
                    var procedureEntity = await _procProcedureRepository.GetByIdAsync(sfcProduceEntity.ProcedureId);
                    if (procedureEntity != null)
                    {
                        dto.ProcedureCode = procedureEntity.Code;
                        dto.ProcedureName = procedureEntity.Name;
                    }
                }
            }

            return dto;
        }

        /// <summary>
        /// 提交（离脱）
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<int> SubmitDetachmentAsync(ManuDetachmentDto requestDto)
        {
            if (requestDto == null) return 0;
            if (requestDto.BarCodes == null || !requestDto.BarCodes.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES15445));

            // 更新时间
            var siteId = _currentSite.SiteId ?? 0;
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // 查询条码
            var sfcEntities = await _manuSfcRepository.GetListAsync(new ManuSfcQuery
            {
                SiteId = siteId,
                SFCs = requestDto.BarCodes
            });
            if (sfcEntities == null || !sfcEntities.Any()) return 0;

            // 状态校验
            var validationFailures = new List<ValidationFailure>();
            var noMatchSFCEntities = sfcEntities.Where(w => ManuSfcStatus.ForbidSfcStatuss.Contains(w.Status));
            if (noMatchSFCEntities.Any())
            {
                foreach (var sfcEntity in noMatchSFCEntities)
                {
                    var validationFailure = new ValidationFailure() { FormattedMessagePlaceholderValues = new() };
                    validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", sfcEntity.SFC);
                    validationFailure.FormattedMessagePlaceholderValues.Add("barCode", sfcEntity.SFC);
                    validationFailure.FormattedMessagePlaceholderValues.Add("status", sfcEntity.Status.GetDescription());
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15447);
                    validationFailures.Add(validationFailure);
                }

                if (validationFailures.Any()) throw new ValidationException("", validationFailures);
            }

            // 查询条码信息
            var sfcInfoEntities = await _manuSfcInfoRepository.GetBySFCIdsAsync(sfcEntities.Select(s => s.Id));

            // 读取在制品
            var sfcProduceEntities = await _manuSfcProduceRepository.GetBySFCIdsAsync(sfcEntities.Where(w => w.Type == SfcTypeEnum.Produce).Select(s => s.Id));

            // 读取产品
            var productEntities = await _procMaterialRepository.GetByIdsAsync(sfcInfoEntities.Select(s => s.ProductId));

            // 遍历所有条码
            List<ManuSfcEntity> updateManuSfcEntities = new();
            List<DeletePhysicalBySfcCommand> deletePhysicalBySfcCommands = new();
            List<ManuSfcStepEntity> sfcStepEntities = new();
            List<WhMaterialStandingbookEntity> materialStandingbookEntities = new();

            PhysicalDeleteSFCProduceByIdsCommand physicalDeleteSFCProduceByIdsCommand = new() { SiteId = siteId };
            UpdateWhMaterialInventoryEmptyCommand updateWhMaterialInventoryEmptyCommand = new()
            {
                SiteId = siteId,
                UserName = updatedBy,
                UpdateTime = updatedOn
            };

            List<long> sfcProduceIds = new();
            List<string> sfcInventorys = new();
            foreach (var sfcEntity in sfcEntities)
            {
                if (sfcEntity.Type == SfcTypeEnum.Produce)
                {
                    // 在制品
                    var sfcProduceEntity = sfcProduceEntities.FirstOrDefault(f => f.SFCId == sfcEntity.Id);
                    if (sfcProduceEntity == null) continue;

                    // 删除在制；
                    sfcProduceIds.Add(sfcProduceEntity.Id);

                    // 添加步骤
                    sfcStepEntities.Add(new ManuSfcStepEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        Operatetype = ManuSfcStepTypeEnum.Detachment,
                        CurrentStatus = SfcStatusEnum.Detachment,
                        SFC = sfcProduceEntity.SFC,
                        ProductId = sfcProduceEntity.ProductId,
                        WorkOrderId = sfcProduceEntity.WorkOrderId,
                        WorkCenterId = sfcProduceEntity.WorkCenterId,
                        ProductBOMId = sfcProduceEntity.ProductBOMId,
                        ProcessRouteId = sfcProduceEntity.ProcessRouteId,
                        SFCInfoId = sfcProduceEntity.BarCodeInfoId,
                        Qty = sfcProduceEntity.Qty,
                        VehicleCode = "", // 这里要赋值？
                        ProcedureId = sfcProduceEntity.ProcedureId,
                        ResourceId = sfcProduceEntity.ResourceId,
                        EquipmentId = sfcProduceEntity.EquipmentId,
                        Remark = requestDto.Remark,
                        SiteId = sfcEntity.SiteId,
                        CreatedBy = updatedBy,
                        CreatedOn = updatedOn,
                        UpdatedBy = updatedBy,
                        UpdatedOn = updatedOn
                    });
                }
                else if (sfcEntity.Type == SfcTypeEnum.NoProduce)
                {
                    // 条码信息
                    var sfcInfoEntity = sfcInfoEntities.FirstOrDefault(f => f.SfcId == sfcEntity.Id);
                    if (sfcInfoEntity == null) continue;

                    // 产品信息
                    var productEntity = productEntities.FirstOrDefault(f => f.Id == sfcInfoEntity.ProductId);
                    if (productEntity == null) continue;

                    // 添加步骤
                    sfcStepEntities.Add(new ManuSfcStepEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        Operatetype = ManuSfcStepTypeEnum.Detachment,
                        CurrentStatus = SfcStatusEnum.Detachment,
                        SFC = sfcEntity.SFC,
                        ProductId = sfcInfoEntity.ProductId,
                        WorkOrderId = sfcInfoEntity.WorkOrderId ?? 0,
                        SFCInfoId = sfcInfoEntity.Id,
                        Qty = 0,
                        Remark = requestDto.Remark,
                        SiteId = sfcEntity.SiteId,
                        CreatedBy = updatedBy,
                        CreatedOn = updatedOn,
                        UpdatedBy = updatedBy,
                        UpdatedOn = updatedOn
                    });

                    // 清零库存；
                    sfcInventorys.Add(sfcEntity.SFC);

                    // 记录台账；
                    materialStandingbookEntities.Add(new WhMaterialStandingbookEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        MaterialCode = productEntity.MaterialCode,
                        MaterialName = productEntity.MaterialName,
                        MaterialVersion = productEntity.Version ?? "",
                        MaterialBarCode = sfcEntity.SFC,
                        //Batch = "",//自制品 没有
                        Quantity = 0,
                        Unit = productEntity.Unit ?? "",
                        Type = WhMaterialInventoryTypeEnum.Detachment,
                        Source = MaterialInventorySourceEnum.Detachment,
                        SiteId = sfcEntity.SiteId,
                        CreatedBy = updatedBy,
                        CreatedOn = updatedOn,
                        UpdatedBy = updatedBy,
                        UpdatedOn = updatedOn
                    });
                }
                else continue;

                // 条码表改状态；
                sfcEntity.Status = SfcStatusEnum.Detachment;
                sfcEntity.UpdatedBy = updatedBy;
                sfcEntity.UpdatedOn = updatedOn;

                updateManuSfcEntities.Add(sfcEntity);
            }

            // 需要删除的条码ID
            physicalDeleteSFCProduceByIdsCommand.Ids = sfcProduceIds;
            updateWhMaterialInventoryEmptyCommand.BarCodeList = sfcInventorys;


            using var trans = TransactionHelper.GetTransactionScope();
            List<Task<int>> tasks = new()
            {
                // 删除在制
                _manuSfcProduceRepository.DeletePhysicalRangeByIdsAsync(physicalDeleteSFCProduceByIdsCommand),

                // 插入 manu_sfc_step
                _manuSfcStepRepository.InsertRangeAsync(sfcStepEntities),

                // 清零库存
               _whMaterialInventoryRepository.UpdateWhMaterialInventoryEmptyByBarCodeAync(updateWhMaterialInventoryEmptyCommand),

                // 台账
                _whMaterialStandingbookRepository.InsertsAsync(materialStandingbookEntities),

                // manu_sfc 更新状态
                _manuSfcRepository.UpdateRangeWithStatusCheckAsync(updateManuSfcEntities)
            };

            var rowArray = await Task.WhenAll(tasks);
            trans.Complete();
            return rowArray.Sum();
        }
        #endregion



        #region 内部方法
        /// <summary>
        /// 根据条码集合查询信息（有不良记录的条码）
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="barCodes"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuProductNGBarCodeDto>> GetNgInfoByBarCodesAsync(long siteId, IEnumerable<string> barCodes)
        {
            if (barCodes == null || !barCodes.Any()) return Array.Empty<ManuProductNGBarCodeDto>();

            // 初始化返回值
            List<ManuProductNGBarCodeDto> dtos = new();

            // 查询条码
            var sfcEntities = await _manuSfcRepository.GetListAsync(new ManuSfcQuery
            {
                SiteId = siteId,
                SFCs = barCodes
            });

            if (sfcEntities == null || !sfcEntities.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES15459)); //return dtos;

            // 状态校验
            var validationFailures = new List<ValidationFailure>();
            var noMatchSFCEntities = sfcEntities.Where(w => ManuSfcStatus.ForbidSfcStatuss.Contains(w.Status));
            if (noMatchSFCEntities.Any())
            {
                foreach (var sfcEntity in noMatchSFCEntities)
                {
                    var validationFailure = new ValidationFailure() { FormattedMessagePlaceholderValues = new() };
                    validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", sfcEntity.SFC);
                    validationFailure.FormattedMessagePlaceholderValues.Add("barCode", sfcEntity.SFC);
                    validationFailure.FormattedMessagePlaceholderValues.Add("status", sfcEntity.Status.GetDescription());
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15447);
                    validationFailures.Add(validationFailure);
                }

                if (validationFailures.Any()) throw new ValidationException("", validationFailures);
            }

            /*
            // 非生产条码
            if (sfcEntity.Type == SfcTypeEnum.NoProduce) throw new CustomerValidationException(nameof(ErrorCode.MES15456))
                    .WithData("barCode", barCode)
                    .WithData("type", sfcEntity.Type.GetDescription());
            */

            // 查询不合格记录（开启的状态）
            var badRecordEntities = await _manuProductBadRecordRepository.GetManuProductBadRecordEntitiesBySFCAsync(new ManuProductBadRecordBySfcQuery
            {
                SiteId = siteId,
                Status = ProductBadRecordStatusEnum.Open,
                SFCs = barCodes
            });
            if (badRecordEntities == null || !badRecordEntities.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES15451)).WithData("barCode", string.Join(',', barCodes));

            // 查询条码信息
            var sfcInfoEntities = await _manuSfcInfoRepository.GetBySFCIdsAsync(sfcEntities.Select(s => s.Id));

            // 读取在制品
            var sfcProduceEntities = await _manuSfcProduceRepository.GetBySFCIdsAsync(sfcEntities.Where(w => w.Type == SfcTypeEnum.Produce).Select(s => s.Id));

            // 读取产品
            var productEntities = await _procMaterialRepository.GetByIdsAsync(sfcInfoEntities.Select(s => s.ProductId));

            // 查询工序
            List<long> procedureIds = new();
            procedureIds.AddRange(sfcProduceEntities.Select(s => s.ProcedureId));
            procedureIds.AddRange(badRecordEntities.Select(s => s.FoundBadOperationId));
            var procedureEntities = await _procProcedureRepository.GetByIdsAsync(procedureIds);

            // 遍历不良记录
            foreach (var item in badRecordEntities)
            {
                var dto = new ManuProductNGBarCodeDto
                {
                    Id = item.Id,
                    BarCode = item.SFC,
                    Qty = item.Qty ?? 0
                };

                // 查询条码
                var sfcEntity = sfcEntities.FirstOrDefault(f => f.SFC == item.SFC)
                    ?? throw new CustomerValidationException(nameof(ErrorCode.MES15446)).WithData("barCode", item.SFC);

                // 查询条码信息
                var sfcInfoEntity = sfcInfoEntities.FirstOrDefault(f => f.SfcId == sfcEntity.Id)
                    ?? throw new CustomerValidationException(nameof(ErrorCode.MES15446)).WithData("barCode", item.SFC);

                // 填充工单
                if (sfcInfoEntity.WorkOrderId.HasValue)
                {
                    var workOrderEntity = await _planWorkOrderRepository.GetByIdAsync(sfcInfoEntity.WorkOrderId.Value);
                    if (workOrderEntity != null) dto.WorkOrderCode = workOrderEntity.OrderCode;
                }

                // 填充产品
                var productEntity = await _procMaterialRepository.GetByIdAsync(sfcInfoEntity.ProductId);
                if (productEntity != null)
                {
                    dto.ProductCode = productEntity.MaterialCode;
                    dto.ProductName = productEntity.MaterialName;
                }

                // 填充发现工序
                var foundProcedureEntity = procedureEntities.FirstOrDefault(f => f.Id == item.FoundBadOperationId);
                if (foundProcedureEntity != null) dto.FoundProcedure = foundProcedureEntity.Name;

                // 查询在制
                var sfcProduceEntity = sfcProduceEntities.FirstOrDefault(f => f.SFCId == sfcEntity.Id);

                // 填充工序
                if (sfcProduceEntity != null)
                {
                    var procedureEntity = procedureEntities.FirstOrDefault(f => f.Id == sfcProduceEntity.ProcedureId);
                    if (procedureEntity != null)
                    {
                        dto.ProcedureCode = procedureEntity.Code;
                        dto.ProcedureName = procedureEntity.Name;
                    }
                }

                dtos.Add(dto);
            }

            return dtos;
        }

        /// <summary>
        /// 根据条码集合查询信息
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="barCodes"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuProductBarCodeDto>> GetInfoByBarCodesAsync(long siteId, IEnumerable<string> barCodes)
        {
            if (barCodes == null || !barCodes.Any()) return Array.Empty<ManuProductBarCodeDto>();

            // 初始化返回值
            List<ManuProductBarCodeDto> dtos = new();

            // 查询条码
            var sfcEntities = await _manuSfcRepository.GetListAsync(new ManuSfcQuery
            {
                SiteId = siteId,
                SFCs = barCodes
            });

            if (sfcEntities == null || !sfcEntities.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES15459)); //return dtos;

            // 状态校验
            var validationFailures = new List<ValidationFailure>();
            var noMatchSFCEntities = sfcEntities.Where(w => ManuSfcStatus.ForbidSfcStatuss.Contains(w.Status));
            if (noMatchSFCEntities.Any())
            {
                foreach (var sfcEntity in noMatchSFCEntities)
                {
                    var validationFailure = new ValidationFailure() { FormattedMessagePlaceholderValues = new() };
                    validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", sfcEntity.SFC);
                    validationFailure.FormattedMessagePlaceholderValues.Add("barCode", sfcEntity.SFC);
                    validationFailure.FormattedMessagePlaceholderValues.Add("status", sfcEntity.Status.GetDescription());
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15447);
                    validationFailures.Add(validationFailure);
                }

                if (validationFailures.Any()) throw new ValidationException("", validationFailures);
            }

            // 查询条码信息
            var sfcInfoEntities = await _manuSfcInfoRepository.GetBySFCIdsAsync(sfcEntities.Select(s => s.Id));

            // 读取在制品
            var sfcProduceEntities = await _manuSfcProduceRepository.GetBySFCIdsAsync(sfcEntities.Where(w => w.Type == SfcTypeEnum.Produce).Select(s => s.Id));

            // 读取产品
            var productEntities = await _procMaterialRepository.GetByIdsAsync(sfcInfoEntities.Select(s => s.ProductId));

            // 遍历所有条码
            foreach (var sfcEntity in sfcEntities)
            {
                var dto = new ManuProductBarCodeDto
                {
                    Id = sfcEntity.Id,
                    BarCode = sfcEntity.SFC,
                    Qty = sfcEntity.Qty
                };

                // 查询条码信息
                var sfcInfoEntity = sfcInfoEntities.FirstOrDefault(f => f.SfcId == sfcEntity.Id)
                    ?? throw new CustomerValidationException(nameof(ErrorCode.MES15446)).WithData("barCode", sfcEntity.SFC);

                // 填充工单
                if (sfcInfoEntity.WorkOrderId.HasValue)
                {
                    var workOrderEntity = await _planWorkOrderRepository.GetByIdAsync(sfcInfoEntity.WorkOrderId.Value);
                    if (workOrderEntity != null) dto.WorkOrderCode = workOrderEntity.OrderCode;
                }

                // 填充产品
                var productEntity = await _procMaterialRepository.GetByIdAsync(sfcInfoEntity.ProductId);
                if (productEntity != null)
                {
                    dto.ProductCode = productEntity.MaterialCode;
                    dto.ProductName = productEntity.MaterialName;
                }

                dtos.Add(dto);
            }

            return dtos;
        }

        /// <summary>
        /// 根据导入内容填充ID信息（让步接收）
        /// </summary>
        /// <param name="excelDtos"></param>
        /// <returns></returns>
        public async Task<ManuCompromiseDto> GetCompromiseDtosFromExcelAsync(IEnumerable<ManuCompromiseExcelDto>? excelDtos)
        {
            // 初始化返回值
            List<ManuCompromiseItemDto> itemDtos = new();
            var responseDto = new ManuCompromiseDto { Remark = "返工模板导入", Compromises = itemDtos };
            if (excelDtos == null || !excelDtos.Any()) return responseDto;

            // 站点
            var siteId = _currentSite.SiteId ?? 0;

            // 工序信息
            var procedureCodes = excelDtos.Where(w => w.FoundProcedure != null).Select(s => s.FoundProcedure);
            procedureCodes = procedureCodes.Union(excelDtos.Where(w => w.OutProcedure != null).Select(s => s.OutProcedure));

            var procedureEntities = await _procProcedureRepository.GetEntitiesAsync(new ProcProcedureQuery
            {
                SiteId = siteId,
                Codes = procedureCodes.Distinct()
            });

            // 不合格代码
            var unqualifiedCodeEntities = await _qualUnqualifiedCodeRepository.GetByCodesAsync(new QualUnqualifiedCodeByCodesQuery
            {
                SiteId = siteId,
                Codes = excelDtos.Where(w => w.UnqualifiedCode != null).Select(s => s.UnqualifiedCode).Distinct()
            });

            foreach (var item in excelDtos)
            {
                var dto = new ManuCompromiseItemDto
                {
                    BarCode = item.BarCode,
                };

                // 填充不合格代码
                var unqualifiedCodeEntity = unqualifiedCodeEntities.FirstOrDefault(f => f.UnqualifiedCode == item.UnqualifiedCode);
                if (unqualifiedCodeEntity != null) dto.UnqualifiedCodeId = unqualifiedCodeEntity.Id;

                // 填充发现工序
                var foundProcedureEntity = procedureEntities.FirstOrDefault(f => f.Code == item.FoundProcedure);
                if (foundProcedureEntity != null) dto.FoundProcedureId = foundProcedureEntity.Id;

                // 填充流出工序
                var outProcedureEntity = procedureEntities.FirstOrDefault(f => f.Code == item.OutProcedure);
                if (outProcedureEntity != null) dto.OutProcedureId = outProcedureEntity.Id;

                itemDtos.Add(dto);
            }

            responseDto.Compromises = itemDtos;
            return responseDto;
        }

        /// <summary>
        /// 根据导入内容填充ID信息（返工）
        /// </summary>
        /// <param name="excelDtos"></param>
        /// <returns></returns>
        public async Task<ManuReworkDto> GetReworkDtosFromExcelAsync(IEnumerable<ManuReworkExcelDto>? excelDtos)
        {
            // 初始化返回值
            List<ManuReworkIemDto> itemDtos = new();
            var responseDto = new ManuReworkDto { Remark = "返工模板导入", Reworks = itemDtos };
            if (excelDtos == null || !excelDtos.Any()) return responseDto;

            // 站点
            var siteId = _currentSite.SiteId ?? 0;

            // 工单信息
            var workOrderEntities = await _planWorkOrderRepository.GetEntitiesAsync(new PlanWorkOrderNewQuery
            {
                SiteId = siteId,
                Codes = excelDtos.Where(w => w.ReworkWorkOrder != null).Select(s => s.ReworkWorkOrder).Distinct()
            });

            // 工序信息
            var procedureCodes = excelDtos.Where(w => w.FoundProcedure != null).Select(s => s.FoundProcedure);
            procedureCodes = procedureCodes.Union(excelDtos.Where(w => w.OutProcedure != null).Select(s => s.OutProcedure));
            procedureCodes = procedureCodes.Union(excelDtos.Where(w => w.ReworkProcedure != null).Select(s => s.ReworkProcedure));

            var procedureEntities = await _procProcedureRepository.GetEntitiesAsync(new ProcProcedureQuery
            {
                SiteId = siteId,
                Codes = procedureCodes.Distinct()
            });

            // 不合格代码
            var unqualifiedCodeEntities = await _qualUnqualifiedCodeRepository.GetByCodesAsync(new QualUnqualifiedCodeByCodesQuery
            {
                SiteId = siteId,
                Codes = excelDtos.Where(w => w.UnqualifiedCode != null).Select(s => s.UnqualifiedCode).Distinct()
            });

            foreach (var item in excelDtos)
            {
                var dto = new ManuReworkIemDto
                {
                    BarCode = item.BarCode,
                    Type = item.Type
                };

                // 填充不合格代码
                var unqualifiedCodeEntity = unqualifiedCodeEntities.FirstOrDefault(f => f.UnqualifiedCode == item.UnqualifiedCode);
                if (unqualifiedCodeEntity != null) dto.UnqualifiedCodeId = unqualifiedCodeEntity.Id;

                // 填充返工工单
                var workOrderEntity = workOrderEntities.FirstOrDefault(f => f.OrderCode == item.ReworkWorkOrder);
                if (workOrderEntity != null) dto.ReworkWorkOrderId = workOrderEntity.Id;

                // 填充返工工序
                var reworkProcedureEntity = procedureEntities.FirstOrDefault(f => f.Code == item.ReworkProcedure);
                if (reworkProcedureEntity != null) dto.ReworkProcedureId = reworkProcedureEntity.Id;

                // 填充发现工序
                var foundProcedureEntity = procedureEntities.FirstOrDefault(f => f.Code == item.FoundProcedure);
                if (foundProcedureEntity != null) dto.FoundProcedureId = foundProcedureEntity.Id;

                // 填充流出工序
                var outProcedureEntity = procedureEntities.FirstOrDefault(f => f.Code == item.OutProcedure);
                if (outProcedureEntity != null) dto.OutProcedureId = outProcedureEntity.Id;

                itemDtos.Add(dto);
            }

            responseDto.Reworks = itemDtos;
            return responseDto;
        }

        /// <summary>
        /// 原工单返工
        /// </summary>
        /// <param name="reworkIemDtos"></param>
        /// <param name="dataBo"></param>
        /// <returns></returns>
        public async Task<ReworkResponseBo> ReWorkForOriginalOrderAsync(IEnumerable<ManuReworkIemDto> reworkIemDtos, ReworkRequestBo dataBo)
        {
            var responseBo = new ReworkResponseBo();

            // 遍历所有条码
            var validationFailures = new List<ValidationFailure>();
            foreach (var dto in reworkIemDtos)
            {
                var validationFailure = new ValidationFailure() { FormattedMessagePlaceholderValues = new() };
                validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", dto.BarCode);

                if (!dto.UnqualifiedCodeId.HasValue)
                {
                    validationFailure.ErrorCode = nameof(ErrorCode.MES19702);
                    validationFailures.Add(validationFailure);
                    continue;
                }
                if (!dto.FoundProcedureId.HasValue)
                {
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15433);
                    validationFailures.Add(validationFailure);
                    continue;
                }
                if (!dto.OutProcedureId.HasValue)
                {
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15457);
                    validationFailures.Add(validationFailure);
                    continue;
                }
                if (!dto.ReworkProcedureId.HasValue)
                {
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15458);
                    validationFailures.Add(validationFailure);
                    continue;
                }

                // 条码
                var sfcEntity = dataBo.SFCEntities.FirstOrDefault(f => f.SFC == dto.BarCode);
                if (sfcEntity == null) continue;

                // 条码信息
                var sfcInfoEntity = dataBo.SFCInfoEntities.FirstOrDefault(f => f.SfcId == sfcEntity.Id);
                if (sfcInfoEntity == null) continue;

                // 不合格代码
                var unqualifiedCodeEntity = dataBo.UnqualifiedCodeEntities.FirstOrDefault(f => f.Id == dto.UnqualifiedCodeId);
                if (unqualifiedCodeEntity == null) continue;

                // 关闭不合格记录（如果有的话）
                if (dataBo.BadRecordEntitiesDict.TryGetValue(sfcEntity.SFC, out var badRecordEntities))
                {
                    responseBo.BadRecordUpdateCommands.AddRange(badRecordEntities.Select(s => new ManuProductBadRecordUpdateCommand
                    {
                        Id = s.Id,
                        Status = ProductBadRecordStatusEnum.Close,
                        DisposalResult = ProductBadDisposalResultEnum.Rework,
                        UpdatedOn = dataBo.UpdatedOn,
                        UserId = dataBo.UpdatedBy,
                        Remark = dataBo.Remark
                    }));
                }

                // 初始化步骤数据
                var sfcStepEntity = new ManuSfcStepEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    Operatetype = ManuSfcStepTypeEnum.Rework,
                    SFC = sfcEntity.SFC,
                    ProductId = sfcInfoEntity.ProductId,
                    SFCInfoId = sfcInfoEntity.Id,
                    Qty = sfcEntity.Qty,
                    VehicleCode = "", // 这里要赋值？
                    Remark = dataBo.Remark,
                    SiteId = dataBo.SiteId,
                    CreatedBy = dataBo.UpdatedBy,
                    CreatedOn = dataBo.UpdatedOn,
                    UpdatedBy = dataBo.UpdatedBy,
                    UpdatedOn = dataBo.UpdatedOn
                };

                ManuSfcProduceEntity? sfcProduceEntity = null;
                if (sfcEntity.Type == SfcTypeEnum.Produce)
                {
                    // 在制品
                    sfcProduceEntity = dataBo.SFCProduceEntities.FirstOrDefault(f => f.SFCId == sfcEntity.Id);
                    if (sfcProduceEntity == null) continue;

                    // 修改在制信息
                    sfcProduceEntity.ProcedureId = dto.ReworkProcedureId ?? 0;
                    sfcProduceEntity.ResourceId = null;
                    sfcProduceEntity.EquipmentId = null;
                    sfcProduceEntity.Status = SfcStatusEnum.lineUp;
                    sfcProduceEntity.UpdatedBy = dataBo.UpdatedBy;
                    sfcProduceEntity.UpdatedOn = dataBo.UpdatedOn;
                    responseBo.UpdateSFCProduceEntities.Add(sfcProduceEntity);
                }
                else if (sfcEntity.Type == SfcTypeEnum.NoProduce)
                {
                    // 产品信息
                    var productEntity = dataBo.ProductEntities.FirstOrDefault(f => f.Id == sfcInfoEntity.ProductId);
                    if (productEntity == null) continue;

                    // 工单信息
                    var workOrderEntity = dataBo.WorkOrderEntities.FirstOrDefault(f => f.Id == sfcInfoEntity.WorkOrderId);
                    if (workOrderEntity == null) continue;

                    // 新增在制品
                    sfcProduceEntity = new ManuSfcProduceEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SFC = sfcEntity.SFC,
                        SFCId = sfcEntity.Id,
                        ProductId = sfcInfoEntity.ProductId,
                        WorkOrderId = sfcInfoEntity.WorkOrderId ?? 0,
                        BarCodeInfoId = sfcInfoEntity.Id,
                        ProcessRouteId = workOrderEntity.ProcessRouteId,
                        WorkCenterId = workOrderEntity.WorkCenterId ?? 0,
                        ProductBOMId = workOrderEntity.ProductBOMId,
                        Qty = productEntity.Batch ?? 0,
                        //Qty = productEntity.Batch ?? 0,
                        ProcedureId = dto.ReworkProcedureId ?? 0,
                        Status = SfcStatusEnum.lineUp,
                        RepeatedCount = 0,
                        IsScrap = TrueOrFalseEnum.No,
                        SiteId = dataBo.SiteId,
                        CreatedBy = dataBo.UpdatedBy,
                        CreatedOn = dataBo.UpdatedOn,
                        UpdatedBy = dataBo.UpdatedBy,
                        UpdatedOn = dataBo.UpdatedOn
                    };
                    responseBo.InsertSFCProduceEntities.Add(sfcProduceEntity);
                }
                else continue;

                // 添加步骤
                sfcStepEntity.WorkOrderId = sfcProduceEntity.WorkOrderId;
                sfcStepEntity.WorkCenterId = sfcProduceEntity.WorkCenterId;
                sfcStepEntity.ProductBOMId = sfcProduceEntity.ProductBOMId;
                sfcStepEntity.ProcedureId = sfcProduceEntity.ProcedureId;
                sfcStepEntity.ResourceId = sfcProduceEntity.ResourceId;
                sfcStepEntity.EquipmentId = sfcProduceEntity.EquipmentId;
                sfcStepEntity.CurrentStatus = sfcProduceEntity.Status;
                responseBo.SFCStepEntities.Add(sfcStepEntity);

                // 添加不良记录
                var badRecordId = IdGenProvider.Instance.CreateId();
                responseBo.ProductBadRecordEntities.Add(new ManuProductBadRecordEntity
                {
                    Id = badRecordId,
                    FoundBadOperationId = dto.FoundProcedureId.Value,
                    FoundBadResourceId = null,
                    OutflowOperationId = dto.OutProcedureId.Value,
                    UnqualifiedId = unqualifiedCodeEntity.Id,
                    SfcStepId = sfcStepEntity.Id,
                    SFC = sfcEntity.SFC,
                    SfcInfoId = sfcInfoEntity.Id,
                    Qty = sfcEntity.Qty,
                    Status = ProductBadRecordStatusEnum.Close,
                    Source = ProductBadRecordSourceEnum.BadManualEntry,
                    DisposalResult = ProductBadDisposalResultEnum.Rework,
                    Remark = dataBo.Remark,
                    SiteId = dataBo.SiteId,
                    CreatedBy = dataBo.UpdatedBy,
                    CreatedOn = dataBo.UpdatedOn,
                    UpdatedBy = dataBo.UpdatedBy,
                    UpdatedOn = dataBo.UpdatedOn
                });

                // 添加NG记录
                responseBo.ProductNgRecordEntities.Add(new ManuProductNgRecordEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    BadRecordId = badRecordId,
                    UnqualifiedId = unqualifiedCodeEntity.Id,
                    NGCode = unqualifiedCodeEntity.UnqualifiedCode,
                    Remark = dataBo.Remark,
                    SiteId = dataBo.SiteId,
                    CreatedBy = dataBo.UpdatedBy,
                    CreatedOn = dataBo.UpdatedOn,
                    UpdatedBy = dataBo.UpdatedBy,
                    UpdatedOn = dataBo.UpdatedOn
                });
            }

            if (validationFailures.Any()) throw new ValidationException("", validationFailures);

            return await Task.FromResult(responseBo);
        }

        /// <summary>
        /// 新工单返工
        /// </summary>
        /// <param name="reworkIemDtos"></param>
        /// <param name="dataBo"></param>
        /// <returns></returns>
        public async Task<ReworkResponseBo> ReWorkForNewOrderAsync(IEnumerable<ManuReworkIemDto> reworkIemDtos, ReworkRequestBo dataBo)
        {
            var responseBo = new ReworkResponseBo();

            // 查询条码（库存中）
            var inventorySFCEntities = dataBo.SFCEntities.Where(w => reworkIemDtos.Select(s => s.BarCode).Contains(w.SFC) && w.Type == SfcTypeEnum.NoProduce);
            if (inventorySFCEntities != null && inventorySFCEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15453)).WithData("barCode", string.Join(",", inventorySFCEntities.Select(s => s.SFC)));
            }

            // 遍历所有条码
            var validationFailures = new List<ValidationFailure>();
            foreach (var dto in reworkIemDtos)
            {
                var validationFailure = new ValidationFailure() { FormattedMessagePlaceholderValues = new() };
                validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", dto.BarCode);

                if (!dto.UnqualifiedCodeId.HasValue)
                {
                    validationFailure.ErrorCode = nameof(ErrorCode.MES19702);
                    validationFailures.Add(validationFailure);
                    continue;
                }
                if (!dto.FoundProcedureId.HasValue)
                {
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15433);
                    validationFailures.Add(validationFailure);
                    continue;
                }
                if (!dto.OutProcedureId.HasValue)
                {
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15457);
                    validationFailures.Add(validationFailure);
                    continue;
                }
                if (!dto.ReworkWorkOrderId.HasValue)
                {
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15456);
                    validationFailures.Add(validationFailure);
                    continue;
                }

                // 条码
                var sfcEntity = dataBo.SFCEntities.FirstOrDefault(f => f.SFC == dto.BarCode);
                if (sfcEntity == null) continue;

                // 条码信息
                var sfcInfoEntity = dataBo.SFCInfoEntities.FirstOrDefault(f => f.SfcId == sfcEntity.Id);
                if (sfcInfoEntity == null) continue;

                // 不合格代码
                var unqualifiedCodeEntity = dataBo.UnqualifiedCodeEntities.FirstOrDefault(f => f.Id == dto.UnqualifiedCodeId);
                if (unqualifiedCodeEntity == null) continue;

                // 查询传入的工单
                var workOrderEntity = await _planWorkOrderRepository.GetByIdAsync(dto.ReworkWorkOrderId.Value)
                    ?? throw new CustomerValidationException(nameof(ErrorCode.MES16301));

                // 检验工单
                await CheckWorkOrderAsync(dataBo, workOrderEntity);

                // 读取工单的工艺路线的首工序
                var routeProcedureDto = await _masterDataService.GetFirstProcedureAsync(workOrderEntity.ProcessRouteId);

                // 关闭不合格记录（如果有的话）
                if (dataBo.BadRecordEntitiesDict.TryGetValue(sfcEntity.SFC, out var badRecordEntities))
                {
                    responseBo.BadRecordUpdateCommands.AddRange(badRecordEntities.Select(s => new ManuProductBadRecordUpdateCommand
                    {
                        Id = s.Id,
                        Status = ProductBadRecordStatusEnum.Close,
                        DisposalResult = ProductBadDisposalResultEnum.Rework,
                        UpdatedOn = dataBo.UpdatedOn,
                        UserId = dataBo.UpdatedBy,
                        Remark = dataBo.Remark ?? ""
                    }));
                }

                // 在制品
                var sfcProduceEntity = dataBo.SFCProduceEntities.FirstOrDefault(f => f.SFCId == sfcEntity.Id);
                if (sfcProduceEntity == null) continue;

                // 修改条码信息
                sfcInfoEntity.WorkOrderId = workOrderEntity.Id;
                sfcInfoEntity.ProductId = workOrderEntity.ProductId;
                sfcInfoEntity.ProcessRouteId = workOrderEntity.ProcessRouteId;
                sfcInfoEntity.ProductBOMId = workOrderEntity.ProductBOMId;
                sfcInfoEntity.UpdatedBy = dataBo.UpdatedBy;
                sfcInfoEntity.UpdatedOn = dataBo.UpdatedOn;
                responseBo.UpdateSFCInfoEntities.Add(sfcInfoEntity);

                // 修改在制信息
                sfcProduceEntity.WorkOrderId = workOrderEntity.Id;
                sfcProduceEntity.WorkCenterId = workOrderEntity.WorkCenterId ?? 0;
                sfcProduceEntity.ProcessRouteId = workOrderEntity.ProcessRouteId;
                sfcProduceEntity.ProductBOMId = workOrderEntity.ProductBOMId;
                sfcProduceEntity.ProcedureId = routeProcedureDto.ProcedureId;
                sfcProduceEntity.ResourceId = null;
                sfcProduceEntity.EquipmentId = null;
                sfcProduceEntity.Status = SfcStatusEnum.lineUp;
                sfcProduceEntity.UpdatedBy = dataBo.UpdatedBy;
                sfcProduceEntity.UpdatedOn = dataBo.UpdatedOn;
                responseBo.UpdateSFCProduceEntities.Add(sfcProduceEntity);

                // 添加步骤
                var sfcStepId = IdGenProvider.Instance.CreateId();
                responseBo.SFCStepEntities.Add(new ManuSfcStepEntity
                {
                    Id = sfcStepId,
                    Operatetype = ManuSfcStepTypeEnum.Rework,
                    SFC = sfcEntity.SFC,
                    ProductId = sfcInfoEntity.ProductId,
                    SFCInfoId = sfcInfoEntity.Id,
                    WorkOrderId = sfcProduceEntity.WorkOrderId,
                    WorkCenterId = sfcProduceEntity.WorkCenterId,
                    ProductBOMId = sfcProduceEntity.ProductBOMId,
                    ProcessRouteId = sfcProduceEntity.ProcessRouteId,
                    ProcedureId = sfcProduceEntity.ProcedureId,
                    ResourceId = sfcProduceEntity.ResourceId,
                    EquipmentId = sfcProduceEntity.EquipmentId,
                    CurrentStatus = sfcProduceEntity.Status,
                    Qty = sfcEntity.Qty,
                    VehicleCode = "", // 这里要赋值？
                    Remark = dataBo.Remark,
                    SiteId = dataBo.SiteId,
                    CreatedBy = dataBo.UpdatedBy,
                    CreatedOn = dataBo.UpdatedOn,
                    UpdatedBy = dataBo.UpdatedBy,
                    UpdatedOn = dataBo.UpdatedOn
                });

                // 添加不良记录
                var badRecordId = IdGenProvider.Instance.CreateId();
                responseBo.ProductBadRecordEntities.Add(new ManuProductBadRecordEntity
                {
                    Id = badRecordId,
                    FoundBadOperationId = dto.FoundProcedureId.Value,
                    FoundBadResourceId = null,
                    OutflowOperationId = dto.OutProcedureId.Value,
                    UnqualifiedId = unqualifiedCodeEntity.Id,
                    SfcStepId = sfcStepId,
                    SFC = sfcEntity.SFC,
                    SfcInfoId = sfcInfoEntity.Id,
                    Qty = sfcEntity.Qty,
                    Status = ProductBadRecordStatusEnum.Close,
                    Source = ProductBadRecordSourceEnum.BadManualEntry,
                    DisposalResult = ProductBadDisposalResultEnum.Rework,
                    Remark = dataBo.Remark,
                    SiteId = dataBo.SiteId,
                    CreatedBy = dataBo.UpdatedBy,
                    CreatedOn = dataBo.UpdatedOn,
                    UpdatedBy = dataBo.UpdatedBy,
                    UpdatedOn = dataBo.UpdatedOn
                });

                // 添加NG记录
                responseBo.ProductNgRecordEntities.Add(new ManuProductNgRecordEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    BadRecordId = badRecordId,
                    UnqualifiedId = unqualifiedCodeEntity.Id,
                    NGCode = unqualifiedCodeEntity.UnqualifiedCode,
                    Remark = dataBo.Remark,
                    SiteId = dataBo.SiteId,
                    CreatedBy = dataBo.UpdatedBy,
                    CreatedOn = dataBo.UpdatedOn,
                    UpdatedBy = dataBo.UpdatedBy,
                    UpdatedOn = dataBo.UpdatedOn
                });
            }

            if (validationFailures.Any()) throw new ValidationException("", validationFailures);

            return responseBo;
        }

        /// <summary>
        /// 新工单返工（成品电芯）
        /// </summary>
        /// <param name="reworkIemDtos"></param>
        /// <param name="dataBo"></param>
        /// <returns></returns>
        public async Task<ReworkResponseBo> ReWorkForNewOrderCellAsync(IEnumerable<ManuReworkIemDto> reworkIemDtos, ReworkRequestBo dataBo)
        {
            var responseBo = new ReworkResponseBo();

            // 查询在制条码
            var produceSFCEntities = dataBo.SFCEntities.Where(w => reworkIemDtos.Select(s => s.BarCode).Contains(w.SFC) && w.Type == SfcTypeEnum.Produce);
            if (produceSFCEntities != null && produceSFCEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15454)).WithData("barCode", string.Join(",", produceSFCEntities.Select(s => s.SFC)));
            }

            // 遍历所有条码
            var validationFailures = new List<ValidationFailure>();
            foreach (var dto in reworkIemDtos)
            {
                var validationFailure = new ValidationFailure() { FormattedMessagePlaceholderValues = new() };
                validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", dto.BarCode);

                if (!dto.UnqualifiedCodeId.HasValue)
                {
                    validationFailure.ErrorCode = nameof(ErrorCode.MES19702);
                    validationFailures.Add(validationFailure);
                    continue;
                }
                if (!dto.ReworkWorkOrderId.HasValue)
                {
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15456);
                    validationFailures.Add(validationFailure);
                    continue;
                }

                // 条码
                var sfcEntity = dataBo.SFCEntities.FirstOrDefault(f => f.SFC == dto.BarCode);
                if (sfcEntity == null) continue;

                // 条码信息
                var sfcInfoEntity = dataBo.SFCInfoEntities.FirstOrDefault(f => f.SfcId == sfcEntity.Id);
                if (sfcInfoEntity == null) continue;

                // 不合格代码
                var unqualifiedCodeEntity = dataBo.UnqualifiedCodeEntities.FirstOrDefault(f => f.Id == dto.UnqualifiedCodeId);
                if (unqualifiedCodeEntity == null) continue;

                // 查询传入的工单
                var workOrderEntity = await _planWorkOrderRepository.GetByIdAsync(dto.ReworkWorkOrderId.Value)
                    ?? throw new CustomerValidationException(nameof(ErrorCode.MES16301));

                // 检验工单
                await CheckWorkOrderAsync(dataBo, workOrderEntity);

                // 读取工单的工艺路线的首工序
                var routeProcedureDto = await _masterDataService.GetFirstProcedureAsync(workOrderEntity.ProcessRouteId);

                // 关闭不合格记录（如果有的话）
                if (dataBo.BadRecordEntitiesDict.TryGetValue(sfcEntity.SFC, out var badRecordEntities))
                {
                    responseBo.BadRecordUpdateCommands.AddRange(badRecordEntities.Select(s => new ManuProductBadRecordUpdateCommand
                    {
                        Id = s.Id,
                        Status = ProductBadRecordStatusEnum.Close,
                        DisposalResult = ProductBadDisposalResultEnum.Rework,
                        UpdatedOn = dataBo.UpdatedOn,
                        UserId = dataBo.UpdatedBy,
                        Remark = dataBo.Remark ?? ""
                    }));
                }

                // 产品信息
                var productEntity = dataBo.ProductEntities.FirstOrDefault(f => f.Id == sfcInfoEntity.ProductId);
                if (productEntity == null) continue;

                // 修改条码信息
                sfcInfoEntity.WorkOrderId = workOrderEntity.Id;
                sfcInfoEntity.ProductId = workOrderEntity.ProductId;
                sfcInfoEntity.ProcessRouteId = workOrderEntity.ProcessRouteId;
                sfcInfoEntity.ProductBOMId = workOrderEntity.ProductBOMId;
                sfcInfoEntity.UpdatedBy = dataBo.UpdatedBy;
                sfcInfoEntity.UpdatedOn = dataBo.UpdatedOn;
                responseBo.UpdateSFCInfoEntities.Add(sfcInfoEntity);

                // 新增在制品
                var sfcProduceEntity = new ManuSfcProduceEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SFC = sfcEntity.SFC,
                    SFCId = sfcEntity.Id,
                    ProductId = sfcInfoEntity.ProductId,
                    BarCodeInfoId = sfcInfoEntity.Id,
                    WorkOrderId = workOrderEntity.Id,
                    WorkCenterId = workOrderEntity.WorkCenterId ?? 0,
                    ProcessRouteId = workOrderEntity.ProcessRouteId,
                    ProductBOMId = workOrderEntity.ProductBOMId,
                    Qty = productEntity.Batch ?? 0,
                    ProcedureId = routeProcedureDto.ProcedureId,
                    Status = SfcStatusEnum.lineUp,
                    RepeatedCount = 0,
                    IsScrap = TrueOrFalseEnum.No,
                    SiteId = dataBo.SiteId,
                    CreatedBy = dataBo.UpdatedBy,
                    CreatedOn = dataBo.UpdatedOn,
                    UpdatedBy = dataBo.UpdatedBy,
                    UpdatedOn = dataBo.UpdatedOn
                };
                responseBo.InsertSFCProduceEntities.Add(sfcProduceEntity);

                // 添加步骤
                var sfcStepId = IdGenProvider.Instance.CreateId();
                responseBo.SFCStepEntities.Add(new ManuSfcStepEntity
                {
                    Id = sfcStepId,
                    Operatetype = ManuSfcStepTypeEnum.Rework,
                    SFC = sfcEntity.SFC,
                    ProductId = sfcInfoEntity.ProductId,
                    SFCInfoId = sfcInfoEntity.Id,
                    WorkOrderId = sfcProduceEntity.WorkOrderId,
                    WorkCenterId = sfcProduceEntity.WorkCenterId,
                    ProductBOMId = sfcProduceEntity.ProductBOMId,
                    ProcessRouteId = sfcProduceEntity.ProcessRouteId,
                    ProcedureId = sfcProduceEntity.ProcedureId,
                    ResourceId = sfcProduceEntity.ResourceId,
                    EquipmentId = sfcProduceEntity.EquipmentId,
                    CurrentStatus = sfcProduceEntity.Status,
                    Qty = sfcEntity.Qty,
                    VehicleCode = "", // 这里要赋值？
                    Remark = dataBo.Remark,
                    SiteId = dataBo.SiteId,
                    CreatedBy = dataBo.UpdatedBy,
                    CreatedOn = dataBo.UpdatedOn,
                    UpdatedBy = dataBo.UpdatedBy,
                    UpdatedOn = dataBo.UpdatedOn
                });

                // 添加不良记录
                var badRecordId = IdGenProvider.Instance.CreateId();
                responseBo.ProductBadRecordEntities.Add(new ManuProductBadRecordEntity
                {
                    Id = badRecordId,
                    FoundBadOperationId = 0,
                    FoundBadResourceId = null,
                    OutflowOperationId = 0,
                    UnqualifiedId = unqualifiedCodeEntity.Id,
                    SfcStepId = sfcStepId,
                    SFC = sfcEntity.SFC,
                    SfcInfoId = sfcInfoEntity.Id,
                    Qty = sfcEntity.Qty,
                    Status = ProductBadRecordStatusEnum.Close,
                    Source = ProductBadRecordSourceEnum.BadManualEntry,
                    DisposalResult = ProductBadDisposalResultEnum.Rework,
                    Remark = dataBo.Remark,
                    SiteId = dataBo.SiteId,
                    CreatedBy = dataBo.UpdatedBy,
                    CreatedOn = dataBo.UpdatedOn,
                    UpdatedBy = dataBo.UpdatedBy,
                    UpdatedOn = dataBo.UpdatedOn
                });

                // 添加NG记录
                responseBo.ProductNgRecordEntities.Add(new ManuProductNgRecordEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    BadRecordId = badRecordId,
                    UnqualifiedId = unqualifiedCodeEntity.Id,
                    NGCode = unqualifiedCodeEntity.UnqualifiedCode,
                    Remark = dataBo.Remark,
                    SiteId = dataBo.SiteId,
                    CreatedBy = dataBo.UpdatedBy,
                    CreatedOn = dataBo.UpdatedOn,
                    UpdatedBy = dataBo.UpdatedBy,
                    UpdatedOn = dataBo.UpdatedOn
                });
            }

            if (validationFailures.Any()) throw new ValidationException("", validationFailures);

            return responseBo;
        }

        /// <summary>
        /// 检验工单
        /// </summary>
        /// <param name="dataBo"></param>
        /// <param name="workOrderEntity"></param>
        /// <returns></returns>
        private async Task CheckWorkOrderAsync(ReworkRequestBo dataBo, PlanWorkOrderEntity workOrderEntity)
        {
            // 校验条码对应的产品与工单产品是否一致
            var noSameProductSFCInfoEntities = dataBo.SFCInfoEntities.Where(w => w.ProductId != workOrderEntity.ProductId);
            if (noSameProductSFCInfoEntities == null || !noSameProductSFCInfoEntities.Any()) return;

            // 读取工单BOM
            var productBOMEntities = await _masterDataService.GetBomDetailEntitiesByBomIdAsync(workOrderEntity.ProductBOMId);
            if (productBOMEntities != null && productBOMEntities.Any())
            {
                // 如果选择的是新工单返工/新工单返工（成品电芯），校验条码对应的产品与工单产品是否一致，产品是否包含在新工单BOM中，若二者条件都不满足
                var ids = noSameProductSFCInfoEntities.Select(w => w.ProductId).Where(w => !productBOMEntities.Select(s => s.MaterialId).Contains(w));
                if (ids != null && ids.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES15455));

                /*
                var ids = noSameProductSFCInfoEntities.Select(w => w.ProductId).Intersect(productBOMEntities.Select(s => s.MaterialId));
                if (ids == null || !ids.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES15455));
                */
            }
        }

        #endregion

    }
}
