using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.WhWarehouseShelf;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.WhWareHouse;
using Hymson.MES.Data.Repositories.WhWareHouse.Query;
using Hymson.MES.Data.Repositories.WhWarehouseLocation;
using Hymson.MES.Data.Repositories.WhWarehouseLocation.Query;
using Hymson.MES.Data.Repositories.WhWarehouseRegion;
using Hymson.MES.Data.Repositories.WhWarehouseRegion.Query;
using Hymson.MES.Data.Repositories.WhWarehouseShelf;
using Hymson.MES.Data.Repositories.WhWarehouseShelf.Query;
using Hymson.MES.Services.Dtos.WhWarehouseRegion;
using Hymson.MES.Services.Dtos.WhWarehouseShelf;
using Hymson.Snowflake;
using Hymson.Utils;
using System.Security.Policy;

namespace Hymson.MES.Services.Services.WhWarehouseShelf
{
    /// <summary>
    /// 服务（货架） 
    /// </summary>
    public class WhWarehouseShelfService : IWhWarehouseShelfService
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
        private readonly AbstractValidator<WhWarehouseShelfSaveDto> _validationSaveRules;
        private readonly AbstractValidator<WhWarehouseShelfModifyDto> _validationModifyRules;

        /// <summary>
        /// 仓储接口（货架）
        /// </summary>
        private readonly IWhWarehouseShelfRepository _whWarehouseShelfRepository;
        private readonly IWhWarehouseRepository _whWarehouseRepository;
        private readonly IWhWarehouseRegionRepository _whWarehouseRegionRepository;
        private readonly IWhWarehouseLocationRepository _whWarehouseLocationRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="whWarehouseShelfRepository"></param>
        /// <param name="whWarehouseRepository"></param>
        /// <param name="whWarehouseRegionRepository"></param>
        /// <param name="whWarehouseLocationRepository"></param>
        /// <param name="validationModifyRules"></param>
        public WhWarehouseShelfService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<WhWarehouseShelfSaveDto> validationSaveRules,
            IWhWarehouseShelfRepository whWarehouseShelfRepository, IWhWarehouseRepository whWarehouseRepository, IWhWarehouseRegionRepository whWarehouseRegionRepository, IWhWarehouseLocationRepository whWarehouseLocationRepository, AbstractValidator<WhWarehouseShelfModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _whWarehouseShelfRepository = whWarehouseShelfRepository;
            _whWarehouseRepository = whWarehouseRepository;
            _whWarehouseRegionRepository = whWarehouseRegionRepository;
            _whWarehouseLocationRepository = whWarehouseLocationRepository;
            _validationModifyRules = validationModifyRules;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(WhWarehouseShelfSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10101));
            }

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            //获取仓库
            var warehouseEntity = await _whWarehouseRepository.GetOneAsync(new WhWarehouseQuery { Code = saveDto.WarehouseCode,SiteId= _currentSite.SiteId??0 });
            if (warehouseEntity == null) {
                throw new CustomerValidationException(nameof(ErrorCode.MES19224));
            }

            //获取库区
            var warehouseRegionEntity = await _whWarehouseRegionRepository.GetOneAsync(new WhWarehouseRegionQuery { Code = saveDto.WarehouseRegionCode,SiteId= _currentSite.SiteId });
            if (warehouseRegionEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19225));
            }

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<WhWarehouseShelfEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.WarehouseId= warehouseEntity.Id;
            entity.WarehouseRegionId= warehouseRegionEntity.Id;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存
            var result = await _whWarehouseShelfRepository.InsertIgnoreAsync(entity);
            if (result == 0) {
                throw new CustomerValidationException(nameof(ErrorCode.MES19207)).WithData("code", saveDto.Code);
            }
            return result;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(WhWarehouseShelfModifyDto modifyDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10101));
            }

            // 验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(modifyDto);

            // DTO转换实体
            var entity = modifyDto.ToEntity<WhWarehouseShelfEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();
            entity.SiteId = _currentSite.SiteId ?? 0;

            return await _whWarehouseShelfRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _whWarehouseShelfRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            var warehouseLocationEntities = await _whWarehouseLocationRepository.GetEntitiesAsync(new WhWarehouseLocationQuery {WarehouseShelfIds= ids });
            if (warehouseLocationEntities != null && warehouseLocationEntities.Any()) {
                throw new CustomerValidationException(nameof(ErrorCode.MES19218));
            }

            return await _whWarehouseShelfRepository.DeletesAsync(new DeleteCommand
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
        public async Task<WhWarehouseShelfDto?> QueryByIdAsync(long id)
        {
            var whWarehouseShelfEntity = await _whWarehouseShelfRepository.GetOneAsync(new WhWarehouseShelfQuery { Id = id, SiteId = _currentSite.SiteId ?? 0 });
            if (whWarehouseShelfEntity == null)
            {
                return null;
            }
            var result = whWarehouseShelfEntity.ToModel<WhWarehouseShelfDto>();

            var wareHouseEntity = await _whWarehouseRepository.GetOneAsync(new WhWarehouseQuery { Id = whWarehouseShelfEntity.WarehouseId, SiteId = _currentSite.SiteId ?? 0 });
            result.WarehouseCode = wareHouseEntity?.Code;
            result.WarehouseName= wareHouseEntity?.Name;

            var wareHouseRegionEntity = await _whWarehouseRegionRepository.GetOneAsync(new WhWarehouseRegionQuery { Id = whWarehouseShelfEntity.WarehouseRegionId, SiteId = _currentSite.SiteId??0 });
            result.WarehouseRegionCode= wareHouseRegionEntity?.Code;
            result.WarehouseRegionName= wareHouseRegionEntity?.Name;

            return result;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<WhWarehouseShelfDto>> GetPagedListAsync(WhWarehouseShelfPagedQueryDto pagedQueryDto)
        {
            //查询仓库
            var returnData = new PagedInfo<WhWarehouseShelfDto>(Enumerable.Empty<WhWarehouseShelfDto>(), 0, 0, 0);
            var wareHouseQuery = new WhWarehouseQuery();
            if (!string.IsNullOrWhiteSpace(pagedQueryDto.WareHouseCode))
            {
                wareHouseQuery.CodeLike = pagedQueryDto.WareHouseCode;
            }
            wareHouseQuery.SiteId = _currentSite.SiteId ?? 0;
            var wareHouseEntities = await _whWarehouseRepository.GetEntitiesAsync(wareHouseQuery);
            if (wareHouseEntities == null || !wareHouseEntities.Any())
            {
                return returnData;
            }
            var wareHouseIds = wareHouseEntities.Select(x => x.Id);

            //查询库区
            var warehouseRegionQuery = new WhWarehouseRegionQuery();
            warehouseRegionQuery.WarehouseIds = wareHouseIds;
            if (!string.IsNullOrWhiteSpace(pagedQueryDto.WareHouseRegionCode))
            {
                warehouseRegionQuery.CodeLike = pagedQueryDto.WareHouseRegionCode;
            }
            var wareHouseRegionEntities = await _whWarehouseRegionRepository.GetEntitiesAsync(warehouseRegionQuery);
            if (wareHouseRegionEntities == null || !wareHouseRegionEntities.Any())
            {
                return returnData;
            }
            var warehouseRegionIds = wareHouseRegionEntities.Select(x => x.Id);

            var pagedQuery = pagedQueryDto.ToQuery<WhWarehouseShelfPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            pagedQuery.WarehouseIds = wareHouseIds;
            pagedQuery.WarehouseRegionIds = warehouseRegionIds;
            var pagedInfo = await _whWarehouseShelfRepository.GetPagedListAsync(pagedQuery);
            if (pagedInfo.Data == null || !pagedInfo.Data.Any())
            {
                return returnData;
            }

            var result = new List<WhWarehouseShelfDto>();
            foreach (var item in pagedInfo.Data)
            {
                var model = item.ToModel<WhWarehouseShelfDto>();
                model.WarehouseShelfCode = model.Code;

                var wareHouseEntity = wareHouseEntities.FirstOrDefault(a => a.Id == item.WarehouseId);
                var wareHouseRegionEntity = wareHouseRegionEntities.FirstOrDefault(a => a.Id == item.WarehouseRegionId);
                if (wareHouseEntity != null && wareHouseRegionEntity != null)
                {
                    model.WarehouseCode = wareHouseEntity.Code;
                    model.WarehouseName = wareHouseEntity.Name;
                    model.WarehouseRegionCode = wareHouseRegionEntity.Code;
                    model.WarehouseRegionName= wareHouseRegionEntity.Name;
                    result.Add(model);
                }
            }

            // 实体到DTO转换 装载数据
            //var dtos = pagedInfo.Data.Select(s => s.ToModel<WhWarehouseShelfDto>());
            return new PagedInfo<WhWarehouseShelfDto>(result, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

    }
}
