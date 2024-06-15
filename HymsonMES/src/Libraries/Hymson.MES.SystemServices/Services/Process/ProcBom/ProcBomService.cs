using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Integrated.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.SystemServices.Dtos;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;

namespace Hymson.MES.SystemServices.Services.Process
{
    /// <summary>
    /// 服务（BOM）
    /// </summary>
    public class ProcBomService : IProcBomService
    {
        /// <summary>
        /// 仓储接口（BOM表）
        /// </summary>
        private readonly IProcBomRepository _procBomRepository;

        /// <summary>
        /// 仓储接口（BOM明细表）
        /// </summary>
        private readonly IProcBomDetailRepository _procBomDetailRepository;

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
        /// <param name="procBomRepository"></param>
        /// <param name="procBomDetailRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="inteWorkCenterRepository"></param>
        public ProcBomService(IProcBomRepository procBomRepository,
            IProcBomDetailRepository procBomDetailRepository,
            IProcMaterialRepository procMaterialRepository,
            IInteWorkCenterRepository inteWorkCenterRepository)
        {
            _procBomRepository = procBomRepository;
            _procBomDetailRepository = procBomDetailRepository;
            _procMaterialRepository = procMaterialRepository;
            _inteWorkCenterRepository = inteWorkCenterRepository;
        }


        /// <summary>
        /// BOM信息（同步）
        /// </summary>
        /// <param name="requestDtos"></param>
        /// <returns></returns>
        public async Task<int> SyncBomAsync(IEnumerable<BomDto> requestDtos)
        {
            if (requestDtos == null || !requestDtos.Any()) return 0;

            var resposeSummaryBo = new SyncBomSummaryBo();

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

                var resposeBo = await GetBomListAsync(lineEntity, lineDict);
                if (resposeBo == null) continue;

                // 添加到集合
                resposeSummaryBo.BomAdds.AddRange(resposeBo.BomAdds);
                resposeSummaryBo.BomUpdates.AddRange(resposeBo.BomUpdates);
                resposeSummaryBo.BomDetailAdds.AddRange(resposeBo.BomDetailAdds);
            }

            // 删除参数
            var deleteCommand = new DeleteCommand
            {
                UserId = "ERP",
                DeleteOn = HymsonClock.Now(),
                Ids = resposeSummaryBo.BomAdds.Select(s => s.Id)
            };

            // 插入数据
            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();
            rows += await _procBomDetailRepository.DeleteBomIDAsync(deleteCommand);
            rows += await _procBomDetailRepository.InsertsAsync(resposeSummaryBo.BomDetailAdds);
            rows += await _procBomRepository.InsertsAsync(resposeSummaryBo.BomAdds);
            rows += await _procBomRepository.UpdatesAsync(resposeSummaryBo.BomUpdates);
            return rows;
        }

        /// <summary>
        /// 转换BOM信息集合
        /// </summary>
        /// <param name="lineEntity"></param>
        /// <param name="lineDtoDict"></param>
        /// <returns></returns>
        private async Task<SyncBomSummaryBo> GetBomListAsync(InteWorkCenterEntity lineEntity, IEnumerable<BomDto> lineDtoDict)
        {
            var resposeBo = new SyncBomSummaryBo();

            // 判断产线是否存在
            if (lineEntity == null) return resposeBo;

            // 初始化
            var siteId = lineEntity.SiteId ?? 0;
            var updateUser = "ERP";
            var updateTime = HymsonClock.Now();

            // 判断是否有不存在的物料编码
            var materialCodes = lineDtoDict.SelectMany(s => s.BomMaterials).Select(s => s.MaterialCode).Distinct();
            var materialEntities = await _procMaterialRepository.GetByCodesAsync(new ProcMaterialsByCodeQuery { SiteId = lineEntity.SiteId, MaterialCodes = materialCodes });
            if (materialEntities == null || materialEntities.Any())
            {
                // 这里应该提示物料不存在
                return resposeBo;
            }

            // 判断BOM编码是否存在
            var bomCodes = lineDtoDict.Select(s => s.BomCode).Distinct();
            var bomEntities = await _procBomRepository.GetByCodesAsync(new ProcBomsByCodeQuery { SiteId = lineEntity.SiteId, Codes = bomCodes });
            foreach (var bomDto in lineDtoDict)
            {
                var bomEntity = bomEntities.FirstOrDefault(f => f.BomCode == bomDto.BomCode);

                // 不存在的新BOM
                if (bomEntity == null)
                {
                    bomEntity = new ProcBomEntity
                    {
                        BomCode = bomDto.BomCode,
                        BomName = bomDto.BomName,
                        Version = bomDto.BomVersion,
                        Status = SysDataStatusEnum.Enable, // 这个是直接启用，还是需要在MES去点启用？
                        Remark = "",
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = siteId,
                        CreatedBy = updateUser,
                        CreatedOn = updateTime,
                        UpdatedBy = updateUser,
                        UpdatedOn = updateTime,
                    };

                    // 添加BOM
                    resposeBo.BomAdds.Add(bomEntity);
                }
                // 之前已存在的BOM
                else
                {
                    bomEntity.BomName = bomDto.BomName;
                    bomEntity.Version = bomDto.BomVersion;
                    bomEntity.UpdatedBy = updateUser;
                    bomEntity.UpdatedOn = updateTime;
                    resposeBo.BomUpdates.Add(bomEntity);
                }

                // 添加BOM明细
                var seq = 1;
                foreach (var bomMaterialDto in bomDto.BomMaterials)
                {
                    var materialEntity = materialEntities.FirstOrDefault(f => f.MaterialCode == bomMaterialDto.MaterialCode);
                    if (materialEntity == null) continue;

                    resposeBo.BomDetailAdds.Add(new ProcBomDetailEntity
                    {
                        BomId = bomEntity.Id,
                        MaterialId = materialEntity.Id,
                        Usages = bomMaterialDto.MaterialDosage,
                        Loss = bomMaterialDto.MaterialLoss,
                        Seq = seq,
                        ProcedureId = 0,// ERP 没有维护工序
                        DataCollectionWay = MaterialSerialNumberEnum.Batch,
                        IsEnableReplace = false,
                        ReferencePoint = "",
                        Remark = "",
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = siteId,
                        CreatedBy = updateUser,
                        CreatedOn = updateTime,
                        UpdatedBy = updateUser,
                        UpdatedOn = updateTime,
                    });

                    seq++;
                }
            }

            return resposeBo;
        }

    }

    /// <summary>
    /// 同步Bom信息BO对象
    /// </summary>
    public class SyncBomSummaryBo
    {
        /// <summary>
        /// 新增（BOM）
        /// </summary>
        public List<ProcBomEntity> BomAdds { get; set; } = new();
        /// <summary>
        /// 更新（BOM）
        /// </summary>
        public List<ProcBomEntity> BomUpdates { get; set; } = new();
        /// <summary>
        /// 新增（BOM明细）
        /// </summary>
        public List<ProcBomDetailEntity> BomDetailAdds { get; set; } = new();
    }
}
