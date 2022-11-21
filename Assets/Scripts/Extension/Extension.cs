using System;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;

public static class Extension {
  #region Enum
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
  #endregion

  #region GameObject
  public static bool SetActiveEx(this GameObject gameObject, bool value) {
    if (gameObject.activeSelf == value) {
      return false;
    }
    gameObject.SetActive(value);
    return true;
  }
  #endregion
}