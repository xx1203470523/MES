﻿using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.SystemApi;
using Hymson.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hymson.MES.Core.Constants;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcCirculation.Query;
using Hymson.MES.SystemServices.Dtos.Api;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Process;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command;
using Hymson.Utils;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Authentication;
using Hymson.MES.Core.Constants.Process;

namespace Hymson.MES.SystemServices.Services.Api;

/// <summary>
/// 系统对接第三方接口
/// </summary>
public class SystemApiService : ISystemApiService
{
    #region 仓储层

    private readonly ICurrentSite _currentSite;
    private readonly ICurrentUser _currentUser;

    private readonly IManuSfcStepRepository _manuSfcStepRepository;
    private readonly IManuProductParameterRepository _manuProductParameterRepository;
    private readonly IManuSfcCirculationRepository _manuCirculationRepository;

    private readonly IEquEquipmentRepository _equEquipmentRepository;
    private readonly IProcProcedureRepository _procProcedureRepository;
    private readonly IProcMaterialRepository _procMaterialRepository;
    private readonly IInteWorkCenterRepository _inteWorkCenterRepository;
    private readonly IProcParameterRepository _procParameterRepository;
    private readonly IProcResourceRepository _procResourceRepository;
    private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
    private readonly IManuSfcProduceRepository _manuSfcProduceRepository;
    private readonly IProcProcessRouteDetailLinkRepository _procProcessRouteDetailLinkRepository;

    #endregion

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="currentSite"></param>
    /// <param name="currentUser"></param>
    /// <param name="manuSfcStepRepository"></param>
    /// <param name="manuProductParameterRepository"></param>
    /// <param name="manuCirculationRepository"></param>
    /// <param name="equEquipmentRepository"></param>
    /// <param name="procProcedureRepository"></param>
    /// <param name="procMaterialRepository"></param>
    /// <param name="inteWorkCenterRepository"></param>
    /// <param name="procParameterRepository"></param>
    /// <param name="procResourceRepository"></param>
    /// <param name="planWorkOrderRepository"></param>
    /// <param name="manuSfcProduceRepository"></param>
    /// <param name="procProcessRouteDetailLinkRepository"></param>
    public SystemApiService(ICurrentSite currentSite,
        ICurrentUser currentUser,
        IManuSfcStepRepository manuSfcStepRepository,
        IManuProductParameterRepository manuProductParameterRepository,
        IManuSfcCirculationRepository manuCirculationRepository,
        IEquEquipmentRepository equEquipmentRepository,
        IProcProcedureRepository procProcedureRepository,
        IProcMaterialRepository procMaterialRepository,
        IInteWorkCenterRepository inteWorkCenterRepository,
        IProcParameterRepository procParameterRepository,
        IProcResourceRepository procResourceRepository,
        IPlanWorkOrderRepository planWorkOrderRepository,
        IManuSfcProduceRepository manuSfcProduceRepository,
        IProcProcessRouteDetailLinkRepository procProcessRouteDetailLinkRepository)
    {
        _currentSite = currentSite;
        _currentUser = currentUser;
        _manuSfcStepRepository = manuSfcStepRepository;
        _manuProductParameterRepository = manuProductParameterRepository;
        _manuCirculationRepository = manuCirculationRepository;
        _equEquipmentRepository = equEquipmentRepository;
        _procProcedureRepository = procProcedureRepository;
        _procMaterialRepository = procMaterialRepository;
        _inteWorkCenterRepository = inteWorkCenterRepository;
        _procParameterRepository = procParameterRepository;
        _procResourceRepository = procResourceRepository;
        _planWorkOrderRepository = planWorkOrderRepository;
        _manuSfcProduceRepository = manuSfcProduceRepository;
        _procProcessRouteDetailLinkRepository = procProcessRouteDetailLinkRepository;
    }

    /// <summary>
    /// 获取条码基本信息
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    public async Task<IEnumerable<GetSFCInfoViewDto>> GetSFCInfoAsync(GetSFCInfoQueryDto queryDto)
    {
        List<GetSFCInfoViewDto> result = new();

        if (queryDto.SFC?.Any() != true) throw new CustomerValidationException(nameof(ErrorCode.MES19003));

        //获取条码绑定信息
        var manuSfcCirculationEntities = await _manuCirculationRepository.GetSfcMoudulesAsync(new ManuSfcCirculationBySfcsQuery() { SiteId = 123456, CirculationBarCodes = queryDto.SFC });

        //获取条码履历信息
        var manuSfcStepEntities = await _manuSfcStepRepository.GetManuSfcStepEntitiesAsync(new() { SiteId = 123456, SFCs = queryDto.SFC });

        //根据条码获取设备采集参数
        var manuSfcParameterEntities = await _manuProductParameterRepository.GetManuProductParameterAsync(new() { SiteId = 123456, SFCs = queryDto.SFC });

        #region 基础信息

        var equipmentIds = new List<long>();
        equipmentIds.AddRange(manuSfcCirculationEntities.Select(a => a.EquipmentId.GetValueOrDefault()));
        equipmentIds.AddRange(manuSfcStepEntities.Select(a => a.EquipmentId.GetValueOrDefault()));
        equipmentIds.AddRange(manuSfcParameterEntities.Select(a => a.EquipmentId));
        var equipmentEntities = await _equEquipmentRepository.GetByIdsAsync(equipmentIds.ToArray());

        var procdureIds = new List<long>();
        procdureIds.AddRange(manuSfcCirculationEntities.Select(a => a.ProcedureId));
        procdureIds.AddRange(manuSfcStepEntities.Select(a => a.ProcedureId.GetValueOrDefault()));
        procdureIds.AddRange(manuSfcParameterEntities.Select(a => a.ProcedureId.GetValueOrDefault()));
        var procdureEntities = await _procProcedureRepository.GetByIdsAsync(procdureIds.ToArray());

        var resourceIds = new List<long>();
        resourceIds.AddRange(manuSfcCirculationEntities.Select(a => a.ResourceId.GetValueOrDefault()));
        resourceIds.AddRange(manuSfcStepEntities.Select(a => a.ResourceId.GetValueOrDefault()));
        resourceIds.AddRange(manuSfcParameterEntities.Select(a => a.ResourceId.GetValueOrDefault()));
        var resourceEntities = await _procResourceRepository.GetByIdsAsync(new() { IdsArr = resourceIds.ToArray(), Status = 1 });

        var productIds = new List<long>();
        productIds.AddRange(manuSfcCirculationEntities.Select(a => a.ProductId));
        productIds.AddRange(manuSfcStepEntities.Select(a => a.ProductId));
        productIds.AddRange(manuSfcParameterEntities.Select(a => a.ProductId.GetValueOrDefault()));
        var productEntities = await _procMaterialRepository.GetByIdsAsync(productIds);

        var workOrderIds = new List<long>();
        workOrderIds.AddRange(manuSfcCirculationEntities.Select(a => a.WorkOrderId));
        workOrderIds.AddRange(manuSfcStepEntities.Select(a => a.WorkOrderId));
        workOrderIds.AddRange(manuSfcParameterEntities.Select(a => a.WorkOrderId.GetValueOrDefault()));
        var workOrderEntities = await _planWorkOrderRepository.GetByIdsAsync(workOrderIds.ToArray());

        var paramIds = manuSfcParameterEntities.Select(a => a.ParameterId);
        var paramEntities = await _procParameterRepository.GetByIdsAsync(paramIds);

        #endregion

        List<ProcductTraceViewDto> tracelist = new();
        foreach (var item in manuSfcCirculationEntities)
        {
            var equipment = equipmentEntities.FirstOrDefault(a => a.Id == item.EquipmentId);

            ProcductTraceViewDto trace = new()
            {
                SFC = item.SFC,
                Level = 0,
                CirculationBarCode = item.CirculationBarCode,
                EquipmentCode = equipment?.EquipmentCode
            };

            tracelist.Add(trace);
        }

        List<SFCStepViewDto> steplist = new();
        foreach (var item in manuSfcStepEntities)
        {
            var procdure = procdureEntities.FirstOrDefault(a => a.Id == item.ProcedureId);
            var resource = resourceEntities.FirstOrDefault(a => a.Id == item.ResourceId);
            var product = productEntities.FirstOrDefault(a => a.Id == item.ProductId);
            var workOrder = workOrderEntities.FirstOrDefault(a => a.Id == item.WorkOrderId);

            steplist.Add(new()
            {
                CreateOn = item.CreatedOn,
                Passed = item.Passed,
                ProcedureCode = procdure?.Code,
                ProcedureName = procdure?.Name,
                ProcedureType = procdure?.Type,
                ResourceName = resource?.ResName,
                ProductName = product?.MaterialName,
                WorkOrderType = workOrder?.Type
            });

        }

        List<ProductParameterViewDto> paramlist = new();
        foreach (var item in manuSfcParameterEntities)
        {
            var equipment = equipmentEntities.FirstOrDefault(a => a.Id == item.EquipmentId);
            var param = paramEntities.FirstOrDefault(a => a.Id == item.ParameterId);
            var procdure = procdureEntities.FirstOrDefault(a => a.Id == item.ProcedureId);

            paramlist.Add(new()
            {
                EquipmentName = equipment?.EquipmentName,
                JudgmentResult = item?.JudgmentResult,
                LocalTime = item?.LocalTime,
                ParameterCode = param?.ParameterCode,
                ParameterName = param?.ParameterName,
                ParameterValue = item?.ParamValue,
                ProcedureCode = procdure?.Code,
                ProcedureName = procdure?.Name,
                StandardLowerLimit = item?.StandardLowerLimit,
                StandardUpperLimit = item?.StandardUpperLimit
            });
        }

        foreach (var item in queryDto.SFC)
        {
            var trace = tracelist.Where(a => a.CirculationBarCode == item);

            GetSFCInfoViewDto view = new GetSFCInfoViewDto()
            {
                SFC = item,
                ProductTrace = trace,
                SfcStep = steplist,
                ProductParameter = paramlist
            };
            result.Add(view);
        }

        return result;
    }

    /// <summary>
    /// 更新当前条码在制状态
    /// </summary>
    /// <param name="isNext"></param>
    /// <param name="SFC"></param>
    /// <returns></returns>
    public async Task UpdateManuSFCProduceStatus(string isNext, string SFC)
    {
        var manuSfcProcdure = await _manuSfcProduceRepository.GetBySFCAsync(new() { SiteId=123456, Sfc = SFC })
            ?? throw new CustomerValidationException(nameof(ErrorCode.MES16600));

        var nextProcdure = 0;
        ////判断是否需要调整在制到下个工序
        //if (isNext == "1")
        //{
        //    var processRoute = await _procProcessRouteDetailLinkRepository.GetNextProcessRouteDetailLinkAsync(new ProcProcessRouteDetailLinkQuery
        //    {
        //        ProcessRouteId = manuSfcProcdure.ProcessRouteId,
        //        ProcedureId = manuSfcProcdure.ProcedureId
        //    });


        //}

        //更新在制状态

        await _manuSfcProduceRepository.UpdateProcedureAndStatusRangeAsync(new UpdateProcedureAndStatusCommand
        {
            SiteId = 123456,
            ResourceId = null,
            Sfcs = new string[1] { SFC },
            //ProcedureId = isNext == "1" ? nextProcdure : manuSfcProcdure.ProcedureId,
            ProcedureId = manuSfcProcdure.ProcedureId,
            Status = Core.Enums.SfcProduceStatusEnum.lineUp,
            UpdatedOn = HymsonClock.Now(),
            UserId = _currentUser.UserName
        });

    }
}