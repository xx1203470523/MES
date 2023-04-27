using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuCommon;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuInStation
{
    /// <summary>
    /// 进站
    /// </summary>
    public class ManuInStationService : IManuInStationService
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
        /// 仓储接口（条码信息）
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        /// <summary>
        /// 仓储接口（条码生产信息）
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 仓储接口（工序维护）
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="manuCommonService"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="procProcedureRepository"></param>
        public ManuInStationService(ICurrentUser currentUser, ICurrentSite currentSite,
            IManuCommonService manuCommonService,
            IManuSfcRepository manuSfcRepository,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IProcProcedureRepository procProcedureRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuCommonService = manuCommonService;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _procProcedureRepository = procProcedureRepository;
        }


        /// <summary>
        /// 执行（进站）
        /// </summary>
        /// <param name="sfcProduceEntity"></param>
        /// <returns></returns>
        public async Task<int> InStationAsync(ManuSfcProduceEntity sfcProduceEntity)
        {
            var rows = 0;

            // 更新状态，将条码由"排队"改为"活动"
            sfcProduceEntity.Status = SfcProduceStatusEnum.Activity;
            sfcProduceEntity.UpdatedBy = _currentUser.UserName;
            sfcProduceEntity.UpdatedOn = HymsonClock.Now();

            // 获取生产工单（附带工单状态校验）
            _ = await _manuCommonService.GetProduceWorkOrderByIdAsync(sfcProduceEntity.WorkOrderId);

            // 获取当前工序信息
            var procedureEntity = await _procProcedureRepository.GetByIdAsync(sfcProduceEntity.ProcedureId);

            // 检查是否测试工序
            if (procedureEntity.Type == ProcedureTypeEnum.Test)
            {
                // 超过复投次数，标识为NG
                if (sfcProduceEntity.RepeatedCount > procedureEntity.Cycle) throw new CustomerValidationException(nameof(ErrorCode.MES16036));
                sfcProduceEntity.RepeatedCount++;
            }

            // 修改条码使用状态为"已使用"
            rows += await _manuSfcRepository.UpdateSfcIsUsedAsync(new ManuSfcUpdateIsUsedCommand
            {
                Sfcs = new string[] { sfcProduceEntity.SFC },
                UserId = sfcProduceEntity.UpdatedBy,
                UpdatedOn = sfcProduceEntity.UpdatedOn,
                IsUsed = YesOrNoEnum.Yes
            });

            // 更改状态
            rows += await _manuSfcProduceRepository.UpdateAsync(sfcProduceEntity);

            return rows;
        }

    }
}
