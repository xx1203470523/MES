using FluentValidation;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcCirculation.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.EquipmentServices.Dtos.InBound;
using Hymson.MES.EquipmentServices.Dtos.Manufacture.ManuMainstreamProcessDto.ManuCommonDto;
using Hymson.MES.EquipmentServices.Dtos.OutBound;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Hymson.Web.Framework.WorkContext;

namespace Hymson.MES.EquipmentServices.Services.InBound
{
    /// <summary>
    /// 进站服务
    /// </summary>
    public class InBoundService : IInBoundService
    {
        #region Repository
        private readonly ICurrentEquipment _currentEquipment;
        private readonly AbstractValidator<InBoundDto> _validationInBoundDtoRules;
        private readonly AbstractValidator<InBoundMoreDto> _validationInBoundMoreDtoRules;
        private readonly IProcResourceRepository _procResourceRepository;
        private readonly IManuSfcRepository _manuSfcRepository;
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;
        private readonly IManuSfcStepRepository _manuSfcStepRepository;
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly IProcProcessRouteDetailNodeRepository _processRouteDetailNodeRepository;
        private readonly IProcProcedureRepository _procedureRepository;
        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;
        private readonly IProcProcedureRepository _procProcedureRepository;
        private readonly IProcProcessRouteDetailNodeRepository _procProcessRouteDetailNodeRepository;
        private readonly IProcResourceEquipmentBindRepository _procResourceEquipmentBindRepository;
        private readonly IManuSfcCirculationRepository _manuSfcCirculationRepository;

        public InBoundService(AbstractValidator<InBoundDto> validationInBoundDtoRules,
            ICurrentEquipment currentEquipment,
            AbstractValidator<InBoundMoreDto> validationInBoundMoreDtoRules,
            IProcResourceRepository procResourceRepository,
            IManuSfcStepRepository manuSfcStepRepository,
            IManuSfcRepository manuSfcRepository,
            IManuSfcInfoRepository manuSfcInfoRepository,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IProcProcessRouteDetailNodeRepository processRouteDetailNodeRepository,
            IProcProcedureRepository procedureRepository,
            IInteWorkCenterRepository inteWorkCenterRepository,
            IProcProcessRouteDetailNodeRepository procProcessRouteDetailNodeRepository,
            IProcProcedureRepository procProcedureRepository,
            IProcResourceEquipmentBindRepository procResourceEquipmentBindRepository,
            IManuSfcCirculationRepository manuSfcCirculationRepository)
        {
            _validationInBoundDtoRules = validationInBoundDtoRules;
            _currentEquipment = currentEquipment;
            _validationInBoundMoreDtoRules = validationInBoundMoreDtoRules;
            _procResourceRepository = procResourceRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _processRouteDetailNodeRepository = processRouteDetailNodeRepository;
            _procedureRepository = procedureRepository;
            _inteWorkCenterRepository = inteWorkCenterRepository;
            _procProcessRouteDetailNodeRepository = procProcessRouteDetailNodeRepository;
            _procProcedureRepository = procProcedureRepository;
            _procResourceEquipmentBindRepository = procResourceEquipmentBindRepository;
            _manuSfcCirculationRepository = manuSfcCirculationRepository;
        }
        #endregion

        /// <summary>
        /// 进站
        /// </summary>
        /// <param name="inBoundDto"></param>
        /// <returns></returns>
        public async Task InBound(InBoundDto inBoundDto)
        {
            await _validationInBoundDtoRules.ValidateAndThrowAsync(inBoundDto);
            if (inBoundDto == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }
            //已经验证过资源是否存在直接使用
            var inBoundMore = new InBoundMoreDto
            {
                SFCs = new string[] { inBoundDto.SFC },
                ResourceCode = inBoundDto.ResourceCode,
                LocalTime = inBoundDto.LocalTime,
                IsVerifyVirtualSFC = inBoundDto.IsVerifyVirtualSFC
            };
            await SFCInBound(inBoundMore);
        }

        /// <summary>
        /// 进站（多个）
        /// </summary>
        /// <param name="inBoundMoreDto"></param>
        /// <returns></returns>
        public async Task InBoundMore(InBoundMoreDto inBoundMoreDto)
        {
            await _validationInBoundMoreDtoRules.ValidateAndThrowAsync(inBoundMoreDto);
            if (inBoundMoreDto == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }
            if (inBoundMoreDto.SFCs.Length <= 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19101));
            }
            await SFCInBound(inBoundMoreDto);
        }

        /// <summary>
        /// 初版
        /// </summary>
        /// <param name="inBoundMoreDto"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        /// <exception cref="ValidationException"></exception>
        public async Task SFCInBound(InBoundMoreDto inBoundMoreDto)
        {
            //已经验证过资源是否存在直接使用
            var procResource = await _procResourceRepository.GetByCodeAsync(new EntityByCodeQuery { Site = _currentEquipment.SiteId, Code = inBoundMoreDto.ResourceCode });

            //查询资源和设备是否绑定
            var resourceEquipmentBindQuery = new ProcResourceEquipmentBindQuery
            {
                SiteId = _currentEquipment.SiteId,
                Ids = new long[] { _currentEquipment.Id ?? 0 },
                ResourceId = procResource.Id,
            };
            var resEquipentBind = await _procResourceEquipmentBindRepository.GetByResourceIdAsync(resourceEquipmentBindQuery);
            if (resEquipentBind.Any() == false)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19131)).WithData("ResCode", procResource.ResCode).WithData("EquCode", _currentEquipment.Code);
            }
            //验证虚拟条码
            if (inBoundMoreDto.IsVerifyVirtualSFC == true)
            {
                await VerifyVirtualSFC(inBoundMoreDto);
            }

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
            var sfclist = await _manuSfcRepository.GetBySFCsAsync(inBoundMoreDto.SFCs);
            //查询已经存在条码的生产信息
            var sfcProduceList = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(new ManuSfcProduceQuery { Sfcs = inBoundMoreDto.SFCs, SiteId = _currentEquipment.SiteId });
            //SFC有条码信息，但已经没有生产信息不允许进站
            var noIncludeSfcs = sfclist.Where(w => sfcProduceList.Select(s => s.SFC.ToUpper()).Contains(w.SFC.ToUpper()) == false);
            if (noIncludeSfcs.Any())
                throw new CustomerValidationException(nameof(ErrorCode.MES19126)).WithData("SFCS", string.Join(',', noIncludeSfcs));

            //查询已存在条码的工序信息
            List<ProcProcedureEntity> procedureEntityList = new List<ProcProcedureEntity>();
            var procProcedures = await _procProcedureRepository.GetByIdsAsync(sfcProduceList.Select(c => c.ProcedureId).ToArray());
            procedureEntityList = procProcedures.ToList();

            //复投次数限制，对于测试类型工序，条码复投数量大于等于工序循环次数限制进站
            var noNeedRepeat = sfcProduceList.Where(c => procedureEntityList.Where(p => p.Type == ProcedureTypeEnum.Test && (p.Cycle ?? 1) <= c.RepeatedCount)
                                                         .Select(p => p.Id).Contains(c.ProcedureId));
            if (noNeedRepeat.Any())
                throw new CustomerValidationException(nameof(ErrorCode.MES19130)).WithData("SFCS", string.Join(',', noNeedRepeat));

            //当前所在工序不是测试工序，条码不是排队状态不允许进站
            var noLinUpSFCs = sfcProduceList.Where(c => c.Status != SfcProduceStatusEnum.lineUp
                                                        && procedureEntityList.Where(p => p.Type == ProcedureTypeEnum.Test)
                                                        .Select(p => p.Id).Contains(c.ProcedureId) == false).Select(p => p.SFC);
            if (noLinUpSFCs.Any())
                throw new CustomerValidationException(nameof(ErrorCode.MES19129)).WithData("SFCS", string.Join(',', noLinUpSFCs));
            //查询已有的条码信息
            var sfcInfoEntities = await _manuSfcInfoRepository.GetBySFCIdsAsync(sfclist.Select(c => c.Id));

            //获取工艺路线首工序
            var processRouteFirstProcedure = await GetFirstProcedureAsync(planWorkOrderEntity.ProcessRouteId);

            List<ManuSfcEntity> manuSfcList = new List<ManuSfcEntity>();
            List<ManuSfcInfoEntity> manuSfcInfoList = new List<ManuSfcInfoEntity>();
            List<ManuSfcProduceEntity> manuSfcProduceList = new List<ManuSfcProduceEntity>();
            List<ManuSfcStepEntity> manuSfcStepList = new List<ManuSfcStepEntity>();
            //更新的信息
            List<ManuSfcProduceEntity> updateManuSfcProduceList = new List<ManuSfcProduceEntity>();
            List<ManuSfcEntity> updateManuSfcList = new List<ManuSfcEntity>();

            //var validationFailures = new List<ValidationFailure>();
            foreach (var sfc in inBoundMoreDto.SFCs)
            {
                var sfcEntity = sfclist.FirstOrDefault(x => x.SFC == sfc);
                if (sfcEntity != null)
                {
                    //当前SFC的生产信息
                    var sfcProduce = sfcProduceList.Where(c => c.SFC == sfc).First();
                    //进站修改为激活
                    sfcProduce.Status = SfcProduceStatusEnum.Activity;
                    //当前SFC的工序信息
                    var sfcprocedureEntity = procedureEntityList.Where(c => c.Id == sfcProduce.ProcedureId).First();
                    // 检查是否测试工序
                    if (sfcprocedureEntity.Type == ProcedureTypeEnum.Test)
                    {
                        // 超过复投次数，标识为NG
                        //if (sfcProduce.RepeatedCount > sfcprocedureEntity.Cycle) throw new CustomerValidationException(nameof(ErrorCode.MES16036));
                        sfcProduce.RepeatedCount++;
                    }
                    //是否首工序
                    var isFirstProcedure = await IsFirstProcedureAsync(sfcProduce.ProcessRouteId, sfcProduce.ProcedureId);
                    // 初始化步骤
                    var sfcStep = new ManuSfcStepEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = sfcProduce.SiteId,
                        SFC = sfcProduce.SFC,
                        ProductId = sfcProduce.ProductId,
                        WorkOrderId = sfcProduce.WorkOrderId,
                        WorkCenterId = sfcProduce.WorkCenterId,
                        ProductBOMId = sfcProduce.ProductBOMId,
                        ProcedureId = sfcProduce.ProcedureId,
                        Qty = 1,//数量1
                        Operatetype = ManuSfcStepTypeEnum.InStock,
                        CurrentStatus = SfcProduceStatusEnum.Activity,
                        EquipmentId = _currentEquipment.Id,
                        ResourceId = procResource.Id,
                        CreatedBy = _currentEquipment.Name,
                        CreatedOn = HymsonClock.Now(),
                        UpdatedBy = _currentEquipment.Name,
                        UpdatedOn = HymsonClock.Now(),
                    };
                    manuSfcStepList.Add(sfcStep);
                    updateManuSfcProduceList.Add(sfcProduce);
                    //更新条码为已使用
                    sfcEntity.UpdatedBy = _currentEquipment.Name;
                    sfcEntity.UpdatedOn = HymsonClock.Now();
                    sfcEntity.IsUsed = YesOrNoEnum.Yes;
                    updateManuSfcList.Add(sfcEntity);

                    //查询已有条码信息
                    var sfcInfoEntity = sfcInfoEntities.Where(c => c.SfcId == sfcEntity.Id).FirstOrDefault();
                    //已有条码信息不是当前工单
                    if ((sfcInfoEntity != null && sfcInfoEntity.WorkOrderId != planWorkOrderEntity.Id) || sfcInfoEntity == null)
                    {
                        manuSfcInfoList.Add(new ManuSfcInfoEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            SiteId = _currentEquipment.SiteId,
                            SfcId = sfcEntity.Id,
                            WorkOrderId = planWorkOrderEntity.Id,
                            ProductId = planWorkOrderEntity.ProductId,
                            IsUsed = true,
                            CreatedBy = _currentEquipment.Name,
                            UpdatedBy = _currentEquipment.Name
                        });
                    }
                    //条码生成信息
                    var manuSfcProduceEntity = sfcProduceList.Where(c => c.SFC == sfcEntity.SFC).FirstOrDefault();
                    if ((manuSfcProduceEntity != null && manuSfcProduceEntity.WorkOrderId != planWorkOrderEntity.Id) || manuSfcProduceEntity == null)
                    {
                        manuSfcProduceList.Add(new ManuSfcProduceEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            SiteId = _currentEquipment.SiteId,
                            SFC = sfc,
                            ProductId = planWorkOrderEntity.ProductId,
                            WorkOrderId = planWorkOrderEntity.Id,
                            BarCodeInfoId = sfcEntity.Id,
                            ProcessRouteId = planWorkOrderEntity.ProcessRouteId,
                            WorkCenterId = planWorkOrderEntity.WorkCenterId ?? 0,
                            ProductBOMId = planWorkOrderEntity.ProductBOMId,
                            EquipmentId = _currentEquipment.Id ?? 0,
                            ResourceId = procResource.Id,
                            Qty = 1,//电芯进站默认都是1个
                            ProcedureId = processRouteFirstProcedure.ProcedureId,
                            Status = SfcProduceStatusEnum.Activity,//接口进站直接为活动
                            RepeatedCount = 0,
                            IsScrap = TrueOrFalseEnum.No,
                            CreatedBy = _currentEquipment.Name,
                            UpdatedBy = _currentEquipment.Name
                        });
                    }
                    continue;
                }

                var manuSfcEntity = new ManuSfcEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentEquipment.SiteId,
                    SFC = sfc,
                    Qty = 1,//电芯进站默认都是1个
                    IsUsed = YesOrNoEnum.Yes,
                    Status = SfcStatusEnum.InProcess,
                    CreatedBy = _currentEquipment.Name,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentEquipment.Name,
                    UpdatedOn = HymsonClock.Now(),
                };
                manuSfcList.Add(manuSfcEntity);

                manuSfcInfoList.Add(new ManuSfcInfoEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentEquipment.SiteId,
                    SfcId = manuSfcEntity.Id,
                    WorkOrderId = planWorkOrderEntity.Id,
                    ProductId = planWorkOrderEntity.ProductId,
                    IsUsed = true,
                    CreatedBy = _currentEquipment.Name,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentEquipment.Name,
                    UpdatedOn = HymsonClock.Now(),
                });

                manuSfcProduceList.Add(new ManuSfcProduceEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentEquipment.SiteId,
                    SFC = sfc,
                    ProductId = planWorkOrderEntity.ProductId,
                    WorkOrderId = planWorkOrderEntity.Id,
                    BarCodeInfoId = manuSfcEntity.Id,
                    ProcessRouteId = planWorkOrderEntity.ProcessRouteId,
                    WorkCenterId = planWorkOrderEntity.WorkCenterId ?? 0,
                    ProductBOMId = planWorkOrderEntity.ProductBOMId,
                    EquipmentId = _currentEquipment.Id ?? 0,
                    ResourceId = procResource.Id,
                    Qty = 1,//电芯进站默认都是1个
                    ProcedureId = processRouteFirstProcedure.ProcedureId,
                    Status = SfcProduceStatusEnum.Activity,//接口进站直接为活动
                    RepeatedCount = 0,
                    IsScrap = TrueOrFalseEnum.No,
                    CreatedBy = _currentEquipment.Name,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentEquipment.Name,
                    UpdatedOn = HymsonClock.Now(),
                });

                manuSfcStepList.Add(new ManuSfcStepEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentEquipment.SiteId,
                    SFC = sfc,
                    ProductId = planWorkOrderEntity.ProductId,
                    WorkOrderId = planWorkOrderEntity.Id,
                    ProductBOMId = planWorkOrderEntity.ProductBOMId,
                    WorkCenterId = planWorkOrderEntity.WorkCenterId ?? 0,
                    Qty = 1,//电芯进站默认都是1个
                    ProcedureId = processRouteFirstProcedure.ProcedureId,
                    Operatetype = ManuSfcStepTypeEnum.InStock,
                    CurrentStatus = SfcProduceStatusEnum.Activity,
                    EquipmentId = _currentEquipment.Id,
                    ResourceId = procResource.Id,
                    CreatedBy = _currentEquipment.Name,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentEquipment.Name,
                    UpdatedOn = HymsonClock.Now(),
                });
            }
            //if (validationFailures.Any())
            //{
            //    throw new ValidationException(_localizationService.GetResource("SFCError"), validationFailures);
            //}

            using var ts = TransactionHelper.GetTransactionScope();
            //更新下达数量
            var row = await _planWorkOrderRepository.UpdatePassDownQuantityByWorkOrderId(new UpdatePassDownQuantityCommand
            {
                WorkOrderId = planWorkOrderEntity.Id,
                PlanQuantity = planWorkOrderEntity.Qty * (1 + planWorkOrderEntity.OverScale / 100),
                PassDownQuantity = inBoundMoreDto.SFCs.Length,
                UserName = _currentEquipment.Name,
                UpdateDate = HymsonClock.Now()
            });
            //更新投入数量
            row += await _planWorkOrderRepository.UpdateInputQtyByWorkOrderId(new UpdateQtyCommand
            {
                UpdatedBy = _currentEquipment.Name,
                UpdatedOn = HymsonClock.Now(),
                WorkOrderId = planWorkOrderEntity.Id,
                Qty = inBoundMoreDto.SFCs.Length,
            });

            // 更新工单统计表的 RealStart
            row += await _planWorkOrderRepository.UpdatePlanWorkOrderRealStartByWorkOrderIdAsync(new UpdateWorkOrderRealTimeCommand
            {
                UpdatedOn = HymsonClock.Now(),
                UpdatedBy = _currentEquipment.Name,
                WorkOrderIds = new long[] { planWorkOrderEntity.Id }
            });

            //更新manuSfcProduce
            if (updateManuSfcProduceList.Any())
            {
                row += await _manuSfcProduceRepository.UpdateRangeAsync(updateManuSfcProduceList);
            }
            //更新updateManuSfcList
            if (updateManuSfcList.Any())
            {
                row += await _manuSfcRepository.UpdateRangeAsync(updateManuSfcList);
            }

            if (row == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16503)).WithData("workorder", planWorkOrderEntity.OrderCode);
            }
            await _manuSfcRepository.InsertRangeAsync(manuSfcList);
            await _manuSfcInfoRepository.InsertsAsync(manuSfcInfoList);
            await _manuSfcProduceRepository.InsertRangeAsync(manuSfcProduceList);
            await _manuSfcStepRepository.InsertRangeAsync(manuSfcStepList);
            ts.Complete();
        }

        /// <summary>
        /// 校验虚拟码
        /// </summary>
        /// <param name="inBoundMoreDto"></param>
        /// <returns></returns>
        private async Task VerifyVirtualSFC(InBoundMoreDto inBoundMoreDto)
        {
            //SFC流转信息处理
            var manuSfcCirculationBarCodeQuery = new ManuSfcCirculationBarCodeQuery
            {
                CirculationType = SfcCirculationTypeEnum.Change,
                IsDisassemble = TrueOrFalseEnum.No,
                SiteId = _currentEquipment.SiteId,
                Sfcs = inBoundMoreDto.SFCs.Select(sfc => sfc).ToArray()
            };
            var manuSfcCirculationBarCodeEntities = await _manuSfcCirculationRepository.GetManuSfcCirculationBarCodeEntities(manuSfcCirculationBarCodeQuery);
            //如果有不存在的SFC就提示
            var noIncludeSfcs = inBoundMoreDto.SFCs.Where(sfc => manuSfcCirculationBarCodeEntities.Select(s => s.SFC).Contains(sfc) == false);
            if (noIncludeSfcs.Any() == true)
                throw new CustomerValidationException(nameof(ErrorCode.MES19134)).WithData("SFCS", string.Join(',', noIncludeSfcs));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="processRouteId"></param>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        public async Task<bool> IsFirstProcedureAsync(long processRouteId, long procedureId)
        {
            var firstProcedureDetailNodeEntity = await _procProcessRouteDetailNodeRepository.GetFirstProcedureByProcessRouteIdAsync(processRouteId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10435));

            return firstProcedureDetailNodeEntity.ProcedureId == procedureId;
        }

        /// <summary>
        /// 获取首工序
        /// </summary>
        /// <param name="processRouteId"></param>
        /// <returns></returns>
        public async Task<ProcessRouteProcedureDto> GetFirstProcedureAsync(long processRouteId)
        {
            var procProcessRouteDetailNodeEntity = await _processRouteDetailNodeRepository.GetFirstProcedureByProcessRouteIdAsync(processRouteId);
            if (procProcessRouteDetailNodeEntity == null) throw new CustomerValidationException(nameof(ErrorCode.MES16304));

            var procProcedureEntity = await _procedureRepository.GetByIdAsync(procProcessRouteDetailNodeEntity.ProcedureId);
            if (procProcedureEntity == null) throw new CustomerValidationException(nameof(ErrorCode.MES10406));

            return new ProcessRouteProcedureDto
            {
                ProcessRouteId = processRouteId,
                SerialNo = procProcessRouteDetailNodeEntity.SerialNo,
                ProcedureId = procProcessRouteDetailNodeEntity.ProcedureId,
                CheckType = procProcessRouteDetailNodeEntity.CheckType,
                CheckRate = procProcessRouteDetailNodeEntity.CheckRate,
                IsWorkReport = procProcessRouteDetailNodeEntity.IsWorkReport,
                ProcedureCode = procProcedureEntity.Code,
                ProcedureName = procProcedureEntity.Name,
                Type = procProcedureEntity.Type,
                PackingLevel = procProcedureEntity.PackingLevel,
                ResourceTypeId = procProcedureEntity.ResourceTypeId,
                Cycle = procProcedureEntity.Cycle,
                IsRepairReturn = procProcedureEntity.IsRepairReturn
            };
        }
    }
}
