using System;
using System.ComponentModel;
using System.Reflection;

public static class Extension {
  /// <summary>
  /// 获取当前枚举描述
  /// </summary>
  /// <param name="enumValue"></param>
  /// <returns></returns>
  public static string GetDescription(this Enum enumValue) {
    Type type = enumValue.GetType();
    MemberInfo[] memInfo = type.GetMember(enumValue.ToString());
    if (memInfo != null && memInfo.Length > 0) {
      object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
      if (attrs != null && attrs.Length > 0)
        return ((DescriptionAttribute)attrs[0]).Description;
    }
    return enumValue.ToString();
  }
}