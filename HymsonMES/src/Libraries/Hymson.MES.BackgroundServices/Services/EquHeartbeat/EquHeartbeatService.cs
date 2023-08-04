using Hymson.MES.BackgroundServices.Dtos.EquHeartbeat;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;

namespace Hymson.MES.BackgroundServices.Services.EquHeartbeat
{
    public class EquHeartbeatService : IEquHeartbeatService
    {
        private readonly IEquHeartbeatRepository _equHeartbeatRepository;
        public EquHeartbeatService(IEquHeartbeatRepository equHeartbeatRepository)
        {
            _equHeartbeatRepository = equHeartbeatRepository;
        }

        private readonly string _serviceName = "BackgroundServices";
        /// <summary>
        /// 设备心跳状态更新
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task EquipmentHeartbeatUpdateAsync(EquipmentHeartbeatUpdateDto request)
        {
            EquHeartbeatQuery query = new()
            {
                Status = true
            };
            //查询所有在线设备
            var equHeartbeatEntities = await _equHeartbeatRepository.GetEquHeartbeatEntitiesAsync(query);
            int intervalSeconds = 0 - (request.IntervalSeconds ?? 30);
            var equHeartbeats = equHeartbeatEntities.Where(c => c.LastOnLineTime < HymsonClock.Now().AddSeconds(intervalSeconds));
            List<EquHeartbeatRecordEntity> equHeartbeatRecords = new List<EquHeartbeatRecordEntity>();
            foreach (var equHeart in equHeartbeats)
            {
                var dateTime = HymsonClock.Now();
                equHeart.Status = false;
                equHeart.UpdatedBy = _serviceName;
                equHeart.UpdatedOn = dateTime;
                //添加心跳记录
                equHeartbeatRecords.Add(new EquHeartbeatRecordEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = equHeart.SiteId,
                    CreatedBy = _serviceName,
                    CreatedOn = dateTime,
                    UpdatedBy = _serviceName,
                    UpdatedOn = dateTime,
                    EquipmentId = equHeart.EquipmentId,
                    LocalTime = dateTime,
                    Status = equHeart.Status
                });
            }
            if (equHeartbeats.Any())
            {
                using var trans = TransactionHelper.GetTransactionScope();
                await _equHeartbeatRepository.UpdatesAsync(equHeartbeats);
                await _equHeartbeatRepository.InsertRecordsAsync(equHeartbeatRecords);
                trans.Complete();
            }
        }

        /// <summary>
        /// 删除之前的心跳数据
        /// </summary>
        /// <param name="months"></param>
        /// <returns></returns>
        public async Task<int> DeleteMonthsBeforeAsync(int months)
        {
            return await _equHeartbeatRepository.DeleteMonthsBeforeAsync(months);
        }
    }
}
