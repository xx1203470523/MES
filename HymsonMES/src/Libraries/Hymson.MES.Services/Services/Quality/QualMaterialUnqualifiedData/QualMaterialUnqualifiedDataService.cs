using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Core.Enums.Quality;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Data.Repositories.Quality.Query;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.Snowflake;
using Hymson.Utils;
using Org.BouncyCastle.Crypto;

namespace Hymson.MES.Services.Services.Quality
{
    /// <summary>
    /// 服务（车间物料不良记录） 
    /// </summary>
    public class QualMaterialUnqualifiedDataService : IQualMaterialUnqualifiedDataService
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
        private readonly AbstractValidator<QualMaterialUnqualifiedDataSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（车间物料不良记录）
        /// </summary>
        private readonly IQualMaterialUnqualifiedDataRepository _qualMaterialUnqualifiedDataRepository;
        private readonly IQualMaterialUnqualifiedDataDetailRepository _unqualifiedDataDetailRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="qualMaterialUnqualifiedDataRepository"></param>
        /// <param name="unqualifiedDataDetailRepository"></param>
        public QualMaterialUnqualifiedDataService(ICurrentUser currentUser, ICurrentSite currentSite,
            AbstractValidator<QualMaterialUnqualifiedDataSaveDto> validationSaveRules,
            IQualMaterialUnqualifiedDataRepository qualMaterialUnqualifiedDataRepository,
            IQualMaterialUnqualifiedDataDetailRepository unqualifiedDataDetailRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _qualMaterialUnqualifiedDataRepository = qualMaterialUnqualifiedDataRepository;
            _unqualifiedDataDetailRepository = unqualifiedDataDetailRepository;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(QualMaterialUnqualifiedDataSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<QualMaterialUnqualifiedDataEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存
            return await _qualMaterialUnqualifiedDataRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(QualMaterialUnqualifiedDataSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<QualMaterialUnqualifiedDataEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            return await _qualMaterialUnqualifiedDataRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _qualMaterialUnqualifiedDataRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            if (ids.Length < 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10102));
            }

            var entitys = await _qualMaterialUnqualifiedDataRepository.GetByIdsAsync(ids);
            if(entitys.Any(x=>x.UnqualifiedStatus== QualMaterialUnqualifiedStatusEnum.Close))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15901));
            }

            //删除后，同时解除物料条码的库存锁定状态
            return await _qualMaterialUnqualifiedDataRepository.DeletesAsync(new DeleteCommand
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
        public async Task<QualMaterialUnqualifiedDataDto?> QueryByIdAsync(long id)
        {
            var qualMaterialUnqualifiedDataEntity = await _qualMaterialUnqualifiedDataRepository.GetByIdAsync(id);
            if (qualMaterialUnqualifiedDataEntity == null) return null;

            return qualMaterialUnqualifiedDataEntity.ToModel<QualMaterialUnqualifiedDataDto>();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualMaterialUnqualifiedDataViewDto>> GetPagedListAsync(QualMaterialUnqualifiedDataPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<QualMaterialUnqualifiedDataPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _qualMaterialUnqualifiedDataRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = new List<QualMaterialUnqualifiedDataViewDto>();
            if (pagedInfo.Data == null || !pagedInfo.Data.Any())
            {
                return new PagedInfo<QualMaterialUnqualifiedDataViewDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
            }

            foreach (var item in pagedInfo.Data)
            {
                dtos.Add(new QualMaterialUnqualifiedDataViewDto
                {
                    Id = item.Id,
                    MaterialBarCode = item.MaterialBarCode,
                    QuantityResidue = item.QuantityResidue,
                    MaterialCode = item.MaterialCode + "/" + item.Version,
                    MaterialName = item.MaterialName,
                    UnqualifiedCode = item.UnqualifiedCode,
                    UnqualifiedStatus = item.UnqualifiedStatus,
                    DisposalResult = item.DisposalResult,
                    DisposalTime = item.DisposalTime,
                    CreatedOn = item.CreatedOn
                });
            }
            return new PagedInfo<QualMaterialUnqualifiedDataViewDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }
    }
}
