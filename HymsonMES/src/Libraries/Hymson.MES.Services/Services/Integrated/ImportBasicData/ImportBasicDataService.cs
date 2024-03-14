using Elastic.Clients.Elasticsearch;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Excel.Abstractions;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentGroup;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentGroup.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Resource;
using Hymson.MES.Data.Repositories.Process.ResourceType;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.Minio;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.AspNetCore.Http;
using Minio.DataModel;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Transactions;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;

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

        private readonly IExcelService _excelService;
        private readonly IMinioService _minioService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ImportBasicDataService(ICurrentUser currentUser, ICurrentSite currentSite,
               IEquEquipmentRepository equipmentRepository, IEquEquipmentGroupRepository equipmentGroupRepository,
               IProcResourceRepository procResourceRepository, IProcResourceTypeRepository resourceTypeRepository,
               IProcResourceEquipmentBindRepository resourceEquipmentBindRepository,
               IExcelService excelService, IMinioService minioService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _equipmentRepository = equipmentRepository;
            _equipmentGroupRepository = equipmentGroupRepository;
            _procResourceRepository = procResourceRepository;
            _resourceTypeRepository = resourceTypeRepository;
            _resourceEquipmentBindRepository = resourceEquipmentBindRepository;
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
                throw new CustomerValidationException("导入数据为空");
            }

            //设备编码、名称、存放位置、使用状态不能为空
            if (excelImportDtos.Any(x => string.IsNullOrWhiteSpace(x.EquipmentCode)))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11602));
            }
            if (excelImportDtos.Any(x => string.IsNullOrWhiteSpace(x.EquipmentName)))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11603));
            }
            if (excelImportDtos.Any(x => string.IsNullOrWhiteSpace(x.Location)))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11604));
            }
            if (excelImportDtos.Any(x => string.IsNullOrWhiteSpace(x.UseStatus)))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11605));
            }

            //重复的设备编码
            var equCodes = excelImportDtos.Select(x => x.EquipmentCode).Distinct().ToArray();
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
            var equGroupCodes = excelImportDtos.Select(x => x.EquipmentGroup).Distinct().ToArray();
            if (equGroupCodes.Any())
            {
                var groupQuery = new EquEquipmentGroupQuery() { SiteId = _currentSite.SiteId ?? 0, EquipmentGroupCodes = equGroupCodes };
                groupEntities = await _equipmentGroupRepository.GetEntitiesAsync(groupQuery);
            }

            var errorMessage = new StringBuilder("");
            var equUseStatus = Enum.GetNames<EquipmentUseStatusEnum>();
            var row = 0;
            foreach (var entity in excelImportDtos)
            {
                row++;
                //使用状态

                var sss = (EquipmentUseStatusEnum)Enum.Parse(typeof(EquipmentUseStatusEnum), entity.UseStatus);
                if (!equUseStatus.Contains(entity.UseStatus))
                {           
                    errorMessage.Append($"第{row}行使用状态值不合法,");
                }

                var validFlag = true;
                //设备类型
                if (!string.IsNullOrWhiteSpace(entity.EquipmentType))
                {
                    if (!Enum.IsDefined(typeof(EquipmentTypeEnum), entity.EquipmentType))
                    {
                        errorMessage.Append($"第{row}行设备类型值不合法,");
                        validFlag = false;
                    }
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
                errorMessage.Append(";");
                if (!validFlag)
                {
                    continue;
                }

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
                        Location = entity.Location.Trim(),
                        UseStatus = (EquipmentUseStatusEnum)Enum.Parse(typeof(EquipmentUseStatusEnum), entity.UseStatus),
                        EquipmentType = string.IsNullOrWhiteSpace(entity.EquipmentType) ? null : (EquipmentTypeEnum)Enum.Parse(typeof(EquipmentTypeEnum), entity.EquipmentType),
                        //UseDepartment=entity.UseDepartment,
                        EntryDate = string.IsNullOrWhiteSpace(entity.EntryDate) ? null : Convert.ToDateTime(entryDate),
                        QualTime = entity.QualTime,
                        Manufacturer = entity.Manufacturer ?? "",
                        Supplier = entity.Supplier ?? "",
                        EquipmentGroupId = equGroupId,
                        Power = entity.Power,
                        EnergyLevel = entity.EnergyLevel,
                        Ip = entity.Ip ?? "",
                        Remark = entity.Remark,
                        SiteId = _currentSite.SiteId ?? 0,
                        WorkCenterFactoryId = 0,
                        EquipmentDesc = entity.EquipmentDesc,
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName
                    });
                }
                else
                {
                    equipment.EquipmentName = entity.EquipmentName;
                    equipment.EquipmentName = entity.EquipmentName.Trim();
                    equipment.Location = entity.Location.Trim();
                    equipment.UseStatus = (EquipmentUseStatusEnum)Enum.Parse(typeof(EquipmentUseStatusEnum), entity.UseStatus);
                    equipment.UseStatus = (EquipmentUseStatusEnum)Enum.Parse(typeof(EquipmentUseStatusEnum), entity.UseStatus);
                    equipment.EquipmentType = string.IsNullOrWhiteSpace(entity.EquipmentType) ? null : (EquipmentTypeEnum)Enum.Parse(typeof(EquipmentTypeEnum), entity.EquipmentType);
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
            //备份用户上传的文件，可选
            //var stream = formFile.OpenReadStream();
            //var uploadResult = await _minioService.PutObjectAsync(formFile.FileName, stream, formFile.ContentType);

            #region 验证基础数据
            if (excelImportDtos == null || !excelImportDtos.Any())
            {
                throw new CustomerValidationException("导入数据为空");
            }

            //资源编码、名称不能为空
            if (excelImportDtos.Any(x => string.IsNullOrWhiteSpace(x.ResCode)))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10301));
            }
            if (excelImportDtos.Any(x => string.IsNullOrWhiteSpace(x.ResName)))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10303));
            }

            //获取资源列表信息
            IEnumerable<ProcResourceEntity> resourceEntities = new List<ProcResourceEntity>();
            var resCodes = excelImportDtos.Select(x => x.ResCode).Distinct().ToArray();
            if (resCodes.Any())
            {
                var resourceQuery = new ProcResourceQuery() { SiteId = _currentSite.SiteId ?? 0, ResCodes = resCodes };
                resourceEntities = await _procResourceRepository.GetEntitiesAsync(resourceQuery);
            }

            //获取设备列表信息
            IEnumerable<EquEquipmentEntity> equEquipmentEntities = new List<EquEquipmentEntity>();
            var equCodes = excelImportDtos.Select(x => x.EquipmentCode ?? "").Distinct().ToArray();
            if (equCodes != null && equCodes.Any())
            {
                var query = new EquEquipmentQuery() { SiteId = _currentSite.SiteId ?? 0, EquipmentCodes = equCodes };
                equEquipmentEntities = await _equipmentRepository.GetEntitiesAsync(query);
            }

            //获取资源类型列表信息
            IEnumerable<ProcResourceTypeEntity> resourceTypeEntities = new List<ProcResourceTypeEntity>();
            var resTypes = excelImportDtos.Select(x => x.ResType ?? "").Distinct().ToArray();
            if (resTypes != null && resTypes.Any())
            {
                var query = new ProcResourceTypeQuery() { SiteId = _currentSite.SiteId ?? 0, ResTypes = resTypes };
                resourceTypeEntities = await _resourceTypeRepository.GetEntitiesAsync(query);
            }

            var importResources = excelImportDtos.DistinctBy(x => x.ResCode).ToList();

            var addResources = new List<ProcResourceEntity>();
            var updateResources = new List<ProcResourceEntity>();
            var errorMessage = new StringBuilder("");
            foreach (var entity in importResources)
            {
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
                    }
                    resTypeId = resourceTypeEntity?.Id ?? 0;
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
                    resourceEntity.CreatedBy = _currentUser.UserName;
                    resourceEntity.UpdatedBy = _currentUser.UserName;
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
                //是否主设备
                if (!string.IsNullOrWhiteSpace(entity.IsMain))
                {
                    if (!Enum.IsDefined(typeof(TrueOrFalseEnum), entity.IsMain))
                    {
                        errorMessage.Append($"第{row}行是否主设备值不合法,");
                        validFlag = false;
                    }
                    isMain = (TrueOrFalseEnum)Enum.Parse(typeof(TrueOrFalseEnum), entity.IsMain) == TrueOrFalseEnum.Yes ? true : false;
                }

                errorMessage.ToString().TrimEnd(',');
                errorMessage.Append(";");
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

    }
}
