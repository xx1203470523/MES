using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Integrated.Query;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Text.RegularExpressions;

namespace Hymson.MES.Services.Services.Integrated
{
    /// <summary>
    /// 服务（字段分配管理） 
    /// </summary>
    public class InteBusinessFieldDistributeService : IInteBusinessFieldDistributeService
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
        private readonly AbstractValidator<InteBusinessFieldDistributeSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（字段分配管理）
        /// </summary>
        private readonly IInteBusinessFieldDistributeRepository _inteBusinessFieldDistributeRepository;

        /// <summary>
        /// 仓储接口（字段定义）
        /// </summary>
        private readonly IInteBusinessFieldRepository _inteBusinessFieldRepository;


        private readonly IInteBusinessFieldDistributeDetailsRepository _inteBusinessFieldDistributeDetailsRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="inteBusinessFieldDistributeRepository"></param>
        /// <param name="inteBusinessFieldDistributeDetailsRepository"></param>
        /// <param name="inteBusinessFieldRepository"></param>
        public InteBusinessFieldDistributeService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<InteBusinessFieldDistributeSaveDto> validationSaveRules,
            IInteBusinessFieldDistributeRepository inteBusinessFieldDistributeRepository,
            IInteBusinessFieldDistributeDetailsRepository inteBusinessFieldDistributeDetailsRepository, IInteBusinessFieldRepository inteBusinessFieldRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _inteBusinessFieldDistributeRepository = inteBusinessFieldDistributeRepository;
            _inteBusinessFieldDistributeDetailsRepository = inteBusinessFieldDistributeDetailsRepository;
            _inteBusinessFieldRepository = inteBusinessFieldRepository;
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<BusinessFieldViewDto>> getBusinessFieldDistributeDetailsByIdAsync(long id)
        {
            var inteBusinessFieldLists = new List<BusinessFieldViewDto>();
            var inteBusinessFieldEntity = await _inteBusinessFieldDistributeDetailsRepository.GetByIdAsync(id);
            //if (inteBusinessFieldEntity == null) return null;
            if (inteBusinessFieldEntity.Any())
            {
                inteBusinessFieldEntity = inteBusinessFieldEntity.OrderBy(x => x.Seq).ToList();
                foreach (var item in inteBusinessFieldEntity)
                {
                    var inteVehicleTypeVerifyDto = item.ToModel<BusinessFieldViewDto>();
                    if (inteVehicleTypeVerifyDto.BusinessFieldId > 0)
                    {
                        var inteBusinessField = await _inteBusinessFieldRepository.GetByIdAsync(inteVehicleTypeVerifyDto.BusinessFieldId);
                        inteVehicleTypeVerifyDto.Code = inteBusinessField.Code;
                        inteVehicleTypeVerifyDto.Name = inteBusinessField.Name;
                    }
                    inteBusinessFieldLists.Add(inteVehicleTypeVerifyDto);
                }
            }
            return inteBusinessFieldLists;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(InteBusinessFieldDistributeSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<InteBusinessFieldDistributeEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 验证是否编码唯一
            var inteBusinessFieldEntity = await _inteBusinessFieldDistributeRepository.GetByCodeAsync(new InteBusinessFieldDistributeQuery
            {
                Code = entity.Code.Trim(),
                SiteId = _currentSite.SiteId ?? 0
            });
            if (inteBusinessFieldEntity != null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19427));
            }

            #region 处理 载具类型验证数据
            List<InteBusinessFieldDistributeDetailsEntity> detailEntities = new();
            if (saveDto.InteBusinessFieldDistributeDetailList != null && saveDto.InteBusinessFieldDistributeDetailList.Any())
            {
                foreach (var item in saveDto.InteBusinessFieldDistributeDetailList)
                {
                    //验证数据
                    var pattern = @"^[1-9]\d*$";
                    if (!Regex.IsMatch($"{item.Seq}", pattern)) throw new CustomerValidationException(nameof(ErrorCode.MES19438));
                    var isSeqCount = saveDto.InteBusinessFieldDistributeDetailList.GroupBy(p => p.Seq)
                                         .Any(g => g.Count() > 1);
                    if (isSeqCount)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES19439));
                    }

                    var isBusinessFieldIds = saveDto.InteBusinessFieldDistributeDetailList.GroupBy(p => p.BusinessFieldId)
                                         .Any(g => g.Count() > 1);
                    if (isBusinessFieldIds)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES19440));
                    }
                    detailEntities.Add(new InteBusinessFieldDistributeDetailsEntity()
                    {
                        BusinessFieldId = item.BusinessFieldId,
                        Seq = item.Seq,
                        BusinessFieldFistributeid = entity.Id,
                        Id = IdGenProvider.Instance.CreateId(),
                        IsRequired = item.IsRequired,
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName,
                        CreatedOn = HymsonClock.Now(),
                        UpdatedOn = HymsonClock.Now(),
                        SiteId = _currentSite.SiteId ?? 0,
                    });
                }
            }
            #endregion

            using var ts = TransactionHelper.GetTransactionScope();
            await _inteBusinessFieldDistributeRepository.InsertAsync(entity);
            if (detailEntities.Any())
            {
                await _inteBusinessFieldDistributeDetailsRepository.InsertRangeAsync(detailEntities);
            }
            ts.Complete();
            return entity.Id;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(InteBusinessFieldDistributeSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<InteBusinessFieldDistributeEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            #region 处理 载具类型验证数据
            List<InteBusinessFieldDistributeDetailsEntity> detailEntities = new();
            if (saveDto.InteBusinessFieldDistributeDetailList != null && saveDto.InteBusinessFieldDistributeDetailList.Any())
            {
                foreach (var item in saveDto.InteBusinessFieldDistributeDetailList)
                {
                    //验证数据
                    var pattern = @"^[1-9]\d*$";
                    if (!Regex.IsMatch($"{item.Seq}", pattern)) throw new CustomerValidationException(nameof(ErrorCode.MES19438));
                    var isSeqCount = saveDto.InteBusinessFieldDistributeDetailList.GroupBy(p => p.Seq)
                                         .Any(g => g.Count() > 1);
                    if (isSeqCount)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES19439));
                    }

                    var isBusinessFieldIds = saveDto.InteBusinessFieldDistributeDetailList.GroupBy(p => p.BusinessFieldId)
                                         .Any(g => g.Count() > 1);
                    if (isBusinessFieldIds)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES19440));
                    }
                    detailEntities.Add(new InteBusinessFieldDistributeDetailsEntity()
                    {
                        BusinessFieldId = item.BusinessFieldId,
                        Seq = item.Seq,
                        BusinessFieldFistributeid = entity.Id,
                        Id = IdGenProvider.Instance.CreateId(),
                        IsRequired = item.IsRequired,
                        UpdatedBy = _currentUser.UserName,
                        UpdatedOn = HymsonClock.Now(),
                        SiteId = _currentSite.SiteId ?? 0,
                    });
                }
            }
            #endregion
            var rows = 0;
            using var ts = TransactionHelper.GetTransactionScope();
            rows += await _inteBusinessFieldDistributeRepository.UpdateAsync(entity);
            await _inteBusinessFieldDistributeDetailsRepository.DeleteAsync(entity.Id);
            if (detailEntities.Any())
            {
                rows += await _inteBusinessFieldDistributeDetailsRepository.InsertRangeAsync(detailEntities);
            }
            ts.Complete();
            return rows;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _inteBusinessFieldDistributeRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            var rows = 0;
            using var ts = TransactionHelper.GetTransactionScope();
            rows += await _inteBusinessFieldDistributeDetailsRepository.DeletesAsync(new DeleteCommand { Ids = ids });

            rows += await _inteBusinessFieldDistributeRepository.DeletesAsync(new DeleteCommand
            {
                Ids = ids,
                DeleteOn = HymsonClock.Now(),
                UserId = _currentUser.UserName
            });
            ts.Complete();
            return rows;
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteBusinessFieldDistributeDto?> QueryByIdAsync(long id) 
        {
           var inteBusinessFieldDistributeEntity = await _inteBusinessFieldDistributeRepository.GetByIdAsync(id);
           if (inteBusinessFieldDistributeEntity == null) return null;
           return inteBusinessFieldDistributeEntity.ToModel<InteBusinessFieldDistributeDto>();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteBusinessFieldDistributeDto>> GetPagedListAsync(InteBusinessFieldDistributePagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<InteBusinessFieldDistributePagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _inteBusinessFieldDistributeRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<InteBusinessFieldDistributeDto>());
            return new PagedInfo<InteBusinessFieldDistributeDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

    }
}
