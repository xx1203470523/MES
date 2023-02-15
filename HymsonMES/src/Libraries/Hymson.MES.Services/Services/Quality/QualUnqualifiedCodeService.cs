using FluentValidation;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Data.Repositories.Quality.IQualityRepository;
using Hymson.MES.Data.Repositories.Quality.QualUnqualifiedCode.Query;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.MES.Services.Services.Quality.IQualityService;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Extensions;

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
        private readonly AbstractValidator<QualUnqualifiedCodeCreateDto> _validationCreateRules;
        private readonly AbstractValidator<QualUnqualifiedCodeModifyDto> _validationModifyRules;

        /// <summary>
        /// 不合格代码服务
        /// </summary>
        /// <param name="qualUnqualifiedCodeRepository"></param>
        /// <param name="validationCreateRules"></param>
        /// <param name="validationModifyRules"></param>
        public QualUnqualifiedCodeService(IQualUnqualifiedCodeRepository qualUnqualifiedCodeRepository, AbstractValidator<QualUnqualifiedCodeCreateDto> validationCreateRules, AbstractValidator<QualUnqualifiedCodeModifyDto> validationModifyRules)
        {
            _qualUnqualifiedCodeRepository = qualUnqualifiedCodeRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task CreateQualUnqualifiedCodeAsync(QualUnqualifiedCodeCreateDto parm)
        {
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(parm);

            //DTO转换实体
            var qualUnqualifiedCodeEntity = parm.ToEntity<QualUnqualifiedCodeEntity>();
            qualUnqualifiedCodeEntity.Id = IdGenProvider.Instance.CreateId();
            qualUnqualifiedCodeEntity.CreatedBy = "TODO";
            qualUnqualifiedCodeEntity.UpdatedBy = "TODO";
            qualUnqualifiedCodeEntity.CreatedOn = HymsonClock.Now();
            qualUnqualifiedCodeEntity.UpdatedOn = HymsonClock.Now();

            //入库
            await _qualUnqualifiedCodeRepository.InsertAsync(qualUnqualifiedCodeEntity);
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
        /// <param name="qualUnqualifiedCodeModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyQualUnqualifiedCodeAsync(QualUnqualifiedCodeModifyDto qualUnqualifiedCodeModifyDto)
        {
            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(qualUnqualifiedCodeModifyDto);

            //DTO转换实体
            var qualUnqualifiedCodeEntity = qualUnqualifiedCodeModifyDto.ToEntity<QualUnqualifiedCodeEntity>();
            qualUnqualifiedCodeEntity.UpdatedBy = "TODO";
            qualUnqualifiedCodeEntity.UpdatedOn = HymsonClock.Now();

            await _qualUnqualifiedCodeRepository.UpdateAsync(qualUnqualifiedCodeEntity);
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
            var qualUnqualifiedCodeEntity = await _qualUnqualifiedCodeRepository.GetByIdAsync(id);
            if (qualUnqualifiedCodeEntity != null)
            {
                return qualUnqualifiedCodeEntity.ToModel<QualUnqualifiedCodeDto>();
            }
            return null;
        }
    }
}
