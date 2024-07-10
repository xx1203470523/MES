using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.WHMaterialReceipt;
using Hymson.MES.Core.Domain.WHMaterialReceiptDetail;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Query;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.WHMaterialReceipt;
using Hymson.MES.SystemServices.Dtos;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.Extensions.Logging;

namespace Hymson.MES.SystemServices.Services.Quality
{
    /// <summary>
    /// 服务（物料）
    /// </summary>
    public class QualIQCService : IQualIQCService
    {
        /// <summary>
        /// 日志对象
        /// </summary>
        private readonly ILogger<QualIQCService> _logger;

        /// <summary>
        /// 仓储接口（系统配置）
        /// </summary>
        private readonly ISysConfigRepository _sysConfigRepository;

        /// <summary>
        /// 仓储接口（物料）
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 仓储接口（供应商）
        /// </summary>
        private readonly IWhSupplierRepository _whSupplierRepository;

        /// <summary>
        /// 仓储接口（物料收货表）
        /// </summary>
        private readonly IWhMaterialReceiptRepository _whMaterialReceiptRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="sysConfigRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="whSupplierRepository"></param>
        /// <param name="whMaterialReceiptRepository"></param>
        public QualIQCService(ILogger<QualIQCService> logger,
            ISysConfigRepository sysConfigRepository,
            IProcMaterialRepository procMaterialRepository,
            IWhSupplierRepository whSupplierRepository,
            IWhMaterialReceiptRepository whMaterialReceiptRepository)
        {
            _logger = logger;
            _sysConfigRepository = sysConfigRepository;
            _procMaterialRepository = procMaterialRepository;
            _whSupplierRepository = whSupplierRepository;
            _whMaterialReceiptRepository = whMaterialReceiptRepository;
        }


        /// <summary>
        /// 提交（来料检验）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<int> SubmitIncomingAsync(WhMaterialReceiptDto dto)
        {
            if (dto == null || dto.Details == null) return 0;
            if (!dto.Details.Any()) return 0;

            var configEntities = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.MainSite });
            if (configEntities == null || !configEntities.Any()) return 0;

            var configEntity = configEntities.FirstOrDefault();

            // 判断是否存在（配置）
            if (configEntity == null) return default;

            // 初始化
            var siteId = configEntity.Value.ParseToLong();
            var updateUser = "ERP";
            var updateTime = HymsonClock.Now();

            // 判断是否有传入供应商代码
            if (string.IsNullOrWhiteSpace(dto.SupplierCode))
            {
                // 供应商代码不能为空
                return 0;
            }

            // 读取供应商
            var supplierEntities = await _whSupplierRepository.GetByCodesAsync(new WhSuppliersByCodeQuery
            {
                SiteId = siteId,
                Codes = new List<string> { dto.SupplierCode }
            });
            if (supplierEntities == null || !supplierEntities.Any()) return 0;

            // 读取物料
            var materialEntities = await _procMaterialRepository.GetByCodesAsync(new ProcMaterialsByCodeQuery
            {
                SiteId = siteId,
                MaterialCodes = dto.Details.Select(s => s.MaterialCode)
            });

            // DTO转换实体
            var entity = dto.ToEntity<WhMaterialReceiptEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updateUser;
            entity.CreatedOn = updateTime;
            entity.UpdatedBy = updateUser;
            entity.UpdatedOn = updateTime;
            entity.SiteId = siteId;
            entity.SupplierId = supplierEntities.FirstOrDefault()?.Id ?? 0;

            // 是否重复收货单号
            var isHasReceipt = await _whMaterialReceiptRepository.GetEntitiesAsync(new WhMaterialReceiptQuery { ReceiptNum = entity.ReceiptNum });
            if (isHasReceipt.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES19227)).WithData("ReceiptNum", entity.ReceiptNum);

            List<WHMaterialReceiptDetailEntity> details = new();
            foreach (var item in dto.Details)
            {
                var materialEntity = materialEntities.FirstOrDefault(f => f.MaterialCode == item.MaterialCode);
                if (materialEntity == null) continue;

                var detail = item.ToEntity<WHMaterialReceiptDetailEntity>();
                if (detail == null) continue;

                detail.Id = IdGenProvider.Instance.CreateId();
                detail.MaterialReceiptId = entity.Id;
                detail.MaterialId = materialEntity.Id;
                detail.CreatedBy = updateUser;
                detail.CreatedOn = updateTime;
                detail.UpdatedBy = updateUser;
                detail.UpdatedOn = updateTime;
                detail.SiteId = siteId;

                details.Add(detail);
            }

            // 插入数据
            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();
            await _whMaterialReceiptRepository.InsertAsync(entity);
            // 先删除
            // await _whMaterialReceiptRepository.DeletesDetailByIdAsync(new long[] { entity.Id });
            if (details.Any()) await _whMaterialReceiptRepository.InsertDetailAsync(details);
            trans.Complete();
            return rows;
        }

    }
}
