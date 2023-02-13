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
        private readonly IEquEquipmentRepository _equEquipmentRepository;

        /// <summary>
        /// 仓储（设备关联API）
        /// </summary>
        private readonly IEquEquipmentLinkApiRepository _equEquipmentLinkApiRepository;

        /// <summary>
        /// 仓储（设备关联硬件）
        /// </summary>
        private readonly IEquEquipmentLinkHardwareRepository _equEquipmentLinkHardwareRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="equEquipmentLinkApiRepository"></param>
        /// <param name="equEquipmentLinkHardwareRepository"></param>
        public EquEquipmentService(IEquEquipmentRepository repository,
            IEquEquipmentLinkApiRepository equEquipmentLinkApiRepository,
            IEquEquipmentLinkHardwareRepository equEquipmentLinkHardwareRepository)
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
            var isExists = await _equEquipmentRepository.IsExistsAsync(entity.EquipmentCode);
            if (isExists == true)
            {
                // TODO 返回值
                return -1;
                //responseDto.Msg = $"此编码{model.EquipmentCode}在系统已经存在！";
                //return responseDto;
            }
            #endregion

            // TODO 事务处理
            var rows = 0;

            rows += await _equEquipmentRepository.InsertAsync(entity);
            rows += await _equEquipmentLinkApiRepository.InsertRangeAsync(linkApiList);
            rows += await _equEquipmentLinkHardwareRepository.InsertRangeAsync(linkHardwareList);

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
                    EquEquipmentLinkHardwareEntity linkHardware;
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
                            if (item.Id != null && item.Id > 0) deleteLinkHardwareIds.Add(item.Id ?? 0);
                            break;
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
                    EquEquipmentLinkApiEntity linkApi;
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
                            if (item.Id != null && item.Id > 0) deleteLinkApiIds.Add(item.Id ?? 0);
                            break;
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

            var rows = 0;

            // TODO 事务处理
            // TODO 需要检查更新的字段
            rows += await _equEquipmentRepository.UpdateAsync(entity);

            // 绑定API数据
            await _equEquipmentLinkApiRepository.SoftDeleteAsync(deleteLinkApiIds);
            await _equEquipmentLinkApiRepository.UpdateRangeAsync(updateLinkApis);
            await _equEquipmentLinkApiRepository.InsertRangeAsync(addLinkApis);


            // 绑定硬件数据
            await _equEquipmentLinkHardwareRepository.SoftDeleteAsync(deleteLinkHardwareIds);
            await _equEquipmentLinkHardwareRepository.UpdateRangeAsync(updateLinkHardwares);
            await _equEquipmentLinkHardwareRepository.InsertRangeAsync(addLinkHardwares);

            return rows;
        }

        /// <summary>
        /// 删除（设备注册）
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeleteEquEquipmentAsync(long[] idsArr)
        {
            // TODO 事务处理
            var rows = 0;
            rows += await _equEquipmentRepository.SoftDeleteAsync(idsArr);
            await _equEquipmentLinkApiRepository.SoftDeleteAsync(idsArr);
            await _equEquipmentLinkHardwareRepository.SoftDeleteAsync(idsArr);

            return rows;
        }

        /// <summary>
        /// 分页查询列表（设备注册）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquEquipmentListDto>> GetPagedListAsync(EquEquipmentPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<EquEquipmentPagedQuery>();
            var pagedInfo = await _equEquipmentRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<EquEquipmentListDto>());
            return new PagedInfo<EquEquipmentListDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 查询列表（设备注册）
        /// </summary>
        /// <returns></returns>
        public async Task<List<EquEquipmentDictionaryDto>> GetEquEquipmentDictionaryAsync()
        {
            var dics = new List<EquEquipmentDictionaryDto> { };
            var list = await _equEquipmentRepository.GetBaseListAsync();
            var equipmentTypeDic = list.ToLookup(g => g.EquipmentType);
            foreach (var item in equipmentTypeDic)
            {
                dics.Add(new EquEquipmentDictionaryDto
                {
                    EquipmentType = item.Key,
                    Equipments = item.Select(s => new EquEquipmentBaseDto
                    {
                        Id = s.Id,
                        EquipmentCode = s.EquipmentCode,
                        EquipmentName = s.EquipmentName
                    })
                });
            }

            return dics;
        }

        /// <summary>
        /// 查询详情（设备注册）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquEquipmentDto> GetEquEquipmentWithGroupNameAsync(long id)
        {
            return (await _equEquipmentRepository.GetViewByIdAsync(id)).ToModel<EquEquipmentDto>();
        }

        /// <summary>
        /// 查询设备关联API列表
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquEquipmentLinkApiBaseDto>> GetEquimentLinkApiAsync(EquEquipmentLinkApiPagedQueryDto pagedQueryDto)
        {
            /*
            //搜索条件查询语法参考Sqlsugar
            var response = await _equEquipmentLinkApiRepository.Queryable()
                 .OrderByDescending(x => x.UpdatedOn)
                 .Where((x) => x.EquipmentId == parm.EquipmentId && !x.IsDeleted)
                 .Select(x => new QueryEquipmentLinkApiDto
                 {
                     Id = x.Id,
                     SiteCode = x.SiteCode,
                     EquipmentId = x.EquipmentId,
                     ApiUrl = x.ApiUrl,
                     ApiType = x.ApiType,
                     Remark = x.Remark,
                     CreatedBy = x.CreatedBy,
                     CreatedOn = x.CreatedOn,
                     UpdatedBy = x.UpdatedBy,
                     UpdatedOn = x.UpdatedOn
                 })
                 .ToPageAsync(parm);

            return response;
            */

            // TODO 
            var pagedQuery = pagedQueryDto.ToQuery<EquEquipmentLinkApiPagedQuery>();
            var pagedInfo = await _equEquipmentLinkApiRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<EquEquipmentLinkApiBaseDto>());
            return new PagedInfo<EquEquipmentLinkApiBaseDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 查询设备关联硬件列表
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquEquipmentLinkHardwareBaseDto>> GetEquimentLinkHardwareAsync(EquEquipmentLinkHardwarePagedQueryDto pagedQueryDto)
        {
            /*
            //搜索条件查询语法参考Sqlsugar
            var response = await _equEquipmentLinkHardwareRepository.Queryable()
                 .OrderByDescending(x => x.UpdatedOn)
                 .Where((x) => x.EquipmentId == parm.EquipmentId && !x.IsDeleted)
                 .Select(x => new QueryEquipmentLinkHardwareDto
                 {
                     Id = x.Id,
                     SiteCode = x.SiteCode,
                     EquipmentId = x.EquipmentId,
                     HardwareCode = x.HardwareCode,
                     HardwareType = x.HardwareType,
                     Remark = x.Remark,
                     CreatedBy = x.CreatedBy,
                     CreatedOn = x.CreatedOn,
                     UpdatedBy = x.UpdatedBy,
                     UpdatedOn = x.UpdatedOn
                 })
                 .ToPageAsync(parm);

            return response;
            */

            // TODO 
            var pagedQuery = pagedQueryDto.ToQuery<EquEquipmentLinkHardwarePagedQuery>();
            var pagedInfo = await _equEquipmentLinkHardwareRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<EquEquipmentLinkHardwareBaseDto>());
            return new PagedInfo<EquEquipmentLinkHardwareBaseDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }




        #region 这里是供其他业务层调用的方法，个人觉得应该直接在其他业务层调用各业务仓储层
        /// <summary>
        /// 查询设备（单个）
        /// </summary>
        /// <param name="equipmentCode">设备编码</param>
        /// <param name="siteCode">站点</param>
        /// <returns></returns>
        public async Task<EquEquipmentDto> GetByEquipmentCodeAsync(string equipmentCode, string siteCode)
        {
            return (await _equEquipmentRepository.GetByEquipmentCodeAsync(equipmentCode.ToUpper())).ToModel<EquEquipmentDto>();
        }

        /// <summary>
        /// 根据设备id+接口类型获取接口地址
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <param name="apiType"></param>
        /// <returns></returns>
        public async Task<EquEquipmentLinkApiDto> GetApiForEquipmentidAndType(long equipmentId, string apiType)
        {
            return (await _equEquipmentLinkApiRepository.GetByEquipmentIdAsync(equipmentId, apiType)).ToModel<EquEquipmentLinkApiDto>();
        }

        /// <summary>
        /// 根据硬件编码硬件类型获取设备
        /// </summary>
        /// <param name="hardwareCode"></param>
        /// <param name="hardwareType"></param>
        /// <returns></returns>
        public async Task<EquEquipmentLinkHardwareDto> GetLinkHardwareForCodeAndTypeAsync(string hardwareCode, string hardwareType)
        {
            return (await _equEquipmentLinkHardwareRepository.GetByHardwareCodeAsync(hardwareCode, hardwareType)).ToModel<EquEquipmentLinkHardwareDto>();
        }
        #endregion



    }
}