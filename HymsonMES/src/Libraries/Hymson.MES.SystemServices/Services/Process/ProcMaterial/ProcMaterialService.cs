using Hymson.MES.Core.Domain.Common;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.SystemServices.Dtos;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.Extensions.Logging;

namespace Hymson.MES.SystemServices.Services.Process
{
    /// <summary>
    /// 服务（物料）
    /// </summary>
    public class ProcMaterialService : IProcMaterialService
    {
        /// <summary>
        /// 日志对象
        /// </summary>
        private readonly ILogger<ProcMaterialService> _logger;

        /// <summary>
        /// 仓储接口（系统配置）
        /// </summary>
        private readonly ISysConfigRepository _sysConfigRepository;

        /// <summary>
        /// 仓储接口（物料）
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="sysConfigRepository"></param>
        /// <param name="procMaterialRepository"></param>
        public ProcMaterialService(ILogger<ProcMaterialService> logger,
            ISysConfigRepository sysConfigRepository,
            IProcMaterialRepository procMaterialRepository)
        {
            _logger = logger;
            _sysConfigRepository = sysConfigRepository;
            _procMaterialRepository = procMaterialRepository;
        }


        /// <summary>
        /// 同步信息（物料）
        /// </summary>
        /// <param name="requestDtos"></param>
        /// <returns></returns>
        public async Task<int> SyncMaterialAsync(IEnumerable<MaterialDto> requestDtos)
        {
            if (requestDtos == null || !requestDtos.Any()) return 0;

            var configEntities = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.ERPSite });
            if (configEntities == null || !configEntities.Any()) return 0;

            var resposeBo = await ConvertMaterialListAsync(configEntities.FirstOrDefault(), requestDtos);
            if (resposeBo == null) return 0;

            // 添加到集合
            var resposeSummaryBo = new SyncMaterialSummaryBo();
            resposeSummaryBo.Adds.AddRange(resposeBo.Adds);
            resposeSummaryBo.Updates.AddRange(resposeBo.Updates);

            // 插入数据
            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();
            rows += await _procMaterialRepository.InsertsAsync(resposeSummaryBo.Adds);
            rows += await _procMaterialRepository.UpdatesAsync(resposeSummaryBo.Updates);
            trans.Complete();
            return rows;
        }

        /// <summary>
        /// 转换信息集合（物料）
        /// </summary>
        /// <param name="configEntity"></param>
        /// <param name="lineDtoDict"></param>
        /// <returns></returns>
        private async Task<SyncMaterialSummaryBo?> ConvertMaterialListAsync(SysConfigEntity? configEntity, IEnumerable<MaterialDto> lineDtoDict)
        {
            // 判断是否存在（配置）
            if (configEntity == null) return default;

            // 初始化
            var siteId = configEntity.Value.ParseToLong();
            var updateUser = "ERP";
            var updateTime = HymsonClock.Now();

            var resposeBo = new SyncMaterialSummaryBo();

            // 判断是否有不存在的物料编码
            var materialCodes = lineDtoDict.Select(s => s.Code).Distinct();
            var materialEntities = await _procMaterialRepository.GetByCodesAsync(new ProcMaterialsByCodeQuery { SiteId = siteId, MaterialCodes = materialCodes });
            if (materialEntities == null || materialEntities.Any())
            {
                // 这里应该提示物料不存在
                return resposeBo;
            }

            // 遍历数据
            foreach (var materialDto in lineDtoDict)
            {
                var materialEntity = materialEntities.FirstOrDefault(f => f.MaterialCode == materialDto.Code);

                // 不存在的新物料
                if (materialEntity == null)
                {
                    materialEntity = new ProcMaterialEntity
                    {
                        MaterialCode = materialDto.Code,
                        MaterialName = materialDto.Name,
                        Version = materialDto.Version,
                        Batch = materialDto.Batch,
                        ShelfLife = materialDto.ShelfLife,
                        ValidTime = materialDto.ValidTime,
                        BuyType = materialDto.BuyType,
                        SerialNumber = materialDto.SerialNumber,
                        Remark = materialDto.Description,

                        // TODO: 这里需要根据物料编码获取物料信息

                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = siteId,
                        CreatedBy = updateUser,
                        CreatedOn = updateTime,
                        UpdatedBy = updateUser,
                        UpdatedOn = updateTime
                    };

                    // 添加物料
                    resposeBo.Adds.Add(materialEntity);
                }
                // 之前已存在的物料
                else
                {
                    // 如果是"启用"的物料，不允许修改
                    if (materialEntity.Status != SysDataStatusEnum.Enable)
                    {
                        // 这里应该提示当前的生产计划状态不允许修改
                        continue;
                    }

                    materialEntity.MaterialName = materialDto.Name;
                    materialEntity.Version = materialDto.Version;
                    materialEntity.Batch = materialDto.Batch;
                    materialEntity.ShelfLife = materialDto.ShelfLife;
                    materialEntity.ValidTime = materialDto.ValidTime;
                    materialEntity.BuyType = materialDto.BuyType;
                    materialEntity.SerialNumber = materialDto.SerialNumber;
                    materialEntity.Remark = materialDto.Description;

                    // TODO: 这里需要根据物料编码获取物料信息

                    materialEntity.UpdatedBy = updateUser;
                    materialEntity.UpdatedOn = updateTime;
                    resposeBo.Updates.Add(materialEntity);
                }
            }

            return resposeBo;
        }

    }

    /// <summary>
    /// 同步信息BO对象（物料）
    /// </summary>
    public class SyncMaterialSummaryBo
    {
        /// <summary>
        /// 新增（物料）
        /// </summary>
        public List<ProcMaterialEntity> Adds { get; set; } = new();
        /// <summary>
        /// 更新（物料）
        /// </summary>
        public List<ProcMaterialEntity> Updates { get; set; } = new();

    }
}
