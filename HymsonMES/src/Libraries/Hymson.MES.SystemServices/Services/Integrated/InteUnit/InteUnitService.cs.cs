using Hymson.MES.Core.Domain.Common;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Integrated.Query;
using Hymson.MES.SystemServices.Dtos;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.Extensions.Logging;

namespace Hymson.MES.SystemServices.Services.Integrated
{
    /// <summary>
    /// 服务（单位）
    /// </summary>
    public class InteUnitService : IInteUnitService
    {
        /// <summary>
        /// 日志对象
        /// </summary>
        private readonly ILogger<InteUnitService> _logger;

        /// <summary>
        /// 仓储接口（系统配置）
        /// </summary>
        private readonly ISysConfigRepository _sysConfigRepository;

        /// <summary>
        /// 仓储接口（单位）
        /// </summary>
        private readonly IInteUnitRepository _inteUnitRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="sysConfigRepository"></param>
        /// <param name="inteUnitRepository"></param>
        public InteUnitService(ILogger<InteUnitService> logger,
            ISysConfigRepository sysConfigRepository,
            IInteUnitRepository inteUnitRepository)
        {
            _logger = logger;
            _sysConfigRepository = sysConfigRepository;
            _inteUnitRepository = inteUnitRepository;
        }

        /// <summary>
        /// 同步信息（单位）
        /// </summary>
        /// <param name="requestDtos"></param>
        /// <returns></returns>
        public async Task<int> SyncUnitAsync(IEnumerable<InteUnitDto> requestDtos)
        {
            if (requestDtos == null || !requestDtos.Any()) return 0;

            var configEntities = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.ERPSite });
            if (configEntities == null || !configEntities.Any()) return 0;

            var resposeBo = await ConvertUnitListAsync(configEntities.FirstOrDefault(), requestDtos);
            if (resposeBo == null) return 0;

            // 添加到集合
            var resposeSummaryBo = new SyncUnitSummaryBo();
            resposeSummaryBo.Adds.AddRange(resposeBo.Adds);
            resposeSummaryBo.Updates.AddRange(resposeBo.Updates);

            // 插入数据
            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();
            rows += await _inteUnitRepository.InsertRangeAsync(resposeSummaryBo.Adds);
            rows += await _inteUnitRepository.UpdateRangeAsync(resposeSummaryBo.Updates);
            trans.Complete();
            return rows;
        }

        /// <summary>
        /// 转换信息集合（单位）
        /// </summary>
        /// <param name="configEntity"></param>
        /// <param name="lineDtoDict"></param>
        /// <returns></returns>
        private async Task<SyncUnitSummaryBo?> ConvertUnitListAsync(SysConfigEntity? configEntity, IEnumerable<InteUnitDto> lineDtoDict)
        {
            // 判断是否存在（配置）
            if (configEntity == null) return default;

            // 初始化
            var siteId = configEntity.Value.ParseToLong();
            var updateUser = "ERP";
            var updateTime = HymsonClock.Now();

            var resposeBo = new SyncUnitSummaryBo();

            // 判断是否有不存在的单位编码
            var unitCodes = lineDtoDict.Select(s => s.UnitCode).Distinct();
            var unitEntities = await _inteUnitRepository.GetEntitiesAsync(new InteUnitQuery { SiteId = siteId, Codes = unitCodes });
            if (unitEntities == null || unitEntities.Any())
            {
                // 这里应该提示单位不存在
                return resposeBo;
            }

            // 遍历数据
            foreach (var unitDto in lineDtoDict)
            {
                var unitEntity = unitEntities.FirstOrDefault(f => f.Code == unitDto.UnitCode);

                // 不存在的新单位
                if (unitEntity == null)
                {
                    unitEntity = new InteUnitEntity
                    {
                        Code = unitDto.UnitCode,
                        Name = unitDto.UnitName,
                        Remark = "",

                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = siteId,
                        CreatedBy = updateUser,
                        CreatedOn = updateTime,
                        UpdatedBy = updateUser,
                        UpdatedOn = updateTime
                    };

                    // 添加单位
                    resposeBo.Adds.Add(unitEntity);
                }
                // 之前已存在的单位
                else
                {
                    unitEntity.Name = unitDto.UnitName;
                    unitEntity.Remark = "";

                    unitEntity.UpdatedBy = updateUser;
                    unitEntity.UpdatedOn = updateTime;
                    resposeBo.Updates.Add(unitEntity);
                }
            }

            return resposeBo;
        }

    }

    /// <summary>
    /// 同步信息BO对象（单位）
    /// </summary>
    public class SyncUnitSummaryBo
    {
        /// <summary>
        /// 新增（单位）
        /// </summary>
        public List<InteUnitEntity> Adds { get; set; } = new();
        /// <summary>
        /// 更新（单位）
        /// </summary>
        public List<InteUnitEntity> Updates { get; set; } = new();
    }
}
