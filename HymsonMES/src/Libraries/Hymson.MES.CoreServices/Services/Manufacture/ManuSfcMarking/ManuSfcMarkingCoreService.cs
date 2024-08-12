using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.Data.Repositories.Common;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.CoreServices.Services.Manufacture.ManuSfcMarking
{
    /// <summary>
    /// Marking录入Service
    /// </summary>
    public class ManuSfcMarkingCoreService : IManuSfcMarkingCoreService
    {
        private readonly IManuSfcMarkingRepository _manuSfcMarkingRepository;
        private readonly IManuSfcMarkingExecuteRepository _manuSfcMarkingExecuteRepository;
        private readonly IManuSfcMarkingInterceptRepository _manuSfcMarkingInterceptRepository;
        private readonly IProcProcedureRepository _procProcedureRepository;
        private readonly IQualUnqualifiedCodeRepository _qualUnqualifiedCodeRepository;
        private readonly ISysConfigRepository _sysConfigRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ManuSfcMarkingCoreService(IManuSfcMarkingRepository manuSfcMarkingRepository,
            IManuSfcMarkingExecuteRepository manuSfcMarkingExecuteRepository,
            IManuSfcMarkingInterceptRepository manuSfcMarkingInterceptRepository,
            IProcProcedureRepository procProcedureRepository,
            IQualUnqualifiedCodeRepository qualUnqualifiedCodeRepository,
            ISysConfigRepository sysConfigRepository)
        {
            _manuSfcMarkingRepository = manuSfcMarkingRepository;
            _manuSfcMarkingExecuteRepository = manuSfcMarkingExecuteRepository;
            _manuSfcMarkingInterceptRepository = manuSfcMarkingInterceptRepository;
            _procProcedureRepository = procProcedureRepository;
            _qualUnqualifiedCodeRepository = qualUnqualifiedCodeRepository;
            _sysConfigRepository = sysConfigRepository;
        }

        /// <summary>
        /// 组装Marking继承参数
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public async Task<(IEnumerable<ManuSfcMarkingEntity>, IEnumerable<ManuSfcMarkingExecuteEntity>)> GetMarkingInheritEntityAsync(ManuSfcMarkingBo bo)
        {
            if (bo == null || string.IsNullOrWhiteSpace(bo.SFC) || bo.ConsumeSFCs == null || !bo.ConsumeSFCs.Any()) return default;

            //查询投入条码Marking信息
            var consumeSfcMarkingExecuteEntities = await _manuSfcMarkingExecuteRepository.GetEntitiesAsync(new ManuSfcMarkingExecuteQuery
            {
                SiteId = bo.SiteId,
                SFCs = bo.ConsumeSFCs
            });
            if (consumeSfcMarkingExecuteEntities == null || !consumeSfcMarkingExecuteEntities.Any())
            {
                return default;
            }

            //过滤已标记过的不合格代码
            var unqualifiedIds = consumeSfcMarkingExecuteEntities.Select(x => x.UnqualifiedCodeId).Distinct();
            var existMarkingEntities = await _manuSfcMarkingExecuteRepository.GetEntitiesAsync(new ManuSfcMarkingExecuteQuery
            {
                SiteId = bo.SiteId,
                SFC = bo.SFC,
                UnqualifiedCodeIds = unqualifiedIds
            });
            if (existMarkingEntities != null && existMarkingEntities.Any())
            {
                consumeSfcMarkingExecuteEntities = consumeSfcMarkingExecuteEntities.Where(x => !existMarkingEntities.Any(z => z.UnqualifiedCodeId == x.UnqualifiedCodeId));
            }

            var markingEntities = await _manuSfcMarkingRepository.GetByIdsAsync(consumeSfcMarkingExecuteEntities.Select(x => x.SfcMarkingId));

            //组装数据
            var manuSfcMarkingEntities = new List<ManuSfcMarkingEntity>();
            var manuSfcMarkingExecuteEntities = new List<ManuSfcMarkingExecuteEntity>();
            foreach (var item in consumeSfcMarkingExecuteEntities)
            {
                //投入条码中相同不合格代码存在多个条码，随机取其中一条
                if (manuSfcMarkingExecuteEntities.Any(x => x.UnqualifiedCodeId == item.UnqualifiedCodeId && x.SFC == bo.SFC))
                {
                    continue;
                }

                var id = IdGenProvider.Instance.CreateId();

                manuSfcMarkingEntities.Add(new ManuSfcMarkingEntity
                {
                    Id = id,
                    SiteId = bo.SiteId,
                    SFC = bo.SFC,
                    FoundBadProcedureId = item.FoundBadProcedureId,
                    UnqualifiedCodeId = item.UnqualifiedCodeId,
                    ShouldInterceptProcedureId = item.ShouldInterceptProcedureId,
                    Status = MarkingStatusEnum.Open,
                    MarkingType = item.MarkingType,
                    SourceType = MarkingSourceTypeEnum.Inherited,
                    ParentSFC = item.SFC,
                    OriginalSFC = markingEntities.FirstOrDefault(x => x.Id == item.SfcMarkingId)?.OriginalSFC ?? "",
                    Remark = item.Remark,
                    CreatedBy = bo.UserName,
                    UpdatedBy = bo.UserName
                });

                manuSfcMarkingExecuteEntities.Add(new ManuSfcMarkingExecuteEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = bo.SiteId,
                    SfcMarkingId = id,
                    SFC = bo.SFC,
                    FoundBadProcedureId = item.FoundBadProcedureId,
                    UnqualifiedCodeId = item.UnqualifiedCodeId,
                    ShouldInterceptProcedureId = item.ShouldInterceptProcedureId,
                    MarkingType = item.MarkingType,
                    Remark = item.Remark,
                    CreatedBy = bo.UserName,
                    UpdatedBy = bo.UserName
                });
            }

            return (manuSfcMarkingEntities, manuSfcMarkingExecuteEntities);
        }

        /// <summary>
        /// Marking拦截
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public async Task MarkingInterceptAsync(MarkingInterceptBo bo)
        {
            if (bo == null || bo.SFCs == null || !bo.SFCs.Any()) return;

            //查询应拦截工序
            var markingExecuteEntities = await _manuSfcMarkingExecuteRepository.GetEntitiesAsync(new ManuSfcMarkingExecuteQuery
            {
                SiteId = bo.SiteId,
                SFCs = bo.SFCs,
                Sorting = "Id ASC"
            });

            //过滤只标记不拦截的Marking
            markingExecuteEntities = markingExecuteEntities.Where(x => x.MarkingType == MarkingTypeEnum.Intercept && x.ShouldInterceptProcedureId > 0);

            if (!markingExecuteEntities.Any()) return;

            //获取所有工序
            var procedureEntities = await _procProcedureRepository.GetEntitiesAsync(new ProcProcedureQuery { SiteId = bo.SiteId });

            //是否拦截
            var isIntercept = false;

            //应在当前工序拦截的Marking规则
            var markingExecuteEntity = markingExecuteEntities.FirstOrDefault(x => x.ShouldInterceptProcedureId == bo.ProcedureId);
            if (markingExecuteEntity != null)
            {
                isIntercept = true;
            }
            else
            {
                //校验当前进站工序是否在应拦截工序之后
                //工序顺序判断逻辑：有自定义配置就按配置的顺序，否则以工序编码大小排序

                var sortedProcedures = new List<string>();  //排序后的工序

                //获取系统配置的工序顺序
                var sysConfigEntities = await _sysConfigRepository.GetEntitiesAsync(new Data.Repositories.Common.Query.SysConfigQuery
                {
                    SiteId = bo.SiteId,
                    Type = Core.Enums.SysConfigEnum.ProcedureSort
                });
                if (sysConfigEntities != null && sysConfigEntities.Any())
                {
                    var configValue = sysConfigEntities.First().Value;
                    if (!string.IsNullOrWhiteSpace(configValue))
                    {
                        sortedProcedures = configValue.Split(",").ToList();
                    }
                }

                //查找满足拦截条件的Marking规则
                var currentProcrdureCode = procedureEntities.FirstOrDefault(x => x.Id == bo.ProcedureId)?.Code ?? "";
                var currentProcrdureIndex = sortedProcedures.IndexOf(currentProcrdureCode);  //当前进站工序索引
                foreach (var item in markingExecuteEntities)
                {
                    var shouldInterceptProcedureCode = procedureEntities.FirstOrDefault(x => x.Id == item.ShouldInterceptProcedureId)?.Code ?? "";
                    var shouldInterceptProcedureIndex = sortedProcedures.IndexOf(shouldInterceptProcedureCode);  //应拦截工序索引

                    //1.当前进站工序与设置拦截工序都在自定义配置中，按自定义顺序判断
                    if (currentProcrdureIndex != -1 && shouldInterceptProcedureIndex != -1)
                    {
                        if (shouldInterceptProcedureIndex < currentProcrdureIndex)
                        {
                            isIntercept = true;
                            markingExecuteEntity = item;
                            break;
                        }
                    }
                    //2.按工序编码大小判断
                    else
                    {
                        if (shouldInterceptProcedureCode.ParseToInt() < currentProcrdureCode.ParseToInt())
                        {
                            isIntercept = true;
                            markingExecuteEntity = item;
                            break;
                        }
                    }
                }
            }

            if (isIntercept && markingExecuteEntity != null)
            {
                //插入拦截记录
                var entity = new ManuSfcMarkingInterceptEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = markingExecuteEntity.SiteId,
                    SfcMarkingId = markingExecuteEntity.SfcMarkingId,
                    InterceptProcedureId = bo.ProcedureId,
                    InterceptEquipmentId = bo.EquipmentId,
                    InterceptResourceId = bo.ResourceId,
                    InterceptOn = HymsonClock.Now(),
                    CreatedBy = bo.UserName,
                    UpdatedBy = bo.UserName
                };
                await _manuSfcMarkingInterceptRepository.InsertAsync(entity);

                //抛出错误
                var unqualifiedEntity = await _qualUnqualifiedCodeRepository.GetByIdAsync(markingExecuteEntity.UnqualifiedCodeId);
                var shouldInterceptProcedureEntity = procedureEntities.FirstOrDefault(x => x.Id == markingExecuteEntity.ShouldInterceptProcedureId);

                throw new CustomerValidationException(nameof(ErrorCode.MES19713)).WithData("sfc", markingExecuteEntity.SFC)
                    .WithData("unqualifiedCode", unqualifiedEntity?.UnqualifiedCode ?? "")
                    .WithData("unqualifiedName", unqualifiedEntity?.UnqualifiedCodeName ?? "")
                    .WithData("procedureCode", shouldInterceptProcedureEntity?.Code ?? "")
                    .WithData("procedureName", shouldInterceptProcedureEntity?.Name ?? "");
            }
        }
    }
}
