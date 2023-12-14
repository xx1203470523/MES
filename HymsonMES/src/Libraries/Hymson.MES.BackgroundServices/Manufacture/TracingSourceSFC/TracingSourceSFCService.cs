using Hymson.MES.Core.Constants.Manufacture;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.Utils.Tools;
using Hymson.WaterMark;

namespace Hymson.MES.BackgroundServices.Manufacture
{
    /// <summary>
    /// 
    /// </summary>
    public class TracingSourceSFCService : ITracingSourceSFCService
    {
        /// <summary>
        /// 服务接口（水位）
        /// </summary>
        public readonly IWaterMarkService _waterMarkService;

        /// <summary>
        /// 仓储接口（条码流转）
        /// </summary>
        private readonly IManuSfcCirculationRepository _manuSfcCirculationRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="waterMarkService"></param>
        /// <param name="manuSfcCirculationRepository"></param>
        public TracingSourceSFCService(IWaterMarkService waterMarkService,
            IManuSfcCirculationRepository manuSfcCirculationRepository)
        {
            _waterMarkService = waterMarkService;
            _manuSfcCirculationRepository = manuSfcCirculationRepository;
        }

        /// <summary>
        /// 执行统计
        /// </summary>
        /// <param name="limitCount"></param>
        /// <returns></returns>
        public async Task ExecuteAsync(int limitCount = 1000)
        {
            var waterMarkId = await _waterMarkService.GetWaterMarkAsync(BusinessKey.TracingSourceSFC);

            // 获取流转表数据
            var manuSfcCirculationList = await _manuSfcCirculationRepository.GetListByStartWaterMarkIdAsync(new EntityByWaterMarkQuery
            {
                StartWaterMarkId = waterMarkId,
                Rows = limitCount
            });
            if (manuSfcCirculationList == null || !manuSfcCirculationList.Any()) return;

            var user = $"{BusinessKey.TracingSourceSFC}作业";

            // 流转前后的条码各记录一条记录
            List<SingleSFCBo> sfcList = new();
            sfcList.AddRange(manuSfcCirculationList.Select(s => new SingleSFCBo { SiteId = s.SiteId, SFC = s.SFC }));
            sfcList.AddRange(manuSfcCirculationList.Select(s => new SingleSFCBo { SiteId = s.SiteId, SFC = s.CirculationBarCode }));

            // 相同条码的数据只记录一条记录
            var sfcDsitinctList = sfcList.DistinctBy(x => x);
            foreach (var sfcItem in sfcDsitinctList)
            {
                // 读取条码之前的流转组装记录

                var circulations = manuSfcCirculationList.Where(x => x.SiteId == sfcItem.SiteId && (x.SFC == sfcItem.SFC || x.CirculationBarCode == sfcItem.SFC));
                foreach (var item in circulations)
                {
                    switch (item.CirculationType)
                    {
                        case SfcCirculationTypeEnum.Split:
                            break;
                        case SfcCirculationTypeEnum.Merge:
                            break;
                        case SfcCirculationTypeEnum.Change:
                            break;
                        case SfcCirculationTypeEnum.Consume:
                            break;
                        case SfcCirculationTypeEnum.Disassembly:
                            break;
                        case SfcCirculationTypeEnum.ModuleAdd:
                            break;
                        case SfcCirculationTypeEnum.ModuleReplace:
                            break;
                        default:
                            break;
                    }
                }
            }

            using var trans = TransactionHelper.GetTransactionScope();

            // TODO 业务待实现

            // 根据条码递归读取所有父级条码

            // 根据条码递归读取所有子级条码

            // 生成链路JSON数据

            // 更新水位
            await _waterMarkService.RecordWaterMarkAsync(BusinessKey.TracingSourceSFC, manuSfcCirculationList.Max(x => x.Id));
            trans.Complete();

        }

    }
}
