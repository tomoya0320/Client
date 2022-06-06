using System;
using System.Collections.Generic;
using UnityEngine;

public enum CompareMethod {
  [InspectorName("等于")]
  EQUAL,
  [InspectorName("大于")]
  GREATER,
  [InspectorName("小于")]
  LESS,
  [InspectorName("大于等于")]
  GREATER_EQUAL,
  [InspectorName("小于等于")]
  LESS_EQUAL,
}

public static class MathUtil {
  public const float EPSILON = 0.0001f;

  public static void FisherYatesShuffle<T>(List<T> list) {
    for (int i = list.Count - 1; i > 0; i--) {
      int index = UnityEngine.Random.Range(0, i);
      (list[index], list[i]) = (list[i], list[index]);
    }
  }

  public static bool Compare(IComparable left, IComparable right, CompareMethod method) {
    int delta = left.CompareTo(right);
    switch (method) {
      case CompareMethod.EQUAL:
        return delta == 0;
      case CompareMethod.GREATER:
        return delta > 0;
      case CompareMethod.LESS:
        return delta < 0;
      case CompareMethod.GREATER_EQUAL:
        return delta >= 0;
      case CompareMethod.LESS_EQUAL:
        return delta <= 0;
    }
    Debug.LogError($"未知的比较方式！{method}");
    return false;
  }
}