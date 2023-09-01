using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.CoreServices.Services.Manufacture
{
    /// <summary>
    /// 降级品继承
    /// @author Czhipu
    /// @date 2023-08-30
    /// </summary>
    public class ManuDegradedProductExtendService : IManuDegradedProductExtendService
    {
        /// <summary>
        /// 仓储接口（降级录入）
        /// </summary>
        private readonly IManuDowngradingRepository _manuDowngradingRepository;

        /// <summary>
        /// 仓储接口（降级品录入记录）
        /// </summary>
        private readonly IManuDowngradingRecordRepository _manuDowngradingRecordRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="manuDowngradingRepository"></param>
        /// <param name="manuDowngradingRecordRepository"></param>
        public ManuDegradedProductExtendService(IManuDowngradingRepository manuDowngradingRepository,
            IManuDowngradingRecordRepository manuDowngradingRecordRepository)
        {
            _manuDowngradingRepository = manuDowngradingRepository;
            _manuDowngradingRecordRepository = manuDowngradingRecordRepository;
        }

        /// <summary>
        /// 创建降级品记录
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public async Task<int> CreateManuDowngradingsAsync(DegradedProductExtendBo bo)
        {
            if (bo == null) return 0;

            var downgradingEntities = await GetManuDownGradingsAsync(bo);
            if (downgradingEntities == null || downgradingEntities.Any() == false) return 0;

            return await CreateManuDowngradingsByConsumesAsync(bo, downgradingEntities);
        }

        /// <summary>
        /// 批量查询条码对应的降级录入数据
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuDowngradingEntity>> GetManuDownGradingsAsync(DegradedProductExtendBo bo)
        {
            return await _manuDowngradingRepository.GetBySFCsAsync(new ManuDowngradingBySFCsQuery
            {
                SiteId = bo.SiteId,
                SFCs = bo.KeyValues.Select(s => s.BarCode)
            });
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentEntities"></param>
        /// <returns></returns>
        public async Task<int> CreateManuDowngradingsByConsumesAsync(DegradedProductExtendBo bo, IEnumerable<ManuDowngradingEntity>? downgradingEntities)
        {
            if (bo == null || downgradingEntities == null || downgradingEntities.Any() == false) return 0;

            // 更新时间
            var updatedOn = HymsonClock.Now();

            List<ManuDowngradingEntity> manuDowngradingEntities = new();
            List<ManuDowngradingRecordEntity> manuDowngradingRecordEntities = new();
            foreach (var entity in downgradingEntities)
            {
                var keyValueBo = bo.KeyValues.FirstOrDefault(f => f.BarCode == entity.SFC);
                if (keyValueBo == null) continue;

                manuDowngradingEntities.Add(new ManuDowngradingEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SFC = keyValueBo.SFC,
                    Grade = entity.Grade,
                    SiteId = entity.SiteId,
                    CreatedBy = bo.UserName ?? ""
                });

                manuDowngradingRecordEntities.Add(new ManuDowngradingRecordEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SFC = keyValueBo.SFC,
                    Grade = entity.Grade,
                    SiteId = entity.SiteId,
                    IsCancellation = Core.Enums.Manufacture.ManuDowngradingRecordTypeEnum.Entry,
                    CreatedBy = bo.UserName ?? ""
                });
            }

            // 调用外层加事务
            var rows = 0;
            rows += await _manuDowngradingRepository.InsertsAsync(manuDowngradingEntities);
            rows += await _manuDowngradingRecordRepository.InsertsAsync(manuDowngradingRecordEntities);
            return rows;
        }

    }
}
