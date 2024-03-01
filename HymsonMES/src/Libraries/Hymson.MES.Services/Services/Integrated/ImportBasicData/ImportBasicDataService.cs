using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Excel.Abstractions;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.Minio;
using Hymson.Utils.Tools;
using Microsoft.AspNetCore.Http;
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

        private readonly IExcelService _excelService;
        private readonly IMinioService _minioService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ImportBasicDataService(ICurrentUser currentUser, ICurrentSite currentSite,
               IEquEquipmentRepository equipmentRepository, IExcelService excelService, IMinioService minioService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _equipmentRepository = equipmentRepository;
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
            var excelImportDtos = _excelService.Import<InteCustomImportDto>(memoryStream);
            //备份用户上传的文件，可选
            var stream = formFile.OpenReadStream();
            var uploadResult = await _minioService.PutObjectAsync(formFile.FileName, stream, formFile.ContentType);
            if (excelImportDtos == null || !excelImportDtos.Any())
            {
                throw new CustomerValidationException("导入数据为空");
            }

            #region 验证基础数据

            //读取部门数据
            #endregion

            var addEquipments = new List<EquEquipmentEntity>();

            #region 入库
            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {

                //保存记录 
                if (addEquipments.Any())
                {
                   // await _equipmentRepository.InsertAsync(addEquipments);
                }
               
                ts.Complete();
            }
            #endregion
        }


    }
}
