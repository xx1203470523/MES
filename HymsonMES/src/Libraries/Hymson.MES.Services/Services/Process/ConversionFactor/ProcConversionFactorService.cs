using Elastic.Clients.Elasticsearch;
using FluentValidation;
using FluentValidation.Results;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuFeeding;
using Hymson.MES.Data.Repositories.Parameter;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.LoadPointLink.Query;
using Hymson.MES.Data.Repositories.Process.ProductSet.Query;
using Hymson.MES.Data.Repositories.Process.ResourceType;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.Snowflake;
using Hymson.SqlActuator.Services;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Security.Policy;
using System.Transactions;

namespace Hymson.MES.Services.Services.Process.Procedure
{
    /// <summary>
    /// 转换系数 服务
    /// </summary>
    public partial class ProcConversionFactorService : IProcConversionFactorService
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
        /// 转换系数表 仓储
        /// </summary>
        private readonly IProcConversionFactorRepository _procConversionFactorRepository;
        /// <summary>
        /// 资源仓储
        /// </summary>
        private readonly IProcResourceRepository _resourceRepository;
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
        /// <summary>
        /// 仓库标签模板 仓储
        /// </summary>
        private readonly IProcLabelTemplateRepository _procLabelTemplateRepository;
        /// <summary>
        /// 产出设置
        /// </summary>
        private readonly IProcProductSetRepository _procProductSetRepository;

        /// <summary>
        /// 多语言服务
        /// </summary>
        private readonly ILocalizationService _localizationService;
        /// <summary>
        /// sql执行器
        /// </summary>
        private readonly ISqlExecuteTaskService _sqlExecuteTaskService;

        /// <summary>
        /// 参数收集仓储
        /// </summary>
        private readonly IManuProductParameterRepository _manuProductParameterRepository;

        private readonly AbstractValidator<ProcProcedureCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ProcProcedureModifyDto> _validationModifyRules;

        /// <summary>
        /// 仓储接口（工序复投设置）
        /// </summary>
        private readonly IProcProcedureRejudgeRepository _procProcedureRejudgeRepository;

        private readonly IQualUnqualifiedCodeRepository _qualUnqualifiedCodeRepository;

        private readonly IProcLoadPointLinkMaterialRepository _procLoadPointLinkMaterialRepository;
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProcConversionFactorService(
            ICurrentUser currentUser, ICurrentSite currentSite,
            IProcProcedureRepository procProcedureRepository,
            IProcResourceRepository resourceRepository,
            IProcConversionFactorRepository procConversionFactorRepository,
            IProcResourceTypeRepository resourceTypeRepository,
            IInteJobBusinessRelationRepository jobBusinessRelationRepository,
            IProcProcedurePrintRelationRepository procedurePrintRelationRepository,
            IProcMaterialRepository procMaterialRepository,
            IInteJobRepository inteJobRepository,
            IProcLabelTemplateRepository procLabelTemplateRepository,
            IProcProductSetRepository procProductSetRepository,
            IManuProductParameterRepository manuProductParameterRepository,
            IQualUnqualifiedCodeRepository qualUnqualifiedCodeRepository,
            IProcProcedureRejudgeRepository procProcedureRejudgeRepository,
            AbstractValidator<ProcProcedureCreateDto> validationCreateRules,
            AbstractValidator<ProcProcedureModifyDto> validationModifyRules, ILocalizationService localizationService, ISqlExecuteTaskService sqlExecuteTaskService)
        {
            _procConversionFactorRepository= procConversionFactorRepository;
            _currentUser = currentUser;
            _currentSite = currentSite;
            _procProcedureRepository = procProcedureRepository;
            _resourceRepository = resourceRepository;
            _resourceTypeRepository = resourceTypeRepository;
            _jobBusinessRelationRepository = jobBusinessRelationRepository;
            _procedurePrintRelationRepository = procedurePrintRelationRepository;
            _procMaterialRepository = procMaterialRepository;
            _inteJobRepository = inteJobRepository;
            _procLabelTemplateRepository = procLabelTemplateRepository;
            _procProductSetRepository = procProductSetRepository;
            _manuProductParameterRepository = manuProductParameterRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _localizationService = localizationService;
            _sqlExecuteTaskService = sqlExecuteTaskService;
            _procProcedureRejudgeRepository = procProcedureRejudgeRepository;
            _qualUnqualifiedCodeRepository = qualUnqualifiedCodeRepository;
        }


        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="procProcedurePagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcConversionFactorViewDto>> GetPageListAsync(ProcConversionFactorPagedQueryDto procProcedurePagedQueryDto)
        {
            try
            {
                var procProcedurePagedQuery = procProcedurePagedQueryDto.ToQuery<IProcConversionFactorPagedQuery>();
                procProcedurePagedQuery.SiteId = _currentSite.SiteId ?? 0;
                var pagedInfo = await _procConversionFactorRepository.GetPagedInfoAsync(procProcedurePagedQuery);
                //实体到DTO转换 装载数据
                List<ProcConversionFactorViewDto> procProcedureDtos = PrepareProcConversionFactorDtos(pagedInfo);
                return new PagedInfo<ProcConversionFactorViewDto>(procProcedureDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
            }
            catch (Exception ex)
            {

                throw new CustomerValidationException(ex.ToString());
            }

        }

        /// <summary>
        /// 分页查询工艺路线的工序列表
        /// </summary>
        /// <param name="procProcedurePagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcProcedureDto>> GetPagedInfoByProcessRouteIdAsync(ProcProcedurePagedQueryDto procProcedurePagedQueryDto)
        {
            var procProcedurePagedQuery = procProcedurePagedQueryDto.ToQuery<ProcProcedurePagedQuery>();
            procProcedurePagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _procProcedureRepository.GetPagedInfoByProcessRouteIdAsync(procProcedurePagedQuery);

            //实体到DTO转换 装载数据
            var procProcedureDtos = new List<ProcProcedureDto>();
            foreach (var procProcedureEntity in pagedInfo.Data)
            {
                var procProcedureDto = procProcedureEntity.ToModel<ProcProcedureDto>();
                procProcedureDtos.Add(procProcedureDto);
            }
            return new PagedInfo<ProcProcedureDto>(procProcedureDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
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
        /// 分页实体转换
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ProcConversionFactorViewDto> PrepareProcConversionFactorDtos(PagedInfo<ProcConversionFactorView> pagedInfo)
        {
            var procProcedureDtos = new List<ProcConversionFactorViewDto>();
            foreach (var procProcedureEntity in pagedInfo.Data)
            {
                var procProcedureDto = procProcedureEntity.ToModel<ProcConversionFactorViewDto>();
                procProcedureDtos.Add(procProcedureDto);
            }

            return procProcedureDtos;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sources"></param>
        /// <returns></returns>
        private static List<ToT> PrepareEntityToDto<SourceT, ToT>(IEnumerable<SourceT> sources) where SourceT : BaseEntity
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

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcConversionFactorDto> QueryProcLoadPointByIdAsync(long id)
        {
            var procConversionFactorEntity = await _procConversionFactorRepository.GetByIdAsync(id);
            if (procConversionFactorEntity == null)
            {
                return new ProcConversionFactorDto();

            }

            ProcConversionFactorDto converdionFactorDto = new ProcConversionFactorDto
            {
                Id = procConversionFactorEntity.Id,
                OpenStatus= procConversionFactorEntity.OpenStatus,
                conversionFactor= procConversionFactorEntity.ConversionFactor,
                procedureId= procConversionFactorEntity.ProcedureId,
                MaterialId=procConversionFactorEntity.MaterialId
            };
            //关连的工序
            var ConversionFactorLinkProcedure = await _procProcedureRepository.GetByIdAsync(procConversionFactorEntity.ProcedureId);
            if (ConversionFactorLinkProcedure!= null) 
            {
                converdionFactorDto.code = ConversionFactorLinkProcedure.Code;
                converdionFactorDto.name = ConversionFactorLinkProcedure.Name;
            }
            //关联物料
            //var ConversionFactorLinkMaterials = await _procLoadPointLinkMaterialRepository.GetLoadPointLinkMaterialAsync(new long[] { procConversionFactorEntity.ProcedureId });
            var ConversionFactorLinkMaterials = await _procMaterialRepository.GetByIdAsync(procConversionFactorEntity.MaterialId);
            if (ConversionFactorLinkMaterials != null)
            {
                // 创建新的 ProcLoadPointLinkMaterialDto 对象并赋值
                var materialDto = new ProcLoadPointLinkMaterialDto
                {
                    MaterialId = ConversionFactorLinkMaterials.Id,
                    MaterialCode = ConversionFactorLinkMaterials.MaterialCode,
                    MaterialName = ConversionFactorLinkMaterials.MaterialName

                };

                // 确保 LinkMaterials 不为 null
                if (converdionFactorDto.LinkMaterials == null)
                {
                    converdionFactorDto.LinkMaterials = new List<ProcLoadPointLinkMaterialDto>();
                }

                // 将新的物料对象添加到 LinkMaterials 列表中
                converdionFactorDto.LinkMaterials.Add(materialDto);
            }

            return converdionFactorDto;
        }

        /// <summary>
        /// 获取工序配置打印信息
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcProcedurePrintReleationDto>> GetProcedureConfigPrintListAsync(ProcProcedurePrintReleationPagedQueryDto queryDto)
        {
            var query = new ProcProcedurePrintReleationPagedQuery()
            {
                SiteId = _currentSite.SiteId ?? 0,
                ProcedureId = queryDto.ProcedureId,
                PageIndex = queryDto.PageIndex,
                PageSize = queryDto.PageSize,
                Sorting = queryDto.Sorting,
            };
            var pagedInfo = await _procedurePrintRelationRepository.GetPagedInfoAsync(query);

            //实体到DTO转换 装载数据
            var dtos = new List<ProcProcedurePrintReleationDto>();
            if (pagedInfo.Data != null && pagedInfo.Data.Any())
            {
                var materialIds = pagedInfo.Data.Select(a => a.MaterialId).ToArray();
                var materialLsit = await _procMaterialRepository.GetByIdsAsync(materialIds);

                var templateIds = pagedInfo.Data.Select(a => a.TemplateId).ToArray();
                var templateLsit = await _procLabelTemplateRepository.GetByIdsAsync(templateIds);
                foreach (var entity in pagedInfo.Data)
                {
                    var printReleationDto = entity.ToModel<ProcProcedurePrintRelationDto>();
                    var material = materialLsit.FirstOrDefault(a => a.Id == printReleationDto.MaterialId)?.ToModel<ProcMaterialDto>();
                    var template = templateLsit.FirstOrDefault(a => a.Id == printReleationDto.TemplateId)?.ToModel<ProcLabelTemplateDto>();
                    var queryEntity = new ProcProcedurePrintReleationDto
                    {
                        TemplateId = entity.TemplateId,
                        Copy = entity.Copy,
                        MaterialId = entity.MaterialId,
                        Version = entity.Version,
                        MaterialCode = material?.MaterialCode ?? "",
                        MaterialName = material?.MaterialName ?? "",
                        Name = template?.Name ?? ""
                    };
                    dtos.Add(queryEntity);
                }
            }

            return new PagedInfo<ProcProcedurePrintReleationDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 获取工序配置Job信息
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcedureJobReleationDto>> GetProcedureConfigJobListAsync(InteJobBusinessRelationPagedQueryDto queryDto)
        {
            var query = new InteJobBusinessRelationPagedQuery()
            {
                SiteId = _currentSite.SiteId ?? 0,
                BusinessId = queryDto.BusinessId,
                BusinessType = (int)InteJobBusinessTypeEnum.Procedure,
                PageIndex = queryDto.PageIndex,
                PageSize = queryDto.PageSize,
                Sorting = queryDto.Sorting
            };
            var pagedInfo = await _jobBusinessRelationRepository.GetPagedInfoAsync(query);

            //实体到DTO转换 装载数据
            var dtos = new List<ProcedureJobReleationDto>();
            if (pagedInfo.Data != null && pagedInfo.Data.Any())
            {
                var jobIds = pagedInfo.Data.Select(a => a.JobId).ToArray();
                var jobList = await _inteJobRepository.GetByIdsAsync(jobIds);

                foreach (var entity in pagedInfo.Data)
                {
                    var job = jobList.FirstOrDefault(a => a.Id == entity.JobId);
                    dtos.Add(new ProcedureJobReleationDto()
                    {
                        LinkPoint = entity.LinkPoint,
                        Parameter = entity.Parameter,
                        JobId = entity.JobId,
                        BusinessId = entity.BusinessId,
                        IsUse = entity.IsUse,
                        Code = job?.Code ?? "",
                        Name = job?.Name ?? "",
                        Remark = job?.Remark ?? ""
                    });
                }
            }
            return new PagedInfo<ProcedureJobReleationDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 获取工序产出设置
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProductSetDto>> GetProcedureProductSetListAsync(ProcProductSetQueryDto queryDto)
        {
            var query = new ProcProductSetQuery()
            {
                SiteId = _currentSite.SiteId ?? 0,
                SetPointId = queryDto.SetPointId
            };
            var procProductSetEntities = await _procProductSetRepository.GetProcProductSetEntitiesAsync(query);

            //实体到DTO转换 装载数据
            List<ProcProductSetDto> procProductSetDtos = new List<ProcProductSetDto>();
            if (procProductSetEntities != null && procProductSetEntities.Any())
            {
                var materialIds = new List<long> { };
                IEnumerable<ProcMaterialEntity> procMaterialList = new List<ProcMaterialEntity>();
                materialIds.AddRange(procProductSetEntities.Select(a => a.ProductId).ToArray());
                materialIds.AddRange(procProductSetEntities.Select(a => a.SemiProductId).ToArray());
                var materialIdList = materialIds.Distinct();
                if (materialIdList.Any())
                {
                    procMaterialList = await _procMaterialRepository.GetByIdsAsync(materialIdList);
                }

                foreach (var entity in procProductSetEntities)
                {
                    var product = procMaterialList.FirstOrDefault(x => x.Id == entity.ProductId);
                    var semiProduct = procMaterialList.FirstOrDefault(x => x.Id == entity.SemiProductId);
                    procProductSetDtos.Add(new ProcProductSetDto()
                    {
                        ProductId = entity.ProductId,
                        ProductCode = product?.MaterialCode ?? "",
                        MaterialName = product?.MaterialName ?? "",
                        Version = product?.Version ?? "",
                        SetPointId = entity.SetPointId,
                        SemiProductId = entity.SemiProductId,
                        SemiMaterialCode = semiProduct?.MaterialCode ?? "",
                        SemiMaterialName = semiProduct?.MaterialName ?? "",
                        SemiVersion = semiProduct?.Version ?? "",
                    });
                }
            }

            return procProductSetDtos;
        }
    

        public async Task<long> AddProcProcedureAsync(AddConversionFactorDto AddConversionFactorDto)
        {
            if (AddConversionFactorDto == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }
            if (AddConversionFactorDto.code == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10401));
            }
            if (AddConversionFactorDto.LinkMaterials.Count == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10214));
             }
            if (AddConversionFactorDto.LinkMaterials.Count > 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10241));
            }


            var procConversionFactorEntity = AddConversionFactorDto.ToEntity<ProcConversionFactorEntity>();
            procConversionFactorEntity.MaterialId = AddConversionFactorDto.LinkMaterials[0].MaterialId;
            procConversionFactorEntity.Id = IdGenProvider.Instance.CreateId();
            procConversionFactorEntity.CreatedBy = _currentUser.UserName;
            procConversionFactorEntity.UpdatedBy = _currentUser.UserName;
            procConversionFactorEntity.CreatedOn = HymsonClock.Now();
            procConversionFactorEntity.UpdatedOn = HymsonClock.Now();
            procConversionFactorEntity.SiteId = _currentSite.SiteId ?? 0;

            //procLoadPointEntity.OpenStatus = SysDataStatusEnum.Build;

            #region 数据库验证
            var isExists = (await _procConversionFactorRepository.GetProcConversionFactorEntitiesAsync(new ProcConversionFactorQuery()
            {
                SiteId = procConversionFactorEntity.SiteId,
                ProcedureId = procConversionFactorEntity.ProcedureId,
                MaterialId=procConversionFactorEntity.MaterialId
            })).Any();
            if (isExists)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10477)).WithData("ProcedureId", procConversionFactorEntity.ProcedureId);
            }
            #endregion

            using TransactionScope trans = TransactionHelper.GetTransactionScope();
            int response = 0;
            // 入库
            response = await _procConversionFactorRepository.InsertAsync(procConversionFactorEntity);
            if (response == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10704));
            }

            trans.Complete();

            return procConversionFactorEntity.Id;


        }
        


        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsAr"></param>
        /// <returns></returns>
        public async Task<int> DeleteProcProcedureAsync(long[] idsAr)
        {
            if (idsAr.Length < 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10102));
            }

            var entitys = await _procConversionFactorRepository.GetByIdsAsync(idsAr);
            if (entitys != null && entitys.Any(a => a.OpenStatus != ManuSfcRepairDetailIsIsCloseEnum.Close))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10135));
            }

            int rows = 0;
            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                rows += await _procConversionFactorRepository.DeletesAsync(new DeleteCommand
                {
                    Ids = idsAr,
                    DeleteOn = HymsonClock.Now(),
                    UserId = _currentUser.UserName
                });
                rows += await _procProcedureRejudgeRepository.DeleteByParentIdAsync(idsAr);
                rows += await _jobBusinessRelationRepository.DeleteByBusinessIdRangeAsync(idsAr);
                ts.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 根据工序编码获取工序信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<ProcProcedureCodeDto> GetByCodeAsync(string code)
        {
            var entitys = await _procProcedureRepository.GetProcProcedureEntitiesAsync(new ProcProcedureQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                Code = code
            });
            if (entitys == null || !entitys.Any())
            {
                return new ProcProcedureCodeDto();
            }

            var model = entitys.ToList()[0];
            return new ProcProcedureCodeDto
            {
                Id = model.Id,
                Code = model.Code,
                Name = model.Name
            };
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procLoadPointModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyProcConversionFactorAsync(ProcConversionFactorModifyDto procLoadPointModifyDto)
        {
            if (procLoadPointModifyDto == null) throw new CustomerValidationException(nameof(ErrorCode.MES10100));          

            // DTO转换实体
            var procConversionFactorEntity = procLoadPointModifyDto.ToEntity<ProcConversionFactorEntity>();
            procConversionFactorEntity.UpdatedBy = _currentUser.UserName;
            procConversionFactorEntity.UpdatedOn = HymsonClock.Now();
            procConversionFactorEntity.SiteId = _currentSite.SiteId ?? 0;
            procConversionFactorEntity.MaterialId = procLoadPointModifyDto.LinkMaterials[0].MaterialId;
            #region 数据库验证
            //var isExists = (await _procConversionFactorRepository.GetProcConversionFactorEntitiesAsync(new ProcConversionFactorQuery()
            //{
            //    SiteId = procConversionFactorEntity.SiteId,
            //    ProcedureId = procConversionFactorEntity.ProcedureId,
            //    MaterialId = procConversionFactorEntity.MaterialId
            //})).Any();
            //if (isExists)
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES10477)).WithData("ProcedureId", procConversionFactorEntity.ProcedureId);
            //}
            #endregion
            


          

            using TransactionScope ts = TransactionHelper.GetTransactionScope();
            int rows = 0;
            // 入库
            rows = await _procConversionFactorRepository.UpdateAsync(procConversionFactorEntity);
            if (rows == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10704));
            }

            ts.Complete();
        }


    }

}