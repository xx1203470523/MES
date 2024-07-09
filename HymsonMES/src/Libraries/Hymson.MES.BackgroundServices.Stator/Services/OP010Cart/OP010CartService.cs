using Hymson.MES.BackgroundServices.Stator.Model;
using Hymson.MES.BackgroundServices.Stator.Repositories;
using Hymson.MES.BackgroundServices.Stator.Services;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.Utils.Tools;
using Hymson.WaterMark;

namespace Hymson.MES.BackgroundServices.Stator
{
    /// <summary>
    /// 服务
    /// </summary>
    public class OP010CartService : IOP010CartService
    {
        /// <summary>
        /// 服务接口（水位）
        /// </summary>
        public readonly IWaterMarkService _waterMarkService;

        /// <summary>
        /// 仓储接口（工序）
        /// </summary>
        private readonly IOPRepository<OP010Cart> _opRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="waterMarkService"></param>
        /// <param name="opRepository"></param>
        public OP010CartService(IWaterMarkService waterMarkService,
            IOPRepository<OP010Cart> opRepository)
        {
            _waterMarkService = waterMarkService;
            _opRepository = opRepository;
        }

        /// <summary>
        /// 执行统计
        /// </summary>
        /// <param name="limitCount"></param>
        /// <returns></returns>
        public async Task ExecuteAsync(int limitCount = 1000)
        {
            var businessKey = $"Stator-{typeof(OP010Cart).Name}";
            var waterMarkId = await _waterMarkService.GetWaterMarkAsync(businessKey);

            // 获取步骤表数据
            var opList = await _opRepository.GetListByStartWaterMarkIdAsync(new EntityByWaterMarkQuery
            {
                StartWaterMarkId = waterMarkId,
                Rows = limitCount
            });
            if (opList == null || !opList.Any()) return;

            // TODO: 业务逻辑


            using var trans = TransactionHelper.GetTransactionScope();

            // 保存数据

            // 更新水位
            await _waterMarkService.RecordWaterMarkAsync(businessKey, opList.Max(x => x.index));
            trans.Complete();
        }

    }
}
