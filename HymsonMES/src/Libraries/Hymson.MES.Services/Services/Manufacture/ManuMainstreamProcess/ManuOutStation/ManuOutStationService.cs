﻿using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto.ManuOutStation;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuCommon;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.OutStation;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuOutStation
{
    /// <summary>
    /// 出站
    /// </summary>
    public class ManuOutStationService : IManuOutStationService
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
        /// 服务接口（生产通用）
        /// </summary>
        private readonly IManuCommonService _manuCommonService;

        /// <summary>
        /// 仓储接口（条码步骤）
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

        /// <summary>
        /// 仓储接口（条码信息）
        /// </summary>
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;

        /// <summary>
        /// 仓储接口（条码生产信息）
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;



        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="manuCommonService"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="manuSfcInfoRepository"></param>
        /// <param name="manuSfcProduceRepository"></param>
        public ManuOutStationService(ICurrentUser currentUser, ICurrentSite currentSite,
            IManuCommonService manuCommonService,
            IManuSfcStepRepository manuSfcStepRepository,
            IManuSfcInfoRepository manuSfcInfoRepository,
            IManuSfcProduceRepository manuSfcProduceRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuCommonService = manuCommonService;
            _manuSfcStepRepository = manuSfcStepRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
        }


        /// <summary>
        /// 出站
        /// </summary>
        /// <param name="outDto"></param>
        public async Task ExecuteAsync(SFCOutStationDto outDto)
        {
            // 获取生产条码信息（附带条码合法性校验）
            var sfcProduceEntity = await _manuCommonService.GetProduceSPCWithCheckAsync(outDto.SFC);

            // 获取生产工单
            var workOrderEntity = await _manuCommonService.GetProduceWorkOrderByIdAsync(sfcProduceEntity.WorkOrderId);

            // 更新时间
            sfcProduceEntity.UpdatedBy = _currentUser.UserName;
            sfcProduceEntity.UpdatedOn = HymsonClock.Now();

            // 初始化步骤
            var sfcStep = new ManuSfcStepEntity
            {
                SiteId = _currentSite.SiteId ?? 0,
                SFC = sfcProduceEntity.SFC,
                ProductId = sfcProduceEntity.ProductId,
                WorkOrderId = sfcProduceEntity.WorkOrderId,
                WorkCenterId = sfcProduceEntity.WorkCenterId,
                ProductBOMId = sfcProduceEntity.ProductBOMId,
                Qty = sfcProduceEntity.Qty,
                EquipmentId = sfcProduceEntity.EquipmentId,
                ResourceId = sfcProduceEntity.ResourceId,
                CreatedBy = sfcProduceEntity.UpdatedBy,
                CreatedOn = sfcProduceEntity.UpdatedOn.Value,
                UpdatedBy = sfcProduceEntity.UpdatedBy,
                UpdatedOn = sfcProduceEntity.UpdatedOn.Value,
            };


            // TODO 是否合格的校验？？
            var result = true;

            if (result)
            {
                var sfcInfo = await _manuSfcInfoRepository.GetBySPCAsync(sfcProduceEntity.SFC);

                // 合格品出站
                // 获取下一个工序（如果没有了，就表示完工）
                var nextProcedure = await _manuCommonService.GetNextProcedureAsync(workOrderEntity.ProcessRouteId, sfcProduceEntity.ProcedureId);
                if (nextProcedure == null)
                {
                    // 完工

                    // 删除 manu_sfc_produce
                    await _manuSfcProduceRepository.DeletePhysicalAsync(sfcProduceEntity.SFC);

                    // TODO 删除 manu_sfc_produce_business

                    // 插入 manu_sfc_step 状态为 完成
                    sfcStep.Type = ManuSfcStepTypeEnum.Complete;    // TODO 这里的状态？？
                    sfcStep.Status = SfcProduceStatusEnum.Complete;  // TODO 这里的状态？？
                    await _manuSfcStepRepository.InsertAsync(sfcStep);

                    // TODO manu_sfc_info 修改为 完成或者入库
                    sfcInfo.Status = SfcStatusEnum.Complete;
                    await _manuSfcInfoRepository.UpdateAsync(sfcInfo);
                }
                else
                {
                    // 未完工

                    // 修改 manu_sfc_produce 为排队, 工序修改为下一工序的id
                    sfcProduceEntity.Status = SfcProduceStatusEnum.lineUp;
                    sfcProduceEntity.ProcedureId = nextProcedure.Id;
                    await _manuSfcProduceRepository.UpdateAsync(sfcProduceEntity);

                    // 插入 manu_sfc_step 状态为 进站
                    sfcStep.Type = ManuSfcStepTypeEnum.InStock;
                    await _manuSfcStepRepository.InsertAsync(sfcStep);
                }
            }
            else
            {
                // NG出站

                // 插入状态为出站
                sfcStep.Type = ManuSfcStepTypeEnum.OutStock;
                await _manuSfcStepRepository.InsertAsync(sfcStep);

                // RepeatedCount+1， IsSuspicious改为 true, Status修改为 活动
                sfcProduceEntity.RepeatedCount += 1;
                sfcProduceEntity.IsSuspicious = true;
                sfcProduceEntity.Status = SfcProduceStatusEnum.Activity;
                await _manuSfcProduceRepository.UpdateAsync(sfcProduceEntity);
            }
        }


    }
}
