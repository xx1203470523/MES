using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.Query;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 服务（废成品入库记录） 
    /// </summary>
    public class ManuWasteProductsReceiptRecordService : IManuWasteProductsReceiptRecordService
    {
        /// <summary>
        /// 当前用户
        /// </summary>
        private readonly ICurrentUser _currentUser;
        /// <summary>
        /// 当前站点
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 参数验证器
        /// </summary>
        private readonly AbstractValidator<ManuWasteProductsReceiptRecordSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（废成品入库记录）
        /// </summary>
        private readonly IManuWasteProductsReceiptRecordRepository _manuWasteProductsReceiptRecordRepository;

        private readonly IManuWasteProductsReceiptRecordDetailRepository _manuWasteProductsReceiptRecordDetailRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="manuWasteProductsReceiptRecordRepository"></param>
        /// <param name="manuWasteProductsReceiptRecordDetailRepository"></param>
        public ManuWasteProductsReceiptRecordService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<ManuWasteProductsReceiptRecordSaveDto> validationSaveRules, 
            IManuWasteProductsReceiptRecordRepository manuWasteProductsReceiptRecordRepository, IManuWasteProductsReceiptRecordDetailRepository manuWasteProductsReceiptRecordDetailRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _manuWasteProductsReceiptRecordRepository = manuWasteProductsReceiptRecordRepository;
            _manuWasteProductsReceiptRecordDetailRepository = manuWasteProductsReceiptRecordDetailRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(ManuWasteProductsReceiptRecordSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<ManuWasteProductsReceiptRecordEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存
            return await _manuWasteProductsReceiptRecordRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(ManuWasteProductsReceiptRecordSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

             // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<ManuWasteProductsReceiptRecordEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            return await _manuWasteProductsReceiptRecordRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _manuWasteProductsReceiptRecordRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            return await _manuWasteProductsReceiptRecordRepository.DeletesAsync(new DeleteCommand
            {
                Ids = ids,
                DeleteOn = HymsonClock.Now(),
                UserId = _currentUser.UserName
            });
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuWasteProductsReceiptRecordDto?> QueryByIdAsync(long id) 
        {
           var manuWasteProductsReceiptRecordEntity = await _manuWasteProductsReceiptRecordRepository.GetByIdAsync(id);
           if (manuWasteProductsReceiptRecordEntity == null) return null;
           
           return manuWasteProductsReceiptRecordEntity.ToModel<ManuWasteProductsReceiptRecordDto>();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuWasteProductsReceiptRecordDto>> GetPagedListAsync(ManuWasteProductsReceiptRecordPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<ManuWasteProductsReceiptRecordPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _manuWasteProductsReceiptRecordRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<ManuWasteProductsReceiptRecordDto>());
            return new PagedInfo<ManuWasteProductsReceiptRecordDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }


        /// <summary>
        /// 查询副产品入库记录
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ManuWasteProductsReceiptRecordDto>> QueryWasteProductsReceiptInfoListAsync()
        {
            var manuProductReceiptOrders = new List<ManuWasteProductsReceiptRecordDto>();
            var manuProductReceiptOrderLists = await _manuWasteProductsReceiptRecordRepository.GetEntitiesAsync(new ManuWasteProductsReceiptRecordQuery { SiteId = _currentSite.SiteId ?? 123456 });
            if (manuProductReceiptOrderLists.Any() == false)
            {
                return manuProductReceiptOrders;
            }
            var manuProductReceiptOrderIds = manuProductReceiptOrderLists.Select(x => x.Id).ToArray();
            var productReceiptOrderDetailEntities = await _manuWasteProductsReceiptRecordDetailRepository.GetByIdsAsync(manuProductReceiptOrderIds);
            if (productReceiptOrderDetailEntities.Any())
            {
                foreach (var entity in productReceiptOrderDetailEntities)
                {
                    var manuProductReceiptOrder = manuProductReceiptOrderLists.FirstOrDefault(x => x.Id == entity.ProductReceiptId);
                    var manuProductReceipt = new ManuWasteProductsReceiptRecordDto
                    {
                        CompletionOrderCode = manuProductReceiptOrder?.CompletionOrderCode ?? "",
                        Status = manuProductReceiptOrder?.Status ?? ProductReceiptStatusEnum.Approvaling,
                        Qty = entity.Qty,
                        Unit = entity.Unit,
                        CreatedOn = entity.CreatedOn,
                        CreatedBy = entity.CreatedBy,
                        MaterialCode = entity.MaterialCode,
                        MaterialName = entity.MaterialName,
                        WarehouseOrderCode = manuProductReceiptOrder?.WarehouseOrderCode ?? "",
                    };
                    manuProductReceiptOrders.Add(manuProductReceipt);
                }
            }
            return manuProductReceiptOrders;
        }


    }
}
