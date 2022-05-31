using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

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
}