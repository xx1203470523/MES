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
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Integrated.Query;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Transactions;

namespace Hymson.MES.Services.Services.Integrated
{
    /// <summary>
    /// 服务（资质认证） 
    /// </summary>
    public class InteQualificationAuthenticationService : IInteQualificationAuthenticationService
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
        private readonly AbstractValidator<InteQualificationAuthenticationSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（资质认证）
        /// </summary>
        private readonly IInteQualificationAuthenticationRepository _inteQualificationAuthenticationRepository;

        private readonly IInteQualificationAuthenticationDetailsRepository _authenticationDetailsRepository;
        //private readonly IProcResourceQualificationAuthenticationRelationRepository _procResourceQualification;
        //private readonly IProcProcedureQualificationAuthenticationRelationRepository _procProcedureQualification;

        /// <summary>
        /// 构造函数
        /// </summary>
        public InteQualificationAuthenticationService(ICurrentUser currentUser, ICurrentSite currentSite,
            AbstractValidator<InteQualificationAuthenticationSaveDto> validationSaveRules,
            IInteQualificationAuthenticationRepository inteQualificationAuthenticationRepository,
            IInteQualificationAuthenticationDetailsRepository authenticationDetailsRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _inteQualificationAuthenticationRepository = inteQualificationAuthenticationRepository;
            _authenticationDetailsRepository = authenticationDetailsRepository;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(InteQualificationAuthenticationSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            var siteId = _currentSite.SiteId ?? 0;
            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // 编码唯一性验证
            var code = saveDto.Code.Trim().ToUpperInvariant();
            var checkEntity = await _inteQualificationAuthenticationRepository.GetByCodeAsync(new EntityByCodeQuery
            {
                Site = siteId,
                Code = code
            });
            if (checkEntity != null) throw new CustomerValidationException(nameof(ErrorCode.MES10521)).WithData("Code", code);

            var entity = new InteQualificationAuthenticationEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                Code = code,
                Name = saveDto.Name.Trim(),
                Remark = saveDto.Remark.Trim(),
                Status = saveDto.Status,
                CreatedBy = updatedBy,
                CreatedOn = updatedOn,
                UpdatedBy = updatedBy,
                UpdatedOn = updatedOn,
                SiteId = _currentSite.SiteId ?? 0
            };

            var detailsEntities = new List<InteQualificationAuthenticationDetailsEntity>();
            if (saveDto.Details != null && saveDto.Details.Any())
            {
                foreach (var detail in saveDto.Details)
                {
                    var model = new InteQualificationAuthenticationDetailsEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        Type = detail.Type,
                        AuthenticationId = entity.Id,
                        CreatedBy = updatedBy,
                        CreatedOn = updatedOn,
                        UpdatedBy = updatedBy,
                        UpdatedOn = updatedOn,
                        SiteId = _currentSite.SiteId ?? 0
                    };
                    string value = detail.Values != null ? string.Join(",", detail.Values) : "";
                    if (detail.Type == QualificationAuthenticationTypeEnum.User)
                    {
                        model.UserNames = value;
                    }
                    else
                    {
                        model.RoleIds = value;
                    }
                    detailsEntities.Add(model);
                }
            }

            var rows = 0;
            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                if (detailsEntities.Any())
                {
                    rows += await _authenticationDetailsRepository.InsertRangeAsync(detailsEntities);
                }

                rows += await _inteQualificationAuthenticationRepository.InsertAsync(entity);

                ts.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(InteQualificationAuthenticationSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            var authenticationEntity = await _inteQualificationAuthenticationRepository.GetByIdAsync(saveDto.Id);
            if (authenticationEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10104));
            }

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();
            // DTO转换实体
            authenticationEntity.Name = saveDto.Name;
            authenticationEntity.Remark = saveDto.Remark;
            authenticationEntity.Status = saveDto.Status;
            authenticationEntity.UpdatedBy = _currentUser.UserName;
            authenticationEntity.UpdatedOn = HymsonClock.Now();

            var detailsEntities = new List<InteQualificationAuthenticationDetailsEntity>();
            if (saveDto.Details != null && saveDto.Details.Any())
            {
                foreach (var detail in saveDto.Details)
                {
                    var model = new InteQualificationAuthenticationDetailsEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        Type = detail.Type,
                        AuthenticationId = authenticationEntity.Id,
                        CreatedBy = updatedBy,
                        CreatedOn = updatedOn,
                        UpdatedBy = updatedBy,
                        UpdatedOn = updatedOn,
                        SiteId = _currentSite.SiteId ?? 0
                    };
                    string value = detail.Values != null ? string.Join(",", detail.Values) : "";
                    if (detail.Type == QualificationAuthenticationTypeEnum.User)
                    {
                        model.UserNames = value;
                    }
                    else
                    {
                        model.RoleIds = value;
                    }
                    detailsEntities.Add(model);
                }
            }

            var rows = 0;
            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                //删除
                await _authenticationDetailsRepository.DeleteByAuthenticationIdAsync(authenticationEntity.Id);
                if (detailsEntities.Any())
                {
                    rows += await _authenticationDetailsRepository.InsertRangeAsync(detailsEntities);
                }

                rows += await _inteQualificationAuthenticationRepository.UpdateAsync(authenticationEntity);

                ts.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _inteQualificationAuthenticationRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            //判断有没有被引用
            return await _inteQualificationAuthenticationRepository.DeletesAsync(new DeleteCommand
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
        public async Task<InteQualificationAuthenticationDto?> QueryByIdAsync(long id)
        {
            var authenticationEntity = await _inteQualificationAuthenticationRepository.GetByIdAsync(id);
            if (authenticationEntity == null) return null;

            return new InteQualificationAuthenticationDto
            {
                Id = authenticationEntity.Id,
                Code = authenticationEntity.Code,
                Name = authenticationEntity.Name,
                Status = authenticationEntity.Status,
                Remark = authenticationEntity.Remark
            };
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteQualificationAuthenticationDetailsDto>> GetcDetailsAsync(long id)
        {
            var list = new List<InteQualificationAuthenticationDetailsDto>();
            var detailsEntities = await _authenticationDetailsRepository.GetEntitiesAsync(new InteQualificationAuthenticationDetailsQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                AuthenticationId = id
            });
            if (detailsEntities == null) return list;

            foreach (var detailsEntity in detailsEntities)
            {
                var detailsDto = new InteQualificationAuthenticationDetailsDto
                {
                    Type = detailsEntity.Type,
                    Values = null
                };
                if (detailsEntity.Type == QualificationAuthenticationTypeEnum.User)
                {
                    detailsDto.Values = string.IsNullOrWhiteSpace(detailsEntity.UserNames) ? null : detailsEntity.UserNames.Split(',');
                }

                if (detailsEntity.Type == QualificationAuthenticationTypeEnum.Role)
                {
                    detailsDto.Values = string.IsNullOrWhiteSpace(detailsEntity.RoleIds) ? null : detailsEntity.RoleIds.Split(',');
                }
                list.Add(detailsDto);
            }
            return list;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteQualificationAuthenticationDto>> GetPagedListAsync(InteQualificationAuthenticationPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<InteQualificationAuthenticationPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _inteQualificationAuthenticationRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<InteQualificationAuthenticationDto>());
            return new PagedInfo<InteQualificationAuthenticationDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

    }
}
