using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Equipment.EquConsumable.Command;
using Hymson.MES.Data.Repositories.Equipment.EquSparePart;
using Hymson.MES.Data.Repositories.Equipment.EquSparePartType;
using Hymson.MES.Data.Repositories.Equipment.EquSparePartType.Query;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;

namespace Hymson.MES.Services.Services.Equipment.EquSparePartType
{
    /// <summary>
    /// 业务处理层（备件类型） 
    /// </summary>
    public class EquSparePartTypeService : IEquSparePartTypeService
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
        /// 枚举服务
        /// </summary>
        private readonly IEnumService _enumService;

        /// <summary>
        /// 仓储（备件类型） 
        /// </summary>
        private readonly IEquSparePartTypeRepository _equSparePartTypeRepository;

        /// <summary>
        /// 仓储（备件注册） 
        /// </summary>
        private readonly IEquSparePartRepository _equSparePartRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentSite"></param>
        /// <param name="currentUser"></param>
        /// <param name="enumService"></param>
        /// <param name="equSparePartTypeRepository"></param>
        /// <param name="equSparePartRepository"></param>
        public EquSparePartTypeService(ICurrentUser currentUser, ICurrentSite currentSite, IEnumService enumService,
            IEquSparePartTypeRepository equSparePartTypeRepository,
            IEquSparePartRepository equSparePartRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _enumService = enumService;
            _equSparePartTypeRepository = equSparePartTypeRepository;
            _equSparePartRepository = equSparePartRepository;
        }


        /// <summary>
        /// 添加（备件类型）
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(EquSparePartTypeCreateDto createDto)
        {
            // TODO 验证DTO
            //TODO  _enumService.GetEnumTypes();

            // DTO转换实体
            var entity = createDto.ToEntity<EquSparePartTypeEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = _currentUser.UserName;
            entity.UpdatedBy = _currentUser.UserName;
            entity.SiteId = _currentSite.SiteId;
            entity.Type = (int)EquipmentPartTypeEnum.SparePart; // 备件

            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows += await _equSparePartTypeRepository.InsertAsync(entity);
                rows += await _equSparePartRepository.UpdateSparePartTypeIdAsync(new UpdateSparePartTypeIdCommand
                {
                    SparePartTypeId = entity.Id,
                    SparePartIds = createDto.SparePartIDs
                });
                trans.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 修改（备件类型）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(EquSparePartTypeModifyDto modifyDto)
        {
            // 验证DTO

            // DTO转换实体
            var entity = modifyDto.ToEntity<EquSparePartTypeEntity>();
            entity.UpdatedBy = _currentUser.UserName;

            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows += await _equSparePartTypeRepository.UpdateAsync(entity);
                rows += await _equSparePartRepository.ClearSparePartTypeIdAsync(entity.Id);
                rows += await _equSparePartRepository.UpdateSparePartTypeIdAsync(new UpdateSparePartTypeIdCommand
                {
                    SparePartTypeId = entity.Id,
                    SparePartIds = modifyDto.SparePartIDs
                });
                trans.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 删除（备件类型）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _equSparePartTypeRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除（备件类型）
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] idsArr)
        {
            return await _equSparePartTypeRepository.DeletesAsync(new DeleteCommand
            {
                Ids = idsArr,
                UserId = _currentUser.UserName,
                DeleteOn = HymsonClock.Now()
            });
        }

        /// <summary>
        /// 分页查询列表（备件类型）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquSparePartTypeDto>> GetPagedListAsync(EquSparePartTypePagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<EquSparePartTypePagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId;
            pagedQuery.Type = (int)EquipmentPartTypeEnum.SparePart; // 备件
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
        public async Task<EquSparePartTypeDto> GetDetailAsync(long id)
        {
            var entity = await _equSparePartTypeRepository.GetByIdAsync(id);
            return entity.ToModel<EquSparePartTypeDto>();
        }

    }
}
