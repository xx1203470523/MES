﻿using Hymson.Infrastructure.Exceptions;
using Hymson.MES.BackgroundServices.Rotor.Dtos.Manu;
using Hymson.MES.BackgroundServices.Rotor.Repositories;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Constants.Manufacture;
using Hymson.MES.Core.Domain.Mavel.Rotor;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture.Query;
using Hymson.MES.Data.Repositories.Mavel.Rotor.PackList;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Hymson.WaterMark;
using NETCore.Encrypt.Internal;
using Quartz.Impl.AdoJobStore.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundServices.Rotor.Services
{
    /// <summary>
    /// 包装服务
    /// </summary>
    public class PackListService : IPackListService
    {
        /// <summary>
        /// 装箱仓储
        /// </summary>
        private IPackListRepository _packListRepository;

        /// <summary>
        /// 服务接口（水位）
        /// </summary>
        private readonly IWaterMarkService _waterMarkService;

        /// <summary>
        /// MES包装
        /// </summary>
        private readonly IManuRotorPackListRepository _manuRotorPackListRepository;

        /// <summary>
        /// 系统配置
        /// </summary>
        private readonly ISysConfigRepository _sysConfigRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public PackListService(
            IPackListRepository packListRepository,
            IWaterMarkService waterMarkService,
            IManuRotorPackListRepository manuRotorPackListRepository,
            ISysConfigRepository sysConfigRepository) 
        {
            _packListRepository = packListRepository;
            _waterMarkService = waterMarkService;
            _manuRotorPackListRepository = manuRotorPackListRepository;
            _sysConfigRepository = sysConfigRepository;
        }

        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        public async Task<int> ExecAsync(int rows)
        {
            string busKey = BusinessKey.RotorPackList;
            //查询水位
            long waterMarkId = await _waterMarkService.GetWaterMarkAsync(busKey);
            DateTime startWaterMarkTime = HymsonClock.Now();
            if(waterMarkId == 333)
            {
                return 0;
            }
            if (waterMarkId != 0)
            {
                startWaterMarkTime = UnixTimestampMillisToDateTime(waterMarkId);
            }
            else
            {
                startWaterMarkTime = DateTime.Parse("2024-08-01 01:01:01");
            }
            //获取站点
            var configEntities = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.MainSite });
            if (configEntities == null || !configEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10139));
            }
            long siteID = long.Parse(configEntities.ElementAt(0).Value);

            var packDbList = await GetInPackListAsync(startWaterMarkTime, rows);
            if(packDbList == null || packDbList.Count == 0)
            {
                return 0;
            }

            //判断条码是否已经存在
            List<string> sfcList = packDbList.Select(m => m.ProductCode).Distinct().ToList();
            ManuRotorSfcListQuery packQuery = new ManuRotorSfcListQuery();
            packQuery.SfcList = sfcList;
            var dbSfcList = await _manuRotorPackListRepository.GetEntitiesAsync(packQuery);

            List<ManuRotorPackListEntity> insertList = new List<ManuRotorPackListEntity>();
            List<ManuRotorPackListEntity> updateList = new List<ManuRotorPackListEntity>();
            DeleteCommand delModel = new DeleteCommand();
            delModel.UserId = "LMSJOB";
            delModel.DeleteOn = HymsonClock.Now();
            delModel.Ids = new List<long>();
            List<long> idList = new List<long>();
            foreach (var item in packDbList)
            {
                ManuRotorPackListEntity? dbSfcModel = dbSfcList.Where(m => m.ProductCode == item.ProductCode).FirstOrDefault();

                if (item.IsDeleted == true)
                {
                    if(dbSfcModel != null)
                    {
                        idList.Add(dbSfcModel.Id);
                    }
                }
                else
                {
                    if(dbSfcModel != null)
                    {
                        dbSfcModel.Id = IdGenProvider.Instance.CreateId();
                        dbSfcModel.UpdatedBy = item.UpdateID;
                        dbSfcModel.UpdatedOn = HymsonClock.Now();
                        dbSfcModel.IsDeleted = item.IsDeleted == true ? 1 : 0;
                        dbSfcModel.BoxCode = item.BoxCode;
                        dbSfcModel.ProductCode = item.ProductCode;
                        dbSfcModel.ProductNo = item.ProductNo;
                        //updateList.Add(dbSfcModel);
                        insertList.Add(dbSfcModel);
                    }
                    else
                    {
                        ManuRotorPackListEntity model = new ManuRotorPackListEntity();

                        model.Id = IdGenProvider.Instance.CreateId();
                        model.CreatedOn = HymsonClock.Now();
                        model.UpdatedOn = model.CreatedOn;
                        model.CreatedBy = "LMSJOB";
                        model.UpdatedBy = "LMSJOB";
                        model.IsDeleted = item.IsDeleted == true ? 1 : 0;
                        model.SiteId = siteID;
                        model.BoxCode = item.BoxCode;
                        model.ProductCode = item.ProductCode;
                        model.ProductNo = item.ProductNo;

                        insertList.Add(model);
                    }
                }
            }
            delModel.Ids = idList;

            //水位数据更新
            DateTime maxUpdateTime = packDbList.Max(x => x.UpdateTime);
            long timestamp = GetTimestampInMilliseconds(maxUpdateTime);

            //MES数据入库
            using var trans = TransactionHelper.GetTransactionScope();

            await _manuRotorPackListRepository.DeletesAsync(delModel);
            await _manuRotorPackListRepository.InsertRangeAsync(insertList);
            await _waterMarkService.RecordWaterMarkAsync(busKey, timestamp);

            trans.Complete();

            return 0;
        }

        /// <summary>
        /// 获取线体MES过站数据
        /// </summary>
        /// <param name="start"></param>
        /// <param name="rows"></param>
        /// <param name="suffixTableName"></param>
        /// <returns></returns>
        private async Task<List<PackListDto>> GetInPackListAsync(DateTime start, int rows)
        {
            List<PackListDto> resultList = new List<PackListDto>();

            string sql = $@"
                select top {rows} * from pack_packlist
                where UpdateTime > '{start.ToString("yyyy-MM-dd HH:mm:ss.fff")}'
                order by UpdateTime asc
            ";

            List<PackListDto> dbList = await _packListRepository.GetList(sql);
            if (dbList == null || dbList.Count == 0)
            {
                return resultList;
            }

            return dbList;
        }

        /// <summary>
        /// 转为毫秒时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private long GetTimestampInMilliseconds(DateTime? dateTime)
        {
            if (dateTime == null)
            {
                return 0;
            }

            // 首先将本地时间转换为UTC时间  
            DateTime utcDateTime = ((DateTime)dateTime).ToUniversalTime();
            // 然后计算UTC时间与Unix纪元（1970年1月1日UTC）之间的差值  
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan timeSpan = utcDateTime - epoch;
            return (long)timeSpan.TotalMilliseconds;
        }

        /// <summary>
        /// 将Unix时间戳（毫秒）转换为DateTime  
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        private DateTime UnixTimestampMillisToDateTime(long timestamp)
        {
            // 将Unix时间戳转换为UTC DateTime  
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime utcDateTime = epoch.AddMilliseconds(timestamp);
            // 然后将UTC DateTime转换为本地时间  
            DateTime localDateTime = utcDateTime.ToLocalTime();
            return localDateTime;
        }

    }
}
