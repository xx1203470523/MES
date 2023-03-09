namespace Hymson.MES.Core.Constants
{
    public class ErrorCode
    {
        #region  用户端错误 
        public const string MES10100 = "请求实体不能为空";
        public const string MES10101 = "站点码获取失败，请重新登录！";
        public const string MES10102 = "删除失败Id 不能为空!";


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
        public const string MES10212 = "有生产中工单引用当前物料，不能删除！";
        public const string MES10213 = "参数不能为空";
        public const string MES10214 = "物料编码不能为空";
        public const string MES10215 = "物料名称不能为空";
        public const string MES10216 = "此物料组编码{groupCode}在系统已经存在！";
        public const string MES10217 = "存在已分配给其他物料组的物料！";
        public const string MES10218 = "新增物料组失败！";
        public const string MES10219 = "此物料组不存在！";
        public const string MES10220 = "修改物料组失败！";
        public const string MES10221 = "已被分配物料，不允许删除！";
        #endregion

        #region 资源 10300
        public const string MES10301 = "资源编码不能为空";
        public const string MES10302 = "资源编码超长";
        public const string MES10303 = "资源名称不能为空";
        public const string MES10304 = "资源名称超长";
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
        #endregion

        #region 工序 10400
        public const string MES10401 = "工序编码不能为空";
        public const string MES10402 = "工序编码超长";
        public const string MES10403 = "工序名称不能为空";
        public const string MES10404 = "工序名称超长";
        public const string MES10405 = "编码:{0}已存在！";

        public const string MES10430 = $"不能删除启用和保留状态的工艺路线！";
        public const string MES10431 = $"此工艺路线在系统中已经存在!";
        public const string MES10432 = $"编码不能为空!";
        public const string MES10433 = $"名称不能为空!";
        public const string MES10434 = $"版本不能为空!";
        public const string MES10435 = $"未设置首工序！";
        public const string MES10436 = $"只允许设置一个首工序！";
        public const string MES10437 = "此工艺路线{Code}+{Version}在系统已经存在！";
        public const string MES10438 = "此工艺路线不存在！";
        public const string MES10439 = $"此工艺路线在系统中不存在!";
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
        #endregion

        #region 不合格组 11200
        public const string MES11200 = "不合格代码组错误";
        public const string MES11201 = "不合格代码组编码不能为空";
        public const string MES11202 = "不合格代码组编码超过最大长度，不合格代码编码最大长度为50";
        public const string MES11203 = "不合格代码组名称不能为空";
        public const string MES11204 = "不合格代码组名称超过最大长度，不合格代码名称最大长度为60";
        public const string MES11205 = "不合格代码备注超过最大长度，不合格代码备注最大长度为255";
        public const string MES11206 = "不合格代码{code}已经存在";
        #endregion

        //综合
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
        public const string MES12112 = "工作中心已经关联数据,允许修改.";
        #endregion

        //设备
        #region 故障原因 13000
        public const string MES13000 = "基础故障原因错误";
        public const string MES13001 = "站点码获取失败，请重新登录！";
        public const string MES13002 = "此故障原因编码{FaultReasonCode}在系统已经存在！";
        public const string MES13003 = "请求实体不能为空！";
        public const string MES13004 = "此标准故障原因不存在！";
        public const string MES13005 = "删除失败Ids 不能为空";
        public const string MES13006 = "故障原因已被设备故障原因或产品故障原因绑定，不允许删除!";
        public const string MES13007 = "修改故障原因关联类型失败!";
        public const string MES13008 = "故障原因状态不能为空";
        public const string MES13009 = "故障原因编码不能为空";
        public const string MES13010 = "故障原因名称不能为空";

        #endregion


        //库存
        #region 供应商库存  15000
        public const string MES15000 = "基础参数错误";
        public const string MES15001 = "站点码获取失败，请重新登录！";
        public const string MES15002 = "此供应商编码{Code}在系统已经存在！";
        public const string MES15003 = "请求实体不能为空！";
        public const string MES15005 = "删除失败Ids 不能为空";
        public const string MES15006 = "供应商编码不能为空";
        public const string MES15007 = "供应商名称不能为空";
        public const string MES15008 = "此供应商编码{Code}不符合规则，字母/数字！";


        #endregion

        //库存
        #region 车间库存接收  15100
        public const string MES15100 = "基础参数错误";
        public const string MES15101 = "物料不存在";
        public const string MES15102 = "物料：{MaterialCode}未关联到供应商";
        public const string MES15103 = "数量需大于0";


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
