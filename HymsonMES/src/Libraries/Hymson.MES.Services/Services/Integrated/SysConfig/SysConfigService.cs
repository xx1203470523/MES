using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Common;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Integrated
{
    /// <summary>
    /// 服务（系统配置） 
    /// </summary>
    public class SysConfigService : ISysConfigService
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
        /// 仓储接口（系统配置）
        /// </summary>
        private readonly ISysConfigRepository _sysConfigRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="sysConfigRepository"></param>
        public SysConfigService(ICurrentUser currentUser, ICurrentSite currentSite,
            ISysConfigRepository sysConfigRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _sysConfigRepository = sysConfigRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(SysConfigSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            if(saveDto.Type == null || 
                string.IsNullOrEmpty(saveDto.Code) == true ||
                string.IsNullOrEmpty(saveDto.Value) == true ||
                string.IsNullOrEmpty(saveDto.Remark) == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19310));
            }

            SysConfigQuery query = new SysConfigQuery();
            query.Type = (SysConfigEnum)saveDto.Type!;
            query.Codes = new List<string>() { saveDto.Code! };
            var dbList = await _sysConfigRepository.GetEntitiesAsync(query);
            if(dbList != null && dbList.Count() > 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19311));
            }

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<SysConfigEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存
            return await _sysConfigRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(SysConfigSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // DTO转换实体
            var entity = saveDto.ToEntity<SysConfigEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            return await _sysConfigRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _sysConfigRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            return await _sysConfigRepository.DeletesAsync(new DeleteCommand
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
        public async Task<SysConfigDto?> QueryByIdAsync(long id) 
        {
           var sysConfigEntity = await _sysConfigRepository.GetByIdAsync(id);
           if (sysConfigEntity == null) return null;
           
           return sysConfigEntity.ToModel<SysConfigDto>();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<SysConfigDto>> GetPagedListAsync(SysConfigPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<SysConfigPagedQuery>();
            //pagedQuery.PageSize = pagedQueryDto.PageSize;
            //pagedQuery.PageIndex = pagedQuery.PageIndex;
            //pagedQuery.Sorting = pagedQueryDto.Sorting;
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _sysConfigRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<SysConfigDto>());
            return new PagedInfo<SysConfigDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

    }
}
