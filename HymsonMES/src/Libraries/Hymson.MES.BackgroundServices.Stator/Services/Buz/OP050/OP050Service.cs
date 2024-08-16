using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Extension;
using Hymson.Snowflake;
using Hymson.Utils;
using System.Data;

namespace Hymson.MES.BackgroundServices.Stator.Services
{
    /// <summary>
    /// 服务
    /// </summary>
    public partial class OP050Service : IOP050Service
    {
        /// <summary>
        /// 日志接口
        /// </summary>
        private readonly ILogger<OP050Service> _logger;

        /// <summary>
        /// 仓储接口（工序）
        /// </summary>
        private readonly IOPRepository<OP050> _opRepository;

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
        public OP050Service(ILogger<OP050Service> logger,
            IOPRepository<OP050> opRepository,
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
            var producreCode = $"{typeof(OP050).Name}";
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
    public partial class OP050Service
    {
        /// <summary>
        /// 参数编码集合
        /// </summary>
        private static readonly List<string> _parameterCodes = new()
        {
            "pressCount",
            "CrimpingPosition01",
            "CrimpingPressDistance01",
            "CrimpingPressLoad01",
            "CrimpingPosition02",
            "CrimpingPressDistance02",
            "CrimpingPressLoad02",
            "CrimpingPosition03",
            "CrimpingPressDistance03",
            "CrimpingPressLoad03",
            "CrimpingPosition04",
            "CrimpingPressDistance04",
            "CrimpingPressLoad04",
            "CrimpingPosition05",
            "CrimpingPressDistance05",
            "CrimpingPressLoad05",
            "CrimpingPosition06",
            "CrimpingPressDistance06",
            "CrimpingPressLoad06",
            "CrimpingPosition07",
            "CrimpingPressDistance07",
            "CrimpingPressLoad07",
            "CrimpingPosition08",
            "CrimpingPressDistance08",
            "CrimpingPressLoad08",
            "CrimpingPosition09",
            "CrimpingPressDistance09",
            "CrimpingPressLoad09",
            "CrimpingPosition10",
            "CrimpingPressDistance10",
            "CrimpingPressLoad10",
            /*
            "CrimpingPosition11",
            "CrimpingPressDistance11",
            "CrimpingPressLoad11",
            "CrimpingPosition12",
            "CrimpingPressDistance12",
            "CrimpingPressLoad12",
            "CrimpingPosition13",
            "CrimpingPressDistance13",
            "CrimpingPressLoad13",
            "CrimpingPosition14",
            "CrimpingPressDistance14",
            "CrimpingPressLoad14",
            "CrimpingPosition15",
            "CrimpingPressDistance15",
            "CrimpingPressLoad15"
            */
        };

    }
}
