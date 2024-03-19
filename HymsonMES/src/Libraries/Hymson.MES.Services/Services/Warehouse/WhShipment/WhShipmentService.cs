using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.WhShipment;
using Hymson.MES.Core.Domain.WhShipmentBarcode;
using Hymson.MES.Core.Domain.WhShipmentMaterial;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.Query;
using Hymson.MES.Data.Repositories.WhShipment;
using Hymson.MES.Data.Repositories.WhShipment.Query;
using Hymson.MES.Services.Dtos.WhShipment;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Transactions;

namespace Hymson.MES.Services.Services.WhShipment
{
    /// <summary>
    /// 服务（出货单） 
    /// </summary>
    public class WhShipmentService : IWhShipmentService
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
        private readonly AbstractValidator<WhShipmentSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（出货单）
        /// </summary>
        private readonly IWhShipmentRepository _whShipmentRepository;
        //private readonly IWhSupplierRepository _whSupplierRepository;
        private readonly IWhShipmentMaterialRepository _whShipmentMaterialRepository;
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IInteCustomRepository _inteCustomRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="whShipmentRepository"></param>
        /// <param name="whShipmentMaterialRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="inteCustomRepository"></param>
        public WhShipmentService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<WhShipmentSaveDto> validationSaveRules,
            IWhShipmentRepository whShipmentRepository,  IWhShipmentMaterialRepository whShipmentMaterialRepository, IProcMaterialRepository procMaterialRepository, IInteCustomRepository inteCustomRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _whShipmentRepository = whShipmentRepository;
            //_whSupplierRepository = whSupplierRepository;
            _whShipmentMaterialRepository = whShipmentMaterialRepository;
            _procMaterialRepository = procMaterialRepository;
            _inteCustomRepository = inteCustomRepository;
        }




        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task CreateAsync(WhShipmentSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<WhShipmentEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            //entity.SiteId = _currentSite.SiteId ?? 0;

            var isEntiy = await _whShipmentRepository.GetEntitiesAsync(new WhShipmentQuery { ShipmentNum = entity.ShipmentNum });
            if (isEntiy.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES19228)).WithData("ShipmentNum", entity.ShipmentNum);

            var details = new List<WhShipmentMaterialEntity>();
            var barcods = new List<WhShipmentBarcodeEntity>();
            if (saveDto.Details != null)
            {
                foreach (var detail in saveDto.Details)
                {
                    long detailid = IdGenProvider.Instance.CreateId();
                    if (detail.Barcods != null)
                    {
                        foreach (var barcod in detail.Barcods)
                        {
                            barcods.Add(new WhShipmentBarcodeEntity()
                            {
                                Id = IdGenProvider.Instance.CreateId(),
                                ShipmentDetailId = detailid,
                                BarCode = barcod.BarCode,
                                Remark = barcod.Remark,
                                SiteId = entity.SiteId

                            });
                        }
                    }
                    details.Add(new WhShipmentMaterialEntity()
                    {
                        Id = detailid,
                        ShipmentId = entity.Id,
                        MaterialId = detail.MaterialId,
                        Qty = detail.Qty,
                        Remark = detail.Remark,
                        CreatedBy = updatedBy,
                        CreatedOn = updatedOn,
                        UpdatedBy = updatedBy,
                        UpdatedOn = updatedOn,
                        SiteId = entity.SiteId
                    });
                }

            }


            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                try { 
                await _whShipmentRepository.InsertAsync(entity);
                //先删除 DETAIL
                //await _whShipmentRepository.DeletesDetailByIdAsync(new long[] { entity.Id });
                //先删除 BARCORDS
                //await _whShipmentRepository.DeletesBarcodeByDetailIdAsync(new long[] { entity.Id });

                if (details.Any())
                    await _whShipmentRepository.InsertRangeAsync(details);

                if (barcods.Any())
                    await _whShipmentRepository.InsertRangeAsync(barcods);
                }
                catch (Exception ex) { }
                ts.Complete();
            }

        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(WhShipmentSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<WhShipmentEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            return await _whShipmentRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _whShipmentRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            return await _whShipmentRepository.DeletesAsync(new DeleteCommand
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
        public async Task<WhShipmentDto?> QueryByIdAsync(long id)
        {
            var whShipmentEntity = await _whShipmentRepository.GetByIdAsync(id);
            if (whShipmentEntity == null) return null;

            return whShipmentEntity.ToModel<WhShipmentDto>();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<WhShipmentDto>> GetPagedListAsync(WhShipmentPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<WhShipmentPagedQuery>();
            if (!pagedQuery.SiteId.HasValue)
            {
                pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            }
            var pagedInfo = await _whShipmentRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<WhShipmentDto>());
            return new PagedInfo<WhShipmentDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }


        /// <summary>
        /// 根据条件查询出货单详情
        /// </summary>
        /// <param name="whShipmentQueryDto"></param>
        /// <returns></returns>
        public async Task<IEnumerable<WhShipmentSupplierMaterialViewDto>> QueryShipmentSupplierMaterialAsync(WhShipmentQueryDto whShipmentQueryDto)
        {
            //获取出货单
            var whShipmentEntity = await _whShipmentRepository.GetByIdAsync(whShipmentQueryDto.Id.GetValueOrDefault());
            if (whShipmentEntity==null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17800));
            }

            //获取供应商
            var inteCustomEntity = await _inteCustomRepository.GetByIdAsync(whShipmentEntity.CustomerId);

            //获取出货详情
            var whShipmentDetailEntities = await _whShipmentMaterialRepository.GetEntitiesAsync(new WhShipmentMaterialQuery { ShipmentId = whShipmentQueryDto.Id.GetValueOrDefault(),SiteId=_currentSite.SiteId??0 });
            if (whShipmentDetailEntities == null || !whShipmentDetailEntities.Any()) {
                throw new CustomerValidationException(nameof(ErrorCode.MES17801));
            }

            //获取物料
            var materialIds = whShipmentDetailEntities.Select(a => a.MaterialId).Distinct();
            var procMaterialEntities =await _procMaterialRepository.GetByIdsAsync(materialIds);
            if (procMaterialEntities == null || !procMaterialEntities.Any()) {
                throw new CustomerValidationException(nameof(ErrorCode.MES17802));
            }

            var result = new List<WhShipmentSupplierMaterialViewDto>();
            foreach (var item in whShipmentDetailEntities) {
                var model = new WhShipmentSupplierMaterialViewDto();

                var procMaterialEntity = procMaterialEntities.Where(a => a.Id == item.MaterialId).FirstOrDefault();
                if (procMaterialEntity == null) {
                    throw new CustomerValidationException(nameof(ErrorCode.MES17802));
                }

                model.MaterialCode= procMaterialEntity.MaterialCode;
                model.MaterialName= procMaterialEntity.MaterialName;
                model.Qty = item.Qty;
                model.Version = procMaterialEntity.Version;
                model.CustomCode = inteCustomEntity?.Code;
                model.Id = item.Id;
                result.Add(model);
            }

            return result;
        }
    }
}
