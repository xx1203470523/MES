using Google.Protobuf.WellKnownTypes;
using Hymson.MES.BackgroundServices.Rotor.Dtos.Manu;
using MySqlX.XDevAPI.Common;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundServices.Rotor.Services
{
    /// <summary>
    /// 过站服务
    /// </summary>
    public class ManuInOutBoundService : IManuInOutBoundService
    {
        /// <summary>
        /// 进站状态
        /// </summary>
        private readonly string ProductStatus_In = "S";

        /// <summary>
        /// 出站状态
        /// </summary>
        private readonly string ProductStatus_Out = "Z";

        /// <summary>
        /// OK标识
        /// </summary>
        private readonly string Result_OK = "OK";

        /// <summary>
        /// 物料类型 1-批次
        /// </summary>
        private readonly int MatType_One = 1;

        /// <summary>
        /// 物料类型 2-批次
        /// </summary>
        private readonly int MatType_Batch = 2;

        /// <summary>
        /// 进出站
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public async Task InOutBoundAsync(DateTime start, DateTime end)
        {
            #region 获取线体LMS数据

            //表名后缀
            string suffixTableName = GetFirstDayOfQuarter(start);
            //1. 根据开始时间，结束时间，读取线体MES过站数据
            List<WorkProcessDto> inOutList = await GetInOutBoundListAsync(start, end, suffixTableName);
            List<string> sfcIdList = inOutList.Select(m => m.ID).ToList();
            //2. 查询参数信息
            List<WorkProcessData> paramList = await GetParamList(suffixTableName, sfcIdList);
            //3. 查询上料信息
            List<WorkItemInfo> upMatList = await GetUpMatList(suffixTableName, sfcIdList);

            #endregion

            //MES进站数据
            List<MesInDto> inList = new List<MesInDto>();
            //MES出战数据
            List<MesOutDto> outList = new List<MesOutDto>();

            #region 线体LMS数据转成MES数据
            //获取MES需要处理管控的工位数据
            List<WorkPosDto> workPosList = GetPosNoList();
            List<WorkPosDto> mesWorkPosList = workPosList.Where(m => m.WorkPosType != 0).ToList();
            List<string> workPosCodeList = mesWorkPosList.Select(m => m.WorkPosNo).ToList();
            //过滤MES不需要的数据
            List<WorkProcessDto> mesList = inOutList.Where(m => workPosCodeList.Contains(m.WorkPosNo) == true).ToList();
            //依次处理每个条码的数据
            foreach(var item in  mesList)
            {
                //获取工位类型
                WorkPosDto? curWorkPos = mesWorkPosList.Where(m => item.WorkPosNo == m.WorkPosNo).FirstOrDefault();
                if(curWorkPos == null)
                {
                    continue;
                }
                //基础数据
                MesDto mesDto = new MesDto();
                mesDto.Sfc = item.ProductNo;
                mesDto.ProcedureCode = item.ProcedureCode;
                mesDto.IsPassed = item.Result == Result_OK ? true : false;
                mesDto.Date = item.CreateTime;
                //进站
                if ((curWorkPos.WorkPosType & 1) == 1 && item.ProductStatus == ProductStatus_In)
                {
                    //基础数据
                    MesInDto indto = new MesInDto();
                    indto = (MesInDto)mesDto;
                    inList.Add(indto);
                }
                //出站
                if ((curWorkPos.WorkPosType & 2) == 2 && item.ProductStatus == ProductStatus_Out)
                {
                    //基础数据
                    MesOutDto outDto = new MesOutDto();
                    outDto = (MesOutDto)mesDto;
                    //处理上料信息
                    List<WorkItemInfo> curUpMatList = upMatList.Where(m => m.ProcessUID == item.ID).ToList();
                    if(curUpMatList != null && curUpMatList.Count > 0)
                    {
                        List<SfcUpMatDto> sfcUpList = new List<SfcUpMatDto>();
                        foreach(var upItem in  curUpMatList)
                        {
                            sfcUpList.Add(new SfcUpMatDto()
                            {
                                MatName = upItem.MatName,
                                MatValue = upItem.MatValue,
                                MatBatchCode = upItem.MatBatchCode,
                                BarCode = string.IsNullOrEmpty(upItem.MatValue) ? upItem.MatBatchCode : upItem.MatValue,
                                MatType = string.IsNullOrEmpty(upItem.MatValue) ? MatType_One : MatType_Batch,
                                MatNum = upItem.MatNum
                            });
                        }
                        outDto.UpMatList = sfcUpList;
                    }
                    //处理参数信息
                    List<WorkProcessData> curParamList = paramList.Where(m => m.ProcessUID == item.ID).ToList();
                    if(curParamList != null && curParamList.Count > 0)
                    {
                        List<SfcParamDto> sfcParamList = new List<SfcParamDto>();
                        foreach(var paramItem in curParamList)
                        {
                            SfcParamDto curParamDto = new SfcParamDto();
                            curParamDto.ParamName = paramItem.Name;
                            curParamDto.Unit = paramItem.Unit;
                            curParamDto.Value = Convert.ToDecimal(paramItem.Value);
                            curParamDto.StrValue = paramItem.StrValue;
                            curParamDto.Result = paramItem.Result == 1 ? true : false;
                            curParamDto.ValueType = string.IsNullOrEmpty(paramItem.StrValue) ? 1 : 2;
                            curParamDto.ParamValue = string.IsNullOrEmpty(paramItem.StrValue) ? curParamDto.Value.ToString() : paramItem.StrValue;
                            sfcParamList.Add(curParamDto);
                        }
                        outDto.ParamList = sfcParamList;
                    }
                    //处理NG信息
                    outDto.ParamToNgList();
                }
            }

            #endregion

            //MES数据入库

        }

        /// <summary>
        /// 获取线体MES过站数据
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="suffixTableName"></param>
        /// <returns></returns>
        private async Task<List<WorkProcessDto>> GetInOutBoundListAsync(DateTime start, DateTime end,
            string suffixTableName)
        {
            List<WorkProcessDto> resultList = new List<WorkProcessDto>();

            string sql = $@"
                SELECT * 
                FROM Work_Process_{suffixTableName} wp 
                WHERE IsDeleted = 0
                AND CreateTime >= '{start.ToString("yyyy-MM-dd HH:mm:ss.fff")}'
                AND CreateTime < '{end.ToString("yyyy-MM-dd HH:mm:ss.fff")}'
                ORDER BY StartTime ASC;
            ";

            return resultList;
        }

        /// <summary>
        /// 获取参数信息
        /// </summary>
        /// <param name="suffixTableName"></param>
        /// <param name="sfcIdList"></param>
        /// <returns></returns>
        private async Task<List<WorkProcessData>> GetParamList(string suffixTableName, List<string> sfcIdList)
        {
            List<WorkProcessData> resultLit = new List<WorkProcessData>();

            if (sfcIdList == null ||  sfcIdList.Count == 0)
            {
                return resultLit;
            }

            int batchDataNum = 999;
            int batchNum = sfcIdList.Count / batchDataNum + 1;
            for(int i = 0; i < batchNum; ++i)
            {
                string idListStr = string.Join(",", sfcIdList.Select(uuid => $"'{uuid}'"));

                string sql = $@"
                    SELECT t2.* -- t2.ProcessUID, t2.WorkPosNo ,t2.Name ,t2.Unit ,t2.Value ,t2.StrValue ,t2.[MinValue] ,t2.[MaxValue],t2.[Result] 
                    FROM Work_Process_{suffixTableName} t1
                    inner join Work_ProcessData_{suffixTableName} t2 on t1.ID  = t2.ProcessUID 
                    WHERE T2.ProcessUID IN ( {idListStr} )
                ";
            }

            return resultLit;
        }

        /// <summary>
        /// 获取上料信息
        /// </summary>
        /// <param name="suffixTableName"></param>
        /// <param name="sfcIdList"></param>
        /// <returns></returns>
        private async Task<List<WorkItemInfo>> GetUpMatList(string suffixTableName, List<string> sfcIdList)
        {
            List<WorkItemInfo> resultLit = new List<WorkItemInfo>();

            if (sfcIdList == null || sfcIdList.Count == 0)
            {
                return resultLit;
            }

            int batchDataNum = 999;
            int batchNum = sfcIdList.Count / batchDataNum + 1;
            for (int i = 0; i < batchNum; ++i)
            {
                string idListStr = string.Join(",", sfcIdList.Select(uuid => $"'{uuid}'"));

                string sql2 = $@"
                    SELECT t1.* -- t1.WorkPosNo ,t1.MatName ,t1.MatValue ,t1.MatBatchCode ,t1.MatNum ,t1.MatStatus ,t1.MatSerialID
                    FROM Work_ItemInfo t1
                    inner join Work_Process_{suffixTableName} t2 on t1.ProcessUID = t2.ID  and t2.IsDeleted  = 0
                    WHERE T1.ProcessUID IN ( {idListStr} )
                ";
            }

            return resultLit;
        }

        /// <summary>
        /// 获取首位工位列表
        /// </summary>
        /// <returns></returns>
        private List<WorkPosDto> GetPosNoList()
        {
            // 创建并初始化工位信息列表
            List<WorkPosDto> workPosList = new List<WorkPosDto>
            {
                new WorkPosDto { WorkPosNo = "OP10.1.1.1", WorkPosName = "扫码工位", SortIndex = 1, WorkPosType = 1},
                new WorkPosDto { WorkPosNo = "OP10.1.1.2", WorkPosName = "测高工位", SortIndex = 2, WorkPosType = 2},
                new WorkPosDto { WorkPosNo = "OP20.1.1.1", WorkPosName = "1号插小磁钢工位", SortIndex = 20 ,WorkPosType = 1},
                new WorkPosDto { WorkPosNo = "OP20.1.1.2", WorkPosName = "1号插大磁钢工位", SortIndex = 21 ,WorkPosType = 0},
                new WorkPosDto { WorkPosNo = "OP20.1.2.1", WorkPosName = "2号插小磁钢工位", SortIndex = 22 , WorkPosType = 1},
                new WorkPosDto { WorkPosNo = "OP20.1.2.2", WorkPosName = "2号插大磁钢工位", SortIndex = 23 , WorkPosType = 0},
                new WorkPosDto { WorkPosNo = "OP20.1.3.1", WorkPosName = "3号插小磁钢工位", SortIndex = 24 , WorkPosType = 1},
                new WorkPosDto { WorkPosNo = "OP20.1.3.2", WorkPosName = "3号插大磁钢工位", SortIndex = 25 , WorkPosType = 0},
                new WorkPosDto { WorkPosNo = "OP20.1.4.1", WorkPosName = "磁钢检测工位", SortIndex = 26 , WorkPosType = 0},
                new WorkPosDto { WorkPosNo = "OP20.1.5.1", WorkPosName = "碟片下料工位", SortIndex = 27 , WorkPosType = 2},
                new WorkPosDto { WorkPosNo = "OP30.1.1.1", WorkPosName = "堆叠工位", SortIndex = 30 , WorkPosType = 4},
                new WorkPosDto { WorkPosNo = "OP40.1.1.1", WorkPosName = "1号加热工位", SortIndex = 40 , WorkPosType = 4},
                new WorkPosDto { WorkPosNo = "OP40.1.2.1", WorkPosName = "2号加热工位", SortIndex = 41 , WorkPosType = 4},
                new WorkPosDto { WorkPosNo = "OP40.1.3.1", WorkPosName = "3号加热工位", SortIndex = 42 ,WorkPosType=4},
                new WorkPosDto { WorkPosNo = "OP50.1.1.1", WorkPosName = "1号注塑工位", SortIndex = 50 ,WorkPosType = 4},
                new WorkPosDto { WorkPosNo = "OP50.1.2.1", WorkPosName = "2号注塑工位", SortIndex = 51 , WorkPosType = 4},
                new WorkPosDto { WorkPosNo = "OP55.1.1.1", WorkPosName = "注塑凸点检测工位", SortIndex = 55 , WorkPosType = 4},
                new WorkPosDto { WorkPosNo = "OP55.1.1.6", WorkPosName = "人工返工工位", SortIndex = 56 , WorkPosType = 0},
                new WorkPosDto { WorkPosNo = "OP710.1.1.1", WorkPosName = "轴上料工位", SortIndex = 70 , WorkPosType = 1},
                new WorkPosDto { WorkPosNo = "OP710.1.3.1", WorkPosName = "上平衡盘压装工位", SortIndex = 71 , WorkPosType = 0},
                new WorkPosDto { WorkPosNo = "OP710.1.4.1", WorkPosName = "下平衡盘压装工位", SortIndex = 72 , WorkPosType = 0},
                new WorkPosDto { WorkPosNo = "OP710.1.4.2", WorkPosName = "热套环压装", SortIndex = 73 , WorkPosType = 2},
                new WorkPosDto { WorkPosNo = "OP720.1.2.1", WorkPosName = "轴环压装工位", SortIndex = 76 , WorkPosType = 4},
                new WorkPosDto { WorkPosNo = "OP80.1.1.1", WorkPosName = "冷却工位", SortIndex = 80 ,WorkPosType = 4},
                new WorkPosDto { WorkPosNo = "OP90.1.2.1", WorkPosName = "充磁工位", SortIndex = 90 ,WorkPosType = 1},
                new WorkPosDto { WorkPosNo = "OP90.1.4.1", WorkPosName = "表磁工位", SortIndex = 91 ,WorkPosType = 2},
                new WorkPosDto { WorkPosNo = "OP100.1.1.1", WorkPosName = "动平衡工位", SortIndex = 100 ,WorkPosType = 4},
                new WorkPosDto { WorkPosNo = "OP110.1.1.1", WorkPosName = "清洁工位", SortIndex = 110 , WorkPosType = 4},
                new WorkPosDto { WorkPosNo = "OP120.1.1.1", WorkPosName = "全尺寸工位", SortIndex = 120 , WorkPosType = 4},
                new WorkPosDto { WorkPosNo = "OP130.1.1.1", WorkPosName = "激光刻印工位", SortIndex = 130 , WorkPosType = 4},
                new WorkPosDto { WorkPosNo = "OP140.1.1.1", WorkPosName = "涂油工位", SortIndex = 140 , WorkPosType = 4},
                new WorkPosDto { WorkPosNo = "OP150.1.1.1", WorkPosName = "成品下料工位", SortIndex = 150 , WorkPosType = 4}
            };

            return workPosList;
        }

        /// <summary>  
        /// 将给定日期转换为该日期所在季度的第一天。  
        /// </summary>  
        /// <param name="date">给定的日期。</param>  
        /// <returns>该日期所在季度的第一天。</returns>  
        private string GetFirstDayOfQuarter(DateTime date)
        {
            // 确定月份所在的季度  
            int quarter = (date.Month - 1) / 3 + 1;
            // 计算季度的第一个月份  
            int firstMonthOfQuarter = (quarter - 1) * 3 + 1;
            // 返回该季度的第一天  
            var datetime = new DateTime(date.Year, firstMonthOfQuarter, 1);

            return datetime.ToString("yyyyMMdd");
        }
    }
}
