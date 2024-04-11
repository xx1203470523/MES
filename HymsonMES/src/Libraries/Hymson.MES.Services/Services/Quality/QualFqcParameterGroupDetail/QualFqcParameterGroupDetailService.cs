using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Qual;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Data.Repositories.Quality.Query;
using Hymson.MES.Services.Dtos.Qual;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Quality
{
    /// <summary>
    /// 服务（OQC检验参数组明细） 
    /// </summary>
    public class QualFqcParameterGroupDetailService : IQualFqcParameterGroupDetailService
    {
        /// <summary>
        /// 当前用户
        /// </summary>
        private readonly ICurrentUser _currentUser;
        /// <summary>
        /// 当前站点
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 参数验证器
        /// </summary>
        private readonly AbstractValidator<QualFqcParameterGroupDetailSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（OQC检验参数组明细）
        /// </summary>
        private readonly IQualFqcParameterGroupDetailRepository _qualFqcParameterGroupDetailRepository;

        /// <summary>
        /// 标准参数
        /// </summary>
        private readonly IProcParameterRepository _procParameterRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="qualFqcParameterGroupDetailRepository"></param>
        public QualFqcParameterGroupDetailService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<QualFqcParameterGroupDetailSaveDto> validationSaveRules, 
            IQualFqcParameterGroupDetailRepository qualFqcParameterGroupDetailRepository, IProcParameterRepository procParameterRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _qualFqcParameterGroupDetailRepository = qualFqcParameterGroupDetailRepository;
            _procParameterRepository = procParameterRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(QualFqcParameterGroupDetailSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<QualFqcParameterGroupDetailEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存
            return await _qualFqcParameterGroupDetailRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(QualFqcParameterGroupDetailSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

             // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<QualFqcParameterGroupDetailEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            return await _qualFqcParameterGroupDetailRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _qualFqcParameterGroupDetailRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            return await _qualFqcParameterGroupDetailRepository.DeletesAsync(new DeleteCommand
            {
                Ids = ids,
                DeleteOn = HymsonClock.Now(),
                UserId = _currentUser.UserName
            });
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<QualFqcParameterGroupDetailDto?> QueryByIdAsync(long id) 
        {
           var qualFqcParameterGroupDetailEntity = await _qualFqcParameterGroupDetailRepository.GetByIdAsync(id);
           if (qualFqcParameterGroupDetailEntity == null) return null;
           
           return qualFqcParameterGroupDetailEntity.ToModel<QualFqcParameterGroupDetailDto>();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualFqcParameterGroupDetailDto>> GetPagedListAsync(QualFqcParameterGroupDetailPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<QualFqcParameterGroupDetailPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _qualFqcParameterGroupDetailRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<QualFqcParameterGroupDetailDto>());
            return new PagedInfo<QualFqcParameterGroupDetailDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 获取数据集
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualFqcParameterGroupDetailOutputDto>> GetListAsync(QualFqcParameterGroupDetailQueryDto queryDto)
        {
            var query = queryDto.ToQuery<QualFqcParameterGroupDetailQuery>();
            query.SiteId =(long) _currentSite.SiteId;

            var qualIqcInspectionItemDetailEntities = await _qualFqcParameterGroupDetailRepository.GetEntitiesAsync(query);
            if (qualIqcInspectionItemDetailEntities == null || !qualIqcInspectionItemDetailEntities.Any())
            {
                return Enumerable.Empty<QualFqcParameterGroupDetailOutputDto>();
            }

            var parameterIds = qualIqcInspectionItemDetailEntities.Select(m => m.ParameterId);
            var parameterEntities = await _procParameterRepository.GetByIdsAsync(parameterIds);

            var result = qualIqcInspectionItemDetailEntities.Select(m =>
            {
                var item = m.ToModel<QualFqcParameterGroupDetailOutputDto>();

                var parameterEntity = parameterEntities.FirstOrDefault(e => e.Id == m.ParameterId);
                if (parameterEntity != null)
                {
                    item.ParameterCode = parameterEntity.ParameterCode;
                    item.ParameterName = parameterEntity.ParameterName;
                    item.ParameterUnit = parameterEntity.ParameterUnit;
                }
                return item;
            });

            return result;

        }

    }
}
