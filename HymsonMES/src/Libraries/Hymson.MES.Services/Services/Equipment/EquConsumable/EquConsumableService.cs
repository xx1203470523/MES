using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquSparePart;
using Hymson.MES.Data.Repositories.Equipment.EquSparePart.Query;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.Snowflake;

namespace Hymson.MES.Services.Services.Equipment.EquSparePart
{
    /// <summary>
    /// 业务处理层（工装注册） 
    /// </summary>
    public class EquConsumableService : IEquConsumableService
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
        /// 仓储（工装注册） 
        /// </summary>
        private readonly IEquConsumableRepository _equConsumableRepository;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentSite"></param>
        /// <param name="currentUser"></param>
        /// <param name="equConsumableRepository"></param>
        public EquConsumableService(ICurrentUser currentUser, ICurrentSite currentSite,
            IEquConsumableRepository equConsumableRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _equConsumableRepository = equConsumableRepository;
        }

        /// <summary>
        /// 添加（工装注册）
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(EquConsumableCreateDto createDto)
        {
            //验证DTO


            // DTO转换实体
            var entity = createDto.ToEntity<EquConsumableEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = _currentUser.UserName;
            entity.UpdatedBy = _currentUser.UserName;

            // 入库
            return await _equConsumableRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改（工装注册）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(EquConsumableModifyDto modifyDto)
        {
            // 验证DTO


            // DTO转换实体
            var entity = modifyDto.ToEntity<EquConsumableEntity>();
            entity.UpdatedBy = _currentUser.UserName;

            return await _equConsumableRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除（工装注册）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _equConsumableRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除（工装注册）
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] idsArr)
        {
            return await _equConsumableRepository.DeletesAsync(idsArr);
        }

        /// <summary>
        /// 分页查询列表（工装注册）
        /// </summary>
        /// <param name="equSparePartPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquConsumableDto>> GetPagedListAsync(EquConsumablePagedQueryDto equSparePartPagedQueryDto)
        {
            var pagedQuery = equSparePartPagedQueryDto.ToQuery<EquConsumablePagedQuery>();
            var pagedInfo = await _equConsumableRepository.GetPagedInfoAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<EquConsumableDto>());
            return new PagedInfo<EquConsumableDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 查询详情（工装注册）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquConsumableDto> GetDetailAsync(long id)
        {
            return (await _equConsumableRepository.GetByIdAsync(id)).ToModel<EquConsumableDto>();
        }

    }
}
