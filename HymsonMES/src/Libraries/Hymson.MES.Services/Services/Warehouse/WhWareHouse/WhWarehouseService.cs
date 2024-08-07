using Elastic.Clients.Elasticsearch;
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.WhWareHouse;
using Hymson.MES.Core.Domain.WhWarehouseRegion;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.WhWareHouse;
using Hymson.MES.Data.Repositories.WhWareHouse.Query;
using Hymson.MES.Data.Repositories.WhWarehouseRegion;
using Hymson.MES.Data.Repositories.WhWarehouseRegion.Query;
using Hymson.MES.Services.Dtos.WhWareHouse;
using Hymson.Snowflake;
using Hymson.Utils;
using Org.BouncyCastle.Asn1.Ocsp;

namespace Hymson.MES.Services.Services.WhWareHouse
{
    /// <summary>
    /// 服务（仓库） 
    /// </summary>
    public class WhWarehouseService : IWhWarehouseService
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
        private readonly AbstractValidator<WhWarehouseSaveDto> _validationSaveRules;
        private readonly AbstractValidator<WhWarehouseModifyDto> _validationModifyRules;

        /// <summary>
        /// 仓储接口（仓库）
        /// </summary>
        private readonly IWhWarehouseRepository _whWarehouseRepository;
        private readonly IWhWarehouseRegionRepository _whWarehouseRegionRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="whWarehouseRepository"></param>
        /// <param name="whWarehouseRegionRepository"></param>
        /// <param name="validationModifyRules"></param>
        public WhWarehouseService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<WhWarehouseSaveDto> validationSaveRules,
            IWhWarehouseRepository whWarehouseRepository, IWhWarehouseRegionRepository whWarehouseRegionRepository, AbstractValidator<WhWarehouseModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _whWarehouseRepository = whWarehouseRepository;
            _whWarehouseRegionRepository = whWarehouseRegionRepository;
            _validationModifyRules = validationModifyRules;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(WhWarehouseSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) 
            { 
                throw new CustomerValidationException(nameof(ErrorCode.MES10101)); 
            }

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<WhWarehouseEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存
            var result= await _whWarehouseRepository.InsertIgnoreAsync(entity);
            if (result == 0) {
                throw new CustomerValidationException(nameof(ErrorCode.MES19203)).WithData("code", saveDto.Code);
            }
            return result;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(WhWarehouseModifyDto modifyDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10101));
            }

            // 验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(modifyDto);

            //var query = new WhWarehouseQuery();
            //query.WareHouseCode = saveDto.WareHouseCode;
            //query.SiteId = saveDto.SiteId;
            //query.IsDeleted = saveDto.IsDeleted;
            //var wareHouseEntities = await _whWarehouseRepository.GetEntitiesAsync(query);
            //if (wareHouseEntities == null || !wareHouseEntities.Any()) {
            //    throw new CustomerValidationException(nameof(ErrorCode.MES19203)).WithData("code", saveDto.WareHouseCode);
            //}

            // DTO转换实体
            var entity = modifyDto.ToEntity<WhWarehouseEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();
            entity.SiteId = _currentSite.SiteId ?? 0;

            return await _whWarehouseRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _whWarehouseRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            var warehouseRegionEntities = await _whWarehouseRegionRepository.GetEntitiesAsync(new WhWarehouseRegionQuery { WarehouseIds = ids });
            if (warehouseRegionEntities != null && warehouseRegionEntities.Any()) {
                throw new CustomerValidationException(nameof(ErrorCode.MES19216));
            }

            return await _whWarehouseRepository.DeletesAsync(new DeleteCommand
            {
                Ids = ids,
                DeleteOn = HymsonClock.Now(),
                UserId = _currentUser.UserName
            });
        }

        /// <summary>
        /// 查询所有仓库
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SelectOptionDto>> GetWarehouseListAsync()
        {
            var lists = await _whWarehouseRepository.GetEntitiesAsync(new WhWarehouseQuery { SiteId = _currentSite.SiteId ?? 0 });
            return lists.Select(s => new SelectOptionDto
            {
                Key = $"{s.Code}",
                Label = $"【{s.Code}】 {s.Name}",
                Value = $"{s.Code}"
            });
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<WhWarehouseDto?> QueryByIdAsync(long id)
        {
            var whWarehouseEntity = await _whWarehouseRepository.GetOneAsync(new WhWarehouseQuery { Id = id, SiteId = _currentSite.SiteId ?? 0 });
            if (whWarehouseEntity == null) return null;

            return whWarehouseEntity.ToModel<WhWarehouseDto>();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<WhWarehouseDto>> GetPagedListAsync(WhWarehousePagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<WhWarehousePagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _whWarehouseRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s =>
            {
                var model = s.ToModel<WhWarehouseDto>();
                model.WarehouseCode = model.Code;

                return model;
            });
            return new PagedInfo<WhWarehouseDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<WhWarehouseDto>> GetPagedListCopyAsync(WhWarehousePagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<WhWarehousePagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _whWarehouseRepository.GetPagedListCopyAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s =>
            {
                var model = s.ToModel<WhWarehouseDto>();
                model.WarehouseCode = model.Code;

                return model;
            });
            return new PagedInfo<WhWarehouseDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

    }
}
