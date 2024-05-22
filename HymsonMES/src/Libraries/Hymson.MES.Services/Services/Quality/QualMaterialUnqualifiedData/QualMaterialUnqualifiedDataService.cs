using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Data.Repositories.Quality.Query;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.AspNetCore.Mvc.Formatters;
using Org.BouncyCastle.Crypto;
using System.Transactions;

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
        private readonly IWhMaterialInventoryRepository _inventoryRepository;
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IQualUnqualifiedGroupRepository _unqualifiedGroupRepository;
        private readonly IQualUnqualifiedCodeRepository _unqualifiedCodeRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public QualMaterialUnqualifiedDataService(ICurrentUser currentUser, ICurrentSite currentSite,
            AbstractValidator<QualMaterialUnqualifiedDataSaveDto> validationSaveRules,
            IQualMaterialUnqualifiedDataRepository qualMaterialUnqualifiedDataRepository,
            IQualMaterialUnqualifiedDataDetailRepository unqualifiedDataDetailRepository,
            IWhMaterialInventoryRepository inventoryRepository,
            IProcMaterialRepository procMaterialRepository,
            IQualUnqualifiedGroupRepository unqualifiedGroupRepository,
            IQualUnqualifiedCodeRepository unqualifiedCodeRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _qualMaterialUnqualifiedDataRepository = qualMaterialUnqualifiedDataRepository;
            _unqualifiedDataDetailRepository = unqualifiedDataDetailRepository;
            _inventoryRepository = inventoryRepository;
            _procMaterialRepository = procMaterialRepository;
            _unqualifiedGroupRepository = unqualifiedGroupRepository;
            _unqualifiedCodeRepository = unqualifiedCodeRepository;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task CreateAsync(QualMaterialUnqualifiedDataSaveDto saveDto)
        {
            #region 验证数据
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10101));
            }

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            //验证条码的数量、状态以及是否有打开的
            var inventoryEntity = await _inventoryRepository.GetByIdAsync(saveDto.MaterialInventoryId);
            if (inventoryEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15902));
            }

            if (inventoryEntity.Status != WhMaterialInventoryStatusEnum.ToBeUsed)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15903));
            }

            if (inventoryEntity.QuantityResidue == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15904));
            }

            //物料条码是否已记录不良且不良状态为打开，若是，则报错：物料条码XXX已录入不良且未进行不良处置
            var qualMaterials=await _qualMaterialUnqualifiedDataRepository.GetEntitiesAsync(new QualMaterialUnqualifiedDataQuery
            {
                SiteId=_currentSite.SiteId??0,
                MaterialInventoryId = saveDto.MaterialInventoryId,
                UnqualifiedStatus= QualMaterialUnqualifiedStatusEnum.Open
            });
            if(qualMaterials!=null && qualMaterials.Any()){
                throw new CustomerValidationException(nameof(ErrorCode.MES15905));
            }
            #endregion

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = new QualMaterialUnqualifiedDataEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                MaterialInventoryId = saveDto.MaterialInventoryId,
                UnqualifiedStatus = QualMaterialUnqualifiedStatusEnum.Open,
                UnqualifiedRemark = saveDto.UnqualifiedRemark,
                CreatedBy = updatedBy,
                CreatedOn = updatedOn,
                UpdatedBy = updatedBy,
                UpdatedOn = updatedOn,
                SiteId = _currentSite.SiteId ?? 0
            };

            var detailEntities = new List<QualMaterialUnqualifiedDataDetailEntity>();
            //记录详情
            foreach (var detail in saveDto.DetailDtos)
            {
                detailEntities.Add(new QualMaterialUnqualifiedDataDetailEntity
                {
                    MaterialUnqualifiedDataId = entity.Id,
                    UnqualifiedGroupId = detail.UnqualifiedGroupId,
                    UnqualifiedCodeId = detail.UnqualifiedCodeId,
                    CreatedBy = updatedBy,
                    CreatedOn = updatedOn
                });
            }

            //更改库存为锁定状态
            var updateStatusCommand = new UpdateStatusByIdCommand
            {
                Status = WhMaterialInventoryStatusEnum.Locked,
                Ids = new[] { saveDto.MaterialInventoryId }
            };

            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                // 保存
                await _qualMaterialUnqualifiedDataRepository.InsertAsync(entity);

                //保存详情
                if (detailEntities.Any())
                {
                    await _unqualifiedDataDetailRepository.InsertRangeAsync(detailEntities);
                }

                //更改条码状态为锁定状态
                await _inventoryRepository.UpdateStatusByIdsAsync(updateStatusCommand);
                ts.Complete();
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task ModifyAsync(QualMaterialUnqualifiedDataSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10101));
            }

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            var entityOld = await _qualMaterialUnqualifiedDataRepository.GetByIdAsync(saveDto?.Id ?? 0);
            if (entityOld == null)
            {
                throw new BusinessException(nameof(ErrorCode.MES10104));
            }

            // DTO转换实体
            entityOld.UnqualifiedRemark = saveDto.UnqualifiedRemark;
            entityOld.UpdatedBy = updatedBy;
            entityOld.UpdatedOn = HymsonClock.Now();

            var detailEntities = new List<QualMaterialUnqualifiedDataDetailEntity>();
            //记录详情
            foreach (var detail in saveDto.DetailDtos)
            {
                detailEntities.Add(new QualMaterialUnqualifiedDataDetailEntity
                {
                    MaterialUnqualifiedDataId = entityOld.Id,
                    UnqualifiedGroupId = detail.UnqualifiedGroupId,
                    UnqualifiedCodeId = detail.UnqualifiedCodeId,
                    CreatedBy = _currentUser.UserName,
                    CreatedOn = updatedOn
                });
            }

            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                // 保存
                await _qualMaterialUnqualifiedDataRepository.UpdateAsync(entityOld);

                //删除之前的数据
                await _unqualifiedDataDetailRepository.DeleteByDataIdAsync(entityOld.Id);

                //保存详情
                if (detailEntities.Any())
                {
                    await _unqualifiedDataDetailRepository.InsertRangeAsync(detailEntities);
                }
                ts.Complete();
            }
        }

        /// <summary>
        /// 处置
        /// </summary>
        /// <param name="disposalDto"></param>
        /// <returns></returns>
        public async Task DisposalAsync(QualMaterialUnqualifiedDataDisposalDto disposalDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10101));
            }

            var entityOld = await _qualMaterialUnqualifiedDataRepository.GetByIdAsync(disposalDto?.Id ?? 0);
            if (entityOld == null)
            {
                throw new BusinessException(nameof(ErrorCode.MES10104));
            }

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            entityOld.UnqualifiedStatus = QualMaterialUnqualifiedStatusEnum.Close;
            entityOld.DisposalResult = disposalDto.DisposalResult;
            entityOld.DisposalRemark = disposalDto.DisposalRemark;
            entityOld.DisposalTime = updatedOn;
            entityOld.UpdatedBy = updatedBy;
            entityOld.UpdatedOn = updatedOn;

            var updateStatusCommand = new UpdateStatusByIdCommand
            {
                Status = WhMaterialInventoryStatusEnum.ToBeUsed,
                Ids = new[] { entityOld.MaterialInventoryId }
            };

            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                // 保存
                await _qualMaterialUnqualifiedDataRepository.UpdateAsync(entityOld);

                //如果处置方式为放行解锁，若处置方式为退料不解锁
                if (disposalDto.DisposalResult == QualMaterialDisposalResultEnum.Release)
                {
                    await _inventoryRepository.UpdateStatusByIdsAsync(updateStatusCommand);
                }
                ts.Complete();
            }
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
            if (entitys.Any(x => x.UnqualifiedStatus == QualMaterialUnqualifiedStatusEnum.Close))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15901));
            }

            var inventoryIds = entitys.Select(x => x.MaterialInventoryId).ToArray();
            var rows = 0;
            var updateOn = HymsonClock.Now();
            var updateStatusCommand = new UpdateStatusByIdCommand
            {
                Status = WhMaterialInventoryStatusEnum.ToBeUsed,
                Ids = inventoryIds,
                UpdatedBy = _currentUser.UserName,
                UpdatedOn = updateOn
            };

            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                //删除后同时解除物料条码的库存锁定状态
                rows += await _inventoryRepository.UpdateStatusByIdsAsync(updateStatusCommand);

                rows += await _qualMaterialUnqualifiedDataRepository.DeletesAsync(new DeleteCommand
                {
                    Ids = ids,
                    DeleteOn = updateOn,
                    UserId = _currentUser.UserName
                });
                ts.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<QualMaterialUnqualifiedDataDto?> QueryByIdAsync(long id)
        {
            var dataEntity = await _qualMaterialUnqualifiedDataRepository.GetByIdAsync(id);
            if (dataEntity == null)
            {
                return null;
            }

            var inventoryEntity = await _inventoryRepository.GetByIdAsync(dataEntity.MaterialInventoryId);

            var procMaterialEntity = new ProcMaterialEntity();
            if (inventoryEntity != null)
            {
                procMaterialEntity = await _procMaterialRepository.GetByIdAsync(inventoryEntity.MaterialId);
            }

            var qualMaterial = new QualMaterialUnqualifiedDataDto
            {
                Id = dataEntity.Id,
                MaterialInventoryId = dataEntity.MaterialInventoryId,
                MaterialBarCode = inventoryEntity?.MaterialBarCode ?? "",
                QuantityResidue = inventoryEntity?.QuantityResidue ?? 0,
                MaterialGroupId = procMaterialEntity.GroupId,
                MaterialCode = procMaterialEntity.MaterialCode + "/" + procMaterialEntity.Version,
                MaterialName = procMaterialEntity.MaterialName,
                UnqualifiedRemark = dataEntity.UnqualifiedRemark ?? "",
                DisposalRemark = dataEntity.DisposalRemark ?? "",
                DisposalResult = dataEntity.DisposalResult,
                Details = new List<QualMaterialUnqualifiedDetailDataDto>()
            };

            //查询关联的不合格代码组和不合格代码
            var details = new List<QualMaterialUnqualifiedDetailDataDto>();
            var qualMaterialUnqualifieds = await _unqualifiedDataDetailRepository.GetEntitiesAsync(new QualMaterialUnqualifiedDataDetailQuery
            {
                MaterialUnqualifiedDataId = qualMaterial.Id
            });
            if (qualMaterialUnqualifieds != null && qualMaterialUnqualifieds.Any())
            {
                var groupIds = qualMaterialUnqualifieds.Select(x => x.UnqualifiedGroupId).Distinct().ToArray();
                var codeIds = qualMaterialUnqualifieds.Select(x => x.UnqualifiedCodeId).Distinct().ToArray();

                var unqualifiedGroupEntities = await _unqualifiedGroupRepository.GetByIdsAsync(groupIds);
                var unqualifiedCodeEntities = await _unqualifiedCodeRepository.GetByIdsAsync(codeIds);

                foreach (var entity in qualMaterialUnqualifieds)
                {
                    var group = unqualifiedGroupEntities.FirstOrDefault(x => x.Id == entity.UnqualifiedGroupId);
                    var code = unqualifiedCodeEntities.FirstOrDefault(x => x.Id == entity.UnqualifiedCodeId);
                    details.Add(new QualMaterialUnqualifiedDetailDataDto
                    {
                        UnqualifiedGroupId= group?.Id??0,
                        UnqualifiedCodeId= code?.Id??0,
                        UnqualifiedCode = code?.UnqualifiedCode??"",
                        UnqualifiedGroupRemark = group!=null? group.UnqualifiedGroup  + "/" + group.UnqualifiedGroupName:"",
                        UnqualifiedCodeRemark = code!=null? code.UnqualifiedCode + "/" + code.UnqualifiedCodeName:"",
                    });
                }
                qualMaterial.Details = details;
            }
            return qualMaterial;
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
                    CreatedOn = item.CreatedOn,

                });
            }
            return new PagedInfo<QualMaterialUnqualifiedDataViewDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }
    }
}
