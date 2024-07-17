/*
 *creator: Karl
 *
 *describe: 编码规则    服务 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-03-17 05:02:26
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Inte;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.Sequences;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Transactions;

namespace Hymson.MES.Services.Services.Integrated
{
    /// <summary>
    /// 编码规则 服务
    /// </summary>
    public class InteCodeRulesService : IInteCodeRulesService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        private readonly IProcMaterialRepository _procMaterialRepository;

        private readonly IInteCodeRulesRepository _inteCodeRulesRepository;
        private readonly IInteCodeRulesMakeRepository _inteCodeRulesMakeRepository;
        private readonly IInteContainerInfoRepository _inteContainerInfoRepository;
        private readonly IInteCodeRulesMaterialRepository _inteCodeRulesMaterialRepository;

        private readonly AbstractValidator<InteCodeRulesCreateDto> _validationCreateRules;
        private readonly AbstractValidator<InteCodeRulesModifyDto> _validationModifyRules;

        private readonly AbstractValidator<InteCodeRulesMakeCreateDto> _validationMakeCreateRules;

        public InteCodeRulesService(
            ICurrentUser currentUser,
            ICurrentSite currentSite,
            IProcMaterialRepository procMaterialRepository,
            IInteCodeRulesRepository inteCodeRulesRepository,
            IInteCodeRulesMakeRepository inteCodeRulesMakeRepository,
            IInteContainerInfoRepository inteContainerInfoRepository,
            AbstractValidator<InteCodeRulesCreateDto> validationCreateRules,
            AbstractValidator<InteCodeRulesModifyDto> validationModifyRules,
            AbstractValidator<InteCodeRulesMakeCreateDto> validationMakeCreateRules,
            IInteCodeRulesMaterialRepository inteCodeRulesMaterialRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _procMaterialRepository = procMaterialRepository;
            _inteCodeRulesRepository = inteCodeRulesRepository;
            _inteCodeRulesMakeRepository = inteCodeRulesMakeRepository;
            _inteContainerInfoRepository = inteContainerInfoRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _validationMakeCreateRules = validationMakeCreateRules;
            _inteCodeRulesMaterialRepository = inteCodeRulesMaterialRepository;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="inteCodeRulesCreateDto"></param>
        /// <returns></returns>
        public async Task CreateInteCodeRulesAsync(InteCodeRulesCreateDto inteCodeRulesCreateDto)
        {
            if (_currentSite.SiteId == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10101));
            }

            await _validationCreateRules.ValidateAndThrowAsync(inteCodeRulesCreateDto);

            //校验OQC与IQC只能创建一个
            if (inteCodeRulesCreateDto.CodeType != null)
            {
                if (inteCodeRulesCreateDto.CodeType == CodeRuleCodeTypeEnum.OQC || inteCodeRulesCreateDto.CodeType == CodeRuleCodeTypeEnum.IQC || inteCodeRulesCreateDto.CodeType == CodeRuleCodeTypeEnum.FQC)
                {
                    var Entities = await _inteCodeRulesRepository.GetInteCodeRulesEntitiesEqualAsync(new InteCodeRulesQuery { SiteId = _currentSite.SiteId ?? 0, CodeType = inteCodeRulesCreateDto.CodeType });
                    if (Entities.Any())
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES12451)).WithData("type", inteCodeRulesCreateDto.CodeType.GetDescription());
                    }
                }
            }

            if (inteCodeRulesCreateDto.CodeRulesMakes == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10103));
            }

            foreach (var item in inteCodeRulesCreateDto.CodeRulesMakes)
            {
                await _validationMakeCreateRules.ValidateAndThrowAsync(item);
            }

            if (!inteCodeRulesCreateDto.CodeRulesMakes.Any(x => x.ValueTakingType == CodeValueTakingTypeEnum.VariableValue && x.SegmentedValue.Trim() == "%ACTIVITY%"))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12438));
            }

            if (inteCodeRulesCreateDto.CodeRulesMakes.Count(x => x.ValueTakingType == CodeValueTakingTypeEnum.VariableValue && x.SegmentedValue.Trim() == "%ACTIVITY%") != 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12444));
            }

            if (inteCodeRulesCreateDto.CodeRulesMakes.Any(x => x.ValueTakingType == CodeValueTakingTypeEnum.VariableValue && x.SegmentedValue.Trim() == "%MULTIPLE_VARIABLE%"))
            {
                if (inteCodeRulesCreateDto.CodeRulesMakes.Count(x => x.ValueTakingType == CodeValueTakingTypeEnum.VariableValue && x.SegmentedValue.Trim() == "%MULTIPLE_VARIABLE%") != 1)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES12445));
                }
                if (inteCodeRulesCreateDto.CodeRulesMakes.Any(x => x.ValueTakingType == CodeValueTakingTypeEnum.VariableValue && x.SegmentedValue.Trim() == "%MULTIPLE_VARIABLE%" && string.IsNullOrEmpty(x.CustomValue)))
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES12446));
                }

                foreach (var item in inteCodeRulesCreateDto.CodeRulesMakes
                    .Where(x => x.ValueTakingType == CodeValueTakingTypeEnum.VariableValue && x.SegmentedValue.Trim() == "%MULTIPLE_VARIABLE%" && !string.IsNullOrEmpty(x.CustomValue))
                    .Where(y => y.CustomValue!.Split(";").GroupBy(x => x).Any(g => g.Count() > 1)))
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES12447));
                }

            }

            //DTO转换实体
            var inteCodeRulesEntity = inteCodeRulesCreateDto.ToEntity<InteCodeRulesEntity>();
            inteCodeRulesEntity.Id = IdGenProvider.Instance.CreateId();
            inteCodeRulesEntity.CreatedBy = _currentUser.UserName;
            inteCodeRulesEntity.UpdatedBy = _currentUser.UserName;
            inteCodeRulesEntity.CreatedOn = HymsonClock.Now();
            inteCodeRulesEntity.UpdatedOn = HymsonClock.Now();
            inteCodeRulesEntity.SiteId = _currentSite.SiteId ?? 0;

            if (inteCodeRulesCreateDto.CodeType == CodeRuleCodeTypeEnum.PackagingSeqCode)
            {
                if (string.IsNullOrWhiteSpace(inteCodeRulesCreateDto.ContainerCode))
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES12448));
                }

                var containerInfoEntity = await _inteContainerInfoRepository.GetOneAsync(new InteContainerInfoQuery { Code = inteCodeRulesCreateDto.ContainerCode, SiteId = _currentSite.SiteId });
                if (containerInfoEntity == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES12449));
                }
                inteCodeRulesEntity.ContainerInfoId = containerInfoEntity.Id;
            }

            //判断是否已经存在该物料数据
            if (inteCodeRulesCreateDto.CodeType == CodeRuleCodeTypeEnum.ProcessControlSeqCode && inteCodeRulesCreateDto.ProductIds != null)
            {
                var codeRulesMaterialEntities = await _inteCodeRulesMaterialRepository.GetEntitiesAsync(new InteCodeRulesMaterialQuery
                {
                    SiteId = _currentSite.SiteId ?? 0,
                    MaterialIds = inteCodeRulesCreateDto.ProductIds
                });
                if (codeRulesMaterialEntities != null && codeRulesMaterialEntities.Any())
                {
                    var codeRulesEntities = await _inteCodeRulesRepository.GetByIdsAsync(codeRulesMaterialEntities.Select(x => x.CodeRulesId).Distinct().ToArray());
                    if (codeRulesEntities != null && codeRulesEntities.Any(x => x.CodeType == CodeRuleCodeTypeEnum.ProcessControlSeqCode))
                    {
                        var codeRulesIds = codeRulesEntities.Where(x => x.CodeType == CodeRuleCodeTypeEnum.ProcessControlSeqCode).Select(x => x.Id);
                        var materialIds = codeRulesMaterialEntities.Where(x => codeRulesIds.Contains(x.CodeRulesId)).Select(x => x.MaterialId);
                        var materialCodes = (await _procMaterialRepository.GetByIdsAsync(materialIds)).Select(x => x.MaterialCode);
                        throw new CustomerValidationException(nameof(ErrorCode.MES12452)).WithData("MaterialCode", string.Join(',', materialCodes));
                    }
                }
            }

            List<InteCodeRulesMakeEntity> inteCodeRulesMakeEntitys = new List<InteCodeRulesMakeEntity>();
            if (inteCodeRulesCreateDto.CodeRulesMakes != null)
            {
                //转换物料组成
                foreach (var item in inteCodeRulesCreateDto.CodeRulesMakes)
                {
                    var inteCodeRulesMakeEntity = item.ToEntity<InteCodeRulesMakeEntity>();
                    inteCodeRulesMakeEntity.Id = IdGenProvider.Instance.CreateId();
                    inteCodeRulesMakeEntity.CodeRulesId = inteCodeRulesEntity.Id;
                    inteCodeRulesMakeEntity.CreatedBy = _currentUser.UserName;
                    inteCodeRulesMakeEntity.CreatedOn = HymsonClock.Now();
                    inteCodeRulesMakeEntity.SiteId = _currentSite.SiteId ?? 0;

                    inteCodeRulesMakeEntitys.Add(inteCodeRulesMakeEntity);
                }
            }

            var codeRulesMaterials = new List<InteCodeRulesMaterialEntity>();
            if (inteCodeRulesCreateDto.ProductIds != null && inteCodeRulesCreateDto.ProductIds.Any())
            {
                codeRulesMaterials = inteCodeRulesCreateDto.ProductIds.Select(item => new InteCodeRulesMaterialEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    CodeRulesId = inteCodeRulesEntity.Id,
                    MaterialId = item,
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName
                }).ToList();
            }
            
            try
            {
                using TransactionScope ts = TransactionHelper.GetTransactionScope();

                int response = 0;

                ////入库
                //response = await _inteCodeRulesRepository.InsertAsync(inteCodeRulesEntity);

                response = await _inteCodeRulesRepository.InsertReAsync(inteCodeRulesEntity);

                if (response <= 0)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES12402));
                }

                if (inteCodeRulesMakeEntitys.Count > 0)
                {
                    //编码组成
                    response = await _inteCodeRulesMakeRepository.InsertsAsync(inteCodeRulesMakeEntitys);
                    if (response < inteCodeRulesMakeEntitys.Count)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES12402));
                    }
                }
                if (codeRulesMaterials.Any())
                {
                    await _inteCodeRulesMaterialRepository.InsertRangeAsync(codeRulesMaterials);
                }

                ts.Complete();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("inte_code_rules.uniq_ContainerInfoId_CodeType_SiteId_IsDeleted"))
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES12450)).WithData("code", inteCodeRulesCreateDto.ContainerCode ?? "");
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteInteCodeRulesAsync(long id)
        {
            await _inteCodeRulesRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesInteCodeRulesAsync(long[] idsArr)
        {
            return await _inteCodeRulesRepository.DeletesAsync(new DeleteCommand { Ids = idsArr, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="inteCodeRulesPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteCodeRulesPageViewDto>> GetPageListAsync(InteCodeRulesPagedQueryDto inteCodeRulesPagedQueryDto)
        {
            var inteCodeRulesPagedQuery = inteCodeRulesPagedQueryDto.ToQuery<InteCodeRulesPagedQuery>();
            inteCodeRulesPagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _inteCodeRulesRepository.GetPagedInfoAsync(inteCodeRulesPagedQuery);

            //实体到DTO转换 装载数据
            List<InteCodeRulesPageViewDto> inteCodeRulesDtos = PrepareInteCodeRulesDtos(pagedInfo);
            return new PagedInfo<InteCodeRulesPageViewDto>(inteCodeRulesDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<InteCodeRulesPageViewDto> PrepareInteCodeRulesDtos(PagedInfo<InteCodeRulesPageView> pagedInfo)
        {
            var inteCodeRulesDtos = new List<InteCodeRulesPageViewDto>();
            foreach (var inteCodeRulesEntity in pagedInfo.Data)
            {
                var inteCodeRulesDto = inteCodeRulesEntity.ToModel<InteCodeRulesPageViewDto>();
                inteCodeRulesDto.RowKey = inteCodeRulesEntity.Id;
                if (inteCodeRulesEntity.CodeType == CodeRuleCodeTypeEnum.ProcessControlSeqCode)
                {
                    inteCodeRulesDto.RowKey = inteCodeRulesEntity.MaterialId;
                }
                inteCodeRulesDtos.Add(inteCodeRulesDto);
            }

            return inteCodeRulesDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="inteCodeRulesModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyInteCodeRulesAsync(InteCodeRulesModifyDto inteCodeRulesModifyDto)
        {
            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(inteCodeRulesModifyDto);
            foreach (var item in inteCodeRulesModifyDto.CodeRulesMakes)
            {
                await _validationMakeCreateRules.ValidateAndThrowAsync(item);
            }
            if (!inteCodeRulesModifyDto.CodeRulesMakes.Any(x => x.ValueTakingType == CodeValueTakingTypeEnum.VariableValue && x.SegmentedValue.Trim() == "%ACTIVITY%"))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12438));
            }
            if (inteCodeRulesModifyDto.CodeRulesMakes.Count(x => x.ValueTakingType == CodeValueTakingTypeEnum.VariableValue && x.SegmentedValue.Trim() == "%ACTIVITY%") != 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12444));
            }
            if (inteCodeRulesModifyDto.CodeRulesMakes.Any(x => x.ValueTakingType == CodeValueTakingTypeEnum.VariableValue && x.SegmentedValue.Trim() == "%MULTIPLE_VARIABLE%"))
            {
                if (inteCodeRulesModifyDto.CodeRulesMakes.Count(x => x.ValueTakingType == CodeValueTakingTypeEnum.VariableValue && x.SegmentedValue.Trim() == "%MULTIPLE_VARIABLE%") != 1)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES12445));
                }
                if (inteCodeRulesModifyDto.CodeRulesMakes.Any(x => x.ValueTakingType == CodeValueTakingTypeEnum.VariableValue && x.SegmentedValue.Trim() == "%MULTIPLE_VARIABLE%" && string.IsNullOrEmpty(x.CustomValue)))
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES12446));
                }

                foreach (var item in inteCodeRulesModifyDto.CodeRulesMakes
                    .Where(x => x.ValueTakingType == CodeValueTakingTypeEnum.VariableValue && x.SegmentedValue.Trim() == "%MULTIPLE_VARIABLE%" && !string.IsNullOrEmpty(x.CustomValue))
                    .Where(y => y.CustomValue!.Split(";").GroupBy(x => x).Any(g => g.Count() > 1)))
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES12447));
                }
            }

            //DTO转换实体
            var inteCodeRulesEntity = inteCodeRulesModifyDto.ToEntity<InteCodeRulesEntity>();
            inteCodeRulesEntity.UpdatedBy = _currentUser.UserName;
            inteCodeRulesEntity.UpdatedOn = HymsonClock.Now();

            if (inteCodeRulesModifyDto.CodeType == CodeRuleCodeTypeEnum.PackagingSeqCode)
            {
                if (string.IsNullOrWhiteSpace(inteCodeRulesModifyDto.ContainerCode))
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES12448));
                }

                var containerInfoEntity = await _inteContainerInfoRepository.GetOneAsync(
                    new InteContainerInfoQuery
                    {
                        Code = inteCodeRulesModifyDto.ContainerCode,
                        SiteId = _currentSite.SiteId
                    });

                if (containerInfoEntity == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES12449));
                }

                inteCodeRulesEntity.ContainerInfoId = containerInfoEntity.Id;
            }


            //判断是否已经存在该物料数据
            if (inteCodeRulesModifyDto.CodeType == CodeRuleCodeTypeEnum.ProcessControlSeqCode && inteCodeRulesModifyDto.ProductIds != null)
            {
                var codeRulesMaterialEntities = await _inteCodeRulesMaterialRepository.GetEntitiesAsync(new InteCodeRulesMaterialQuery
                {
                    SiteId = _currentSite.SiteId ?? 0,
                    MaterialIds = inteCodeRulesModifyDto.ProductIds
                });
                if (codeRulesMaterialEntities != null && codeRulesMaterialEntities.Any(x => x.CodeRulesId != inteCodeRulesModifyDto.Id))
                {
                    var codeRulesEntities = await _inteCodeRulesRepository.GetByIdsAsync(codeRulesMaterialEntities.Where(x => x.CodeRulesId != inteCodeRulesModifyDto.Id).Select(x => x.CodeRulesId).Distinct().ToArray());
                    if (codeRulesEntities != null && codeRulesEntities.Any(x => x.CodeType == CodeRuleCodeTypeEnum.ProcessControlSeqCode))
                    {
                        var codeRulesIds = codeRulesEntities.Where(x => x.CodeType == CodeRuleCodeTypeEnum.ProcessControlSeqCode).Select(x => x.Id);
                        var existMaterialIds = codeRulesMaterialEntities.Where(x => codeRulesIds.Contains(x.CodeRulesId)).Select(x => x.MaterialId);
                        var materialCodes = (await _procMaterialRepository.GetByIdsAsync(existMaterialIds)).Select(x => x.MaterialCode);
                        throw new CustomerValidationException(nameof(ErrorCode.MES12452)).WithData("MaterialCode", string.Join(',', materialCodes));
                    }
                }
            }

            List<InteCodeRulesMakeEntity> inteCodeRulesMakeEntitys = new List<InteCodeRulesMakeEntity>();
            if (inteCodeRulesModifyDto.CodeRulesMakes != null)
            {
                int initSeq = 0;
                //转换物料组成
                foreach (var item in inteCodeRulesModifyDto.CodeRulesMakes)
                {
                    initSeq = initSeq + 10;
                    var inteCodeRulesMakeEntity = item.ToEntity<InteCodeRulesMakeEntity>();
                    inteCodeRulesMakeEntity.Id = IdGenProvider.Instance.CreateId();
                    inteCodeRulesMakeEntity.CodeRulesId = inteCodeRulesEntity.Id;
                    inteCodeRulesMakeEntity.CreatedBy = _currentUser.UserName;
                    inteCodeRulesMakeEntity.CreatedOn = HymsonClock.Now();
                    inteCodeRulesMakeEntity.SiteId = _currentSite.SiteId ?? 0;
                    inteCodeRulesMakeEntity.Seq = initSeq;

                    inteCodeRulesMakeEntitys.Add(inteCodeRulesMakeEntity);
                }
            }

            var codeRulesMaterials = new List<InteCodeRulesMaterialEntity>();
            if (inteCodeRulesModifyDto.ProductIds != null && inteCodeRulesModifyDto.ProductIds.Any())
            {
                codeRulesMaterials = inteCodeRulesModifyDto.ProductIds.Select(item => new InteCodeRulesMaterialEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    CodeRulesId = inteCodeRulesEntity.Id,
                    MaterialId = item,
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName
                }).ToList();
            }

            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                int response = 0;
                response = await _inteCodeRulesRepository.UpdateReAsync(inteCodeRulesEntity);

                if (response <= 0)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES12402));
                }

                await _inteCodeRulesMakeRepository.DeleteByCodeRulesIdAsync(inteCodeRulesEntity.Id);

                if (inteCodeRulesMakeEntitys.Count > 0)
                {
                    //编码组成
                    response = await _inteCodeRulesMakeRepository.InsertsAsync(inteCodeRulesMakeEntitys);
                    if (response < inteCodeRulesMakeEntitys.Count)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES12402));
                    }
                }

                await _inteCodeRulesMaterialRepository.DeleteByCodeRulesIdAsync(inteCodeRulesEntity.Id);
                if (codeRulesMaterials.Any())
                {
                    await _inteCodeRulesMaterialRepository.InsertRangeAsync(codeRulesMaterials);
                }

                ts.Complete();
            }


        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteCodeRulesDetailViewDto> QueryInteCodeRulesByIdAsync(long id)
        {
            var inteCodeRulesEntity = await _inteCodeRulesRepository.GetByIdAsync(id);

            if (inteCodeRulesEntity != null)
            {
                var inteCodeRulesDetailViewDto = inteCodeRulesEntity.ToModel<InteCodeRulesDetailViewDto>();

                ////查询关联数据
                //var material = await _procMaterialRepository.GetByIdAsync(inteCodeRulesEntity.ProductId.GetValueOrDefault());
                //if (material != null)
                //{
                //    inteCodeRulesDetailViewDto.MaterialCode = material.MaterialCode ;
                //    inteCodeRulesDetailViewDto.MaterialName = material.MaterialName;
                //    inteCodeRulesDetailViewDto.MaterialVersion = material.Version ?? "";
                //}

                //查询编码规则对应物料
                var codeRulesMaterialEntities = await _inteCodeRulesMaterialRepository.GetEntitiesAsync(new InteCodeRulesMaterialQuery
                {
                    SiteId = inteCodeRulesEntity.SiteId,
                    CodeRulesId = id,
                    Sorting = "Id"
                });
                if (codeRulesMaterialEntities != null && codeRulesMaterialEntities.Any())
                {
                    inteCodeRulesDetailViewDto.ProductIds = codeRulesMaterialEntities.Select(x => x.MaterialId);
                    var materialEntities = await _procMaterialRepository.GetByIdsAsync(inteCodeRulesDetailViewDto.ProductIds);
                    inteCodeRulesDetailViewDto.MaterialCodes = materialEntities.Select(x => x.MaterialCode);
                    inteCodeRulesDetailViewDto.MaterialCodesJoinStr = string.Join(',', materialEntities.Select(x => x.MaterialCode));
                }

                //查询关联的编码规则组成
                var inteCodeRulesMakeEntitys = (await _inteCodeRulesMakeRepository.GetInteCodeRulesMakeEntitiesAsync(new InteCodeRulesMakeQuery { SiteId = _currentSite.SiteId ?? 0, CodeRulesId = inteCodeRulesEntity.Id })).OrderBy(x => x.Seq);

                List<InteCodeRulesMakeDto> inteCodeRulesDtos = new List<InteCodeRulesMakeDto>();
                if (inteCodeRulesMakeEntitys != null && inteCodeRulesMakeEntitys.Any())
                {
                    //转换    
                    foreach (var item in inteCodeRulesMakeEntitys)
                    {
                        inteCodeRulesDtos.Add(item.ToModel<InteCodeRulesMakeDto>());
                    }
                }

                inteCodeRulesDetailViewDto.CodeRulesMakes = inteCodeRulesDtos;

                if (inteCodeRulesDetailViewDto.CodeType == CodeRuleCodeTypeEnum.PackagingSeqCode)
                {
                    var containerInfoEntity = await _inteContainerInfoRepository.GetOneAsync(new InteContainerInfoQuery { Id = inteCodeRulesEntity.ContainerInfoId });
                    if (containerInfoEntity != null)
                    {
                        inteCodeRulesDetailViewDto.ContainerId = containerInfoEntity.Id;
                        inteCodeRulesDetailViewDto.ContainerCode = containerInfoEntity.Code;
                    }
                }

                return inteCodeRulesDetailViewDto;
            }
            return new InteCodeRulesDetailViewDto();
        }
    }
}
