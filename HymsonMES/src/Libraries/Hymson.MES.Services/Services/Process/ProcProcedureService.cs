/*
 *creator: Karl
 *
 *describe: 工序表    服务 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-13 09:06:05
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.ResourceType;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process.IProcessService;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Transactions;

namespace Hymson.MES.Services.Services.Process
{
    /// <summary>
    /// 工序表 服务
    /// </summary>
    public class ProcProcedureService : IProcProcedureService
    {
        /// <summary>
        /// 当前登录用户对象
        /// </summary>
        private readonly ICurrentUser _currentUser;
        /// <summary>
        /// 当前站点
        /// </summary>
        private readonly ICurrentSite _currentSite;
        /// <summary>
        /// 工序表 仓储
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;
        /// <summary>
        /// 资源类型仓储
        /// </summary>
        private readonly IProcResourceTypeRepository _resourceTypeRepository;
        /// <summary>
        /// 工序配置作业表仓储
        /// </summary>
        private readonly IInteJobBusinessRelationRepository _jobBusinessRelationRepository;
        /// <summary>
        /// 工序配置打印表仓储
        /// </summary>
        private readonly IProcProcedurePrintRelationRepository _procedurePrintRelationRepository;
        /// <summary>
        /// 物料维护 仓储
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;
        /// <summary>
        /// 作业表仓储
        /// </summary>
        private readonly IInteJobRepository _inteJobRepository;

        private readonly AbstractValidator<ProcProcedureCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ProcProcedureModifyDto> _validationModifyRules;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ProcProcedureService(
            ICurrentUser currentUser, ICurrentSite currentSite,
            IProcProcedureRepository procProcedureRepository,
            IProcResourceTypeRepository resourceTypeRepository,
            IInteJobBusinessRelationRepository jobBusinessRelationRepository,
            IProcProcedurePrintRelationRepository procedurePrintRelationRepository,
            IProcMaterialRepository procMaterialRepository,
            IInteJobRepository inteJobRepository,
            AbstractValidator<ProcProcedureCreateDto> validationCreateRules,
            AbstractValidator<ProcProcedureModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _procProcedureRepository = procProcedureRepository;
            _resourceTypeRepository = resourceTypeRepository;
            _jobBusinessRelationRepository = jobBusinessRelationRepository;
            _procedurePrintRelationRepository = procedurePrintRelationRepository;
            _procMaterialRepository = procMaterialRepository;
            _inteJobRepository = inteJobRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="procProcedurePagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcProcedureViewDto>> GetPageListAsync(ProcProcedurePagedQueryDto procProcedurePagedQueryDto)
        {
            var procProcedurePagedQuery = procProcedurePagedQueryDto.ToQuery<ProcProcedurePagedQuery>();
            procProcedurePagedQuery.SiteId = _currentSite.SiteId??0;
            var pagedInfo = await _procProcedureRepository.GetPagedInfoAsync(procProcedurePagedQuery);

            //实体到DTO转换 装载数据
            List<ProcProcedureViewDto> procProcedureDtos = PrepareProcProcedureDtos(pagedInfo);
            return new PagedInfo<ProcProcedureViewDto>(procProcedureDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 分页实体转换
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ProcProcedureViewDto> PrepareProcProcedureDtos(PagedInfo<ProcProcedureView> pagedInfo)
        {
            var procProcedureDtos = new List<ProcProcedureViewDto>();
            foreach (var procProcedureEntity in pagedInfo.Data)
            {
                var procProcedureDto = procProcedureEntity.ToModel<ProcProcedureViewDto>();
                procProcedureDtos.Add(procProcedureDto);
            }

            return procProcedureDtos;
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<QueryProcProcedureDto> GetProcProcedureByIdAsync(long id)
        {
            QueryProcProcedureDto queryProcDto = new QueryProcProcedureDto();
            var procProcedureEntity = await _procProcedureRepository.GetByIdAsync(id);
            if (procProcedureEntity != null)
            {
                queryProcDto.Procedure = procProcedureEntity.ToModel<ProcProcedureDto>();
                if (procProcedureEntity.ResourceTypeId > 0)
                {
                    var resourceTypeId = procProcedureEntity.ResourceTypeId ?? 0;
                    var procResourceType = await _resourceTypeRepository.GetByIdAsync(resourceTypeId);
                    queryProcDto.ResourceType = new ProcResourceTypeDto
                    {
                        Id = procResourceType?.Id ?? 0,
                        ResType = procResourceType?.ResType ?? "",
                        ResTypeName = procResourceType?.ResTypeName ?? ""
                    };
                }
            }
            return queryProcDto;
        }

        /// <summary>
        /// 获取工序配置打印信息
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QueryProcProcedurePrintReleationDto>> GetProcedureConfigPrintListAsync(ProcProcedurePrintReleationPagedQueryDto queryDto)
        {
            var query = new ProcProcedurePrintReleationPagedQuery()
            {
                SiteId = _currentSite.SiteId??0,
                ProcedureId = queryDto.ProcedureId,
                PageIndex = queryDto.PageIndex,
                PageSize = queryDto.PageSize,
                Sorting = queryDto.Sorting,
            };
            var pagedInfo = await _procedurePrintRelationRepository.GetPagedInfoAsync(query);

            //实体到DTO转换 装载数据
            var dtos = new List<QueryProcProcedurePrintReleationDto>();
            if (pagedInfo.Data != null && pagedInfo.Data.Any())
            {
                var materialIds = pagedInfo.Data.ToList().Select(a => a.MaterialId).ToArray();
                var materialLsit = await _procMaterialRepository.GetByIdsAsync(materialIds);
                foreach (var entity in pagedInfo.Data)
                {
                    var printReleationDto = entity.ToModel<ProcProcedurePrintRelationDto>();
                    var material = materialLsit.FirstOrDefault(a => a.Id == printReleationDto.MaterialId)?.ToModel<ProcMaterialDto>();
                    var queryEntity = new QueryProcProcedurePrintReleationDto { ProcedureBomConfigPrint = printReleationDto, Material = material ?? new ProcMaterialDto() };
                    dtos.Add(queryEntity);
                    //TODO 模板 by wangkeming 
                }
            }

            return new PagedInfo<QueryProcProcedurePrintReleationDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 获取工序配置Job信息
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QueryProcedureJobReleationDto>> GetProcedureConfigJobListAsync(InteJobBusinessRelationPagedQueryDto queryDto)
        {
            var query = new InteJobBusinessRelationPagedQuery()
            {
                SiteId = _currentSite.SiteId??0,
                BusinessId = queryDto.BusinessId,
                BusinessType = InteJobBusinessTypeEnum.Procedure.ToString(),
                PageIndex = queryDto.PageIndex,
                PageSize = queryDto.PageSize,
                Sorting = queryDto.Sorting
            };
            var pagedInfo = await _jobBusinessRelationRepository.GetPagedInfoAsync(query);

            //实体到DTO转换 装载数据
            var dtos = new List<QueryProcedureJobReleationDto>();
            foreach (var entity in pagedInfo.Data)
            {
                var jobReleationDto = entity.ToModel<InteJobBusinessRelationDto>();
                dtos.Add(new QueryProcedureJobReleationDto { ProcedureBomConfigJob = jobReleationDto });
            }

            var jobIds = dtos.Select(a => a.ProcedureBomConfigJob.JobId).ToArray();
            var jobList = await _inteJobRepository.GetByIdsAsync(jobIds);
            var jobDtoList = new List<InteJobDto>();
            foreach (var job in jobList)
            {
                var inteJob = job.ToModel<InteJobDto>();
                jobDtoList.Add(inteJob);
            }

            foreach (var entity in dtos)
            {
                entity.Job = jobDtoList.FirstOrDefault(a => a.Id == entity.ProcedureBomConfigJob.JobId);
            }

            return new PagedInfo<QueryProcedureJobReleationDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task AddProcProcedureAsync(AddProcProcedureDto parm)
        {
            #region 验证
            if (parm == null)
            {
                throw new ValidationException(ErrorCode.MES10100);
            }

            var siteId = _currentSite.SiteId??0;
            var userName = _currentUser.UserName;
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(parm.Procedure);

            var code = parm.Procedure.Code.ToUpperInvariant();
            var query = new ProcProcedureQuery
            {
                SiteId = siteId,
                Code = code
            };
            if (await _procProcedureRepository.IsExistsAsync(query))
            {
                throw new BusinessException(ErrorCode.MES10405).WithData("Code", parm.Procedure.Code);
            }
            #endregion

            //DTO转换实体
            var procProcedureEntity = parm.Procedure.ToEntity<ProcProcedureEntity>();
            procProcedureEntity.Id = IdGenProvider.Instance.CreateId();
            procProcedureEntity.Code = code;
            procProcedureEntity.SiteId= siteId;
            procProcedureEntity.CreatedBy = userName;
            procProcedureEntity.UpdatedBy = userName;

            //打印
            List<ProcProcedurePrintRelationEntity> procedureConfigPrintList = new List<ProcProcedurePrintRelationEntity>();
            if (parm.ProcedurePrintList != null && parm.ProcedurePrintList.Count > 0)
            {
                foreach (var item in parm.ProcedurePrintList)
                {
                    var releationEntity = item.ToEntity<ProcProcedurePrintRelationEntity>(); ;
                    releationEntity.Id = IdGenProvider.Instance.CreateId();
                    releationEntity.ProcedureId = procProcedureEntity.Id;
                    releationEntity.SiteId = siteId;
                    releationEntity.CreatedBy = userName;
                    releationEntity.UpdatedBy = userName;
                    procedureConfigPrintList.Add(releationEntity);
                }
            }

            //job
            List<InteJobBusinessRelationEntity> procedureConfigJobList = new List<InteJobBusinessRelationEntity>();
            if (parm.ProcedureJobList != null && parm.ProcedureJobList.Count > 0)
            {
                foreach (var item in parm.ProcedureJobList)
                {
                    var relationEntity = item.ToEntity<InteJobBusinessRelationEntity>(); ;
                    relationEntity.Id = IdGenProvider.Instance.CreateId();
                    relationEntity.BusinessType = (int)InteJobBusinessTypeEnum.Procedure;
                    relationEntity.BusinessId = procProcedureEntity.Id;
                    relationEntity.SiteId = siteId;
                    relationEntity.CreatedBy = userName;
                    relationEntity.UpdatedBy = userName;
                    procedureConfigJobList.Add(relationEntity);
                }
            }

            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                //入库
                await _procProcedureRepository.InsertAsync(procProcedureEntity);

                if (procedureConfigPrintList != null && procedureConfigPrintList.Count > 0)
                {
                    await _procedurePrintRelationRepository.InsertRangeAsync(procedureConfigPrintList);
                }

                if (procedureConfigJobList != null && procedureConfigJobList.Count > 0)
                {
                    await _jobBusinessRelationRepository.InsertRangeAsync(procedureConfigJobList);
                }

                //提交
                ts.Complete();
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task UpdateProcProcedureAsync(UpdateProcProcedureDto parm)
        {
            #region
            if (parm == null)
            {
                throw new ValidationException(ErrorCode.MES10100);
            }
            var siteId = _currentSite.SiteId??0;
            var userName = _currentUser.UserName;

            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(parm.Procedure);
            #endregion

            //DTO转换实体
            var procProcedureEntity = parm.Procedure.ToEntity<ProcProcedureEntity>();
            procProcedureEntity.UpdatedBy = _currentUser.UserName;

            //TODO 现在关联表批量删除批量新增，后面再修改
            //打印
            List<ProcProcedurePrintRelationEntity> procedureConfigPrintList = new List<ProcProcedurePrintRelationEntity>();
            if (parm.ProcedurePrintList != null && parm.ProcedurePrintList.Count > 0)
            {
                foreach (var item in parm.ProcedurePrintList)
                {
                    var releationEntity = item.ToEntity<ProcProcedurePrintRelationEntity>(); ;
                    releationEntity.Id = IdGenProvider.Instance.CreateId();
                    releationEntity.ProcedureId = procProcedureEntity.Id;
                    releationEntity.SiteId = siteId;
                    releationEntity.CreatedBy = userName;
                    releationEntity.UpdatedBy = userName;
                    procedureConfigPrintList.Add(releationEntity);
                }
            }

            //job
            List<InteJobBusinessRelationEntity> procedureConfigJobList = new List<InteJobBusinessRelationEntity>();
            if (parm.ProcedureJobList != null && parm.ProcedureJobList.Count > 0)
            {
                foreach (var item in parm.ProcedureJobList)
                {
                    var relationEntity = item.ToEntity<InteJobBusinessRelationEntity>(); ;
                    relationEntity.Id = IdGenProvider.Instance.CreateId();
                    relationEntity.BusinessType = (int)InteJobBusinessTypeEnum.Procedure;
                    relationEntity.BusinessId = procProcedureEntity.Id;
                    relationEntity.SiteId = siteId;
                    relationEntity.CreatedBy = userName;
                    relationEntity.UpdatedBy = userName;
                    procedureConfigJobList.Add(relationEntity);
                }
            }

            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                //入库
                await _procProcedureRepository.UpdateAsync(procProcedureEntity);

                await _procedurePrintRelationRepository.DeleteByProcedureIdAsync(procProcedureEntity.Id);
                if (procedureConfigPrintList != null && procedureConfigPrintList.Count > 0)
                {
                    await _procedurePrintRelationRepository.InsertRangeAsync(procedureConfigPrintList);
                }

                await _jobBusinessRelationRepository.DeleteByBusinessIdAsync(procProcedureEntity.Id);
                if (procedureConfigJobList != null && procedureConfigJobList.Count > 0)
                {
                    await _jobBusinessRelationRepository.InsertRangeAsync(procedureConfigJobList);
                }

                //提交
                ts.Complete();
            }
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeleteProcProcedureAsync(string ids)
        {
            var idsArr = StringExtension.SpitLongArrary(ids);
            if (idsArr.Length < 1)
            {
                throw new NotFoundException(ErrorCode.MES10102);
            }

            var command = new DeleteCommand
            {
                UserId = _currentUser.UserName,
                DeleteOn = HymsonClock.Now(),
                Ids= idsArr
            };
            return await _procProcedureRepository.DeleteRangeAsync(command);
        }
    }
}
