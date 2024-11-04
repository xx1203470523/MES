using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Process;
using Hymson.Snowflake;
using Hymson.Utils;
using System.Transactions;

namespace Hymson.MES.Services.Services.Process
{
    /// <summary>
    /// 标准参数关联类型表 服务
    /// </summary>
    public class ProcParameterLinkTypeService : IProcParameterLinkTypeService
    {
        /// <summary>
        /// 标准参数关联类型表 仓储
        /// </summary>
        private readonly IProcParameterLinkTypeRepository _procParameterLinkTypeRepository;
        private readonly AbstractValidator<ProcParameterLinkTypeCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ProcParameterLinkTypeModifyDto> _validationModifyRules;

        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="procParameterLinkTypeRepository"></param>
        /// <param name="validationCreateRules"></param>
        /// <param name="validationModifyRules"></param>
        /// <param name="currentSite"></param>
        public ProcParameterLinkTypeService(ICurrentUser currentUser, IProcParameterLinkTypeRepository procParameterLinkTypeRepository, AbstractValidator<ProcParameterLinkTypeCreateDto> validationCreateRules, AbstractValidator<ProcParameterLinkTypeModifyDto> validationModifyRules, ICurrentSite currentSite)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _procParameterLinkTypeRepository = procParameterLinkTypeRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="procParameterLinkTypeCreateDto"></param>
        /// <returns></returns>
        public async Task CreateProcParameterLinkTypeAsync(ProcParameterLinkTypeCreateDto procParameterLinkTypeCreateDto)
        {
            // 检查SiteId
            if (_currentSite.SiteId == 0 || _currentSite.SiteId == null) throw new BusinessException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(procParameterLinkTypeCreateDto);

            // 转换数据
            var links = procParameterLinkTypeCreateDto.Parameters.Select(s => new ProcParameterLinkTypeEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                CreatedBy = _currentUser.UserName,
                UpdatedBy = _currentUser.UserName,
                CreatedOn = HymsonClock.Now(),
                UpdatedOn = HymsonClock.Now(),
                ParameterType = procParameterLinkTypeCreateDto.ParameterType ?? ParameterTypeEnum.Equipment,
                ParameterID = s,
                SiteId = _currentSite.SiteId ?? 123456
            });

            var currentEntities = await _procParameterLinkTypeRepository.GetProcParameterLinkTypeEntitiesAsync(new ProcParameterLinkTypeQuery
            {
                SiteId = _currentSite.SiteId,
                ParameterType = procParameterLinkTypeCreateDto.ParameterType ?? ParameterTypeEnum.Equipment
            });
            var adds = links.Where(w => currentEntities.Any(e => e.ParameterID == w.ParameterID) == false);

            await _procParameterLinkTypeRepository.InsertsAsync(adds);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procParameterLinkTypeModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyProcParameterLinkTypeAsync(ProcParameterLinkTypeModifyDto procParameterLinkTypeModifyDto)
        {
            if (procParameterLinkTypeModifyDto == null)
            {
                throw new ValidationException(nameof(ErrorCode.MES10100));
            }

            //检查SiteId
            if (_currentSite.SiteId == 0)
            {
                throw new BusinessException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(procParameterLinkTypeModifyDto);

            var links = procParameterLinkTypeModifyDto.Parameters.Select(s =>
            {
                var item = new ProcParameterLinkTypeEntity();
                //item.Id = IdGenProvider.Instance.CreateId();
                //item.CreatedBy = _currentUser.UserName;
                item.UpdatedBy = _currentUser.UserName;
                //item.CreatedOn = HymsonClock.Now();
                item.UpdatedOn = HymsonClock.Now();
                item.ParameterType = procParameterLinkTypeModifyDto.ParameterType;
                item.ParameterID = s;
                return item;
            }).ToList();

            var current = await _procParameterLinkTypeRepository.GetProcParameterLinkTypeEntitiesAsync(new ProcParameterLinkTypeQuery
            {
                SiteId = _currentSite.SiteId,
                ParameterType = procParameterLinkTypeModifyDto.ParameterType
            });

            var adds = links.Where(w => current.Any(e => e.ParameterID == w.ParameterID) == false)
                   .Select(s =>
                   {
                       s.Id = IdGenProvider.Instance.CreateId();
                       s.CreatedBy = _currentUser.UserName;
                       s.CreatedOn = HymsonClock.Now();
                       return s;
                   })
                   .ToList();

            var deletes = current.Where(w => links.Any(e => e.ParameterID == w.ParameterID) == false).ToList();

            links.RemoveAll(w => adds.Any(e => e.ParameterID == w.ParameterID) == true);

            using (TransactionScope ts = new TransactionScope())
            {
                var response = 0;
                if (adds.Count > 0)
                {
                    response = await _procParameterLinkTypeRepository.InsertsAsync(adds);
                    if (response == 0)
                    {
                        throw new BusinessException(nameof(ErrorCode.MES10507));
                    }
                }

                if (deletes.Count > 0)
                {
                    response = await _procParameterLinkTypeRepository.DeletesTrueAsync(deletes.Select(x => x.Id).ToArray());
                    if (response == 0)
                    {
                        throw new BusinessException(nameof(ErrorCode.MES10507));
                    }
                }

                response = await _procParameterLinkTypeRepository.UpdatesAsync(links);
                if (response == 0)
                {
                    throw new BusinessException(nameof(ErrorCode.MES10507));
                }

                ts.Complete();
            }

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteProcParameterLinkTypeAsync(long id)
        {
            await _procParameterLinkTypeRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesProcParameterLinkTypeAsync(long[] idsArr)
        {
            if (idsArr.Length < 1)
            {
                throw new ValidationException(nameof(ErrorCode.MES10100));
            }

            return await _procParameterLinkTypeRepository.DeletesAsync(new DeleteCommand { Ids = idsArr, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcParameterLinkTypeDto> QueryProcParameterLinkTypeByIdAsync(long id)
        {
            var procParameterLinkTypeEntity = await _procParameterLinkTypeRepository.GetByIdAsync(id);
            if (procParameterLinkTypeEntity != null)
            {
                return procParameterLinkTypeEntity.ToModel<ProcParameterLinkTypeDto>();
            }
            return null;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="procParameterLinkTypePagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcParameterLinkTypeViewDto>> GetPageListAsync(ProcParameterLinkTypePagedQueryDto procParameterLinkTypePagedQueryDto)
        {
            var procParameterLinkTypePagedQuery = procParameterLinkTypePagedQueryDto.ToQuery<ProcParameterLinkTypePagedQuery>();
            procParameterLinkTypePagedQuery.SiteId = _currentSite.SiteId;
            var pagedInfo = await _procParameterLinkTypeRepository.GetPagedInfoAsync(procParameterLinkTypePagedQuery);

            //实体到DTO转换 装载数据
            List<ProcParameterLinkTypeViewDto> procParameterLinkTypeDtos = PrepareProcParameterLinkTypeDtos(pagedInfo);
            return new PagedInfo<ProcParameterLinkTypeViewDto>(procParameterLinkTypeDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 分页查询详情（设备/产品参数）
        /// </summary>
        /// <param name="procParameterDetailPagerQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcParameterLinkTypeViewDto>> QueryPagedProcParameterLinkTypeByTypeAsync(ProcParameterDetailPagerQueryDto procParameterDetailPagerQueryDto)
        {
            var procParameterDetailPagerQuery = procParameterDetailPagerQueryDto.ToQuery<ProcParameterDetailPagerQuery>();
            procParameterDetailPagerQuery.SiteId = _currentSite.SiteId;
            var pagedInfo = await _procParameterLinkTypeRepository.GetPagedProcParameterLinkTypeByTypeAsync(procParameterDetailPagerQuery);

            //实体到DTO转换 装载数据
            List<ProcParameterLinkTypeViewDto> procParameterLinkTypeDtos = PrepareProcParameterLinkTypeDtos(pagedInfo);
            return new PagedInfo<ProcParameterLinkTypeViewDto>(procParameterLinkTypeDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ProcParameterLinkTypeViewDto> PrepareProcParameterLinkTypeDtos(PagedInfo<ProcParameterLinkTypeView> pagedInfo)
        {
            var procParameterLinkTypeDtos = new List<ProcParameterLinkTypeViewDto>();
            foreach (var procParameterLinkTypeEntity in pagedInfo.Data)
            {
                var procParameterLinkTypeDto = procParameterLinkTypeEntity.ToModel<ProcParameterLinkTypeViewDto>();
                procParameterLinkTypeDtos.Add(procParameterLinkTypeDto);
            }

            return procParameterLinkTypeDtos;
        }

    }
}
