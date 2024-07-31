//namespace Hymson.MES.Core.Constants
//{
//    /// <summary>
//    /// 错误码（欣视界项目占用14000-15000）
//    /// </summary>
//    public static partial class ErrorCode
//    {
//        #region 烘烤
//        public const string MES14001 = "该条码已经在烘烤中！";
//        public const string MES14002 = "该条码的烘烤快照不存在！";
//        public const string MES14003 = "该位置已经被占用！";
//        public const string MES14004 = "极卷条码不能为空！";
//        public const string MES14005 = "计划烘烤时间不能为空！";
//        public const string MES14006 = "烘烤位置不能为空！";
//        #endregion

//        #region 设备对接错误
//        public const string MES14030 = "条码生成失败,半成品记录未找到！";
//        public const string MES14031 = "条码流转操作失败";
//        public const string MES14032 = "指定参数不存在：{ParameterCodes}";
//        public const string MES14033 = "主物料：{id}设置了不允许使用替代料";
//        public const string MES14034 = "首工序工单绑定记录未找到，指定资源编码为：{code}";
//        public const string MES14035 = "该外部条码{sfc}已经生成了在制记录，不可重复绑定";
//        public const string MES14036 = "物料条码{barCode}状态为使用中,请检查!";
//        public const string MES14037 = "上料记录未找到或未进行上料,请检查!";
//        public const string MES14038 = "该物料{code}对应的工单未找到,请检查!";
//        public const string MES14039 = "该条码{sfc}不存在与当前托盘{vehicleCode}，请检查!";
//        public const string MES14040 = "该条码{sfc}没有跟随当前托盘{vehicleCode}进行操作，请检查!";
//        public const string MES14041 = "工艺设备组：未找到设备{name}指定的工艺设备组！";
//        public const string MES14042 = "工艺设备组：系统中未配置有效的工艺设备组！";
//        public const string MES14043 = "工艺配方上报：新增配方集合插入失败！";
//        public const string MES14044 = "上料完整性校验：这些物料(Id){code}未上料!";
//        #endregion

//        #region  14100 金蝶ERP对接
//        public const string MES14100 = "工单{orderCode}的产品编码{code},产品版本{version}在系统中不存在!";
//        public const string MES14101 = "工单{orderCode}的工作中心{code}在系统中不存在!";
//        public const string MES14102 = "工单{orderCode}的Bom {code}物料清单不能为空!";
//        public const string MES14103 = " 领料单据号{code}在系统中已存在!";
//        public const string MES14104 = "工单号 {code}在系统中不存在!";
//        public const string MES14105 = "仓库编号{code}在系统中不存在!";
//        public const string MES14106 = "退料单据号{code}在系统中已存在!";
//        public const string MES14107 = "退料信息不能为空!";
//        public const string MES14108 = "生产入库单箱条码{barCode}不存在!";
//        public const string MES14109 = "领料信息不能为空!";
//        public const string MES14110 = "物料版本不能为空!";
//        public const string MES14111 = "物料组编码不能为空!";
//        public const string MES14112 = "物料组名称不能为空!";
//        public const string MES14113 = "物料编码不能为空!";
//        public const string MES14114 = "物料名称不能为空!";
//        public const string MES14115 = "供应商编码不能为空!";
//        public const string MES14116 = "供应商名称不能为空!";
//        public const string MES14117 = "仓库编码不能为空!";
//        public const string MES14118 = "仓库名称不能为空!";
//        public const string MES14119 = "工作中心编码不能为空!";
//        public const string MES14120 = "工作中心名称不能为空!";
//        public const string MES14121 = "计量单位编码不能为空!";
//        public const string MES14122 = "计量单位名称不能为空!";
//        public const string MES14123 = "入库单号不能为空!";
//        public const string MES14124 = "工单号不能为空!";
//        public const string MES14125 = "入库箱条码不能为空!";
//        public const string MES14126 = "领料单号不能为空!";
//        public const string MES14127 = "退料单号不能为空!";
//        public const string MES14128 = "班次编码不能为空!";
//        public const string MES14129 = "班次名称不能为空!";
//        public const string MES14130 = "物料在系统中不存在!";
//        public const string MES14131 = "包装条码不能为空!";
//        public const string MES14132 = "包装条码在系统中不存在!";
//        #endregion

//        #region 跨工序时间 14200
//        public const string MES14200 = "时间管控编号不能为空";
//        public const string MES14201 = "时间管控编号最大长度为50";
//        public const string MES14202 = "名称不能为空";
//        public const string MES14203 = "名称最大长度为60";
//        public const string MES14204 = "描述最大长度为255";
//        public const string MES14205 = "此时间管控编号{code}在系统已经存在！";
//        public const string MES14206 = "同产品、同起始工序、同结束工不能重复录入!";
//        public const string MES14207 = "起始工序不能与到达工序为同一工序!";
//        public const string MES14208 = "起始工序不属于到达工序的前工序!";
//        public const string MES14209 = "上限时间不可小于下限时间!";
//        public const string MES14210 = "上限时间和下限时间不可相等!";
//        public const string MES14211 = "上限时间需大于0";
//        public const string MES14212 = "下限时间需大于0";
//        public const string MES14213 = "此跨工序时间管控信息不存在!";
//        public const string MES14214 = "选择的产品不存在！";
//        public const string MES14215 = "托盘内产品没有维护产品工序时间，不允许进站!";
//        #endregion

//        #region 产品工序时间 14300
//        public const string MES14300 = "缓存时间需大于0";
//        public const string MES14301 = "同物料、同工序不能重复录入";
//        public const string MES14302 = "此产品工序时间不存在!";
//        public const string MES14303 = "此产品工序时间在系统已经存在!";
//        #endregion

//        #region 陈化操作 14400
//        public const string MES14400 = "托盘码{vehicleCode}在系统不存在!";
//        public const string MES14401 = "托盘{vehicleCode}装载信息不能为空!";
//        public const string MES14402 = "托盘位置号不能为空!";
//        public const string MES14403 = "该位置已存在正在陈化的产品，不可重复录入!";
//        public const string MES14404 = "{vehicleCode}托盘中存在条码未在本工序排队，请检查！";
//        public const string MES14405 = "{vehicleCode}托盘中的条码不是在制品！";
//        public const string MES14406 = "{vehicleCode}托盘已经在站内，不可重复进站!";
//        public const string MES14407 = "陈化的开始温度必须大于0℃";
//        public const string MES14408 = "陈化的结束温度必须大于0℃";
//        public const string MES14409 = "{vehicleCode}托盘不在站内,不可出站!";
//        public const string MES14410 = "{vehicleCode}托盘已禁用,不可操作!";
//        #endregion

//        #region 物料报废 14500
//        public const string MES14500 = "该条码不属于库存内可用条码，不可进行报废操作！";
//        public const string MES14501 = "条码状态必须为待使用";
//        public const string MES14502 = "当前工单不包含物料条码,不可进行报废操作";
//        public const string MES14503 = "报废数量不能大于剩余数量";
//        public const string MES14504 = "报废数量必须大于0";
//        public const string MES14505 = "该条码不存在物料报废记录中，不可进行报废取消操作！";
//        public const string MES14506 = "请选择不合格代码！";
//        #endregion

//        #region 搅拌 14600
//        public const string MES14600 = "半成品记录未找到!";
//        public const string MES14601 = "查询的工单不存在!";
//        public const string MES14602 = "该半成品ID不存在物料BOM明细表中!";
//        public const string MES14603 = "收货数量超出工单的已下达数量,不予生成!";
//        public const string MES14604 = "下达数量必须大于0!";
//        public const string MES14605 = "请先配置资源打印信息!";
//        public const string MES14606 = "实际数量必须要大于0!";
//        public const string MES14607 = "请先维护产品参数收集数据!";
//        public const string MES14608 = "请先维护工序半成品信息!";
//        public const string MES14609 = "物料数量不能大于剩余数量!";
//        public const string MES14610 = "该设备存在生产中的条码【条码:{SFC}】，不允许重复下达";
//        #endregion

//        #region 手动分选操作 14700
//        public const string MES14700 = "产品编码无可用分选规则!";
//        public const string MES14701 = "条码信息不能为空!";
//        public const string MES14702 = "条码的产品参数不能为空!";
//        public const string MES14703 = "条码的最终档次信息获取不到!";
//        public const string MES14704 = "该条码没有上报产品参数!";
//        public const string MES14705 = "该条码无可用分选规则!";
//        #endregion

//        #region 工单报工 14800
//        public const string MES14800 = "非手动添加的报工单，不可删除!";
//        public const string MES14801 = "已经报工的报工单，不可删除!";
//        public const string MES14802 = "日期：{reportDate} ，班次：{class}，工单{orderCode}已经存在了!";
//        public const string MES14803 = "工单未激活";
//        public const string MES14804 = "工单在系统不存在";
//        public const string MES14805 = "该工单已报工，不允许重复报工!";
//        public const string MES14806 = "该工单未报工，不可取消上报!";
//        #endregion

//        #region Marking 14900 
//        public const string MES14900 = "Marking错误";
//        public const string MES14901 = "发现工序不能为空";
//        public const string MES14902 = "拦截工序不能为空";
//        public const string MES14903 = "不合格代码不能为空";
//        public const string MES14904 = "条码不能为空";
//        public const string MES14905 = "条码最大长度为100";
//        public const string MES14906 = "所有条码不存在";
//        public const string MES14907 = "{sfc}相关的数据已存在";
//        public const string MES14908 = "导入的Marking录入数据为空";
//        public const string MES14909 = "在第{row}行,发现工序[{code}]没有找到对应的数据";
//        public const string MES14910 = "在第{row}行,拦截工序[{code}]没有找到对应的数据";
//        public const string MES14911 = "在第{row}行,不合格代码[{code}]没有找到对应的数据";
//        public const string MES14912 = "在第{row}行,{sfc}相关的数据已存在";
//        public const string MES14913 = "产品序列码[{sfc}]对应行数据中，发现工序没有找到对应的数据";
//        public const string MES14914 = "产品序列码[{sfc}]对应行数据中，拦截工序没有找到对应的数据";
//        public const string MES14915 = "产品序列码[{sfc}]对应行数据中，不合格代码没有找到对应的数据";
//        public const string MES14916 = "没有找到对应的Marking标识";
//        public const string MES14917 = "导入的移除Marking数据为空";
//        public const string MES14919 = "发现工序[{code}]没有找到对应的数据";
//        public const string MES14920 = "拦截工序[{code}]没有找到对应的数据";
//        public const string MES14921 = "不合格代码[{code}]没有找到对应的数据";
//        public const string MES14922 = "{sfc}相关的数据已存在";
//        public const string MES14923 = "{sfc}相关的数据不存在";
//        public const string MES14924 = "{repeats}相关的数据重复";

//        public const string MES14925 = "产品序列码[{sfc}]对应行数据中，发现工序不是启用状态，无法使用";
//        public const string MES14926 = "产品序列码[{sfc}]对应行数据中，拦截工序不是启用状态，无法使用";
//        public const string MES14927 = "产品序列码[{sfc}]对应行数据中，不合格代码不是启用状态，无法使用";
//        public const string MES14928 = "产品序列码[{sfc}]对应行数据中，不合格代码不是标识类型";
//        public const string MES14929 = "发现工序[{code}]不是启用状态";
//        public const string MES14930 = "拦截工序[{code}]不是启用状态";
//        public const string MES14931 = "不合格代码[{code}]不是启用状态";
//        public const string MES14932 = "不合格代码[{code}]不是标识类型";

//        #endregion

//        #region Report MES14950 
//        public const string MES14950 = "选择的日期不能大于15周";
//        public const string MES14951 = "选择的日期不能大于15天";
//        public const string MES14952 = "选择的日期不能大于12个月";
//        public const string MES14953 = "查询日期不能为空";
//        public const string MES14954 = "工序不能为空";
//        public const string MES14955 = "找不到激活的工单！";
//        #endregion

//    }
//}
