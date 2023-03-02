using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
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
using IdGen;

namespace Hymson.MES.Services.Services.Equipment.EquSparePartType
{
    /// <summary>
    /// 业务处理层（工装类型） 
    /// </summary>
    public class EquConsumableTypeService : IEquConsumableTypeService
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
        /// 仓储（工装类型） 
        /// </summary>
        private readonly IEquSparePartTypeRepository _equConsumableTypeRepository;

        /// <summary>
        /// 仓储（工装注册） 
        /// </summary>
        private readonly IEquSparePartRepository _equConsumableRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentSite"></param>
        /// <param name="currentUser"></param>
        /// <param name="equConsumableTypeRepository"></param>
        /// <param name="equConsumableRepository"></param>
        public EquConsumableTypeService(ICurrentUser currentUser, ICurrentSite currentSite,
            IEquSparePartTypeRepository equConsumableTypeRepository,
            IEquSparePartRepository equConsumableRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _equConsumableTypeRepository = equConsumableTypeRepository;
            _equConsumableRepository = equConsumableRepository;
        }


        /// <summary>
        /// 添加（工装类型）
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(EquConsumableTypeSaveDto createDto)
        {
            // TODO 验证DTO


            // DTO转换实体
            var entity = createDto.ToEntity<EquSparePartTypeEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = _currentUser.UserName;
            entity.UpdatedBy = _currentUser.UserName;
            entity.Type = (int)EquipmentPartTypeEnum.Consumable; // 工装

            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows += await _equConsumableTypeRepository.InsertAsync(entity);
                rows += await _equConsumableRepository.UpdateSparePartTypeIdAsync(new UpdateSparePartTypeIdCommand
                {
                    SparePartTypeId = entity.Id,
                    SparePartIds = createDto.ConsumableIDs
                });
                trans.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 修改（工装类型）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(EquConsumableTypeSaveDto modifyDto)
        {
            // 验证DTO

            // DTO转换实体
            var entity = modifyDto.ToEntity<EquSparePartTypeEntity>();
            entity.UpdatedBy = _currentUser.UserName;

            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows += await _equConsumableTypeRepository.UpdateAsync(entity);
                rows += await _equConsumableRepository.ClearSparePartTypeIdAsync(entity.Id);
                rows += await _equConsumableRepository.UpdateSparePartTypeIdAsync(new UpdateSparePartTypeIdCommand
                {
                    SparePartTypeId = entity.Id,
                    SparePartIds = modifyDto.ConsumableIDs
                });
                trans.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 删除（工装类型）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _equConsumableTypeRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除（工装类型）
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] idsArr)
        {
            return await _equConsumableTypeRepository.DeletesAsync(new DeleteCommand
            {
                Ids = idsArr,
                UserId = _currentUser.UserName,
                DeleteOn = HymsonClock.Now()
            });
        }

        /// <summary>
        /// 分页查询列表（工装类型）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquConsumableTypeDto>> GetPagedListAsync(EquConsumableTypePagedQueryDto pagedQueryDto)
        {
            var pagedQuery = new EquSparePartTypePagedQuery()
            {
                PageIndex = pagedQueryDto.PageIndex,
                PageSize = pagedQueryDto.PageSize,
                SiteId = _currentSite.SiteId,
                SparePartTypeCode = pagedQueryDto.ConsumableTypeCode,
                SparePartTypeName = pagedQueryDto.ConsumableTypeName,
                Status = pagedQueryDto.Status,
                Type = (int)EquipmentPartTypeEnum.Consumable // 工装
            };

            var pagedInfo = await _equConsumableTypeRepository.GetPagedInfoAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            //var dtos = pagedInfo.Data.Select(s => s.ToModel<EquConsumableTypeDto>());
            var dtos = pagedInfo.Data.Select(s => new EquConsumableTypeDto()
            {
                Id = s.Id,
                ConsumableTypeCode = s.SparePartTypeCode,
                ConsumableTypeName = s.SparePartTypeName,
                Status = s.Status,
                Remark = s.Remark,
                UpdatedBy = s.UpdatedBy ?? "",
                UpdatedOn = s.UpdatedOn
            });
            return new PagedInfo<EquConsumableTypeDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 查询详情（工装类型）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquConsumableTypeDto> GetDetailAsync(long id)
        {
            return (await _equConsumableTypeRepository.GetByIdAsync(id)).ToModel<EquConsumableTypeDto>();
        }

    }
}
