using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Search;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Excel.Abstractions;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Constants.Process;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentGroup;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentGroup.Query;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Integrated.InteWorkCenter;
using Hymson.MES.Data.Repositories.Integrated.InteWorkCenter.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.MaskCode;
using Hymson.MES.Data.Repositories.Process.Query;
using Hymson.MES.Data.Repositories.Process.Resource;
using Hymson.MES.Data.Repositories.Process.ResourceType;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process;
using Hymson.Minio;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.AspNetCore.Http;
using Minio.DataModel;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reactive;
using System.Security.Policy;
using System.Text;
using System.Transactions;
using static Dapper.SqlMapper;


namespace Hymson.MES.Services.Services.Integrated
{
    /// <summary>
    /// 服务（基础数据导入） 
    /// </summary>
    public class ImportBasicDataService : IImportBasicDataService
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
        /// 设备仓储
        /// </summary>
        private readonly IEquEquipmentRepository _equipmentRepository;

        /// <summary>
        /// 设备组仓储
        /// </summary>
        private readonly IEquEquipmentGroupRepository _equipmentGroupRepository;

        private readonly IProcResourceRepository _procResourceRepository;

        private readonly IProcResourceTypeRepository _resourceTypeRepository;
        /// <summary>
        /// 资源关联设备仓储
        /// </summary>
        private readonly IProcResourceEquipmentBindRepository _resourceEquipmentBindRepository;

        private readonly IProcProcedureRepository _procedureRepository;

        private readonly IInteWorkCenterRepository _workCenterRepository;
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IProcMaterialGroupRepository _materialGroupRepository;
        private readonly IProcBomRepository _procBomRepository;
        private readonly IProcProcessRouteRepository _processRouteRepository;
        private readonly IProcMaskCodeRepository _maskCodeRepository;
        private readonly IProcParameterRepository _procParameterRepository;
        private readonly IProcParameterLinkTypeRepository _parameterLinkTypeRepository;
        private readonly IExcelService _excelService;
        private readonly IMinioService _minioService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ImportBasicDataService(ICurrentUser currentUser, ICurrentSite currentSite,
               IEquEquipmentRepository equipmentRepository, IEquEquipmentGroupRepository equipmentGroupRepository,
               IProcResourceRepository procResourceRepository, IProcResourceTypeRepository resourceTypeRepository,
               IProcResourceEquipmentBindRepository resourceEquipmentBindRepository,
               IProcProcedureRepository procedureRepository,
               IInteWorkCenterRepository workCenterRepository,
               IProcMaterialRepository procMaterialRepository,
               IProcMaterialGroupRepository materialGroupRepository,
               IProcBomRepository procBomRepository,
               IProcProcessRouteRepository processRouteRepository,
               IProcMaskCodeRepository maskCodeRepository,
               IProcParameterRepository procParameterRepository,
               IProcParameterLinkTypeRepository parameterLinkTypeRepository,
               IExcelService excelService, IMinioService minioService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _equipmentRepository = equipmentRepository;
            _equipmentGroupRepository = equipmentGroupRepository;
            _procResourceRepository = procResourceRepository;
            _resourceTypeRepository = resourceTypeRepository;
            _resourceEquipmentBindRepository = resourceEquipmentBindRepository;
            _procedureRepository = procedureRepository;
            _workCenterRepository = workCenterRepository;
            _procMaterialRepository = procMaterialRepository;
            _materialGroupRepository = materialGroupRepository;
            _procBomRepository = procBomRepository;
            _processRouteRepository = processRouteRepository;
            _maskCodeRepository = maskCodeRepository;
            _procParameterRepository = procParameterRepository;
            _parameterLinkTypeRepository = parameterLinkTypeRepository;
            _excelService = excelService;
            _minioService = minioService;
        }

        /// <summary>
        /// 设备数据导入
        /// </summary>
        /// <returns></returns>
        public async Task ImportEquDataAsync(IFormFile formFile)
        {
            using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream).ConfigureAwait(false);
            var excelImportDtos = _excelService.Import<ImportEquipmentDto>(memoryStream);
            //备份用户上传的文件，可选
            //var stream = formFile.OpenReadStream();
            //var uploadResult = await _minioService.PutObjectAsync(formFile.FileName, stream, formFile.ContentType);

            #region 验证基础数据
            if (excelImportDtos == null || !excelImportDtos.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11601));
            }

            //重复的设备编码
            var equCodes = excelImportDtos.Where(x => !string.IsNullOrWhiteSpace(x.EquipmentCode)).Select(x => x.EquipmentCode).Distinct().ToArray();
            if (equCodes.Length < excelImportDtos.Count())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11601));
            }

            var addEquipments = new List<EquEquipmentEntity>();
            var updateEquipments = new List<EquEquipmentEntity>();
            //获取设备列表信息
            var query = new EquEquipmentQuery() { SiteId = _currentSite.SiteId ?? 0, EquipmentCodes = equCodes };
            var equipmentEntities = await _equipmentRepository.GetEntitiesAsync(query);

            //获取设备组列表信息
            IEnumerable<EquEquipmentGroupEntity> groupEntities = new List<EquEquipmentGroupEntity>();
            var equGroupCodes = excelImportDtos.Where(x => !string.IsNullOrWhiteSpace(x.EquipmentGroup)).Select(x => x.EquipmentGroup).Distinct().ToArray();
            if (equGroupCodes != null && equGroupCodes.Any())
            {
                var groupQuery = new EquEquipmentGroupQuery() { SiteId = _currentSite.SiteId ?? 0, EquipmentGroupCodes = equGroupCodes };
                groupEntities = await _equipmentGroupRepository.GetEntitiesAsync(groupQuery);
            }

            var errorMessage = new StringBuilder("");
            var row = 0;
            foreach (var entity in excelImportDtos)
            {
                row++;

                if (string.IsNullOrWhiteSpace(entity.EquipmentCode) && string.IsNullOrWhiteSpace(entity.EquipmentName) && string.IsNullOrWhiteSpace(entity.Location))
                {
                    continue;
                }

                var validFlag = true;
                if (string.IsNullOrWhiteSpace(entity.EquipmentCode))
                {
                    errorMessage.Append($"第{row}行的设备编码不能为空,");
                    validFlag = false;
                }

                if (string.IsNullOrWhiteSpace(entity.EquipmentName))
                {
                    errorMessage.Append($"第{row}行的设备名称不能为空,");
                    validFlag = false;
                }

                if (string.IsNullOrWhiteSpace(entity.Location))
                {
                    errorMessage.Append($"第{row}行的存放位置不能为空,");
                    validFlag = false;
                }

                //使用部门
                //入场日期
                DateTime entryDate = DateTime.MinValue;
                if (!string.IsNullOrWhiteSpace(entity.EntryDate))
                {
                    var isValidDate = DateTime.TryParse(entity.EntryDate, out entryDate);
                    if (!isValidDate)
                    {
                        errorMessage.Append($"第{row}行入场日期不是有效的时间,");
                        validFlag = false;
                    }
                }

                //设备组
                var equGroupId = 0L;
                if (!string.IsNullOrWhiteSpace(entity.EquipmentGroup))
                {
                    var group = groupEntities.FirstOrDefault(x => x.EquipmentGroupCode == entity.EquipmentGroup);
                    if (group == null)
                    {
                        errorMessage.Append($"第{row}行设备组编码在系统中不存在,");
                        validFlag = false;
                    }
                    equGroupId = group?.Id ?? 0;
                }

                errorMessage.ToString().TrimEnd(',');
                if (!string.IsNullOrWhiteSpace(errorMessage.ToString()))
                {
                    errorMessage.Append(";");
                }
                if (!validFlag)
                {
                    continue;
                }

                //TODO
                //读取部门数据,部门数据来源用户中心
                var equipmentCode = entity.EquipmentCode.ToTrimSpace().ToUpperInvariant();
                var equipment = equipmentEntities.FirstOrDefault(x => x.EquipmentCode == equipmentCode);
                if (equipment == null)
                {
                    addEquipments.Add(new EquEquipmentEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        EquipmentCode = equipmentCode,
                        EquipmentName = entity.EquipmentName.ToTrimSpace(),
                        EquipmentGroupId = equGroupId,
                        WorkCenterFactoryId = 0,
                        Location = entity.Location.Trim(),
                        UseStatus = entity.UseStatus,
                        EquipmentType = entity.EquipmentType,
                        //UseDepartment=entity.UseDepartment,
                        EntryDate = string.IsNullOrWhiteSpace(entity.EntryDate) ? null : Convert.ToDateTime(entryDate),
                        QualTime = entity.QualTime,
                        Manufacturer = entity.Manufacturer ?? "",
                        Supplier = entity.Supplier ?? "",
                        Power = entity.Power,
                        EnergyLevel = entity.EnergyLevel,
                        Ip = entity.Ip ?? "",
                        Remark = entity.Remark,
                        EquipmentDesc = entity.EquipmentDesc,
                        SiteId = _currentSite.SiteId ?? 0,
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName
                    });
                }
                else
                {
                    equipment.EquipmentName = entity.EquipmentName.Trim();
                    equipment.Location = entity.Location.Trim();
                    equipment.UseStatus = entity.UseStatus;
                    equipment.EquipmentType = entity.EquipmentType;
                    equipment.EntryDate = string.IsNullOrWhiteSpace(entity.EntryDate) ? null : Convert.ToDateTime(entryDate);
                    equipment.QualTime = entity.QualTime;
                    equipment.Manufacturer = entity.Manufacturer ?? "";
                    equipment.Supplier = entity.Supplier ?? "";
                    equipment.EquipmentGroupId = equGroupId;
                    equipment.Power = entity.Power;
                    equipment.EnergyLevel = entity.EnergyLevel;
                    equipment.Ip = entity.Ip ?? "";
                    equipment.Remark = entity.Remark;
                    equipment.EquipmentDesc = entity.EquipmentDesc;
                    equipment.UpdatedBy = _currentUser.UserName;
                    equipment.UpdatedOn = HymsonClock.Now();
                    updateEquipments.Add(equipment);
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
                if (addEquipments.Any())
                {
                    await _equipmentRepository.InsertsAsync(addEquipments);
                }
                if (updateEquipments.Any())
                {
                    await _equipmentRepository.UpdatesAsync(updateEquipments);
                }
                ts.Complete();
            }
            #endregion
        }

        /// <summary>
        /// 资源数据导入
        /// </summary>
        /// <returns></returns>
        public async Task ImportResourceDataAsync(IFormFile formFile)
        {
            using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream).ConfigureAwait(false);
            var excelImportDtos = _excelService.Import<ImportResourceDto>(memoryStream);

            #region 验证基础数据
            if (excelImportDtos == null || !excelImportDtos.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11601));
            }

            //资源编码、名称不能为空
            //if (excelImportDtos.Any(x => string.IsNullOrWhiteSpace(x.ResCode)))
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES10301));
            //}
            //if (excelImportDtos.Any(x => string.IsNullOrWhiteSpace(x.ResName)))
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES10303));
            //}

            //获取资源列表信息
            IEnumerable<ProcResourceEntity> resourceEntities = new List<ProcResourceEntity>();
            var resCodes = excelImportDtos.Where(x => !string.IsNullOrWhiteSpace(x.ResCode)).Select(x => x.ResCode).Distinct().ToArray();
            if (resCodes.Any())
            {
                var resourceQuery = new ProcResourceQuery() { SiteId = _currentSite.SiteId ?? 0, ResCodes = resCodes };
                resourceEntities = await _procResourceRepository.GetEntitiesAsync(resourceQuery);
            }

            //获取设备列表信息
            IEnumerable<EquEquipmentEntity> equEquipmentEntities = new List<EquEquipmentEntity>();
            var equCodes = excelImportDtos.Where(x => !string.IsNullOrWhiteSpace(x.EquipmentCode)).Select(x => x.EquipmentCode).Distinct().ToArray();
            if (equCodes != null && equCodes.Any())
            {
                var query = new EquEquipmentQuery() { SiteId = _currentSite.SiteId ?? 0, EquipmentCodes = equCodes };
                equEquipmentEntities = await _equipmentRepository.GetEntitiesAsync(query);
            }

            //获取资源类型列表信息
            IEnumerable<ProcResourceTypeEntity> resourceTypeEntities = new List<ProcResourceTypeEntity>();
            var resTypes = excelImportDtos.Where(x => !string.IsNullOrWhiteSpace(x.ResType)).Select(x => x.ResType).Distinct().ToArray();
            if (resTypes != null && resTypes.Any())
            {
                var query = new ProcResourceTypeQuery() { SiteId = _currentSite.SiteId ?? 0, ResTypes = resTypes };
                resourceTypeEntities = await _resourceTypeRepository.GetEntitiesAsync(query);
            }

            var importResources = excelImportDtos.Where(x => !string.IsNullOrWhiteSpace(x.ResCode)).DistinctBy(x => x.ResCode).ToList();

            var addResources = new List<ProcResourceEntity>();
            var updateResources = new List<ProcResourceEntity>();
            var errorMessage = new StringBuilder("");
            foreach (var entity in importResources)
            {
                if (string.IsNullOrWhiteSpace(entity.ResCode) && string.IsNullOrWhiteSpace(entity.ResName))
                {
                    continue;
                }

                var validFlag = true;
                if (string.IsNullOrWhiteSpace(entity.ResName))
                {
                    errorMessage.Append($"资源编码{entity.ResCode}的资源名称不能为空,");
                    validFlag = false;
                }

                var resCode = entity.ResCode.ToTrimSpace().ToUpperInvariant();
                var resourceEntity = resourceEntities.FirstOrDefault(x => x.ResCode == resCode);

                //资源类型验证
                var resTypeId = 0L;
                if (!string.IsNullOrWhiteSpace(entity.ResType))
                {
                    var resourceTypeEntity = resourceTypeEntities.FirstOrDefault(x => x.ResType == entity.ResType.ToTrimSpace().ToUpperInvariant());
                    if (resourceTypeEntity == null)
                    {
                        errorMessage.Append($"资源{resCode}的资源类型在系统中不存在,");
                        validFlag = false;
                    }
                    resTypeId = resourceTypeEntity?.Id ?? 0;
                }

                if (!validFlag)
                {
                    continue;
                }

                if (resourceEntity == null)
                {
                    addResources.Add(new ProcResourceEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        ResCode = resCode,
                        ResName = entity.ResName.Trim(),
                        Status = (int)SysDataStatusEnum.Enable,
                        ResTypeId = resTypeId,
                        Remark = entity.Remark,
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName,
                        SiteId = _currentSite.SiteId ?? 0
                    });
                }
                else
                {
                    resourceEntity.ResName = entity.ResName.Trim();
                    resourceEntity.Remark = entity.Remark;
                    resourceEntity.ResTypeId = resTypeId;
                    resourceEntity.UpdatedBy = _currentUser.UserName;
                    resourceEntity.UpdatedOn = HymsonClock.Now();
                    updateResources.Add(resourceEntity);
                }
            }

            var resources = new List<ProcResourceEntity>();
            resources.AddRange(addResources);
            resources.AddRange(updateResources);

            //设备绑定设置数据
            var equList = new List<ProcResourceEquipmentBindEntity>();
            var row = 0;
            foreach (var entity in excelImportDtos)
            {
                row++;
                if (string.IsNullOrWhiteSpace(entity.ResCode) && string.IsNullOrWhiteSpace(entity.ResName))
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(entity.ResCode))
                {
                    errorMessage.Append($"第{row}行资源编码不能为空,");
                    continue;
                }

                if (string.IsNullOrWhiteSpace(entity.ResName))
                {
                    errorMessage.Append($"第{row}行资源名称不能为空,");
                    continue;
                }

                //设备编码验证
                var equipmentEntity = new EquEquipmentEntity();
                var validFlag = true;
                if (!string.IsNullOrWhiteSpace(entity.EquipmentCode))
                {
                    equipmentEntity = equEquipmentEntities.FirstOrDefault(x => x.EquipmentCode == entity.EquipmentCode.ToTrimSpace().ToUpperInvariant());
                    if (equipmentEntity == null)
                    {
                        errorMessage.Append($"第{row}行设备编码在系统中不存在,");
                        validFlag = false;
                    }
                }

                var isMain = false;
                errorMessage.ToString().TrimEnd(',').TrimEnd(';');
                if (!string.IsNullOrWhiteSpace(errorMessage.ToString()))
                {
                    errorMessage.Append(";");
                }
                if (!validFlag)
                {
                    continue;
                }
                equList.Add(new ProcResourceEquipmentBindEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    ResourceId = resources.FirstOrDefault(x => x.ResCode == entity.ResCode.ToTrimSpace().ToUpperInvariant())?.Id ?? 0,
                    EquipmentId = equipmentEntity?.Id ?? 0,
                    IsMain = isMain,
                    Remark = "",
                    SiteId = _currentSite.SiteId ?? 0,
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName
                });
            }

            if (!string.IsNullOrWhiteSpace(errorMessage.ToString()))
            {
                throw new CustomerValidationException(errorMessage.ToString());
            }

            if (equList.GroupBy(m => new { m.ResourceId, m.EquipmentId }).Any(x => x.Count() > 1))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10314));
            }
            //TODO 一个资源只能对应一个主设备
            #endregion

            #region 入库
            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                //保存记录 
                if (addResources.Any())
                {
                    await _procResourceRepository.InsertsAsync(addResources);
                }
                if (updateResources.Any())
                {
                    await _procResourceRepository.UpdatesAsync(updateResources);
                }

                //资源跟设备的关联关系
                if (equList != null && equList.Count > 0)
                {
                    await _resourceEquipmentBindRepository.InsertRangeAsync(equList);
                }
                ts.Complete();
            }
            #endregion
        }

        /// <summary>
        /// 资源类型数据导入
        /// </summary>
        /// <returns></returns>
        public async Task ImportResourceTypeDataAsync(IFormFile formFile)
        {
            using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream).ConfigureAwait(false);
            var excelImportDtos = _excelService.Import<ImportResourceTypeDto>(memoryStream);

            #region 验证基础数据
            if (excelImportDtos == null || !excelImportDtos.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11601));
            }

            var addTypeEntities = new List<ProcResourceTypeEntity>();
            var updateResourceTypes = new List<ProcResourceTypeEntity>();

            //获取资源类型信息
            var resTypes = excelImportDtos.Where(x => !string.IsNullOrWhiteSpace(x.ResType)).Select(x => x.ResType).Distinct().ToArray();
            var query = new ProcResourceTypeQuery() { SiteId = _currentSite.SiteId ?? 0, ResTypes = resTypes };
            var resourceTypeEntities = await _resourceTypeRepository.GetEntitiesAsync(query);

            var errorMessage = new StringBuilder("");
            var importResourceTypes = excelImportDtos.Where(x => !string.IsNullOrWhiteSpace(x.ResType)).DistinctBy(x => x.ResType).ToList();
            foreach (var entity in importResourceTypes)
            {
                var validFlag = true;
                if (string.IsNullOrWhiteSpace(entity.ResType) && string.IsNullOrWhiteSpace(entity.ResTypeName))
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(entity.ResTypeName))
                {
                    errorMessage.Append($"资源类型编码{entity.ResType}的资源类型名称不能为空,");
                    validFlag = false;
                }

                //资源,修改资源表的资源类型
                errorMessage.ToString().TrimEnd(',');
                if (!string.IsNullOrWhiteSpace(errorMessage.ToString()))
                {
                    errorMessage.Append(";");
                }
                if (!validFlag)
                {
                    continue;
                }

                //读取部门数据,部门数据来源用户中心
                var resType = entity.ResType.ToTrimSpace().ToUpperInvariant();
                var procResourceType = resourceTypeEntities.FirstOrDefault(x => x.ResType == resType);
                if (procResourceType == null)
                {
                    addTypeEntities.Add(new ProcResourceTypeEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        ResType = resType,
                        ResTypeName = entity.ResTypeName.Trim(),
                        Remark = entity.Remark,
                        SiteId = _currentSite.SiteId ?? 0,
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName
                    });
                }
                else
                {
                    procResourceType.ResTypeName = entity.ResTypeName;
                    procResourceType.Remark = entity.Remark.Trim();
                    procResourceType.UpdatedBy = _currentUser.UserName;
                    procResourceType.UpdatedOn = HymsonClock.Now();
                    updateResourceTypes.Add(procResourceType);
                }
            }

            if (!string.IsNullOrWhiteSpace(errorMessage.ToString()))
            {
                throw new CustomerValidationException(errorMessage.ToString());
            }

            //获取资源列表信息
            //IEnumerable<ProcResourceEntity> resourceEntities = new List<ProcResourceEntity>();
            //var resCodes = excelImportDtos.Where(x => !string.IsNullOrWhiteSpace(x.ResCode)).Select(x => x.ResCode).Distinct().ToArray();
            //if (resCodes.Any())
            //{
            //    var resourceQuery = new ProcResourceQuery() { SiteId = _currentSite.SiteId ?? 0, ResCodes = resCodes };
            //    resourceEntities = await _procResourceRepository.GetEntitiesAsync(resourceQuery);
            //}
            //TODO 修改资源的资源类型,验证资源信息,资源的资源类型信息修改
            #endregion

            #region 入库
            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                //保存记录 
                if (addTypeEntities.Any())
                {
                    await _resourceTypeRepository.InsertsAsync(addTypeEntities);
                }
                if (updateResourceTypes.Any())
                {
                    await _resourceTypeRepository.UpdatesAsync(updateResourceTypes);
                }

                //更新资源的资源类型
                // await _procResourceRepository.UpdateResTypeAsync(updateCommand);
                ts.Complete();
            }
            #endregion
        }

        /// <summary>
        /// 工序数据导入
        /// </summary>
        /// <returns></returns>
        public async Task ImportProcedureDataAsync(IFormFile formFile)
        {
            using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream).ConfigureAwait(false);
            var excelImportDtos = _excelService.Import<ImportProcedureDto>(memoryStream);

            #region 验证基础数据
            if (excelImportDtos == null || !excelImportDtos.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11601));
            }

            //重复的工序编码
            //var equCodes = excelImportDtos.Where(x => !string.IsNullOrWhiteSpace(x.Code)).Select(x => x.Code).Distinct().ToArray();
            //if (equCodes.Length < excelImportDtos.Count())
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES11607));
            //}

            var addProcedureEntities = new List<ProcProcedureEntity>();
            var updateProcedureeEntities = new List<ProcProcedureEntity>();

            //获取工序信息
            var codes = excelImportDtos.Where(x => !string.IsNullOrWhiteSpace(x.Code)).Select(x => x.Code).Distinct().ToArray();
            var query = new ProcProcedureQuery() { SiteId = _currentSite.SiteId ?? 0, Codes = codes };
            var procedureEntities = await _procedureRepository.GetEntitiesAsync(query);

            //获取资源类型信息
            IEnumerable<ProcResourceTypeEntity> resourceTypeEntities = new List<ProcResourceTypeEntity>();
            var resTypes = excelImportDtos.Where(x => !string.IsNullOrWhiteSpace(x.ResType)).Select(x => x.ResType).Distinct().ToArray();
            if (resTypes.Any())
            {
                var typeQuery = new ProcResourceTypeQuery() { SiteId = _currentSite.SiteId ?? 0, ResTypes = resTypes };
                resourceTypeEntities = await _resourceTypeRepository.GetEntitiesAsync(typeQuery);
            }

            var errorMessage = new StringBuilder("");
            var row = 0;
            foreach (var entity in excelImportDtos)
            {
                row++;
                if (string.IsNullOrWhiteSpace(entity.Code) && string.IsNullOrWhiteSpace(entity.Name))
                {
                    continue;
                }

                var validFlag = true;
                if (string.IsNullOrWhiteSpace(entity.Name))
                {
                    errorMessage.Append($"第{row}行的工序名称不能为空,");
                    validFlag = false;
                }

                //资源类型验证
                var resTypeId = 0L;
                if (!string.IsNullOrWhiteSpace(entity.ResType))
                {
                    var resourceTypeEntity = resourceTypeEntities.FirstOrDefault(x => x.ResType == entity.ResType.ToTrimSpace().ToUpperInvariant());
                    if (resourceTypeEntity == null)
                    {
                        errorMessage.Append($"第{row}行的资源类型{entity.ResType}在系统中不存在,");
                        validFlag = false;
                    }
                    resTypeId = resourceTypeEntity?.Id ?? 0;
                }

                errorMessage.ToString().TrimEnd(',');
                if (!string.IsNullOrWhiteSpace(errorMessage.ToString()))
                {
                    errorMessage.Append(";");
                }
                if (!validFlag)
                {
                    continue;
                }

                var code = entity.Code.ToTrimSpace().ToUpperInvariant();
                var procProcedure = procedureEntities.FirstOrDefault(x => x.Code == code);
                var isRepairReturn = entity.IsRepairReturn == TrueOrFalseEnum.Yes ? 1 : 0;
                if (procProcedure == null)
                {
                    addProcedureEntities.Add(new ProcProcedureEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        Code = code,
                        Name = entity.Name.Trim(),
                        Status = SysDataStatusEnum.Enable,
                        Type = entity.Type,
                        IsRepairReturn = (byte)isRepairReturn,
                        ResourceTypeId = resTypeId,
                        SiteId = _currentSite.SiteId ?? 0,
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName
                    });
                }
                else
                {
                    procProcedure.Name = entity.Name;
                    procProcedure.Remark = entity.Remark.Trim();
                    procProcedure.IsRepairReturn = (byte)isRepairReturn;
                    procProcedure.ResourceTypeId = resTypeId;
                    procProcedure.UpdatedBy = _currentUser.UserName;
                    procProcedure.UpdatedOn = HymsonClock.Now();
                    updateProcedureeEntities.Add(procProcedure);
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
                if (addProcedureEntities.Any())
                {
                    await _procedureRepository.InsertsAsync(addProcedureEntities);
                }
                if (updateProcedureeEntities.Any())
                {
                    await _procedureRepository.UpdatesAsync(updateProcedureeEntities);
                }
                ts.Complete();
            }
            #endregion
        }

        /// <summary>
        /// 产线数据导入
        /// </summary>
        /// <returns></returns>
        public async Task ImportWorkLineDataAsync(IFormFile formFile)
        {
            using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream).ConfigureAwait(false);
            var excelImportDtos = _excelService.Import<ImportWorkLineDto>(memoryStream);

            #region 验证基础数据
            if (excelImportDtos == null || !excelImportDtos.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11601));
            }

            //获取产线列表信息
            IEnumerable<InteWorkCenterEntity> workCenterEntities = new List<InteWorkCenterEntity>();
            var workLineCodes = excelImportDtos.Where(x => !string.IsNullOrWhiteSpace(x.Code)).Select(x => x.Code).Distinct().ToArray();
            if (workLineCodes.Any())
            {
                var inteWorkCenterQuery = new InteWorkCenterQuery() { SiteId = _currentSite.SiteId ?? 0, Codes = workLineCodes };
                workCenterEntities = await _workCenterRepository.GetEntitiesAsync(inteWorkCenterQuery);
            }

            //获取资源列表信息
            IEnumerable<ProcResourceEntity> resourceEntities = new List<ProcResourceEntity>();
            var resCodes = excelImportDtos.Where(x => !string.IsNullOrWhiteSpace(x.ResCode)).Select(x => x.ResCode).Distinct().ToArray();
            if (resCodes.Any())
            {
                var resourceQuery = new ProcResourceQuery() { SiteId = _currentSite.SiteId ?? 0, ResCodes = resCodes };
                resourceEntities = await _procResourceRepository.GetEntitiesAsync(resourceQuery);
            }

            var importWorkLines = excelImportDtos.Where(x => !string.IsNullOrWhiteSpace(x.Code)).DistinctBy(x => x.Code).ToList();

            var addWorkLines = new List<InteWorkCenterEntity>();
            var updateWorkLines = new List<InteWorkCenterEntity>();
            var errorMessage = new StringBuilder("");
            foreach (var entity in importWorkLines)
            {
                if (string.IsNullOrWhiteSpace(entity.Code) && string.IsNullOrWhiteSpace(entity.Name))
                {
                    continue;
                }

                var validFlag = true;
                if (string.IsNullOrWhiteSpace(entity.Name))
                {
                    errorMessage.Append($"产线编码{entity.Code}的产线名称不能为空,");
                    validFlag = false;
                }

                var lineCode = entity.Code.ToTrimSpace().ToUpperInvariant();
                var workCenterEntity = workCenterEntities.FirstOrDefault(x => x.Code == lineCode);

                if (!validFlag)
                {
                    continue;
                }

                if (workCenterEntity == null)
                {
                    addWorkLines.Add(new InteWorkCenterEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        Code = lineCode,
                        Name = entity.Name.Trim(),
                        Type = WorkCenterTypeEnum.Line,
                        Source = WorkCenterSourceEnum.MES,
                        Status = SysDataStatusEnum.Enable,
                        IsMixLine = entity.IsMixLine == TrueOrFalseEnum.Yes ? true : false,
                        Remark = entity.Remark,
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName,
                        SiteId = _currentSite.SiteId ?? 0
                    });
                }
                else
                {
                    workCenterEntity.Name = entity.Name.Trim();
                    workCenterEntity.Remark = entity.Remark;
                    workCenterEntity.IsMixLine = entity.IsMixLine == TrueOrFalseEnum.Yes ? true : false;
                    workCenterEntity.UpdatedBy = _currentUser.UserName;
                    workCenterEntity.UpdatedOn = HymsonClock.Now();
                    updateWorkLines.Add(workCenterEntity);
                }
            }

            var inteWorkCenters = new List<InteWorkCenterEntity>();
            inteWorkCenters.AddRange(addWorkLines);
            inteWorkCenters.AddRange(updateWorkLines);

            //线体绑定资源数据
            List<InteWorkCenterResourceRelation> inteWorkCenterResourceRelations = new();
            var row = 0;
            foreach (var entity in excelImportDtos)
            {
                row++;
                if (string.IsNullOrWhiteSpace(entity.Code) && string.IsNullOrWhiteSpace(entity.Name) || string.IsNullOrWhiteSpace(entity.ResCode))
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(entity.ResCode))
                {
                    errorMessage.Append($"第{row}行产线编码不能为空,");
                    continue;
                }

                if (string.IsNullOrWhiteSpace(entity.Name))
                {
                    errorMessage.Append($"第{row}行产线名称不能为空,");
                    continue;
                }

                //资源编码验证
                var resourceEntity = new ProcResourceEntity();
                var validFlag = true;
                if (!string.IsNullOrWhiteSpace(entity.ResCode))
                {
                    resourceEntity = resourceEntities.FirstOrDefault(x => x.ResCode == entity.ResCode.ToTrimSpace().ToUpperInvariant());
                    if (resourceEntity == null)
                    {
                        errorMessage.Append($"第{row}行资源编码在系统中不存在,");
                        validFlag = false;
                    }
                }

                errorMessage.ToString().TrimEnd(',');
                if (!string.IsNullOrWhiteSpace(errorMessage.ToString()))
                {
                    errorMessage.Append(";");
                }
                if (!validFlag)
                {
                    continue;
                }

                var workCenterId = inteWorkCenters.FirstOrDefault(x => x.Code == entity.Code)?.Id ?? 0;
                inteWorkCenterResourceRelations.Add(new InteWorkCenterResourceRelation
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    ResourceId = resourceEntity?.Id,
                    WorkCenterId = workCenterId,
                    Remark = entity.Remark,
                    SiteId = _currentSite.SiteId ?? 0,
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName
                });
            }

            // 是否存在相同资源
            //if (inteWorkCenterResourceRelations.GroupBy(g => g.ResourceId).Count()
            //    < inteWorkCenterResourceRelations.Count)
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES12120));
            //}

            //// 判断资源的状态是否存在新建和废除状态
            //var resources = await _procResourceRepository.GetListByIdsAsync(param.ResourceIds.ToArray());
            //if (resources != null && resources.Any(a => a.Status == (int)SysDataStatusEnum.Build || a.Status == (int)SysDataStatusEnum.Abolish))
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES12121));
            //}

            //// 判断资源是否被重复绑定
            //var workCenterIds = await _inteWorkCenterRepository.GetWorkCenterIdByResourceIdAsync(param.ResourceIds);
            //if (workCenterIds != null && workCenterIds.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES12117));

            if (!string.IsNullOrWhiteSpace(errorMessage.ToString()))
            {
                throw new CustomerValidationException(errorMessage.ToString());
            }
            #endregion

            #region 入库
            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                //保存记录
                if (addWorkLines.Any())
                {
                    await _workCenterRepository.InsertRangAsync(addWorkLines);
                }
                if (updateWorkLines.Any())
                {
                    await _workCenterRepository.UpdateRangAsync(updateWorkLines);
                }

                //线体跟资源的关联关系
                if (inteWorkCenterResourceRelations.Any())
                {
                    await _workCenterRepository.InsertInteWorkCenterResourceRelationRangAsync(inteWorkCenterResourceRelations);
                }
                ts.Complete();
            }
            #endregion
        }

        /// <summary>
        /// 车间数据导入
        /// </summary>
        /// <returns></returns>
        public async Task ImportWorkShopDataAsync(IFormFile formFile)
        {
            using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream).ConfigureAwait(false);
            var excelImportDtos = _excelService.Import<ImportWorkShopDto>(memoryStream);

            #region 验证基础数据
            if (excelImportDtos == null || !excelImportDtos.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11601));
            }

            //获取工作中心(车间、产线)列表信息
            IEnumerable<InteWorkCenterEntity> workCenterEntities = new List<InteWorkCenterEntity>();
            var workShopCodes = excelImportDtos.Where(x => !string.IsNullOrWhiteSpace(x.Code)).Select(x => x.Code).Distinct().ToArray();
            var workLineCodes = excelImportDtos.Where(x => !string.IsNullOrWhiteSpace(x.LineCode)).Select(x => x.LineCode).Distinct().ToArray();

            var workCodes = new List<string>();
            if (workShopCodes.Any())
            {
                workCodes.AddRange(workShopCodes);
            }
            if (workLineCodes.Any())
            {
                workCodes.AddRange(workLineCodes);
            }
            if (workCodes.Any())
            {
                var inteWorkCenterQuery = new InteWorkCenterQuery() { SiteId = _currentSite.SiteId ?? 0, Codes = workCodes.ToArray() };
                workCenterEntities = await _workCenterRepository.GetEntitiesAsync(inteWorkCenterQuery);
            }

            var importWorkShops = excelImportDtos.Where(x => !string.IsNullOrWhiteSpace(x.Code)).DistinctBy(x => x.Code).ToList();
            var addWorkShops = new List<InteWorkCenterEntity>();
            var updateWorkShops = new List<InteWorkCenterEntity>();
            var errorMessage = new StringBuilder("");
            foreach (var entity in importWorkShops)
            {
                if (string.IsNullOrWhiteSpace(entity.Code) && string.IsNullOrWhiteSpace(entity.Name))
                {
                    continue;
                }

                var validFlag = true;
                if (string.IsNullOrWhiteSpace(entity.Name))
                {
                    errorMessage.Append($"车间编码{entity.Code}的车间名称不能为空,");
                    validFlag = false;
                }

                var lineCode = entity.Code.ToTrimSpace().ToUpperInvariant();
                var workCenterEntity = workCenterEntities.FirstOrDefault(x => x.Code == lineCode);

                if (!validFlag)
                {
                    continue;
                }

                if (workCenterEntity == null)
                {
                    addWorkShops.Add(new InteWorkCenterEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        Code = lineCode,
                        Name = entity.Name.Trim(),
                        Type = WorkCenterTypeEnum.Farm,
                        Source = WorkCenterSourceEnum.MES,
                        Status = SysDataStatusEnum.Enable,
                        IsMixLine = false,
                        Remark = entity.Remark,
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName,
                        SiteId = _currentSite.SiteId ?? 0
                    });
                }
                else
                {
                    workCenterEntity.Name = entity.Name.Trim();
                    workCenterEntity.Remark = entity.Remark;
                    workCenterEntity.IsMixLine = false;
                    workCenterEntity.UpdatedBy = _currentUser.UserName;
                    workCenterEntity.UpdatedOn = HymsonClock.Now();
                    updateWorkShops.Add(workCenterEntity);
                }
            }

            var inteWorkCenters = new List<InteWorkCenterEntity>();
            inteWorkCenters.AddRange(addWorkShops);
            inteWorkCenters.AddRange(updateWorkShops);

            //车间绑定线体数据
            List<InteWorkCenterRelation> inteWorkCenterRelations = new();
            var row = 0;
            foreach (var entity in excelImportDtos)
            {
                row++;
                if (string.IsNullOrWhiteSpace(entity.Code) && string.IsNullOrWhiteSpace(entity.Name) || string.IsNullOrWhiteSpace(entity.LineCode))
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(entity.Code))
                {
                    errorMessage.Append($"第{row}行车间编码不能为空,");
                    continue;
                }

                if (string.IsNullOrWhiteSpace(entity.Name))
                {
                    errorMessage.Append($"第{row}行车间名称不能为空,");
                    continue;
                }

                //产线编码验证
                var workLineEntity = new InteWorkCenterEntity();
                var validFlag = true;
                if (!string.IsNullOrWhiteSpace(entity.LineCode))
                {
                    workLineEntity = workCenterEntities.FirstOrDefault(x => x.Code == entity.LineCode.ToTrimSpace().ToUpperInvariant());
                    if (workLineEntity == null)
                    {
                        errorMessage.Append($"第{row}行产线编码在系统中不存在,");
                        validFlag = false;
                    }
                }

                errorMessage.ToString().TrimEnd(',');
                if (!string.IsNullOrWhiteSpace(errorMessage.ToString()))
                {
                    errorMessage.Append(";");
                }
                if (!validFlag)
                {
                    continue;
                }

                var workCenterId = inteWorkCenters.FirstOrDefault(x => x.Code == entity.Code)?.Id ?? 0;
                inteWorkCenterRelations.Add(new InteWorkCenterRelation
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    WorkCenterId = workCenterId,
                    SubWorkCenterId = workLineEntity?.Id ?? 0,
                    Remark = entity.Remark,
                    SiteId = _currentSite.SiteId ?? 0,
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName
                });
            }

            if (!string.IsNullOrWhiteSpace(errorMessage.ToString()))
            {
                throw new CustomerValidationException(errorMessage.ToString());
            }

            //// 是否存在相同车间/产线
            //if (inteWorkCenterRelations.GroupBy(g => g.SubWorkCenterId).Count()
            //    < inteWorkCenterRelations.Count)
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES12119));
            //}

            ////验证产线是否已经绑定过车间，一个产线只能绑定一个车间
            ////根据产线获取信息
            //var inteWorkCenterByLineEntities = await _inteWorkCenterRepository.GetInteWorkCenterRelationEntityAsync(new InteWorkCenterRelationQuery { SubWorkCenterIds = param.WorkCenterIds });
            //if (inteWorkCenterByLineEntities != null && inteWorkCenterByLineEntities.Any())
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES12126));
            //}
            #endregion

            #region 入库
            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                //保存记录
                if (addWorkShops.Any())
                {
                    await _workCenterRepository.InsertRangAsync(addWorkShops);
                }
                if (updateWorkShops.Any())
                {
                    await _workCenterRepository.UpdateRangAsync(updateWorkShops);
                }

                //车间跟线体的关联关系
                if (inteWorkCenterRelations.Any())
                {
                    await _workCenterRepository.InsertInteWorkCenterRelationRangAsync(inteWorkCenterRelations);
                }
                ts.Complete();
            }
            #endregion
        }

        /// <summary>
        /// 物料组数据导入
        /// </summary>
        /// <returns></returns>
        public async Task ImportMaterialGroupDataAsync(IFormFile formFile)
        {
            using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream).ConfigureAwait(false);
            var excelImportDtos = _excelService.Import<ImportMaterialGroupDto>(memoryStream);

            #region 验证基础数据
            if (excelImportDtos == null || !excelImportDtos.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11601));
            }

            //获取物料组列表信息
            IEnumerable<ProcMaterialGroupEntity> groupEntities = new List<ProcMaterialGroupEntity>();
            var materialGroupCodes = excelImportDtos.Where(x => !string.IsNullOrWhiteSpace(x.GroupCode)).Select(x => x.GroupCode).Distinct().ToArray();
            if (materialGroupCodes.Any())
            {
                var groupQuery = new ProcMaterialGroupQuery() { SiteId = _currentSite.SiteId ?? 0, GroupCodes = materialGroupCodes.ToArray() };
                groupEntities = await _materialGroupRepository.GetProcMaterialGroupEntitiesAsync(groupQuery);
            }

            //获取物料列表信息
            IEnumerable<ProcMaterialEntity> procMaterials = new List<ProcMaterialEntity>();
            var materialCodes = excelImportDtos.Where(x => !string.IsNullOrWhiteSpace(x.MaterialCode)).Select(x => x.MaterialCode).Distinct().ToArray();
            if (materialCodes.Any())
            {
                var materialQuery = new ProcMaterialQuery() { SiteId = _currentSite.SiteId ?? 0, MaterialCodes = materialCodes.ToArray() };
                procMaterials = await _procMaterialRepository.GetProcMaterialEntitiesAsync(materialQuery);
            }

            var importMaterialGroups = excelImportDtos.Where(x => !string.IsNullOrWhiteSpace(x.GroupCode)).DistinctBy(x => x.GroupCode).ToList();
            var addMaterialGroups = new List<ProcMaterialGroupEntity>();
            var updateMaterialGroups = new List<ProcMaterialGroupEntity>();

            var errorMessage = new StringBuilder("");
            foreach (var entity in importMaterialGroups)
            {
                if (string.IsNullOrWhiteSpace(entity.GroupCode) && string.IsNullOrWhiteSpace(entity.GroupName))
                {
                    continue;
                }

                var validFlag = true;
                if (string.IsNullOrWhiteSpace(entity.GroupName))
                {
                    errorMessage.Append($"物料组编码{entity.GroupName}的物料组名称不能为空,");
                    validFlag = false;
                }

                var groupCode = entity.GroupCode.ToTrimSpace().ToUpperInvariant();
                if (!validFlag)
                {
                    continue;
                }

                var materialGroupEntity = groupEntities?.FirstOrDefault(x => x.GroupCode == groupCode);
                if (materialGroupEntity == null)
                {
                    addMaterialGroups.Add(new ProcMaterialGroupEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        GroupCode = groupCode,
                        GroupName = entity.GroupName.Trim(),
                        Remark = entity.Remark,
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName,
                        SiteId = _currentSite.SiteId ?? 0
                    });
                }
                else
                {
                    materialGroupEntity.GroupName = entity.GroupName.Trim();
                    materialGroupEntity.Remark = entity.Remark;
                    materialGroupEntity.UpdatedBy = _currentUser.UserName;
                    materialGroupEntity.UpdatedOn = HymsonClock.Now();
                    updateMaterialGroups.Add(materialGroupEntity);
                }
            }

            var procMaterialGroups = new List<ProcMaterialGroupEntity>();
            procMaterialGroups.AddRange(addMaterialGroups);
            procMaterialGroups.AddRange(updateMaterialGroups);

            var procMaterialEntities = new List<ProcMaterialEntity>();
            var row = 0;
            foreach (var entity in excelImportDtos)
            {
                row++;
                if (string.IsNullOrWhiteSpace(entity.GroupCode) && string.IsNullOrWhiteSpace(entity.GroupName) || string.IsNullOrWhiteSpace(entity.MaterialCode))
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(entity.GroupCode))
                {
                    errorMessage.Append($"第{row}行物料组编码不能为空,");
                    continue;
                }

                if (string.IsNullOrWhiteSpace(entity.GroupName))
                {
                    errorMessage.Append($"第{row}行物料组名称不能为空,");
                    continue;
                }

                //物料编码验证
                var procMaterialEntity = new ProcMaterialEntity();
                var validFlag = true;
                if (!string.IsNullOrWhiteSpace(entity.MaterialCode))
                {
                    procMaterialEntity = procMaterials.FirstOrDefault(x => x.MaterialCode == entity.MaterialCode.ToTrimSpace().ToUpperInvariant());
                    if (procMaterialEntity == null)
                    {
                        errorMessage.Append($"第{row}行物料编码在系统中不存在,");
                        validFlag = false;
                    }
                }

                errorMessage.ToString().TrimEnd(',');
                if (!string.IsNullOrWhiteSpace(errorMessage.ToString()))
                {
                    errorMessage.Append(";");
                }
                if (!validFlag)
                {
                    continue;
                }

                var code = entity.GroupCode.ToTrimSpace().ToUpperInvariant();
                var groupId = procMaterialGroups.FirstOrDefault(x => x.GroupCode == code)?.Id ?? 0;
                procMaterialEntities.Add(new ProcMaterialEntity
                {
                    Id = procMaterialEntity?.Id ?? 0,
                    GroupId = groupId,
                    Remark = entity.Remark,
                    UpdatedBy = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now()
                });
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
                if (addMaterialGroups.Any())
                {
                    await _materialGroupRepository.InsertsAsync(addMaterialGroups);
                }
                if (updateMaterialGroups.Any())
                {
                    await _materialGroupRepository.UpdatesAsync(updateMaterialGroups);
                }

                //物料跟物料组的关联关系
                if (procMaterialEntities.Any())
                {
                    await _procMaterialRepository.UpdateProcMaterialGroupAsync(procMaterialEntities);
                }
                ts.Complete();
            }
            #endregion
        }

        /// <summary>
        /// 物料数据导入
        /// </summary>
        /// <returns></returns>
        public async Task ImportMaterialDataAsync(IFormFile formFile)
        {
            using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream).ConfigureAwait(false);
            var excelImportDtos = _excelService.Import<ImportMaterialDto>(memoryStream);

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
                processRouteEntities = await _processRouteRepository.GetByCodesAsync(routesByCodeQuery);
            }

            //获取掩码组列表信息
            IEnumerable<ProcMaskCodeEntity> maskCodeEntities = new List<ProcMaskCodeEntity>();
            var maskCodes = excelImportDtos.Where(x => !string.IsNullOrWhiteSpace(x.MaskCode)).Select(x => x.MaskCode).Distinct().ToArray();
            if (routeCodes.Any())
            {
                var codesByCodeQuery = new ProcMaskCodesByCodeQuery() { SiteId = _currentSite.SiteId ?? 0, Codes = maskCodes.ToArray() };
                maskCodeEntities = await _maskCodeRepository.GetByCodesAsync(codesByCodeQuery);
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
        /// 参数数据导入
        /// </summary>
        /// <returns></returns>
        public async Task ImportParameterDataAsync(IFormFile formFile)
        {
            using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream).ConfigureAwait(false);
            var excelImportDtos = _excelService.Import<ImportParameterDto>(memoryStream);

            #region 验证基础数据
            if (excelImportDtos == null || !excelImportDtos.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11601));
            }

            //获取参数列表信息
            IEnumerable<ProcParameterEntity> parameterEntities = new List<ProcParameterEntity>();
            var parameterCodes = excelImportDtos.Where(x => !string.IsNullOrWhiteSpace(x.ParameterCode)).Select(x => x.ParameterCode).Distinct().ToArray();
            if (parameterCodes.Any())
            {
                var codeQuery = new ProcParametersByCodeQuery() { SiteId = _currentSite.SiteId ?? 0, Codes = parameterCodes.ToArray() };
                parameterEntities = await _procParameterRepository.GetByCodesAsync(codeQuery);
            }

            var addParameters = new List<ProcParameterEntity>();
            var updateParameters = new List<ProcParameterEntity>();
            var linkTypeEntities = new List<ProcParameterLinkTypeEntity>();

            var errorMessage = new StringBuilder("");
            var row = 0;
            foreach (var entity in excelImportDtos)
            {
                row++;
                if (string.IsNullOrWhiteSpace(entity.ParameterCode) && string.IsNullOrWhiteSpace(entity.ParameterName))
                {
                    continue;
                }

                var validFlag = true;
                if (string.IsNullOrWhiteSpace(entity.ParameterCode))
                {
                    errorMessage.Append($"参数编码{entity.ParameterCode}不能为空,");
                    validFlag = false;
                }

                if (string.IsNullOrWhiteSpace(entity.ParameterName))
                {
                    errorMessage.Append($"参数编码{entity.ParameterName}的参数名称不能为空,");
                    validFlag = false;
                }

                var parameterCode = entity.ParameterCode.ToTrimSpace().ToUpperInvariant();
                if (!validFlag)
                {
                    continue;
                }

                var parameterEntity = parameterEntities?.FirstOrDefault(x => x.ParameterCode == parameterCode);
                var parameterId = parameterEntity?.Id ?? 0;
                if (parameterEntity == null)
                {
                    parameterId = IdGenProvider.Instance.CreateId();
                    addParameters.Add(new ProcParameterEntity
                    {
                        Id = parameterId,
                        ParameterCode = parameterCode,
                        ParameterName = entity.ParameterName.Trim(),
                        ParameterUnit = entity.ParameterUnit,
                        DataType = entity.DataType,
                        Remark = entity.Remark ?? "",
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName,
                        SiteId = _currentSite.SiteId ?? 0
                    });
                }
                else
                {
                    parameterEntity.ParameterName = entity.ParameterName.Trim();
                    parameterEntity.ParameterUnit = entity.ParameterUnit;
                    parameterEntity.DataType = entity.DataType;
                    parameterEntity.Remark = entity.Remark;
                    parameterEntity.UpdatedBy = _currentUser.UserName;
                    parameterEntity.UpdatedOn = HymsonClock.Now();
                    updateParameters.Add(parameterEntity);
                }

                //环境参数
                if (entity.IsEnvironment.HasValue && entity.IsEnvironment == TrueOrFalseEnum.Yes)
                {
                    linkTypeEntities.Add(new ProcParameterLinkTypeEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        ParameterID = parameterId,
                        ParameterType = ParameterTypeEnum.Environment,
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName,
                        SiteId = _currentSite.SiteId ?? 0
                    });
                }

                //设备参数
                if (entity.IsEquipment.HasValue && entity.IsEquipment == TrueOrFalseEnum.Yes)
                {
                    linkTypeEntities.Add(new ProcParameterLinkTypeEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        ParameterID = parameterId,
                        ParameterType = ParameterTypeEnum.Equipment,
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName,
                        SiteId = _currentSite.SiteId ?? 0
                    });
                }

                //品质参数
                if (entity.IsIQC.HasValue && entity.IsIQC == TrueOrFalseEnum.Yes)
                {
                    linkTypeEntities.Add(new ProcParameterLinkTypeEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        ParameterID = parameterId,
                        ParameterType = ParameterTypeEnum.IQC,
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName,
                        SiteId = _currentSite.SiteId ?? 0
                    });
                }

                //产品参数
                if (entity.IsIQC.HasValue && entity.IsIQC == TrueOrFalseEnum.Yes)
                {
                    linkTypeEntities.Add(new ProcParameterLinkTypeEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        ParameterID = parameterId,
                        ParameterType = ParameterTypeEnum.Product,
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName,
                        SiteId = _currentSite.SiteId ?? 0
                    });
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
                if (addParameters.Any())
                {
                    await _procParameterRepository.InsertsAsync(addParameters);
                }
                if (updateParameters.Any())
                {
                    await _procParameterRepository.UpdatesAsync(updateParameters);
                }

                if (linkTypeEntities.Any())
                {
                    await _parameterLinkTypeRepository.InsertsAsync(linkTypeEntities);
                }
                ts.Complete();
            }
            #endregion
        }

        /// <summary>
        /// 下载物料导入模板
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public async Task DownloadMaterialTemplateAsync(Stream stream)
        {
            var excelTemplateDtos = new List<ImportMaterialDto>();
            await _excelService.ExportAsync(excelTemplateDtos, stream, "物料导入模板");
        }

        /// <summary>
        /// 下载参数导入模板
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public async Task DownloadParameterTemplateAsync(Stream stream)
        {
            var excelTemplateDtos = new List<ImportParameterDto>();
            await _excelService.ExportAsync(excelTemplateDtos, stream, "参数导入模板");
        }
    }
}
