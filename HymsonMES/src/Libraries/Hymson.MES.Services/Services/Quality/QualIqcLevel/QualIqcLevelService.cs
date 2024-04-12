using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Data.Repositories.Quality.Query;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;

namespace Hymson.MES.Services.Services.Quality
{
    /// <summary>
    /// 服务（IQC检验水平） 
    /// </summary>
    public class QualIqcLevelService : IQualIqcLevelService
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
        /// 验证器
        /// </summary>
        private readonly AbstractValidator<QualIqcLevelSaveDto> _validationSaveRules;

        /// <summary>
        /// 验证器
        /// </summary>
        private readonly AbstractValidator<QualIqcLevelDetailDto> _validationDetailRules;

        /// <summary>
        /// 仓储接口（IQC检验水平）
        /// </summary>
        private readonly IQualIqcLevelRepository _qualIqcLevelRepository;

        /// <summary>
        /// 仓储接口（IQC检验水平明细）
        /// </summary>
        private readonly IQualIqcLevelDetailRepository _qualIqcLevelDetailRepository;

        /// <summary>
        /// 仓储接口（物料维护）
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 仓储接口（供应商维护）
        /// </summary>
        private readonly IWhSupplierRepository _whSupplierRepository;

        /// <summary>
        /// 多语言服务
        /// </summary>
        private readonly ILocalizationService _localizationService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="validationDetailRules"></param>
        /// <param name="qualIqcLevelRepository"></param>
        /// <param name="qualIqcLevelDetailRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="whSupplierRepository"></param>
        /// <param name="localizationService"></param>
        public QualIqcLevelService(ICurrentUser currentUser, ICurrentSite currentSite,
            AbstractValidator<QualIqcLevelSaveDto> validationSaveRules,
            AbstractValidator<QualIqcLevelDetailDto> validationDetailRules,
            IQualIqcLevelRepository qualIqcLevelRepository,
            IQualIqcLevelDetailRepository qualIqcLevelDetailRepository,
            IProcMaterialRepository procMaterialRepository,
            IWhSupplierRepository whSupplierRepository,
            ILocalizationService localizationService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _validationDetailRules = validationDetailRules;
            _qualIqcLevelRepository = qualIqcLevelRepository;
            _qualIqcLevelDetailRepository = qualIqcLevelDetailRepository;
            _procMaterialRepository = procMaterialRepository;
            _whSupplierRepository = whSupplierRepository;
            _localizationService = localizationService;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(QualIqcLevelSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<QualIqcLevelEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.SiteId = _currentSite.SiteId ?? 0;
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;

            // 查询已存在系统中的数据
            var entitiesInSystem = await _qualIqcLevelRepository.GetEntitiesAsync(new QualIqcLevelQuery
            {
                SiteId = entity.SiteId,
                Type = entity.Type,
            });

            #region 唯一性校验
            switch (entity.Type)
            {
                case QCMaterialTypeEnum.General:
                    entity.MaterialId = null;
                    entity.SupplierId = null;

                    // 如果是通用类型（只允许存在一条）
                    if (entitiesInSystem.Any())
                    {
                        // 通用设置类型数据已存在，不允许重复创建！
                        throw new CustomerValidationException(nameof(ErrorCode.MES19408));
                    }
                    break;
                case QCMaterialTypeEnum.Material:
                    if (entity.MaterialId == null) throw new CustomerValidationException(nameof(ErrorCode.MES19406));
                    if (entity.SupplierId == null) throw new CustomerValidationException(nameof(ErrorCode.MES19407));

                    if (entitiesInSystem.Any(a => a.MaterialId == entity.MaterialId && a.SupplierId == a.SupplierId))
                    {
                        var materialEntity = await _procMaterialRepository.GetByIdAsync(entity.MaterialId.Value);
                        var supplierEntity = await _whSupplierRepository.GetByIdAsync(entity.SupplierId.Value);

                        // 物料编码XXX，供应商XXX已在系统存在
                        throw new CustomerValidationException(nameof(ErrorCode.MES19409))
                            .WithData("MaterialCode", materialEntity.MaterialCode)
                            .WithData("SupplierCode", supplierEntity.Code);
                    }
                    break;
                default:
                    break;
            }
            #endregion

            // 检验类型必须填写
            if (saveDto.Details == null || !saveDto.Details.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES19423));

            List<QualIqcLevelDetailEntity> details = new();
            foreach (var item in saveDto.Details)
            {
                if (item.Type.HasValue == false) throw new CustomerValidationException(nameof(ErrorCode.MES19420));
                if (item.VerificationLevel.HasValue == false) throw new CustomerValidationException(nameof(ErrorCode.MES19421));
                if (item.AcceptanceLevel.HasValue == false) throw new CustomerValidationException(nameof(ErrorCode.MES19422));

                // 验证DTO
                await _validationDetailRules.ValidateAndThrowAsync(item);

                var detailEntity = item.ToEntity<QualIqcLevelDetailEntity>();
                detailEntity.Id = IdGenProvider.Instance.CreateId();
                detailEntity.SiteId = entity.SiteId;
                detailEntity.IqcLevelId = entity.Id;
                detailEntity.CreatedBy = updatedBy;
                detailEntity.CreatedOn = updatedOn;
                detailEntity.UpdatedBy = updatedBy;
                detailEntity.UpdatedOn = updatedOn;

                details.Add(detailEntity);
            }

            // 每种检验类型只允许添加一次
            var typeCount = details.DistinctBy(s => s.Type).Count();
            if (typeCount < details.Count)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19418));
            }

            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows = await _qualIqcLevelRepository.InsertAsync(entity);
                if (rows <= 0)
                {
                    trans.Dispose();
                }
                else
                {
                    rows += await _qualIqcLevelDetailRepository.InsertRangeAsync(details);
                    trans.Complete();
                }
            }
            return entity.Id;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<long> ModifyAsync(QualIqcLevelSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<QualIqcLevelEntity>();
            entity.SiteId = _currentSite.SiteId ?? 0;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;

            // 查询已存在系统中的数据
            var entitiesInSystem = await _qualIqcLevelRepository.GetEntitiesAsync(new QualIqcLevelQuery
            {
                SiteId = entity.SiteId,
                Type = entity.Type,
            });

            #region 唯一性校验
            switch (entity.Type)
            {
                case QCMaterialTypeEnum.General:
                    entity.MaterialId = null;
                    entity.SupplierId = null;

                    // 如果是通用类型（只允许存在一条）
                    if (entitiesInSystem.Any(a => a.Id != entity.Id))
                    {
                        // 通用设置类型数据已存在，不允许重复创建！
                        throw new CustomerValidationException(nameof(ErrorCode.MES19408));
                    }
                    break;
                case QCMaterialTypeEnum.Material:
                    if (entity.MaterialId == null) throw new CustomerValidationException(nameof(ErrorCode.MES19406));
                    if (entity.SupplierId == null) throw new CustomerValidationException(nameof(ErrorCode.MES19407));

                    if (entitiesInSystem.Any(a => a.Id != entity.Id && a.MaterialId == entity.MaterialId && a.SupplierId == a.SupplierId))
                    {
                        var materialEntity = await _procMaterialRepository.GetByIdAsync(entity.MaterialId.Value);
                        var supplierEntity = await _whSupplierRepository.GetByIdAsync(entity.SupplierId.Value);

                        // 物料编码XXX，供应商XXX已在系统存在
                        throw new CustomerValidationException(nameof(ErrorCode.MES19409))
                            .WithData("MaterialCode", materialEntity.MaterialCode)
                            .WithData("SupplierCode", supplierEntity.Code);
                    }
                    break;
                default:
                    break;
            }
            #endregion

            // 检验类型必须填写
            if (saveDto.Details == null || !saveDto.Details.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES19423));

            List<QualIqcLevelDetailEntity> details = new();
            foreach (var item in saveDto.Details)
            {
                if (item.Type.HasValue == false) throw new CustomerValidationException(nameof(ErrorCode.MES19420));
                if (item.VerificationLevel.HasValue == false) throw new CustomerValidationException(nameof(ErrorCode.MES19421));
                if (item.AcceptanceLevel.HasValue == false) throw new CustomerValidationException(nameof(ErrorCode.MES19422));

                // 验证DTO
                await _validationDetailRules.ValidateAndThrowAsync(item);

                var detailEntity = item.ToEntity<QualIqcLevelDetailEntity>();
                detailEntity.Id = IdGenProvider.Instance.CreateId();
                detailEntity.SiteId = entity.SiteId;
                detailEntity.IqcLevelId = entity.Id;
                detailEntity.CreatedBy = updatedBy;
                detailEntity.CreatedOn = updatedOn;
                detailEntity.UpdatedBy = updatedBy;
                detailEntity.UpdatedOn = updatedOn;

                details.Add(detailEntity);
            }

            // 每种检验类型只允许添加一次
            var typeCount = details.DistinctBy(s => s.Type).Count();
            if (typeCount < details.Count())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19418));
            }

            var command = new DeleteByParentIdCommand
            {
                ParentId = entity.Id,
                UpdatedBy = updatedBy,
                UpdatedOn = updatedOn
            };

            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows += await _qualIqcLevelRepository.UpdateAsync(entity);
                rows += await _qualIqcLevelDetailRepository.DeleteByParentIdAsync(command);
                rows += await _qualIqcLevelDetailRepository.InsertRangeAsync(details);
                trans.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            if (!ids.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES10213));

            var entities = await _qualIqcLevelRepository.GetByIdsAsync(ids);
            if (entities != null && entities.Any(a => a.Status == DisableOrEnableEnum.Enable))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10135));
            }

            return await _qualIqcLevelRepository.DeletesAsync(new DeleteCommand
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
        public async Task<QualIqcLevelDto?> QueryByIdAsync(long id)
        {
            var entity = await _qualIqcLevelRepository.GetByIdAsync(id);
            if (entity == null) return null;

            // 实体到DTO转换
            var dto = entity.ToModel<QualIqcLevelDto>();

            // 读取产品
            if (entity.MaterialId.HasValue)
            {
                var materialEntity = await _procMaterialRepository.GetByIdAsync(entity.MaterialId.Value);
                if (materialEntity != null)
                {
                    dto.MaterialCode = materialEntity.MaterialCode;
                    dto.MaterialName = materialEntity.MaterialName;
                    dto.MaterialVersion = materialEntity.Version ?? "";
                }
            }

            // 读取供应商
            if (entity.SupplierId.HasValue)
            {
                var supplierEntity = await _whSupplierRepository.GetByIdAsync(entity.SupplierId.Value);
                if (supplierEntity != null)
                {
                    dto.SupplierCode = supplierEntity.Code;
                    dto.SupplierName = supplierEntity.Name;
                }
            }

            var details = await _qualIqcLevelDetailRepository.GetEntitiesAsync(new QualIqcLevelDetailQuery
            {
                IqcLevelIds = new[] { id }
            });

            dto.Details = details.Select(s => s.ToModel<QualIqcLevelDetailDto>());
            return dto;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualIqcLevelDto>> GetPagedListAsync(QualIqcLevelPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<QualIqcLevelPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;

            // 转换产品编码/版本变为产品ID
            if (!string.IsNullOrWhiteSpace(pagedQueryDto.MaterialCode) || !string.IsNullOrWhiteSpace(pagedQueryDto.MaterialName))
            {
                var procMaterialEntities = await _procMaterialRepository.GetProcMaterialEntitiesAsync(new ProcMaterialQuery
                {
                    SiteId = pagedQuery.SiteId,
                    MaterialCode = pagedQueryDto.MaterialCode,
                    MaterialName = pagedQueryDto.MaterialName
                });
                if (procMaterialEntities != null && procMaterialEntities.Any()) pagedQuery.MaterialIds = procMaterialEntities.Select(s => s.Id);
                else pagedQuery.MaterialIds = Array.Empty<long>();
            }

            // 转换供应商编码变为供应商ID
            if (!string.IsNullOrWhiteSpace(pagedQueryDto.SupplierCode))
            {
                var whSupplierEntities = await _whSupplierRepository.GetWhSupplierEntitiesAsync(new WhSupplierQuery
                {
                    SiteId = pagedQuery.SiteId,
                    Code = pagedQueryDto.SupplierCode
                });
                if (whSupplierEntities != null && whSupplierEntities.Any()) pagedQuery.SupplierIds = whSupplierEntities.Select(s => s.Id);
                else pagedQuery.SupplierIds = Array.Empty<long>();
            }

            // 查询数据
            var pagedInfo = await _qualIqcLevelRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = await PrepareDtos(pagedInfo.Data);
            return new PagedInfo<QualIqcLevelDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }



        /// <summary>
        /// 转换为Dto对象
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        private async Task<IEnumerable<QualIqcLevelDto>> PrepareDtos(IEnumerable<QualIqcLevelEntity> entities)
        {
            List<QualIqcLevelDto> dtos = new();

            // 读取产品
            var materialEntities = await _procMaterialRepository.GetByIdsAsync(entities.Where(w => w.MaterialId.HasValue).Select(x => x.MaterialId!.Value));
            var materialDic = materialEntities.ToDictionary(x => x.Id, x => x);

            // 读取供应商
            var supplierEntities = await _whSupplierRepository.GetByIdsAsync(entities.Where(w => w.SupplierId.HasValue).Select(x => x.SupplierId!.Value));
            var supplierDic = supplierEntities.ToDictionary(x => x.Id, x => x);

            foreach (var entity in entities)
            {
                var dto = entity.ToModel<QualIqcLevelDto>();
                if (dto == null) continue;

                // 产品
                if (!entity.MaterialId.HasValue)
                {
                    dto.MaterialCode = "-";
                    dto.MaterialName = "-";
                    dto.MaterialVersion = "-";
                    dto.SupplierCode = "-";
                    dto.SupplierName = "-";
                    dtos.Add(dto);
                    continue;
                }

                var productEntity = materialDic[entity.MaterialId.Value];
                if (productEntity != null)
                {
                    dto.MaterialCode = productEntity.MaterialCode;
                    dto.MaterialName = productEntity.MaterialName;
                    dto.MaterialVersion = productEntity.Version ?? "";
                }

                // 供应商
                if (!entity.SupplierId.HasValue)
                {
                    dto.SupplierCode = "-";
                    dto.SupplierName = "-";
                    dtos.Add(dto);
                    continue;
                }

                var supplierEntity = supplierDic[entity.SupplierId.Value];
                if (supplierEntity != null)
                {
                    dto.SupplierCode = supplierEntity.Code;
                    dto.SupplierName = supplierEntity.Name;
                }

                dtos.Add(dto);
            }

            return dtos;
        }


    }
}
