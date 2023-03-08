using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated.InteContainer;
using Hymson.MES.Data.Repositories.Integrated.InteContainer.Query;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.Sequences;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Integrated.InteContainer
{
    /// <summary>
    /// 服务（容器维护）
    /// </summary>
    public class InteContainerService : IInteContainerService
    {
        /// <summary>
        /// 当前对象（登录用户）
        /// </summary>
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 当前对象（站点）
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 容器维护 仓储
        /// </summary>
        private readonly IInteContainerRepository _inteContainerRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="sequenceService"></param>
        /// <param name="inteContainerRepository"></param>
        public InteContainerService(ICurrentUser currentUser, ICurrentSite currentSite, ISequenceService sequenceService,
            IInteContainerRepository inteContainerRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _inteContainerRepository = inteContainerRepository;
        }


        /// <summary>
        /// 添加（容器维护）
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(InteContainerSaveDto createDto)
        {
            //验证DTO
            //await _validationCreateRules.ValidateAndThrowAsync(createDto);

            // DTO转换实体
            var entity = createDto.ToEntity<InteContainerEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = _currentUser.UserName;
            entity.UpdatedBy = _currentUser.UserName;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存实体
            return await _inteContainerRepository.InsertAsync(entity); ;
        }

        /// <summary>
        /// 更新（容器维护）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(InteContainerSaveDto modifyDto)
        {
            var entity = modifyDto.ToEntity<InteContainerEntity>();
            entity.UpdatedBy = _currentUser.UserName;

            // 更新实体
            return await _inteContainerRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除（容器维护）
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] idsArr)
        {
            return await _inteContainerRepository.DeletesAsync(new DeleteCommand
            {
                Ids = idsArr,
                UserId = _currentUser.UserName,
                DeleteOn = HymsonClock.Now()
            });
        }

        /// <summary>
        /// 获取分页数据（容器维护）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteContainerDto>> GetPagedListAsync(InteContainerPagedQueryDto pagedQueryDto)
        {
            var inteContainerPagedQuery = pagedQueryDto.ToQuery<InteContainerPagedQuery>();
            var pagedInfo = await _inteContainerRepository.GetPagedInfoAsync(inteContainerPagedQuery);

            // 实体到DTO转换 装载数据// 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<InteContainerDto>());
            return new PagedInfo<InteContainerDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 查询详情（容器维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteContainerDto> GetDetailAsync(long id)
        {
            var inteContainerEntity = await _inteContainerRepository.GetByIdAsync(id);
            if (inteContainerEntity != null)
            {
                return inteContainerEntity.ToModel<InteContainerDto>();
            }
            return null;
        }
    }
}
