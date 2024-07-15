/*
 *creator: Karl
 *
 *describe: 物料台账    服务 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-13 10:03:29
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuFeeding;
using Hymson.MES.Data.Repositories.Manufacture.ManuFeeding.Query;
using Hymson.MES.Data.Repositories.Manufacture.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Services.Dtos.Warehouse;
using Hymson.Snowflake;
using Hymson.Utils;
using System.Runtime.Versioning;

namespace Hymson.MES.Services.Services.Warehouse
{
    /// <summary>
    /// 物料台账 服务
    /// </summary>
    public class WhMaterialStandingbookService : IWhMaterialStandingbookService
    {
        private readonly ICurrentUser _currentUser;

        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 物料台账 仓储
        /// </summary>
        private readonly IWhMaterialStandingbookRepository _whMaterialStandingbookRepository;
        private readonly IManuFeedingRecordRepository _manuFeedingRecordRepository;
        private readonly AbstractValidator<WhMaterialStandingbookCreateDto> _validationCreateRules;
        private readonly AbstractValidator<WhMaterialStandingbookModifyDto> _validationModifyRules;

        private readonly IWhSupplierRepository _whSupplierRepository;

        private readonly IProcResourceRepository _procResourceRepository;

        private readonly IProcLoadPointRepository _procLoadPointRepository;

        private readonly IManuBarCodeRelationRepository _manuBarCodeRelationRepository;

        public WhMaterialStandingbookService(ICurrentUser currentUser, ICurrentSite currentSite,
                  IWhMaterialStandingbookRepository whMaterialStandingbookRepository,
                  AbstractValidator<WhMaterialStandingbookCreateDto> validationCreateRules,
                  AbstractValidator<WhMaterialStandingbookModifyDto> validationModifyRules,
                  IWhSupplierRepository whSupplierRepository,
                  IManuFeedingRecordRepository manuFeedingRecordRepository,
                  IProcResourceRepository procResourceRepository,
                  IProcLoadPointRepository procLoadPointRepository,
                  IManuBarCodeRelationRepository manuBarCodeRelationRepository)
        {
            _currentUser = currentUser;
            _whMaterialStandingbookRepository = whMaterialStandingbookRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _currentSite = currentSite;
            _whSupplierRepository = whSupplierRepository;
            _manuFeedingRecordRepository = manuFeedingRecordRepository;
            _procResourceRepository = procResourceRepository;
            _procLoadPointRepository = procLoadPointRepository;
            _manuBarCodeRelationRepository = manuBarCodeRelationRepository;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="whMaterialStandingbookCreateDto"></param>
        /// <returns></returns>
        public async Task CreateWhMaterialStandingbookAsync(WhMaterialStandingbookCreateDto whMaterialStandingbookCreateDto)
        {
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(whMaterialStandingbookCreateDto);

            //DTO转换实体
            var whMaterialStandingbookEntity = whMaterialStandingbookCreateDto.ToEntity<WhMaterialStandingbookEntity>();
            whMaterialStandingbookEntity.Id = IdGenProvider.Instance.CreateId();
            whMaterialStandingbookEntity.CreatedBy = _currentUser.UserName;
            whMaterialStandingbookEntity.UpdatedBy = _currentUser.UserName;
            whMaterialStandingbookEntity.CreatedOn = HymsonClock.Now();
            whMaterialStandingbookEntity.UpdatedOn = HymsonClock.Now();

            //入库
            await _whMaterialStandingbookRepository.InsertAsync(whMaterialStandingbookEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteWhMaterialStandingbookAsync(long id)
        {
            await _whMaterialStandingbookRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesWhMaterialStandingbookAsync(string ids)
        {
            var idsArr = ids.ToSpitLongArray();
            return await _whMaterialStandingbookRepository.DeletesAsync(idsArr);
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="whMaterialStandingbookPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<WhMaterialStandingbookDto>> GetPageListAsync(WhMaterialStandingbookPagedQueryDto whMaterialStandingbookPagedQueryDto)
        {
            var whMaterialStandingbookPagedQuery = whMaterialStandingbookPagedQueryDto.ToQuery<WhMaterialStandingbookPagedQuery>();
            whMaterialStandingbookPagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _whMaterialStandingbookRepository.GetPagedInfoAsync(whMaterialStandingbookPagedQuery);

            //查询供应商
            var suppliers = await _whSupplierRepository.GetByIdsAsync(pagedInfo.Data.Select(x => x.SupplierId).ToArray());

            //实体到DTO转换 装载数据
            List<WhMaterialStandingbookDto> whMaterialStandingbookDtos = PrepareWhMaterialStandingbookDtos(pagedInfo);

            foreach (var item in whMaterialStandingbookDtos)
            {
                if (item.SupplierId > 0)
                {
                    item.SupplierCode = suppliers.FirstOrDefault(x => x.Id == item.SupplierId)?.Code ?? "";
                }
            }

            return new PagedInfo<WhMaterialStandingbookDto>(whMaterialStandingbookDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<WhMaterialStandingbookDto> PrepareWhMaterialStandingbookDtos(PagedInfo<WhMaterialStandingbookEntity> pagedInfo)
        {
            var whMaterialStandingbookDtos = new List<WhMaterialStandingbookDto>();

            foreach (var whMaterialStandingbookEntity in pagedInfo.Data)
            {

                var whMaterialStandingbookDto = whMaterialStandingbookEntity.ToModel<WhMaterialStandingbookDto>();
                whMaterialStandingbookDtos.Add(whMaterialStandingbookDto);
            }

            return whMaterialStandingbookDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="whMaterialStandingbookModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyWhMaterialStandingbookAsync(WhMaterialStandingbookModifyDto whMaterialStandingbookModifyDto)
        {
            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(whMaterialStandingbookModifyDto);

            //DTO转换实体
            var whMaterialStandingbookEntity = whMaterialStandingbookModifyDto.ToEntity<WhMaterialStandingbookEntity>();
            whMaterialStandingbookEntity.UpdatedBy = _currentUser.UserName;
            whMaterialStandingbookEntity.UpdatedOn = HymsonClock.Now();

            await _whMaterialStandingbookRepository.UpdateAsync(whMaterialStandingbookEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<WhMaterialStandingbookDto> QueryWhMaterialStandingbookByIdAsync(long id)
        {
            var whMaterialStandingbookEntity = await _whMaterialStandingbookRepository.GetByIdAsync(id);
            if (whMaterialStandingbookEntity != null)
            {
                return whMaterialStandingbookEntity.ToModel<WhMaterialStandingbookDto>();
            }
            return new WhMaterialStandingbookDto();
        }

        /// <summary>
        /// 获取绑定信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<WhMaterialStandingBookRelationDto> > GetWhMaterialStandingBookRelationByIdAsync(long id)
        {
            var whMaterialStandingbookEntity = await _whMaterialStandingbookRepository.GetByIdAsync(id);
            var whMaterialStandingBookRelationEntities = new List<WhMaterialStandingBookRelationDto> ();
            IEnumerable<ManuBarCodeRelationEntity> manubarcoderelationenties = new List<ManuBarCodeRelationEntity>() { };
            switch (whMaterialStandingbookEntity.Type)
            {
                case WhMaterialInventoryTypeEnum.MaterialBarCodeSplit:
                    manubarcoderelationenties = await _manuBarCodeRelationRepository.GetEntitiesAsync(new ManuBarcodeRelationQuery
                    {
                        InputMaterialStandingBookId = id,
                        SiteId = _currentSite.SiteId ?? 0,
                    });
                    break;
                case WhMaterialInventoryTypeEnum.MaterialBarCodeMerge:
                    manubarcoderelationenties = await _manuBarCodeRelationRepository.GetEntitiesAsync(new ManuBarcodeRelationQuery
                    {
                        OutputMaterialStandingBookId = id,
                        SiteId = _currentSite.SiteId ?? 0,
                    });
                    break;
            }

            foreach (var item in manubarcoderelationenties)
            {
                whMaterialStandingBookRelationEntities.Add(new WhMaterialStandingBookRelationDto()
                {
                    ParentBarcode = item.InputBarCode,
                    ChildrenParentBarcode = item.OutputBarCode,
                    Qty = item.InputQty
                });
            }
            return whMaterialStandingBookRelationEntities;
        }

        /// <summary>
        /// 上料信息表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<WhMaterialStandingBookFeedingDto> GetWhMaterialStandingBookFeedingByIdAsync(long id)
        {
            var manuFeedingRecordEntity = await _manuFeedingRecordRepository.GetEntity(
                new ManuFeedingRecordQuery
                {
                    SiteId = _currentSite.SiteId ?? 0,
                    MaterialStandingbookId = id
                }
            );
            var procResourceEntity = await _procResourceRepository.GetByIdAsync(manuFeedingRecordEntity.ResourceId);
            var whMaterialStandingBookFeeding = new WhMaterialStandingBookFeedingDto()
            {
                ResourceCode = procResourceEntity.ResCode,
                ResourceName = procResourceEntity.ResName,
                Qty = manuFeedingRecordEntity.Qty ?? 0
            };
            if (manuFeedingRecordEntity.FeedingPointId.HasValue)
            {
                var procLoadPointEntity = await _procLoadPointRepository.GetByIdAsync(manuFeedingRecordEntity.FeedingPointId??0);
                whMaterialStandingBookFeeding.LoadingPointCode = procLoadPointEntity.LoadPoint;
                whMaterialStandingBookFeeding.LoadingPointName = procLoadPointEntity.LoadPointName;
            }

            return whMaterialStandingBookFeeding;
        }
    }
}
