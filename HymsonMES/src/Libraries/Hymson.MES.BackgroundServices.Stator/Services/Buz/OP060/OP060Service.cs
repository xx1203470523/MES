using Hymson.Utils;
using System.Data;

namespace Hymson.MES.BackgroundServices.Stator.Services
{
    /// <summary>
    /// 服务
    /// </summary>
    public partial class OP060Service : IOP060Service
    {
        /// <summary>
        /// 日志接口
        /// </summary>
        private readonly ILogger<OP060Service> _logger;

        /// <summary>
        /// 仓储接口（工序）
        /// </summary>
        private readonly IOPRepository<OP060> _opRepository;

        /// <summary>
        /// 服务接口（基础）
        /// </summary>
        public readonly IMainService _mainService;

        /// <summary>
        /// 服务接口（水位）
        /// </summary>
        public readonly IWaterMarkService _waterMarkService;

        /// <summary>
        /// 仓储接口（参数维护）
        /// </summary>
        private readonly IProcParameterRepository _procParameterRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="opRepository"></param>
        /// <param name="mainService"></param>
        /// <param name="waterMarkService"></param>
        /// <param name="procParameterRepository"></param>
        public OP060Service(ILogger<OP060Service> logger,
            IOPRepository<OP060> opRepository,
            IMainService mainService,
            IWaterMarkService waterMarkService,
            IProcParameterRepository procParameterRepository)
        {
            _logger = logger;
            _opRepository = opRepository;
            _mainService = mainService;
            _waterMarkService = waterMarkService;
            _procParameterRepository = procParameterRepository;
        }


        /// <summary>
        /// 执行统计
        /// </summary>
        /// <param name="limitCount"></param>
        /// <returns></returns>
        public async Task<int> ExecuteAsync(int limitCount)
        {
            var producreCode = $"{typeof(OP060).Name}";
            var buzKey = $"{StatorConst.BUZ_KEY_PREFIX}-{producreCode}";
            var waterMarkId = await _waterMarkService.GetWaterMarkAsync(buzKey);

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
            var summaryBo = await _mainService.ConvertDataTableWireAsync(dataTable, producreCode, _parameterCodes);

            // 保存数据
            var waterLevel = dataTable.AsEnumerable().Select(s => s["index"].ParseToLong());
            return await _mainService.SaveBaseDataWithCommitAsync(buzKey, waterLevel.Max(m => m), summaryBo);
        }

    }

    /// <summary>
    /// 服务
    /// </summary>
    public partial class OP060Service
    {
        /// <summary>
        /// 参数编码集合
        /// </summary>
        private static readonly List<string> _parameterCodes = new()
        {
            "JigTraverser1stWorkPosition01",
            "JigTraverser1stWorkPosition02",
            "JigTraverser1stWorkPosition03",
            "JigTraverser1stWorkPosition04",
            "JigTraverser1stWorkPosition05",
            "JigTraverser1stWorkPosition06",
            "JigTraverser1stWorkPosition07",
            "JigTraverser1stWorkPosition08",
            "JigTraverser1stWorkPosition09",
            "JigTraverser1stWorkPosition10",
            "JigTraverser1stWorkPosition11",
            "JigTraverser1stWorkPosition12",
            "JigTraverser1stWorkPosition13",
            "JigTraverser1stWorkPosition14",
            "JigTraverser1stWorkPosition15",
            "JigTraverser1stWorkPosition16",
            "JigTraverser1stWorkPosition17",
            "JigTraverser1stWorkPosition18",
            "JigTraverser2ndWorkPosition01",
            "JigTraverser2ndWorkPosition02",
            "JigTraverser2ndWorkPosition03",
            "JigTraverser2ndWorkPosition04",
            "JigTraverser2ndWorkPosition05",
            "JigTraverser2ndWorkPosition06",
            "JigTraverser2ndWorkPosition07",
            "JigTraverser2ndWorkPosition08",
            "JigTraverser2ndWorkPosition09",
            "JigTraverser2ndWorkPosition10",
            "JigTraverser2ndWorkPosition11",
            "JigTraverser2ndWorkPosition12",
            "JigTraverser2ndWorkPosition13",
            "JigTraverser2ndWorkPosition14",
            "JigTraverser2ndWorkPosition15",
            "JigTraverser2ndWorkPosition16",
            "JigTraverser2ndWorkPosition17",
            "JigTraverser2ndWorkPosition18",
            "LaserStrip1stWorkPosition01",
            "LaserStrip1stWorkPosition02",
            "LaserStrip1stWorkPosition03",
            "LaserStrip1stWorkPosition04",
            "LaserStrip1stWorkPosition05",
            "LaserStrip1stWorkPosition06",
            "LaserStrip1stWorkPosition07",
            "LaserStrip1stWorkPosition08",
            "LaserStrip1stWorkPosition09",
            "LaserStrip1stWorkPosition10",
            "LaserStrip1stWorkPosition11",
            "LaserStrip1stWorkPosition12",
            "LaserStrip1stWorkPosition13",
            "LaserStrip1stWorkPosition14",
            "LaserStrip1stWorkPosition15",
            "LaserStrip1stWorkPosition16",
            "LaserStrip1stWorkPosition17",
            "LaserStrip1stWorkPosition18",
            "LaserStrip2ndWorkPosition01",
            "LaserStrip2ndWorkPosition02",
            "LaserStrip2ndWorkPosition03",
            "LaserStrip2ndWorkPosition04",
            "LaserStrip2ndWorkPosition05",
            "LaserStrip2ndWorkPosition06",
            "LaserStrip2ndWorkPosition07",
            "LaserStrip2ndWorkPosition08",
            "LaserStrip2ndWorkPosition09",
            "LaserStrip2ndWorkPosition10",
            "LaserStrip2ndWorkPosition11",
            "LaserStrip2ndWorkPosition12",
            "LaserStrip2ndWorkPosition13",
            "LaserStrip2ndWorkPosition14",
            "LaserStrip2ndWorkPosition15",
            "LaserStrip2ndWorkPosition16",
            "LaserStrip2ndWorkPosition17",
            "LaserStrip2ndWorkPosition18"
        };

    }
}
