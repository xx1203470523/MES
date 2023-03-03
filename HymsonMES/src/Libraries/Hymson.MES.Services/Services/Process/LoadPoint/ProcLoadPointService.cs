/*
 *creator: Karl
 *
 *describe: 上料点表    服务 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-17 08:57:53
 */
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
using Microsoft.AspNetCore.Http;
using Mysqlx.Crud;
using System.Runtime.CompilerServices;
using System.Transactions;

namespace Hymson.MES.Services.Services.Process
{
    /// <summary>
    /// 上料点表 服务
    /// </summary>
    public class ProcLoadPointService : IProcLoadPointService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 上料点表 仓储
        /// </summary>
        private readonly IProcLoadPointRepository _procLoadPointRepository;
        private readonly AbstractValidator<ProcLoadPointCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ProcLoadPointModifyDto> _validationModifyRules;

        private readonly IProcLoadPointLinkMaterialRepository _procLoadPointLinkMaterialRepository;
        private readonly IProcLoadPointLinkResourceRepository _procLoadPointLinkResourceRepository;

        public ProcLoadPointService(ICurrentUser currentUser,IProcLoadPointRepository procLoadPointRepository, AbstractValidator<ProcLoadPointCreateDto> validationCreateRules, AbstractValidator<ProcLoadPointModifyDto> validationModifyRules, IProcLoadPointLinkMaterialRepository procLoadPointLinkMaterialRepository, IProcLoadPointLinkResourceRepository procLoadPointLinkResourceRepository, ICurrentSite currentSite)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _procLoadPointRepository = procLoadPointRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _procLoadPointLinkMaterialRepository = procLoadPointLinkMaterialRepository;
            _procLoadPointLinkResourceRepository = procLoadPointLinkResourceRepository;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="procLoadPointDto"></param>
        /// <returns></returns>
        public async Task CreateProcLoadPointAsync(ProcLoadPointCreateDto procLoadPointCreateDto)
        {
            if (procLoadPointCreateDto == null) 
            {
                throw new ValidationException(ErrorCode.MES10100);
            }

            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(procLoadPointCreateDto);
            if (procLoadPointCreateDto.LinkMaterials!=null&&procLoadPointCreateDto.LinkMaterials.Any(a => string.IsNullOrWhiteSpace(a.MaterialId.ToString())))
            {
                throw new ValidationException(ErrorCode.MES10702);
            }
            if (procLoadPointCreateDto.LinkMaterials != null && procLoadPointCreateDto.LinkMaterials.Any(a => string.IsNullOrWhiteSpace(a.MaterialCode)))
            {
                throw new ValidationException(ErrorCode.MES10702);
            }

            if (procLoadPointCreateDto.LinkResources != null && procLoadPointCreateDto.LinkResources.Any(a => string.IsNullOrWhiteSpace(a.ResourceId.ToString())))
            {
                throw new ValidationException(ErrorCode.MES10703);
            }

            if (procLoadPointCreateDto.LinkResources != null && procLoadPointCreateDto.LinkResources.Any(a => string.IsNullOrWhiteSpace(a.ResCode)))
            {
                throw new ValidationException(ErrorCode.MES10703);
            }

            //DTO转换实体
            var procLoadPointEntity = procLoadPointCreateDto.ToEntity<ProcLoadPointEntity>();
            procLoadPointEntity.Id= IdGenProvider.Instance.CreateId();
            procLoadPointEntity.CreatedBy = _currentUser.UserName;
            procLoadPointEntity.UpdatedBy = _currentUser.UserName;
            procLoadPointEntity.CreatedOn = HymsonClock.Now();
            procLoadPointEntity.UpdatedOn = HymsonClock.Now();

            procLoadPointEntity.SiteId = _currentSite.SiteId??0;

            #region 数据库验证
            var isExists = (await _procLoadPointRepository.GetProcLoadPointEntitiesAsync(new ProcLoadPointQuery()
            {
                SiteId = procLoadPointEntity.SiteId,
                LoadPoint = procLoadPointEntity.LoadPoint
            })).Any();  
            if (isExists == true)
            {
                throw new BusinessException(ErrorCode.MES10701).WithData("LoadPoint", procLoadPointEntity.LoadPoint);
            }

            #endregion

            #region 准备数据
            //上料点关联物料列表
            var linkMaterials = new List<ProcLoadPointLinkMaterialEntity>();
            if (procLoadPointCreateDto.LinkMaterials != null && procLoadPointCreateDto.LinkMaterials.Any())
            {
                foreach (var material in procLoadPointCreateDto.LinkMaterials)
                {
                    linkMaterials.Add(new ProcLoadPointLinkMaterialEntity
                    {
                        SiteId = procLoadPointEntity.SiteId,
                        LoadPointId = procLoadPointEntity.Id,
                        MaterialId = material.MaterialId.ParseToLong(),
                        Version = material.Version,
                        ReferencePoint = material.ReferencePoint,
                        CreatedBy = _currentUser.UserName,
                        CreatedOn = HymsonClock.Now()
                    });
                }
            }
            
            //上料点关联资源列表
            var linkResources = new List<ProcLoadPointLinkResourceEntity>();
            if (procLoadPointCreateDto.LinkResources != null && procLoadPointCreateDto.LinkResources.Any())
            {
                foreach (var resource in procLoadPointCreateDto.LinkResources)
                {
                    linkResources.Add(new ProcLoadPointLinkResourceEntity
                    {
                        SiteId = procLoadPointEntity.SiteId,
                        LoadPointId = procLoadPointEntity.Id,
                        ResourceId = resource.ResourceId.ParseToLong(),
                        CreatedBy = _currentUser.UserName,
                        CreatedOn = HymsonClock.Now()
                    });
                }
            }
            #endregion

            using (TransactionScope ts = new TransactionScope())
            {
                int response = 0;
                //入库
                response = await _procLoadPointRepository.InsertAsync(procLoadPointEntity);

                if (response == 0) 
                {
                    throw new BusinessException(ErrorCode.MES10704);
                }

                if (linkMaterials.Count > 0)
                {
                    response = await _procLoadPointLinkMaterialRepository.InsertsAsync(linkMaterials);

                    if (response <= 0) 
                    {
                        throw new BusinessException(ErrorCode.MES10704);
                    }
                }
                if (linkResources.Count > 0)
                {
                    await _procLoadPointLinkResourceRepository.InsertsAsync(linkResources);

                    if (response <= 0)
                    {
                        throw new BusinessException(ErrorCode.MES10704);
                    }
                }

                ts.Complete();
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteProcLoadPointAsync(long id)
        {
            await _procLoadPointRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesProcLoadPointAsync(long[] idsArr)
        {
            if (idsArr.Length < 1)
            {
                throw new ValidationException(ErrorCode.MES10707);
            }

            var loadPoints= await _procLoadPointRepository.GetByIdsAsync(idsArr);
            if (loadPoints.Any(x => (SysDataStatusEnum.Enable == x.Status || SysDataStatusEnum.Retain == x.Status)))
            {
                throw new BusinessException(ErrorCode.MES10709);
            }

            int response = 0;
            using (TransactionScope ts = new TransactionScope())
            {
                
                response = await _procLoadPointRepository.DeletesAsync(new DeleteCommand { Ids = idsArr, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
                if (response <= 0)
                {
                    throw new BusinessException(ErrorCode.MES10708);
                }

                await _procLoadPointLinkMaterialRepository.DeletesByLoadPointIdTrueAsync(idsArr);

                await _procLoadPointLinkResourceRepository.DeletesByLoadPointIdTrueAsync(idsArr);

                ts.Complete();
            }
            return response;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="procLoadPointPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcLoadPointDto>> GetPageListAsync(ProcLoadPointPagedQueryDto procLoadPointPagedQueryDto)
        {
            var procLoadPointPagedQuery = procLoadPointPagedQueryDto.ToQuery<ProcLoadPointPagedQuery>();
            procLoadPointPagedQuery.SiteId = _currentSite.SiteId??0;
            var pagedInfo = await _procLoadPointRepository.GetPagedInfoAsync(procLoadPointPagedQuery);

            //实体到DTO转换 装载数据
            List<ProcLoadPointDto> procLoadPointDtos = PrepareProcLoadPointDtos(pagedInfo);
            return new PagedInfo<ProcLoadPointDto>(procLoadPointDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ProcLoadPointDto> PrepareProcLoadPointDtos(PagedInfo<ProcLoadPointEntity>   pagedInfo)
        {
            var procLoadPointDtos = new List<ProcLoadPointDto>();
            foreach (var procLoadPointEntity in pagedInfo.Data)
            {
                var procLoadPointDto = procLoadPointEntity.ToModel<ProcLoadPointDto>();
                procLoadPointDtos.Add(procLoadPointDto);
            }

            return procLoadPointDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procLoadPointDto"></param>
        /// <returns></returns>
        public async Task ModifyProcLoadPointAsync(ProcLoadPointModifyDto procLoadPointModifyDto)
        {
            if (procLoadPointModifyDto == null)
            {
                throw new ValidationException(ErrorCode.MES10100);
            }

            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(procLoadPointModifyDto);
            if (procLoadPointModifyDto.LinkMaterials != null && procLoadPointModifyDto.LinkMaterials.Any(a => string.IsNullOrWhiteSpace(a.MaterialId.ToString())))
            {
                throw new ValidationException(ErrorCode.MES10702);
            }
            if (procLoadPointModifyDto.LinkMaterials != null && procLoadPointModifyDto.LinkMaterials.Any(a => string.IsNullOrWhiteSpace(a.MaterialCode)))
            {
                throw new ValidationException(ErrorCode.MES10702);
            }

            if (procLoadPointModifyDto.LinkResources != null && procLoadPointModifyDto.LinkResources.Any(a => string.IsNullOrWhiteSpace(a.ResourceId.ToString())))
            {
                throw new ValidationException(ErrorCode.MES10703);
            }

            if (procLoadPointModifyDto.LinkResources != null && procLoadPointModifyDto.LinkResources.Any(a => string.IsNullOrWhiteSpace(a.ResCode)))
            {
                throw new ValidationException(ErrorCode.MES10703);
            }

            //DTO转换实体
            var procLoadPointEntity = procLoadPointModifyDto.ToEntity<ProcLoadPointEntity>();
            procLoadPointEntity.UpdatedBy = _currentUser.UserName;
            procLoadPointEntity.UpdatedOn = HymsonClock.Now();
            procLoadPointEntity.SiteId = _currentSite.SiteId??0;

            #region 数据库验证
            var modelOrigin = await _procLoadPointRepository.GetByIdAsync(procLoadPointModifyDto.Id);
            if (modelOrigin == null)
            {
                throw new ValidationException(ErrorCode.MES10705);
            }
            #endregion


            #region 组装数据
            //上料点关联物料列表
            var linkMaterials = new List<ProcLoadPointLinkMaterialEntity>();
            if (procLoadPointModifyDto.LinkMaterials != null && procLoadPointModifyDto.LinkMaterials.Any())
            {
                foreach (var material in procLoadPointModifyDto.LinkMaterials)
                {
                    linkMaterials.Add(new ProcLoadPointLinkMaterialEntity
                    {
                        SiteId = procLoadPointEntity.SiteId,
                        LoadPointId = procLoadPointEntity.Id,
                        MaterialId = material.MaterialId.ParseToLong(),
                        Version = material.Version,
                        ReferencePoint = material.ReferencePoint,
                        CreatedBy = _currentUser.UserName,
                        CreatedOn = HymsonClock.Now()
                });
                }
            }

            //上料点关联资源列表
            var linkResources = new List<ProcLoadPointLinkResourceEntity>();
            if (procLoadPointModifyDto.LinkResources != null && procLoadPointModifyDto.LinkResources.Any())
            {
                foreach (var resource in procLoadPointModifyDto.LinkResources)
                {
                    linkResources.Add(new ProcLoadPointLinkResourceEntity
                    {
                        SiteId = procLoadPointEntity.SiteId,
                        LoadPointId = procLoadPointEntity.Id,
                        ResourceId = resource.ResourceId,
                        CreatedBy = _currentUser.UserName,
                        CreatedOn = HymsonClock.Now()
                    });
                }
            }


            #endregion

            using (TransactionScope ts = new TransactionScope())
            {
                int response = 0;
                //入库
                response = await _procLoadPointRepository.UpdateAsync(procLoadPointEntity);

                if (response == 0)
                {
                    throw new BusinessException(ErrorCode.MES10706);
                }

                await _procLoadPointLinkMaterialRepository.DeletesByLoadPointIdTrueAsync(new long[] { procLoadPointEntity.Id });
                if (linkMaterials.Count > 0)
                {
                    response = await _procLoadPointLinkMaterialRepository.InsertsAsync(linkMaterials);

                    if (response <= 0)
                    {
                        throw new BusinessException(ErrorCode.MES10706);
                    }
                }
                await _procLoadPointLinkResourceRepository.DeletesByLoadPointIdTrueAsync(new long[] { procLoadPointEntity.Id });
                if (linkResources.Count > 0)
                {
                    await _procLoadPointLinkResourceRepository.InsertsAsync(linkResources);

                    if (response <= 0)
                    {
                        throw new BusinessException(ErrorCode.MES10706);
                    }
                }
                ts.Complete();
            }
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcLoadPointDetailDto> QueryProcLoadPointByIdAsync(long id) 
        {
           var procLoadPointEntity = await _procLoadPointRepository.GetByIdAsync(id);
           if (procLoadPointEntity == null) 
           {
                return new ProcLoadPointDetailDto();

               //return procLoadPointEntity.ToModel<ProcLoadPointDto>();
           }

            ProcLoadPointDetailDto loadPointDto = new ProcLoadPointDetailDto
            {
                Id = procLoadPointEntity.Id,
                SiteId = procLoadPointEntity.SiteId,
                LoadPoint = procLoadPointEntity.LoadPoint,
                LoadPointName = procLoadPointEntity.LoadPointName,
                Status = procLoadPointEntity.Status,
                Remark = procLoadPointEntity.Remark,
                LinkMaterials = new List<ProcLoadPointLinkMaterialViewDto>(),
                LinkResources = new List<ProcLoadPointLinkResourceViewDto>()
            };

            //上料点关联物料
            var loadPointLinkMaterials = await _procLoadPointLinkMaterialRepository.GetLoadPointLinkMaterialAsync(new long[] { id});
            if (loadPointLinkMaterials != null && loadPointLinkMaterials.Count() > 0)
            {
                loadPointDto.LinkMaterials =  PrepareEntityToDto<ProcLoadPointLinkMaterialView, ProcLoadPointLinkMaterialViewDto>(loadPointLinkMaterials);
            }

            //上料点关联资源
            var loadPointLinkResources = await _procLoadPointLinkResourceRepository.GetLoadPointLinkResourceAsync(new long[] { id } );
            if (loadPointLinkResources != null && loadPointLinkResources.Count() > 0)
            {
                loadPointDto.LinkResources = PrepareEntityToDto<ProcLoadPointLinkResourceView, ProcLoadPointLinkResourceViewDto>(loadPointLinkResources);
            }


            return loadPointDto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sources"></param>
        /// <returns></returns>
        private static List<ToT> PrepareEntityToDto<SourceT,ToT>(IEnumerable<SourceT> sources) where SourceT : BaseEntity
            where ToT : BaseEntityDto
        {
            var toTs = new List<ToT>();
            foreach (var source in sources)
            {
                var toT = source.ToModel<ToT>();
                toTs.Add(toT);
            }

            return toTs;
        }
    }
}
