using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.MaskCode;
using Hymson.MES.Services.Dtos.Process;
using Hymson.Snowflake;
using Hymson.Utils;
using System.Transactions;

namespace Hymson.MES.Services.Services.Process
{
    /// <summary>
    /// 物料维护 服务
    /// </summary>
    public class ProcMaterialService : IProcMaterialService
    {
        /// <summary>
        /// 物料维护 仓储
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly AbstractValidator<ProcMaterialCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ProcMaterialModifyDto> _validationModifyRules;

        private readonly IProcReplaceMaterialRepository _procReplaceMaterialRepository;
        private readonly IProcMaterialSupplierRelationRepository _procMaterialSupplierRelationRepository;
        private readonly IProcMaskCodeRepository _procMaskCodeRepository;

        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="validationCreateRules"></param>
        /// <param name="validationModifyRules"></param>
        /// <param name="procReplaceMaterialRepository"></param>
        /// <param name="currentSite"></param>
        /// <param name="procMaterialSupplierRelationRepository"></param>
        /// <param name="procMaskCodeRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        public ProcMaterialService(ICurrentUser currentUser, IProcMaterialRepository procMaterialRepository,
            AbstractValidator<ProcMaterialCreateDto> validationCreateRules,
            AbstractValidator<ProcMaterialModifyDto> validationModifyRules,
            IProcReplaceMaterialRepository procReplaceMaterialRepository,
            ICurrentSite currentSite,
            IProcMaterialSupplierRelationRepository procMaterialSupplierRelationRepository,
            IProcMaskCodeRepository procMaskCodeRepository,
            IPlanWorkOrderRepository planWorkOrderRepository)
        {
            _currentUser = currentUser;
            _procMaterialRepository = procMaterialRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;

            _procReplaceMaterialRepository = procReplaceMaterialRepository;
            _currentSite = currentSite;
            _procMaterialSupplierRelationRepository = procMaterialSupplierRelationRepository;
            _procMaskCodeRepository = procMaskCodeRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="procMaterialCreateDto"></param>
        /// <returns></returns>
        public async Task CreateProcMaterialAsync(ProcMaterialCreateDto procMaterialCreateDto)
        {
            #region 参数校验
            //// 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                //responseDto.Msg = "站点码获取失败，请重新登录！";
                //return responseDto;
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }
            procMaterialCreateDto.MaterialCode = procMaterialCreateDto.MaterialCode.ToTrimSpace().ToUpperInvariant();
            procMaterialCreateDto.MaterialName = procMaterialCreateDto.MaterialName.Trim();
            procMaterialCreateDto.Version = procMaterialCreateDto.Version.Trim();
            procMaterialCreateDto.Remark = procMaterialCreateDto?.Remark ?? "".Trim();
            procMaterialCreateDto.Unit = procMaterialCreateDto?.Unit ?? "".Trim();
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(procMaterialCreateDto);

            procMaterialCreateDto.MaterialCode = procMaterialCreateDto.MaterialCode.ToUpper();
            procMaterialCreateDto.Origin = MaterialOriginEnum.ManualEntry; // ERP/EIS（sys_source_type）

            //判断编号是否已存在
            var haveEntity = await _procMaterialRepository.GetByCodeAsync(new ProcMaterialQuery()
            {
                SiteId = _currentSite.SiteId,
                MaterialCode = procMaterialCreateDto.MaterialCode,
                Version = procMaterialCreateDto.Version
            });
            //存在则抛异常
            if (haveEntity != null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10201)).WithData("materialCode", procMaterialCreateDto.MaterialCode).WithData("version", procMaterialCreateDto.Version);
            }

            #endregion

            #region 组装数据
            //DTO转换实体
            var procMaterialEntity = procMaterialCreateDto.ToEntity<ProcMaterialEntity>();
            procMaterialEntity.Id = IdGenProvider.Instance.CreateId();
            procMaterialEntity.CreatedBy = _currentUser.UserName;
            procMaterialEntity.UpdatedBy = _currentUser.UserName;
            procMaterialEntity.CreatedOn = HymsonClock.Now();
            procMaterialEntity.UpdatedOn = HymsonClock.Now();
            procMaterialEntity.SiteId = _currentSite.SiteId ?? 123456;

            //替代品数据
            List<ProcReplaceMaterialEntity> addProcReplaceList = new List<ProcReplaceMaterialEntity>();
            if (procMaterialCreateDto.DynamicList != null && procMaterialCreateDto.DynamicList.Count > 0)
            {
                foreach (var item in procMaterialCreateDto.DynamicList)
                {
                    ProcReplaceMaterialEntity procReplaceMaterial = item.ToEntity<ProcReplaceMaterialEntity>();
                    procReplaceMaterial.Id = IdGenProvider.Instance.CreateId();
                    procReplaceMaterial.IsUse = item.IsEnabled;
                    procReplaceMaterial.ReplaceMaterialId = item.Id;
                    procReplaceMaterial.MaterialId = procMaterialEntity.Id;
                    procReplaceMaterial.CreatedBy = _currentUser.UserName;
                    procReplaceMaterial.CreatedOn = HymsonClock.Now();
                    procReplaceMaterial.SiteId = _currentSite.SiteId ?? 123456;
                    addProcReplaceList.Add(procReplaceMaterial);
                }
            }

            //供应商
            List<ProcMaterialSupplierRelationEntity> addMaterialSupplierList = new List<ProcMaterialSupplierRelationEntity>();
            if (procMaterialCreateDto.MaterialSupplierList != null && procMaterialCreateDto.MaterialSupplierList.Count > 0)
            {
                foreach (var item in procMaterialCreateDto.MaterialSupplierList)
                {
                    ProcMaterialSupplierRelationEntity materialSupplier = new ProcMaterialSupplierRelationEntity();
                    materialSupplier.Id = IdGenProvider.Instance.CreateId();
                    materialSupplier.SupplierId = item.SupplierId;
                    materialSupplier.MaterialId = procMaterialEntity.Id;

                    materialSupplier.CreatedBy = _currentUser.UserName;
                    materialSupplier.CreatedOn = HymsonClock.Now();
                    addMaterialSupplierList.Add(materialSupplier);
                }
            }

            #endregion

            #region 保存到数据库
            using (TransactionScope ts = new TransactionScope())
            {
                var response = 0;

                if (procMaterialEntity.IsDefaultVersion)
                {
                    // 先将同编码的其他物料设置为非当前版本
                    await _procMaterialRepository.UpdateSameMaterialCodeToNoVersionAsync(new ProcMaterialEntity()
                    {
                        MaterialCode = procMaterialEntity.MaterialCode,
                    });
                }

                //入库
                response = await _procMaterialRepository.InsertAsync(procMaterialEntity);

                if (response == 0)
                {
                    throw new BusinessException(nameof(ErrorCode.MES10202));
                }

                if (procMaterialCreateDto.DynamicList != null && procMaterialCreateDto.DynamicList.Count > 0)
                {
                    response = await _procReplaceMaterialRepository.InsertsAsync(addProcReplaceList);

                    if (response != procMaterialCreateDto.DynamicList.Count)
                    {
                        throw new BusinessException(nameof(ErrorCode.MES10202));
                    }
                }

                if (procMaterialCreateDto.MaterialSupplierList != null && procMaterialCreateDto.MaterialSupplierList.Count > 0)
                {
                    response = await _procMaterialSupplierRelationRepository.InsertsAsync(addMaterialSupplierList);

                    if (response != procMaterialCreateDto.MaterialSupplierList.Count)
                    {
                        throw new BusinessException(nameof(ErrorCode.MES10202));
                    }
                }

                ts.Complete();
            }

            #endregion

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteProcMaterialAsync(long id)
        {
            await _procMaterialRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesProcMaterialAsync(long[] idsArr)
        {
            if (idsArr.Length < 1) throw new ValidationException(nameof(ErrorCode.MES10213));

            //var statusArr = new int[] { 2, 3 }; //可下达和保留 时无法删除
            //判断这些ID 对应的物料是否在 可下达和保留中  （1:新建;2:可下达;3:保留;4:废除）
            var entitys = await _procMaterialRepository.GetByIdsAsync(idsArr);
            //if (entitys.Any(a => a.Status == SysDataStatusEnum.Enable
            //|| a.Status == SysDataStatusEnum.Retain) == true) throw new BusinessException(nameof(ErrorCode.MES10212));
            if (entitys != null && entitys.Any(a => a.Status != SysDataStatusEnum.Build))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10106));
            }

            //判断这些物料在 生产工单（已下达、生产中）中是否被使用，被使用则无法删除
            var statusList = new List<PlanWorkOrderStatusEnum>();
            statusList.Add(PlanWorkOrderStatusEnum.SendDown);
            statusList.Add(PlanWorkOrderStatusEnum.InProduction);
            var useMaterilWorkOrders = await _planWorkOrderRepository.GetEqualPlanWorkOrderEntitiesAsync(new PlanWorkOrderQuery()
            {
                SiteId = _currentSite.SiteId ?? 123456,
                ProductIds = idsArr.ToList(),
                StatusList = statusList
            });
            if (useMaterilWorkOrders != null && useMaterilWorkOrders.Any())
            {
                throw new BusinessException(nameof(ErrorCode.MES10225));
            }

            return await _procMaterialRepository.DeletesAsync(new DeleteCommand
            {
                Ids = idsArr,
                DeleteOn = HymsonClock.Now(),
                UserId = _currentUser.UserName
            });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="procMaterialPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcMaterialDto>> GetPageListAsync(ProcMaterialPagedQueryDto procMaterialPagedQueryDto)
        {
            var procMaterialPagedQuery = procMaterialPagedQueryDto.ToQuery<ProcMaterialPagedQuery>();
            procMaterialPagedQuery.SiteId = _currentSite.SiteId ?? 123456;
            var pagedInfo = await _procMaterialRepository.GetPagedInfoAsync(procMaterialPagedQuery);

            //实体到DTO转换 装载数据
            List<ProcMaterialDto> procMaterialDtos = PrepareProcMaterialDtos(pagedInfo);
            return new PagedInfo<ProcMaterialDto>(procMaterialDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ProcMaterialDto> PrepareProcMaterialDtos(PagedInfo<ProcMaterialEntity> pagedInfo)
        {
            var procMaterialDtos = new List<ProcMaterialDto>();
            foreach (var procMaterialEntity in pagedInfo.Data)
            {
                var procMaterialDto = procMaterialEntity.ToModel<ProcMaterialDto>();
                procMaterialDtos.Add(procMaterialDto);
            }

            return procMaterialDtos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="procMaterialPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcMaterialDto>> GetPageListForGroupAsync(ProcMaterialPagedQueryDto procMaterialPagedQueryDto)
        {
            var procMaterialPagedQuery = procMaterialPagedQueryDto.ToQuery<ProcMaterialPagedQuery>();
            procMaterialPagedQuery.SiteId = _currentSite.SiteId ?? 123456;
            var pagedInfo = await _procMaterialRepository.GetPagedInfoForGroupAsync(procMaterialPagedQuery);

            //实体到DTO转换 装载数据
            List<ProcMaterialDto> procMaterialDtos = PrepareProcMaterialDtos(pagedInfo);
            return new PagedInfo<ProcMaterialDto>(procMaterialDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procMaterialModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyProcMaterialAsync(ProcMaterialModifyDto procMaterialModifyDto)
        {
            procMaterialModifyDto.MaterialName = procMaterialModifyDto.MaterialName.Trim();
            procMaterialModifyDto.Version = procMaterialModifyDto.Version.Trim();
            procMaterialModifyDto.Remark = procMaterialModifyDto?.Remark ?? "".Trim();
            procMaterialModifyDto.Unit = procMaterialModifyDto?.Unit ?? "".Trim();

            //DTO转换实体
            var procMaterialEntity = procMaterialModifyDto.ToEntity<ProcMaterialEntity>();
            procMaterialEntity.UpdatedBy = _currentUser.UserName;
            procMaterialEntity.UpdatedOn = HymsonClock.Now();

            #region 校验
            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(procMaterialModifyDto);

            var modelOrigin = await _procMaterialRepository.GetByIdAsync(procMaterialModifyDto.Id, _currentSite.SiteId ?? 123456);
            if (modelOrigin == null)
            {
                throw new NotFoundException(nameof(ErrorCode.MES10204));
            }

            if (modelOrigin.Status != SysDataStatusEnum.Build && procMaterialModifyDto.Status == SysDataStatusEnum.Build)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10108));
            }

            if (procMaterialModifyDto.Origin != modelOrigin.Origin)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10205));
            }

            // 判断替代品是否包含当前物料
            var replaceMaterialList = ConvertProcReplaceMaterialList(procMaterialModifyDto.DynamicList, procMaterialEntity);
            if (replaceMaterialList.Any(a => a.ReplaceMaterialId == procMaterialEntity.Id) == true)
            {
                throw new BusinessException(nameof(ErrorCode.MES10206));
            }

            // 判断编号是否已存在
            var exists = await _procMaterialRepository.GetByCodeAsync(new ProcMaterialQuery()
            {
                SiteId = _currentSite.SiteId,
                MaterialCode = procMaterialEntity.MaterialCode,
                Version = procMaterialEntity.Version
            });
            if (exists != null && exists.Id != procMaterialEntity.Id)
            {
                throw new BusinessException(nameof(ErrorCode.MES10201)).WithData("materialCode", procMaterialEntity.MaterialCode).WithData("version", procMaterialEntity.Version);
            }

            #endregion

            #region 组装数据
            //替代品数据
            List<ProcReplaceMaterialEntity> addProcReplaceList = new List<ProcReplaceMaterialEntity>();
            if (procMaterialModifyDto.DynamicList != null && procMaterialModifyDto.DynamicList.Count > 0)
            {
                foreach (var item in procMaterialModifyDto.DynamicList)
                {
                    ProcReplaceMaterialEntity procReplaceMaterial = item.ToEntity<ProcReplaceMaterialEntity>();
                    procReplaceMaterial.Id = IdGenProvider.Instance.CreateId();
                    procReplaceMaterial.IsUse = item.IsEnabled;
                    procReplaceMaterial.ReplaceMaterialId = item.Id;
                    procReplaceMaterial.MaterialId = procMaterialEntity.Id;
                    procReplaceMaterial.CreatedBy = _currentUser.UserName;
                    procReplaceMaterial.CreatedOn = HymsonClock.Now();
                    procReplaceMaterial.SiteId = _currentSite.SiteId ?? 123456;
                    addProcReplaceList.Add(procReplaceMaterial);
                }
            }

            //供应商
            List<ProcMaterialSupplierRelationEntity> addMaterialSupplierList = new List<ProcMaterialSupplierRelationEntity>();
            if (procMaterialModifyDto.MaterialSupplierList != null && procMaterialModifyDto.MaterialSupplierList.Count > 0)
            {
                foreach (var item in procMaterialModifyDto.MaterialSupplierList)
                {
                    ProcMaterialSupplierRelationEntity materialSupplier = new ProcMaterialSupplierRelationEntity();
                    materialSupplier.Id = IdGenProvider.Instance.CreateId();
                    materialSupplier.SupplierId = item.SupplierId;
                    materialSupplier.MaterialId = procMaterialEntity.Id;

                    materialSupplier.CreatedBy = _currentUser.UserName;
                    materialSupplier.CreatedOn = HymsonClock.Now();
                    addMaterialSupplierList.Add(materialSupplier);
                }
            }
            #endregion 

            #region 操作数据库
            using (TransactionScope ts = new TransactionScope())
            {
                int response = 0;
                if (procMaterialEntity.IsDefaultVersion)
                {
                    // 先将同编码的其他物料设置为非当前版本
                    await _procMaterialRepository.UpdateSameMaterialCodeToNoVersionAsync(new ProcMaterialEntity()
                    {
                        MaterialCode = procMaterialEntity.MaterialCode,
                    });
                }

                response = await _procMaterialRepository.UpdateAsync(new ProcMaterialEntity()
                {
                    Id = procMaterialEntity.Id,
                    GroupId = procMaterialEntity.GroupId,
                    MaterialName = procMaterialEntity.MaterialName,
                    Status = procMaterialEntity.Status,
                    Origin = procMaterialEntity.Origin,
                    Version = procMaterialEntity.Version,
                    Remark = procMaterialEntity.Remark,
                    BuyType = procMaterialEntity.BuyType,
                    ProcessRouteId = procMaterialEntity.ProcessRouteId,
                    BomId = procMaterialEntity.BomId,
                    Batch = procMaterialEntity.Batch,
                    PackageNum = procMaterialEntity.PackageNum,
                    Unit = procMaterialEntity.Unit,
                    SerialNumber = procMaterialEntity.SerialNumber,
                    BaseTime = procMaterialEntity.BaseTime,
                    ConsumptionTolerance = procMaterialEntity.ConsumptionTolerance,
                    IsDefaultVersion = procMaterialEntity.IsDefaultVersion,
                    MaskCodeId = procMaterialEntity.MaskCodeId,
                    UpdatedBy = procMaterialEntity.UpdatedBy,
                    UpdatedOn = procMaterialEntity.UpdatedOn,
                    ConsumeRatio = procMaterialEntity.ConsumeRatio,
                });

                if (response == 0)
                {
                    throw new BusinessException(nameof(ErrorCode.MES10208));
                }

                //先真删除替换物料
                response = await _procReplaceMaterialRepository.DeleteTrueByMaterialIdsAsync(new long[] { procMaterialEntity.Id });

                //替代组设置数据
                if (addProcReplaceList != null && addProcReplaceList.Count > 0)
                {
                    if ((await _procReplaceMaterialRepository.InsertsAsync(addProcReplaceList)) <= 0)
                    {
                        throw new BusinessException(nameof(ErrorCode.MES10209));
                    }
                }


                //先真删除关联供应商
                response = await _procMaterialSupplierRelationRepository.DeleteTrueByMaterialIdsAsync(new long[] { procMaterialEntity.Id });

                if (procMaterialModifyDto.MaterialSupplierList != null && procMaterialModifyDto.MaterialSupplierList.Count > 0)
                {
                    response = await _procMaterialSupplierRelationRepository.InsertsAsync(addMaterialSupplierList);

                    if (response != procMaterialModifyDto.MaterialSupplierList.Count)
                    {
                        throw new BusinessException(nameof(ErrorCode.MES10222));
                    }
                }

                ts.Complete();
            }


            #endregion

        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcMaterialViewDto> QueryProcMaterialByIdAsync(long id)
        {
            var procMaterialView = await _procMaterialRepository.GetByIdAsync(id, _currentSite.SiteId ?? 123456);
            if (procMaterialView != null)
            {
                var procMaterialViewDto = procMaterialView.ToModel<ProcMaterialViewDto>();

                if (procMaterialViewDto.MaskCodeId.HasValue)
                {
                    var maskCodeEntity = await _procMaskCodeRepository.GetByIdAsync(procMaterialViewDto.MaskCodeId.Value);
                    procMaterialViewDto.ValidationMaskGroup = maskCodeEntity?.Code ?? "";
                }

                //查询替代物料
                var replaceMaterialViews = await _procReplaceMaterialRepository.GetProcReplaceMaterialViewsAsync(new ProcReplaceMaterialQuery
                {
                    SiteId = procMaterialView.SiteId,
                    MaterialId = procMaterialView.Id
                });
                foreach (var replaceMaterialView in replaceMaterialViews)
                {
                    procMaterialViewDto.ReplaceMaterialList.Add(replaceMaterialView.ToModel<ProcMaterialReplaceViewDto>());
                }

                return procMaterialViewDto;
            }
            return null;
        }

        /// <summary>
        /// 根据物料ID查询对应的关联供应商
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<ProcMaterialSupplierViewDto>> QueryProcMaterialSupplierByMaterialIdAsync(long id)
        {
            List<ProcMaterialSupplierViewDto> list = new List<ProcMaterialSupplierViewDto>();

            //查询关联的供应商
            var materialSuppliers = await _procMaterialSupplierRelationRepository.GetByMaterialIdAsync(id);
            foreach (var item in materialSuppliers)
            {
                list.Add(item.ToModel<ProcMaterialSupplierViewDto>());
            }

            return list;
        }

        #region 业务扩展方法
        /// <summary>
        /// 转换集合（物料替代品）
        /// </summary>
        /// <param name="dynamicList"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<ProcReplaceMaterialEntity> ConvertProcReplaceMaterialList(IEnumerable<ProcMaterialReplaceDto> dynamicList, ProcMaterialEntity model)
        {
            if (dynamicList == null || dynamicList.Any() == false) return new List<ProcReplaceMaterialEntity> { };
            return dynamicList.Select(s => new ProcReplaceMaterialEntity
            {
                MaterialId = model.Id,
                ReplaceMaterialId = s.Id,
                IsUse = s.IsEnabled,
                CreatedBy = model.UpdatedBy,
                CreatedOn = model.UpdatedOn ?? HymsonClock.Now()
            }).ToList();
        }
        #endregion
    }
}
