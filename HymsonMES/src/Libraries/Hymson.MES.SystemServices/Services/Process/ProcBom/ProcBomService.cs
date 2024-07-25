using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Common;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Common;
using Hymson.MES.Data.Repositories.Common.Command;
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
    /// 服务（BOM）
    /// </summary>
    public class ProcBomService : IProcBomService
    {
        /// <summary>
        /// 日志对象
        /// </summary>
        private readonly ILogger<ProcBomService> _logger;

        /// <summary>
        /// 仓储接口（系统配置）
        /// </summary>
        private readonly ISysConfigRepository _sysConfigRepository;

        /// <summary>
        /// 仓储接口（BOM表）
        /// </summary>
        private readonly IProcBomRepository _procBomRepository;

        /// <summary>
        /// 仓储接口（BOM明细表）
        /// </summary>
        private readonly IProcBomDetailRepository _procBomDetailRepository;

        /// <summary>
        /// 仓储接口（BOM明细替代料表）
        /// </summary>
        private readonly IProcBomDetailReplaceMaterialRepository _procBomDetailReplaceMaterialRepository;

        /// <summary>
        /// 仓储接口（物料）
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 仓储接口（工序）
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="sysConfigRepository"></param>
        /// <param name="procBomRepository"></param>
        /// <param name="procBomDetailRepository"></param>
        /// <param name="procBomDetailReplaceMaterialRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="procProcedureRepository"></param>
        public ProcBomService(ILogger<ProcBomService> logger,
            ISysConfigRepository sysConfigRepository,
            IProcBomRepository procBomRepository,
            IProcBomDetailRepository procBomDetailRepository,
            IProcBomDetailReplaceMaterialRepository procBomDetailReplaceMaterialRepository,
            IProcMaterialRepository procMaterialRepository,
            IProcProcedureRepository procProcedureRepository)
        {
            _logger = logger;
            _sysConfigRepository = sysConfigRepository;
            _procBomRepository = procBomRepository;
            _procBomDetailRepository = procBomDetailRepository;
            _procBomDetailReplaceMaterialRepository = procBomDetailReplaceMaterialRepository;
            _procMaterialRepository = procMaterialRepository;
            _procProcedureRepository = procProcedureRepository;
        }


        /// <summary>
        /// 同步信息（BOM）
        /// </summary>
        /// <param name="requestDtos"></param>
        /// <returns></returns>
        public async Task<int> SyncBomAsync(IEnumerable<SyncBomDto> requestDtos)
        {
            if (requestDtos == null || !requestDtos.Any()) return 0;

            var configEntities = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.MainSite });
            if (configEntities == null || !configEntities.Any()) return 0;

            var resposeBo = await ConvertBomListAsync(configEntities.FirstOrDefault(), requestDtos);
            if (resposeBo == null) return 0;

            // 添加到集合
            var resposeSummaryBo = new SyncBomSummaryBo();
            resposeSummaryBo.Adds.AddRange(resposeBo.Adds);
            resposeSummaryBo.Updates.AddRange(resposeBo.Updates);
            resposeSummaryBo.DetailAdds.AddRange(resposeBo.DetailAdds);

            var ids = resposeSummaryBo.Adds.Select(s => s.Id);
            ids = ids.Concat(resposeSummaryBo.Updates.Select(s => s.Id));

            // 删除参数
            var deleteCommand = new DeleteCommand
            {
                UserId = "ERP",
                DeleteOn = HymsonClock.Now(),
                Ids = ids
            };

            // 插入数据
            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();
            rows += await _procBomDetailRepository.DeleteBomIDAsync(deleteCommand);
            rows += await _procBomDetailReplaceMaterialRepository.DeleteBomIDAsync(deleteCommand);

            rows += await _procBomDetailReplaceMaterialRepository.InsertsAsync(resposeSummaryBo.ReplaceDetailAdds);
            rows += await _procBomDetailRepository.InsertsAsync(resposeSummaryBo.DetailAdds);
            rows += await _procBomRepository.InsertsAsync(resposeSummaryBo.Adds);
            rows += await _procBomRepository.UpdatesAsync(resposeSummaryBo.Updates);
            trans.Complete();
            return rows;
        }

        /// <summary>
        /// 转换信息集合（BOM）
        /// </summary>
        /// <param name="configEntity"></param>
        /// <param name="lineDtoDict"></param>
        /// <returns></returns>
        private async Task<SyncBomSummaryBo?> ConvertBomListAsync(SysConfigEntity? configEntity, IEnumerable<SyncBomDto> lineDtoDict)
        {
            // 判断是否存在（配置）
            if (configEntity == null) return default;

            // 初始化
            var siteId = configEntity.Value.ParseToLong();
            var updateUser = "ERP";
            var updateTime = HymsonClock.Now();

            var resposeBo = new SyncBomSummaryBo();

            // 判断是否有不存在的物料编码
            var materialCodes = lineDtoDict.SelectMany(s => s.BomMaterials).Select(s => s.MaterialCode).Distinct();
            var materialEntities = await _procMaterialRepository.GetEntitiesAsync(new ProcMaterialQuery { SiteId = siteId, MaterialCodes = materialCodes });
            if (materialEntities == null || !materialEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES152011));
            }

            // 读取已存在的BOM记录
            /*
            var bomCodes = lineDtoDict.Select(s => s.BomCode).Distinct();
            var bomEntities = await _procBomRepository.GetByCodesAsync(new ProcBomsByCodeQuery { SiteId = siteId, Codes = bomCodes });
            */
            var bomIds = lineDtoDict.Select(s => s.Id ?? IdGenProvider.Instance.CreateId()).Distinct();
            var bomEntities = await _procBomRepository.GetByIdsAsync(bomIds);

            // 读取已存在的工序记录
            var procedureCodes = lineDtoDict.SelectMany(s => s.BomMaterials).Select(s => s.ProcedureCode).Distinct();
            var procedureEntities = await _procProcedureRepository.GetEntitiesAsync(new ProcProcedureQuery { SiteId = siteId, Codes = procedureCodes });
            if (materialEntities == null || !materialEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10406));
            }

            var defaultStatus = SysDataStatusEnum.Enable;

            // 遍历数据
            foreach (var bomDto in lineDtoDict)
            {
                var bomEntity = bomEntities.FirstOrDefault(f => f.Id == bomDto.Id);

                // 不存在的新BOM
                if (bomEntity == null)
                {
                    var bomId = bomDto.Id ?? IdGenProvider.Instance.CreateId();
                    bomEntity = new ProcBomEntity
                    {
                        Id = bomId,
                        BomCode = bomDto.BomCode,
                        BomName = bomDto.BomName,
                        Version = bomDto.BomVersion,
                        Status = defaultStatus,
                        Remark = "",

                        SiteId = siteId,
                        CreatedBy = updateUser,
                        CreatedOn = updateTime,
                        UpdatedBy = updateUser,
                        UpdatedOn = updateTime
                    };

                    // 添加BOM
                    resposeBo.Adds.Add(bomEntity);
                }
                // 之前已存在的BOM
                else
                {
                    bomEntity.BomCode = bomDto.BomCode;
                    bomEntity.BomName = bomDto.BomName;
                    bomEntity.Version = bomDto.BomVersion;
                    bomEntity.Status = defaultStatus;
                    bomEntity.UpdatedBy = updateUser;
                    bomEntity.UpdatedOn = updateTime;
                    resposeBo.Updates.Add(bomEntity);
                }

                // 添加BOM明细
                var seq = 1;
                foreach (var bomMaterialDto in bomDto.BomMaterials)
                {
                    var materialEntity = materialEntities.FirstOrDefault(f => f.MaterialCode == bomMaterialDto.MaterialCode)
                        ?? throw new CustomerValidationException(nameof(ErrorCode.MES152011));

                    var bomDetailId = bomMaterialDto.Id ?? IdGenProvider.Instance.CreateId();

                    // 工序
                    var procedureEntity = procedureEntities.FirstOrDefault(f => f.Code == bomMaterialDto.ProcedureCode);

                    // BOM明细
                    resposeBo.DetailAdds.Add(new ProcBomDetailEntity
                    {
                        Id = bomDetailId,
                        BomId = bomEntity.Id,
                        MaterialId = materialEntity.Id,
                        Usages = bomMaterialDto.MaterialDosage,
                        Loss = bomMaterialDto.MaterialLoss,
                        Seq = seq,
                        BomProductType = bomMaterialDto.BomProductType ?? ManuProductTypeEnum.Normal,
                        ProcedureId = procedureEntity?.Id ?? 0, // ERP 没有维护工序
                        DataCollectionWay = MaterialSerialNumberEnum.Batch,
                        IsEnableReplace = false,
                        ReferencePoint = "",
                        Remark = "",

                        SiteId = siteId,
                        CreatedBy = updateUser,
                        CreatedOn = updateTime,
                        UpdatedBy = updateUser,
                        UpdatedOn = updateTime,
                    });

                    // BOM明细-替代料
                    if (bomMaterialDto.ReplaceMaterials != null && bomMaterialDto.ReplaceMaterials.Any())
                    {
                        resposeBo.ReplaceDetailAdds.AddRange(bomMaterialDto.ReplaceMaterials.Select(s => new ProcBomDetailReplaceMaterialEntity
                        {
                            Id = s.Id ?? IdGenProvider.Instance.CreateId(),
                            BomId = bomEntity.Id,
                            BomDetailId = bomDetailId,
                            ReplaceMaterialId = s.Id ?? IdGenProvider.Instance.CreateId(),
                            Usages = s.MaterialDosage,
                            Loss = s.MaterialLoss,
                            DataCollectionWay = MaterialSerialNumberEnum.Batch,
                            ReferencePoint = "",
                            Remark = "",

                            SiteId = siteId,
                            CreatedBy = updateUser,
                            CreatedOn = updateTime,
                            UpdatedBy = updateUser,
                            UpdatedOn = updateTime,
                        }));
                    }

                    seq++;
                }
            }

            return resposeBo;
        }

    }

    /// <summary>
    /// 同步信息BO对象（BOM）
    /// </summary>
    public class SyncBomSummaryBo
    {
        /// <summary>
        /// 新增（BOM）
        /// </summary>
        public List<ProcBomEntity> Adds { get; set; } = new();
        /// <summary>
        /// 更新（BOM）
        /// </summary>
        public List<ProcBomEntity> Updates { get; set; } = new();
        /// <summary>
        /// 新增（BOM明细）
        /// </summary>
        public List<ProcBomDetailEntity> DetailAdds { get; set; } = new();
        /// <summary>
        /// 新增（BOM明细-替代料）
        /// </summary>
        public List<ProcBomDetailReplaceMaterialEntity> ReplaceDetailAdds { get; set; } = new();

    }
}
