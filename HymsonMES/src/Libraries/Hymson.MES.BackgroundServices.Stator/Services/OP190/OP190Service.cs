namespace Hymson.MES.BackgroundServices.Stator.Services
{
    /// <summary>
    /// 服务
    /// </summary>
    public partial class OP190Service : IOP190Service
    {
        /// <summary>
        /// 日志接口
        /// </summary>
        private readonly ILogger<OP190Service> _logger;

        /// <summary>
        /// 仓储接口（工序）
        /// </summary>
        private readonly IOPRepository<OP190> _opRepository;

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
        public OP190Service(ILogger<OP190Service> logger,
            IOPRepository<OP190> opRepository,
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
            var producreCode = $"{typeof(OP190).Name}";
            var buzKey = $"{StatorConst.BUZ_KEY_PREFIX}-{producreCode}";
            var waterMarkId = await _waterMarkService.GetWaterMarkAsync(buzKey);

            // 根据水位读取数据
            var entities = await _opRepository.GetListByStartWaterMarkIdAsync(new EntityByWaterMarkQuery
            {
                StartWaterMarkId = waterMarkId,
                Rows = limitCount
            });
            if (entities == null || !entities.Any())
            {
                _logger.LogDebug($"【{producreCode}】没有要拉取的数据！");
                return 0;
            }

            // 先定位条码位置
            var barCodes = entities.Select(s => s.Barcode);

            // 获取转换数据（基础数据）
            var summaryBo = await _mainService.ConvertDataListAsync(entities, barCodes);

            // 保存数据
            return await _mainService.SaveBaseDataWithCommitAsync(buzKey, entities.Max(m => m.index), summaryBo);
        }

    }
}
