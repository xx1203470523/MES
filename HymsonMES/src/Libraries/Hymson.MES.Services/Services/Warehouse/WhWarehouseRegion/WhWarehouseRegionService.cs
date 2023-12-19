using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.WhWarehouseRegion;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.WhWareHouse;
using Hymson.MES.Data.Repositories.WhWareHouse.Query;
using Hymson.MES.Data.Repositories.WhWarehouseRegion;
using Hymson.MES.Data.Repositories.WhWarehouseRegion.Query;
using Hymson.MES.Data.Repositories.WhWarehouseShelf;
using Hymson.MES.Data.Repositories.WhWarehouseShelf.Query;
using Hymson.MES.Services.Dtos.WhWarehouseRegion;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.WhWarehouseRegion
{
    /// <summary>
    /// 服务（库区） 
    /// </summary>
    public class WhWarehouseRegionService : IWhWarehouseRegionService
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
        private readonly AbstractValidator<WhWarehouseRegionSaveDto> _validationSaveRules;
        private readonly AbstractValidator<WhWarehouseRegionModifyDto> _validationModifyRules;

        /// <summary>
        /// 仓储接口（库区）
        /// </summary>
        private readonly IWhWarehouseRegionRepository _whWarehouseRegionRepository;
        private readonly IWhWarehouseRepository _whWarehouseRepository;
        private readonly IWhWarehouseShelfRepository _whWarehouseShelfRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="whWarehouseRegionRepository"></param>
        /// <param name="whWarehouseRepository"></param>
        /// <param name="whWarehouseShelfRepository"></param>
        /// <param name="validationModifyRules"></param>
        public WhWarehouseRegionService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<WhWarehouseRegionSaveDto> validationSaveRules,
            IWhWarehouseRegionRepository whWarehouseRegionRepository, IWhWarehouseRepository whWarehouseRepository, IWhWarehouseShelfRepository whWarehouseShelfRepository, AbstractValidator<WhWarehouseRegionModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _whWarehouseRegionRepository = whWarehouseRegionRepository;
            _whWarehouseRepository = whWarehouseRepository;
            _whWarehouseShelfRepository = whWarehouseShelfRepository;
            _validationModifyRules = validationModifyRules;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(WhWarehouseRegionSaveDto saveDto)
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
            if (warehouseEntity==null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19224));
            }

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<WhWarehouseRegionEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.WarehouseId = warehouseEntity.Id;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存
            var result= await _whWarehouseRegionRepository.InsertIgnoreAsync(entity);
            if (result == 0) {
                throw new CustomerValidationException(nameof(ErrorCode.MES19206)).WithData("code", saveDto.Code);
            }
            return result;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(WhWarehouseRegionModifyDto modifyDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10101));
            }

            // 验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(modifyDto);

            // DTO转换实体
            var entity = modifyDto.ToEntity<WhWarehouseRegionEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            return await _whWarehouseRegionRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _whWarehouseRegionRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            var warehouseShelfEntities = await _whWarehouseShelfRepository.GetEntitiesAsync(new WhWarehouseShelfQuery { WarehouseRegionIds= ids });
            if (warehouseShelfEntities != null&& warehouseShelfEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19217));
            }

            return await _whWarehouseRegionRepository.DeletesAsync(new DeleteCommand
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
        public async Task<WhWarehouseRegionDto?> QueryByIdAsync(long id)
        {
            var whWarehouseRegionEntity = await _whWarehouseRegionRepository.GetOneAsync(new WhWarehouseRegionQuery { Id = id, SiteId = _currentSite.SiteId ?? 0 });
            if (whWarehouseRegionEntity == null)
            {
                return null;
            }

            var wareHouseEntity = await _whWarehouseRepository.GetOneAsync(new WhWarehouseQuery { Id = whWarehouseRegionEntity.WarehouseId, SiteId = _currentSite.SiteId ?? 0 });
            var result= whWarehouseRegionEntity.ToModel<WhWarehouseRegionDto>();
            result.WarehouseName = wareHouseEntity.Name;
            result.WarehouseCode= wareHouseEntity.Code;
            return result;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<WhWarehouseRegionDto>> GetPagedListAsync(WhWarehouseRegionPagedQueryDto pagedQueryDto)
            {
            var returnData = new PagedInfo<WhWarehouseRegionDto>(Enumerable.Empty<WhWarehouseRegionDto>(), 0, 0, 0);
            var wareHouseQuery = new WhWarehouseQuery();
            if (!string.IsNullOrWhiteSpace(pagedQueryDto.WareHouseCode))
            {
                wareHouseQuery.CodeLike = pagedQueryDto.WareHouseCode;
            }
            if (!string.IsNullOrWhiteSpace(pagedQueryDto.WareHouseName))
            {
                wareHouseQuery.NameLike = pagedQueryDto.WareHouseName;
            }
            wareHouseQuery.SiteId = _currentSite.SiteId ?? 0;
            var wareHouseEntities = await _whWarehouseRepository.GetEntitiesAsync(wareHouseQuery);
            if (wareHouseEntities == null || !wareHouseEntities.Any())
            {
                return returnData;
            }
            var wareHouseIds = wareHouseEntities.Select(a => a.Id);

            var pagedQuery = pagedQueryDto.ToQuery<WhWarehouseRegionPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            pagedQuery.WareHouseIds = wareHouseIds;
            var pagedInfo = await _whWarehouseRegionRepository.GetPagedListAsync(pagedQuery);
            if (pagedInfo.Data == null || !pagedInfo.Data.Any())
            {
                return returnData;
            }

            var result = new List<WhWarehouseRegionDto>();
            foreach (var item in pagedInfo.Data)
            {
                var model = item.ToModel<WhWarehouseRegionDto>();

                var wareHouseEntity = wareHouseEntities.FirstOrDefault(a => a.Id == model.WarehouseId);
                if (wareHouseEntity != null)
                {
                    model.WarehouseCode = wareHouseEntity?.Code;
                    model.WarehouseName = wareHouseEntity?.Name;
                    model.WarehouseRegionCode = model.Code;
                    result.Add(model);
                }
                
            }
            //// 实体到DTO转换 装载数据
            //var dtos = pagedInfo.Data.Select(s => {
            //    var model=s.ToModel<WhWarehouseRegionDto>();

            //    var wareHouseEntity = wareHouseEntities.FirstOrDefault(a => a.Id == model.WarehouseId);

            //    model.WarehouseCode = wareHouseEntity?.Code;
            //    model.WarehouseName= wareHouseEntity?.Name;

            //    return model;
            //}
            //);
            return new PagedInfo<WhWarehouseRegionDto>(result, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

    }
}
