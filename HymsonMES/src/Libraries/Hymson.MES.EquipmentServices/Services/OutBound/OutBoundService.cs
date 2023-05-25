using FluentValidation;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums.Process;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Quality.IQualityRepository;
using Hymson.MES.Data.Repositories.Quality.QualUnqualifiedCode.Query;
using Hymson.MES.EquipmentServices.Dtos.OutBound;
using Hymson.Sequences;
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
        /// <summary>
        /// 序列号服务
        /// </summary>
        private readonly ISequenceService _sequenceService;
        private readonly AbstractValidator<OutBoundDto> _validationOutBoundDtoRules;
        private readonly AbstractValidator<OutBoundMoreDto> _validationOutBoundMoreDtoRules;
        private readonly IManuSfcStepRepository _manuSfcStepRepository;
        private readonly IProcResourceRepository _procResourceRepository;
        private readonly IProcParameterRepository _procParameterRepository;
        private readonly IManuProductParameterRepository _manuProductParameterRepository;
        private readonly IQualUnqualifiedCodeRepository _qualUnqualifiedCodeRepository;
        private readonly IManuSfcStepNgRepository _manuSfcStepNgRepository;
        private readonly IManuSfcRepository _manuSfcRepository;
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;
        private readonly IProcProcedureRepository _procProcedureRepository;
        private readonly IProcProcessRouteDetailLinkRepository _procProcessRouteDetailLinkRepository;
        private readonly IProcProcessRouteDetailNodeRepository _procProcessRouteDetailNodeRepository;
        private readonly IManuSfcCirculationRepository _manuSfcCirculationRepository;
        private readonly IManuSfcStepMaterialRepository _manuSfcStepMaterialRepository;

        public OutBoundService(AbstractValidator<OutBoundDto> validationOutBoundDtoRules,
            ICurrentEquipment currentEquipment,
            AbstractValidator<OutBoundMoreDto> validationOutBoundMoreDtoRules,
            IManuSfcStepRepository manuSfcStepRepository,
            IProcResourceRepository procResourceRepository,
            IProcParameterRepository procParameterRepository,
            IManuProductParameterRepository manuProductParameterRepository,
            IQualUnqualifiedCodeRepository qualUnqualifiedCodeRepository,
            IManuSfcStepNgRepository manuSfcStepNgRepository,
            IManuSfcRepository manuSfcRepository,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IProcProcedureRepository procProcedureRepository,
            ISequenceService sequenceService,
            IProcProcessRouteDetailLinkRepository procProcessRouteDetailLinkRepository,
            IProcProcessRouteDetailNodeRepository procProcessRouteDetailNodeRepository,
            IManuSfcCirculationRepository manuSfcCirculationRepository,
            IInteWorkCenterRepository inteWorkCenterRepository,
            IManuSfcStepMaterialRepository manuSfcStepMaterialRepository)
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
            _manuSfcRepository = manuSfcRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _procProcedureRepository = procProcedureRepository;
            _sequenceService = sequenceService;
            _procProcessRouteDetailLinkRepository = procProcessRouteDetailLinkRepository;
            _procProcessRouteDetailNodeRepository = procProcessRouteDetailNodeRepository;
            _manuSfcCirculationRepository = manuSfcCirculationRepository;
            _inteWorkCenterRepository = inteWorkCenterRepository;
            _manuSfcStepMaterialRepository = manuSfcStepMaterialRepository;
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
            //使用批量出站Dto
            OutBoundMoreDto outBoundMoreDto = new()
            {
                EquipmentCode = _currentEquipment.Code,
                ResourceCode = outBoundDto.ResourceCode,
                LocalTime = outBoundDto.LocalTime,
                SFCs = new OutBoundSFCDto[] {
                    new OutBoundSFCDto{
                        SFC=outBoundDto.SFC,
                        BindFeedingCodes= outBoundDto.BindFeedingCodes,
                        NG=outBoundDto.NG,
                        ParamList=outBoundDto.ParamList,
                        Passed = outBoundDto.Passed
                    }
                }
            };
            await SFCOutBound(outBoundMoreDto);
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

            await SFCOutBound(outBoundMoreDto);
        }

        /// <summary>
        /// 出站
        /// </summary>
        /// <param name="outBoundMoreDto"></param>
        /// <returns></returns>

        public async Task SFCOutBound(OutBoundMoreDto outBoundMoreDto)
        {
            //已经验证过资源是否存在直接使用
            var procResource = await _procResourceRepository.GetByCodeAsync(new EntityByCodeQuery { Site = _currentEquipment.SiteId, Code = outBoundMoreDto.ResourceCode });
            //查找当前工作中心（产线）
            var workLine = await _inteWorkCenterRepository.GetByResourceIdAsync(procResource.Id);
            if (workLine == null)
            {
                //通过资源未找到关联产线
                throw new CustomerValidationException(nameof(ErrorCode.MES19123)).WithData("ResourceCode", procResource.ResCode);
            }
            //查找激活工单
            var planWorkOrders = await _planWorkOrderRepository.GetByWorkLineIdAsync(workLine.Id);
            if (!planWorkOrders.Any())
            {
                //产线未激活工单
                throw new CustomerValidationException(nameof(ErrorCode.MES19124)).WithData("WorkCenterCode", workLine.Code);
            }
            //不考虑混线
            var planWorkOrder = planWorkOrders.First();
            var planWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(planWorkOrder.Id);

            var sfcs = outBoundMoreDto.SFCs.Select(c => c.SFC).ToArray();
            //条码信息
            var sfclist = await _manuSfcRepository.GetBySFCsAsync(sfcs);
            //查询已经存在条码的生产信息
            var sfcProduceList = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(new ManuSfcProduceQuery { Sfcs = sfcs, SiteId = _currentEquipment.SiteId });

            //条码流转信息
            List<ManuSfcCirculationEntity> manuSfcCirculationEntities = new List<ManuSfcCirculationEntity>();
            List<string> delManuSfcProduces = new List<string>();
            List<ManuSfcStepEntity> manuSfcStepEntities = new List<ManuSfcStepEntity>();
            List<ManuSfcEntity> manuSfcEntities = new List<ManuSfcEntity>();
            List<ManuSfcProduceEntity> manuSfcProduceEntities = new List<ManuSfcProduceEntity>();

            foreach (var outBoundSFCDto in outBoundMoreDto.SFCs)
            {
                var sfcEntity = sfclist.Where(c => c.SFC == outBoundSFCDto.SFC).First();
                var sfcProduceEntity = sfcProduceList.Where(c => c.SFC == outBoundSFCDto.SFC).First();
                // 更新时间
                sfcProduceEntity.UpdatedBy = _currentEquipment.Name;
                sfcProduceEntity.UpdatedOn = HymsonClock.Now();
                //步骤记录
                var manuSfcStepEntity = new ManuSfcStepEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentEquipment.SiteId,
                    ProductId = planWorkOrderEntity.ProductId,
                    WorkOrderId = planWorkOrderEntity.Id,
                    ProductBOMId = planWorkOrderEntity.ProductBOMId,
                    WorkCenterId = planWorkOrderEntity.WorkCenterId ?? 0,
                    SFC = outBoundSFCDto.SFC,
                    Qty = 1,//出站数量
                    Passed = outBoundSFCDto.Passed,//是否合格
                    EquipmentId = _currentEquipment.Id,
                    ResourceId = procResource.Id,
                    CurrentStatus = SfcProduceStatusEnum.Complete,
                    Operatetype = ManuSfcStepTypeEnum.OutStock,
                    ProcedureId = sfcProduceEntity.ProcedureId,
                    CreatedBy = _currentEquipment.Code,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentEquipment.Code,
                    UpdatedOn = HymsonClock.Now(),
                    IsPassingStation = outBoundSFCDto.IsPassingStation//是否是过站
                };

                // 获取下一个工序（如果没有了，就表示完工） TODO 这里GetNextProcedureAsync里需要缓存
                var nextProcedure = await GetNextProcedureAsync(sfcProduceEntity.WorkOrderId, sfcProduceEntity.ProcessRouteId, sfcProduceEntity.ProcedureId);
                //完工
                if (nextProcedure == null)
                {
                    //完工删除
                    delManuSfcProduces.Add(manuSfcStepEntity.SFC);
                    //记录流转信息
                    manuSfcCirculationEntities.Add(new ManuSfcCirculationEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = sfcProduceEntity.SiteId,
                        ProcedureId = sfcProduceEntity.ProcedureId,
                        ResourceId = sfcProduceEntity.ResourceId,
                        SFC = sfcProduceEntity.SFC,
                        WorkOrderId = sfcProduceEntity.WorkOrderId,
                        ProductId = sfcProduceEntity.ProductId,
                        CirculationBarCode = "",//不关联上料
                        CirculationProductId = manuSfcStepEntity.ProductId,
                        CirculationMainProductId = manuSfcStepEntity.ProductId,
                        CirculationQty = 1,
                        CirculationType = SfcCirculationTypeEnum.Consume,
                        CreatedBy = _currentEquipment.Name,
                        CreatedOn = HymsonClock.Now(),
                        UpdatedBy = _currentEquipment.Name,
                        UpdatedOn = HymsonClock.Now()
                    });
                    //更新状态
                    manuSfcStepEntity.Operatetype = ManuSfcStepTypeEnum.OutStock; // TODO 这里的状态？？
                    manuSfcStepEntity.CurrentStatus = SfcProduceStatusEnum.Complete; // TODO 这里的状态？？
                    manuSfcStepEntities.Add(manuSfcStepEntity);
                    //更新条码信息
                    sfcEntity.Status = SfcStatusEnum.Complete;
                    sfcEntity.UpdatedBy = _currentEquipment.Name;
                    sfcEntity.UpdatedOn = HymsonClock.Now();
                    manuSfcEntities.Add(sfcEntity);
                    //Sfc生成信息
                    manuSfcProduceEntities.Add(sfcProduceEntity);
                }
                else
                { //未完工
                    // 修改 manu_sfc_produce 为排队, 工序修改为下一工序的id
                    sfcProduceEntity.Status = SfcProduceStatusEnum.lineUp;
                    sfcProduceEntity.ProcedureId = nextProcedure.Id;
                    manuSfcProduceEntities.Add(sfcProduceEntity);
                    //插入 manu_sfc_step 状态为 进站
                    manuSfcStepEntity.CurrentStatus = SfcProduceStatusEnum.Activity;
                    manuSfcStepEntity.Operatetype = ManuSfcStepTypeEnum.OutStock;
                    manuSfcStepEntities.Add(manuSfcStepEntity);
                }
            }
            //标准参数
            List<ManuProductParameterEntity> manuProductParameterEntities = await PrepareProductParameterEntity(outBoundMoreDto, manuSfcStepEntities, procResource.Id);
            //NG
            List<ManuSfcStepNgEntity> manuProductNGEntities = await PrepareProductNgEntity(outBoundMoreDto, manuSfcStepEntities);
            //批次码信息
            List<ManuSfcStepMaterialEntity> manuProductMaterialEntities = PrepareProductMaterialEntity(outBoundMoreDto, manuSfcStepEntities);

            //数据提交
            using var trans = TransactionHelper.GetTransactionScope();
            int rows = 0;
            // 添加流转记录
            if (manuSfcCirculationEntities.Any())
            {
                rows += await _manuSfcCirculationRepository.InsertRangeAsync(manuSfcCirculationEntities);
            }
            //删除完工条码信息
            if (delManuSfcProduces.Any())
            {
                rows += await _manuSfcProduceRepository.DeletePhysicalRangeAsync(new DeletePhysicalBySfcsCommand()
                {
                    SiteId = _currentEquipment.SiteId,
                    Sfcs = delManuSfcProduces
                });
            }
            if (manuSfcStepEntities.Any())
            {
                //添加步骤记录
                rows += await _manuSfcStepRepository.InsertRangeAsync(manuSfcStepEntities);
            }
            // 更新状态
            if (manuSfcEntities.Any())
            {
                rows += await _manuSfcRepository.UpdateRangeAsync(manuSfcEntities);
            }
            //更新sfc生成信息
            if (manuSfcProduceEntities.Any())
            {
                rows += await _manuSfcProduceRepository.UpdateRangeAsync(manuSfcProduceEntities);
            }
            //产品参数信息
            if (manuProductParameterEntities.Any())
            {
                rows += await _manuProductParameterRepository.InsertsAsync(manuProductParameterEntities);
            }
            //Ng信息
            if (manuProductNGEntities.Any())
            {
                rows += await _manuSfcStepNgRepository.InsertsAsync(manuProductNGEntities);
            }
            //批次码信息
            if (manuProductMaterialEntities.Any())
            {
                rows += await _manuSfcStepMaterialRepository.InsertsAsync(manuProductMaterialEntities);
            }

            // 更新工单统计表的 RealEnd
            rows += await _planWorkOrderRepository.UpdatePlanWorkOrderRealEndByWorkOrderIdAsync(new UpdateWorkOrderRealTimeCommand
            {
                UpdatedOn = HymsonClock.Now(),
                UpdatedBy = _currentEquipment.Name,
                WorkOrderIds = new long[] { planWorkOrder.Id }
            });
            // 更新完工数量
            rows += await _planWorkOrderRepository.UpdateFinishProductQuantityByWorkOrderId(new UpdateQtyCommand
            {
                UpdatedBy = _currentEquipment.Name,
                UpdatedOn = HymsonClock.Now(),
                WorkOrderId = planWorkOrder.Id,
                Qty = outBoundMoreDto.SFCs.Length,
            });
            trans.Complete();
        }

        /// <summary>
        /// 获当前工序对应的下一工序
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <param name="processRouteId"></param>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        public async Task<ProcProcedureEntity?> GetNextProcedureAsync(long workOrderId, long processRouteId, long procedureId)
        {
            // 因为可能有分叉，所以返回的下一步工序是集合
            var netxtProcessRouteDetailLinks = await _procProcessRouteDetailLinkRepository.GetNextProcessRouteDetailLinkAsync(new ProcProcessRouteDetailLinkQuery
            {
                ProcessRouteId = processRouteId,
                ProcedureId = procedureId
            });
            if (netxtProcessRouteDetailLinks == null || netxtProcessRouteDetailLinks.Any() == false) throw new CustomerValidationException(nameof(ErrorCode.MES10440));

            // 获取当前工序在工艺路线里面的扩展信息（这里存放是Node表的工序ID，而不是主键ID，后期建议改为主键ID）
            var procedureNodes = await _procProcessRouteDetailNodeRepository.GetByProcedureIdsAsync(new ProcProcessRouteDetailNodesQuery
            {
                ProcessRouteId = processRouteId,
                ProcedureIds = netxtProcessRouteDetailLinks.Select(s => s.ProcessRouteDetailId)
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES10440));

            // 随机工序Key
            var cacheKey = $"{procedureId}-{workOrderId}";
            var count = await _sequenceService.GetSerialNumberAsync(Sequences.Enums.SerialNumberTypeEnum.None, cacheKey);

            // 这个Key太长了
            //var cacheKey = $"{manuSfcProduce.ProcessRouteId}-{manuSfcProduce.ProcedureId}-{manuSfcProduce.ResourceId}-{manuSfcProduce.WorkOrderId}";

            // 默认下一工序
            ProcProcessRouteDetailNodeEntity? defaultNextProcedure = null;

            // 有多工序分叉的情况
            if (procedureNodes.Count() > 1)
            {
                // 检查是否有"空值"类型的工序
                defaultNextProcedure = procedureNodes.FirstOrDefault(f => f.CheckType == ProcessRouteInspectTypeEnum.None)
                    ?? throw new CustomerValidationException(nameof(ErrorCode.MES10441));

                // 如果不是第一次走该工序，count是从1开始，不包括0。
                if (count > 1)
                {
                    // 抽检类型不为空值的下一工序
                    var nextProcedureOfNone = procedureNodes.FirstOrDefault(f => f.CheckType != ProcessRouteInspectTypeEnum.None)
                        ?? throw new CustomerValidationException(nameof(ErrorCode.MES10447));

                    // 判断工序抽检比例
                    if (nextProcedureOfNone.CheckType == ProcessRouteInspectTypeEnum.FixedScale
                        && nextProcedureOfNone.CheckRate == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10446));

                    // 如果满足抽检次数，就取出一个非"空值"的随机工序作为下一工序
                    if (count > 1 && count % nextProcedureOfNone.CheckRate == 0) defaultNextProcedure = nextProcedureOfNone;
                }
            }
            // 没有分叉的情况
            else
            {
                // 抽检类型不为空值的下一工序
                defaultNextProcedure = procedureNodes.FirstOrDefault()
                    ?? throw new CustomerValidationException(nameof(ErrorCode.MES10440));

                switch (defaultNextProcedure.CheckType)
                {
                    case ProcessRouteInspectTypeEnum.FixedScale:
                        // 判断工序抽检比例
                        if (defaultNextProcedure.CheckRate == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10446));
                        break;
                    case ProcessRouteInspectTypeEnum.None:
                    case ProcessRouteInspectTypeEnum.RandomInspection:
                    case ProcessRouteInspectTypeEnum.SpecialSamplingInspection:
                    default:
                        break;
                }
            }

            // 获取下一工序
            if (defaultNextProcedure == null) throw new CustomerValidationException(nameof(ErrorCode.MES10440));
            return await _procProcedureRepository.GetByIdAsync(defaultNextProcedure.ProcedureId);
        }

        /// <summary>
        /// 组装参数信息
        /// </summary>
        /// <param name="outBoundMoreDto"></param>
        /// <param name="manuSfcStepEntities"></param>
        /// <param name="procResourceId"></param>
        /// <returns></returns>
        private async Task<List<ManuProductParameterEntity>> PrepareProductParameterEntity(OutBoundMoreDto outBoundMoreDto, List<ManuSfcStepEntity> manuSfcStepEntities, long procResourceId)
        {
            List<ManuProductParameterEntity> manuProductParameterEntities = new();
            //所有参数
            List<string> paramCodeList = new();
            foreach (var item in outBoundMoreDto.SFCs)
            {
                if (item.ParamList != null)
                {
                    //系统中参数编码为大写
                    var paramCodes = item.ParamList.Select(c => c.ParamCode.ToUpper());
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
            var noIncludeCodes = paramCodeList.Where(w => procParameter.Select(s => s.ParameterCode.ToUpper()).Contains(w.ToUpper()) == false);
            if (noIncludeCodes.Any() == true)
                throw new CustomerValidationException(nameof(ErrorCode.MES19108)).WithData("Code", string.Join(',', noIncludeCodes));

            foreach (var outBoundDto in outBoundMoreDto.SFCs)
            {
                if (outBoundDto.ParamList != null)
                {
                    var stepId = manuSfcStepEntities.Where(c => c.SFC == outBoundDto.SFC).First().Id;
                    var paramList = outBoundDto.ParamList.Select(s =>
                         new ManuProductParameterEntity
                         {
                             Id = IdGenProvider.Instance.CreateId(),
                             SiteId = _currentEquipment.SiteId,
                             StepId = stepId,//记录步骤ID
                             CreatedBy = _currentEquipment.Code,
                             UpdatedBy = _currentEquipment.Code,
                             CreatedOn = HymsonClock.Now(),
                             UpdatedOn = HymsonClock.Now(),
                             EquipmentId = _currentEquipment.Id ?? 0,
                             LocalTime = outBoundMoreDto.LocalTime,
                             SFC = outBoundDto.SFC,
                             ResourceId = procResourceId,
                             ParameterId = procParameter.Where(c => c.ParameterCode.ToUpper().Equals(s.ParamCode.ToUpper())).First().Id,
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
                    var ngCodes = item.NG.Select(c => c.NGCode.ToUpper());
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
            var noIncludeCodes = ngCodeList.Where(w => qualUnqualifiedCodes.Select(s => s.UnqualifiedCode.ToUpper()).Contains(w.ToUpper()) == false);
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
                         UnqualifiedCode = s.NGCode.ToUpper(),
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

        /// <summary>
        /// 组装Material信息
        /// </summary>
        /// <param name="outBoundMoreDto"></param>
        /// <param name="manuSfcStepEntities"></param>
        /// <returns></returns>
        private List<ManuSfcStepMaterialEntity> PrepareProductMaterialEntity(OutBoundMoreDto outBoundMoreDto, List<ManuSfcStepEntity> manuSfcStepEntities)
        {
            List<ManuSfcStepMaterialEntity> manuSfcStepMaterialEntities = new();

            foreach (var outBoundDto in outBoundMoreDto.SFCs)
            {
                var stepId = manuSfcStepEntities.Where(c => c.SFC == outBoundDto.SFC).First().Id;
                if (outBoundDto.BindFeedingCodes != null && outBoundDto.BindFeedingCodes.Any())
                {
                    var materialList = outBoundDto.BindFeedingCodes.Select(materialCode =>
                     new ManuSfcStepMaterialEntity
                     {
                         Id = IdGenProvider.Instance.CreateId(),
                         StepId = stepId,
                         SiteId = _currentEquipment.SiteId,
                         SFC = outBoundDto.SFC,
                         MaterialBarcode = materialCode,
                         CreatedBy = _currentEquipment.Code,
                         UpdatedBy = _currentEquipment.Code,
                         CreatedOn = HymsonClock.Now(),
                         UpdatedOn = HymsonClock.Now()
                     });
                    manuSfcStepMaterialEntities.AddRange(materialList);
                }
            }
            return manuSfcStepMaterialEntities;
        }
    }
}
