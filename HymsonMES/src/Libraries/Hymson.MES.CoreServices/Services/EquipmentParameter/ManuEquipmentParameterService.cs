using Force.Crc32;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Constants.Parameter;
using Hymson.MES.Core.Domain.Parameter;
using Hymson.MES.CoreServices.Dtos.Parameter;
using Hymson.MES.CoreServices.Options;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Parameter.ManuProductParameter;
using Hymson.Snowflake;
using Hymson.Utils.Tools;
using Microsoft.Extensions.Options;
using System.Text;

namespace Hymson.MES.CoreServices.Services.Parameter
{
    /// <summary>
    /// 参数采集
    /// </summary>
    public class ManuEquipmentParameterService : IManuEquipmentParameterService
    {
        private readonly IManuEquipmentParameterRepository _manuEquipmentParameterRepository;
        private readonly IEquEquipmentRepository _equEquipmentRepository;
        private readonly ParameterOptions _parameterOptions;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="manuEquipmentParameterRepository"></param>
        /// <param name="parameterOptions"></param>
        /// <param name="equEquipmentRepository"></param>
        public ManuEquipmentParameterService(IManuEquipmentParameterRepository  manuEquipmentParameterRepository, IOptions<ParameterOptions> parameterOptions, IEquEquipmentRepository equEquipmentRepository)
        {
            _manuEquipmentParameterRepository = manuEquipmentParameterRepository;
            _equEquipmentRepository = equEquipmentRepository;
            _parameterOptions = parameterOptions.Value;
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task InsertRangeAsync(IEnumerable<EquipmentParameterDto> param)
        {
            var dic = new Dictionary<string, List<EquipmentParameterEntity>>();

            var equipmentList = await _equEquipmentRepository.GetByIdAsync(param.Select(x => x.EquipmentId).ToList<long>());

            foreach (var paramDto in param)
            {
                var entity = paramDto.ToEntity<EquipmentParameterEntity>();
                entity.CreatedBy = paramDto.UserName;
                entity.UpdatedBy = paramDto.UserName;
                entity.CreatedOn = paramDto.Date;
                entity.UpdatedOn = paramDto.Date;
                entity.Id = IdGenProvider.Instance.CreateId();

                var equipment = equipmentList.FirstOrDefault(x => x.Id == paramDto.EquipmentId);
                if (equipment != null)
                {
                    var tableNameByEquipmentCode = GetTableNameByEquipmentCode(paramDto.SiteId, equipment.EquipmentCode);

                    if (!dic.ContainsKey(tableNameByEquipmentCode))
                    {
                        dic[tableNameByEquipmentCode] = new List<EquipmentParameterEntity>();
                    }
                    dic[tableNameByEquipmentCode].Add(entity);
                }
                else
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10476));
                }
            }

            using (var trans = TransactionHelper.GetTransactionScope())
            {
                // 插入数据
                List<Task<int>> tasks = new();
                foreach (var dicItem in dic)
                {
                    tasks.Add(_manuEquipmentParameterRepository.InsertRangeAsync(dicItem.Value, dicItem.Key));
                }
                await Task.WhenAll(tasks);
                trans.Complete();
            }
        }

        /// <summary>
        ///创建数据库表
        /// </summary>
        /// <returns></returns>
        public async Task CreateEquipmentParameterTable(string tabname)
        {
            var sql = await _manuEquipmentParameterRepository.ShowCreateTable(EquipmentParameter.EquipmentProcedureParameterTemplateName);
            sql= sql?.Replace(EquipmentParameter.EquipmentProcedureParameterTemplateName, tabname);
            sql= sql?.Replace($"CREATE TABLE", $"CREATE TABLE  IF NOT EXISTS");
            await _manuEquipmentParameterRepository.CreateProductParameterTable(sql ?? "");
        }

        #region 内部方法
        /// <summary>
        /// 更具设备编码获取表名
        /// </summary>
        /// <param name="paran"></param>
        /// <returns></returns>
        private string GetTableNameByEquipmentCode(long siteId, string equipmentCode)
        {
            var key = CalculateCrc32($"{siteId}{equipmentCode}");

            return $"{EquipmentParameter.EquipmentParameterPrefix}{key % _parameterOptions.ParameterDelivery}";
        }

        /// <summary>
        /// Crc32 加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static uint CalculateCrc32(string input)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            return Crc32Algorithm.Compute(bytes);
        }
        #endregion
    }
}
