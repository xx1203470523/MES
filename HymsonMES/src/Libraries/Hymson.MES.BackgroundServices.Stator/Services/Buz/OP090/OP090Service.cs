﻿using Hymson.Utils;
using System.Data;

namespace Hymson.MES.BackgroundServices.Stator.Services
{
    /// <summary>
    /// 服务
    /// </summary>
    public partial class OP090Service : IOP090Service
    {
        /// <summary>
        /// 日志接口
        /// </summary>
        private readonly ILogger<OP090Service> _logger;

        /// <summary>
        /// 仓储接口（工序）
        /// </summary>
        private readonly IOPRepository<OP090> _opRepository;

        /// <summary>
        /// 服务接口（基础）
        /// </summary>
        public readonly IMainService _mainService;

        /// <summary>
        /// 服务接口（水位）
        /// </summary>
        public readonly IWaterMarkService _waterMarkService;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="opRepository"></param>
        /// <param name="mainService"></param>
        /// <param name="waterMarkService"></param>
        public OP090Service(ILogger<OP090Service> logger,
            IOPRepository<OP090> opRepository,
            IMainService mainService,
            IWaterMarkService waterMarkService)
        {
            _logger = logger;
            _opRepository = opRepository;
            _mainService = mainService;
            _waterMarkService = waterMarkService;
        }


        /// <summary>
        /// 执行统计
        /// </summary>
        /// <param name="limitCount"></param>
        /// <returns></returns>
        public async Task<int> ExecuteAsync(int limitCount)
        {
            var producreCode = $"{typeof(OP090).Name}";
            var buzKey = $"{StatorConst.BUZ_KEY_PREFIX}-{producreCode}";
            var waterMarkId = await _waterMarkService.GetWaterMarkAsync(buzKey);
            
            _logger.LogInformation($"OP090 水位线；时间： {waterMarkId.ToString()}");

            // 根据水位读取数据
            var dataTable = await _opRepository.GetDataTableByStartWaterMarkIdAsync(new EntityByWaterMarkQuery
            {
                StartWaterMarkId = waterMarkId,
                Rows = limitCount
            });
            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                _logger.LogDebug($"【{producreCode}】没有要拉取的数据！");
                return 0;
            }

            // 获取转换数据（基础数据）
            var summaryBo = await _mainService.ConvertDataTableInnerAsync(dataTable, producreCode);

            // 保存数据
            var waterLevel = dataTable.AsEnumerable().Select(s => s["index"].ParseToLong());
            return await _mainService.SaveBaseDataWithCommitAsync(buzKey, waterLevel.Max(m => m), summaryBo);
        }

    }
}
