using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;
using static Dapper.SqlMapper;

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
        private readonly IManuDowngradingRuleRepository _manuDowngradingRuleRepository;

        /// <summary>
        /// 条码生产信息（物理删除） 仓储
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 条码信息表 仓储
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="manuDowngradingRepository"></param>
        /// <param name="manuDowngradingRecordRepository"></param>
        public ManuDegradedProductExtendService(IManuDowngradingRepository manuDowngradingRepository,
            IManuDowngradingRuleRepository manuDowngradingRuleRepository,
            IManuDowngradingRecordRepository manuDowngradingRecordRepository,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IManuSfcRepository manuSfcRepository)
        {
            _manuDowngradingRepository = manuDowngradingRepository;
            _manuDowngradingRecordRepository = manuDowngradingRecordRepository;
            _manuDowngradingRuleRepository = manuDowngradingRuleRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuSfcRepository = manuSfcRepository;
        }

        /// <summary>
        /// 创建降级品记录（里面包含读数据和写数据）
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public async Task<int> CreateManuDowngradingsAsync(DegradedProductExtendBo bo)
        {
            if (bo == null) return 0;

            var downgradingEntities = await GetManuDownGradingsAsync(bo);
            if (downgradingEntities == null || !downgradingEntities.Any()) return 0;

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
        /// 创建降级品记录（当需要读写分拆时，调用该方法）
        /// </summary>
        /// <param name="bo"></param>
        /// <param name="downgradingEntities"></param>
        /// <returns></returns>
        [Obsolete("建议用下面的方法，去方法外保存", false)]
        public async Task<int> CreateManuDowngradingsByConsumesAsync(DegradedProductExtendBo bo, IEnumerable<ManuDowngradingEntity>? downgradingEntities)
        {
            if (bo == null || downgradingEntities == null || !downgradingEntities.Any()) return 0;

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

        /// <summary>
        /// 创建降级品记录（当需要读写分拆时，调用该方法）
        /// </summary>
        /// <param name="bo"></param>
        /// <param name="downgradingEntities"></param>
        /// <returns></returns>
        public async Task<(List<ManuDowngradingEntity>, List<ManuDowngradingRecordEntity>)> GetManuDowngradingsByConsumesAsync(DegradedProductExtendBo bo, IEnumerable<ManuDowngradingEntity>? downgradingEntities)
        {
            if (bo == null || downgradingEntities == null || !downgradingEntities.Any()) return default;

            List<ManuDowngradingEntity> manuDowngradingEntities = new();
            List<ManuDowngradingRecordEntity> manuDowngradingRecordEntities = new();
            var downgradingRuleEntities = await _manuDowngradingRuleRepository.GetByCodesAsync(new ManuDowngradingRuleCodesQuery
            {
                SiteId = bo.SiteId,
                Codes = downgradingEntities.Select(d => d.Grade).ToArray()
            });

            //查询sfc对应的过程
            var sfcs = bo.KeyValues.Select(x=>x.SFC).Distinct().ToList();
            var sfcProduces = await _manuSfcProduceRepository.GetListBySfcsAsync(new ManuSfcProduceBySfcsQuery
            {
                SiteId =bo.SiteId,
                Sfcs = sfcs,
            });
            var sfcList = await _manuSfcRepository.GetManuSfcInfoEntitiesAsync(new ManuSfcStatusQuery { SiteId = bo.SiteId , Sfcs = sfcs });
            foreach (var groupitem in bo.KeyValues.GroupBy(k => k.SFC))
            {
                var targetdowngrads = downgradingEntities.Where(d => groupitem.ToList().Exists(c => c.BarCode == d.SFC)).Select(s => s.Grade);
                if (targetdowngrads.Any())
                {
                    var targetrulecode = downgradingRuleEntities
                                      .Where(d => targetdowngrads.Contains(d.Code))
                                      .OrderByDescending(r => r.SerialNumber).FirstOrDefault();
                    if (targetrulecode != null)
                    {
                        manuDowngradingEntities.Add(new ManuDowngradingEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            SFC = groupitem.Key,
                            Grade = targetrulecode.Code,
                            SiteId = bo.SiteId,
                            CreatedBy = bo.UserName ?? ""
                        });

                        manuDowngradingRecordEntities.Add(new ManuDowngradingRecordEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            SFC = groupitem.Key,
                            Grade = targetrulecode.Code,
                            SiteId = bo.SiteId,
                            IsCancellation = Core.Enums.Manufacture.ManuDowngradingRecordTypeEnum.Entry,
                            CreatedBy = bo.UserName ?? "",
                            SfcInfoId= sfcList.FirstOrDefault(x => x.SFC == groupitem.Key)?.SFCInfoId ?? 0,
                            ProcedureId = sfcProduces.FirstOrDefault(x => x.SFC ==groupitem.Key)?.ProcedureId??0
                        });
                    }
                }
            }
            return await Task.FromResult((manuDowngradingEntities, manuDowngradingRecordEntities));
        }
    }
}
