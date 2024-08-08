using Hymson.Utils.Tools;

namespace Hymson.MES.BackgroundServices.Stator.Services
{
    /// <summary>
    /// 服务
    /// </summary>
    public class OP070Service : IOP070Service
    {
        /// <summary>
        /// 日志接口
        /// </summary>
        private readonly ILogger<OP070Service> _logger;

        /// <summary>
        /// 仓储接口（工序）
        /// </summary>
        private readonly IOPRepository<OP070> _opRepository;

        /// <summary>
        /// 服务接口（基础）
        /// </summary>
        public readonly IBaseService _baseService;

        /// <summary>
        /// 服务接口（水位）
        /// </summary>
        public readonly IWaterMarkService _waterMarkService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="opRepository"></param>
        /// <param name="baseService"></param>
        /// <param name="waterMarkService"></param>
        public OP070Service(ILogger<OP070Service> logger,
            IOPRepository<OP070> opRepository,
            IBaseService baseService,
            IWaterMarkService waterMarkService)
        {
            _logger = logger;
            _opRepository = opRepository;
            _baseService = baseService;
            _waterMarkService = waterMarkService;
        }

        /// <summary>
        /// 执行统计
        /// </summary>
        /// <param name="limitCount"></param>
        /// <returns></returns>
        public async Task<int> ExecuteAsync(int limitCount)
        {
            var producreCode = $"{typeof(OP070).Name}";
            var buzKey_1 = $"{StatorConst.BUZ_KEY_PREFIX}-{producreCode}-1";
            var buzKey_2 = $"{StatorConst.BUZ_KEY_PREFIX}-{producreCode}-2";
            var buzKey_3 = $"{StatorConst.BUZ_KEY_PREFIX}-{producreCode}-3";

            var waterMarkId_1 = await _waterMarkService.GetWaterMarkAsync(buzKey_1);
            var waterMarkId_2 = await _waterMarkService.GetWaterMarkAsync(buzKey_2);
            var waterMarkId_3 = await _waterMarkService.GetWaterMarkAsync(buzKey_3);

            // 根据水位读取数据
            List<Task<IEnumerable<OP070>>> readTasks = new()
            {
                _opRepository.GetListByStartWaterMarkIdAsync(new EntityByWaterMarkQuery
                {
                    StartWaterMarkId = waterMarkId_1,
                    Rows = limitCount
                }, "op070_1"),
                _opRepository.GetListByStartWaterMarkIdAsync(new EntityByWaterMarkQuery
                {
                    StartWaterMarkId = waterMarkId_2,
                    Rows = limitCount
                }, "op070_2"),
                _opRepository.GetListByStartWaterMarkIdAsync(new EntityByWaterMarkQuery
                {
                    StartWaterMarkId = waterMarkId_3,
                    Rows = limitCount
                }, "op070_3")
            };

            var opArray = await Task.WhenAll(readTasks);
            var entities = opArray.SelectMany(s => s);
            if (entities == null || !entities.Any())
            {
                _logger.LogDebug($"【{producreCode}】没有要拉取的数据 -> {producreCode}");
                return 0;
            }

            // 最大序列号
            var maxIndex_1 = 0L;
            var maxIndex_2 = 0L;
            var maxIndex_3 = 0L;

            if (opArray[0] != null && opArray[0].Any()) maxIndex_1 = opArray[0].Max(m => m.index);
            if (opArray[1] != null && opArray[1].Any()) maxIndex_2 = opArray[1].Max(m => m.index);
            if (opArray[2] != null && opArray[2].Any()) maxIndex_3 = opArray[2].Max(m => m.index);

            // 先定位条码位置
            var barCodes = entities.Select(s => s.Barcode);

            // 获取转换数据
            var summaryBo = await _baseService.ConvertDataAsync(entities, barCodes);

            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();

            // 保存基础数据
            rows += await _baseService.SaveBaseDataAsync(summaryBo);

            if (maxIndex_1 > 0) rows += await _waterMarkService.RecordWaterMarkAsync(buzKey_1, maxIndex_1);
            if (maxIndex_2 > 0) rows += await _waterMarkService.RecordWaterMarkAsync(buzKey_2, maxIndex_2);
            if (maxIndex_3 > 0) rows += await _waterMarkService.RecordWaterMarkAsync(buzKey_3, maxIndex_3);

            trans.Complete();
            return rows;
        }

    }
}
