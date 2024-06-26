using Hymson.MES.Core.Domain.Common;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.SystemServices.Dtos;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.Extensions.Logging;

namespace Hymson.MES.SystemServices.Services.Integrated
{
    /// <summary>
    /// 服务（客户）
    /// </summary>
    public class InteCustomerService : IInteCustomerService
    {
        /// <summary>
        /// 日志对象
        /// </summary>
        private readonly ILogger<InteCustomerService> _logger;

        /// <summary>
        /// 仓储接口（系统配置）
        /// </summary>
        private readonly ISysConfigRepository _sysConfigRepository;

        /// <summary>
        /// 仓储接口（客户）
        /// </summary>
        private readonly IInteCustomRepository _inteCustomRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="sysConfigRepository"></param>
        /// <param name="inteCustomRepository"></param>
        public InteCustomerService(ILogger<InteCustomerService> logger,
            ISysConfigRepository sysConfigRepository,
            IInteCustomRepository inteCustomRepository)
        {
            _logger = logger;
            _sysConfigRepository = sysConfigRepository;
            _inteCustomRepository = inteCustomRepository;
        }

        /// <summary>
        /// 同步信息（客户）
        /// </summary>
        /// <param name="requestDtos"></param>
        /// <returns></returns>
        public async Task<int> SyncCustomerAsync(IEnumerable<InteCustomerDto> requestDtos)
        {
            if (requestDtos == null || !requestDtos.Any()) return 0;

            var configEntities = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.ERPSite });
            if (configEntities == null || !configEntities.Any()) return 0;

            var resposeBo = await ConvertCustomerListAsync(configEntities.FirstOrDefault(), requestDtos);
            if (resposeBo == null) return 0;

            // 添加到集合
            var resposeSummaryBo = new SyncCustomerSummaryBo();
            resposeSummaryBo.Adds.AddRange(resposeBo.Adds);
            resposeSummaryBo.Updates.AddRange(resposeBo.Updates);

            // 插入数据
            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();
            rows += await _inteCustomRepository.InsertsAsync(resposeSummaryBo.Adds);
            rows += await _inteCustomRepository.UpdatesAsync(resposeSummaryBo.Updates);
            trans.Complete();
            return rows;
        }

        /// <summary>
        /// 转换信息集合（客户）
        /// </summary>
        /// <param name="configEntity"></param>
        /// <param name="lineDtoDict"></param>
        /// <returns></returns>
        private async Task<SyncCustomerSummaryBo?> ConvertCustomerListAsync(SysConfigEntity? configEntity, IEnumerable<InteCustomerDto> lineDtoDict)
        {
            // 判断是否存在（配置）
            if (configEntity == null) return default;

            // 初始化
            var siteId = configEntity.Value.ParseToLong();
            var updateUser = "ERP";
            var updateTime = HymsonClock.Now();

            var resposeBo = new SyncCustomerSummaryBo();

            // 判断是否有不存在的供应商编码
            var customerCodes = lineDtoDict.Select(s => s.Code).Distinct();
            var customerEntities = await _inteCustomRepository.GetEntitiesAsync(new InteCustomQuery { SiteId = siteId, Codes = customerCodes });
            if (customerEntities == null || customerEntities.Any())
            {
                // 这里应该提示供应商不存在
                return resposeBo;
            }

            // 遍历数据
            foreach (var customerDto in lineDtoDict)
            {
                var customerEntity = customerEntities.FirstOrDefault(f => f.Code == customerDto.Code);

                // 不存在的新供应商
                if (customerEntity == null)
                {
                    customerEntity = new InteCustomEntity
                    {
                        Code = customerDto.Code,
                        Name = customerDto.Name,
                        Describe = customerDto.Describe,
                        Address = customerDto.Address,
                        Telephone = customerDto.Telephone,

                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = siteId,
                        CreatedBy = updateUser,
                        CreatedOn = updateTime,
                        UpdatedBy = updateUser,
                        UpdatedOn = updateTime
                    };

                    // 添加供应商
                    resposeBo.Adds.Add(customerEntity);
                }
                // 之前已存在的供应商
                else
                {
                    customerEntity.Name = customerDto.Name;
                    customerEntity.Describe = customerDto.Describe;
                    customerEntity.Address = customerDto.Address;
                    customerEntity.Telephone = customerDto.Telephone;

                    customerEntity.UpdatedBy = updateUser;
                    customerEntity.UpdatedOn = updateTime;
                    resposeBo.Updates.Add(customerEntity);
                }
            }

            return resposeBo;
        }

    }

    /// <summary>
    /// 同步信息BO对象（客户）
    /// </summary>
    public class SyncCustomerSummaryBo
    {
        /// <summary>
        /// 新增（客户）
        /// </summary>
        public List<InteCustomEntity> Adds { get; set; } = new();
        /// <summary>
        /// 更新（客户）
        /// </summary>
        public List<InteCustomEntity> Updates { get; set; } = new();
    }
}
