using FluentValidation;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Data.Repositories.Quality.IQualityRepository;
using Hymson.MES.Data.Repositories.Quality.QualUnqualifiedCode.Query;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.MES.Services.Services.Quality.IQualityService;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;

namespace Hymson.MES.Services.Services.Quality
{
    /// <summary>
    /// 不合格代码服务
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public class QualUnqualifiedCodeService : IQualUnqualifiedCodeService
    {
        private readonly IQualUnqualifiedCodeRepository _qualUnqualifiedCodeRepository;
        private readonly IQualUnqualifiedGroupRepository _qualUnqualifiedGroupRepository;
        private readonly AbstractValidator<QualUnqualifiedCodeCreateDto> _validationCreateRules;
        private readonly AbstractValidator<QualUnqualifiedCodeModifyDto> _validationModifyRules;

        /// <summary>
        /// 不合格代码服务
        /// </summary>
        /// <param name="qualUnqualifiedCodeRepository"></param>
        /// <param name="validationCreateRules"></param>
        /// <param name="validationModifyRules"></param>
        public QualUnqualifiedCodeService(IQualUnqualifiedCodeRepository qualUnqualifiedCodeRepository, IQualUnqualifiedGroupRepository qualUnqualifiedGroupRepository, AbstractValidator<QualUnqualifiedCodeCreateDto> validationCreateRules, AbstractValidator<QualUnqualifiedCodeModifyDto> validationModifyRules)
        {
            _qualUnqualifiedCodeRepository = qualUnqualifiedCodeRepository;
            _qualUnqualifiedGroupRepository = qualUnqualifiedGroupRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="parm">新增参数</param>
        /// <returns></returns>
        /// <exception cref="ValidationException">参数为空</exception>
        /// <exception cref="BusinessException">编码复用</exception>
        public async Task CreateQualUnqualifiedCodeAsync(QualUnqualifiedCodeCreateDto parm)
        {
            if (parm == null)
            {
                throw new ValidationException(ErrorCode.MES10100);
            }
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(parm);

            var qualUnqualifiedCodeEntity = await _qualUnqualifiedCodeRepository.GetByCodeAsync(new QualUnqualifiedCodeByCodeQuery { Code = parm.UnqualifiedCode, Site = "" });
            if (qualUnqualifiedCodeEntity != null)
            {
                throw new BusinessException(ErrorCode.MES11108).WithData("code", parm.UnqualifiedCode);
            }
            var userId = "TODO";
            //DTO转换实体
            qualUnqualifiedCodeEntity = parm.ToEntity<QualUnqualifiedCodeEntity>();
            qualUnqualifiedCodeEntity.Id = IdGenProvider.Instance.CreateId();
            qualUnqualifiedCodeEntity.CreatedBy = userId;
            qualUnqualifiedCodeEntity.UpdatedBy = userId;
            qualUnqualifiedCodeEntity.CreatedOn = HymsonClock.Now();
            qualUnqualifiedCodeEntity.UpdatedOn = HymsonClock.Now();

            List<QualUnqualifiedCodeGroupRelation> list = new List<QualUnqualifiedCodeGroupRelation>();
            if (parm.UnqualifiedGroupIds != null && parm.UnqualifiedGroupIds.Any())
            {
                foreach (var item in parm.UnqualifiedGroupIds)
                {
                    list.Add(new QualUnqualifiedCodeGroupRelation
                    {
                        UnqualifiedGroupId = qualUnqualifiedCodeEntity.Id,
                        UnqualifiedCodeId = item,
                        CreatedBy = userId,
                        UpdatedBy = userId
                    });
                }
            }

            using var ts = TransactionHelper.GetTransactionScope();
            //插入不合格代码表
            await _qualUnqualifiedCodeRepository.InsertAsync(qualUnqualifiedCodeEntity);
            if (list != null && list.Any())
            {
                //插入不合格代码
                await _qualUnqualifiedGroupRepository.AddQualUnqualifiedCodeGroupRelationAsync(list);
            }
            ts.Complete();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteQualUnqualifiedCodeAsync(long id)
        {
            await _qualUnqualifiedCodeRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesQualUnqualifiedCodeAsync(string ids)
        {
            var idsArr = StringExtension.SpitLongArrary(ids);
            return await _qualUnqualifiedCodeRepository.DeletesAsync(idsArr);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task ModifyQualUnqualifiedCodeAsync(QualUnqualifiedCodeModifyDto parm)
        {
            if (parm == null)
            {
                throw new ValidationException(ErrorCode.MES10100);
            }
            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(parm);
            var userId = "TODO";
            //DTO转换实体
            var qualUnqualifiedCodeEntity = parm.ToEntity<QualUnqualifiedCodeEntity>();
            qualUnqualifiedCodeEntity.UpdatedBy = userId;
            qualUnqualifiedCodeEntity.UpdatedOn = HymsonClock.Now();

            List<QualUnqualifiedCodeGroupRelation> list = new List<QualUnqualifiedCodeGroupRelation>();
            if (parm.UnqualifiedGroupIds != null && parm.UnqualifiedGroupIds.Any())
            {
                foreach (var item in parm.UnqualifiedGroupIds)
                {
                    list.Add(new QualUnqualifiedCodeGroupRelation
                    {
                        UnqualifiedGroupId = qualUnqualifiedCodeEntity.Id,
                        UnqualifiedCodeId = item,
                        CreatedBy = userId,
                        UpdatedBy = userId
                    });
                }
            }

            using var ts = TransactionHelper.GetTransactionScope();
            //更新不合格代码表
            await _qualUnqualifiedCodeRepository.UpdateAsync(qualUnqualifiedCodeEntity);
            //TODO 清理关联表
            await _qualUnqualifiedGroupRepository.RealDelteQualUnqualifiedCodeGroupRelationAsync(parm.Id);

            if (list != null && list.Any())
            {
                //插入不合格代码
                await _qualUnqualifiedGroupRepository.AddQualUnqualifiedCodeGroupRelationAsync(list);
            }
            ts.Complete();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pram"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualUnqualifiedCodeDto>> GetPageListAsync(QualUnqualifiedCodePagedQueryDto pram)
        {
            var qualUnqualifiedCodePagedQuery = pram.ToQuery<QualUnqualifiedCodePagedQuery>();
            var pagedInfo = await _qualUnqualifiedCodeRepository.GetPagedInfoAsync(qualUnqualifiedCodePagedQuery);

            //实体到DTO转换 装载数据
            List<QualUnqualifiedCodeDto> qualUnqualifiedCodeDtos = PrepareQualUnqualifiedCodeDtos(pagedInfo);
            return new PagedInfo<QualUnqualifiedCodeDto>(qualUnqualifiedCodeDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<QualUnqualifiedCodeDto> PrepareQualUnqualifiedCodeDtos(PagedInfo<QualUnqualifiedCodeEntity> pagedInfo)
        {
            var qualUnqualifiedCodeDtos = new List<QualUnqualifiedCodeDto>();
            foreach (var qualUnqualifiedCodeEntity in pagedInfo.Data)
            {
                var qualUnqualifiedCodeDto = qualUnqualifiedCodeEntity.ToModel<QualUnqualifiedCodeDto>();
                qualUnqualifiedCodeDtos.Add(qualUnqualifiedCodeDto);
            }

            return qualUnqualifiedCodeDtos;
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<QualUnqualifiedCodeDto> QueryQualUnqualifiedCodeByIdAsync(long id)
        {
            var qualUnqualifiedCodeTask = _qualUnqualifiedCodeRepository.GetByIdAsync(id);
            var unqualifiedCodeGroupRelationTask = _qualUnqualifiedCodeRepository.GetQualUnqualifiedCodeGroupRelationAsync(id);
            var qualUnqualifiedCodeEntity = await qualUnqualifiedCodeTask;
            var unqualifiedCodeGroupRelations = await unqualifiedCodeGroupRelationTask;
            QualUnqualifiedCodeDto qualUnqualifiedCodeDto = new QualUnqualifiedCodeDto();
            if (qualUnqualifiedCodeEntity != null)
            {
                qualUnqualifiedCodeDto = qualUnqualifiedCodeEntity.ToModel<QualUnqualifiedCodeDto>();
            }
            if (unqualifiedCodeGroupRelations != null && unqualifiedCodeGroupRelations.Any())
            {
                qualUnqualifiedCodeDto.UnqualifiedCodeGroupRelationList = new List<UnqualifiedCodeGroupRelationDto>();
                foreach (var item in unqualifiedCodeGroupRelations)
                {
                    qualUnqualifiedCodeDto.UnqualifiedCodeGroupRelationList.Add(new UnqualifiedCodeGroupRelationDto()
                    {
                        Id = item.Id,
                        UnqualifiedGroupId = item.UnqualifiedGroupId,
                        UnqualifiedCode = item.UnqualifiedCode,
                        UnqualifiedCodeName = item.UnqualifiedCodeName,
                        CreatedBy = item.CreatedBy,
                        CreatedOn = item.CreatedOn,
                        UpdatedBy = item.UpdatedBy,
                        UpdatedOn = item.UpdatedOn
                    });
                }
            }

            return qualUnqualifiedCodeDto;
        }
    }
}
