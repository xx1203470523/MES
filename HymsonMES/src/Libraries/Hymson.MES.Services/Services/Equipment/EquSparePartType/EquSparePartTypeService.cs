using Hymson.Authentication;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquSparePart;
using Hymson.MES.Data.Repositories.Equipment.EquSparePartType;
using Hymson.MES.Data.Repositories.Equipment.EquSparePartType.Query;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.Snowflake;

namespace Hymson.MES.Services.Services.Equipment.EquSparePartType
{
    /// <summary>
    /// 业务处理层（备件类型） 
    /// </summary>
    public class EquSparePartTypeService : IEquSparePartTypeService
    {
        /// <summary>
        /// 当前登录用户对象
        /// </summary>
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 仓储（备件类型） 
        /// </summary>
        private readonly IEquSparePartTypeRepository _equSparePartTypeRepository;

        /// <summary>
        /// 仓储（备件注册） 
        /// </summary>
        private readonly IEquSparePartRepository _equSparePartRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="equSparePartTypeRepository"></param>
        /// <param name="equSparePartRepository"></param>
        public EquSparePartTypeService(ICurrentUser currentUser, 
            IEquSparePartTypeRepository equSparePartTypeRepository,
            IEquSparePartRepository equSparePartRepository)
        {
            _currentUser = currentUser;
            _equSparePartTypeRepository = equSparePartTypeRepository;
            _equSparePartRepository = equSparePartRepository;
        }


        /// <summary>
        /// 添加（备件类型）
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        public async Task<int> CreateEquSparePartTypeAsync(EquSparePartTypeCreateDto createDto)
        {
            // TODO 验证DTO


            // DTO转换实体
            var entity = createDto.ToEntity<EquSparePartTypeEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = _currentUser.UserName;
            entity.UpdatedBy = _currentUser.UserName;

            // TODO 事务处理
            var rows = 0;
            rows += await _equSparePartTypeRepository.InsertAsync(entity);
            rows += await _equSparePartRepository.UpdateSparePartTypeIdAsync(entity.Id, createDto.SparePartIDs);
            return rows;
        }

        /// <summary>
        /// 修改（备件类型）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyEquSparePartTypeAsync(EquSparePartTypeModifyDto modifyDto)
        {
            // 验证DTO

            // DTO转换实体
            var entity = modifyDto.ToEntity<EquSparePartTypeEntity>();
            entity.UpdatedBy = _currentUser.UserName;

            // TODO 事务处理
            var rows = 0;
            rows += await _equSparePartTypeRepository.UpdateAsync(entity);
            rows += await _equSparePartRepository.ClearSparePartTypeIdAsync(entity.Id);
            rows += await _equSparePartRepository.UpdateSparePartTypeIdAsync(entity.Id, modifyDto.SparePartIDs);
            return rows;
        }

        /// <summary>
        /// 删除（备件类型）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteEquSparePartTypeAsync(long id)
        {
            return await _equSparePartTypeRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除（备件类型）
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesEquSparePartTypeAsync(long[] idsArr)
        {
            return await _equSparePartTypeRepository.DeletesAsync(idsArr);
        }

        /// <summary>
        /// 分页查询列表（备件类型）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquSparePartTypeDto>> GetPageListAsync(EquSparePartTypePagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<EquSparePartTypePagedQuery>();
            var pagedInfo = await _equSparePartTypeRepository.GetPagedInfoAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<EquSparePartTypeDto>());
            return new PagedInfo<EquSparePartTypeDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 查询详情（备件类型）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquSparePartTypeDto> QueryEquSparePartTypeByIdAsync(long id)
        {
            return (await _equSparePartTypeRepository.GetByIdAsync(id)).ToModel<EquSparePartTypeDto>();
        }

    }
}
