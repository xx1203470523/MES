using FluentValidation;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Data.Repositories.Quality.QualUnqualifiedGroup.Query;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Extensions;

namespace Hymson.MES.Services.Services.Quality
{
    /// <summary>
    /// 不合格代码组服务
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public class QualUnqualifiedGroupService : IQualUnqualifiedGroupService
    {
        private readonly IQualUnqualifiedGroupRepository _qualUnqualifiedGroupRepository;
        private readonly AbstractValidator<QualUnqualifiedGroupCreateDto> _validationCreateRules;
        private readonly AbstractValidator<QualUnqualifiedGroupModifyDto> _validationModifyRules;

        /// <summary>
        /// 不合格代码组服务
        /// </summary>
        /// <param name="qualUnqualifiedGroupRepository"></param>
        /// <param name="validationCreateRules"></param>
        /// <param name="validationModifyRules"></param>
        public QualUnqualifiedGroupService(IQualUnqualifiedGroupRepository qualUnqualifiedGroupRepository, AbstractValidator<QualUnqualifiedGroupCreateDto> validationCreateRules, AbstractValidator<QualUnqualifiedGroupModifyDto> validationModifyRules)
        {
            _qualUnqualifiedGroupRepository = qualUnqualifiedGroupRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualUnqualifiedGroupDto>> GetPageListAsync(QualUnqualifiedGroupPagedQueryDto parm)
        {
            var qualUnqualifiedGroupPagedQuery = parm.ToQuery<QualUnqualifiedGroupPagedQuery>();
            var pagedInfo = await _qualUnqualifiedGroupRepository.GetPagedInfoAsync(qualUnqualifiedGroupPagedQuery);

            //实体到DTO转换 装载数据
            List<QualUnqualifiedGroupDto> qualUnqualifiedGroupDtos = PrepareQualUnqualifiedGroupDtos(pagedInfo);
            return new PagedInfo<QualUnqualifiedGroupDto>(qualUnqualifiedGroupDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task CreateQualUnqualifiedGroupAsync(QualUnqualifiedGroupCreateDto parm)
        {
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(parm);

            //DTO转换实体
            var qualUnqualifiedGroupEntity = parm.ToEntity<QualUnqualifiedGroupEntity>();
            qualUnqualifiedGroupEntity.Id = IdGenProvider.Instance.CreateId();
            qualUnqualifiedGroupEntity.CreatedBy = "TODO";
            qualUnqualifiedGroupEntity.UpdatedBy = "TODO";
            qualUnqualifiedGroupEntity.CreatedOn = HymsonClock.Now();
            qualUnqualifiedGroupEntity.UpdatedOn = HymsonClock.Now();

            //入库
            await _qualUnqualifiedGroupRepository.InsertAsync(qualUnqualifiedGroupEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteQualUnqualifiedGroupAsync(long id)
        {
            await _qualUnqualifiedGroupRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesQualUnqualifiedGroupAsync(string ids)
        {
            var idsArr = StringExtension.SpitLongArrary(ids);
            return await _qualUnqualifiedGroupRepository.DeletesAsync(idsArr);
        }


        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<QualUnqualifiedGroupDto> PrepareQualUnqualifiedGroupDtos(PagedInfo<QualUnqualifiedGroupEntity> pagedInfo)
        {
            var qualUnqualifiedGroupDtos = new List<QualUnqualifiedGroupDto>();
            foreach (var qualUnqualifiedGroupEntity in pagedInfo.Data)
            {
                var qualUnqualifiedGroupDto = qualUnqualifiedGroupEntity.ToModel<QualUnqualifiedGroupDto>();
                qualUnqualifiedGroupDtos.Add(qualUnqualifiedGroupDto);
            }

            return qualUnqualifiedGroupDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task ModifyQualUnqualifiedGroupAsync(QualUnqualifiedGroupModifyDto parm)
        {
            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(parm);

            //DTO转换实体
            var qualUnqualifiedGroupEntity = parm.ToEntity<QualUnqualifiedGroupEntity>();
            qualUnqualifiedGroupEntity.UpdatedBy = "TODO";
            qualUnqualifiedGroupEntity.UpdatedOn = HymsonClock.Now();

            await _qualUnqualifiedGroupRepository.UpdateAsync(qualUnqualifiedGroupEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<QualUnqualifiedGroupDto> QueryQualUnqualifiedGroupByIdAsync(long id)
        {
            var qualUnqualifiedGroupEntity = await _qualUnqualifiedGroupRepository.GetByIdAsync(id);
            if (qualUnqualifiedGroupEntity != null)
            {
                return qualUnqualifiedGroupEntity.ToModel<QualUnqualifiedGroupDto>();
            }
            return null;
        }
    }
}
