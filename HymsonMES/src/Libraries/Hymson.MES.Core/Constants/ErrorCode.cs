namespace Hymson.MES.Core.Constants
{
    /// <summary>
    /// 错误码
    /// </summary>
    public static class ErrorCode
    {
        #region  用户端错误 
        public const string MES10100 = "请求实体不能为空";
        public const string MES10101 = "站点码获取失败，请重新登录！";
        public const string MES10102 = "删除失败Id 不能为空!";
        public const string MES10103 = "请求参数格式错误!";
        public const string MES10104 = "请求数据不存在!";
        public const string MES10105 = "有生产中工单引用当前物料，不能删除！";
        public const string MES10106 = "只能删除新建状态的数据";
        public const string MES10107 = "第{0}行:";
        public const string MES10108 = "无法将其他状态修改成新建状态!";
        public const string MES10109 = "编码最大长度为50";
        public const string MES10110 = "名称最大长度为50";
        public const string MES10111 = "参数不能为空";
        public const string MES10112 = "站点不能为空";
        public const string MES10113 = "编码不能为空";
        public const string MES10114 = "编码不允许有空格";
        public const string MES10115 = "编码最大长度为100";
        public const string MES10116 = "名称不能为空";
        public const string MES10117 = "名称最大长度为100";
        public const string MES10118 = "版本不能为空";
        public const string MES10119 = "版本最大长度为100";
        public const string MES10120 = "状态不合法";
        public const string MES10121 = "描述最大长度为255";
        public const string MES10122 = "版本不允许有空格";
        public const string MES10123 = "启用状态不允许修改";
        public const string MES10124 = "启用状态只能修改为保留或废除";
        public const string MES10125 = "变更状态的ID不能为空";
        public const string MES10126 = "变更状态的状态不合法";
        public const string MES10127 = "状态已经是[{status}],无法重复设置";
        public const string MES10128 = "变更状态的状态不能为新建";
        public const string MES10129 = "只有新建或者保留才能编辑";


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
        public const string MES10226 = "数据收集方式不能为空！";
        public const string MES10227 = "数据收集方式不合法！";
        public const string MES10228 = "批次需大于0！";
        public const string MES10229 = "采购类型不合法！";
        public const string MES10230 = "状态不合法！";
        public const string MES10231 = "版本不能为空！";
        public const string MES10232 = "工艺路线{code}没有找到对应的数据！";
        public const string MES10233 = "Bom {bomCode}没有找到对应的数据！";
        public const string MES10234 = "掩码{code}没有找到对应的数据！";
        public const string MES10235 = "批次需为大于0整数！";
        public const string MES10236 = "是否默认版本不合法！";
        public const string MES10237 = "标包数量需为大于0整数！";
        public const string MES10238 = "基于时间不合法！";
        public const string MES10239 = "消耗公差需为大于0整数！";
        public const string MES10240 = "消耗系数需大于0！";
        #endregion

        #region 资源 10300
        public const string MES10301 = "资源编码不能为空";
        public const string MES10302 = "资源编码最大长度不超过50";
        public const string MES10303 = "资源名称不能为空";
        public const string MES10304 = "资源名称最大长度不超过50";
        public const string MES10305 = "资源状态不能为空";
        public const string MES10306 = "资源配置打印机中，重复配置设备!";
        public const string MES10307 = "一个资源只能对应一个主设备";
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

        public const string MES10380 = "资源状态不合法，请检查！";
        public const string MES10381 = "打印机不能为空！";
        public const string MES10382 = "设备不能为空！";
        public const string MES10383 = "资源设置类型不能为空！";
        public const string MES10384 = "资源设置值不能为空！";
        public const string MES10385 = "作业关联点不能为空！";
        public const string MES10386 = "作业不能为空！";
        public const string MES10387 = "作业参数不能为空！";
        public const string MES10388 = "资源不存在！";
        #endregion

        #region 标签模板 MES10340
        public const string MES10340 = "模板名称已经存在!";
        public const string MES10341 = "打印机名称重复!";
        public const string MES10342 = "模板名称最大长度为50";
        public const string MES10343 = "模板名称不能为空!";
        public const string MES10344 = "存储路径不能为空!";
        public const string MES10345 = "模板文件不能为空!";
        public const string MES10346 = "打印机名称不能为空!";
        public const string MES10347 = "打印机IP不能为空!";
        public const string MES10348 = "打印机IP重复！";
        public const string MES10349 = "模板预览内容为空！";
        public const string MES10350 = "未找到指定模板！";
        public const string MES10356 = "获取模板上下文信息失败,模板名称:{name}！";
        public const string MES10370 = "存储路径超长，最大255!";
        public const string MES10371 = "IP最大长度为50";

        public const string MES10372 = "模板没有打印设计!";
        public const string MES10373 = "不存在[{class}打印类]";
        public const string MES10374 = "获取打印数据ID为空";

        #endregion

        #region 工序 10400
        public const string MES10401 = "工序编码不能为空";
        public const string MES10402 = "工序编码最大长度不超过50";
        public const string MES10403 = "工序名称不能为空";
        public const string MES10404 = "工序名称最大长度不超过50";
        public const string MES10405 = "编码:{Code}已存在！";
        public const string MES10406 = "工序不存在！";
        public const string MES10407 = "工序类型不合法，请检查！";
        public const string MES10408 = "工序状态不合法，请检查！";
        public const string MES10409 = "参数不能为空，请检查！";
        public const string MES10410 = "作业关联点不合法，请检查！";
        public const string MES10411 = "关联物料不能为空，请检查！";
        public const string MES10412 = "作业参数不能为空！";

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
        public const string MES10450 = $"版本最大长度为50";
        public const string MES10451 = $"状态不合法";
        public const string MES10452 = $"类型不合法";
        public const string MES10453 = $"工艺路线不能为空";
        public const string MES10454 = $"工艺路线的线条不能为空集合";
        public const string MES10455 = $"工艺路线的工序不能为空集合";
        public const string MES10456 = $"线条序号不能为空";
        public const string MES10457 = $"线条中起点工序不能为空";
        public const string MES10458 = $"线条中终点工序不能为空";
        public const string MES10459 = $"线条中扩展信息不能为空";
        public const string MES10460 = $"工序节点序号不能为空";
        public const string MES10461 = $"工序节点工序ID不能为空";
        public const string MES10462 = $"工序节点编码不能为空";
        public const string MES10463 = $"工序节点名称不能为空";
        public const string MES10464 = $"工序节点工序类型不能为空";
        public const string MES10465 = $"工序节点抽检类型不能为空";
        public const string MES10466 = $"工序节点是否报工不能为空";
        public const string MES10467 = $"工序节点是否首工序不能为空";
        public const string MES10468 = $"工序节点中扩展信息不能为空";
        public const string MES10469 = $"工序节点中工序类型不合法";
        public const string MES10470 = $"工序节点中抽检类型不合法";
        public const string MES10471 = $"工序节点中是否报工不合法";
        public const string MES10472 = $"工序节点中是否首工序不合法";
        public const string MES10473 = $"工序节点中，抽检类型为固定比例时，抽检比例需要大于等于2";
        public const string MES10474 = $"工序节点手动排序号不能为空";
        public const string MES10475 = $"工序节点手动排序号最大长度为18";

        public const string MES10476 = $"工序不存在";
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
        public const string MES10512 = "参数单位最大长度为50";
        public const string MES10513 = "参数类型不合法，请检查！";
        public const string MES10514 = "参数ID不能为空";
        public const string MES10515 = "标准参数名称最大长度为50";
        public const string MES10516 = "参数{Code}的规格上限和规格下限配置异常，上限不能小于下限！";

        public const string MES10517 = "工作中心不能为空";
        public const string MES10518 = "产品不能为空";
        public const string MES10519 = "工序不能为空";
        public const string MES10520 = "此编码{Code}版本{Version}已存在，请重新输入！";
        public const string MES10521 = "此编码{Code}已存在，请重新输入！";
        public const string MES10522 = "车间不能为空";
        public const string MES10523 = "产品{ProductCode}+工序{ProcedureCode}启用状态的已存在，请重新输入！";
        public const string MES10524 = "工作中心{WorkCenterCode}+工序{ProcedureCode}启用状态的已存在，请重新输入！";
        public const string MES10525 = "工作中心{WorkCenterCode}+工序{ProcedureCode}+版本{Version}已存在，请重新输入！";
        public const string MES10526 = "产品{ProductCode}+工序{ProcedureCode}+版本{Version}已存在，请重新输入！";
        public const string MES10527 = "数据类型不合法";
        public const string MES10528 = "条码不在当前工序";
        public const string MES10529 = "没有当前条码与工序的产品参数收集组";
        public const string MES10530 = "条码或载具编码不能为空";
        public const string MES10531 = "条码[{sfc}]没有找到对应的生产信息";
        public const string MES10532 = "条码[{sfc}]不在当前工序";
        public const string MES10533 = "条码[{sfc}]不在当前工序活动";
        public const string MES10534 = "有条码不是同一个产品";
        public const string MES10535 = "载具没有绑定条码";

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
        public const string MES10617 = "状态不合法，请检查";
        public const string MES10618 = "版本不能为空";
        public const string MES10619 = "用量不能为空";
        public const string MES10620 = "收集方式不合法，请检查！";

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
        public const string MES10717 = "状态不合法，请检查!";

        public const string MES10718 = "关联物料不能为空，请检查!";
        public const string MES10719 = "关联资源不能为空，请检查!";
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
        public const string MES10809 = "状态不合法，请检查";
        public const string MES10810 = "掩码编码不能为空";
        public const string MES10811 = "掩码编码最大长度为50";
        public const string MES10812 = "掩码名称最大长度为50";


        #endregion

        #region 异常消息 10900
        public const string MES10901 = "事件类型不能为空";
        public const string MES10902 = "编码生成失败";
        public const string MES10903 = "消息状态不合法";
        public const string MES10904 = "原因分析不允许有空格";
        public const string MES10905 = "处理方案不允许有空格";
        public const string MES10906 = "推送对象不允许有空格";
        public const string MES10907 = "私钥不允许有空格";
        public const string MES10908 = "关键字不允许有空格";
        #endregion

        #region 降级规则 11000
        public const string MES11000 = "降级规则错误";
        public const string MES11001 = "对应降级规则不存在";
        public const string MES11002 = "降级规则编码已存在";
        public const string MES11003 = "降级规则编码不能为空";
        public const string MES11004 = "降级规则名称不能为空";
        public const string MES11005 = "降级规则编码最大长度为100";
        public const string MES11006 = "降级规则名称最大长度为100";
        public const string MES11007 = "描述最大长度为255";
        public const string MES11008 = "降级规则编码不允许有空格";
        public const string MES11009 = "Id不合法";
        public const string MES11010 = "参数为空";
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
        public const string MES11112 = "状态不合法，请检查!";
        public const string MES11113 = "类型不合法，请检查!";
        public const string MES11114 = "等级不合法，请检查!";
        public const string MES11115 = "该不合格代码不存在";
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

        #region 分选规则  11300
        public const string MES11300 = "分选规则错误";
        public const string MES11301 = "分选规则编码不能为空";
        public const string MES11302 = "分选规则名称不能为空";
        public const string MES11303 = "分选规则版本不能为空";
        public const string MES11304 = "分选规则物料不能为空";
        public const string MES11305 = "分选规则状态不能为空";
        public const string MES11306 = "参数的最大值不能小于最小值";
        public const string MES11307 = "当前编码和版本已存在";
        public const string MES11308 = "当前物料已存在的分选规则编码与当前编码不一致";
        public const string MES11309 = "分选规则不存在";
        public const string MES11310 = "分选规则编码长度为50";
        public const string MES11311 = "分选规则名称长度为50";
        public const string MES11312 = "分选规则版本长度为10";
        public const string MES11313 = "分选规则备注长度为255";
        public const string MES11314 = "参数集合中有交集";
        public const string MES11315 = "查询产品分选条码不能为空";
        #endregion

        #region 降级录入/移除 11400
        public const string MES11400 = "降级录入/移除错误";
        public const string MES11401 = "缺少降级编码参数";
        public const string MES11402 = "缺少产品序列码参数";
        public const string MES11403 = "产品序列码【{sfc}】没有找到相关信息";
        public const string MES11404 = "产品序列码【{sfc}】状态为报废";
        public const string MES11405 = "产品序列码【{sfc}】状态为锁定";
        public const string MES11406 = "不存在降级编码【{code}】的相关信息";
        public const string MES11407 = "产品序列码[{sfc}]没有录入降级编码，无法移除！";
        public const string MES11408 = "产品序列码【{sfc}】的降级规则不存在";
        public const string MES11409 = "当前录入的等级高于产品序列码【{sfc}】的降级等级";
        public const string MES11410 = "降级移除失败:有数据已经被移除了,请清除后重新处理";

        #endregion

        #region ESOP维护 11500
        public const string MES11500 = "ESOP已存在同一物料、工序、状态数据";
        public const string MES11501 = "不能删除启用状态的数据";
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
        public const string MES12010 = "作业不存在，请刷新页面";
        public const string MES12011 = "类程序不合法，请检查";
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
        public const string MES12122 = "工作中心状态不能为空.";
        public const string MES12123 = "工作中心状态不合法，请检查.";
        public const string MES12124 = "工作中心类型不合法，请检查.";
        public const string MES12125 = "对应工作中心数据不存在.";
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

        public const string MES12435 = "编码规则编码类型为包装序列码时，包装等级需要有对应的值！";
        public const string MES12436 = "编码规则忽略字符输入规则为只能是大写字母，用英文分号隔开";
        public const string MES12437 = "编码规则组成取值方式 不合法！";
        public const string MES12438 = "编码规则组成必须要有一行取值方式为可变值且分段值为%ACTIVITY%的数据！";
        public const string MES12439 = "编码规则重置序号 不合法！";
        public const string MES12440 = "编码规则初始值需要大于等于1的整数！";
        public const string MES12441 = "编码规则基数不合法！";

        public const string MES12442 = "编码模式 不能为空！";
        public const string MES12443 = "编码模式 不合法！";
        public const string MES12444 = "编码规则组成只能有一行取值方式为可变值且分段值为%ACTIVITY%的数据！";
        public const string MES12445 = "编码规则组成最多只有一行取值方式为可变值且分段值为%MULTIPLE_VARIABLE%的数据！";
        public const string MES12446 = "编码规则组成取值方式为可变值且分段值为%MULTIPLE_VARIABLE%的数据需要填写自定义值！";
        public const string MES12447 = "编码规则组成取值方式为可变值且分段值为%MULTIPLE_VARIABLE%数据中自定义值的区间值存在重复！";
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
        public const string MES12510 = "非新建状态的数据不能修改为新建状态";
        public const string MES12511 = "状态不合法，请检查";
        public const string MES12512 = "保证等级不合法，请检查";
        public const string MES12513 = "没有找到对应的容器信息";
        #endregion

        #region 设备 12600
        public const string MES12600 = "此编码{Code}在系统已经存在!";
        public const string MES12601 = "设备编码不能为空";
        public const string MES12602 = "设备名称不能为空";
        public const string MES12603 = "请求实体不能为空！";
        public const string MES12604 = "设备信息不存在！";
        public const string MES12605 = "状态不合法，请检查！";
        public const string MES12606 = "存放位置不能为空";

        public const string MES12610 = "设备验证账号不能为空";
        public const string MES12611 = "设备验证密码不能为空";
        public const string MES12612 = "设备验证账号不允许有空格";
        public const string MES12613 = "设备验证账号最大长度为50";
        public const string MES12614 = "设备验证密码最大长度为50";
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
        public const string MES12904 = "设备组不能为空！";
        public const string MES12905 = "设备故障现象不存在！";
        public const string MES12906 = "状态不合法！";
        public const string MES12907 = "状态不能为空！";
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
        public const string MES13012 = "状态不合法，请检查!";
        public const string MES13013 = "设备故障原因不存在！";
        #endregion

        #region IPQC检验项目 13100

        public const string MES13101 = "参数集编码不能为空！";
        public const string MES13102 = "检验类型不合法！";
        public const string MES13103 = "样本数量必须大于0且小于等于10000！";
        public const string MES13104 = "生成条件必须大于0且小于等于10000！";
        public const string MES13105 = "生成条件单位不合法！";
        public const string MES13106 = "管控时间必须大于0且小于等于10000！";
        public const string MES13107 = "管控时间单位不合法！";
        public const string MES13108 = "版本不能为空！";
        public const string MES13109 = "版本长度不能超过50！";
        public const string MES13110 = "状态不合法！";
        public const string MES13111 = "参数项目重复！";
        public const string MES13112 = "检验规则-检验方式重复！";
        public const string MES13121 = "启用状态数据参数项目不能为空！";
        public const string MES13122 = "启用状态数据检验规则与资源不能为空！";
        public const string MES13123 = "检验规则【{Way}】下关联资源不能为空！";
        public const string MES13151 = "参数集编码【{Code}】已存在相同生成条件【{Condition}】检验类型【{Type}】版本【{Version}】的IPQC检验项目，请重新输入！";
        public const string MES13152 = "参数项目与所选参数集不匹配！";

        #endregion

        #region 首检 13200

        public const string MES13201 = "生成条件不合法！";
        public const string MES13202 = "工单不能为空！";
        public const string MES13203 = "工序不能为空！";
        public const string MES13204 = "资源不能为空！";
        public const string MES13205 = "检验值不能为空！";
        public const string MES13206 = "是否合格不能为空！";
        public const string MES13221 = "工单不存在！";
        public const string MES13222 = "资源不属于该工序！";
        public const string MES13223 = "IPQC检验项目不存在，请先维护并启用！";
        public const string MES13224 = "IPQC检验项目错误，该产品、工序存在多个启用的IPQC检验项目，请检查数据！";
        public const string MES13225 = "该资源不在对应IPQC检验项目关联资源列表中，请先进行关联！";
        public const string MES13226 = "没有检验类型为首检的启用状态IPQC检验项目，请先维护！";
        public const string MES13227 = "没有需要生成首检单的工单！";
        public const string MES13228 = "没有可生成首件检验单的数据！";
        public const string MES13229 = "该检验单已完成，不允许再进行当前操作！";
        public const string MES13230 = "只有检验中的单据才允许完成！";
        public const string MES13231 = "已检样品数量小于单据应检数量，不允许完成！";
        public const string MES13232 = "该检验单非完成状态，不允许进行不合格处理！";
        public const string MES13233 = "存在非待检验状态的单据，不允许删除！";
        public const string MES13234 = "样品条码【{SampleCode}】已检验，不能重复检验！";
        public const string MES13235 = "样品条码【{SampleCode}】不存在！";
        public const string MES13236 = "样品条码【{SampleCode}】已经报废！";
        public const string MES13237 = "条码工单和检验单工单不一致！";
        #endregion

        #region 过程检 13300


        #endregion

        #region 尾检 13400


        #endregion

        #region  14000段项目使用 平台请勿使用

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
        public const string MES15009 = "供应商编码最大长度50";
        public const string MES15010 = "供应商名称最大长度50";
        public const string MES15011 = "供应商已关联物料，不允许删除！";


        #endregion

        #region 车间库存接收  15100
        public const string MES15100 = "物料库存错误";
        public const string MES15101 = "物料不存在";
        public const string MES15102 = "物料条码未关联到供应商";
        public const string MES15103 = "物料条码：{MaterialCode}数量需大于0";
        public const string MES15104 = " 物料条码：{MaterialCode}在车间库存中已存在！";
        public const string MES15105 = " 增加库存失败";
        public const string MES15106 = " 请扫描物料条码";
        public const string MES15107 = " 重复扫描！";
        public const string MES15108 = " 供应商不能为空";

        public const string MES15120 = " 没有查询到对应原材料库存数据";
        public const string MES15121 = " 维护库存剩余数量要不大于接收数量且不小于0";
        public const string MES15122 = " 物料条码{materialBarCode}状态{status},不允许执行该操作！";
        public const string MES15123 = " 不允许操作非原材料库存！";
        public const string MES15124 = " 没有查询到对应库存数据";
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
        public const string MES15209 = "物料来源不符合规则：1.手动录入/2.WMS";
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

        public const string MES15320 = "条码【{sfcs}】已不存在";
        public const string MES15321 = "条码【{sfc}】缺失步骤信息，无法查询到开始时间";
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
        public const string MES15412 = "条码{sfcs}已取消报废,不能重复取消!";
        public const string MES15413 = "条码{sfcs}已报废,不能重复报废!";
        public const string MES15414 = "条码{sfcs}标识已取消,不能重复取消!";
        public const string MES15415 = "条码不存在";
        public const string MES15416 = "条码已经被锁定";
        public const string MES15417 = "条码已报废";
        public const string MES15418 = "条码工单未激活,无法返修";
        public const string MES15419 = "条码状态已经被更新，请刷新后再操作";
        public const string MES15420 = "返工工艺路线与条码工艺路线不能一致";
        public const string MES15421 = "条码无库存，无法返修";
        public const string MES15422 = "条码库存锁定，无法返修";
        public const string MES15423 = "无不合格信息，不需要复判";
        public const string MES15424 = "正在返修中，不需要复判";
        public const string MES15425 = "不合格信息最新状态,请刷新后再次操作";
        public const string MES15426 = "条码状态不为最新新状态,请刷新后再次操作";
        public const string MES15427 = "条码工单未激活,无法取消报废";
        public const string MES15428 = "数据有变动，请刷新后再操作!";
        public const string MES15430 = "载具编码不能为空";
        #endregion

        #region 物料加载 15500
        public const string MES15501 = "当前线体无激活工单，请先激活工单！";
        // MES15502-MES15503 已被占用
        public const string MES15504 = "未找到资源关联的产线！";
        public const string MES15505 = "未找到该条码相匹配的物料！";
        public const string MES15506 = "该条码与选定的物料不匹配！";
        public const string MES15507 = "该条码{BarCode}已存在于当前上料点！";
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
        public const string MES16015 = "工单没有锁定前的状态，无法解锁";

        public const string MES16020 = "工单号 不能为空！";
        public const string MES16021 = "物料编码 不能为空！";
        public const string MES16022 = "数量 必须大于0！";
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
        public const string MES16037 = "工单状态已经被修改，请刷新重试";
        public const string MES16038 = "编码类型不合法，请检查！";
        public const string MES16039 = "工作中心不合法，请检查！";
        public const string MES16040 = "工作中心类型不合法，请检查！";
        public const string MES16041 = "工单类型不合法，请检查！";
        public const string MES16042 = "超产比例需大于0";
        public const string MES16043 = "工单号长度最大50";
        public const string MES16044 = "数量需为整数且大于0";
        public const string MES16045 = "工单状态不合法！";
        public const string MES16046 = "工单状态不为未开始，不能编辑！";
        public const string MES16047 = "条码{SFC}超过最大复投次数{Cycle}，当前复投次数{RepeatedCount}，不允许生产！";
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
        public const string MES16129 = "条码物料和新工单物料编码不一致";
        public const string MES16130 = "已报废的条码，不允许删除";
        public const string MES16131 = "组件条码对应的批次大小未维护";
        public const string MES16132 = "条码{sfc}已经被装箱了";
        public const string MES16133 = "已经被装箱了";
        public const string MES16134 = "但在bom属性为外部，条码已经存在";
        public const string MES16135 = "条码不在工单{WorkOrder}的bom中";
        public const string MES16136 = "库存不足";
        public const string MES16137 = "物料{MaterialCode}批次数量大于0";
        public const string MES16138 = "条码状态不为最新，请刷新后再操作";
        #endregion

        #region 条码生成 MES16200
        public const string MES16200 = "条码生成失败";
        public const string MES16201 = "条码流水超过设定长度";
        public const string MES16202 = "基数只有 10,16,32";
        public const string MES16203 = "未找到条码生成规则";
        public const string MES16204 = "{base}进制字符串全部忽略,无法生成条码";
        public const string MES16205 = "通配符{value}未定义";
        public const string MES16206 = "流水号转换只实现了16,32进制";
        public const string MES16207 = "生成的序列号{BarCode}超过规则限制";
        public const string MES16208 = "统配失败-统配编码{code},类型{type}查询不到值";
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
        public const string MES16323 = "物料未添加组件，请检查";
        public const string MES16324 = "存在已报废SFC条码，不允许操作。";
        public const string MES16325 = "存在已锁定SFC条码，不允许操作。";
        public const string MES16326 = "存在SFC状态不是{Status}状态，不允许操作。";
        public const string MES16330 = "工序存在多个";
        public const string MES16331 = "工艺路线存在多个";
        public const string MES16332 = "SFC列表不能为空";
        public const string MES16333 = "SFC条码和资源不匹配！";
        public const string MES16334 = "资源不能为空！";
        public const string MES16335 = "工序不能为空！";
        public const string MES16336 = "{SFC}非在制状态，不允许操作！";
        public const string MES16337 = "资源不存在";
        public const string MES16338 = "设备不存在";
        public const string MES16339 = "设备不能为空";

        public const string MES16340 = "条码{0}已中止！";
        public const string MES16341 = "条码{0}完成，已于NF排队！";
        public const string MES16342 = "条码{0}开始录入不良";
        public const string MES16343 = "条码{0}已经完成录入，无需重复录入！";
        public const string MES16344 = "关闭成功";
        public const string MES16345 = "该容器已经关闭！";
        public const string MES16346 = "打开成功！";
        public const string MES16347 = "该容器已经打开！";
        public const string MES16348 = "条码{0}已于NF排队！";
        public const string MES16349 = "条码{0}出站完成！";
        public const string MES16350 = "工单状态为完工，不允许再对工单投入！";
        public const string MES16351 = "条码{0}完成，已于{1}排队！";
        public const string MES16352 = "工序不存在";
        public const string MES16353 = "产品序列码【{SFC}】已被Marking拦截！";
        public const string MES16354 = "当前进站工序【{Current}】，属于应进站工序【{Procedure}】的前面工序，不允许操作。";
        public const string MES16355 = "未找到工序【{ProcedureCode}】关联的资源";
        public const string MES16357 = "当前工序是【{Current}】，条码【{SFC}】应在工序【{Procedure}】处排队进站，请检查。";
        public const string MES16358 = "指定的工序【{Procedure}】不存在";
        public const string MES16359 = "产品序列码【{SFC}】由工序【{InProcedure}】进站，与当前出站工序【{OutProcedure}】不一致，不允许出站操作。";
        public const string MES16360 = "产品序列码【{SFC}】循环次数【{Current}】不允许启动！";//"条码【{SFC}】当前循环次数【{Current}】已达到当前工序的循环次数上限【{Cycle}】，不允许操作。";
        public const string MES16361 = "产品序列码【{SFC}】当前状态【{Current}】，不是【{Status}】状态，不允许操作。";
        public const string MES16362 = "产品序列码【{SFC}】已被将来锁锁定，锁定工序【{Procedure}】，不允许操作。";
        public const string MES16363 = "参数收集开始！";
        public const string MES16364 = "未找到条码的产品参数信息！";
        public const string MES16365 = "获取不到条码【{SFC}】的等级信息";
        public const string MES16366 = "获取不到条码【{SFC}】的最终档次信息";
        public const string MES16367 = "产品序列码【{SFC}】所记录的工单不存在";
        public const string MES16368 = "产品序列码【{SFC}】已在原工序【{Procedure}】处不合格出站【{Cycle}】次，请继续复投。";
        public const string MES16369 = "产品序列码【{SFC}】指定的工序【{Procedure}】不存在";
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
        public const string MES16415 = "工单[{orderCode}]处于暂停中，无法操作！";
        #endregion

        #region 条码下达 MES16500
        public const string MES16500 = "下达条码失败。";
        public const string MES16501 = "产品{product}未维护编码规则,无法下达条码。";
        public const string MES16502 = "产品{product}批次大写为0,无法下达条码。";
        public const string MES16503 = "工单{workorder}超过计划数量,下达条码失败。";
        public const string MES16504 = "条码已经存在。";
        public const string MES16505 = "条码不存在，无法复用。";
        public const string MES16506 = "条码不为已完成状态，无法复用。";
        public const string MES16507 = "资源和工序对应的资源类型不匹配。";
        public const string MES16508 = "下达数量不合法。";
        public const string MES16509 = "工序不在工艺路线上。";
        public const string MES16510 = "工序不存在。";
        public const string MES16511 = "资源编码{Code}未与线体绑定";
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
        public const string MES16619 = "当前条码【{Current}】的位置号【{Location}】已被【{BarCode}】使用！";
        #endregion

        #region 容器包装 MES 16700
        public const string MES16701 = "容器包装，条码信息未找到";
        public const string MES16702 = "容器包装，包装码不存在";

        public const string MES16704 = "容器包装，配置面板编号为空";
        public const string MES16705 = "容器包装，配置面板不存在或未启用";
        public const string MES16706 = "容器包装，配置面板不允许混工单,当前容器工单{first},当前条码工单{second}";

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
        public const string MES16732 = "装载信息不存在!";
        public const string MES16733 = "容器已打开!";
        public const string MES16734 = "容器已关闭!";
        public const string MES16735 = "产品{product}未维护包装编码规则,无法创建容器";
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
        public const string MES16904 = "找不到实际使用的物料信息!";
        public const string MES16905 = "找不到实际物料{materialCode}对应的数据收集方式!";

        public const string MES16908 = "物料条码{barCode}库存不存在!";
        public const string MES16909 = "物料条码{barCode}库存不足,请检查!";
        public const string MES16910 = "实际使用的物料为空!";
        public const string MES16911 = "实际使用的物料与条码不合!";

        public const string MES16913 = "不能删除启用或保留的记录！";
        public const string MES16914 = "未找到能挂载物料条码的主物料!";
        #endregion

        #region 条码打印 MES17000
        public const string MES17001 = "条码打印，打印模板文件未找到";
        public const string MES17002 = "条码打印，打印机信息未找到";
        public const string MES17003 = "条码打印，打印失败msg:{msg}.";
        public const string MES17004 = "条码打印，预览失败";
        public const string MES17005 = "条码打印，模板数据格式错误";
        #endregion

        #region 生产MES17100
        public const string MES17101 = "物料条码:{barCode}不存在！";
        public const string MES17102 = "获取条码{SFC}信息失败！";
        public const string MES17103 = "获取出站产品信息失败！";
        public const string MES17104 = "获取条码信息失败！";
        public const string MES17105 = "存在不在BOM消耗清单里面的消耗条码！";
        public const string MES17106 = "产品【{Ids}】信息读取失败！";
        public const string MES17107 = "消耗条码{BarCodes}不属于BOM消耗清单！";
        public const string MES17108 = "条码【{SFCs}】存在物未关闭的不合格信息，请去【不合格报告】查看详细信息";
        public const string MES17109 = "条码【{SFC}】对应的不合代码信息为空，请检查！";
        public const string MES17110 = "条码【{SFC}】处有不属于系统的不合格代码【{NGCode}】，请检查！";
        public const string MES17111 = "工艺路线【{Ids}】信息读取失败！";
        public const string MES17112 = "工单【{Ids}】信息读取失败！";
        public const string MES17113 = "消耗条码{BarCodes}不属于已上料清单！";
        public const string MES17114 = "获取工序基本信息失败，请检查工序参数【{Procedure}】！";
        public const string MES17115 = "工序【{Procedure}】未设置【缺陷编码】参数项，具体请查看【工序维护】！";
        public const string MES17116 = "不合格代码【{Code}】未设置【不合格工艺路线】参数项，具体请查看【不合格代码】！";
        #endregion

        #region 面板维护 MES17200
        public const string MES17201 = "面板类型不能为空";
        public const string MES17202 = "面板编码不能为空";
        public const string MES17203 = "面板名称不能为空";
        public const string MES17204 = "面板状态不能为空";
        public const string MES17205 = "面板编码已经存在";
        public const string MES17206 = "面板编码最大长度为255";
        public const string MES17207 = "面板名称最大长度为255";
        public const string MES17208 = "面板有更新，请刷新页面！";
        public const string MES17209 = "面板不存在！";
        public const string MES17210 = "面板类型不合法！";
        public const string MES17211 = "状态不合法！";
        public const string MES17212 = "会话超时时间不能为空！";

        public const string MES17251 = "按钮名称不能为空！";
        public const string MES17252 = "按钮序列号必须大于0！";
        public const string MES17253 = "按钮中存在，作业序列号不合法，请检查！";
        public const string MES17254 = "按钮中存在，作业为空，请检查！";
        public const string MES17255 = "未读取到有效作业，请检查！";
        public const string MES17256 = "作业参数【{Param}】获取失败，请检查！";
        #endregion

        #region 在制维修 MES17300
        public const string MES17301 = "工序不能为空";
        public const string MES17302 = "资源不能为空";
        public const string MES17303 = "产品条码不能为空";
        public const string MES17304 = "更改产品条码生产状态失败";
        public const string MES17305 = "获取维修信息失败";
        public const string MES17306 = "获取条码生产信息失败";
        public const string MES17307 = "存在未关闭的缺陷，请检查！";
        public const string MES17308 = "返回工序失败！";
        public const string MES17309 = "当前面板不存在！";
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
        public const string MES17325 = "未获取到维修业务表的数据";
        public const string MES17326 = "未获取到维修业务内容";
        public const string MES17327 = "工序未关联到资源";
        public const string MES17328 = "不合格代码:{0}";
        public const string MES17329 = "分析原因不能为空！";
        public const string MES17330 = "维修方法不能为空！";
        public const string MES17331 = "未找到不良录入信息！";


        #endregion

        #region 在制维修 MES17400
        public const string MES17400 = "条码绑定异常";
        public const string MES17401 = "绑定条码【{SFCs}】不存在";
        public const string MES17402 = "绑定条码已经不是在制品，具体请查看【产品序列码报告】。";
        public const string MES17403 = "绑定条码已经不为活动状态，具体请查看【产品序列码报告】。";
        public const string MES17404 = "绑定条码已经不为活动状态，具体请查看【产品序列码报告】。";
        public const string MES17405 = "条码工单为【{WorkOrder}】工单不一致，具体请查看【产品序列码报告】。";
        public const string MES17406 = "条码所在工序为【{ProcedureName}】与操作工序不一致，具体请查看【产品序列码报告】。";
        public const string MES17407 = "条码物料【{MaterialName}】不是工单【{WorkOrder}】的Bom【{BomName}】在工序【{ProcedureName}】使用物料，具体请查看【BOM维护】。";
        public const string MES17408 = "绑定条码【{SFCs}】与将绑定条码工单不一致，具体请查看【产品序列码报告】。";
        public const string MES17409 = "条码【{SFCs}】已经被绑定，具体请查看【原建汇总】。";
        public const string MES17410 = "条码绑定位置【{Location}】已被其他条码绑定，具体请查看【原建汇总】。";
        public const string MES17411 = "条码【{SFC}】已绑定数量【{BindQty}】，现需绑定【{TreatQty}】超过所需绑定【{NeedQty}】，具体请查看【原建汇总】。";
        public const string MES17412 = "绑定条码【{SFCs}】工单不一致，具体请查看【产品序列码报告】。";
        public const string MES17413 = "绑定条码【{SFCs}】物料不一致，具体请查看【产品序列码报告】。";
        public const string MES17414 = "工单【{WorkOrder}】在工序【{ProcedureName}】无物料清单信息，具体请查看【BOM维护】。";
        public const string MES17415 = "条码【{SFC}】不存在或者已不是在制品，具体请查看【产品序列码报告】。";
        public const string MES17416 = "绑定条码不是完成进入线边仓，具体请查看【物料库存】。";
        public const string MES17417 = "绑定条码【{SFCs}】工单不一致，具体请查看【物料库存】。";
        public const string MES17418 = "绑定条码【{SFCs}】物料不一致，具体请查看【物料库存】。";
        public const string MES17419 = "条码【{SFC}】与绑定条码工单不一致，具体请查看【产品序列码报告】。";
        public const string MES17420 = "绑定条码【SFCs】不是完成进入线边仓，具体请查看【物料库存】。";
        public const string MES17421 = "与【{SFC}】无绑定关系，无法进行解绑，具体请查看【原建汇总】。";
        public const string MES17422 = "条码【{SFC}】无绑定条码，无法进行解绑，具体请查看【原建汇总】。";

        public const string MES17423 = "条码【{SFC}】不存在，无法进行解绑，具体请查看【产品序列码报告】。";
        public const string MES17424 = "条码无法识别，无法绑定，具体请查看【BOM维护】【掩码维护】。";
        #endregion

        #region 在制品步骤控制 MES18000

        public const string MES18001 = "条码信息不存在";
        public const string MES18002 = "请扫描相同工单的条码";
        public const string MES18003 = "工单信息不存在";
        public const string MES18004 = "需扫描相同工艺路线条码";
        public const string MES18005 = "工艺路线不存在该工序";
        public const string MES18006 = "未获取到条码:{SFC}";
        public const string MES18007 = "条码没有对应的生产工序";
        public const string MES18008 = "已锁定";
        public const string MES18009 = "工单:{OrderCode},状态为:{Status}不允许操作";
        public const string MES18010 = "条码已锁定不允许操作";
        public const string MES18011 = "获取工序信息失败";


        public const string MES18012 = "工序不能为空";
        public const string MES18013 = "类型不能为空";
        public const string MES18014 = "条码不能为空";
        public const string MES18015 = "条码:{SFC}已装箱，请先解包";
        public const string MES18016 = "在制品步骤控制保存失败";
        public const string MES18017 = "工艺路线不存在尾工序";
        public const string MES18018 = "工艺路线存在多个尾工序";
        public const string MES18019 = "条码:{SFCs}已包装，不允许操作";
        public const string MES18020 = "库存信息不存在";
        public const string MES18021 = "物料库存不足";
        public const string MES18022 = "条码{SFC}不存在或已报废，不允许操作";
        public const string MES18023 = "条码{SFC}不能直接从尾工序完成到其他工序完成，请先至于排队或活动";


        #endregion

        #region 车间作业控制 报告 MES18100
        public const string MES18100 = "车间作业控制报告错误";
        public const string MES18101 = "没有获取到{sfc}对应步骤的信息";
        public const string MES18102 = "没有获取到{sfc}对应步骤中的工单信息";
        public const string MES18103 = "没有获取到{sfc}对应步骤中的产品信息";
        public const string MES18104 = "没有获取到{sfc}对应步骤中的工艺路线信息";
        public const string MES18105 = "没有获取到{sfc}对应步骤中的BOM信息";
        public const string MES18106 = "没有获取到{sfc}对应正在使用的条码信息";
        public const string MES18107 = "没有获取到条码对应步骤中的工艺路线信息";
        public const string MES18110 = "没有传入SFC条码信息";
        #endregion

        #region 生产更改 MES18200
        public const string MES18200 = "最大扫描{number}个";
        public const string MES18201 = "条码不能为空";
        public const string MES18202 = "工单不能为空";
        public const string MES18203 = "工序不能为空";

        public const string MES18204 = "条码:{SFC}已锁定";
        public const string MES18205 = "条码:{SFC}对应的工单状态为锁定不允许操作";
        public const string MES18206 = "选择的条码的工单必须相同";
        public const string MES18207 = "新工单可生产数量不足";
        public const string MES18208 = "未获取到工艺路线节点信息";
        public const string MES18209 = "工单:{Code}状态不允许操作！";
        public const string MES18210 = "生产更改错误";
        public const string MES18211 = "条码非在制或排队中状态，不允许操作";
        public const string MES18212 = "相同工单{Code}，不允许操作";
        public const string MES18213 = "未获取到工艺路线连线信息";
        public const string MES18214 = "工序不存在";

        public const string MES18215 = "条码{0}设置为活动状态成功！";
        public const string MES18216 = "条码{0}状态修改失败，请联系管理员！";
        public const string MES18217 = "条码{SFC}状态不合法！";
        public const string MES18218 = "条码{0}库存修改失败，请联系管理员！";
        public const string MES18219 = "条码对应物料和工单对应物料相同！";
        public const string MES18220 = "条码已是完成状态！";
        public const string MES18221 = "条码:{SFC}对应的工单状态为关闭不允许操作";
        public const string MES18222 = "条码:{SFC}在不合格工艺路线不允许操作";
        public const string MES18223 = "请选择您要更改的工单/物料/Bom/工艺路线";
        #endregion

        #region 系统Token MES18300
        public const string MES18300 = "系统编码{code}已经存在";
        public const string MES18301 = "系统编码不存在";
        public const string MES18302 = "系统编码不允许空格";
        public const string MES18303 = "系统名称不能为空";
        public const string MES18304 = "系统编码最大长度为50";
        public const string MES18305 = "系统名称最大长度为50";
        #endregion

        #region 客户维护 18400
        public const string MES18400 = "客户维护错误";
        public const string MES18401 = "对应客户维护不存在";
        public const string MES18402 = "客户编码已存在";
        public const string MES18403 = "客户编码不能为空";
        public const string MES18404 = "客户名称不能为空";
        public const string MES18405 = "客户编码最大长度为50";
        public const string MES18406 = "客户名称最大长度为50";
        public const string MES18407 = "地址最大长度为255";
        public const string MES18408 = "描述最大长度为255";
        public const string MES18409 = "电话最大长度为50";
        public const string MES18410 = "客户编码不允许有空格";

        #endregion

        #region 载具类型维护 18500
        public const string MES18500 = "载具类型错误";
        public const string MES18501 = "对应载具类型不存在";
        public const string MES18502 = "载具类型编码已存在";
        public const string MES18503 = "载具类型编码不能为空";
        public const string MES18504 = "载具类型名称不能为空";
        public const string MES18505 = "载具类型编码最大长度为100";
        public const string MES18506 = "载具类型名称最大长度为100";
        public const string MES18507 = "描述最大长度为255";
        public const string MES18508 = "状态不合法";
        public const string MES18509 = "行数应该为正整数";
        public const string MES18510 = "列数应该为正整数";
        public const string MES18511 = "单元数量应该为正整数";
        public const string MES18512 = "Id不合法";

        public const string MES18513 = "载具类型验证类型不能为空";
        public const string MES18514 = "载具类型验证类型不合法";
        public const string MES18515 = "载具类型验证物料或物料组不合法";
        public const string MES18516 = "载具类型编码不允许有空格";
        public const string MES18517 = "选择的载具类型有被载具绑定，请先去载具上解绑！";
        public const string MES18518 = "该载具类型已被关联了产品的载具关联，不允许更改该载具类型";
        public const string MES18519 = "不支持对载具进行组装操作！";
        public const string MES18520 = "不支持对载具进行参数收集操作！";
        public const string MES18521 = "只允许对单一条码进行参数收集操作！";
        #endregion

        #region 载具注册 18600
        public const string MES18600 = "载具错误";
        public const string MES18601 = "对应载具不存在";
        public const string MES18602 = "载具编码已存在";
        public const string MES18603 = "载具编码[{codes}]有关联的产品序列码,请先解绑该载具所关联的产品序列码！";
        public const string MES18604 = "载具编码不能为空";
        public const string MES18605 = "载具名称不能为空";
        public const string MES18606 = "载具编码最大长度为100";
        public const string MES18607 = "载具名称最大长度为100";
        public const string MES18608 = "载具描述最大长度为255";
        public const string MES18609 = "载具状态不合法";
        public const string MES18610 = "载具存放位置最大长度为100";
        public const string MES18611 = "载具类型不能为空";
        public const string MES18612 = "载具Id不能为空";
        public const string MES18613 = "载具装载已达最大数量";
        public const string MES18614 = "载具该单元格装载已达最大数量";
        public const string MES18615 = "载具该单元格已装载了条码:{sfc}";
        public const string MES18616 = "载具：重复装载该条码:{sfc},托盘码:{palletNo}";
        public const string MES18617 = "载具不存在或者载具未激活";
        public const string MES18618 = "载具编码不允许有空格";
        public const string MES18619 = "载具信息不存在或未启用！";
        public const string MES18620 = "载具位置Id不能为0！";
        public const string MES18621 = "载具SFC信息不能为空！";
        public const string MES18622 = "载具待解盘条码不能为空！";
        public const string MES18623 = "载具编码【{Code}】读取失败！";
        public const string MES18624 = "载具编码【{Code}】不存在，具体请查看【载具注册】！";
        public const string MES18625 = "载具编码【{Code}】已禁用，具体请查看【载具注册】！";
        public const string MES18626 = "载具编码【{Code}】未绑定任何条码，具体请查看【载具绑定解绑】！";
        #endregion

        #region Recipe参数 18700
        public const string MES18700 = "Recipe参数错误";
        public const string MES18701 = "对应Recipe参数不存在";
        public const string MES18702 = "Recipe参数编码已存在";
        public const string MES18704 = "Recipe参数编码不能为空";
        public const string MES18703 = "Recipe参数编码不允许空格";
        public const string MES18705 = "Recipe参数名称不能为空";
        public const string MES18706 = "Recipe参数编码最大长度为100";
        public const string MES18707 = "Recipe参数名称最大长度为100";
        public const string MES18709 = "Recipe参数状态不合法";
        public const string MES18710 = "Recipe参数功能类型不合法";

        public const string MES18712 = "Recipe参数Id不能为空";
        public const string MES18713 = "无法将其他状态修改成新建状态!";
        public const string MES18714 = "无法删除非新建状态数据";
        public const string MES18715 = "Recipe参数版本不能为空";
        public const string MES18716 = "Recipe参数版本最大长度为100";
        public const string MES18717 = "Recipe参数版本不允许有空格";
        public const string MES18718 = "Recipe参数产品不能为空";
        public const string MES18719 = "Recipe参数工序不能为空";
        public const string MES18720 = "Recipe参数工艺组不能为空";

        public const string MES18721 = "Recipe参数项目参数不能为空";
        public const string MES18722 = "Recipe参数项目小数位应该为0到9的整数";
        public const string MES18723 = "Recipe参数项目最大值应该大于或等于最小值";

        public const string MES18724 = "Recipe参数已存在同一的产品、工序、工艺组数据";
        #endregion

        #region 单位维护 18800
        public const string MES18800 = "单位编码不能含有空格";
        public const string MES18801 = "单位名称不能为空";
        #endregion

        #region 工艺设备组 18900
        public const string MES18900 = "此编码{Code}在系统已经存在!";
        public const string MES18901 = "工艺设备组编码不允许有空格";
        public const string MES18902 = "工艺设备组名称不能为空";
        public const string MES18903 = "请求实体不能为空！";
        public const string MES18904 = "工序与设备组对应关系不唯一";
        #endregion


        #region 设备对接错误

        #region 设备对接公用
        public const string MES19001 = "设备编码不能为空";
        public const string MES19002 = "资源编码不能为空";
        public const string MES19003 = "SFC条码不能为空";
        public const string MES19004 = "调用本地时间不能为空";
        public const string MES19005 = "设备编码：{Code}不存在";
        public const string MES19006 = "资源编码不存在";
        public const string MES19007 = "SFC条码不允许重复";
        public const string MES19008 = "资源不匹配";
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
        public const string MES19114 = "NG编码：{Code}不存在";
        public const string MES19115 = "产品编码不能为空";
        public const string MES19116 = "请维护产品编码：{Code}的编码规则";
        public const string MES19117 = "条码：{SFC}不存在";
        public const string MES19118 = "产品编码：{Code}不存在";
        public const string MES19119 = "绑定条码列表不能为空";
        public const string MES19120 = "解绑条码列表不能为空";
        public const string MES19121 = "绑定条码：{SFC}和BindSFC：{BindSFC}绑定关系已经存在";
        public const string MES19122 = "容器：{ContainerCode}和SFC：{SFC}绑定关系已经存在";
        public const string MES19123 = "资源：{ResourceCode}未找到关联的产线信息";
        public const string MES19124 = "产线：{WorkCenterCode}未找到激活的工单信息";
        public const string MES19125 = "条码：{SFCS}未找到条码信息,请先执行进站操作";
        public const string MES19126 = "条码：{SFCS}未找到生产信息,或已完工";
        public const string MES19127 = "条码：{SFCS}未进站不能操作出站";
        public const string MES19128 = "条码：{SFCS}已进站不能操作过站";
        public const string MES19129 = "条码：{SFCS}当前状态不是排队状态，不允许进站";
        public const string MES19130 = "条码：{SFCS}超过或达到最大复投次数限制，不允许进站";
        public const string MES19131 = "资源：{ResCode}和设备：{EquCode}不存在绑定关系，请检查配置";
        public const string MES19132 = "条码：{SFCS}未进站不能操作绑定";
        public const string MES19133 = "IsVirtualSFC为True时不需要传递SFC";
        public const string MES19134 = "条码：{SFCS}没找到关联虚拟码绑定关系";
        public const string MES19135 = "IsBindVirtualSFC为True时不能传递多个条码";
        public const string MES19136 = "条码：{SFCS}出站存在NG信息，Passed字段应传0";
        public const string MES19137 = "条码：{SFCS}状态为不合格，不允许进站";
        public const string MES19138 = "条码：{SFCS}已经存在绑定记录";
        public const string MES19139 = "位置：{Location}已经存在条码：{SFCS}绑定记录";
        public const string MES19140 = "错误的Location {SFCLocation}，只能为：{Location}";
        public const string MES19141 = "CCS设定NG时Location和BindSfc方式必须任选其一";
        public const string MES19142 = "条码：{SFC}没找到关联CCS码绑定关系";
        public const string MES19143 = "条码：{SFC}指定位置未关联CCS码或和指定CSS码不存在绑定关系";
        public const string MES19144 = "Location不能为空";
        public const string MES19145 = "模组对应型号编码ModelCode不能为空";
        public const string MES19146 = "条码：{SFCS}已被绑过其他条码";

        #region 产品参数采集
        public const string MES19600 = "产品参数采集异常";
        public const string MES19601 = "未找到资源{ResCode}对应工序";
        public const string MES19602 = "参数{ResCode}不存在";
        public const string MES19603 = "资源编码{Code}不存在";
        public const string MES19604 = "资源：{ResourceCode}未找到激活的工单信息";
        public const string MES19605 = "条码生成失败,半成品记录未找到！";
        public const string MES19606 = "参数{ParameterCodes}不存在";
        #endregion


        public const string MES19910 = "资源：{ResCode}和设备：{EquCode}不存在绑定关系，请检查配置";
        public const string MES19911 = "资源：{ResourceCode}未找到关联的产线信息";
        public const string MES19912 = "产线：{WorkCenterCode}未找到激活的工单信息";
        public const string MES19913 = "资源：{ResCode}未绑定工序，请检查配置";
        public const string MES19914 = "工单：{OrderCode}未绑定BOM，请检查配置";
        public const string MES19915 = "Bom：{BomCode}未绑定物料明细，请检查配置";
        public const string MES19916 = "条码：{SFC}不符合掩码规则，请检查配置";
        public const string MES19917 = "条码：{SFC}已存在";
        public const string MES19918 = "条码：{SFC}不存在生产信息";
        public const string MES19919 = "资源：{ResCode}不存在";
        public const string MES19920 = "条码：{SFC}不在当前工序活动";
        public const string MES19921 = "子条码：{SFC}已经绑定其它主条码";
        public const string MES19922 = "主条码：{SFC}已经使用";
        public const string MES19923 = "未获取到工单信息";
        public const string MES19924 = "子条码位置重复,请检查！";
        public const string MES19925 = "子条码重复,请检查！";
        public const string MES19926 = "子条码列表不允许为空！";
        public const string MES19927 = "子条码不能为空";
        public const string MES19928 = "资源：{ResCode}未绑定工单，请检查配置";
        public const string MES19929 = "工单不存在";
        public const string MES19930 = "条码：{SFC}状态为在制，但没有获取到在制信息";
        public const string MES19931 = "条码：{SFC}已锁定";
        public const string MES19932 = "条码：{SFC}已报废";
        public const string MES19933 = "条码：{SFC}在{Procedure}工序状态为{Status}，不允许操作";
        public const string MES19934 = "设备{EquipmentCode}不存在";
        public const string MES19935 = "资源【{ResCode}】未绑定启用/保留状态的工序，请检查配置";


        #endregion

        #endregion

        #region 系统执行出错 业务逻辑出错

        #endregion

        #region 调用第三方服务出错

        #endregion
    }
}
