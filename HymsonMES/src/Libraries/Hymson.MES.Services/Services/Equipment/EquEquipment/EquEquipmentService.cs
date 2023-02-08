using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentLinkApi;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentUnit.Query;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.Snowflake;
using System.Data.SqlTypes;

namespace Hymson.MES.Services.Services.Equipment.EquEquipment
{
    /// <summary>
    /// 业务处理层（设备注册）
    /// @author Czhipu
    /// @date 2022-11-08
    /// </summary>
    public class EquEquipmentService : IEquEquipmentService
    {
        /// <summary>
        /// 仓储（设备注册）
        /// </summary>
        private readonly EquEquipmentRepository _equEquipmentRepository;

        /// <summary>
        /// 仓储（设备关联API）
        /// </summary>
        private readonly EquEquipmentLinkApiRepository _equEquipmentLinkApiRepository;

        /// <summary>
        /// 仓储（设备关联硬件）
        /// </summary>
        private readonly EquEquipmentLinkHardwareRepository _equEquipmentLinkHardwareRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="equEquipmentLinkApiRepository"></param>
        /// <param name="equEquipmentLinkHardwareRepository"></param>
        public EquEquipmentService(EquEquipmentRepository repository,
            EquEquipmentLinkApiRepository equEquipmentLinkApiRepository,
            EquEquipmentLinkHardwareRepository equEquipmentLinkHardwareRepository)
        {
            _equEquipmentRepository = repository;
            _equEquipmentLinkApiRepository = equEquipmentLinkApiRepository;
            _equEquipmentLinkHardwareRepository = equEquipmentLinkHardwareRepository;
        }


        /// <summary>
        /// 添加（设备注册）
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        public async Task<int> CreateEquEquipmentAsync(EquEquipmentCreateDto createDto)
        {
            #region 参数处理
            if (string.IsNullOrEmpty(createDto.EntryDate) == true) createDto.EntryDate = SqlDateTime.MinValue.Value.ToString();

            // DTO转换实体
            var entity = createDto.ToEntity<EquEquipmentEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = "TODO";
            entity.UpdatedBy = "TODO";
            entity.EquipmentCode = createDto.EquipmentCode.ToUpper();

            if (entity.QualTime > 0 && entity.EntryDate > SqlDateTime.MinValue.Value) entity.ExpireDate = entity.EntryDate.AddMonths(entity.QualTime);

            // 绑定硬件
            List<EquEquipmentLinkHardwareEntity> linkHardwareList = new();
            if (createDto.HardwareLinks != null && createDto.HardwareLinks.Any() == true)
            {
                foreach (var item in createDto.HardwareLinks)
                {
                    EquEquipmentLinkHardwareEntity linkHardware = item.ToEntity<EquEquipmentLinkHardwareEntity>();
                    linkHardware.EquipmentId = entity.Id;
                    linkHardwareList.Add(linkHardware);
                }
            }

            // 绑定Api
            List<EquEquipmentLinkApiEntity> linkApiList = new();
            if (createDto.ApiLinks != null && createDto.ApiLinks.Any() == true)
            {
                foreach (var item in createDto.ApiLinks)
                {
                    EquEquipmentLinkApiEntity linkApi = item.ToEntity<EquEquipmentLinkApiEntity>();
                    linkApi.EquipmentId = entity.Id;
                    linkApiList.Add(linkApi);
                }
            }
            #endregion

            #region 参数校验
            // 判断编号是否已存在
            var isExists = await _equEquipmentRepository.ExistsAsync(entity.EquipmentCode);
            if (isExists == true)
            {
                // TODO 返回值
                return -1;
                //responseDto.Msg = $"此编码{model.EquipmentCode}在系统已经存在！";
                //return responseDto;
            }
            #endregion

            // TODO 加事务
            var rows = 0;
            rows += await _equEquipmentRepository.InsertAsync(entity);
            //rows += await _equEquipmentLinkHardwareRepository.InsertAsync(linkHardwareList);
            //rows += await _equEquipmentLinkApiRepository.InsertAsync(linkApiList);

            return rows;
        }

        /// <summary>
        /// 修改（设备注册）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyEquEquipmentAsync(EquEquipmentModifyDto modifyDto)
        {
            #region 参数处理
            if (string.IsNullOrEmpty(modifyDto.EntryDate) == true) modifyDto.EntryDate = SqlDateTime.MinValue.Value.ToString();

            // DTO转换实体
            var entity = modifyDto.ToEntity<EquEquipmentEntity>();
            entity.UpdatedBy = "TODO";

            if (entity.QualTime > 0 && entity.EntryDate > SqlDateTime.MinValue.Value) entity.ExpireDate = entity.EntryDate.AddMonths(entity.QualTime);

            //绑定硬件
            List<EquEquipmentLinkHardwareEntity> addLinkHardwares = new();
            List<EquEquipmentLinkHardwareEntity> updateLinkHardwares = new();
            List<long> deleteLinkHardwareIds = new();
            if (modifyDto.HardwareLinks != null && modifyDto.HardwareLinks.Any() == true)
            {
                foreach (var item in modifyDto.HardwareLinks)
                {
                    EquEquipmentLinkHardwareEntity linkHardware = new();
                    switch (item.OperationType)
                    {
                        case 1:
                            linkHardware = item.ToEntity<EquEquipmentLinkHardwareEntity>();
                            linkHardware.EquipmentId = modifyDto.Id;
                            addLinkHardwares.Add(linkHardware);
                            break;
                        case 2:
                            linkHardware = item.ToEntity<EquEquipmentLinkHardwareEntity>();
                            updateLinkHardwares.Add(linkHardware);
                            break;
                        case 3:
                            if (item.Id != null && item.Id > 0)
                            {
                                deleteLinkHardwareIds.Add(item.Id ?? 0);
                            }
                            break;
                            //default:
                            //    return Error(ResultCode.PARAM_ERROR, $"设备绑定硬件操作类型OperationType:{item.OperationType}异常，只能传入1，2，3！");
                    }

                }
            }

            // 绑定Api
            List<EquEquipmentLinkApiEntity> addLinkApis = new();
            List<EquEquipmentLinkApiEntity> updateLinkApis = new();
            List<long> deleteLinkApiIds = new();
            if (modifyDto.ApiLinks != null && modifyDto.ApiLinks.Any() == true)
            {
                foreach (var item in modifyDto.ApiLinks)
                {
                    EquEquipmentLinkApiEntity linkApi = new();
                    switch (item.OperationType)
                    {
                        case 1:
                            linkApi = item.ToEntity<EquEquipmentLinkApiEntity>();
                            linkApi.EquipmentId = modifyDto.Id;
                            addLinkApis.Add(linkApi);
                            break;
                        case 2:
                            linkApi = item.ToEntity<EquEquipmentLinkApiEntity>();
                            updateLinkApis.Add(linkApi);
                            break;
                        case 3:
                            if (item.Id != null && item.Id > 0)
                            {
                                deleteLinkApiIds.Add(item.Id ?? 0);
                            }
                            break;
                            //default:
                            //    return Error(ResultCode.PARAM_ERROR, $"设备绑定API操作类型OperationType:{item.OperationType}异常，只能传入1，2，3！");
                    }

                }
            }

            #endregion

            #region 参数校验
            var modelOrigin = await _equEquipmentRepository.GetByIdAsync(entity.Id);
            if (modelOrigin == null)
            {
                // TODO 返回值
                return -1;
                //responseDto.Msg = "此设备不存在！";
                //return responseDto;
            }
            #endregion

            // TODO 加事务
            var rows = 0;
            rows += await _equEquipmentRepository.UpdateAsync(entity);

            // 绑定硬件数据
            //await _equEquipmentLinkHardwareRepository.InsertRangeAsync(addLinkHardwares);
            //await _equEquipmentLinkHardwareRepository.UpdateRangeAsync(addLinkHardwares);
            //await _equEquipmentLinkHardwareRepository.SoftDeleteAsync(addLinkHardwares);

            // 绑定API数据
            // TODO

            return rows;
        }

        /// <summary>
        /// 删除（设备注册）
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeleteEquEquipmentAsync(long[] idsArr)
        {
            var rows = 0;
            rows += await _equEquipmentRepository.DeleteAsync(idsArr);
            //await _equEquipmentLinkHardwareRepository.SoftDeleteAsync((x => idsArr.Contains(x.EquipmentId) && !x.IsDeleted), App.GetName());
            //await _equEquipmentLinkApiRepository.SoftDeleteAsync((x => idsArr.Contains(x.EquipmentId) && !x.IsDeleted), App.GetName());
            // TODO
            return rows;
        }

        /// <summary>
        /// 分页查询列表（设备注册）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquEquipmentDto>> GetPagedListAsync(EquEquipmentPagedQueryDto pagedQueryDto)
        {
            /*
            var predicate = Expressionable.Create<EquEquipment, InteWorkCenter>();
            predicate = predicate.And((g, o) => g.SiteCode == App.GetSite());
            predicate = predicate.AndIF(!string.IsNullOrEmpty(pagedQueryDto.EquipmentCode), (g, o) => g.EquipmentCode.Contains(pagedQueryDto.EquipmentCode));
            predicate = predicate.AndIF(!string.IsNullOrEmpty(pagedQueryDto.EquipmentName), (g, o) => g.EquipmentName.Contains(pagedQueryDto.EquipmentName));
            predicate = predicate.AndIF(!string.IsNullOrEmpty(pagedQueryDto.Location), (g, o) => g.Location.Contains(pagedQueryDto.Location));

            // TODO 这里暂时用名字替代
            predicate = predicate.AndIF(!string.IsNullOrEmpty(pagedQueryDto.EquipmentType), (g, o) => g.EquipmentType == pagedQueryDto.EquipmentType);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(pagedQueryDto.UseDepartment), (g, o) => g.UseDepartment.Contains(pagedQueryDto.UseDepartment));
            predicate = predicate.AndIF(!string.IsNullOrEmpty(pagedQueryDto.WorkCenterShopName), (g, o) => o.Name.Contains(pagedQueryDto.WorkCenterShopName));
            predicate = predicate.AndIF(pagedQueryDto.UseStatus != null, (g, o) => g.UseStatus == pagedQueryDto.UseStatus);

            var response = await _equEquipmentRepository.Queryable()
                .LeftJoin<InteWorkCenter>((g, o) => g.WorkCenterShopId == o.Id)
                .Where(predicate.ToExpression())
                .Select((g, o) => new CustomEquEquipmentListDto
                {
                    Id = g.Id,
                    SiteCode = g.SiteCode,
                    EquipmentCode = g.EquipmentCode,
                    EquipmentName = g.EquipmentName,
                    EquipmentType = g.EquipmentType,
                    EquipmentDesc = g.EquipmentDesc,
                    UseDepartment = g.UseDepartment,
                    UseStatus = g.UseStatus,
                    WorkCenterShopName = o.Name,
                    Location = g.Location,
                    CreateBy = g.CreateBy,
                    CreateOn = g.CreateOn,
                    UpdateBy = g.UpdateBy,
                    UpdateOn = g.UpdateOn
                })
                 .ToPageAsync(pagedQueryDto);

            return response;
            */

            var pagedQuery = pagedQueryDto.ToQuery<EquEquipmentPagedQuery>();
            var pagedInfo = await _equEquipmentRepository.GetPagedInfoAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            List<EquEquipmentDto> equipmentDtos = PrepareEquEquipmentDtos(pagedInfo);
            return new PagedInfo<EquEquipmentDto>(equipmentDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 查询设备维护表列表
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquEquipmentDto>> GetListAsync(EquEquipmentPagedQueryDto pagedQueryDto)
        {
            /*
            //开始拼装查询条件
            var predicate = Expressionable.Create<EquEquipment>();
            predicate = predicate.And(g => g.SiteCode == parm.SiteCode && g.IsDeleted == false);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.EquipmentCode), g => g.EquipmentCode.Contains(parm.EquipmentCode));
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.EquipmentName), g => g.EquipmentCode.Contains(parm.EquipmentName));

            //搜索条件查询语法参考Sqlsugar
            var response = await _equEquipmentRepository
              .Queryable()
              .OrderByDescending(g => g.CreateOn)
              .Where(predicate.ToExpression())
              .ToPageAsync(parm);

            return response;
            */

            var pagedQuery = pagedQueryDto.ToQuery<EquEquipmentPagedQuery>();
            var pagedInfo = await _equEquipmentRepository.GetPagedInfoAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            List<EquEquipmentDto> equipmentDtos = PrepareEquEquipmentDtos(pagedInfo);
            return new PagedInfo<EquEquipmentDto>(equipmentDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 查询列表（设备注册）
        /// </summary>
        /// <returns></returns>
        public async Task<List<EquEquipmentDictionaryDto>> QueryEquEquipmentDictionaryAsync()
        {
            /*
            var predicate = Expressionable.Create<EquEquipment>();
            predicate = predicate.And(g => g.SiteCode == App.GetSite());

            var list = await _equEquipmentRepository.Queryable()
                .Where(predicate.ToExpression())
                .Select(g => new EquEquipment
                {
                    Id = g.Id,
                    EquipmentCode = g.EquipmentCode,
                    EquipmentName = g.EquipmentName,
                    EquipmentType = g.EquipmentType
                })
                 .ToListAsync();

            var dics = new List<EquEquipmentDictionaryDto> { };
            var listGroup = list.GroupBy(g => g.EquipmentType);
            foreach (var item in listGroup)
            {
                dics.Add(new EquEquipmentDictionaryDto
                {
                    EquipmentType = item.Key,
                    Equipments = item.Select(s => new EquEquipmentDictionaryValueDto
                    {
                        Id = s.Id,
                        EquipmentCode = s.EquipmentCode,
                        EquipmentName = s.EquipmentName
                    }).ToList()
                });
            }

            return dics;
            */

            // TODO 
            return null;
        }

        /// <summary>
        /// 查询详情（设备注册）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<CustomEquEquipmentDetailDto> GetCustomEquEquipmentAsync(long id)
        {
            /*
            var model = await _equEquipmentRepository.Queryable()
                .LeftJoin<EquEquipmentGroup>((g, o) => g.EquipmentGroupId == o.Id)
                .Where((g, o) => g.SiteCode == App.GetSite() && g.Id == id)
                .Select((g, o) => new CustomEquEquipmentDetailDto
                {
                    Id = g.Id,
                    EquipmentCode = g.EquipmentCode,
                    EquipmentName = g.EquipmentName,
                    EquipmentGroupId = g.EquipmentGroupId,
                    EquipmentGroupName = o.EquipmentGroupName,
                    EquipmentDesc = g.EquipmentDesc,
                    WorkCenterFactoryId = g.WorkCenterFactoryId,
                    WorkCenterShopId = g.WorkCenterShopId,
                    WorkCenterLineId = g.WorkCenterLineId,
                    Location = g.Location,
                    EquipmentType = g.EquipmentType,
                    UseDepartment = g.UseDepartment,
                    EntryDate = g.EntryDate,
                    QualTime = g.QualTime,
                    ExpireDate = g.ExpireDate,
                    Manufacturer = g.Manufacturer,
                    Supplier = g.Supplier,
                    UseStatus = g.UseStatus,
                    Power = g.Power,
                    EnergyLevel = g.EnergyLevel,
                    Ip = g.Ip,
                    CreateBy = g.CreateBy,
                    CreateOn = g.CreateOn,
                    UpdateBy = g.UpdateBy,
                    UpdateOn = g.UpdateOn,
                    Remark = g.Remark,
                    TaktTime = g.TaktTime
                })
                .FirstAsync();

            return model;
            */

            // TODO 
            return null;
        }

        /// <summary>
        /// 查询设备（单个）
        /// </summary>
        /// <param name="equipmentCode">设备编码</param>
        /// <param name="siteCode">站点</param>
        /// <returns></returns>
        public async Task<EquEquipmentEntity> QueryEquEquipmentAsync(string equipmentCode, string siteCode)
        {
            /*
            EquEquipment model = new EquEquipment();
            try
            {
                model = await _equEquipmentRepository.Queryable()
                    .Where(m => m.EquipmentCode == equipmentCode.ToUpper() && m.SiteCode == siteCode)
                    .FirstAsync();
            }
            catch (Exception ex)
            {
                throw new CustomException(ConstEqu.EQU_30001, $"设备表根据设备编码+工厂查询异常：{ex}");
            }

            return model;
            */
            // TODO 
            return null;
        }

        /// <summary>
        /// 查询设备关联API列表
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquEquipmentLinkApiDto>> GetEquimentLinkApiAsync(EquEquipmentLinkApiPagedQueryDto pagedQueryDto)
        {
            /*
            //搜索条件查询语法参考Sqlsugar
            var response = await _equEquipmentLinkApiRepository.Queryable()
                 .OrderByDescending(x => x.UpdateOn)
                 .Where((x) => x.EquipmentId == parm.EquipmentId && !x.IsDeleted)
                 .Select(x => new QueryEquipmentLinkApiDto
                 {
                     Id = x.Id,
                     SiteCode = x.SiteCode,
                     EquipmentId = x.EquipmentId,
                     ApiUrl = x.ApiUrl,
                     ApiType = x.ApiType,
                     Remark = x.Remark,
                     CreateBy = x.CreateBy,
                     CreateOn = x.CreateOn,
                     UpdateBy = x.UpdateBy,
                     UpdateOn = x.UpdateOn
                 })
                 .ToPageAsync(parm);

            return response;
            */

            // TODO 
            var pagedQuery = pagedQueryDto.ToQuery<EquEquipmentLinkApiPagedQuery>();
            var pagedInfo = await _equEquipmentLinkApiRepository.GetPagedInfoAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            List<EquEquipmentLinkApiDto> equipmentDtos = PrepareEquEquipmentLinkApiDtos(pagedInfo);
            return new PagedInfo<EquEquipmentLinkApiDto>(equipmentDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 根据设备id+接口类型获取接口地址
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <param name="apiType"></param>
        /// <returns></returns>
        public async Task<EquEquipmentLinkApiEntity> GetApiForEquipmentidAndType(long equipmentId, string apiType)
        {
            /*
            var response = await _equEquipmentLinkApiRepository.Queryable()
                 .OrderByDescending(x => x.UpdateOn)
                 .Where((x) => x.EquipmentId == equipmentId && x.ApiType == apiType && !x.IsDeleted)
                 .Select(x => new QueryEquipmentLinkApiDto
                 {
                     Id = x.Id,
                     SiteCode = x.SiteCode,
                     EquipmentId = x.EquipmentId,
                     ApiUrl = x.ApiUrl,
                     ApiType = x.ApiType,
                     Remark = x.Remark,
                     CreateBy = x.CreateBy,
                     CreateOn = x.CreateOn,
                     UpdateBy = x.UpdateBy,
                     UpdateOn = x.UpdateOn
                 })
                 .FirstAsync();

            return response;
            */

            // TODO 
            return null;
        }

        /// <summary>
        /// 查询设备关联硬件列表
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquEquipmentLinkHardwareDto>> GetEquimentLinkHardwareAsync(EquEquipmentLinkHardwarePagedQueryDto pagedQueryDto)
        {
            /*
            //搜索条件查询语法参考Sqlsugar
            var response = await _equEquipmentLinkHardwareRepository.Queryable()
                 .OrderByDescending(x => x.UpdateOn)
                 .Where((x) => x.EquipmentId == parm.EquipmentId && !x.IsDeleted)
                 .Select(x => new QueryEquipmentLinkHardwareDto
                 {
                     Id = x.Id,
                     SiteCode = x.SiteCode,
                     EquipmentId = x.EquipmentId,
                     HardwareCode = x.HardwareCode,
                     HardwareType = x.HardwareType,
                     Remark = x.Remark,
                     CreateBy = x.CreateBy,
                     CreateOn = x.CreateOn,
                     UpdateBy = x.UpdateBy,
                     UpdateOn = x.UpdateOn
                 })
                 .ToPageAsync(parm);

            return response;
            */

            // TODO 
            var pagedQuery = pagedQueryDto.ToQuery<EquEquipmentLinkHardwarePagedQuery>();
            var pagedInfo = await _equEquipmentLinkHardwareRepository.GetPagedInfoAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            List<EquEquipmentLinkHardwareDto> equipmentDtos = PrepareEquEquipmentLinkHardwareDtos(pagedInfo);
            return new PagedInfo<EquEquipmentLinkHardwareDto>(equipmentDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 根据硬件编码硬件类型获取设备
        /// </summary>
        /// <param name="HardwareCode"></param>
        /// <param name="HardwareType"></param>
        /// <returns></returns>
        public async Task<EquEquipmentLinkHardwareEntity> GetLinkHardwareForCodeAndTypeAsync(string HardwareCode, string HardwareType)
        {
            /*
            var model = await _equEquipmentLinkHardwareRepository.Queryable()
                .Where(m => m.HardwareCode == HardwareCode && m.HardwareType == HardwareType)
                .Select(x => new EquEquipmentLinkHardware
                {
                    Id = x.Id,
                    SiteCode = x.SiteCode,
                    EquipmentId = x.EquipmentId,
                    HardwareCode = x.HardwareCode,
                    HardwareType = x.HardwareType,
                    Remark = x.Remark,
                    CreateBy = x.CreateBy,
                    CreateOn = x.CreateOn,
                    UpdateBy = x.UpdateBy,
                    UpdateOn = x.UpdateOn
                }).FirstAsync();
            return model;
            */

            // TODO 
            return null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<EquEquipmentDto> PrepareEquEquipmentDtos(PagedInfo<EquEquipmentEntity> pagedInfo)
        {
            var dtos = new List<EquEquipmentDto>();
            foreach (var entity in pagedInfo.Data)
            {
                var dto = entity.ToModel<EquEquipmentDto>();
                dtos.Add(dto);
            }

            return dtos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<EquEquipmentLinkApiDto> PrepareEquEquipmentLinkApiDtos(PagedInfo<EquEquipmentLinkApiEntity> pagedInfo)
        {
            var dtos = new List<EquEquipmentLinkApiDto>();
            foreach (var entity in pagedInfo.Data)
            {
                var dto = entity.ToModel<EquEquipmentLinkApiDto>();
                dtos.Add(dto);
            }

            return dtos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<EquEquipmentLinkHardwareDto> PrepareEquEquipmentLinkHardwareDtos(PagedInfo<EquEquipmentLinkHardwareEntity> pagedInfo)
        {
            var dtos = new List<EquEquipmentLinkHardwareDto>();
            foreach (var entity in pagedInfo.Data)
            {
                var dto = entity.ToModel<EquEquipmentLinkHardwareDto>();
                dtos.Add(dto);
            }

            return dtos;
        }
    }
}