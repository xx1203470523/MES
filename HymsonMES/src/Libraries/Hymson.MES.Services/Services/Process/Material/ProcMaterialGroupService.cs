using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Process;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Transactions;

namespace Hymson.MES.Services.Services.Process
{
    /// <summary>
    /// 物料组维护表 服务
    /// </summary>
    public class ProcMaterialGroupService : IProcMaterialGroupService
    {
        /// <summary>
        /// 物料组维护表 仓储
        /// </summary>
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 仓储接口（物料维护）
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 仓储接口（物料组维护）
        /// </summary>
        private readonly IProcMaterialGroupRepository _procMaterialGroupRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="procMaterialGroupRepository"></param>
        public ProcMaterialGroupService(ICurrentUser currentUser, ICurrentSite currentSite,
            IProcMaterialRepository procMaterialRepository,
            IProcMaterialGroupRepository procMaterialGroupRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _procMaterialGroupRepository = procMaterialGroupRepository;
            _procMaterialRepository = procMaterialRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="procMaterialGroupCreateDto"></param>
        /// <returns></returns>
        public async Task<long> CreateProcMaterialGroupAsync(ProcMaterialGroupCreateDto procMaterialGroupCreateDto)
        {
            if (procMaterialGroupCreateDto == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }
            procMaterialGroupCreateDto.GroupCode = procMaterialGroupCreateDto.GroupCode.ToTrimSpace().ToUpperInvariant();
            procMaterialGroupCreateDto.GroupName = procMaterialGroupCreateDto.GroupName.ToTrimSpace();
            procMaterialGroupCreateDto.Remark = procMaterialGroupCreateDto?.Remark ?? "".Trim();

            //DTO转换实体
            var procMaterialGroupEntity = procMaterialGroupCreateDto!.ToEntity<ProcMaterialGroupEntity>();
            procMaterialGroupEntity.Id = IdGenProvider.Instance.CreateId();
            procMaterialGroupEntity.CreatedBy = _currentUser.UserName;
            procMaterialGroupEntity.UpdatedBy = _currentUser.UserName;
            procMaterialGroupEntity.CreatedOn = HymsonClock.Now();
            procMaterialGroupEntity.UpdatedOn = HymsonClock.Now();
            procMaterialGroupEntity.SiteId = _currentSite.SiteId ?? 0;

            #region 参数校验
            //判断编号是否已存在
            var existGroupCodes = await _procMaterialGroupRepository.GetProcMaterialGroupEntitiesAsync(new ProcMaterialGroupQuery { SiteId = _currentSite.SiteId ?? 0, GroupCode = procMaterialGroupCreateDto!.GroupCode });
            if (existGroupCodes != null && existGroupCodes.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10216)).WithData("groupCode", procMaterialGroupCreateDto.GroupCode);
            }

            var procMaterialList = ConvertProcMaterialList(procMaterialGroupCreateDto.DynamicList, procMaterialGroupEntity);
            var procMaterialIds = procMaterialList.Select(x => x.Id).ToArray();

            // 判断物料是否已被使用
            var procMaterials = await _procMaterialRepository.GetByIdsAsync(procMaterialIds);
            if (procMaterials.Any() && procMaterials.Any(x => x.GroupId != 0))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10217));
            }

            #endregion

            #region 保存到数据库
            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                var response = 0;

                //入库
                response = await _procMaterialGroupRepository.InsertAsync(procMaterialGroupEntity);

                if (response == 0)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10218));
                }

                foreach (var item in procMaterialList)
                {
                    item.UpdatedBy = procMaterialGroupEntity.UpdatedBy;
                    item.UpdatedOn = procMaterialGroupEntity.UpdatedOn;
                }
                response = await _procMaterialRepository.UpdateProcMaterialGroupAsync(procMaterialList);

                if (response < procMaterialList.Count())
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10218));
                }

                ts.Complete();
            }

            return procMaterialGroupEntity.Id;
            #endregion

        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procMaterialGroupModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyProcMaterialGroupAsync(ProcMaterialGroupModifyDto procMaterialGroupModifyDto)
        {
            procMaterialGroupModifyDto.Remark ??= "".Trim();

            // 物料组是否存在
            var entity = await _procMaterialGroupRepository.GetByIdAsync(procMaterialGroupModifyDto.Id)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10219));

            entity.Remark = procMaterialGroupModifyDto.Remark;
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            var procMaterialList = ConvertProcMaterialList(procMaterialGroupModifyDto.DynamicList, entity);

            // 判断物料是否已被使用
            var procMaterials = await _procMaterialRepository.GetByIdsAsync(procMaterialList.Select(x => x.Id).ToArray());
            if (procMaterials.Any()
                && procMaterials.Any(x => x.GroupId != 0 && x.GroupId != entity.Id))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10217));
            }

            #region 保存到数据库
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                var rows = 0;
                rows = await _procMaterialGroupRepository.UpdateAsync(entity);
                if (rows == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10220));

                // 将之前所有该物料组的物料改为未绑定 
                await _procMaterialRepository.UpdateProcMaterialUnboundAsync(entity.Id);

                rows = await _procMaterialRepository.UpdateProcMaterialGroupAsync(procMaterialList);

                trans.Complete();
            }

            #endregion
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesProcMaterialGroupAsync(long[] idsArr)
        {
            if (idsArr.Length < 1) throw new CustomerValidationException(nameof(ErrorCode.MES10213));

            // 判断物料中是否有当前物料组
            var procMaterials = await _procMaterialRepository.GetByGroupIdsAsync(idsArr);
            if (procMaterials != null && procMaterials.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10221));
            }

            return await _procMaterialGroupRepository.DeletesAsync(new DeleteCommand { Ids = idsArr, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="procMaterialGroupPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcMaterialGroupDto>> GetPageListAsync(ProcMaterialGroupPagedQueryDto procMaterialGroupPagedQueryDto)
        {
            var procMaterialGroupPagedQuery = procMaterialGroupPagedQueryDto.ToQuery<ProcMaterialGroupPagedQuery>();
            procMaterialGroupPagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _procMaterialGroupRepository.GetPagedInfoAsync(procMaterialGroupPagedQuery);

            //实体到DTO转换 装载数据
            List<ProcMaterialGroupDto> procMaterialGroupDtos = PrepareProcMaterialGroupDtos(pagedInfo);
            return new PagedInfo<ProcMaterialGroupDto>(procMaterialGroupDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ProcMaterialGroupDto> PrepareProcMaterialGroupDtos(PagedInfo<ProcMaterialGroupEntity> pagedInfo)
        {
            var procMaterialGroupDtos = new List<ProcMaterialGroupDto>();
            foreach (var procMaterialGroupEntity in pagedInfo.Data)
            {
                var procMaterialGroupDto = procMaterialGroupEntity.ToModel<ProcMaterialGroupDto>();
                procMaterialGroupDtos.Add(procMaterialGroupDto);
            }

            return procMaterialGroupDtos;
        }

        /// <summary>
        /// 获取分页自定义List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<CustomProcMaterialGroupViewDto>> GetPageCustomListAsync(CustomProcMaterialGroupPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<ProcMaterialGroupCustomPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _procMaterialGroupRepository.GetPageListNewAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            List<CustomProcMaterialGroupViewDto> procMaterialGroupDtos = PrepareCustomProcMaterialGroupDtos(pagedInfo);
            return new PagedInfo<CustomProcMaterialGroupViewDto>(procMaterialGroupDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<CustomProcMaterialGroupViewDto> PrepareCustomProcMaterialGroupDtos(PagedInfo<CustomProcMaterialGroupView> pagedInfo)
        {
            var dtos = new List<CustomProcMaterialGroupViewDto>();
            foreach (var customProcMaterialGroupView in pagedInfo.Data)
            {
                var dto = customProcMaterialGroupView.ToModel<CustomProcMaterialGroupViewDto>();

                dto.GroupId = dto.Id;
                dto.Id = IdGenProvider.Instance.CreateId();

                dtos.Add(dto);
            }

            return dtos;
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcMaterialGroupDto> QueryProcMaterialGroupByIdAsync(long id)
        {
            var procMaterialGroupEntity = await _procMaterialGroupRepository.GetByIdAndSiteIdAsync(id, _currentSite.SiteId ?? 0);
            if (procMaterialGroupEntity != null)
            {
                return procMaterialGroupEntity.ToModel<ProcMaterialGroupDto>();
            }
            return new ProcMaterialGroupDto();
        }



        /// <summary>
        /// 转换集合（物料组）
        /// </summary>
        /// <param name="dynamicList"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static IEnumerable<ProcMaterialEntity> ConvertProcMaterialList(IEnumerable<string>? dynamicList, ProcMaterialGroupEntity entity)
        {
            if (dynamicList == null || !dynamicList.Any()) return new List<ProcMaterialEntity> { };

            return dynamicList.Select(s => new ProcMaterialEntity
            {
                Id = s.ParseToLong(),
                GroupId = entity.Id,
                UpdatedBy = entity.UpdatedBy,
                UpdatedOn = entity.UpdatedOn
            });
        }

    }
}
