using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Excel.Abstractions;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentGroup;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentGroup.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.Minio;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Transactions;

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

        private readonly IExcelService _excelService;
        private readonly IMinioService _minioService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ImportBasicDataService(ICurrentUser currentUser, ICurrentSite currentSite,
               IEquEquipmentRepository equipmentRepository, IEquEquipmentGroupRepository equipmentGroupRepository,
               IExcelService excelService, IMinioService minioService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _equipmentRepository = equipmentRepository;
            _equipmentGroupRepository = equipmentGroupRepository;
            _excelService = excelService;
            _minioService = minioService;
        }

        /// <summary>
        /// 基础数据导入
        /// </summary>
        /// <returns></returns>
        public async Task ImportDataAsync(IFormFile formFile)
        {
            using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream).ConfigureAwait(false);
            var excelImportDtos = _excelService.Import<ImportEquipmentDto>(memoryStream);
            //备份用户上传的文件，可选
            var stream = formFile.OpenReadStream();
            var uploadResult = await _minioService.PutObjectAsync(formFile.FileName, stream, formFile.ContentType);

            #region 验证基础数据
            if (excelImportDtos == null || !excelImportDtos.Any())
            {
                throw new CustomerValidationException("导入数据为空");
            }

            //设备编码、名称、存放位置、使用状态不能为空
            if (excelImportDtos.Any(x => string.IsNullOrWhiteSpace(x.EquipmentCode.Trim())))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11602));
            }
            if (excelImportDtos.Any(x => string.IsNullOrWhiteSpace(x.EquipmentName.Trim())))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11603));
            }
            if (excelImportDtos.Any(x => string.IsNullOrWhiteSpace(x.Location.Trim())))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11604));
            }
            if (excelImportDtos.Any(x => string.IsNullOrWhiteSpace(x.UseStatus.Trim())))
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
                var groupQuery = new EquEquipmentGroupQuery() { SiteId = _currentSite.SiteId ?? 0, EquipmentGroupCodes = equCodes };
                groupEntities = await _equipmentGroupRepository.GetEntitiesAsync(groupQuery);
            }

            var errorMessage = new StringBuilder("");
            var row = 1;
            foreach (var entity in excelImportDtos)
            {
                var equGroupId = 0;
                if (!string.IsNullOrWhiteSpace(entity.EquipmentCode))
                {
                    var group= groupEntities.FirstOrDefault(x=>x.EquipmentGroupCode== entity.EquipmentCode);
                    if (group == null)
                    {
                        errorMessage.Append($"第{row}行设备组编码在系统中不存在!");
                    }
                }

                //读取部门数据,部门数据来源用户中心
                var equipmentCode = entity.EquipmentCode.ToTrimSpace();
                var equipment = equipmentEntities.FirstOrDefault(x => x.EquipmentCode == entity.EquipmentCode.ToUpperInvariant());
                if (equipment != null)
                {
                    addEquipments.Add(new EquEquipmentEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        EquipmentCode = entity.EquipmentCode.Trim(),
                        EquipmentName = entity.EquipmentName.Trim(),
                        Location = entity.Location.Trim(),
                        EquipmentGroupId = equGroupId,
                    });
                }
                else
                {

                }
                row++;
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

    }
}
