using Hymson.MES.Core.Domain.Common;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.SystemServices.Dtos;
using Hymson.MES.SystemServices.Services.Process;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.Extensions.Logging;
using System.Data.SqlTypes;

namespace Hymson.MES.SystemServices.Services.Integrated
{
    /// <summary>
    /// 服务（供应商）
    /// </summary>
    public class InteSupplierService : IInteSupplierService
    {
        /// <summary>
        /// 日志对象
        /// </summary>
        private readonly ILogger<InteSupplierService> _logger;

        /// <summary>
        /// 仓储接口（系统配置）
        /// </summary>
        private readonly ISysConfigRepository _sysConfigRepository;

        /// <summary>
        /// 仓储接口（供应商）
        /// </summary>
        private readonly IWhSupplierRepository _whSupplierRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="sysConfigRepository"></param>
        /// <param name="whSupplierRepository"></param>
        public InteSupplierService(ILogger<InteSupplierService> logger,
            ISysConfigRepository sysConfigRepository,
            IWhSupplierRepository whSupplierRepository)
        {
            _logger = logger;
            _sysConfigRepository = sysConfigRepository;
            _whSupplierRepository = whSupplierRepository;
        }

        /// <summary>
        /// 同步信息（供应商）
        /// </summary>
        /// <param name="requestDtos"></param>
        /// <returns></returns>
        public async Task<int> SyncSupplierAsync(IEnumerable<InteSupplierDto> requestDtos)
        {
            if (requestDtos == null || !requestDtos.Any()) return 0;

            var configEntities = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.MainSite });
            if (configEntities == null || !configEntities.Any()) return 0;

            var resposeBo = await ConvertSupplierListAsync(configEntities.FirstOrDefault(), requestDtos);
            if (resposeBo == null) return 0;

            // 添加到集合
            var resposeSummaryBo = new SyncSupplierSummaryBo();
            resposeSummaryBo.Adds.AddRange(resposeBo.Adds);
            resposeSummaryBo.Updates.AddRange(resposeBo.Updates);

            // 插入数据
            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();
            rows += await _whSupplierRepository.InsertsAsync(resposeSummaryBo.Adds);
            rows += await _whSupplierRepository.UpdatesAsync(resposeSummaryBo.Updates);
            trans.Complete();
            return rows;
        }

        /// <summary>
        /// 转换信息集合（供应商）
        /// </summary>
        /// <param name="configEntity"></param>
        /// <param name="lineDtoDict"></param>
        /// <returns></returns>
        private async Task<SyncSupplierSummaryBo?> ConvertSupplierListAsync(SysConfigEntity? configEntity, IEnumerable<InteSupplierDto> lineDtoDict)
        {
            // 判断是否存在（配置）
            if (configEntity == null) return default;

            // 初始化
            var siteId = configEntity.Value.ParseToLong();
            var updateUser = "ERP";
            var updateTime = HymsonClock.Now();

            var resposeBo = new SyncSupplierSummaryBo();

            // 判断是否有不存在的供应商编码
            var supplierCodes = lineDtoDict.Select(s => s.Code).Distinct();
            var supplierEntities = await _whSupplierRepository.GetByCodesAsync(new WhSuppliersByCodeQuery { SiteId = siteId, Codes = supplierCodes });
            if (supplierEntities == null || supplierEntities.Any())
            {
                // 这里应该提示供应商不存在
                return resposeBo;
            }

            // 遍历数据
            foreach (var supplierDto in lineDtoDict)
            {
                var supplierEntity = supplierEntities.FirstOrDefault(f => f.Code == supplierDto.Code);

                // 不存在的新供应商
                if (supplierEntity == null)
                {
                    supplierEntity = new WhSupplierEntity
                    {
                        Code = supplierDto.Code,
                        Name = supplierDto.Name,
                        Remark = "",

                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = siteId,
                        CreatedBy = updateUser,
                        CreatedOn = updateTime,
                        UpdatedBy = updateUser,
                        UpdatedOn = updateTime
                    };

                    // 添加供应商
                    resposeBo.Adds.Add(supplierEntity);
                }
                // 之前已存在的供应商
                else
                {
                    supplierEntity.Name = supplierDto.Name;
                    supplierEntity.Remark = "";

                    supplierEntity.UpdatedBy = updateUser;
                    supplierEntity.UpdatedOn = updateTime;
                    resposeBo.Updates.Add(supplierEntity);
                }
            }

            return resposeBo;
        }

    }

    /// <summary>
    /// 同步信息BO对象（供应商）
    /// </summary>
    public class SyncSupplierSummaryBo
    {
        /// <summary>
        /// 新增（供应商）
        /// </summary>
        public List<WhSupplierEntity> Adds { get; set; } = new();
        /// <summary>
        /// 更新（供应商）
        /// </summary>
        public List<WhSupplierEntity> Updates { get; set; } = new();
    }
}
