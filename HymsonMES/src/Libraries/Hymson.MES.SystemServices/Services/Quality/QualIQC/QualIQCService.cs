using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Core.Domain.WHMaterialReceipt;
using Hymson.MES.Core.Domain.WHMaterialReceiptDetail;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;
using Hymson.MES.CoreServices.Services.Quality;
using Hymson.MES.Data.Repositories.Common;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Quality;
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
        /// 服务接口（检验单生成）
        /// </summary>
        private readonly IIQCOrderCreateService _iqcOrderCreateService;

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
        /// 仓储接口（iqc检验单）
        /// </summary>
        private readonly IQualIqcOrderLiteRepository _qualIqcOrderLiteRepository;

        /// <summary>
        /// 仓储接口（iqc检验单明细）
        /// </summary>
        private readonly IQualIqcOrderLiteDetailRepository _qualIqcOrderLiteDetailRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="sysConfigRepository"></param>
        /// <param name="iqcOrderCreateService"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="whSupplierRepository"></param>
        /// <param name="whMaterialReceiptRepository"></param>
        /// <param name="qualIqcOrderLiteRepository"></param>
        /// <param name="qualIqcOrderLiteDetailRepository"></param>
        public QualIQCService(ILogger<QualIQCService> logger,
            ISysConfigRepository sysConfigRepository,
            IIQCOrderCreateService iqcOrderCreateService,
            IProcMaterialRepository procMaterialRepository,
            IWhSupplierRepository whSupplierRepository,
            IWhMaterialReceiptRepository whMaterialReceiptRepository,
            IQualIqcOrderLiteRepository qualIqcOrderLiteRepository,
            IQualIqcOrderLiteDetailRepository qualIqcOrderLiteDetailRepository)
        {
            _logger = logger;
            _sysConfigRepository = sysConfigRepository;
            _iqcOrderCreateService = iqcOrderCreateService;
            _procMaterialRepository = procMaterialRepository;
            _whSupplierRepository = whSupplierRepository;
            _whMaterialReceiptRepository = whMaterialReceiptRepository;
            _qualIqcOrderLiteRepository = qualIqcOrderLiteRepository;
            _qualIqcOrderLiteDetailRepository = qualIqcOrderLiteDetailRepository;
        }


        /// <summary>
        /// 提交（来料检验）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<int> SubmitIncomingAsync(WhMaterialReceiptDto dto)
        {
            if (dto == null || dto.Details == null) throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            if (!dto.Details.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES16602));

            var configEntities = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.MainSite });
            if (configEntities == null || !configEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10139)).WithData("name", SysConfigEnum.MainSite.GetDescription());
            }

            // 判断是否存在（配置）
            var configEntity = configEntities.FirstOrDefault()
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10104));

            // 初始化
            var siteId = configEntity.Value.ParseToLong();
            var updateUser = "ERP";
            var updateTime = HymsonClock.Now();

            // 判断是否有传入供应商代码
            if (string.IsNullOrWhiteSpace(dto.SupplierCode))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES152015));
            }

            // 读取供应商
            var supplierEntities = await _whSupplierRepository.GetByCodesAsync(new WhSuppliersByCodeQuery
            {
                SiteId = siteId,
                Codes = new List<string> { dto.SupplierCode }
            });
            if (supplierEntities == null || !supplierEntities.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES152015));

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

                detail.Id = item.ReceiptDetailId ?? IdGenProvider.Instance.CreateId();
                detail.MaterialReceiptId = entity.Id;
                detail.MaterialId = materialEntity.Id;
                detail.CreatedBy = updateUser;
                detail.CreatedOn = updateTime;
                detail.UpdatedBy = updateUser;
                detail.UpdatedOn = updateTime;
                detail.SiteId = siteId;

                details.Add(detail);
            }

            // 生成检验单号
            var inspectionOrder = await _iqcOrderCreateService.GenerateCommonIQCOrderCodeAsync(new CoreServices.Bos.Common.BaseBo
            {
                SiteId = siteId,
                User = updateUser
            });

            // 检验单
            var orderEntity = new QualIqcOrderLiteEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = entity.SiteId,
                InspectionOrder = inspectionOrder,
                MaterialReceiptId = entity.Id,
                SupplierId = entity.SupplierId,
                Status = IQCLiteStatusEnum.WaitInspect,
                IsQualified = null,
                CreatedBy = updateUser,
                CreatedOn = updateTime
            };

            // 检验单明细
            var orderDetailEntities = details.Select(s => new QualIqcOrderLiteDetailEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = entity.SiteId,
                IQCOrderId = orderEntity.Id,
                MaterialReceiptDetailId = s.Id,
                MaterialId = s.MaterialId,
                IsQualified = null,
                CreatedBy = updateUser,
                CreatedOn = updateTime
            });

            // 插入数据
            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();
            await _whMaterialReceiptRepository.InsertAsync(entity);
            // 先删除
            // await _whMaterialReceiptRepository.DeletesDetailByIdAsync(new long[] { entity.Id });
            if (details.Any()) await _whMaterialReceiptRepository.InsertDetailAsync(details);

            // 保存IQC检验单
            rows += await _qualIqcOrderLiteRepository.InsertAsync(orderEntity);
            rows += await _qualIqcOrderLiteDetailRepository.InsertRangeAsync(orderDetailEntities);

            trans.Complete();
            return rows;
        }

    }
}
