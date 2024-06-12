using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Constants.Manufacture;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Parameter.Query;
using Hymson.MES.Data.Repositories.Quality.IQualityRepository;
using Hymson.MES.Data.Repositories.Quality.QualUnqualifiedCode.Query;
using Hymson.MES.EquipmentServices.Bos;
using Hymson.MES.EquipmentServices.Dtos;
using Hymson.MES.EquipmentServices.Dtos.EquipmentCollect;
using Hymson.MES.EquipmentServices.Services.Process;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Hymson.Web.Framework.WorkContext;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.EquipmentServices.Services.EquipmentCollect;

/// <summary>
/// 设备信息收集服务
/// @author Czhipu
/// @date 2023-05-16 04:51:15
/// </summary>
public class EquipmentCollectService : IEquipmentCollectService
{
    /// <summary>
    /// 
    /// </summary>
    private readonly ICurrentEquipment _currentEquipment;

    /// <summary>
    /// 仓储（设备心跳）
    /// </summary>
    private readonly IEquHeartbeatRepository _equipmentHeartbeatRepository;

    /// <summary>
    /// 仓储（设备报警）
    /// </summary>
    private readonly IEquAlarmRepository _equipmentAlarmRepository;

    /// <summary>
    /// 仓储（设备状态）
    /// </summary>
    private readonly IEquStatusRepository _equipmentStatusRepository;

    /// <summary>
    /// 仓储（设备生产参数）
    /// </summary>
    private readonly IEquProductParameterRepository _equProductParameterRepository;

    /// <summary>
    /// 仓储（资源维护）
    /// </summary>
    private readonly IProcResourceRepository _procResourceRepository;

    /// <summary>
    /// 仓储（标准参数）
    /// </summary>
    private readonly IProcParameterRepository _procParameterRepository;

    /// <summary>
    /// 仓储（标准参数）
    /// </summary>
    private readonly IManuProductParameterRepository _manuProductParameterRepository;
    /// <summary>
    /// 工作中心
    /// </summary>
    private readonly IInteWorkCenterRepository _inteWorkCenterRepository;
    /// <summary>
    /// 工单
    /// </summary>
    private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

    private readonly IQualUnqualifiedCodeRepository _qualUnqualifiedCodeRepository;

    private readonly IManuSfcStepRepository _manuSfcStepRepository;

    private readonly IManuSfcStepNgRepository _manuSfcStepNgRepository;

    private readonly IManuSfcSummaryRepository _manuSfcSummaryRepository;

    private readonly IProcBootuprecipeService _procBootuprecipeService;

    private readonly IProcBootupparamService _procBootupparamService;

    private readonly IProcProcedureRepository _procProcedureRepository;


    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="currentEquipment"></param>
    /// <param name="equipmentHeartbeatRepository"></param>
    /// <param name="equipmentAlarmRepository"></param>
    /// <param name="equipmentStatusRepository"></param>
    /// <param name="equProductParameterRepository"></param>
    /// <param name="procResourceRepository"></param>
    /// <param name="procParameterRepository"></param>
    /// <param name="manuProductParameterRepository"></param>
    /// <param name="inteWorkCenterRepository"></param>
    /// <param name="planWorkOrderRepository"></param>
    /// <param name="qualUnqualifiedCodeRepository"></param>
    /// <param name="manuSfcStepNgRepository"></param>
    /// <param name="manuSfcStepRepository"></param>
    /// <param name="manuSfcSummaryRepository"></param>
    public EquipmentCollectService(ICurrentEquipment currentEquipment,
        IEquHeartbeatRepository equipmentHeartbeatRepository,
        IEquAlarmRepository equipmentAlarmRepository,
        IEquStatusRepository equipmentStatusRepository,
        IEquProductParameterRepository equProductParameterRepository,
        IProcResourceRepository procResourceRepository,
        IProcParameterRepository procParameterRepository,
        IManuProductParameterRepository manuProductParameterRepository,
        IInteWorkCenterRepository inteWorkCenterRepository,
        IPlanWorkOrderRepository planWorkOrderRepository,
        IQualUnqualifiedCodeRepository qualUnqualifiedCodeRepository,
        IManuSfcStepNgRepository manuSfcStepNgRepository,
        IManuSfcStepRepository manuSfcStepRepository,
        IManuSfcSummaryRepository manuSfcSummaryRepository,
        IProcBootuprecipeService procBootuprecipeService,
        IProcBootupparamService procBootupparamService,
        IProcProcedureRepository procProcedureRepository)
    {
        _currentEquipment = currentEquipment;
        _equipmentHeartbeatRepository = equipmentHeartbeatRepository;
        _equipmentAlarmRepository = equipmentAlarmRepository;
        _equipmentStatusRepository = equipmentStatusRepository;
        _equProductParameterRepository = equProductParameterRepository;
        _procResourceRepository = procResourceRepository;
        _procParameterRepository = procParameterRepository;
        _manuProductParameterRepository = manuProductParameterRepository;
        _inteWorkCenterRepository = inteWorkCenterRepository;
        _planWorkOrderRepository = planWorkOrderRepository;
        _qualUnqualifiedCodeRepository = qualUnqualifiedCodeRepository;
        _manuSfcStepNgRepository = manuSfcStepNgRepository;
        _manuSfcStepRepository = manuSfcStepRepository;
        _manuSfcSummaryRepository = manuSfcSummaryRepository;
        _procBootuprecipeService = procBootuprecipeService;
        _procBootupparamService = procBootupparamService;
        _procProcedureRepository = procProcedureRepository;
    }


    /// <summary>
    /// 设备心跳
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task EquipmentHeartbeatAsync(EquipmentHeartbeatDto request)
    {
        var nowTime = HymsonClock.Now();

        var entity = new EquHeartbeatEntity
        {
            Id = IdGenProvider.Instance.CreateId(),
            SiteId = _currentEquipment.SiteId,
            CreatedBy = _currentEquipment.Code,
            UpdatedBy = _currentEquipment.Code,
            CreatedOn = nowTime,
            UpdatedOn = nowTime,
            EquipmentId = _currentEquipment.Id ?? 0,
            Status = request.IsOnline,
            LastOnLineTime = request.LocalTime
        };

        using var trans = TransactionHelper.GetTransactionScope();
        await _equipmentHeartbeatRepository.InsertAsync(entity);
        await _equipmentHeartbeatRepository.InsertRecordAsync(new EquHeartbeatRecordEntity
        {
            Id = IdGenProvider.Instance.CreateId(),
            SiteId = entity.SiteId,
            CreatedBy = entity.CreatedBy,
            CreatedOn = entity.CreatedOn,
            UpdatedBy = entity.UpdatedBy,
            UpdatedOn = entity.UpdatedOn,
            EquipmentId = entity.EquipmentId,
            LocalTime = request.LocalTime,
            Status = entity.Status
        });
        trans.Complete();
    }

    /// <summary>
    /// 设备状态监控
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task EquipmentStateAsync(EquipmentStateDto request)
    {
        var nowTime = HymsonClock.Now();

        await UpdateEquipmentStatusAsync(new EquStatusEntity
        {
            Id = IdGenProvider.Instance.CreateId(),
            SiteId = _currentEquipment.SiteId,
            CreatedBy = _currentEquipment.Code,
            UpdatedBy = _currentEquipment.Code,
            CreatedOn = nowTime,
            UpdatedOn = nowTime,
            EquipmentId = _currentEquipment.Id ?? 0,
            LocalTime = request.LocalTime,
            EquipmentStatus = request.StateCode
        });
    }

    /// <summary>
    /// 设备报警
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task EquipmentAlarmAsync(EquipmentAlarmDto request)
    {
        var nowTime = HymsonClock.Now();

        await _equipmentAlarmRepository.InsertAsync(new EquAlarmEntity
        {
            Id = IdGenProvider.Instance.CreateId(),
            SiteId = _currentEquipment.SiteId,
            CreatedBy = _currentEquipment.Code,
            UpdatedBy = _currentEquipment.Code,
            CreatedOn = nowTime,
            UpdatedOn = nowTime,
            EquipmentId = _currentEquipment.Id ?? 0,
            LocalTime = request.LocalTime,
            FaultCode = request.AlarmCode,
            AlarmMsg = request.AlarmMsg ?? "",
            Status = request.Status
        });
    }

    /// <summary>
    /// 设备停机原因
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task EquipmentDownReasonAsync(EquipmentDownReasonDto request)
    {
        var nowTime = HymsonClock.Now();

        await UpdateEquipmentStatusAsync(new EquStatusEntity
        {
            Id = IdGenProvider.Instance.CreateId(),
            SiteId = _currentEquipment.SiteId,
            CreatedBy = _currentEquipment.Code,
            UpdatedBy = _currentEquipment.Code,
            CreatedOn = nowTime,
            UpdatedOn = nowTime,
            EquipmentId = _currentEquipment.Id ?? 0,
            LocalTime = request.LocalTime,
            EquipmentStatus = EquipmentStateEnum.DownNormal, // 暂定为正常停机
            LossRemark = request.DownReasonCode.GetDescription(),
            BeginTime = request.BeginTime,
            EndTime = request.EndTime
        });
    }



    /// <summary>
    /// 设备过程参数采集(无在制品条码)
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task EquipmentProcessParamAsync(EquipmentProcessParamDto request)
    {
        var nowTime = HymsonClock.Now();

        if (request.ParamList == null || request.ParamList.Any() == false) throw new CustomerValidationException(nameof(ErrorCode.MES19107));

        // 查询设备参数
        var paramCodes = request.ParamList.Select(s => s.ParamCode);
        var (parameterEntities, resourceEntity) = await GetEntitiesWithCheckAsync(paramCodes, request.ResourceCode);

        var entities = request.ParamList.Select(s => new EquProductParameterEntity
        {
            Id = IdGenProvider.Instance.CreateId(),
            SiteId = _currentEquipment.SiteId,
            CreatedBy = _currentEquipment.Code,
            UpdatedBy = _currentEquipment.Code,
            CreatedOn = nowTime,
            UpdatedOn = nowTime,
            EquipmentId = _currentEquipment.Id ?? 0,
            LocalTime = request.LocalTime,

            //ProcedureId = 0,
            ResourceId = resourceEntity.Id,
            ParameterId = GetParameterIdByParameterCode(s.ParamCode, parameterEntities),
            ParamValue = s.ParamValue,
            StandardUpperLimit = s.StandardUpperLimit,
            StandardLowerLimit = s.StandardLowerLimit,
            JudgmentResult = s.JudgmentResult,
            TestDuration = s.TestDuration,
            TestTime = s.TestTime,
            TestResult = s.TestResult,
            Timestamp = s.Timestamp
        });

        await _equProductParameterRepository.InsertsAsync(entities);
    }

    /// <summary>
    /// 设备产品过程参数采集(无在制品条码)
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<string> EquipmentProductProcessParamInNotCanSFCAsync(EquipmentProductProcessParamInNotCanSFCDto request)
    {
        var nowTime = HymsonClock.Now();

        if (request.ParamList == null || request.ParamList.Any() == false) throw new CustomerValidationException(nameof(ErrorCode.MES19107));

        // 查询设备参数
        var paramCodes = request.ParamList.Select(s => s.ParamCode);
        var (parameterEntities, resourceEntity) = await GetEntitiesWithCheckAsync(paramCodes, request.ResourceCode);

        // 校验数据库是否存在该设备的值
        var isExists = await _manuProductParameterRepository.IsExistsAsync(new EquipmentIdQuery
        {
            SiteId = resourceEntity.SiteId,
            EquipmentId = _currentEquipment.Id ?? 0,
            ResourceId = resourceEntity.Id,
            SFC = ManuProductParameter.DefaultSFC
        });
        if (isExists == true) throw new CustomerValidationException(nameof(ErrorCode.MES19113));

        var entities = request.ParamList.Select(s => new ManuProductParameterEntity
        {
            Id = IdGenProvider.Instance.CreateId(),
            SiteId = _currentEquipment.SiteId,
            CreatedBy = _currentEquipment.Code,
            UpdatedBy = _currentEquipment.Code,
            CreatedOn = nowTime,
            UpdatedOn = nowTime,
            EquipmentId = _currentEquipment.Id ?? 0,
            LocalTime = request.LocalTime,

            //ProcedureId = 0,
            SFC = ManuProductParameter.DefaultSFC,
            ResourceId = resourceEntity.Id,
            ParameterId = GetParameterIdByParameterCode(s.ParamCode, parameterEntities),
            ParamValue = s.ParamValue,
            StandardUpperLimit = s.StandardUpperLimit,
            StandardLowerLimit = s.StandardLowerLimit,
            JudgmentResult = s.JudgmentResult,
            TestDuration = s.TestDuration,
            TestTime = s.TestTime,
            TestResult = s.TestResult,
            Timestamp = s.Timestamp
        });

        await _manuProductParameterRepository.InsertsAsync(entities);

        return ManuProductParameter.DefaultSFC;
    }

    /// <summary>
    /// 设备产品过程参数采集
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task EquipmentProductProcessParamAsync(EquipmentProductProcessParamDto request)
    {
        var nowTime = HymsonClock.Now();

        if (request.SFCParams == null || request.SFCParams.Any() == false) throw new CustomerValidationException(nameof(ErrorCode.MES19110));
        if (request.SFCParams.Any(a => a.ParamList == null
        || a.SFC.IsEmpty() == true
        || a.ParamList.Any() == false)) throw new CustomerValidationException(nameof(ErrorCode.MES19107));

        //查询当前资源
        var procResource = await _procResourceRepository.GetByCodeAsync(new EntityByCodeQuery { Site = _currentEquipment.SiteId, Code = request.ResourceCode });
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

        // 查询设备参数
        List<EquipmentProductParamBo> paramList = new();
        foreach (var item in request.SFCParams)
        {
            paramList.AddRange(item.ParamList.Select(s => new EquipmentProductParamBo
            {
                SFC = item.SFC,
                ParamCode = s.ParamCode,
                ParamValue = s.ParamValue,
                Timestamp = s.Timestamp,
                StandardLowerLimit = s.StandardLowerLimit,
                StandardUpperLimit = s.StandardUpperLimit,
                TestDuration = s.TestDuration,
                TestResult = s.TestResult,
                TestTime = s.TestTime,
                JudgmentResult = s.JudgmentResult,
            }));
        }

        var paramCodes = paramList.Select(s => s.ParamCode);
        // var (parameterEntities, resourceEntity) = await GetEntitiesWithCheckAsync(paramCodes, request.ResourceCode);

        // 找出不在数据库里面的Code，自动新增标准参数，应对设备不合理需求
        List<ProcParameterEntity> procParameterEntities = new List<ProcParameterEntity>();
        var codesQuery = new ProcParametersByCodeQuery
        {
            SiteId = _currentEquipment.SiteId,
            Codes = paramCodes.ToArray()
        };
        var parameterEntities = await _procParameterRepository.GetByCodesAsync(codesQuery);
        var noIncludeCodes = paramCodes.Where(w => parameterEntities.Select(s => s.ParameterCode).Contains(w) == false);
        if (noIncludeCodes.Any())
        {
            foreach (var item in noIncludeCodes)
            {
                if (!procParameterEntities.Where(c => c.ParameterCode == item).Any())
                {
                    procParameterEntities.Add(new ProcParameterEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = _currentEquipment.SiteId,
                        CreatedBy = _currentEquipment.Code,
                        UpdatedBy = _currentEquipment.Code,
                        CreatedOn = nowTime,
                        UpdatedOn = nowTime,
                        ParameterCode = item,
                        ParameterName = item,
                        Remark = "自动创建"
                    });
                }
            }
            //将要新增的参数也添加到查询列表中
            parameterEntities = parameterEntities.Concat(procParameterEntities);
        }
        // 查询资源
        var resourceEntity = await _procResourceRepository.GetByCodeAsync(new EntityByCodeQuery
        {
            Site = _currentEquipment.SiteId,
            Code = request.ResourceCode,
        }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES19109)).WithData("Code", request.ResourceCode);

        var entities = paramList.Select(s => new ManuProductParameterEntity
        {
            Id = IdGenProvider.Instance.CreateId(),
            SiteId = _currentEquipment.SiteId,
            CreatedBy = _currentEquipment.Code,
            UpdatedBy = _currentEquipment.Code,
            CreatedOn = nowTime,
            UpdatedOn = nowTime,
            EquipmentId = _currentEquipment.Id ?? 0,
            LocalTime = request.LocalTime,
            WorkOrderId = planWorkOrder.Id,//工单ID
            //ProcedureId = planWorkOrder.ProductId,//工序ID
            ProductId = planWorkOrder.ProductId,//产品ID
            SFC = s.SFC,
            ResourceId = resourceEntity.Id,
            ParameterId = GetParameterIdByParameterCode(s.ParamCode, parameterEntities),
            ParamValue = s.ParamValue,
            StandardUpperLimit = s.StandardUpperLimit,
            StandardLowerLimit = s.StandardLowerLimit,
            JudgmentResult = s.JudgmentResult,
            TestDuration = s.TestDuration,
            TestTime = s.TestTime,
            TestResult = s.TestResult,
            Timestamp = s.Timestamp
        });

        //查询已有汇总信息
        ManuSfcSummaryQuery manuSfcSummaryQuery = new ManuSfcSummaryQuery
        {
            SiteId = _currentEquipment.SiteId,
            SFCS = request.SFCParams.Select(c => c.SFC).ToArray()
        };
        var manuSfcSummaryEntities = await _manuSfcSummaryRepository.GetManuSfcSummaryEntitiesAsync(manuSfcSummaryQuery);

        //List<ManuSfcSummaryEntity> manuSfcSummaryList = new List<ManuSfcSummaryEntity>();
        //Ng列表
        var manuSfcStepEntities = request.SFCParams.Select(s =>
        {
            ////汇总信息
            //var manuSfcSummaryEntity = manuSfcSummaryEntities.Where(c => c.SFC == s.SFC).FirstOrDefault();
            ////汇总表 模组OCVR不再更改结果
            //if (manuSfcSummaryEntity != null && _currentEquipment.Code != "YTLPACK01AE018")
            //{
            //    manuSfcSummaryEntity.UpdatedBy = _currentEquipment.Name;
            //    manuSfcSummaryEntity.UpdatedOn = HymsonClock.Now();
            //    manuSfcSummaryEntity.QualityStatus = 0;
            //    manuSfcSummaryEntity.NgNum++;
            //    manuSfcSummaryList.Add(manuSfcSummaryEntity);
            //}
            return new ManuSfcStepEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = _currentEquipment.SiteId,
                SFC = s.SFC,
                ProductId = planWorkOrder.ProductId,
                WorkOrderId = planWorkOrder.Id,
                WorkCenterId = planWorkOrder.WorkCenterId,
                ProductBOMId = planWorkOrder.ProductBOMId,
                //ProcedureId = planWorkOrder.ProcedureId,
                Qty = 1,//数量1
                Operatetype = ManuSfcStepTypeEnum.NG,
                CurrentStatus = SfcProduceStatusEnum.Activity,
                EquipmentId = _currentEquipment.Id,
                ResourceId = procResource.Id,
                CreatedBy = _currentEquipment.Name,
                CreatedOn = HymsonClock.Now(),
                UpdatedBy = _currentEquipment.Name,
                UpdatedOn = HymsonClock.Now(),
                Passed = 0//NG上报的都不合格
            };
        }).ToList();
        var manuSfcStepNgs = await PrepareProductNgEntityAsync(request.SFCParams, manuSfcStepEntities);

        // 开启事务
        using var trans = TransactionHelper.GetTransactionScope();
        await _manuSfcStepRepository.InsertRangeAsync(manuSfcStepEntities);
        await _manuSfcStepNgRepository.InsertsAsync(manuSfcStepNgs);
        await _procParameterRepository.InsertRangeAsync(procParameterEntities);
        await _manuProductParameterRepository.InsertsAsync(entities);
        //await _manuSfcSummaryRepository.InsertOrUpdateRangeAsync(manuSfcSummaryList);
        trans.Complete();
    }


    /// <summary>
    /// 设备产品NG录入
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task EquipmentProductNgAsync(EquipmentProductNgDto request)
    {
        var nowTime = HymsonClock.Now();

        if (request.SFCParams == null || request.SFCParams.Any() == false) throw new CustomerValidationException(nameof(ErrorCode.MES19110));
        //查询当前资源
        var procResource = await _procResourceRepository.GetByCodeAsync(new EntityByCodeQuery { Site = _currentEquipment.SiteId, Code = request.ResourceCode });
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

        // 查询资源
        var resourceEntity = await _procResourceRepository.GetByCodeAsync(new EntityByCodeQuery
        {
            Site = _currentEquipment.SiteId,
            Code = request.ResourceCode,
        }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES19109)).WithData("Code", request.ResourceCode);

        //查询已有汇总信息
        ManuSfcSummaryQuery manuSfcSummaryQuery = new ManuSfcSummaryQuery
        {
            SiteId = _currentEquipment.SiteId,
            SFCS = request.SFCParams.Select(c => c.SFC).ToArray()
        };
        var manuSfcSummaryEntities = await _manuSfcSummaryRepository.GetManuSfcSummaryEntitiesAsync(manuSfcSummaryQuery);

        List<ManuSfcSummaryEntity> manuSfcSummaryList = new List<ManuSfcSummaryEntity>();
        //Ng列表
        var manuSfcStepEntities = request.SFCParams.Select(s =>
        {
            //汇总信息
            var manuSfcSummaryEntity = manuSfcSummaryEntities.Where(c => c.SFC == s.SFC).FirstOrDefault();
            //汇总表
            if (manuSfcSummaryEntity != null)
            {
                manuSfcSummaryEntity.UpdatedBy = _currentEquipment.Name;
                manuSfcSummaryEntity.UpdatedOn = HymsonClock.Now();
                manuSfcSummaryEntity.QualityStatus = 0;
                manuSfcSummaryEntity.NgNum++;
                manuSfcSummaryList.Add(manuSfcSummaryEntity);
            }
            return new ManuSfcStepEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = _currentEquipment.SiteId,
                SFC = s.SFC,
                ProductId = planWorkOrder.ProductId,
                WorkOrderId = planWorkOrder.Id,
                WorkCenterId = planWorkOrder.WorkCenterId,
                ProductBOMId = planWorkOrder.ProductBOMId,
                //ProcedureId = planWorkOrder.ProcedureId,
                Qty = 1,//数量1
                Operatetype = ManuSfcStepTypeEnum.NG,
                CurrentStatus = SfcProduceStatusEnum.Activity,
                EquipmentId = _currentEquipment.Id,
                ResourceId = procResource.Id,
                CreatedBy = _currentEquipment.Name,
                CreatedOn = HymsonClock.Now(),
                UpdatedBy = _currentEquipment.Name,
                UpdatedOn = HymsonClock.Now(),
                Passed = 0//NG上报的都不合格
            };
        }).ToList();
        var productProcessParamSFCDtos = request.SFCParams.Select(c => new EquipmentProductProcessParamSFCDto
        {
            SFC = c.SFC,
            NgList = c.NgList,
        });
        var manuSfcStepNgs = await PrepareProductNgEntityAsync(productProcessParamSFCDtos, manuSfcStepEntities);

        // 开启事务
        using var trans = TransactionHelper.GetTransactionScope();
        await _manuSfcStepRepository.InsertRangeAsync(manuSfcStepEntities);
        await _manuSfcStepNgRepository.InsertsAsync(manuSfcStepNgs);
        await _manuSfcSummaryRepository.InsertOrUpdateRangeAsync(manuSfcSummaryList);
        trans.Complete();
    }


    /// <summary>
    /// 组装NG信息
    /// </summary>
    /// <param name="productProcessParamSFCDtos"></param>
    /// <param name="manuSfcStepEntities"></param>
    /// <returns></returns>
    private async Task<List<ManuSfcStepNgEntity>> PrepareProductNgEntityAsync(IEnumerable<EquipmentProductProcessParamSFCDto> productProcessParamSFCDtos, List<ManuSfcStepEntity> manuSfcStepEntities)
    {
        List<ManuSfcStepNgEntity> manuSfcStepNgEntities = new();
        //所有NG编码
        List<string> ngCodeList = new();
        foreach (var item in productProcessParamSFCDtos)
        {
            if (item.NgList != null)
            {
                var ngCodes = item.NgList.Select(c => c.NGCode.ToUpper());
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

        foreach (var processParamSFCDto in productProcessParamSFCDtos)
        {
            var stepId = manuSfcStepEntities.Where(c => c.SFC == processParamSFCDto.SFC).First().Id;
            if (processParamSFCDto.NgList != null)
            {
                var ngList = processParamSFCDto.NgList.Select(s =>
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

    #region 内部方法
    /// <summary>
    /// 更新设备状态
    /// </summary>
    /// <param name="currentStatusEntity"></param>
    /// <returns></returns>
    private async Task UpdateEquipmentStatusAsync(EquStatusEntity currentStatusEntity)
    {
        if (currentStatusEntity == null) return;

        // 最近的状态记录
        var lastStatusEntity = await _equipmentStatusRepository.GetLastEntityByEquipmentIdAsync(currentStatusEntity.EquipmentId);

        // 开启事务
        using var trans = TransactionHelper.GetTransactionScope();

        // 更新设备当前状态
        await _equipmentStatusRepository.InsertAsync(currentStatusEntity);

        // 更新统计表
        if (lastStatusEntity != null && currentStatusEntity.EquipmentStatus != lastStatusEntity.EquipmentStatus)
        {
            await _equipmentStatusRepository.InsertStatisticsAsync(new EquStatusStatisticsEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = currentStatusEntity.SiteId,
                CreatedBy = currentStatusEntity.CreatedBy,
                CreatedOn = currentStatusEntity.CreatedOn,
                UpdatedBy = currentStatusEntity.UpdatedBy,
                UpdatedOn = currentStatusEntity.UpdatedOn,
                EquipmentId = currentStatusEntity.EquipmentId,
                EquipmentStatus = lastStatusEntity.EquipmentStatus,
                SwitchEquipmentStatus = currentStatusEntity.EquipmentStatus,
                BeginTime = lastStatusEntity.LocalTime,
                EndTime = currentStatusEntity.LocalTime
            });
        }

        trans.Complete();
    }

    /// <summary>
    /// 获取相关实体（附带校验）
    /// </summary>
    /// <param name="paramCodes"></param>
    /// <param name="resourceCode"></param>
    /// <returns></returns>
    private async Task<(IEnumerable<ProcParameterEntity>, ProcResourceEntity)> GetEntitiesWithCheckAsync(IEnumerable<string> paramCodes, string resourceCode)
    {
        var parameterEntities = await _procParameterRepository.GetAllAsync(_currentEquipment.SiteId);
        parameterEntities = parameterEntities.Where(w => paramCodes.Contains(w.ParameterCode));

        // 找出不在数据库里面的Code
        var noIncludeCodes = paramCodes.Where(w => parameterEntities.Select(s => s.ParameterCode).Contains(w) == false);
        if (noIncludeCodes.Any() == true) throw new CustomerValidationException(nameof(ErrorCode.MES19108)).WithData("Code", string.Join(',', noIncludeCodes));

        // 查询资源
        var resourceEntity = await _procResourceRepository.GetByCodeAsync(new EntityByCodeQuery
        {
            Site = _currentEquipment.SiteId,
            Code = resourceCode,
        }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES19109)).WithData("Code", resourceCode);

        return (parameterEntities, resourceEntity);
    }

    /// <summary>
    /// 根据参数编码获取参数Id
    /// </summary>
    /// <param name="parameterCode"></param>
    /// <param name="parameterEntities"></param>
    /// <returns></returns>
    private static long GetParameterIdByParameterCode(string parameterCode, IEnumerable<ProcParameterEntity> parameterEntities)
    {
        var entity = parameterEntities.FirstOrDefault(f => f.ParameterCode == parameterCode);
        if (entity == null) return 0;

        return entity.Id;
    }

    #endregion

    /// <summary>
    /// 获取开机配方版本集合
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public async Task<List<BootupParam>> GetEquipmentBootupRecipeSetAsync(GetEquipmentBootupRecipeSetDto dto)
    {
        return await _procBootuprecipeService.GetEquipmentBootupRecipeSetAsync(dto);
    }


    /// <summary>
    /// 设备开机参数采集
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public async Task EquipmentBootupParamCollectAsync(BootupParamCollectDto dto)
    {
        await _procBootupparamService.EquipmentBootupParamCollectAsync(dto);
    }
    /// <summary>
    /// 设备开机参数版本校验
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public async Task EquipmentBootupParamVersonCheckAsync(EquipmentBootupParamVersonCheckDto dto)
    {
        await _procBootuprecipeService.EquipmentBootupParamVersonCheckAsync(dto);
    }
    /// <summary>
    /// 获取设备开机参数明细
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public async Task<BootupParamDetail> GetEquipmentBootupRecipeDetailAsync(GetEquipmentBootupParamDetailDto dto)
    {
        return await _procBootupparamService.GetEquipmentBootupRecipeDetailAsync(dto);
    }

    /// <summary>
    /// 获取条码记录状态
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    public async Task<ManuSfcStatusOutputDto> GetManuSfcStatusByProcedureAsync(ManuSfcStatusQueryDto queryDto)
    {
        if (queryDto.SFC == null) throw new CustomerValidationException(ErrorCode.MES19003);
        if (queryDto.ProcedureCode == null) throw new CustomerValidationException(ErrorCode.MES19010);

        var procedureEntity = await _procProcedureRepository.GetByCodeAsync(queryDto.ProcedureCode ?? "", 123456);

        var manuSfcSummaryEntities = await _manuSfcSummaryRepository.GetManuSfcSummaryEntitiesAsync(new()
        {
            SFCS = new string[] { queryDto.SFC },
            QualityStatus = 0,
            ProcedureIds = new long[] { procedureEntity.Id }
        });

        var manuSfcSummaryEntity = manuSfcSummaryEntities.OrderByDescending(a => a.CreatedOn).FirstOrDefault();

        ManuSfcStatusOutputDto result = new ManuSfcStatusOutputDto()
        {
            SFC = queryDto.SFC,
            ProcedureCode = procedureEntity.Code ?? "",
            ProcedureName = procedureEntity.Name ?? "",
            FirstQualityStatus = manuSfcSummaryEntity?.FirstQualityStatus ?? 1,
            QualityStatus = manuSfcSummaryEntity?.QualityStatus ?? 1
        };

        return result;
    }
}
