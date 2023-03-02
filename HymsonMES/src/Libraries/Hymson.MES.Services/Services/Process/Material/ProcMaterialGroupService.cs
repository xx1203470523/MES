/*
 *creator: Karl
 *
 *describe: 物料组维护表    服务 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-10 03:54:07
 */
using FluentValidation;
using Google.Protobuf.WellKnownTypes;
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
using Hymson.Utils;
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
        private readonly IProcMaterialGroupRepository _procMaterialGroupRepository;
        private readonly AbstractValidator<ProcMaterialGroupCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ProcMaterialGroupModifyDto> _validationModifyRules;

        private readonly IProcMaterialRepository _procMaterialRepository;

        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        public ProcMaterialGroupService(ICurrentUser currentUser, ICurrentSite currentSite , IProcMaterialGroupRepository procMaterialGroupRepository, AbstractValidator<ProcMaterialGroupCreateDto> validationCreateRules, AbstractValidator<ProcMaterialGroupModifyDto> validationModifyRules, IProcMaterialRepository procMaterialRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _procMaterialGroupRepository = procMaterialGroupRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _procMaterialRepository = procMaterialRepository;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="procMaterialGroupDto"></param>
        /// <returns></returns>
        public async Task CreateProcMaterialGroupAsync(ProcMaterialGroupCreateDto procMaterialGroupCreateDto)
        {
            //DTO转换实体
            var procMaterialGroupEntity = procMaterialGroupCreateDto.ToEntity<ProcMaterialGroupEntity>();
            procMaterialGroupEntity.Id = IdGenProvider.Instance.CreateId();
            procMaterialGroupEntity.CreatedBy = _currentUser.UserName;
            procMaterialGroupEntity.UpdatedBy = _currentUser.UserName;
            procMaterialGroupEntity.CreatedOn = HymsonClock.Now();
            procMaterialGroupEntity.UpdatedOn = HymsonClock.Now();
            procMaterialGroupEntity.SiteId = _currentSite.SiteId??0;

            #region 参数校验
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(procMaterialGroupCreateDto);

            //判断编号是否已存在
            var existGroupCodes = await _procMaterialGroupRepository.GetProcMaterialGroupEntitiesAsync(new ProcMaterialGroupQuery { SiteId = _currentSite.SiteId??0, GroupCode = procMaterialGroupCreateDto.GroupCode });
            if (existGroupCodes != null && existGroupCodes.Count() > 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10216)).WithData("groupCode", procMaterialGroupCreateDto.GroupCode);
            }

            var procMaterialList = ConvertProcMaterialList(procMaterialGroupCreateDto.DynamicList, procMaterialGroupEntity.Id);
            var procMaterialIds = procMaterialList.Select(x => x.Id).ToArray();

            // 判断物料是否已被使用
            var procMaterials = await _procMaterialRepository.GetByIdsAsync(procMaterialIds);
            if (procMaterials.Where(x => x.GroupId != 0).Count() > 0)
            {
                throw new CustomerValidationException(ErrorCode.MES10217);
            }

            #endregion

            #region 保存到数据库
            using (TransactionScope ts = new TransactionScope())
            {
                var response = 0;

                //入库
                response = await _procMaterialGroupRepository.InsertAsync(procMaterialGroupEntity);

                if (response == 0)
                {
                    throw new BusinessException(ErrorCode.MES10218);
                }

                foreach (var item in procMaterialList)
                {
                    item.UpdatedBy = procMaterialGroupEntity.UpdatedBy;
                    item.UpdatedOn = procMaterialGroupEntity.UpdatedOn;
                }
                response = await _procMaterialRepository.UpdateProcMaterialGroupAsync(procMaterialList);

                if (response < procMaterialList.Count)
                {
                    throw new BusinessException(ErrorCode.MES10218);
                }

                ts.Complete();
            }

            #endregion

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteProcMaterialGroupAsync(long id)
        {
            await _procMaterialGroupRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesProcMaterialGroupAsync(long[] idsArr)
        {
            if (idsArr.Length<1)
            {
                throw new ValidationException(ErrorCode.MES10213);
            }

            //判断物料中是否有当前物料组
            var procMaterials = await _procMaterialRepository.GetByGroupIdsAsync(idsArr);
            if (procMaterials != null && procMaterials.Count() > 0)
            {
                throw new CustomerValidationException(ErrorCode.MES10221);
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
            //TODO 
            // TODO   procMaterialGroupPagedQueryDto.SiteCode = "";

            var procMaterialGroupPagedQuery = procMaterialGroupPagedQueryDto.ToQuery<ProcMaterialGroupPagedQuery>();
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
        /// <param name="customProcMaterialGroupPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<CustomProcMaterialGroupViewDto>> GetPageCustomListAsync(CustomProcMaterialGroupPagedQueryDto customProcMaterialGroupPagedQueryDto)
        {
            customProcMaterialGroupPagedQueryDto.SiteId = _currentSite.SiteId??0;

            var procMaterialGroupCustomPagedQuery = customProcMaterialGroupPagedQueryDto.ToQuery<ProcMaterialGroupCustomPagedQuery>();
            var pagedInfo = await _procMaterialGroupRepository.GetPagedCustomInfoAsync(procMaterialGroupCustomPagedQuery);

            //实体到DTO转换 装载数据
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
            var customProcMaterialGroupViewDtos = new List<CustomProcMaterialGroupViewDto>();
            foreach (var customProcMaterialGroupView in pagedInfo.Data)
            {
                var customProcMaterialGroupViewDto = customProcMaterialGroupView.ToModel<CustomProcMaterialGroupViewDto>();
                customProcMaterialGroupViewDtos.Add(customProcMaterialGroupViewDto);
            }

            return customProcMaterialGroupViewDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procMaterialGroupDto"></param>
        /// <returns></returns>
        public async Task ModifyProcMaterialGroupAsync(ProcMaterialGroupModifyDto procMaterialGroupModifyDto)
        {
            if (procMaterialGroupModifyDto == null)
            {
                throw new ValidationException(ErrorCode.MES10213);
            }
            // TODO SiteId  procMaterialGroupModifyDto.SiteCode = "";//TODO  App.GetSite();

            //DTO转换实体
            var procMaterialGroupEntity = procMaterialGroupModifyDto.ToEntity<ProcMaterialGroupEntity>();
            procMaterialGroupEntity.UpdatedBy = _currentUser.UserName;
            procMaterialGroupEntity.UpdatedOn = HymsonClock.Now();
            procMaterialGroupEntity.GroupCode = procMaterialGroupEntity.GroupCode.ToUpper();

            #region 检验
            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(procMaterialGroupModifyDto);

            //检验物料组是否存在
            var procMaterialGroup = await _procMaterialGroupRepository.GetByIdAsync(procMaterialGroupEntity.Id);
            if (procMaterialGroup == null)
            {
                throw new BusinessException(ErrorCode.MES10219);
            }

            //判断编号是否已存在
            var existGroupCodes = await _procMaterialGroupRepository.GetProcMaterialGroupEntitiesAsync(new ProcMaterialGroupQuery { SiteId = 0, GroupCode = procMaterialGroupEntity.GroupCode });
            if (existGroupCodes != null && existGroupCodes.Count() > 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10216)).WithData("GroupCode", procMaterialGroupEntity.GroupCode);
            }

            var procMaterialList = ConvertProcMaterialList(procMaterialGroupModifyDto.DynamicList, procMaterialGroupEntity.Id);
            var procMaterialIds = procMaterialList.Select(x => x.Id).ToArray();

            // 判断物料是否已被使用
            var procMaterials = await _procMaterialRepository.GetByIdsAsync(procMaterialIds);
            if (procMaterials.Where(x => x.GroupId != 0 && x.GroupId != procMaterialGroupEntity.Id).Count() > 0)
            {
                throw new CustomerValidationException(ErrorCode.MES10217);
            }
            #endregion

            #region 保存到数据库
            using (TransactionScope ts = new TransactionScope())
            {
                var response = 0;
                response = await _procMaterialGroupRepository.UpdateAsync(procMaterialGroupEntity);

                if (response == 0)
                {
                    throw new BusinessException(ErrorCode.MES10220);
                }

                //将之前所有该物料组的物料改为未绑定 
                await _procMaterialRepository.UpdateProcMaterialUnboundAsync(procMaterialGroupEntity.Id);

                //绑定本次的物料
                foreach (var item in procMaterialList)
                {
                    item.UpdatedBy = procMaterialGroupEntity.UpdatedBy;
                    item.UpdatedOn = procMaterialGroupEntity.UpdatedOn;
                }
                response = await _procMaterialRepository.UpdateProcMaterialGroupAsync(procMaterialList);
                if (response < procMaterialList.Count)
                {
                    throw new BusinessException(ErrorCode.MES10220);
                }
            }

            #endregion


        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcMaterialGroupDto> QueryProcMaterialGroupByIdAsync(long id)
        {
            var procMaterialGroupEntity = await _procMaterialGroupRepository.GetByIdAndSiteIdAsync(id, _currentSite.SiteId??0);
            if (procMaterialGroupEntity != null)
            {
                return procMaterialGroupEntity.ToModel<ProcMaterialGroupDto>();
            }
            return null;
        }


        #region 业务扩展方法
        /// <summary>
        /// 转换集合（物料组）
        /// </summary>
        /// <param name="dynamicList"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<ProcMaterialEntity> ConvertProcMaterialList(IEnumerable<string> dynamicList, long id)
        {
            if (dynamicList == null || dynamicList.Any() == false) return new List<ProcMaterialEntity> { };

            return dynamicList.Select(s => new ProcMaterialEntity
            {
                Id = ObjToLong(s),
                GroupId = id
            }).ToList();
        }

        public static long ObjToLong(object obj)
        {
            try
            {
                return long.Parse(obj.ToString());
            }
            catch
            {
                return 0L;
            }
        }

        #endregion
    }
}
