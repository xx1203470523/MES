using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Common;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.Snowflake;
using Hymson.Utils.Tools;
using Hymson.WaterMark;
using Microsoft.Extensions.Logging;

namespace Hymson.MES.BackgroundServices.Stator.Services
{
    /// <summary>
    /// 服务
    /// </summary>
    public class OP010Service : IOP010Service
    {
        /// <summary>
        /// 日志接口
        /// </summary>
        private readonly ILogger<OP010Service> _logger;

        /// <summary>
        /// 仓储接口（工序）
        /// </summary>
        private readonly IOPRepository<OP010> _opRepository;

        /// <summary>
        /// 服务接口（基础）
        /// </summary>
        public readonly IBaseService _baseService;

        /// <summary>
        /// 服务接口（水位）
        /// </summary>
        public readonly IWaterMarkService _waterMarkService;

        /// <summary>
        /// 仓储接口（系统配置）
        /// </summary>
        private readonly ISysConfigRepository _sysConfigRepository;

        /// <summary>
        /// 仓储接口（工作中心）
        /// </summary>
        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;

        /// <summary>
        /// 仓储接口（生产工单）
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        /// 仓储接口（工序维护）
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;

        /// <summary>
        /// 仓储接口（条码）
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        /// <summary>
        /// 仓储接口（条码信息）
        /// </summary>
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;

        /// <summary>
        /// 仓储接口（条码步骤）
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

        /// <summary>
        /// 仓储接口（条码流转）
        /// </summary>
        private readonly IManuSfcCirculationRepository _manuSfcCirculationRepository;

        /// <summary>
        /// 仓储接口（产品不良录入）
        /// </summary>
        private readonly IManuProductBadRecordRepository _manuProductBadRecordRepository;

        /// <summary>
        /// 仓储接口（产品NG记录表）
        /// </summary>
        private readonly IManuProductNgRecordRepository _manuProductNgRecordRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="opRepository"></param>
        /// <param name="baseService"></param>
        /// <param name="waterMarkService"></param>
        /// <param name="sysConfigRepository"></param>
        /// <param name="inteWorkCenterRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="manuSfcInfoRepository"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="manuSfcCirculationRepository"></param>
        /// <param name="manuProductBadRecordRepository"></param>
        /// <param name="manuProductNgRecordRepository"></param>
        public OP010Service(ILogger<OP010Service> logger,
            IOPRepository<OP010> opRepository,
            IBaseService baseService,
            IWaterMarkService waterMarkService,
            ISysConfigRepository sysConfigRepository,
            IInteWorkCenterRepository inteWorkCenterRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IProcProcedureRepository procProcedureRepository,
            IManuSfcRepository manuSfcRepository,
            IManuSfcInfoRepository manuSfcInfoRepository,
            IManuSfcStepRepository manuSfcStepRepository,
            IManuSfcCirculationRepository manuSfcCirculationRepository,
            IManuProductBadRecordRepository manuProductBadRecordRepository,
            IManuProductNgRecordRepository manuProductNgRecordRepository)
        {
            _logger = logger;
            _opRepository = opRepository;
            _baseService = baseService;
            _waterMarkService = waterMarkService;
            _procProcedureRepository = procProcedureRepository;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _manuSfcCirculationRepository = manuSfcCirculationRepository;
            _manuProductBadRecordRepository = manuProductBadRecordRepository;
            _manuProductNgRecordRepository = manuProductNgRecordRepository;
        }

        /// <summary>
        /// 执行统计
        /// </summary>
        /// <param name="limitCount"></param>
        /// <returns></returns>
        public async Task<int> ExecuteAsync(int limitCount)
        {
            var producreCode = $"{typeof(OP010).Name}";
            var buzKey = $"Stator-{producreCode}";
            var waterMarkId = await _waterMarkService.GetWaterMarkAsync(buzKey);

            // 根据水位读取数据
            var entities = await _opRepository.GetListByStartWaterMarkIdAsync(new EntityByWaterMarkQuery
            {
                StartWaterMarkId = waterMarkId,
                Rows = limitCount
            });

            // 获取转换数据
            var summaryBo = await _baseService.ConvertDataAsync(entities);

            // 保存数据
            return await _baseService.SaveDataAsync(buzKey, entities.Max(m => m.index), summaryBo);
        }

    }
}
