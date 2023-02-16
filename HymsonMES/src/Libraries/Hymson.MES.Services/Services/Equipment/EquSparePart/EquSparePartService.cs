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
    /// 业务处理层（备件注册） 
    /// </summary>
    public class EquSparePartService : IEquSparePartService
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
        /// 仓储（备件注册） 
        /// </summary>
        private readonly IEquSparePartRepository _equSparePartRepository;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentSite"></param>
        /// <param name="currentUser"></param>
        /// <param name="equSparePartRepository"></param>
        public EquSparePartService(ICurrentUser currentUser, ICurrentSite currentSite,
            IEquSparePartRepository equSparePartRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _equSparePartRepository = equSparePartRepository;
        }

        /// <summary>
        /// 添加（备件注册）
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(EquSparePartCreateDto createDto)
        {
            //验证DTO


            //DTO转换实体
            var entity = createDto.ToEntity<EquSparePartEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = _currentUser.UserName;
            entity.UpdatedBy = _currentUser.UserName;

            // 入库
            return await _equSparePartRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改（备件注册）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(EquSparePartModifyDto modifyDto)
        {
            // 验证DTO


            // DTO转换实体
            var entity = modifyDto.ToEntity<EquSparePartEntity>();
            entity.UpdatedBy = _currentUser.UserName;

            return await _equSparePartRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除（备件注册）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _equSparePartRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除（备件注册）
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] idsArr)
        {
            return await _equSparePartRepository.DeletesAsync(idsArr);
        }

        /// <summary>
        /// 分页查询列表（备件注册）
        /// </summary>
        /// <param name="equSparePartPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquSparePartDto>> GetPagedListAsync(EquSparePartPagedQueryDto equSparePartPagedQueryDto)
        {
            var pagedQuery = equSparePartPagedQueryDto.ToQuery<EquSparePartPagedQuery>();
            var pagedInfo = await _equSparePartRepository.GetPagedInfoAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<EquSparePartDto>());
            return new PagedInfo<EquSparePartDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 查询详情（备件注册）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquSparePartDto> GetDetailAsync(long id)
        {
            return (await _equSparePartRepository.GetByIdAsync(id)).ToModel<EquSparePartDto>();
        }

    }
}
