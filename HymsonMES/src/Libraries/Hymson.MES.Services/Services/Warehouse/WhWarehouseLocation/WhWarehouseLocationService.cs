using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.WhWarehouseLocation;
using Hymson.MES.Core.Domain.WhWarehouseShelf;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.WhWareHouse;
using Hymson.MES.Data.Repositories.WhWareHouse.Query;
using Hymson.MES.Data.Repositories.WhWarehouseLocation;
using Hymson.MES.Data.Repositories.WhWarehouseLocation.Query;
using Hymson.MES.Data.Repositories.WhWarehouseRegion;
using Hymson.MES.Data.Repositories.WhWarehouseRegion.Query;
using Hymson.MES.Data.Repositories.WhWarehouseShelf;
using Hymson.MES.Data.Repositories.WhWarehouseShelf.Query;
using Hymson.MES.Services.Dtos.WhWarehouseLocation;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Linq;
using System.Security.Policy;
using System.Text.RegularExpressions;

namespace Hymson.MES.Services.Services.WhWarehouseLocation
{
    /// <summary>
    /// 服务（库位） 
    /// </summary>
    public class WhWarehouseLocationService : IWhWarehouseLocationService
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
        private readonly AbstractValidator<WhWarehouseLocationSaveDto> _validationSaveRules;
        private readonly AbstractValidator<WhWarehouseLocationModifyDto> _validationModifyRules;

        /// <summary>
        /// 仓储接口（库位）
        /// </summary>
        private readonly IWhWarehouseLocationRepository _whWarehouseLocationRepository;
        private readonly IWhWarehouseShelfRepository _whWarehouseShelfRepository;
        private readonly IWhWarehouseRegionRepository _whWarehouseRegionRepository;
        private readonly IWhWarehouseRepository _whWarehouseRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="whWarehouseLocationRepository"></param>
        /// <param name="whWarehouseShelfRepository"></param>
        /// <param name="whWarehouseRegionRepository"></param>
        /// <param name="whWarehouseRepository"></param>
        /// <param name="validationModifyRules"></param>
        public WhWarehouseLocationService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<WhWarehouseLocationSaveDto> validationSaveRules,
            IWhWarehouseLocationRepository whWarehouseLocationRepository, IWhWarehouseShelfRepository whWarehouseShelfRepository, IWhWarehouseRegionRepository whWarehouseRegionRepository, IWhWarehouseRepository whWarehouseRepository, AbstractValidator<WhWarehouseLocationModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _whWarehouseLocationRepository = whWarehouseLocationRepository;
            _whWarehouseShelfRepository = whWarehouseShelfRepository;
            _whWarehouseRegionRepository = whWarehouseRegionRepository;
            _whWarehouseRepository = whWarehouseRepository;
            _validationModifyRules = validationModifyRules;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(WhWarehouseLocationSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10101));
            }

            if (saveDto.Type == WhWarehouseLocationTypeEnum.Customize && string.IsNullOrWhiteSpace(saveDto.Code)) {
                throw new CustomerValidationException(nameof(ErrorCode.MES19223));
            }

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            //获取货架
            var warehouseShelfEntity = await _whWarehouseShelfRepository.GetOneAsync(new WhWarehouseShelfQuery {Code= saveDto.WarehouseShelfCode, SiteId = _currentSite.SiteId ?? 0 });
            if (warehouseShelfEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19226));
            }

            //获取库区
            var warehouseRegionEntity = await _whWarehouseRegionRepository.GetOneAsync(new WhWarehouseRegionQuery { Id = warehouseShelfEntity.WarehouseRegionId, Code = saveDto.WarehouseRegionCode ,SiteId= _currentSite.SiteId ??0});
            if (warehouseRegionEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19225));
            }

            //获取仓库
            var warehouseEntity = await _whWarehouseRepository.GetOneAsync(new WhWarehouseQuery { Id = warehouseRegionEntity.WarehouseId,Code= saveDto.WarehouseCode,SiteId= _currentSite.SiteId ?? 0 });
            if (warehouseEntity == null) {
                throw new CustomerValidationException(nameof(ErrorCode.MES19224));
            }

            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            var warehouseLocationEntities = await _whWarehouseLocationRepository.GetEntitiesAsync(new WhWarehouseLocationQuery { WarehouseShelfId = warehouseShelfEntity.Id, SiteId = _currentSite.SiteId ?? 0 });

            //获取货架下的最大库位
            var maxColumn = 0;
            if (warehouseLocationEntities != null && warehouseLocationEntities.Any()&& saveDto.Type== WhWarehouseLocationTypeEnum.SpecifyRow)
            {
                //int number = 0;
                var code = $"{warehouseShelfEntity.Code}-{saveDto.Row}-";
                var warehouseLocationList = warehouseLocationEntities.Where(a=>a.Code.Contains(code));
                if (warehouseLocationList.Any())
                {
                    Regex regex = new Regex(@"^[0-9]+$");
                    warehouseLocationList = warehouseLocationList.Where(a => regex.IsMatch(a.Code.Split('-').Last().ToString()));
                    maxColumn = warehouseLocationList.Max(a => int.Parse(a.Code.Split('-').Last()));
                }
            }

            //获取库位编码
            var locationCodes = GetLocationCode(saveDto, warehouseShelfEntity, maxColumn);
            var entitys = new List<WhWarehouseLocationEntity>();

            foreach (var item in locationCodes)
            {
                var entity = saveDto.ToEntity<WhWarehouseLocationEntity>();
                entity.Id = IdGenProvider.Instance.CreateId();
                entity.CreatedBy = updatedBy;
                entity.CreatedOn = updatedOn;
                entity.UpdatedBy = updatedBy;
                entity.UpdatedOn = updatedOn;
                entity.SiteId = _currentSite.SiteId ?? 0;
                entity.Status = DisableOrEnableEnum.Enable;
                entity.Code = item;
                entity.WarehouseShelfId = warehouseShelfEntity.Id;
                entitys.Add(entity);
            }

            using var scope = TransactionHelper.GetTransactionScope();

            //自动生成时，需要先清空之前生成的库位
            if (saveDto.Type == WhWarehouseLocationTypeEnum.Automatically) {
                var warehouseLocationIds = warehouseLocationEntities?.Select(x => x.Id);
                if (warehouseLocationIds != null && warehouseLocationIds.Any())
                {
                    await _whWarehouseLocationRepository.DeletesPhysicsAsync(new DeleteCommand { Ids = warehouseLocationIds });
                }
            }

            // 保存
            var result= await _whWarehouseLocationRepository.InsertIgnoreRangeAsync(entitys);
            if (result != entitys.Count) {
                if (saveDto.Type == WhWarehouseLocationTypeEnum.Customize) {
                    throw new CustomerValidationException(nameof(ErrorCode.MES19214)).WithData("code", locationCodes.First());
                }
                throw new CustomerValidationException(nameof(ErrorCode.MES19213));
            }
            scope.Complete();
            return result;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(WhWarehouseLocationModifyDto modifyDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(modifyDto);

            // DTO转换实体
            var entity = modifyDto.ToEntity<WhWarehouseLocationEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();
            entity.SiteId = _currentSite.SiteId ?? 0;

            return await _whWarehouseLocationRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _whWarehouseLocationRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            return await _whWarehouseLocationRepository.DeletesAsync(new DeleteCommand
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
        public async Task<WhWarehouseLocationDto?> QueryByIdAsync(long id)
        {
            var whWarehouseLocationEntity = await _whWarehouseLocationRepository.GetOneAsync(new WhWarehouseLocationQuery {Id= id, SiteId = _currentSite.SiteId ?? 0 });
            if (whWarehouseLocationEntity == null)
            { 
                return null; 
            }

            var result= whWarehouseLocationEntity.ToModel<WhWarehouseLocationDto>();
            var warehouseShelfEntity = await _whWarehouseShelfRepository.GetOneAsync(new WhWarehouseShelfQuery {Id= whWarehouseLocationEntity.WarehouseShelfId, SiteId = _currentSite.SiteId??0 } );
            if (warehouseShelfEntity != null)
            {
                result.WarehouseShelfCode = warehouseShelfEntity.Code;
                result.WarehouseShelfName = warehouseShelfEntity.Name;

                var warehouseRegionEntity = await _whWarehouseRegionRepository.GetOneAsync(new WhWarehouseRegionQuery {Id= warehouseShelfEntity.WarehouseRegionId, SiteId = _currentSite.SiteId ?? 0 } );
                if (warehouseRegionEntity != null)
                {
                    result.WarehouseRegionCode= warehouseRegionEntity.Code;
                    result.WarehouseRegionName= warehouseRegionEntity.Name;
                }

                var warehouseEntity = await _whWarehouseRepository.GetOneAsync(new WhWarehouseQuery { Id= warehouseShelfEntity.WarehouseId, SiteId = _currentSite.SiteId ?? 0 } );
                if (warehouseEntity != null)
                {
                    result.WarehouseCode = warehouseEntity.Code;
                    result.WarehouseName = warehouseEntity.Name;
                }
            }

            return result;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<WhWarehouseLocationDto>> GetPagedListAsync(WhWarehouseLocationPagedQueryDto pagedQueryDto)
        {
            var returnData = new PagedInfo<WhWarehouseLocationDto>(new List<WhWarehouseLocationDto>(), pagedQueryDto.PageIndex, pagedQueryDto.PageSize);
            var pagedQuery = pagedQueryDto.ToQuery<WhWarehouseLocationPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            pagedQuery.CodeLike = pagedQueryDto.Code;

            var whWarehouseQuery = new WhWarehouseQuery();
            var whWarehouseRegionQuery = new WhWarehouseRegionQuery();
            var whWarehouseShelfQuery = new WhWarehouseShelfQuery();

            //获取仓库信息
            if (!string.IsNullOrWhiteSpace(pagedQueryDto.WarehouseCode)) {
                whWarehouseQuery.CodeLike = pagedQueryDto.WarehouseCode;
                
            }
            whWarehouseQuery.SiteId = _currentSite.SiteId ?? 0;
            var warehouseEntities = await _whWarehouseRepository.GetEntitiesAsync(whWarehouseQuery);
            if (warehouseEntities == null || !warehouseEntities.Any())
            {
                return returnData;
            }

            //获取库区信息
            if (!string.IsNullOrWhiteSpace(pagedQueryDto.WarehouseRegionCode)) {
                whWarehouseRegionQuery.CodeLike= pagedQueryDto.WarehouseRegionCode;
            }
            whWarehouseRegionQuery.WarehouseIds = warehouseEntities.Select(x => x.Id);
            var warehouseRegionEntities = await _whWarehouseRegionRepository.GetEntitiesAsync(whWarehouseRegionQuery);
            if (warehouseRegionEntities == null || !warehouseRegionEntities.Any())
            {
                return returnData;
            }

            //获取货架信息
            if (!string.IsNullOrWhiteSpace(pagedQueryDto.WarehouseShelfCode))
            {
                whWarehouseShelfQuery.CodeLike = pagedQueryDto.WarehouseShelfCode;
            }
            whWarehouseShelfQuery.WarehouseIds = warehouseEntities.Select(x => x.Id);
            whWarehouseShelfQuery.WarehouseRegionIds = warehouseRegionEntities.Select(x => x.Id);
            var warehouseShelfEntities = await _whWarehouseShelfRepository.GetEntitiesAsync(whWarehouseShelfQuery);
            if (warehouseShelfEntities == null || !warehouseShelfEntities.Any())
            {
                return returnData;
            }

            var pagedInfo = await _whWarehouseLocationRepository.GetPagedListAsync(pagedQuery);
            if (pagedInfo.Data == null || !pagedInfo.Data.Any()) { 
                return returnData;
            }

            var query = new WhWarehouseLocationQuery();
            query.CodeLike = pagedQueryDto.Code;
            query.WarehouseShelfIds = warehouseShelfEntities.Select(a => a.Id);
            query.SiteId= _currentSite.SiteId ?? 0;
            query.Status= pagedQueryDto.Status;
            var warehouseLocationEntities = await _whWarehouseLocationRepository.GetEntitiesAsync(query);

            var result=new List<WhWarehouseLocationDto>();
            var warehouseLocationOrderList = warehouseLocationEntities.OrderByDescending(a => a.Code).OrderByDescending(a => a.UpdatedOn);
            foreach (var item in warehouseLocationOrderList) {
                var model = item.ToModel<WhWarehouseLocationDto>();

                var warehouseShelfEntity = warehouseShelfEntities.FirstOrDefault(a => a.Id == item.WarehouseShelfId);
                if (warehouseShelfEntity != null) {
                    var warehouseRegionEntity = warehouseRegionEntities.FirstOrDefault(a => a.Id == warehouseShelfEntity.WarehouseRegionId);
                    var warehouseEntity = warehouseEntities.FirstOrDefault(a => a.Id == warehouseShelfEntity.WarehouseId);
                    if (warehouseRegionEntity != null && warehouseEntity != null) {
                        model.WarehouseCode = warehouseEntity.Code;
                        model.WarehouseName= warehouseEntity.Name;
                        model.WarehouseRegionCode= warehouseRegionEntity.Code;
                        model.WarehouseShelfCode= warehouseShelfEntity.Code;
                        result.Add(model);
                    }
                }
            }
            var pageIndex = (pagedQueryDto.PageIndex - 1) * pagedQueryDto.PageSize;
            returnData.Data= result.Skip(pageIndex).Take(pagedQueryDto.PageSize).OrderByDescending(a => a.Code).OrderByDescending(a=>a.UpdatedOn);
            returnData.TotalCount = result.Count;

            return returnData;
        }


        /// <summary>
        /// 根据查询条件获取数据
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<IEnumerable<WhWarehouseLocationDto>> GetListAsync(WhWarehouseLocationQueryDto queryDto)
        {
            var query = queryDto.ToQuery<WhWarehouseLocationQuery>();
            //query.WarehouseShelfId = queryDto.WarehouseShelfId;
            query.SiteId = _currentSite.SiteId ?? 0;
            var whWarehouseLocationEntities = await _whWarehouseLocationRepository.GetEntitiesAsync(query);

            // 实体到DTO转换 装载数据
            var dtos = whWarehouseLocationEntities.Select(s => s.ToModel<WhWarehouseLocationDto>());
            return dtos;
        }

        #region 私有方法

        /// <summary>
        /// 生成库位编码
        /// </summary>
        /// <param name="saveDto"></param>
        /// <param name="whWarehouseShelfEntity"></param>
        /// <param name="maxColumn"></param>
        /// <returns></returns>
        private List<string> GetLocationCode(WhWarehouseLocationSaveDto saveDto, WhWarehouseShelfEntity whWarehouseShelfEntity,int maxColumn)
        {
            var result = new List<string>();
           
            switch (saveDto.Type)
            {
                //自动生成
                case WhWarehouseLocationTypeEnum.Automatically:
                    for (int i = 1; i <= whWarehouseShelfEntity.Row; i++)
                    {
                        for (int j = 1; j <= whWarehouseShelfEntity.Column; j++)
                        {
                            var acode = $"{whWarehouseShelfEntity.Code}-{i}-{j}";
                            result.Add(acode);
                        }
                    }
                    break;
                //指定行生成
                case WhWarehouseLocationTypeEnum.SpecifyRow:
                    for (int j = maxColumn+1; j <=saveDto.Column+ maxColumn; j++)
                    {
                        var scode = $"{whWarehouseShelfEntity.Code}-{saveDto.Row}-{j}";
                        result.Add(scode);
                    }
                    break;
                //自定义生成
                case WhWarehouseLocationTypeEnum.Customize:
                    //for (int i = 1; i <= saveDto.Row; i++)
                    //{
                    //    for (int j = 1; j <= saveDto.Column; j++)
                    //    {
                    //        var ccode = $"{whWarehouseShelfEntity.Code}-{i}-{j}";
                    //        result.Add(ccode);
                    //    }
                    //}
                    //var ccode = $"{whWarehouseShelfEntity.Code}-{saveDto.Row}-{saveDto.Column}";
                    if (!string.IsNullOrWhiteSpace(saveDto.Code))
                    {
                        result.Add(saveDto.Code);
                    }
                    break;
                default:
                    break;
            }
            return result;
        }

        #endregion
    }
}
