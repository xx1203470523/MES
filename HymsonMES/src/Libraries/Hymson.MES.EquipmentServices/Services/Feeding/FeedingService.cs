using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture.ManuFeeding;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.EquipmentServices.Dtos.Feeding;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Hymson.Web.Framework.WorkContext;

namespace Hymson.MES.EquipmentServices.Services.Feeding
{
    /// <summary>
    /// 上卸料服务
    /// @author Czhipu
    /// @date 2023-05-16 04:51:15
    /// </summary>
    public class FeedingService : IFeedingService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ICurrentEquipment _currentEquipment;

        /// <summary>
        /// 仓储（资源维护）
        /// </summary>
        private readonly IProcResourceRepository _procResourceRepository;

        /// <summary>
        /// 仓储（上料/卸料）
        /// </summary>
        private readonly IManuFeedingLiteRepository _manuFeedingLiteRepository;

        /// <summary>
        /// 仓储（上/卸料记录）
        /// </summary>
        private readonly IManuFeedingLiteRecordRepository _manuFeedingLiteRecordRepository;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentEquipment"></param>
        /// <param name="procResourceRepository"></param>
        /// <param name="manuFeedingLiteRepository"></param>
        /// <param name="manuFeedingLiteRecordRepository"></param>
        public FeedingService(ICurrentEquipment currentEquipment,
            IProcResourceRepository procResourceRepository,
            IManuFeedingLiteRepository manuFeedingLiteRepository,
            IManuFeedingLiteRecordRepository manuFeedingLiteRecordRepository)
        {
            _currentEquipment = currentEquipment;
            _procResourceRepository = procResourceRepository;
            _manuFeedingLiteRepository = manuFeedingLiteRepository;
            _manuFeedingLiteRecordRepository = manuFeedingLiteRecordRepository;
        }


        /// <summary>
        /// 上料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task FeedingLoadingAsync(FeedingLoadingDto request)
        {
            var nowTime = HymsonClock.Now();

            // 查询资源
            var resourceEntity = await _procResourceRepository.GetByCodeAsync(new EntityByCodeQuery
            {
                Site = _currentEquipment.SiteId,
                Code = request.ResourceCode,
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES19109)).WithData("Code", request.ResourceCode);

            // 空则取原材料条码解析数据
            decimal qty = 0;
            if (request.Qty.HasValue == true && request.Qty.IsEmpty() == false) qty = request.Qty.Value;
            else
            {
                // TODO 这里需要从条码里面截取初始数量
            }

            // 检查条码是否已被使用
            var codeEntity = await _manuFeedingLiteRepository.GetByCodeAsync(new EntityByCodeQuery
            {
                Site = _currentEquipment.SiteId,
                Code = request.SFC
            });
            if (codeEntity != null) throw new CustomerValidationException(nameof(ErrorCode.MES15502)).WithData("Code", request.SFC);

            // 初始化实体
            var entity = new ManuFeedingLiteEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = _currentEquipment.SiteId,
                CreatedBy = _currentEquipment.Code,
                UpdatedBy = _currentEquipment.Code,
                CreatedOn = nowTime,
                UpdatedOn = nowTime,
                EquipmentId = _currentEquipment.Id ?? 0,
                ResourceId = resourceEntity.Id,
                BarCode = request.SFC,
                InitQty = qty,
                Qty = qty
            };

            using var trans = TransactionHelper.GetTransactionScope();
            await _manuFeedingLiteRepository.InsertAsync(entity);
            await _manuFeedingLiteRecordRepository.InsertAsync(new ManuFeedingLiteRecordEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = entity.SiteId,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn,
                UpdatedBy = entity.UpdatedBy,
                UpdatedOn = entity.UpdatedOn,
                EquipmentId = entity.EquipmentId,
                ResourceId = entity.ResourceId,
                LocalTime = request.LocalTime,
                BarCode = entity.BarCode,
                DirectionType = FeedingDirectionTypeEnum.Load,
                Qty = entity.Qty,
                IsFeedingPoint = request.IsFeedingPoint
            });
            trans.Complete();
        }

        /// <summary>
        /// 卸料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task FeedingUnloadingAsync(FeedingUnloadingDto request)
        {
            var nowTime = HymsonClock.Now();

            // 查询资源
            var resourceEntity = await _procResourceRepository.GetByCodeAsync(new EntityByCodeQuery
            {
                Site = _currentEquipment.SiteId,
                Code = request.ResourceCode,
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES19109)).WithData("Code", request.ResourceCode);

            // 根据条码获取
            var entity = await _manuFeedingLiteRepository.GetByCodeAsync(new EntityByCodeQuery
            {
                Site = _currentEquipment.SiteId,
                Code = request.SFC
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES15503)).WithData("Code", request.SFC);

            entity.UpdatedBy = _currentEquipment.Code;
            entity.UpdatedOn = nowTime;

            // TODO 根据卸载类型做出对应的操作
            switch (request.Type)
            {
                case FeedingUnloadingTypeEnum.UnLoad:
                    entity.Qty = 0;
                    break;
                case FeedingUnloadingTypeEnum.UnLoadAndAbandon:
                    entity.Qty = 0;
                    entity.IsDeleted = entity.Id;
                    break;
                default: break;
            }

            using var trans = TransactionHelper.GetTransactionScope();
            await _manuFeedingLiteRepository.UpdateAsync(entity);
            await _manuFeedingLiteRecordRepository.InsertAsync(new ManuFeedingLiteRecordEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = entity.SiteId,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn,
                UpdatedBy = entity.UpdatedBy,
                UpdatedOn = entity.UpdatedOn,
                EquipmentId = entity.EquipmentId,
                ResourceId = entity.ResourceId,
                LocalTime = request.LocalTime,
                BarCode = entity.BarCode,
                DirectionType = FeedingDirectionTypeEnum.Unload,
                Qty = entity.Qty,
                UnloadingType = request.Type
            });
            trans.Complete();
        }


    }
}
