using Elastic.Clients.Elasticsearch;
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.EventBus.Abstractions;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Bos.Manufacture.ManuCreateBarcode;
using Hymson.MES.CoreServices.Dtos.Process.LabelTemplate.Utility;
using Hymson.MES.CoreServices.Events.ProcessEvents.PrintEvents;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.CoreServices.Services.Manufacture.ManuCreateBarcode;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.HttpClients;
using Hymson.MES.HttpClients.Requests.Print;
using Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto.ManuCreateBarcodeDto;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.Snowflake;
using Hymson.Utils.Tools;
using System.Security.Policy;

namespace Hymson.MES.Services.Services.Plan
{
    /// <summary>
    /// 条码打印 服务
    /// </summary>
    public class PlanSfcPrintService : IPlanSfcPrintService
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
        /// 验证器
        /// </summary>
        private readonly AbstractValidator<PlanSfcPrintCreateDto> _validationCreateRules;
        private readonly AbstractValidator<PlanSfcPrintCreatePrintDto> _validationCreatePrintRules;

        /// <summary>
        /// 服务接口（生产通用）
        /// </summary>
        private readonly IManuCommonService _manuCommonService;

        /// <summary>
        /// 仓储（条码）
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        /// <summary>
        /// 仓储（条码信息）
        /// </summary>
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;

        /// <summary>
        /// 仓储（条码在制品）
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 仓储接口（条码步骤）
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

        /// <summary>
        /// 仓储（工单）
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        private readonly IManuCreateBarcodeService _manuCreateBarcodeService;

        /// <summary>
        /// 打印机配置
        /// </summary>
        private readonly IProcPrintConfigRepository _procPrintConfigRepository;
        private readonly IProcResourceRepository _procResourceRepository;
        private readonly IProcProcedureRepository _procProcedureRepository;
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IProcLabelTemplateRepository _procLabelTemplateRepository;
        private readonly ILabelPrintRequest _labelPrintRequest;
        private readonly ILocalizationService _localizationService;

        /// <summary>
        /// 事件总线
        /// </summary>
        private readonly IEventBus<EventBusInstance2> _eventBus;

        /// <summary>
        /// 工序-物料-打印模板
        /// </summary>
        private readonly IProcProcedurePrintRelationRepository _procProcedurePrintRelationRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationCreateRules"></param>
        /// <param name="validationCreatePrintRules"></param>
        /// <param name="manuCommonService"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="manuSfcInfoRepository"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="procPrintConfigRepository"></param>
        /// <param name="procResourceRepository"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="procProcedurePrintRelationRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="procLabelTemplateRepository"></param>
        /// <param name="labelPrintRequest"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="manuCreateBarcodeService"></param>
        /// <param name="localizationService"></param>
        public PlanSfcPrintService(ICurrentUser currentUser, ICurrentSite currentSite,
            AbstractValidator<PlanSfcPrintCreateDto> validationCreateRules,
            AbstractValidator<PlanSfcPrintCreatePrintDto> validationCreatePrintRules,
            IManuCommonService manuCommonService,
            IManuSfcRepository manuSfcRepository,
            IManuSfcInfoRepository manuSfcInfoRepository,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IManuSfcStepRepository manuSfcStepRepository,
            IProcPrintConfigRepository procPrintConfigRepository,
            IProcResourceRepository procResourceRepository,
            IProcProcedureRepository procProcedureRepository,
            IProcProcedurePrintRelationRepository procProcedurePrintRelationRepository,
            IProcMaterialRepository procMaterialRepository,
            IProcLabelTemplateRepository procLabelTemplateRepository,
            ILabelPrintRequest labelPrintRequest,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IManuCreateBarcodeService manuCreateBarcodeService,
            ILocalizationService localizationService,
            IEventBus<EventBusInstance2> eventBus)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationCreateRules = validationCreateRules;
            _validationCreatePrintRules = validationCreatePrintRules;
            _manuCommonService = manuCommonService;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _procPrintConfigRepository = procPrintConfigRepository;
            _procResourceRepository = procResourceRepository;
            _procProcedureRepository = procProcedureRepository;
            _procProcedurePrintRelationRepository = procProcedurePrintRelationRepository;
            _procMaterialRepository = procMaterialRepository;
            _procLabelTemplateRepository = procLabelTemplateRepository;
            _labelPrintRequest = labelPrintRequest;
            _manuCreateBarcodeService = manuCreateBarcodeService;
            _localizationService = localizationService;
            _eventBus = eventBus;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        public async Task CreateAsync(PlanSfcPrintCreateDto createDto)
        {
            // 验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(createDto);
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        public async Task CreatePrintAsync(PlanSfcPrintCreatePrintDto createDto)
        {
            // 验证DTO
            await _validationCreatePrintRules.ValidateAndThrowAsync(createDto);

            var resourceEntity = await _procResourceRepository.GetResByIdAsync(createDto.ResourceId);
            var procedureEntity = await _procProcedureRepository.GetByIdAsync(createDto.ProcedureId);
            // 对工序资源类型和资源的资源类型校验
            if (resourceEntity != null && procedureEntity != null && procedureEntity.ResourceTypeId.HasValue && resourceEntity.ResTypeId != procedureEntity.ResourceTypeId.Value)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16507));
            }

            var print = await _procPrintConfigRepository.GetByIdAsync(createDto.PrintId??0);
            if (print == null)
                throw new CustomerValidationException(nameof(ErrorCode.MES17002));
            PlanWorkOrderEntity work;
            if (createDto.WorkOrderId == 0)
            {
                work = await _planWorkOrderRepository.GetByCodeAsync(new PlanWorkOrderQuery()
                {
                    OrderCode = createDto.OrderCode,
                    SiteId = _currentSite.SiteId ?? 0
                });
            }
            else
            {
                work = await _planWorkOrderRepository.GetByIdAsync(createDto.WorkOrderId);
            }
            _eventBus.PublishDelay(new PrintIntegrationEvent
            {
                SiteId = _currentSite.SiteId ?? 0,
                PrintId = createDto.PrintId,
                ResourceId = createDto.ResourceId,
                BarCodes = new List<LabelTemplateBarCodeDto>
                {
                    new LabelTemplateBarCodeDto
                    {
                    BarCode=createDto.SFC??"",
                    MateriaId=work.ProductId
                    }
                },
                UserName = _currentUser.UserName
            },10);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(IEnumerable<long> idsArr)
        {
            var sfcEntities = await _manuSfcRepository.GetListAsync(new ManuSfcQuery
            {
                Ids = idsArr
            }); ;
            if (sfcEntities.Any(it => it.IsUsed == YesOrNoEnum.Yes)) throw new CustomerValidationException(nameof(ErrorCode.MES16116));
            if (sfcEntities.Any(it => it.Status == SfcStatusEnum.Scrapping)) throw new CustomerValidationException(nameof(ErrorCode.MES16130));

            // 对锁定状态进行验证
            await _manuCommonService.VerifySfcsLockAsync(new ManuProcedureBo
            {
                SiteId = _currentSite.SiteId ?? 0,
                SFCs = sfcEntities.Select(s => s.SFC)
            });

            // 条码集合
            var sfcInfoEntities = await _manuSfcInfoRepository.GetBySFCIdsWithIsUseAsync(sfcEntities.Select(s => s.Id));

            var manuSfcUpdateStatusByIdCommands = new List<ManuSfcUpdateStatusByIdCommand>();

            foreach (var item in sfcEntities)
            {
                manuSfcUpdateStatusByIdCommands.Add(new ManuSfcUpdateStatusByIdCommand
                {
                    Id = item.Id,
                    Status = SfcStatusEnum.Delete,
                    CurrentStatus = item.Status,
                    UpdatedBy = _currentUser.UserName
                });
            }

            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows += await _manuSfcRepository.ManuSfcUpdateStatuByIdRangeAsync(manuSfcUpdateStatusByIdCommands);
                if (rows != manuSfcUpdateStatusByIdCommands.Count)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16138));
                }

                rows += await _manuSfcProduceRepository.DeletePhysicalRangeAsync(new DeletePhysicalBySfcsCommand()
                {
                    SiteId = _currentSite.SiteId ?? 0,
                    Sfcs = sfcEntities.Select(s => s.SFC).ToArray()
                });

                rows += await _manuSfcStepRepository.InsertRangeAsync(sfcEntities.Select(s => new ManuSfcStepEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    SFC = s.SFC,
                    Qty = s.Qty,
                    ProductId = sfcInfoEntities.FirstOrDefault(f => f.SfcId == s.Id)!.ProductId,
                    WorkOrderId = sfcInfoEntities.FirstOrDefault(f => f.SfcId == s.Id)!.WorkOrderId ?? 0,
                    Operatetype = ManuSfcStepTypeEnum.Delete,
                    CurrentStatus = SfcStatusEnum.Complete,
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName
                }));

                trans.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 分页查询列表（条码打印）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<PlanSfcPrintDto>> GetPagedListAsync(PlanSfcPrintPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<ManuSfcProduceNewPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId;

            // 将工单号转换为工单Id
            if (!string.IsNullOrWhiteSpace(pagedQueryDto.OrderCode))
            {
                var workOrderEntities = await _planWorkOrderRepository.GetEntitiesAsync(new PlanWorkOrderNewQuery
                {
                    OrderCode = pagedQueryDto.OrderCode,
                    SiteId = pagedQuery.SiteId
                });
                if (workOrderEntities != null && workOrderEntities.Any()) pagedQuery.WorkOrderIds = workOrderEntities.Select(s => s.Id);
                else pagedQuery.WorkOrderIds = Array.Empty<long>();
            }

            // 查询数据
            var pagedInfo = await _manuSfcProduceRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = await PrepareDtosAsync(pagedInfo.Data);
            return new PagedInfo<PlanSfcPrintDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 在条码下达时获取最新条码状态
        /// </summary>
        /// <param name="planSfcPrintQueryDto"></param>
        /// <returns></returns>
        public async Task<IEnumerable<CreateBarcodeByWorkOrderOutputBo>> GetNewBarCodeOnBarCodeCreatedAsync(PlanSfcPrintQueryDto planSfcPrintQueryDto)
        {
            if (planSfcPrintQueryDto.Datas?.Any() != true)
            {
                return new List<CreateBarcodeByWorkOrderOutputBo>();
            }

            var pendingDatas = planSfcPrintQueryDto.Datas.ToList();

            var result = new List<CreateBarcodeByWorkOrderOutputBo>(pendingDatas.Count);

            var manuSFCIds = pendingDatas.Select(m => m.ManuSFCId);
            var manuSFCEntities = await _manuSfcRepository.GetListAsync(new ManuSfcQuery
            {
                Ids = manuSFCIds
            });
            foreach (var pendingData in pendingDatas)
            {
                var manuSFCEntity = manuSFCEntities.FirstOrDefault(m => m.Id == pendingData.ManuSFCId);
                if (manuSFCEntity == null)
                {
                    continue;
                }

                pendingData.BarcodeStatus = manuSFCEntity.Status;

                result.Add(pendingData);
            }

            return result;
        }

        /// <summary>
        /// 生成条码
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<List<CreateBarcodeByWorkOrderOutputBo>> CreateBarcodeByWorkOrderIdAsync(CreateBarcodeByWorkOrderDto parm)
        {
            return await _manuCreateBarcodeService.CreateBarcodeByWorkOrderIdAsync(new CreateBarcodeByWorkOrderBo
            {
                SiteId = _currentSite.SiteId ?? 0,
                UserName = _currentUser.UserName,
                WorkOrderId = parm.WorkOrderId,
                ProcedureId = parm.ProcedureId,
                ResourceId = parm.ResourceId,
                Qty = parm.Qty
            }, _localizationService);
        }

        /// <summary>
        /// 工单下达及打印
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task CreateBarcodeByWorkOrderIdAndPrintAsync(CreateBarcodeByWorkOrderAndPrintDto parm)
        {
            var list = await _manuCreateBarcodeService.CreateBarcodeByWorkOrderIdAsync(new CreateBarcodeByWorkOrderBo
            {
                SiteId = _currentSite.SiteId ?? 0,
                UserName = _currentUser.UserName,
                WorkOrderId = parm.WorkOrderId,
                ProcedureId = parm.ProcedureId,
                ResourceId = parm.ResourceId,
                Qty = parm.Qty
            }, _localizationService);
            var barCodes = new List<LabelTemplateBarCodeDto>();
            var work = await _planWorkOrderRepository.GetByIdAsync(parm.WorkOrderId);
            foreach (var item in list)
            {
                barCodes.Add(new LabelTemplateBarCodeDto
                {
                    BarCode = item.SFC,
                    MateriaId = work.ProductId
                });
            }

            _eventBus.PublishDelay(new PrintIntegrationEvent
            {
                SiteId = _currentSite.SiteId ?? 0,
                PrintId = parm.PrintId,
                ResourceId = parm.ResourceId,
                BarCodes = barCodes,
                UserName = _currentUser.UserName
            }, 1);
        }

        /// <summary>
        /// 转换为Dto对象
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        private async Task<IEnumerable<PlanSfcPrintDto>> PrepareDtosAsync(IEnumerable<ManuSfcProduceEntity> entities)
        {
            List<PlanSfcPrintDto> dtos = new();

            // 读取物料
            var materialEntities = await _procMaterialRepository.GetByIdsAsync(entities.Select(x => x.ProductId));
            var materialDic = materialEntities.ToDictionary(x => x.Id, x => x);

            // 读取工单
            var workOrderEntities = await _planWorkOrderRepository.GetByIdsAsync(entities.Select(x => x.WorkOrderId));
            var workOrderDic = workOrderEntities.ToDictionary(x => x.Id, x => x);

            // 读取条码
            var sfcEntities = await _manuSfcRepository.GetListAsync(new ManuSfcQuery
            {
                Ids = entities.Select(x => x.SFCId)
            });
            var sfcDic = sfcEntities.ToDictionary(x => x.Id, x => x);

            // 遍历填充
            foreach (var entity in entities)
            {
                var dto = entity.ToModel<PlanSfcPrintDto>();
                if (dto == null) continue;

                dto.UpdatedOn = entity.CreatedOn;    // 这里用创建时间作为条码生成时间更准确

                // 条码信息
                if (!sfcDic.ContainsKey(entity.SFCId)) continue;
                var sfcEntity = sfcDic[entity.SFCId];
                if (sfcEntity != null)
                {
                    dto.Id = sfcEntity.Id;
                    dto.SFC = sfcEntity.SFC;
                    dto.IsUsed = sfcEntity.IsUsed;
                }

                // 工单
                if (!workOrderDic.ContainsKey(entity.WorkOrderId)) continue;
                var workOrderEntity = workOrderDic[entity.WorkOrderId];
                if (workOrderEntity != null)
                {
                    dto.OrderCode = workOrderEntity.OrderCode;
                }

                // 产品
                if (!materialDic.ContainsKey(entity.ProductId)) continue;
                var materialEntity = materialDic[entity.ProductId];
                if (materialEntity != null)
                {
                    dto.MaterialCode = materialEntity.MaterialCode;
                    dto.MaterialName = materialEntity.MaterialName;
                    dto.BuyType = materialEntity.BuyType;
                }

                dtos.Add(dto);
            }

            return dtos;
        }
    }
}
