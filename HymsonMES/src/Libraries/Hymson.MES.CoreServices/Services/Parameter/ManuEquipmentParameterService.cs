using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.Parameter;
using Hymson.MES.CoreServices.Dtos.Parameter;
using Hymson.MES.Data.Repositories.Parameter;
using Hymson.Snowflake;
using Hymson.Utils.Tools;
using System.Transactions;

namespace Hymson.MES.CoreServices.Services.Parameter
{
    /// <summary>
    /// 设备参数采集
    /// </summary>
    public class ManuEquipmentParameterService : IManuEquipmentParameterService
    {
        private readonly IManuEquipmentParameterRepository _manuEquipmentParameterRepository;

        /// <summary>
        /// 设备参数采集
        /// </summary>
        /// <param name="manuEquipmentParameterRepository"></param>
        public ManuEquipmentParameterService(IManuEquipmentParameterRepository manuEquipmentParameterRepository)
        {
            _manuEquipmentParameterRepository = manuEquipmentParameterRepository;
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task InsertRangeAsync(IEnumerable<EquipmentParameterDto> param)
        {
            List<EquipmentParameterEntity> list = new List<EquipmentParameterEntity>();
            foreach (var paramDto in param)
            {
                var entity = paramDto.ToEntity<EquipmentParameterEntity>();
                entity.Id = IdGenProvider.Instance.CreateId();
                entity.CreatedBy = paramDto.UserName;
                entity.UpdatedBy = paramDto.UserName;
                list.Add(entity);
            }

            using var trans = TransactionHelper.GetTransactionScope(TransactionScopeOption.RequiresNew);

            await _manuEquipmentParameterRepository.InsertRangeAsync(list);

            trans.Complete();
        }
    }
}
