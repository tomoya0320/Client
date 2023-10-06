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
    if (memInfo.Length <= 0) return enumValue.ToString();
    object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
    return attrs.Length > 0 ? ((DescriptionAttribute)attrs[0]).Description : enumValue.ToString();
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