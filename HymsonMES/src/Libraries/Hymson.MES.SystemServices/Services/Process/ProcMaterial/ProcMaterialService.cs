using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Integrated.Query;
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
        /// 仓储接口（物料）
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 仓储接口（工作中心）
        /// </summary>
        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="inteWorkCenterRepository"></param>
        public ProcMaterialService(ILogger<ProcMaterialService> logger,
            IProcMaterialRepository procMaterialRepository,
            IInteWorkCenterRepository inteWorkCenterRepository)
        {
            _logger = logger;
            _procMaterialRepository = procMaterialRepository;
            _inteWorkCenterRepository = inteWorkCenterRepository;
        }


        /// <summary>
        /// 同步信息（物料）
        /// </summary>
        /// <param name="requestDtos"></param>
        /// <returns></returns>
        public async Task<int> SyncMaterialAsync(IEnumerable<MaterialDto> requestDtos)
        {
            if (requestDtos == null || !requestDtos.Any()) return 0;

            var resposeSummaryBo = new SyncMaterialSummaryBo();

            // 判断产线是否存在
            var lineCodes = requestDtos.Select(s => s.LineCode).Distinct();
            var lineEntities = await _inteWorkCenterRepository.GetAllSiteEntitiesAsync(new InteWorkCenterQuery { Codes = lineCodes });

            // 通过产线分组数据（支持一次传多个站点的数据，但是不建议这么传）
            var requestDict = requestDtos.GroupBy(g => g.LineCode);
            foreach (var lineDict in requestDict)
            {
                var lineEntity = lineEntities.FirstOrDefault(f => f.Code == lineDict.Key);
                if (lineEntity == null)
                {
                    // 这里应该提示产线不存在
                    continue;
                }

                var resposeBo = await ConvertMaterialListAsync(lineEntity, lineDict);
                if (resposeBo == null) continue;

                // 添加到集合
                resposeSummaryBo.MaterialAdds.AddRange(resposeBo.MaterialAdds);
                resposeSummaryBo.MaterialUpdates.AddRange(resposeBo.MaterialUpdates);
            }

            // 插入数据
            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();
            rows += await _procMaterialRepository.InsertsAsync(resposeSummaryBo.MaterialAdds);
            rows += await _procMaterialRepository.UpdatesAsync(resposeSummaryBo.MaterialUpdates);
            return rows;
        }

        /// <summary>
        /// 转换信息集合（物料）
        /// </summary>
        /// <param name="lineEntity"></param>
        /// <param name="lineDtoDict"></param>
        /// <returns></returns>
        private async Task<SyncMaterialSummaryBo> ConvertMaterialListAsync(InteWorkCenterEntity lineEntity, IEnumerable<MaterialDto> lineDtoDict)
        {
            var resposeBo = new SyncMaterialSummaryBo();

            // 判断产线是否存在
            if (lineEntity == null) return resposeBo;

            // 初始化
            var siteId = lineEntity.SiteId ?? 0;
            var updateUser = "ERP";
            var updateTime = HymsonClock.Now();

            // 判断是否有不存在的物料编码
            var materialCodes = lineDtoDict.Select(s => s.Code).Distinct();
            var materialEntities = await _procMaterialRepository.GetByCodesAsync(new ProcMaterialsByCodeQuery { SiteId = lineEntity.SiteId, MaterialCodes = materialCodes });
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
                    resposeBo.MaterialAdds.Add(materialEntity);
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
                    resposeBo.MaterialUpdates.Add(materialEntity);
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
        public List<ProcMaterialEntity> MaterialAdds { get; set; } = new();
        /// <summary>
        /// 更新（物料）
        /// </summary>
        public List<ProcMaterialEntity> MaterialUpdates { get; set; } = new();

    }
}
