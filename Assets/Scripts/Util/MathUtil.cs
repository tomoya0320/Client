using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public enum CompareMethod {
  [InspectorName("����")]
  EQUAL,
  [InspectorName("����")]
  GREATER,
  [InspectorName("С��")]
  LESS,
  [InspectorName("���ڵ���")]
  GREATER_EQUAL,
  [InspectorName("С�ڵ���")]
  LESS_EQUAL,
}

public static class MathUtil {
  public const float EPSILON = 0.0001f;
  public static readonly Random Random = new Random();

  public static void FisherYatesShuffle<T>(T[] array) {
    for (int i = array.Length - 1; i > 0; i--) {
      int index = Random.Next(0, i);
      (array[index], array[i]) = (array[i], array[index]);
    }
  }

  public static void FisherYatesShuffle<T>(List<T> list) {
    for (int i = list.Count - 1; i > 0; i--) {
      int index = Random.Next(0, i);
      (list[index], list[i]) = (list[i], list[index]);
    }
  }

  public static bool Prob(float prob) {
    return Random.NextDouble() < prob;
  }

  public static bool Approximately(float f1, float f2) {
    return Mathf.Abs(f1 - f2) < EPSILON;
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
    Debug.LogError($"δ֪�ıȽϷ�ʽ��{method}");
    return false;
  }
}