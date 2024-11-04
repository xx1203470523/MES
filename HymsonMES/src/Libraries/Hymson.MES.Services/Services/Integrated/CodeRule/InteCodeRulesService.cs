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
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.Sequences;
using Hymson.Snowflake;
using Hymson.Utils;
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

        /// <summary>
        /// 编码规则 仓储
        /// </summary>
        private readonly IInteCodeRulesRepository _inteCodeRulesRepository;
        private readonly ISequenceService _sequenceService;
        private readonly AbstractValidator<InteCodeRulesCreateDto> _validationCreateRules;
        private readonly AbstractValidator<InteCodeRulesModifyDto> _validationModifyRules;

        private readonly IProcMaterialRepository _procMaterialRepository;

        private readonly IInteCodeRulesMakeRepository _inteCodeRulesMakeRepository;

        private readonly AbstractValidator<InteCodeRulesMakeCreateDto> _validationMakeCreateRules;

        public InteCodeRulesService(ICurrentUser currentUser, ICurrentSite currentSite, IInteCodeRulesRepository inteCodeRulesRepository,
ISequenceService sequenceService, AbstractValidator<InteCodeRulesCreateDto> validationCreateRules, AbstractValidator<InteCodeRulesModifyDto> validationModifyRules, IProcMaterialRepository procMaterialRepository, IInteCodeRulesMakeRepository inteCodeRulesMakeRepository, AbstractValidator<InteCodeRulesMakeCreateDto> validationMakeCreateRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;

            _inteCodeRulesRepository = inteCodeRulesRepository;
            this._sequenceService = sequenceService;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;

            _procMaterialRepository = procMaterialRepository;
            _inteCodeRulesMakeRepository = inteCodeRulesMakeRepository;
            _validationMakeCreateRules = validationMakeCreateRules;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="inteCodeRulesDto"></param>
        /// <returns></returns>
        public async Task CreateInteCodeRulesAsync(InteCodeRulesCreateDto inteCodeRulesCreateDto)
        {
            //// 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new BusinessException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(inteCodeRulesCreateDto);
            foreach (var item in inteCodeRulesCreateDto.CodeRulesMakes)
            {
                await _validationMakeCreateRules.ValidateAndThrowAsync(item);
            }
            if (!inteCodeRulesCreateDto.CodeRulesMakes.Any(x => x.ValueTakingType == CodeValueTakingTypeEnum.VariableValue && x.SegmentedValue.Trim() == "%ACTIVITY%")) 
            {
                throw new BusinessException(nameof(ErrorCode.MES12438));
            }


            //DTO转换实体
            var inteCodeRulesEntity = inteCodeRulesCreateDto.ToEntity<InteCodeRulesEntity>();
            inteCodeRulesEntity.Id = IdGenProvider.Instance.CreateId();
            inteCodeRulesEntity.CreatedBy = _currentUser.UserName;
            inteCodeRulesEntity.UpdatedBy = _currentUser.UserName;
            inteCodeRulesEntity.CreatedOn = HymsonClock.Now();
            inteCodeRulesEntity.UpdatedOn = HymsonClock.Now();
            inteCodeRulesEntity.SiteId = _currentSite.SiteId ?? 123456;

            //判断是否已经存在该物料数据
            var hasCodeRulesEntities = await _inteCodeRulesRepository.GetInteCodeRulesEntitiesEqualAsync(new InteCodeRulesQuery { SiteId = _currentSite.SiteId ?? 123456, ProductId = inteCodeRulesCreateDto.ProductId });
            if (hasCodeRulesEntities != null && hasCodeRulesEntities.Any())
            {
                IEnumerable<InteCodeRulesEntity> repeats = new List<InteCodeRulesEntity>();
                //判断 编码类型和包装类型是否重复，重复则报错   2023/04/25 加的需求
                if (inteCodeRulesCreateDto.CodeType == CodeRuleCodeTypeEnum.ProcessControlSeqCode)
                {
                    repeats = hasCodeRulesEntities.Where(x => x.CodeType == CodeRuleCodeTypeEnum.ProcessControlSeqCode).ToList();
                }
                else
                {
                    repeats = hasCodeRulesEntities.Where(x => x.CodeType == CodeRuleCodeTypeEnum.PackagingSeqCode && x.PackType == inteCodeRulesCreateDto.PackType).ToList();
                }

                if (repeats != null && repeats.Any())
                {
                    if (inteCodeRulesCreateDto.CodeType == CodeRuleCodeTypeEnum.ProcessControlSeqCode)
                        throw new BusinessException(nameof(ErrorCode.MES12401)).WithData("productId", inteCodeRulesCreateDto.ProductId);
                    else
                        throw new BusinessException(nameof(ErrorCode.MES12403)).WithData("productId", inteCodeRulesCreateDto.ProductId);
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
                    inteCodeRulesMakeEntity.SiteId = _currentSite.SiteId ?? 123456;

                    inteCodeRulesMakeEntitys.Add(inteCodeRulesMakeEntity);
                }
            }

            using (TransactionScope ts = new TransactionScope())
            {
                int response = 0;
                //入库
                response = await _inteCodeRulesRepository.InsertAsync(inteCodeRulesEntity);
                if (response <= 0)
                {
                    throw new BusinessException(nameof(ErrorCode.MES12402));
                }

                if (inteCodeRulesMakeEntitys.Count > 0)
                {
                    //编码组成
                    response = await _inteCodeRulesMakeRepository.InsertsAsync(inteCodeRulesMakeEntitys);
                    if (response < inteCodeRulesMakeEntitys.Count)
                    {
                        throw new BusinessException(nameof(ErrorCode.MES12402));
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
            inteCodeRulesPagedQuery.SiteId = _currentSite.SiteId ?? 123456;
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
                inteCodeRulesDtos.Add(inteCodeRulesDto);
            }

            return inteCodeRulesDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="inteCodeRulesDto"></param>
        /// <returns></returns>
        public async Task ModifyInteCodeRulesAsync(InteCodeRulesModifyDto inteCodeRulesModifyDto)
        {
            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(inteCodeRulesModifyDto);
            foreach (var item in inteCodeRulesModifyDto.CodeRulesMakes)
            {
                await _validationMakeCreateRules.ValidateAndThrowAsync(item);
            }

            //DTO转换实体
            var inteCodeRulesEntity = inteCodeRulesModifyDto.ToEntity<InteCodeRulesEntity>();
            inteCodeRulesEntity.UpdatedBy = _currentUser.UserName;
            inteCodeRulesEntity.UpdatedOn = HymsonClock.Now();


            //判断是否已经存在该物料数据
            var hasCodeRulesEntities = await _inteCodeRulesRepository.GetInteCodeRulesEntitiesEqualAsync(new InteCodeRulesQuery { SiteId=_currentSite.SiteId??0,ProductId = inteCodeRulesModifyDto.ProductId });
            if (hasCodeRulesEntities != null && hasCodeRulesEntities.Any())
            {
                IEnumerable<InteCodeRulesEntity> repeats = new List<InteCodeRulesEntity>();
                //判断 编码类型和包装类型是否重复，重复则报错   2023/04/25 加的需求
                if (inteCodeRulesModifyDto.CodeType == CodeRuleCodeTypeEnum.ProcessControlSeqCode)
                {
                    repeats = hasCodeRulesEntities.Where(x => x.CodeType == CodeRuleCodeTypeEnum.ProcessControlSeqCode).ToList();
                }
                else
                {
                    repeats = hasCodeRulesEntities.Where(x => x.CodeType == CodeRuleCodeTypeEnum.PackagingSeqCode && x.PackType == inteCodeRulesModifyDto.PackType).ToList();
                }

                if (repeats != null && repeats.Any())
                {
                    if (!(repeats.Count() == 1 && repeats.First().Id == inteCodeRulesModifyDto.Id)) //去掉当前修改的数据的
                    {
                        if (inteCodeRulesModifyDto.CodeType == CodeRuleCodeTypeEnum.ProcessControlSeqCode)
                            throw new BusinessException(nameof(ErrorCode.MES12401)).WithData("productId", inteCodeRulesModifyDto.ProductId);
                        else
                            throw new BusinessException(nameof(ErrorCode.MES12403)).WithData("productId", inteCodeRulesModifyDto.ProductId);
                    }
                }
            }

            List<InteCodeRulesMakeEntity> inteCodeRulesMakeEntitys = new List<InteCodeRulesMakeEntity>();
            if (inteCodeRulesModifyDto.CodeRulesMakes != null)
            {
                //转换物料组成
                foreach (var item in inteCodeRulesModifyDto.CodeRulesMakes)
                {
                    var inteCodeRulesMakeEntity = item.ToEntity<InteCodeRulesMakeEntity>();
                    inteCodeRulesMakeEntity.Id = IdGenProvider.Instance.CreateId();
                    inteCodeRulesMakeEntity.CodeRulesId = inteCodeRulesEntity.Id;
                    inteCodeRulesMakeEntity.CreatedBy = _currentUser.UserName;
                    inteCodeRulesMakeEntity.CreatedOn = HymsonClock.Now();
                    inteCodeRulesMakeEntity.SiteId = _currentSite.SiteId ?? 123456;

                    inteCodeRulesMakeEntitys.Add(inteCodeRulesMakeEntity);
                }
            }

            using (TransactionScope ts = new TransactionScope())
            {
                int response = 0;
                response = await _inteCodeRulesRepository.UpdateAsync(inteCodeRulesEntity);

                if (response <= 0)
                {
                    throw new BusinessException(nameof(ErrorCode.MES12402));
                }

                await _inteCodeRulesMakeRepository.DeleteByCodeRulesIdAsync(inteCodeRulesEntity.Id);

                if (inteCodeRulesMakeEntitys.Count > 0)
                {
                    //编码组成
                    response = await _inteCodeRulesMakeRepository.InsertsAsync(inteCodeRulesMakeEntitys);
                    if (response < inteCodeRulesMakeEntitys.Count)
                    {
                        throw new BusinessException(nameof(ErrorCode.MES12402));
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
        public async Task<InteCodeRulesDetailViewDto> QueryInteCodeRulesByIdAsync(long id)
        {
            var inteCodeRulesEntity = await _inteCodeRulesRepository.GetByIdAsync(id);
            if (inteCodeRulesEntity != null)
            {
                var inteCodeRulesDetailViewDto = inteCodeRulesEntity.ToModel<InteCodeRulesDetailViewDto>();
                //查询关联数据
                var material = await _procMaterialRepository.GetByIdAsync(inteCodeRulesEntity.ProductId, _currentSite.SiteId ?? 123456);
                if (material != null)
                {
                    inteCodeRulesDetailViewDto.MaterialCode = material.MaterialCode;
                    inteCodeRulesDetailViewDto.MaterialName = material.MaterialName;
                    inteCodeRulesDetailViewDto.MaterialVersion = material.Version;
                }

                //查询关联的编码规则组成
                var inteCodeRulesMakeEntitys = await _inteCodeRulesMakeRepository.GetInteCodeRulesMakeEntitiesAsync(new InteCodeRulesMakeQuery { SiteId = _currentSite.SiteId ?? 123456, CodeRulesId = inteCodeRulesEntity.Id });

                List<InteCodeRulesMakeDto> inteCodeRulesDtos = new List<InteCodeRulesMakeDto>();
                if (inteCodeRulesMakeEntitys != null && inteCodeRulesMakeEntitys.Count() > 0)
                {
                    //转换
                    foreach (var item in inteCodeRulesMakeEntitys)
                    {
                        inteCodeRulesDtos.Add(item.ToModel<InteCodeRulesMakeDto>());
                    }
                }

                inteCodeRulesDetailViewDto.CodeRulesMakes = inteCodeRulesDtos;

                return inteCodeRulesDetailViewDto;
            }
            return null;
        }
    }
}
