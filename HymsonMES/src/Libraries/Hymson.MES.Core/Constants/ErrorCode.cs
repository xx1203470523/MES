﻿namespace Hymson.MES.Core.Constants
{
    /// <summary>
    /// 
    /// </summary>
    public class ErrorCode
    {
        #region  用户端错误 
        public const string MES10100 = "请求实体不能为空";
        public const string MES10101 = "站点码获取失败，请重新登录！";
        public const string MES10102 = "删除失败Id 不能为空!";
        public const string MES10103 = "请求参数格式错误!";
        public const string MES10104 = "请求数据不存在!";
        public const string MES10105 = "有生产中工单引用当前物料，不能删除！";

        #region 物料 10200
        public const string MES10200 = "物料维护错误";
        public const string MES10201 = "此物料编码{materialCode}版本{version}在系统已经存在！";
        public const string MES10202 = "添加物料表失败！";
        public const string MES10203 = "站点码获取失败，请重新登录！";
        public const string MES10204 = "此物料不存在！";
        public const string MES10205 = "来源不允许修改！";
        public const string MES10206 = "替代品已包含当前物料！！";
        public const string MES10207 = "替代品操作类型OperationType:{operationType}异常，只能传入1，2，3！";
        public const string MES10208 = "修改物料表失败！";
        public const string MES10209 = "插入物料替代组件表失败！";
        public const string MES10210 = "修改物料替代组件表失败！";
        public const string MES10211 = "删除物料替代组件表失败！";
        public const string MES10212 = "不能删除启用和保留状态的物料！";
        public const string MES10213 = "参数不能为空";
        public const string MES10214 = "物料编码不能为空";
        public const string MES10215 = "物料名称不能为空";
        public const string MES10216 = "此物料组编码{groupCode}在系统已经存在！";
        public const string MES10217 = "存在已分配给其他物料组的物料！";
        public const string MES10218 = "新增物料组失败！";
        public const string MES10219 = "此物料组不存在！";
        public const string MES10220 = "修改物料组失败！";
        public const string MES10221 = "已被分配物料，不允许删除！";
        public const string MES10222 = "插入物料供应商关联表失败！";
        public const string MES10223 = "物料编码最大长度为50";
        public const string MES10224 = "物料名称最大长度为50";
        public const string MES10225 = "有生产中的工单引用其中的物料，不允许删除！";
        #endregion

        #region 资源 10300
        public const string MES10301 = "资源编码不能为空";
        public const string MES10302 = "资源编码最大长度不超过50";
        public const string MES10303 = "资源名称不能为空";
        public const string MES10304 = "资源名称最大长度不超过50";
        public const string MES10305 = "资源状态不能为空";
        public const string MES10306 = "资源配置打印机中，重复配置设备!";
        public const string MES10307 = "一个资源只能对用对应一个主设备";
        public const string MES10308 = "此资源【{ResCode}】在系统中已经存在!";
        public const string MES10309 = "此资源类型不存在!";
        public const string MES10310 = "资源类型在系统中不存在,请重新输入!";
        public const string MES10311 = "此资源类型{ResType}在系统中已经存在!";
        public const string MES10312 = $"资源类型有被分配的资源，不允许删除!";
        public const string MES10313 = $"资源配置打印机中，重复配置打印机!";
        public const string MES10314 = $"资源配置设备中，重复配置设备!";
        public const string MES10315 = "资源打印配置操作类型OperationType{OperationType}异常，只能传入1，2，3！";
        public const string MES10316 = "设备绑定设置数据操作类型OperationTyp{OperationType}异常，只能传入1，2，3！";
        public const string MES10317 = "资源设置数据操作类型OperationType{OperationType}异常，只能传入1，2，3！";
        public const string MES10318 = "作业设置数据操作类型OperationType{OperationType}异常，只能传入1，2，3！";
        public const string MES10319 = $"不能删除启用状态的资源!";
        public const string MES10320 = "所属资源类型ID不能为空";
        public const string MES10321 = "工序名称超长";
        public const string MES10351 = "资源类型编码不能为空";
        public const string MES10352 = "资源类型编码最大长度不超过50";
        public const string MES10353 = "资源类型名称不能为空";
        public const string MES10354 = "资源类型名称最大长度不超过50";
        public const string MES10355 = $"资源已被工作中心引用,不能删除!";
        #endregion

        #region 标签模板
        public const string MES10340 = "模板名称已经存在!";
        public const string MES10341 = "打印机名称重复!";
        public const string MES10342 = "模板名称最大长度为50";
        public const string MES10343 = "模板名称不能为空!";
        public const string MES10344 = "存储路径不能为空!";
        public const string MES10345 = "模板文件不能为空!";
        public const string MES10346 = "打印机名称不能为空!";
        public const string MES10347 = "打印机IP不能为空!";
        public const string MES10348 = "打印机IP重复！";
        #endregion

        #region 工序 10400
        public const string MES10401 = "工序编码不能为空";
        public const string MES10402 = "工序编码最大长度不超过50";
        public const string MES10403 = "工序名称不能为空";
        public const string MES10404 = "工序名称最大长度不超过50";
        public const string MES10405 = "编码:{Code}已存在！";
        public const string MES10406 = "工序不存在！";

        public const string MES10430 = $"不能删除启用和保留状态的工艺路线！";
        public const string MES10431 = $"此工艺路线在系统中已经存在!";
        public const string MES10432 = $"工艺编码不能为空!";
        public const string MES10433 = $"工艺名称不能为空!";
        public const string MES10434 = $"版本不能为空!";
        public const string MES10435 = $"未设置首工序！";
        public const string MES10436 = $"只允许设置一个首工序！";
        public const string MES10437 = "此工艺路线{Code}+{Version}在系统已经存在！";
        public const string MES10438 = "此工艺路线不存在！";
        public const string MES10439 = $"此工艺路线在系统中不存在!";
        public const string MES10440 = $"获取下一工序失败!";
        public const string MES10441 = $"下一工序不存在空值类型工序!";
        public const string MES10442 = $"工序不匹配或前工序不是随机工序!";
        public const string MES10443 = $"启用状态或保留状态不可删除!";
        public const string MES10444 = $"工艺编码最大长度为60";
        public const string MES10445 = $"工艺名称最大长度为60";
        public const string MES10446 = $"抽检比例设置错误";
        public const string MES10447 = $"下一工序不存在非空值类型工序";
        public const string MES10448 = $"只允许选择启用和保留状态的工艺路线！";
        public const string MES10449 = $"当前工艺路线存在重复的工序！";
        #endregion

        #region 参数 10500
        public const string MES10500 = "基础参数错误";
        public const string MES10501 = "站点码获取失败，请重新登录！";
        public const string MES10502 = "此参数编码{parameterCode}在系统已经存在！";
        public const string MES10503 = "请求实体不能为空！";
        public const string MES10504 = "此标准参数不存在！";
        public const string MES10505 = "删除失败Ids 不能为空";
        public const string MES10506 = "参数已被设备参数或产品参数绑定，不允许删除!";
        public const string MES10507 = "修改参数关联类型失败!";
        public const string MES10508 = "参数单位不能为空";
        public const string MES10509 = "参数编码不能为空";
        public const string MES10510 = "参数名称不能为空";
        public const string MES10511 = "标准参数代码最大长度为50";

        #endregion

        #region Bom 10600
        public const string MES10600 = "Bom维护错误";
        public const string MES10601 = "此Bom{bomCode}和版本{version}在系统已经存在!";
        public const string MES10602 = "添加Bom失败！";
        public const string MES10603 = "物料编码不能为空!";
        public const string MES10604 = "替代物料编码不能为空!";
        public const string MES10605 = "工序不能为空!";
        public const string MES10606 = "主物料编码+工序不能重复!";
        public const string MES10607 = "替代物料不能跟主物料重复!";
        public const string MES10608 = "主物料关联的替代物料不能重复!";
        public const string MES10609 = "更新Bom失败！";
        public const string MES10610 = "删除失败 Id不能为空!";
        public const string MES10611 = "不能删除启用状态的Bom!";
        public const string MES10612 = "此Bom在系统中不存在!";
        public const string MES10613 = "Bom编码不能为空";
        public const string MES10614 = "Bom编码最大长度不超过50";
        public const string MES10615 = "Bom名称不能为空";
        public const string MES10616 = "Bom名称最大长度不超过50";
        #endregion

        #region 上料点 10700
        public const string MES10700 = "上料单维护错误";
        public const string MES10701 = "此上料点{LoadPoint}在系统已经存在!";
        public const string MES10702 = "关联物料页签物料不能为空!";
        public const string MES10703 = "关联资源页签资源不能为空!";
        public const string MES10704 = "新增上料单维护失败!";
        public const string MES10705 = "此上料点在系统不存在!";
        public const string MES10706 = "更新上料单维护失败!";
        public const string MES10707 = "请选择数据!";
        public const string MES10708 = "删除数据失败!";
        public const string MES10709 = "删除数据失败!无法删除保留或者启用的数据！";
        public const string MES10710 = "关联物料页签物料不能重复!";
        public const string MES10711 = "关联资源页签资源不能重复!";
        public const string MES10712 = "上料点不能为空";
        public const string MES10713 = "上料点名称不能为空";
        public const string MES10714 = "上料点最大长度为50";
        public const string MES10715 = "上料点名称最大长度为60";
        public const string MES10716 = "无法将其他状态修改成新建状态!";
        #endregion

        #region 掩码维护 10800
        public const string MES10800 = "掩码维护错误";
        public const string MES10801 = "掩码编码不能为空";
        public const string MES10802 = "此编码【{Code}】在系统中已经存在!";
        public const string MES10803 = "掩码规则不能为空!";
        public const string MES10804 = "匹配方式不能为空!";
        public const string MES10805 = "匹配方式为全码时掩码规则长度为10!";
        public const string MES10806 = "起始方式掩码末尾不能为特殊字符\"?\"";
        public const string MES10807 = "中间方式掩码首位和末尾不能为特殊字符\"?\"";
        public const string MES10808 = "结束方式掩码首位不能为特殊字符\"?\"";

        #endregion

        #region 不合格代码 11100
        public const string MES11100 = "不合格代码维护错误";
        public const string MES11101 = "不合格代码编码不能为空";
        public const string MES11102 = "不合格代码编码超过最大长度，不合格代码编码最大长度为50";
        public const string MES11103 = "不合格代码名称不能为空";
        public const string MES11104 = "不合格代码名称超过最大长度，不合格代码名称最大长度为60";
        public const string MES11105 = "不合格代码类型不能为空";
        public const string MES11106 = "不合格代码等级不能为空";
        public const string MES11107 = "不合格代码备注超过最大长度，不合格代码备注最大长度为255";
        public const string MES11108 = "不合格代码{code}已经存在";
        public const string MES11109 = "不合格代码状态不能为空";
        public const string MES11110 = "不合格代码状态不为新建无法被删除";
        public const string MES11111 = "无法将不合格代码状态由其他状态修改为新建";
        #endregion

        #region 不合格组 11200
        public const string MES11200 = "不合格代码组错误";
        public const string MES11201 = "不合格组编码不能为空";
        public const string MES11202 = "不合格组编码超过最大长度，不合格代码编码最大长度为50";
        public const string MES11203 = "不合格组名称不能为空";
        public const string MES11204 = "不合格组名称超过最大长度，不合格代码名称最大长度为60";
        public const string MES11205 = "说明超过最大长度，不合格代码备注最大长度为255";
        public const string MES11206 = "不合格组编码{code}已经存在";
        #endregion

        #region 作业12000
        public const string MES12000 = "作业维护错误";
        public const string MES12001 = "作业{code}已经存在";
        public const string MES12002 = "作业编号 不能为空.";
        public const string MES12003 = "作业编号 超过最大长度，最大长度为50.";
        public const string MES12004 = "作业名称 不能为空.";
        public const string MES12005 = "作业名称 超过最大长度，最大长度为50.";
        public const string MES12006 = "类程序 不能为空.";
        public const string MES12007 = "类程序 超过最大长度，最大长度为255.";
        public const string MES12008 = "备注 超过最大长度，最大长度为255.";
        public const string MES12009 = "作业已经被使用无法删除";
        #endregion

        #region 工作中心 12100
        public const string MES12100 = "工作中心维护错误";
        public const string MES12101 = "工作中心{code}已经存在";
        public const string MES12102 = "工作中心代码不能为空.";
        public const string MES12103 = "工作中心代码超过最大长度，最大长度为50.";
        public const string MES12104 = "工作中心名称不能为空.";
        public const string MES12105 = "工作中心名称超过最大长度，最大长度为50.";
        public const string MES12106 = "工作中心类型不能为空.";
        public const string MES12107 = "工作中心数据来源不能为空.";
        public const string MES12108 = "工作中心是否混线不能为空.";
        public const string MES12109 = "说明 超过最大长度，最大长度为255.";
        public const string MES12110 = "工作中心状态不能为空.";
        public const string MES12111 = "工作中心修改的数据不存在.";
        public const string MES12112 = "工作中心已经关联数据,不允许修改.";
        public const string MES12113 = "启用状态或保留状态不可删除.";
        public const string MES12114 = "产线已关联资源，不允许删除.";
        public const string MES12115 = "存在已被关联的资源,不允许重复关联.";
        public const string MES12116 = "未配置产线对应的资源！";
        public const string MES12117 = "存在已被其他产线关联的资源！";
        public const string MES12118 = "当前线体已激活多个工单，需取消激活工单，剩余激活工单<={maxCount}时才能完成设置！";
        public const string MES12119 = "不能配置重复的车间/产线";
        public const string MES12120 = "不能配置重复的资源";
        public const string MES12121 = "新建或者废除状态的资源不能用于工作中心";
        #endregion

        #region 编码规则 12400
        public const string MES12400 = "代码规则维护错误";
        public const string MES12401 = "代码规则中同样物料Id[{productId}],编码类型已经存在";

        public const string MES12402 = "代码规则新增失败";
        public const string MES12403 = "代码规则中同样物料Id[{productId}],编码类型,包装类型已经存在";

        public const string MES12410 = "物料 不能为空！";
        public const string MES12411 = "编码类型 不能为空！";
        public const string MES12412 = "基数 不能为空！";
        public const string MES12413 = "增量 必须大于0！";
        public const string MES12414 = "序列长度 不能小于0！";
        public const string MES12415 = "重置序号 不能为空！";

        public const string MES12416 = "编码规则组成 不能为空！";

        public const string MES12417 = "忽略字符 超过最大长度,最大长度为100！";
        public const string MES12418 = "编码规则描述 超过最大长度,最大长度为255！";
        public const string MES12419 = "编码规则Id 不能为空！";

        public const string MES12430 = "编码规则组成序号 不能为空！";
        public const string MES12431 = "编码规则组成取值方式 不能为空！";
        public const string MES12432 = "编码规则组成分段值 不能为空！";
        public const string MES12433 = "编码规则组成分段值 超过最大长度 ，最大长度为100！";
        public const string MES12434 = "编码规则组成备注 超过最大长度 ，最大长度为255！";

        #endregion

        #region 容器维护 12500
        public const string MES12500 = "容器维护错误";
        public const string MES12501 = "最大数量不能小于最小数量{Minimum}";
        public const string MES12502 = "最大数量不能小于最小数量";
        public const string MES12503 = "同一物料/物料组只允许设置一次";
        public const string MES12504 = "最小数量须为正整数";
        public const string MES12505 = "最大数量须为正整数";
        public const string MES12506 = "最小数量须大于0";
        public const string MES12507 = "最大数量须大于0";
        public const string MES12508 = "最大数量须大于最小数量";
        public const string MES12509 = "只能删除新建状态的数据";
        public const string MES12510 = "只能删除新建状态的数据";
        #endregion

        #region 设备 12600
        public const string MES12600 = "此编码{Code}在系统已经存在!";
        public const string MES12601 = "设备编码不能为空";
        public const string MES12602 = "设备名称不能为空";
        public const string MES12603 = "请求实体不能为空！";
        #endregion

        #region 设备组 12700
        public const string MES12700 = "此编码{Code}在系统已经存在!";
        public const string MES12701 = "设备组编码不能为空";
        public const string MES12702 = "设备组名称不能为空";
        public const string MES12703 = "请求实体不能为空！";
        #endregion

        #region 故障现象 12900
        public const string MES12900 = "此编码{Code}在系统已经存在!";
        public const string MES12901 = "故障现象编码不能为空";
        public const string MES12902 = "故障现象名称不能为空";
        public const string MES12903 = "请求实体不能为空！";
        #endregion

        #region 故障原因 13000
        public const string MES13000 = "基础故障原因错误";
        public const string MES13001 = "站点码获取失败，请重新登录！";
        public const string MES13002 = "此故障原因编码{Code}在系统已经存在！";
        public const string MES13003 = "请求实体不能为空！";
        public const string MES13004 = "此标准故障原因不存在！";
        public const string MES13005 = "删除失败Ids 不能为空";
        public const string MES13006 = "故障原因已被设备故障原因或产品故障原因绑定，不允许删除!";
        public const string MES13007 = "修改故障原因关联类型失败!";
        public const string MES13008 = "故障原因状态不能为空";
        public const string MES13009 = "故障原因编码不能为空";
        public const string MES13010 = "故障原因名称不能为空";
        public const string MES13011 = "此故障原因编码{Code}在系统已经存在！";

        #endregion

        #region 供应商  15000
        public const string MES15000 = "库存供应商错误";
        public const string MES15001 = "站点码获取失败，请重新登录！";
        public const string MES15002 = "此供应商编码{Code}在系统已经存在！";
        public const string MES15003 = "请求实体不能为空！";
        public const string MES15005 = "删除失败Ids 不能为空";
        public const string MES15006 = "供应商编码不能为空";
        public const string MES15007 = "供应商名称不能为空";
        public const string MES15008 = "此供应商编码{Code}不符合规则，字母/数字！";


        #endregion

        #region 车间库存接收  15100
        public const string MES15100 = "物料库存错误";
        public const string MES15101 = "物料不存在";
        public const string MES15102 = "物料条码未关联到供应商";
        public const string MES15103 = "物料条码：{MaterialCode}数量需大于0";
        public const string MES15104 = " 物料条码：{MaterialCode}在车间库存中已存在！";
        public const string MES15105 = " 增加库存失败";
        public const string MES15106 = " 请扫描物料条码";
        public const string MES15107 = " 物料条码：{MaterialCode}重复扫描！";

        #endregion

        #region 物料台账 15200
        public const string MES15200 = "物料台账错误";
        public const string MES15201 = "物料编码不能为空";
        public const string MES15202 = "物料版本不能为空";
        public const string MES15203 = "物料条码不能为空";
        public const string MES15204 = "物料批次不能为空";
        public const string MES15205 = "物料数量不能为空";
        public const string MES15206 = "物料流程类型不能为空";
        public const string MES15207 = "物料流程类型不符合规则：1.物料接收/2.物料退料/3.物料加载";
        public const string MES15208 = "物料来源不能为空";
        public const string MES15209 = "物料来源不符合规则：1.手动录入/2.WMS/3.上料点编号";
        public const string MES152010 = "物料超出，一次最多扫描100个";
        public const string MES152011 = "物料不存在";
        public const string MES152012 = "物料条码：{MaterialCode}未关联到供应商";
        public const string MES152013 = "物料条码：{MaterialCode}数量需大于0";
        public const string MES152014 = " 条码：{MaterialCode}在车间库存中已存在！";
        public const string MES152015 = " 供应商不能为空";
        public const string MES152016 = " 条码：{MaterialCode}已存在！";


        #endregion

        #region 质量锁定 15300
        public const string MES15300 = "将来锁定工序必填";
        public const string MES15301 = "产品条码不能为空";
        public const string MES15302 = "条码已报废/删除，不可再操作锁定/取消锁定";
        public const string MES15303 = "将来锁定操作必须是同一工单下的条码";
        public const string MES15304 = "条码{sfcs}不是在制品";
        public const string MES15305 = "条码数量上限为100行";
        public const string MES15306 = "选中的条码状态与选择的操作类型不匹配！";
        public const string MES15307 = "扫描的条码状态都必须是“锁定”或者有未关闭的将来锁定指令存在";
        //public const string MES15308= " 当前条码状态为{operationType}，与选择的操作类型不匹配";
        public const string MES15308 = "将来锁定操作必须是同一工单下的条码";
        public const string MES15309 = "条码全部不是在制品";
        public const string MES15310 = "将来锁工序{lockproduction}不在条码所用工艺路线中";
        public const string MES15311 = "将来锁锁定工序不存在";
        public const string MES15312 = "条码不是在制品！";
        public const string MES15313 = "条码已经锁定，无法添加将来锁";
        public const string MES15314 = "锁定工序{sfcproduction}不在条码所在工序{lockproductionname}之后";
        public const string MES15315 = "条码存在及时锁定，无法添加及时锁";
        public const string MES15316 = "条码未被锁定，无法执行解锁操作";
        public const string MES15317 = "将来锁工序{lockproduction}不在条码所在工序之后";
        public const string MES15318 = "条码已经被锁定";
        #endregion

        #region 质量录入 15400
        public const string MES15400 = "产品条码不能为空";
        public const string MES15401 = "存在已报废的条码,不可再次报废!";
        public const string MES15402 = "条码不是在制品,不可再执行当前操作!";
        public const string MES15403 = "条码{sfcs}状态不是报废,不可再执行当前操作!";
        public const string MES15404 = "工单{orders}不是激活状态,不可再执行当前操作!";
        public const string MES15405 = "不合格缺陷信息不能为空!";
        public const string MES15406 = "已存在返修信息!";
        public const string MES15407 = "SFC {sfcs}已锁定，不可再执行当前操作!";
        public const string MES15408 = "存在未关闭的不合格信息，工艺路线为必填!";
        public const string MES15409 = "不合格代码{codes}已录入,请勿重复录入!";
        public const string MES15410 = "SFC {sfcs}已存在返修信息，不可再执行当前操作!";
        public const string MES15411 = "条码{sfcs}已报废,不可再执行当前操作！";
        #endregion

        #region 物料加载 15500
        public const string MES15501 = "当前线体无激活工单，请先激活工单！";
        #endregion

        #region 工单 16000
        public const string MES16000 = "工单错误";
        public const string MES16001 = "此工单编码{orderCode}在系统已经存在！";
        public const string MES16002 = "添加生产工单失败！";
        public const string MES16003 = "没有查找到该数据！请检查数据是否还存在";
        public const string MES16004 = "修改生产工单失败！";
        public const string MES16005 = "修改生产工单状态失败！";
        public const string MES16006 = "修改生产工单状态失败：有工单不为未开始,不允许重复下达";
        public const string MES16007 = "工单不为生产中,不允许锁定";
        public const string MES16008 = "工单没有被锁定,不需要解锁";
        public const string MES16009 = "锁定生产工单失败！";
        public const string MES16010 = "解锁生产工单失败！";
        public const string MES16011 = "修改生产工单状态失败：有工单不为生产中,不允许完工";
        public const string MES16012 = "修改生产工单状态失败：有工单不为完工,不允许关闭";
        public const string MES16013 = "工单状态不为未开始,不允许删除";
        public const string MES16014 = "有工单不存在";

        public const string MES16020 = "工单号 不能为空！";
        public const string MES16021 = "物料编码 不能为空！";
        public const string MES16022 = "数量 不能小于0！";
        public const string MES16023 = "产品Bom 不能为空！";
        public const string MES16024 = "工艺路线 不能为空！";
        public const string MES16025 = "工作中心 不能为空！";
        public const string MES16026 = "工单类型 不能为空！";
        public const string MES16027 = "计划开始时间 不能为空！";
        public const string MES16028 = "计划结束时间 不能为空！";
        public const string MES16029 = "计划开始时间必须要小于等于计划结束时间！";
        public const string MES16030 = "工单号 超过最大长度，最大长度为100！";
        public const string MES16031 = "备注 超过最大长度，最大长度为500！";

        public const string MES16032 = "工单Id 不能为空！";
        public const string MES16033 = "工单状态 不能为空！";

        public const string MES16034 = "超生产比例 不能小于0！";
        public const string MES16035 = "SFC对应工单状态为非生产中状态，不可执行操作！";
        public const string MES16036 = "SFC超过最大复投次数，不允许生产！";
        #endregion

        #region 条码接收 16100
        public const string MES16100 = "条码接收错误";
        public const string MES16101 = "接收类型不能为空";
        public const string MES16102 = "工单不能为空";
        public const string MES16103 = "关联工单不能为空";
        public const string MES16104 = "SN不能为空";
        public const string MES16105 = "SN：{SFC}已存在";

        public const string MES16106 = "工单:{OrderCode}状态不可用";
        public const string MES16107 = "工单:{OrderCode}没有匹配的SN:{SFC}";
        public const string MES16108 = "工单:{OrderCode}与关联工单产品不匹配";
        public const string MES16109 = "工单:{OrderCode}与关联工单不能一样";
        public const string MES16110 = "扫描条码失败";
        public const string MES16111 = "条码:{SFC}已使用，不允许删除";
        public const string MES16112 = "扫描SN与关联工单号不符，请重新扫描！";
        public const string MES16113 = "请配置当前工单产品的编码规则！";
        public const string MES16114 = "扫描SN与工单产品编码规则不符！基数为:{Base}位";
        public const string MES16115 = "扫描SN与工单产品编码规则不符！需包含{ValuesType}值:{SegmentedValue}";
        public const string MES16116 = "已使用的条码，不允许删除";

        public const string MES16117 = "工单{OrderCode}已经被锁定，无法继续生产";
        public const string MES16118 = "工单{OrderCode}状态为未开始，无法继续生产";
        public const string MES16119 = "工单{OrderCode}已经关闭，无法继续生产";
        public const string MES16120 = "库存不存在";
        public const string MES16121 = "不满足产品{product}的掩码规则";
        public const string MES16122 = "条码{sfc}已存在";
        public const string MES16123 = "条码{sfc}已经是在制状态";
        public const string MES16124 = "条码已存在";
        public const string MES16125 = "条码已经是在制状态";
        public const string MES16126 = "工单和关联工单不能一致";
        public const string MES16127 = "条码工单和关联工单必须一致";
        public const string MES16128 = "条码{sfc}不存在";
        public const string MES16129 = "条码物料和工单物料编码不一致";
        public const string MES16130 = "已报废的条码，不允许删除";
        #endregion

        #region 条码生成 MES16200
        public const string MES16200 = "条码生成失败";
        public const string MES16201 = "条码流水超过设定长度";
        public const string MES16202 = "基数只有 10,16,32";
        public const string MES16203 = "未找到条码生成规则";
        public const string MES16204 = "{base}进制字符串全部忽略,无法生成条码";
        public const string MES16205 = "通配符{value}未实现";
        public const string MES16206 = "流水号转换只实现了16,32进制";
        #endregion

        #region 生产通用 MES16300
        public const string MES16300 = "生产中异常。";
        public const string MES16301 = "工单不存在。";
        public const string MES16302 = "工单{ordercode}已经被锁定，无法继续生产。";
        public const string MES16303 = "工单{ordercode}状态不为已下达|生产中|已完工，无法继续生产。";
        public const string MES16304 = "获取首工序失败。";
        public const string MES16305 = "条码格式不合法。";
        public const string MES16306 = "条码不存在。";
        public const string MES16307 = "SFC状态不合法，不允许操作。";
        public const string MES16308 = "SFC不在当前工序排队，请检查。";
        public const string MES16309 = "SFC状态非活动，请先置于活动。";
        public const string MES16310 = "SFC状态为完成，不允许操作。";
        public const string MES16311 = "SFC在库存中状态为：{Status}，但不存在在制信息。";
        public const string MES16312 = "请求参数不合法，不允许操作。";
        public const string MES16313 = "SFC状态不是{Status}状态，不允许操作。";
        public const string MES16314 = "SFC条码{SFC}已锁定，不允许操作。";
        public const string MES16315 = "库存{barCode}和待加载物料编码/版本不符！";
        public const string MES16316 = "SFC条码{SFC}和资源不匹配！";
        public const string MES16317 = "工序和资源不匹配！";
        public const string MES16318 = "SFC状态为已完成，不允许操作！";
        public const string MES16319 = "SFC条码{SFC}已存在返修信息，不允许操作。";
        public const string MES16320 = "工单{ordercode}状态不为已下达|生产中，无法继续操作。";
        public const string MES16321 = "物料{Code}未添加组件，请检查";
        public const string MES16322 = "SFC条码{SFC}已报废，不允许操作。";
        #endregion

        #region 生产
        public const string MES17101 = "物料条码:{barCode}不存在！";

        #region 面板维护
        public const string MES17201 = "面板类型不能为空";
        public const string MES17202 = "面板编码不能为空";
        public const string MES17203 = "面板名称不能为空";
        public const string MES17204 = "面板状态不能为空";
        public const string MES17205 = "面板编码已经存在";
        public const string MES17206 = "面板编码最大长度为255";
        public const string MES17207 = "面板名称最大长度为255";
        public const string MES17208 = "面板有更新，请刷新页面！";
        #endregion


        #region 在制维修
        public const string MES17301 = "工序不能为空";
        public const string MES17302 = "资源不能为空";
        public const string MES17303 = "产品条码不能为空";
        public const string MES17304 = "更改产品条码生产状态失败";
        public const string MES17305 = "获取维修信息失败";
        public const string MES17306 = "获取条码生产信息失败";
        public const string MES17307 = "存在未关闭的缺陷，请检查！";
        public const string MES17308 = "返回工序失败！";
        public const string MES17309 = "当前面板不存在在制维修信息！";
        public const string MES17310 = "结束维修，保存数据失败！";
        public const string MES17311 = "未获取到工序信息";
        public const string MES17312 = "未获取到资源信息";
        public const string MES17313 = "未获取到工单信息";
        public const string MES17314 = "未获取到产品信息";
        public const string MES17315 = "未获取到在制维修信息";
        public const string MES17316 = "未获取到不良录入信息";
        public const string MES17317 = "更新条码生产状态失败";
        public const string MES17318 = "返回工序不能为空";
        public const string MES17319 = "作业:{key}执行失败";
        public const string MES17320 = "作业返回空，请检查作业是否正确配置";
        public const string MES17321 = "不识别的类型：{key}";
        public const string MES17322 = "请先开始维修";
        public const string MES17323 = "排队中";
        public const string MES17324 = "活动中";
        public const string MES17325 = "未获取到维修业务";


        #endregion

        #endregion

        #region 工单激活 MES16400
        public const string MES16400 = "工单激活错误";
        public const string MES16401 = "查询工单激活必须选择线体！";
        public const string MES16402 = "当前选择的线体不存在！";
        public const string MES16403 = "当前选择的不是线体！";
        public const string MES16404 = "当前工单已不存在！";
        public const string MES16405 = "工单[{orderCode}]状态为未开始，不允许激活！";
        public const string MES16406 = "工单[{orderCode}]已经是激活状态，无需再次激活！";
        public const string MES16407 = "工单[{orderCode}]不是激活状态，无需取消激活！";
        public const string MES16408 = "工单[{orderCode}]已被锁定，不允许激活！";
        public const string MES16409 = "当前线体不允许混线，请先取消激活工单[{orderCode}]！";
        public const string MES16410 = "工单状态未激活，不允许生产！";
        public const string MES16411 = "工单被锁定，不允许生产！";
        public const string MES16412 = "根据资源查询工单激活必须有资源Id参数！";
        public const string MES16413 = "没有找到该资源对应的工作中心";
        public const string MES16414 = "当前资源所对应的工作中心不是线体";
        #endregion

        #region 条码下达 MES16500
        public const string MES16500 = "下达条码失败。";
        public const string MES16501 = "产品{product}未维护编码规则,无法下达条码。";
        public const string MES16502 = "产品{product}批次大写为0,无法下达条码。";
        public const string MES16503 = "工单{workorder}超过计划数量,下达条码失败。";
        public const string MES16504 = "条码已经存在。";
        public const string MES16505 = "条码不存在，无法复用。";
        public const string MES16506 = "条码不为完成和在库状态，无法复用。";
        #endregion

        #region 在制品移除添加 16600

        public const string MES16600 = "条码不存在或不是在制品!";
        public const string MES16601 = "组件{CirculationBarCode}同SFC{SFC}已绑定,请检查!";
        public const string MES16602 = "数据不存在!";
        public const string MES16603 = "组件条码{barCode}不存在!";
        public const string MES16604 = "组件条码{barCode}库存不足,请检查!";
        public const string MES16605 = "组件条码{barCode}不符合掩码规则!";
        public const string MES16606 = "组件条码{barCode}同掩码规则不符,请检查!";
        public const string MES16607 = "选择的替换组件不存在!";
        public const string MES16608 = "组件条码{barCode}与选择的产品不一致!";
        public const string MES16609 = "找不到条码{barCode}对应物料的数据数据收集方式!";
        public const string MES16610 = "组件条码{barCode}对应的批次大小未维护!";
        public const string MES16611 = "组件条码{barCode}的批次大小超出可装载数量!";
        public const string MES16612 = "当前工序与条码生产信息中的不一致！";
        public const string MES16613 = "请选择活动状态下的组件移除！";
        public const string MES16614 = "请选择活动状态下的组件替换！";
        public const string MES16615 = "组件条码已装配所有组件,无需添加!";
        public const string MES16616 = "组件条码{barCode}未设置掩码规则!";
        public const string MES16617 = "条码已报废,不可再执行当前操作!";
        public const string MES16618 = "组件条码{barCode}的数据收集方式与主物料数据收集方式冲突!";
        #endregion

        #region 容器包装 MES 16700
        public const string MES16701 = "容器包装，条码信息未找到";
        public const string MES16702 = "容器包装，包装码不存在";
        public const string MES16703 = "容器包装，条码的包装维护记录未找到";
        public const string MES16704 = "容器包装，配置面板编号为空";
        public const string MES16705 = "容器包装，配置面板不存在或未启用";
        public const string MES16706 = "容器包装，配置面板不允许混工单,当前容器工单{first},当前条码工单{second}";
        public const string MES16707 = "容器包装，包装码不能为空";
        public const string MES16710 = "不识别的类型：{key}";
        public const string MES16708 = "产品条码不能为空";
        public const string MES16709 = "作业返回空，请检查作业是否正确配置";
        public const string MES16711 = "容器包装，配置面板不允许活动产品";
        public const string MES16712 = "容器包装，配置面板不允许完成产品";
        public const string MES16713 = "容器包装，配置面板不允许排队产品";
        public const string MES16714 = "容器包装，工序信息未找到";
        public const string MES16715 = "容器包装，工序中未指定包装等级";
        public const string MES16716 = "容器包装，不允许混物料版本包装 ";
        public const string MES16717 = "容器包装，超过了最大包装数量，不允许包装";
        public const string MES16718 = "容器包装，未找到该条码的子级包装记录";
        public const string MES16719 = "容器包装，物料编码没有维护容器规格";
        public const string MES16720 = "容器包装，条码已报废";
        public const string MES16721 = "容器包装，该条码{sfc}已装箱{barcode}";
        public const string MES16722 = "容器包装，该包装{packId}已关闭或删除";
        public const string MES16723 = "容器包装，包装数量未达到最小包装数，不允许关闭";
        public const string MES16724 = "容器包装，资源信息未找到";
        public const string MES16725 = "容器包装，资源对应资源类型和工序对应资源类型不一致";
        public const string MES16726 = "容器编码不存在";
        public const string MES16727 = "容器包装，二级包装只能装一级容器!";
        public const string MES16728 = "容器包装，三级包装只能装二级容器!";
        public const string MES16729 = "容器包装，容器{barcode}未关闭!";
        public const string MES16730 = "同一个容器不允许不同物料进行包装!";
        public const string MES16731 = "容器包装，容器包装等级跟工序包装等级不匹配!";
        #endregion

        #region 绑定工单激活  MES16800
        public const string MES16800 = "绑定工单激活错误";
        public const string MES16801 = "当前资源所对应的工作中心不是线体";
        public const string MES16802 = "有工单没有被激活，无法绑定";
        public const string MES16803 = "没有找到该资源对应的工作中心";
        public const string MES16804 = "有工单ID重复";
        #endregion

        #region 面板操作-生产过站面板 MES16900
        public const string MES16900 = "面板操作-生产过站面板错误";
        public const string MES16901 = "没有查找到对应条码的生产信息！";
        public const string MES16902 = "无法将主物料ID转为long类型！";
        public const string MES16903 = "当前工序与条码生产信息中的不一致！";
        public const string MES16904 = "找不到实际使用的物料信息!";
        public const string MES16905 = "找不到实际物料{materialCode}对应的数据收集方式!";
        public const string MES16906 = "物料选择不符合!";
        public const string MES16907 = "bom没有配置替代物料,物料选择不符合!";
        public const string MES16908 = "物料条码{barCode}库存不存在!";
        public const string MES16909 = "物料条码{barCode}库存不足,请检查!";
        public const string MES16910 = "实际使用的物料为空!";
        public const string MES16911 = "实际使用的物料与条码不合!";
        public const string MES16912 = "条码为空！";
        public const string MES16913 = "不能删除启用或保留的记录！";
        #endregion

        #region 条码打印 MES17000
        public const string MES17001 = "条码打印，打印模板文件未找到";
        public const string MES17002 = "条码打印，打印机信息未找到";
        public const string MES17003 = "条码打印，打印失败msg:{msg}.";
        #endregion

        #region 在制品步骤控制 MES18000
        public const string MES18000 = "在制品步骤控制错误";
        public const string MES18001 = "条码信息不存在";
        public const string MES18002 = "请扫描相同工单的条码";
        public const string MES18003 = "工单信息不存在";
        public const string MES18004 = "需扫描相同工艺路线条码";
        public const string MES18005 = "工艺路线不存在节点";
        public const string MES18006 = "条码:{SFC}不允许操作";
        public const string MES18007 = "条码没有对应的生产工序";
        public const string MES18008 = "条码生产状态异常:{Status}";
        public const string MES18009 = "工单状态不允许";
        public const string MES18010 = "条码已锁定不允许操作";
        public const string MES18011 = "获取工序信息失败";


        public const string MES18012 = "工序不能为空";
        public const string MES18013 = "类型不能为空";
        public const string MES18014 = "条码不能为空";
        public const string MES18015 = "条码:{SFC}已装箱，请先解包";
        public const string MES18016 = "在制品步骤控制入库失败";
        public const string MES18017 = "工艺路线不存在尾工序";
        public const string MES18018 = "工艺路线存在多个尾工序";
        public const string MES18019 = "条码:{SFCs}已包装，不允许操作";
        public const string MES18020 = "库存信息不存在";
        public const string MES18021 = "物料库存不足";
        public const string MES18022 = "条码{SFC}不存在或已报废，不允许操作";


        #endregion

        #region 车间作业控制 报告 MES18100
        public const string MES18100 = "车间作业控制报告错误";
        public const string MES18101 = "没有获取到{sfc}对应步骤的信息";
        public const string MES18102 = "没有获取到{sfc}对应步骤中的工单信息";
        public const string MES18103 = "没有获取到{sfc}对应步骤中的产品信息";
        public const string MES18104 = "没有获取到{sfc}对应步骤中的工艺路线信息";
        public const string MES18105 = "没有获取到{sfc}对应步骤中的BOM信息";

        public const string MES18110 = "没有传入SFC条码信息";
        #endregion

        #region 生产更改 MES18200
        public const string MES18200 = "最大扫描{number}个";
        public const string MES18201 = "条码不能为空";
        public const string MES18202 = "工单不能为空";
        public const string MES18203 = "工序不能为空";

        public const string MES18204 = "条码:{SFC}已锁定";
        public const string MES18205 = "条码:{SFC}的工单状态不允许操作";
        public const string MES18206 = "工单必须相同";
        public const string MES18207 = "新工单可生产数量不足";
        public const string MES18208 = "未获取到工艺路线节点信息";
        public const string MES18209 = "工单:{Code}状态不允许操作！";
        public const string MES18210 = "生产更改错误";
        public const string MES18211 = "条码非在制状态，不允许操作";


        #endregion

        #region 设备对接错误

        #region 设备对接公用
        public const string MES19001 = "设备编码不能为空";
        public const string MES19002 = "资源编码不能为空";
        public const string MES19003 = "SFC条码不能为空";
        public const string MES19004 = "调用本地时间不能为空";
        public const string MES19005 = "设备编码不存在";
        public const string MES19006 = "资源编码不存在";
        public const string MES19007 = "SFC条码不允许重复";
        #endregion

        public const string MES19101 = "条码列表不能为空";
        public const string MES19102 = "容器编码不能为空";
        public const string MES19103 = "容器条码列表不能为空";
        public const string MES19104 = "托盘装载信息不存在";
        public const string MES19105 = "此托盘未装载任何信息";
        public const string MES19106 = "不存在任何绑定关系不需要解绑";
        public const string MES19107 = "参数列表不能为空";
        public const string MES19108 = "参数编码{Code}不存在";
        public const string MES19109 = "资源编码{Code}不存在";
        public const string MES19110 = "产品条码参数列表不能为空";
        public const string MES19111 = "托盘条码不能为空";
        public const string MES19112 = "上传SFC信息不能为空";
        public const string MES19113 = "该设备已经设置临时SFC";
        public const string MES19114 = "NG编码{Code}不存在";
        public const string MES19115 = "产品编码不能为空";
        public const string MES19116 = "请维护产品编码：{Code}的编码规则";
        public const string MES19117 = "条码：{SFC}不存在";
        public const string MES19118 = "产品编码：{Code}不存在";



        #endregion

        #endregion

        #region 系统执行出错 业务逻辑出错
        //public const string MES20001 = "MES20001";
        //public const string MES20100 = "MES20100";
        //public const string MES20101 = "MES20101";

        #endregion

        #region 调用第三方服务出错
        //public const string MES30001 = "MES30001";
        //public const string MES30100 = "MES30100";
        //public const string MES30101 = "MES30101";
        #endregion

    }
}
