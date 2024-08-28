using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Constants.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.NioPushCollection;
using Hymson.MES.Data.Repositories.NioPushCollection.Query;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundServices.NIO.Services
{
    /// <summary>
    /// 异常数据处理
    /// </summary>
    public class AbnormalDataService : IAbnormalDataService
    {
        /// <summary>
        /// NIO推送参数
        /// </summary>
        private readonly INioPushCollectionRepository _nioPushCollectionRepository;

        /// <summary>
        /// 配置
        /// </summary>
        private readonly ISysConfigRepository _sysConfigRepository;

        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<IAbnormalDataService> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        public AbnormalDataService(INioPushCollectionRepository nioPushCollectionRepository,
            ISysConfigRepository sysConfigRepository,
            ILogger<IAbnormalDataService> logger)
        {
            _nioPushCollectionRepository = nioPushCollectionRepository;
            _sysConfigRepository = sysConfigRepository;
            _logger = logger;
        }

        /// <summary>
        /// 重复参数处理
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public async Task<int> RepeatParamAsync(int day)
        {
            try
            {
                return await RepeatParamTaskAsync(day);
            }
            catch (Exception ex)
            {
                _logger.LogError($"重复参数处理异常：{ex}；堆栈:{ex.StackTrace}");
            }

            return 0;
        }

        /// <summary>
        /// 重复参数处理
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public async Task<int> RepeatParamTaskAsync(int day)
        {
            _logger.LogError($"重复参数处理开始");

            DateTime now = HymsonClock.Now();
            //now = Convert.ToDateTime("2024-08-22 14:36:00");

            List<string> configProcedureList = new List<string>();

            //站点配置
            var configSiteEntities = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.MainSite });
            if (configSiteEntities == null || !configSiteEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10139));
            }
            long siteId = long.Parse(configSiteEntities.ElementAt(0).Value);

            //工序配置
            SysConfigQuery operQuery = new SysConfigQuery();
            operQuery.Type = SysConfigEnum.NioRepeatParam;
            operQuery.SiteId = siteId;
            var configEntities = await _sysConfigRepository.GetEntitiesAsync(operQuery);
            if (configEntities != null && configEntities.Count() > 0)
            {
                string configValue = configEntities.First().Value;
                configProcedureList = configValue.Split('&').ToList();
            }

            //获取推送的的参数
            NioPushCollectionRepeatQuery nioQuery = new NioPushCollectionRepeatQuery();
            nioQuery.EndDate = now;
            nioQuery.BeginDate = now.AddDays(-7);
            nioQuery.ProcedureList = configProcedureList;
            var nioParamList = await _nioPushCollectionRepository.GetRepeatEntitiesAsync(nioQuery);
            if(nioParamList == null || nioParamList.Count() == 0)
            {
                return 0;
            }

            List<string> sfcList = nioParamList.Select(m => m.VendorProductTempSn).Distinct().ToList();
            List<string> procedureList = nioParamList.Select(m => m.StationId).Distinct().ToList();
            sfcList = sfcList.Take(100).ToList(); //每次只处理100条数据

            //查询重复的条码数据
            NioPushCollectionSfcQuery sfcQuery = new NioPushCollectionSfcQuery();
            sfcQuery.SfcList = sfcList;
            sfcQuery.ProcedureList = procedureList;
            var dbSfcList = await _nioPushCollectionRepository.GetEntitiesBySfcAsync(sfcQuery);

            if(dbSfcList == null || dbSfcList.Count() == 0)
            {
                return 0;
            }

            List<string> paramList = dbSfcList.Select(m => m.VendorFieldCode).Distinct().ToList();

            List<long> delList = new List<long>();
            //遍历条码
            foreach (var curSfc in sfcList)
            {
                //遍历参数
                foreach(var curParam in paramList)
                {
                    //获取当前参数和条码对应的所有数据
                    var curList = dbSfcList.Where(m => m.VendorProductTempSn == curSfc && m.VendorFieldCode == curParam).ToList();
                    if (curList == null || curList.Count() == 0)
                    {
                        continue;
                    }
                    //获取当前条码+参数的最后的一条数据
                    long maxId = curList.Select(m => m.Id).Max();
                    //找到在这之前的
                    foreach (var idItem in curList)
                    {
                        if (idItem.Id < maxId)
                        {
                            delList.Add(idItem.Id);
                        }
                    }
                }
            }

            DeleteCommand delCom = new DeleteCommand();
            delCom.DeleteOn = HymsonClock.Now();
            delCom.UserId = "RepeatParamTask";

            int maxNum = 500;
            int batchNum = delList.Count / maxNum + 1;

            using var trans = TransactionHelper.GetTransactionScope();

            for (int i = 0;i < batchNum; ++i)
            {
                delCom.Ids = delList.Skip(i * maxNum).Take(maxNum).ToList();
                await _nioPushCollectionRepository.DeletesAsync(delCom);
            }

            trans.Complete();

            _logger.LogError($"重复参数处理结束");

            return 0;
        }
    }
}
