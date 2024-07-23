using Hymson.MES.Core.Attribute.Manufacture;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Services.Dtos.Report;
using System.Reflection;

namespace Hymson.MES.Services.Extension
{
    public static class EnumHelper
    {
        private static readonly List<GetManuSfcStepTypeJobOrAssemblyNameDto> _list = new List<GetManuSfcStepTypeJobOrAssemblyNameDto>();
        private static  bool _flag=false;
        /// <summary>
        /// 使用切面来获取
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<GetManuSfcStepTypeJobOrAssemblyNameDto> GetManuSfcStepTypeobOrAssemblys()
        {

            if (_flag) return _list;
            lock (_list)
            {
                if (_flag) return _list;
                // 获取枚举类型
                Type enumType = typeof(ManuSfcStepTypeEnum);
                // 遍历枚举值
                foreach (var enumValue in Enum.GetValues(enumType))
                {
                    if (enumValue == null) continue;
                    // 获取枚举字段
                    var field = enumType.GetField(enumValue.ToString());

                    if (field == null) continue;

                    // 获取枚举值上的 ManuSfcStepOperationTypeAttrribute 特性
                    var manuSfcStepOperationTypeAttribute = field.GetCustomAttribute<ManuSfcStepOperationTypeAttrribute>(false);
                    if (manuSfcStepOperationTypeAttribute != null)
                    {
                        _list.Add(new GetManuSfcStepTypeJobOrAssemblyNameDto
                        {
                            Key = (ManuSfcStepTypeEnum)enumValue,
                            JobOrAssemblyCode = manuSfcStepOperationTypeAttribute.JobOrAssemblyCode,
                            JobOrAssemblyName = manuSfcStepOperationTypeAttribute.JobOrAssemblyName
                        });
                    }
                }
                _flag = true;
                return _list;
            }
        }
    }
}
