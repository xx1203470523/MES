using FluentValidation;
using FluentValidation.Results;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Excel.Abstractions;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.MaskCode;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Common;
using Hymson.Minio;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using OfficeOpenXml.Attributes;
using System.Reflection;
using System.Text;
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
        private readonly AbstractValidator<ProcMaterialImportDto> _validationImportRules;

        private readonly IProcReplaceMaterialRepository _procReplaceMaterialRepository;
        private readonly IProcMaterialSupplierRelationRepository _procMaterialSupplierRelationRepository;
        private readonly IProcMaskCodeRepository _procMaskCodeRepository;
        private readonly IProcProcessRouteRepository _procProcessRouteRepository;
        private readonly IProcBomRepository _procBomRepository;

        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        private readonly ILocalizationService _localizationService;

        private readonly IExcelService _excelService;
        private readonly IMinioService _minioService;

        private readonly IProcMaterialGroupRepository _procMaterialGroupRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="validationCreateRules"></param>
        /// <param name="validationModifyRules"></param>
        /// <param name="validationImportRules"></param>
        /// <param name="procReplaceMaterialRepository"></param>
        /// <param name="currentSite"></param>
        /// <param name="procMaterialSupplierRelationRepository"></param>
        /// <param name="procMaskCodeRepository"></param>
        /// <param name="procProcessRouteRepository"></param>
        /// <param name="procBomRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="localizationService"></param>
        /// <param name="excelService"></param>
        /// <param name="minioService"></param>
        /// <param name="procMaterialGroupRepository"></param>
        public ProcMaterialService(ICurrentUser currentUser, IProcMaterialRepository procMaterialRepository,
            AbstractValidator<ProcMaterialCreateDto> validationCreateRules,
            AbstractValidator<ProcMaterialModifyDto> validationModifyRules,
            AbstractValidator<ProcMaterialImportDto> validationImportRules,
            IProcReplaceMaterialRepository procReplaceMaterialRepository,
            ICurrentSite currentSite,
            IProcMaterialSupplierRelationRepository procMaterialSupplierRelationRepository,
            IProcMaskCodeRepository procMaskCodeRepository,
            IProcProcessRouteRepository procProcessRouteRepository,
            IProcBomRepository procBomRepository,
            IPlanWorkOrderRepository planWorkOrderRepository, ILocalizationService localizationService,
            IExcelService excelService,
            IMinioService minioService, IProcMaterialGroupRepository procMaterialGroupRepository)
        {
            _currentUser = currentUser;
            _procMaterialRepository = procMaterialRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _validationImportRules = validationImportRules;
            _procProcessRouteRepository = procProcessRouteRepository;
            _procBomRepository = procBomRepository;
            _procReplaceMaterialRepository = procReplaceMaterialRepository;
            _currentSite = currentSite;
            _procMaterialSupplierRelationRepository = procMaterialSupplierRelationRepository;
            _procMaskCodeRepository = procMaskCodeRepository;

            _planWorkOrderRepository = planWorkOrderRepository;

            _localizationService = localizationService;

            _excelService = excelService;
            _minioService = minioService;

            _procMaterialGroupRepository = procMaterialGroupRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<long> CreateProcMaterialAsync(ProcMaterialCreateDto saveDto)
        {
            if (saveDto == null) throw new CustomerValidationException(nameof(ErrorCode.MES10100));

            #region 参数校验
            saveDto.MaterialCode = saveDto.MaterialCode.ToTrimSpace().ToUpperInvariant();
            saveDto.MaterialName = saveDto.MaterialName.Trim();
            saveDto.Version = saveDto.Version.Trim();
            saveDto.Remark = saveDto?.Remark ?? "".Trim();
            saveDto!.Unit = saveDto?.Unit ?? "".Trim();

            // 验证DTO
            await _validationCreateRules!.ValidateAndThrowAsync(saveDto);

            saveDto!.MaterialCode = saveDto.MaterialCode.ToUpper();
            saveDto.Origin = MaterialOriginEnum.ManualEntry; // ERP/EIS（sys_source_type）

            // 判断编号是否已存在
            var haveEntity = await _procMaterialRepository.GetByCodeAsync(new ProcMaterialQuery()
            {
                SiteId = _currentSite.SiteId,
                MaterialCode = saveDto.MaterialCode,
                Version = saveDto.Version
            });
            // 存在则抛异常
            if (haveEntity != null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10201)).WithData("materialCode", saveDto.MaterialCode).WithData("version", saveDto.Version);
            }

            #endregion

            #region 组装数据
            // DTO转换实体
            var procMaterialEntity = saveDto.ToEntity<ProcMaterialEntity>();
            procMaterialEntity.Id = IdGenProvider.Instance.CreateId();
            procMaterialEntity.CreatedBy = _currentUser.UserName;
            procMaterialEntity.UpdatedBy = _currentUser.UserName;
            procMaterialEntity.CreatedOn = HymsonClock.Now();
            procMaterialEntity.UpdatedOn = HymsonClock.Now();
            procMaterialEntity.SiteId = _currentSite.SiteId ?? 0;

            procMaterialEntity.Status = SysDataStatusEnum.Build;

            //替代品数据
            List<ProcReplaceMaterialEntity> addProcReplaceList = new List<ProcReplaceMaterialEntity>();
            if (saveDto.DynamicList != null && saveDto.DynamicList.Count > 0)
            {
                foreach (var item in saveDto.DynamicList)
                {
                    ProcReplaceMaterialEntity procReplaceMaterial = item.ToEntity<ProcReplaceMaterialEntity>();
                    procReplaceMaterial.Id = IdGenProvider.Instance.CreateId();
                    procReplaceMaterial.IsUse = item.IsEnabled;
                    procReplaceMaterial.ReplaceMaterialId = item.Id;
                    procReplaceMaterial.MaterialId = procMaterialEntity.Id;
                    procReplaceMaterial.CreatedBy = _currentUser.UserName;
                    procReplaceMaterial.CreatedOn = HymsonClock.Now();
                    procReplaceMaterial.SiteId = _currentSite.SiteId ?? 0;
                    addProcReplaceList.Add(procReplaceMaterial);
                }
            }

            //供应商
            List<ProcMaterialSupplierRelationEntity> addMaterialSupplierList = new List<ProcMaterialSupplierRelationEntity>();
            if (saveDto.MaterialSupplierList != null && saveDto.MaterialSupplierList.Count > 0)
            {
                foreach (var item in saveDto.MaterialSupplierList)
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
            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
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

                // 保存
                response = await _procMaterialRepository.InsertAsync(procMaterialEntity);
                if (response == 0)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10202));
                }

                if (saveDto.DynamicList != null && saveDto.DynamicList.Count > 0)
                {
                    response = await _procReplaceMaterialRepository.InsertsAsync(addProcReplaceList);
                    if (response != saveDto.DynamicList.Count)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES10202));
                    }
                }

                if (saveDto.MaterialSupplierList != null && saveDto.MaterialSupplierList.Count > 0)
                {
                    response = await _procMaterialSupplierRelationRepository.InsertsAsync(addMaterialSupplierList);
                    if (response != saveDto.MaterialSupplierList.Count)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES10202));
                    }
                }

                ts.Complete();
            }

            return procMaterialEntity.Id;

            #endregion

        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task ModifyProcMaterialAsync(ProcMaterialModifyDto saveDto)
        {
            saveDto.MaterialName = saveDto.MaterialName.Trim();
            saveDto.Version = saveDto.Version.Trim();
            saveDto.Remark = saveDto?.Remark ?? "".Trim();
            saveDto!.Unit = saveDto?.Unit ?? "".Trim();
            saveDto!.ValidTime = saveDto?.ValidTime ?? null;

            // DTO转换实体
            var procMaterialEntity = saveDto!.ToEntity<ProcMaterialEntity>();
            procMaterialEntity.UpdatedBy = _currentUser.UserName;
            procMaterialEntity.UpdatedOn = HymsonClock.Now();

            #region 校验
            // 验证DTO
            await _validationModifyRules!.ValidateAndThrowAsync(saveDto);

            var modelOrigin = await _procMaterialRepository.GetByIdAsync(saveDto!.Id, _currentSite.SiteId ?? 0);
            if (modelOrigin == null)
            {
                throw new NotFoundException(nameof(ErrorCode.MES10204));
            }

            // 验证某些是不能编辑的
            var canEditStatusEnum = new SysDataStatusEnum[] { SysDataStatusEnum.Build, SysDataStatusEnum.Retain };
            if (!canEditStatusEnum.Any(x => x == modelOrigin.Status))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10129));
            }

            if (saveDto.Origin != modelOrigin.Origin)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10205));
            }

            // 判断替代品是否包含当前物料
            var replaceMaterialList = ConvertProcReplaceMaterialList(saveDto.DynamicList!, procMaterialEntity);
            if (replaceMaterialList.Any(a => a.ReplaceMaterialId == procMaterialEntity.Id))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10206));
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
                throw new CustomerValidationException(nameof(ErrorCode.MES10201)).WithData("materialCode", procMaterialEntity.MaterialCode).WithData("version", procMaterialEntity.Version!);
            }

            #endregion

            #region 组装数据
            //替代品数据
            List<ProcReplaceMaterialEntity> addProcReplaceList = new List<ProcReplaceMaterialEntity>();
            if (saveDto.DynamicList != null && saveDto.DynamicList.Count > 0)
            {
                foreach (var item in saveDto.DynamicList)
                {
                    ProcReplaceMaterialEntity procReplaceMaterial = item.ToEntity<ProcReplaceMaterialEntity>();
                    procReplaceMaterial.Id = IdGenProvider.Instance.CreateId();
                    procReplaceMaterial.IsUse = item.IsEnabled;
                    procReplaceMaterial.ReplaceMaterialId = item.Id;
                    procReplaceMaterial.MaterialId = procMaterialEntity.Id;
                    procReplaceMaterial.CreatedBy = _currentUser.UserName;
                    procReplaceMaterial.CreatedOn = HymsonClock.Now();
                    procReplaceMaterial.SiteId = _currentSite.SiteId ?? 0;
                    addProcReplaceList.Add(procReplaceMaterial);
                }
            }

            //供应商
            List<ProcMaterialSupplierRelationEntity> addMaterialSupplierList = new List<ProcMaterialSupplierRelationEntity>();
            if (saveDto.MaterialSupplierList != null && saveDto.MaterialSupplierList.Count > 0)
            {
                foreach (var item in saveDto.MaterialSupplierList)
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
            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
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
                    //Status = procMaterialEntity.Status,
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
                    QuantityLimit = procMaterialEntity.QuantityLimit,
                    ProductModel = procMaterialEntity.ProductModel,
                    Specifications = procMaterialEntity.Specifications,
                    MaterialType = procMaterialEntity.MaterialType,
                    ValidTime = procMaterialEntity.ValidTime,
                });

                if (response == 0)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10208));
                }

                //先真删除替换物料
                response = await _procReplaceMaterialRepository.DeleteTrueByMaterialIdsAsync(new long[] { procMaterialEntity.Id });

                //替代组设置数据
                if (addProcReplaceList != null && addProcReplaceList.Count > 0 && (await _procReplaceMaterialRepository.InsertsAsync(addProcReplaceList)) <= 0)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10209));
                }


                //先真删除关联供应商
                response = await _procMaterialSupplierRelationRepository.DeleteTrueByMaterialIdsAsync(new long[] { procMaterialEntity.Id });

                if (saveDto.MaterialSupplierList != null && saveDto.MaterialSupplierList.Count > 0)
                {
                    response = await _procMaterialSupplierRelationRepository.InsertsAsync(addMaterialSupplierList);

                    if (response != saveDto.MaterialSupplierList.Count)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES10222));
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
        public async Task DeletesProcMaterialAsync(long[] idsArr)
        {
            if (idsArr.Length < 1) throw new CustomerValidationException(nameof(ErrorCode.MES10213));

            //var statusArr = new int[] { 2, 3 }; //可下达和保留 时无法删除
            //判断这些ID 对应的物料是否在 可下达和保留中  （1:新建;2:可下达;3:保留;4:废除）
            var entitys = await _procMaterialRepository.GetByIdsAsync(idsArr);

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
                SiteId = _currentSite.SiteId ?? 0,
                ProductIds = idsArr.ToList(),
                StatusList = statusList
            });
            if (useMaterilWorkOrders != null && useMaterilWorkOrders.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10225));
            }

            #region 删除物料的关联供应商关系
            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                await _procMaterialRepository.DeletesAsync(new DeleteCommand
                {
                    Ids = idsArr,
                    DeleteOn = HymsonClock.Now(),
                    UserId = _currentUser.UserName
                });

                await _procMaterialSupplierRelationRepository.DeleteTrueByMaterialIdsAsync(idsArr);

                ts.Complete();
            }
            #endregion
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="procMaterialPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcMaterialDto>> GetPageListAsync(ProcMaterialPagedQueryDto procMaterialPagedQueryDto)
        {
            var procMaterialPagedQuery = procMaterialPagedQueryDto.ToQuery<ProcMaterialPagedQuery>();
            procMaterialPagedQuery.SiteId = _currentSite.SiteId ?? 0;

            var materialGroupEntities = await _procMaterialGroupRepository.GetProcMaterialGroupEntitiesAsync(new ProcMaterialGroupQuery { SiteId = _currentSite.SiteId ?? 0, GroupCode = procMaterialPagedQueryDto.MaterialGroupCode });

            //判断是否需要查询物料组编码 -- 全匹配查询
            if (!string.IsNullOrWhiteSpace(procMaterialPagedQueryDto.MaterialGroupCode))
            {
                //var materialGroup = (await _procMaterialGroupRepository.GetProcMaterialGroupEntitiesAsync(new ProcMaterialGroupQuery() { SiteId = _currentSite.SiteId ?? 0, GroupCode = procMaterialPagedQueryDto.MaterialGroupCode })).FirstOrDefault();
                var materialGroup = materialGroupEntities.FirstOrDefault();
                if (materialGroup == null)
                {
                    return new PagedInfo<ProcMaterialDto>(new List<ProcMaterialDto>(), procMaterialPagedQueryDto.PageIndex, procMaterialPagedQueryDto.PageSize, 0);
                }
                procMaterialPagedQuery.GroupId = materialGroup?.Id;
            }

            var pagedInfo = await _procMaterialRepository.GetPagedInfoAsync(procMaterialPagedQuery);

            //实体到DTO转换 装载数据
            List<ProcMaterialDto> procMaterialDtos = PrepareProcMaterialDtos(pagedInfo);

            //获取物料组信息
            foreach (var item in procMaterialDtos)
            {
                var materialGroupEntity = materialGroupEntities.Where(a => a.Id == item.GroupId).FirstOrDefault();
                item.MaterialGroupCode = materialGroupEntity?.GroupCode ?? "";
            }

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
            procMaterialPagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _procMaterialRepository.GetPagedInfoForGroupAsync(procMaterialPagedQuery);

            //实体到DTO转换 装载数据
            List<ProcMaterialDto> procMaterialDtos = PrepareProcMaterialDtos(pagedInfo);
            return new PagedInfo<ProcMaterialDto>(procMaterialDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcMaterialViewDto> QueryProcMaterialByIdAsync(long id)
        {
            // 建议改为读取单对象实体，再瓶装的方式，而不是连表查询
            var procMaterialView = await _procMaterialRepository.GetByIdAsync(id, _currentSite.SiteId ?? 0);
            if (procMaterialView != null)
            {
                var procMaterialViewDto = procMaterialView.ToModel<ProcMaterialViewDto>();

                if (procMaterialViewDto.MaskCodeId.HasValue)
                {
                    var maskCodeEntity = await _procMaskCodeRepository.GetByIdAsync(procMaterialViewDto.MaskCodeId.Value);
                    procMaterialViewDto.ValidationMaskGroup = maskCodeEntity?.Code ?? "";
                }

                // 查询替代物料
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
            return new ProcMaterialViewDto();
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

        /// <summary>
        /// 根据物料ID查询对应的关联供应商
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SelectOptionDto>> QuerySuppliersAsync(long id)
        {
            // 查询关联的供应商
            var materialSupplierViews = await _procMaterialSupplierRelationRepository.GetByMaterialIdAsync(id);
            return materialSupplierViews.Select(s => new SelectOptionDto
            {
                Key = $"{s.SupplierId}",
                Label = $"{s.Code} - {s.Name}",
                Value = $"{s.SupplierId}"
            });
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
            if (dynamicList == null || !dynamicList.Any()) return new List<ProcReplaceMaterialEntity> { };
            return dynamicList.Select(s => new ProcReplaceMaterialEntity
            {
                MaterialId = model.Id,
                ReplaceMaterialId = s.Id,
                IsUse = s.IsEnabled,
                CreatedBy = model.UpdatedBy!,
                CreatedOn = model.UpdatedOn ?? HymsonClock.Now()
            }).ToList();
        }
        #endregion

        #region 状态变更
        /// <summary>
        /// 状态变更
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task UpdateStatusAsync(ChangeStatusDto param)
        {
            #region 参数校验
            if (param.Id == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10125));
            }
            if (!Enum.IsDefined(typeof(SysDataStatusEnum), param.Status))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10126));
            }
            if (param.Status == SysDataStatusEnum.Build)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10128));
            }

            #endregion

            var changeStatusCommand = new ChangeStatusCommand()
            {
                Id = param.Id,
                Status = param.Status,

                UpdatedBy = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now()
            };

            #region 校验数据
            var material = await _procMaterialRepository.GetByIdAsync(changeStatusCommand.Id);
            if (material == null || material.IsDeleted != 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10204));
            }
            if (material.Status == changeStatusCommand.Status)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10127)).WithData("status", _localizationService.GetResource($"{typeof(SysDataStatusEnum).FullName}.{Enum.GetName(typeof(SysDataStatusEnum), material.Status)}"));
            }
            #endregion

            #region 操作数据库
            await _procMaterialRepository.UpdateStatusAsync(changeStatusCommand);
            #endregion
        }

        #endregion

        /// <summary>
        /// 物料数据导入
        /// </summary>
        /// <returns></returns>
        public async Task ImportProcMaterialAsync(IFormFile formFile)
        {
            using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream).ConfigureAwait(false);
            var excelImportDtos = _excelService.Import<ProcMaterialImportDto>(memoryStream);

            #region 验证基础数据
            if (excelImportDtos == null || !excelImportDtos.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11601));
            }

            //获取物料列表信息
            IEnumerable<ProcMaterialEntity> materialEntities = new List<ProcMaterialEntity>();
            var materialCodes = excelImportDtos.Where(x => !string.IsNullOrWhiteSpace(x.MaterialCode)).Select(x => x.MaterialCode).Distinct().ToArray();
            if (materialCodes.Any())
            {
                var materialQuery = new ProcMaterialQuery() { SiteId = _currentSite.SiteId ?? 0, MaterialCodes = materialCodes.ToArray() };
                materialEntities = await _procMaterialRepository.GetProcMaterialEntitiesAsync(materialQuery);
            }

            //获取Bom列表信息
            IEnumerable<ProcBomEntity> procBomEntities = new List<ProcBomEntity>();
            var bomCodes = excelImportDtos.Where(x => !string.IsNullOrWhiteSpace(x.BomCode)).Select(x => x.BomCode).Distinct().ToArray();
            if (bomCodes.Any())
            {
                var materialQuery = new ProcBomsByCodeQuery() { SiteId = _currentSite.SiteId ?? 0, Codes = bomCodes.ToArray() };
                procBomEntities = await _procBomRepository.GetByCodesAsync(materialQuery);
            }

            //获取工艺路线列表信息
            IEnumerable<ProcProcessRouteEntity> processRouteEntities = new List<ProcProcessRouteEntity>();
            var routeCodes = excelImportDtos.Where(x => !string.IsNullOrWhiteSpace(x.ProcessRouteCode)).Select(x => x.ProcessRouteCode).Distinct().ToArray();
            if (routeCodes.Any())
            {
                var routesByCodeQuery = new ProcProcessRoutesByCodeQuery() { SiteId = _currentSite.SiteId ?? 0, Codes = routeCodes.ToArray() };
                processRouteEntities = await _procProcessRouteRepository.GetByCodesAsync(routesByCodeQuery);
            }

            //获取掩码组列表信息
            IEnumerable<ProcMaskCodeEntity> maskCodeEntities = new List<ProcMaskCodeEntity>();
            var maskCodes = excelImportDtos.Where(x => !string.IsNullOrWhiteSpace(x.MaskCode)).Select(x => x.MaskCode).Distinct().ToArray();
            if (routeCodes.Any())
            {
                var codesByCodeQuery = new ProcMaskCodesByCodeQuery() { SiteId = _currentSite.SiteId ?? 0, Codes = maskCodes.ToArray() };
                maskCodeEntities = await _procMaskCodeRepository.GetByCodesAsync(codesByCodeQuery);
            }

            var importMaterial = excelImportDtos.Where(x => !string.IsNullOrWhiteSpace(x.MaterialCode)).ToList();
            var addMaterials = new List<ProcMaterialEntity>();
            var updateMaterials = new List<ProcMaterialEntity>();

            var errorMessage = new StringBuilder("");
            var row = 0;
            foreach (var entity in excelImportDtos)
            {
                row++;
                if (string.IsNullOrWhiteSpace(entity.MaterialCode) && string.IsNullOrWhiteSpace(entity.MaterialName))
                {
                    continue;
                }

                var validFlag = true;
                if (string.IsNullOrWhiteSpace(entity.MaterialName))
                {
                    errorMessage.Append($"物料编码{entity.MaterialCode}的物料名称不能为空,");
                    validFlag = false;
                }

                var processRouteId = 0L;
                var bomId = 0L;
                var maskCodeId = 0L;
                //工艺路线验证
                if (!string.IsNullOrWhiteSpace(entity.ProcessRouteCode))
                {
                    var processRouteEntity = processRouteEntities.FirstOrDefault(x => x.Code == entity.ProcessRouteCode.ToTrimSpace().ToUpperInvariant());
                    if (processRouteEntity == null)
                    {
                        errorMessage.Append($"第{row}行的工艺路线编码{entity.ProcessRouteCode}在系统中不存在,");
                        validFlag = false;
                    }
                    processRouteId = processRouteEntity?.Id ?? 0;
                }

                //Bom验证
                if (!string.IsNullOrWhiteSpace(entity.BomCode))
                {
                    var procBomEntity = procBomEntities.FirstOrDefault(x => x.BomCode == entity.BomCode.ToTrimSpace().ToUpperInvariant());
                    if (procBomEntity == null)
                    {
                        errorMessage.Append($"第{row}行的Bom编码{entity.BomCode}在系统中不存在,");
                        validFlag = false;
                    }
                    bomId = procBomEntity?.Id ?? 0;
                }

                //掩码组验证
                if (!string.IsNullOrWhiteSpace(entity.MaskCode))
                {
                    var maskCodeEntity = maskCodeEntities.FirstOrDefault(x => x.Code == entity.MaskCode.ToTrimSpace().ToUpperInvariant());
                    if (maskCodeEntity == null)
                    {
                        errorMessage.Append($"第{row}行的掩码组{entity.MaskCode}在系统中不存在,");
                        validFlag = false;
                    }
                    maskCodeId = maskCodeEntity?.Id ?? 0;
                }

                var materialCode = entity.MaterialCode.ToTrimSpace().ToUpperInvariant();
                if (!validFlag)
                {
                    continue;
                }

                var materialEntity = materialEntities?.FirstOrDefault(x => x.MaterialCode == materialCode && x.Version == entity.Version);
                var isDefaultVersion = true;
                if (entity.IsDefaultVersion.HasValue && entity.IsDefaultVersion == TrueOrFalseEnum.No)
                {
                    isDefaultVersion = false;
                }

                if (materialEntity == null)
                {
                    addMaterials.Add(new ProcMaterialEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        MaterialCode = materialCode,
                        MaterialName = entity.MaterialName.Trim(),
                        GroupId = 0,
                        Version = entity.Version,
                        Batch = entity.Batch,
                        BuyType = entity.BuyType,
                        SerialNumber = entity.SerialNumber,
                        IsDefaultVersion = isDefaultVersion,
                        PackageNum = entity.PackageNum,
                        Remark = entity.Remark,
                        Unit = entity.Unit,
                        BaseTime = entity.BaseTime,
                        ConsumptionTolerance = entity.ConsumptionTolerance,
                        ProcessRouteId = processRouteId,
                        BomId = bomId,
                        ConsumeRatio = entity.ConsumeRatio,
                        MaskCodeId = maskCodeId,
                        Status = SysDataStatusEnum.Enable,
                        ValidTime = entity.ValidTime,
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName,
                        SiteId = _currentSite.SiteId ?? 0
                    });
                }
                else
                {
                    materialEntity.MaterialName = entity.MaterialName.Trim();
                    materialEntity.Version = entity.Version;
                    materialEntity.Batch = entity.Batch;
                    materialEntity.BuyType = entity.BuyType;
                    materialEntity.SerialNumber = entity.SerialNumber;
                    materialEntity.IsDefaultVersion = isDefaultVersion;
                    materialEntity.PackageNum = entity.PackageNum;
                    materialEntity.Remark = entity.Remark;
                    materialEntity.Unit = entity.Unit;
                    materialEntity.BaseTime = entity.BaseTime;
                    materialEntity.ConsumptionTolerance = entity.ConsumptionTolerance;
                    materialEntity.ProcessRouteId = processRouteId;
                    materialEntity.BomId = bomId;
                    materialEntity.ConsumeRatio = entity.ConsumeRatio;
                    materialEntity.MaskCodeId = maskCodeId;
                    materialEntity.Remark = entity.Remark;
                    materialEntity.ValidTime = entity.ValidTime;
                    materialEntity.UpdatedBy = _currentUser.UserName;
                    materialEntity.UpdatedOn = HymsonClock.Now();
                    updateMaterials.Add(materialEntity);
                }
            }

            if (!string.IsNullOrWhiteSpace(errorMessage.ToString()))
            {
                throw new CustomerValidationException(errorMessage.ToString());
            }
            #endregion

            #region 入库
            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                //保存记录
                if (addMaterials.Any())
                {
                    await _procMaterialRepository.InsertsAsync(addMaterials);
                }
                if (updateMaterials.Any())
                {
                    await _procMaterialRepository.UpdatesAsync(updateMaterials);
                }
                ts.Complete();
            }
            #endregion
        }

        /// <summary>
        /// 获取验证对象
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="codeFormattedMessage"></param>
        /// <param name="cuurrentRow"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static ValidationFailure GetValidationFailure(string errorCode, string codeFormattedMessage, int cuurrentRow = 1, string key = "code")
        {
            var validationFailure = new ValidationFailure
            {
                ErrorCode = errorCode
            };
            validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object>
            {
                { "CollectionIndex", cuurrentRow },
                { key, codeFormattedMessage }
            };
            return validationFailure;
        }
        private static ValidationFailure GetValidationFailure(string errorCode, string codeFormattedMessage, string versionFormattedMessage, int cuurrentRow = 1, string codeKey = "code", string versionKey = "version")
        {
            var validationFailure = new ValidationFailure
            {
                ErrorCode = errorCode
            };
            validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object>
            {
                { "CollectionIndex", cuurrentRow },
                { codeKey, codeFormattedMessage },
                { versionKey, versionFormattedMessage }
            };
            return validationFailure;
        }

        /// <summary>
        /// 下载导入模板
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public async Task DownloadImportTemplateAsync(Stream stream)
        {
            var excelTemplateDtos = new List<ProcMaterialImportDto>();
            await _excelService.ExportAsync(excelTemplateDtos, stream, "物料导入模板");
        }

        /// <summary>
        /// 根据查询条件导出物料数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ProcMaterialExportResultDto> ExprotProcMaterialListAsync(ProcMaterialPagedQueryDto param)
        {
            var pagedQuery = param.ToQuery<ProcMaterialPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            pagedQuery.PageSize = 1000;
            var pagedInfo = await _procMaterialRepository.GetPagedInfoAsync(pagedQuery);

            List<ProcMaterialExportDto> listDto = new List<ProcMaterialExportDto>();

            if (pagedInfo.Data == null || !pagedInfo.Data.Any())
            {
                var filePathN = await _excelService.ExportAsync(listDto, _localizationService.GetResource("Material"), _localizationService.GetResource("Material"));
                //上传到文件服务器
                var uploadResultN = await _minioService.PutObjectAsync(filePathN);
                return new ProcMaterialExportResultDto
                {
                    FileName = _localizationService.GetResource("Material"),
                    Path = uploadResultN.AbsoluteUrl,
                };
            }

            //工艺路线
            var processRouteIds = pagedInfo.Data.Select(x => x.ProcessRouteId.GetValueOrDefault()).Distinct().ToArray();
            var processRoutes = await _procProcessRouteRepository.GetByIdsAsync(processRouteIds);
            //Bom
            var bomIds = pagedInfo.Data.Select(x => x.BomId.GetValueOrDefault()).Distinct().ToArray();
            var boms = await _procBomRepository.GetByIdsAsync(bomIds);
            //掩码
            var maskCodeIds = pagedInfo.Data.Select(x => x.MaskCodeId.GetValueOrDefault()).Distinct().ToArray();
            var maskCodes = await _procMaskCodeRepository.GetByIdsAsync(maskCodeIds);

            foreach (var item in pagedInfo.Data)
            {
                var procMaterialDto = item.ToExcelModel<ProcMaterialExportDto>();
                //单独给是否默认版本赋值
                if (item.IsDefaultVersion)
                {
                    procMaterialDto.DefaultVersion = YesOrNoEnum.Yes;
                }
                else
                {
                    procMaterialDto.DefaultVersion = YesOrNoEnum.No;
                }

                if (item.ProcessRouteId.HasValue)
                {
                    var processRouteInfo = processRoutes.FirstOrDefault(y => y.Id == item.ProcessRouteId);
                    if (processRouteInfo != null) { procMaterialDto.ProcessRouteCode = processRouteInfo.Code; }
                }
                if (item.BomId.HasValue)
                {
                    var bomInfo = boms.FirstOrDefault(y => y.Id == item.BomId);
                    if (bomInfo != null) { procMaterialDto.BomCode = bomInfo.BomCode; }
                }
                if (item.MaskCodeId.HasValue)
                {
                    var maskCodeInfo = maskCodes.FirstOrDefault(y => y.Id == item.MaskCodeId);
                    if (maskCodeInfo != null) { procMaterialDto.MaskCode = maskCodeInfo.Code; }
                }

                listDto.Add(procMaterialDto);
            }

            var filePath = await _excelService.ExportAsync(listDto, _localizationService.GetResource("Material"), _localizationService.GetResource("Material"));
            //上传到文件服务器
            var uploadResult = await _minioService.PutObjectAsync(filePath);
            return new ProcMaterialExportResultDto
            {
                FileName = _localizationService.GetResource("Material"),
                Path = uploadResult.AbsoluteUrl,
            };
        }
    }
}
