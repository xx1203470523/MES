using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Equipment.EquSparePart;
using Hymson.MES.Data.Repositories.Equipment.EquSparePart.Query;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.Snowflake;
using Hymson.Utils;

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
        /// 仓储（备件/工装注册） 
        /// </summary>
        private readonly IEquSparePartRepository _equConsumableRepository;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentSite"></param>
        /// <param name="currentUser"></param>
        /// <param name="equConsumableRepository"></param>
        public EquConsumableService(ICurrentUser currentUser, ICurrentSite currentSite,
            IEquSparePartRepository equConsumableRepository)
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
        public async Task<int> CreateAsync(EquConsumableSaveDto createDto)
        {
            // 验证DTO


            // DTO转换实体
            var entity = createDto.ToEntity<EquSparePartEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = _currentUser.UserName;
            entity.UpdatedBy = _currentUser.UserName;
            entity.Type = EquipmentPartTypeEnum.Consumable; // 工装

            // 入库
            return await _equConsumableRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改（工装注册）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(EquConsumableSaveDto modifyDto)
        {
            // 验证DTO


            // DTO转换实体
            var entity = modifyDto.ToEntity<EquSparePartEntity>();
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
            return await _equConsumableRepository.DeletesAsync(new DeleteCommand
            {
                Ids = idsArr,
                UserId = _currentUser.UserName,
                DeleteOn = HymsonClock.Now()
            });
        }

        /// <summary>
        /// 分页查询列表（工装注册）
        /// </summary>
        /// <param name="equSparePartPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquConsumableDto>> GetPagedListAsync(EquConsumablePagedQueryDto equSparePartPagedQueryDto)
        {
            var pagedQuery = equSparePartPagedQueryDto.ToQuery<EquSparePartPagedQuery>();
            pagedQuery.Type = EquipmentPartTypeEnum.Consumable; // 工装
            pagedQuery.SiteId = _currentSite.SiteId ?? 123456;
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
