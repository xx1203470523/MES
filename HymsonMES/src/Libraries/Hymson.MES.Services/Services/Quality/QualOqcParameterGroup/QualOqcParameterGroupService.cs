using AutoMapper.Execution;
using Elastic.Clients.Elasticsearch.QueryDsl;
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Qual;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Data.Repositories.Quality.Query;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Services;
using Hymson.MES.Services.Dtos.Qual;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Text;

namespace Hymson.MES.Services.Services.Quality
{
    /// <summary>
    /// 服务（OQC检验参数组） 
    /// </summary>
    public class QualOqcParameterGroupService : IQualOqcParameterGroupService
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
        private readonly AbstractValidator<QualOqcParameterGroupDto> _validationSaveRules;

        /// <summary>
        /// 更新验证器
        /// </summary>
        private readonly AbstractValidator<QualOqcParameterGroupUpdateDto> _validationUpdateRules;

        /// <summary>
        /// 删除验证器
        /// </summary>
        private readonly AbstractValidator<QualOqcParameterGroupDeleteDto> _validationDeleteRules;

        /// <summary>
        /// 仓储接口（OQC检验参数组）
        /// </summary>
        private readonly IQualOqcParameterGroupRepository _qualOqcParameterGroupRepository;
        private readonly IQualOqcParameterGroupDetailRepository _qualOqcParameterGroupDetailRepository;


        private readonly IProcMaterialRepository _procMaterialRepository;

        private readonly IInteCustomRepository _inteCustomRepository;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="validationUpdateRules"></param>
        /// <param name="validationDeleteRules"></param>
        /// <param name="qualOqcParameterGroupRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="inteCustomRepository"></param>
        /// <param name="qualOqcParameterGroupDetailRepository"></param>
        public QualOqcParameterGroupService(ICurrentUser currentUser, ICurrentSite currentSite,
            AbstractValidator<QualOqcParameterGroupDto> validationSaveRules,
            AbstractValidator<QualOqcParameterGroupUpdateDto> validationUpdateRules,
            AbstractValidator<QualOqcParameterGroupDeleteDto> validationDeleteRules,
            IQualOqcParameterGroupRepository qualOqcParameterGroupRepository,
            IProcMaterialRepository procMaterialRepository, IInteCustomRepository inteCustomRepository,
            IQualOqcParameterGroupDetailRepository qualOqcParameterGroupDetailRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _validationUpdateRules = validationUpdateRules;
            _validationDeleteRules = validationDeleteRules;
            _qualOqcParameterGroupRepository = qualOqcParameterGroupRepository;
            _procMaterialRepository = procMaterialRepository;
            _inteCustomRepository = inteCustomRepository;
            _qualOqcParameterGroupDetailRepository = qualOqcParameterGroupDetailRepository;
        }

        /// <summary>
        /// 获取单个
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        public async Task<QualOqcParameterGroupOutputDto> GetOneAsync(QualOqcParameterGroupQueryDto queryDto)
        {
            var query = queryDto.ToQuery<QualOqcParameterGroupToQuery>();
            query.SiteId = _currentSite.SiteId;

            var qualIqcInspectionItemEntity = await _qualOqcParameterGroupRepository.GetOneAsync(query);
            if (qualIqcInspectionItemEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10104));
            }

            var result = qualIqcInspectionItemEntity.ToModel<QualOqcParameterGroupOutputDto>();
            if (result == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10104));
            }

            var materialEntity = await _procMaterialRepository.GetByIdAsync(result.MaterialId.GetValueOrDefault());
            if (materialEntity != null)
            {
                result.MaterialName = materialEntity.MaterialName;
                result.MaterialCode = materialEntity.MaterialCode;
                result.MaterialUnit = materialEntity.Unit;
                result.MaterialVersion = materialEntity.Version;
            }

            var customerEntity = await _inteCustomRepository.GetByIdAsync(result.CustomerId.GetValueOrDefault());
            if (customerEntity != null)
            {
                result.CustomerCode = customerEntity.Code;
                result.CustomerName = customerEntity.Name;
            }

            return result;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        public async Task CreateAsync(QualOqcParameterGroupDto createDto)
        {
            await _validationSaveRules.ValidateAndThrowAsync(createDto);

            var command = createDto.ToCommand<QualOqcParameterGroupCreateCommand>();
            command.Init();
            command.CreatedBy = _currentUser.UserName;
            command.UpdatedBy = _currentUser.UserName;
            command.SiteId = _currentSite.SiteId;

            if (command.Code != null || command.Name != null)
            {
                var projectCode = new QualOqcParameterGroupToQuery
                {
                    Code = command.Code,
                    Name = command.Name,
                };
                //校验项目编码
                var qualIqcInspectionItemEntity = await _qualOqcParameterGroupRepository.GetOneAsync(projectCode);
                if (qualIqcInspectionItemEntity != null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES19955));
                }
            }

            if (command.MaterialId != null && command.CustomerId != null && command.Version != null)
            {
                var projectCode = new QualOqcParameterGroupToQuery
                {
                    MaterialId = command.MaterialId,
                    CustomerId = command.CustomerId,
                    Version = command.Version,
                };
                //相同的客户 + 物料 + 检验项目版本
                var Entity = await _qualOqcParameterGroupRepository.GetOneAsync(projectCode);
                if (Entity != null)
                {
                    var materialEntity = await _procMaterialRepository.GetByIdAsync(Entity.MaterialId);
                    var customerEntity = await _inteCustomRepository.GetByIdAsync(Entity.CustomerId);
                    throw new CustomerValidationException(nameof(ErrorCode.MES19956)).WithData("materialCode", materialEntity.MaterialCode).WithData("customCode", customerEntity.Code).WithData("version", command.Version);
                }
            }

            var detailCommands = Enumerable.Empty<QualOqcParameterGroupDetailCreateCommand>();
            if (createDto.qualOqcParameterGroupDetailDtos != null && createDto.qualOqcParameterGroupDetailDtos.Any())
            {
                detailCommands = createDto.qualOqcParameterGroupDetailDtos.Select(m =>
                {
                    var detailCommand = m.ToCommand<QualOqcParameterGroupDetailCreateCommand>();
                    detailCommand.Init();
                    detailCommand.ParameterGroupId = command.Id;
                    detailCommand.CreatedBy = command.CreatedBy;
                    detailCommand.UpdatedBy = command.UpdatedBy;
                    detailCommand.SiteId = command.SiteId;

                    return detailCommand;
                });
            }


            using var scope = TransactionHelper.GetTransactionScope();

            var affectedRow = await _qualOqcParameterGroupRepository.InsertIgnoreAsync(command);
            if (affectedRow == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19950));
            }

            await _qualOqcParameterGroupDetailRepository.InsertAsync(detailCommands);

            scope.Complete();


        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="updateDto"></param>
        /// <returns></returns>
        public async Task ModifyAsync(QualOqcParameterGroupUpdateDto updateDto)
        {
            await _validationUpdateRules.ValidateAndThrowAsync(updateDto);

            var entity = await _qualOqcParameterGroupRepository.GetOneAsync(new QualOqcParameterGroupToQuery { Id = updateDto.Id });
            if (entity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19954));
            }

            var command = updateDto.ToCommand<QualOqcParameterGroupUpdateCommand>();
            command.Init();
            command.SiteId = _currentSite.SiteId;
            command.UpdatedBy = _currentUser.UserName;

            var detailCommands = Enumerable.Empty<QualOqcParameterGroupDetailCreateCommand>();
            if (updateDto.qualOqcParameterGroupDetailDtos != null && updateDto.qualOqcParameterGroupDetailDtos.Any())
            {
                detailCommands = updateDto.qualOqcParameterGroupDetailDtos.Select(m =>
                {
                    var detailCommand = m.ToCommand<QualOqcParameterGroupDetailCreateCommand>();
                    detailCommand.Init();
                    detailCommand.ParameterGroupId = command.Id;
                    detailCommand.CreatedOn = entity.CreatedOn;
                    detailCommand.CreatedBy = entity.CreatedBy;
                    detailCommand.UpdatedBy = command.UpdatedBy;
                    detailCommand.UpdatedOn = command.UpdatedOn;
                    detailCommand.SiteId = command.SiteId;

                    return detailCommand;
                });
            }

            using var scope = TransactionHelper.GetTransactionScope();

            await _qualOqcParameterGroupDetailRepository.DeleteByMainIdAsync(entity.Id);

            await _qualOqcParameterGroupRepository.UpdateAsync(command);

            await _qualOqcParameterGroupDetailRepository.InsertAsync(detailCommands);

            scope.Complete();

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _qualOqcParameterGroupRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            return await _qualOqcParameterGroupRepository.DeletesAsync(new DeleteCommand
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
        public async Task<QualOqcParameterGroupDto?> QueryByIdAsync(long id)
        {
            var qualOqcParameterGroupEntity = await _qualOqcParameterGroupRepository.GetByIdAsync(id);
            if (qualOqcParameterGroupEntity == null) return null;

            return qualOqcParameterGroupEntity.ToModel<QualOqcParameterGroupDto>();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualOqcParameterGroupDto>> GetPagedListAsync(QualOqcParameterGroupPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<QualOqcParameterGroupPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _qualOqcParameterGroupRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<QualOqcParameterGroupDto>());
            return new PagedInfo<QualOqcParameterGroupDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 分页查询-包含参数明细
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualOqcParameterGroupOutputDto>> GetPagedAsync(QualOqcParameterGroupPagedQueryDto queryDto)
        {
            var _siteId = _currentSite.SiteId;
            List<long> emptyDefault = new List<long> { 0 };

            var materialIds = Enumerable.Empty<long>();
            if (!string.IsNullOrWhiteSpace(queryDto.MaterialCode))
            {
                var materialEntities = await _procMaterialRepository.GetProcMaterialEntitiesAsync(new ProcMaterialQuery { SiteId = _siteId, MaterialCode = queryDto.MaterialCode });
                materialIds = materialEntities.Select(m => m.Id);
                if (!materialIds.Any())
                {
                    materialIds = emptyDefault;
                }
            }
            if (!string.IsNullOrWhiteSpace(queryDto.MaterialName))
            {
                var materialEntities = await _procMaterialRepository.GetProcMaterialEntitiesAsync(new ProcMaterialQuery { SiteId = _siteId, MaterialName = queryDto.MaterialName });
                materialIds = materialIds.Concat(materialEntities.Select(m => m.Id));
                if (!materialIds.Any())
                {
                    materialIds = emptyDefault;
                }
            }
            materialIds = materialIds.Distinct();

            var customerIds = Enumerable.Empty<long>();
            if (!string.IsNullOrWhiteSpace(queryDto.CustomerName))
            {
                var customerEntities = await _inteCustomRepository.GetInteCustomEntitiesAsync(new InteCustomQuery { SiteId = _siteId, Name = queryDto.CustomerName });
                customerIds = customerEntities.Select(m => m.Id);
                if (!customerIds.Any())
                {
                    customerIds = emptyDefault;
                }
            }

            var query = new QualOqcParameterGroupPagedQuery
            {
                CodeLike = queryDto.Code,
                NameLike = queryDto.Name,
                MaterialIds = materialIds,
                CustomerIds = customerIds,
                Status = queryDto.Status,
                PageIndex = queryDto.PageIndex,
                PageSize = queryDto.PageSize,
                SiteId = _siteId
            };
            query.Sorting = "Id Desc";

            var result = new PagedInfo<QualOqcParameterGroupOutputDto>(Enumerable.Empty<QualOqcParameterGroupOutputDto>(), query.PageIndex, query.PageSize);

            var pageResult = await _qualOqcParameterGroupRepository.GetPagedListAsync(query);
            if (pageResult.Data != null && pageResult.Data.Any())
            {
                result.Data = pageResult.Data.Select(m => m.ToModel<QualOqcParameterGroupOutputDto>());
                result.TotalCount = pageResult.TotalCount;

                var resultMaterialIds = result.Data.Select(m => m.MaterialId.GetValueOrDefault());
                var resultCustomerIds = result.Data.Select(m => m.CustomerId.GetValueOrDefault());
                try
                {
                    var materialEntities = await _procMaterialRepository.GetByIdsAsync(resultMaterialIds);
                    var customerEntities = await _inteCustomRepository.GetByIdsAsync(resultCustomerIds);

                    result.Data = result.Data.Select(m =>
                    {
                        var materialEntity = materialEntities.FirstOrDefault(e => e.Id == m.MaterialId);
                        if (materialEntity != default)
                        {
                            m.MaterialCode = materialEntity.MaterialCode;
                            m.MaterialName = materialEntity.MaterialName;
                            m.MaterialUnit = materialEntity.Unit;
                            m.MaterialVersion = materialEntity.Version;
                        }

                        var customerEntity = customerEntities.FirstOrDefault(e => e.Id == m.CustomerId);
                        if (customerEntity != default)
                        {
                            m.CustomerName = customerEntity.Name;
                        }

                        return m;
                    });

                }
                catch (Exception ex) { }


            }

            return result;
        }


        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="deleteDto"></param>
        /// <returns></returns>
        public async Task DeleteAsync(QualOqcParameterGroupDeleteDto deleteDto)
        {
            await _validationDeleteRules.ValidateAndThrowAsync(deleteDto);

            var qualIqcInspectionItemEntities = await _qualOqcParameterGroupRepository.GetEntitiesAsync(new QualOqcParameterGroupQuery { Ids = deleteDto.Ids });

            if (qualIqcInspectionItemEntities.Any(m => m.Status == SysDataStatusEnum.Enable))
            {
                var codes = new StringBuilder();
                foreach (var item in qualIqcInspectionItemEntities.Where(m => m.Status == SysDataStatusEnum.Enable))
                {
                    codes.Append(item.Code);
                    codes.Append(',');
                }
                throw new CustomerValidationException(nameof(ErrorCode.MES19953)).WithData("codes", codes.ToString());
            }

            var command = new DeleteCommand { Ids = deleteDto.Ids };

            using var scope = TransactionHelper.GetTransactionScope();

            await _qualOqcParameterGroupRepository.DeleteMoreAsync(command);

            await _qualOqcParameterGroupDetailRepository.DeleteByMainIdsAsync(command.Ids);

            scope.Complete();
        }

    }
}
