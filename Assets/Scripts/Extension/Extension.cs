using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;

public static class Extension {
  #region CSharp
  public static bool TryPop<T>(this Stack<T> stack, out T item) {
    if (stack.Count > 0) {
      item = stack.Pop();
      return true;
    }
    item = default;
    return false;
  }
  
  public static bool TryPeek<T>(this Stack<T> stack, out T item) {
    if (stack.Count > 0) {
      item = stack.Peek();
      return true;
    }
    item = default;
    return false;
  }
  
  public static bool TryDequeue<T>(this Queue<T> queue, out T item) {
    if (queue.Count > 0) {
      item = queue.Dequeue();
      return true;
    }
    item = default;
    return false;
  }
  #endregion
  
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