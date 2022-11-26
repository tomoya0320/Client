using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace GameCore {
  [Serializable]
  public class Map {
    public const int WIDTH = 4;
    public const int HEIGHT = 3;

    public int[] Nodes;
    public int DestNode;
    public int CurPos;
    public Dictionary<int, List<int>> Paths;

    public static Map Generate() {
      List<int> tempList = new List<int>();
      int[] nodes = new int[WIDTH * HEIGHT];
      int destNode = 0; // TEST
      Dictionary<int, List<int>> paths = new Dictionary<int, List<int>>();
      for (int i = 0; i < WIDTH; i++) {
        for (int j = 0; j < HEIGHT; j++) {
          nodes[i * HEIGHT + j] = 0/*TEST*/;
        }
      }
      for (int i = 0; i < WIDTH; i++) {
        for (int j = 0; j < HEIGHT; j++) {
          if (!paths.TryGetValue(i * HEIGHT + j, out var next)) {
            next = new List<int>();
            paths.Add(i * HEIGHT + j, next);
          }
          if (i < WIDTH - 1) {
            next.Add((i + 1) * HEIGHT + j);
            if (j + 1 < HEIGHT) {
              tempList.Add((i + 1) * HEIGHT + j + 1);
            }
            if (j - 1 >= 0) {
              tempList.Add((i + 1) * HEIGHT + j - 1);
            }
            int index = Random.Range(-1, tempList.Count);
            if (index >= 0) {
              next.Add(tempList[index]);
            }
            tempList.Clear();
          } else {
            next.Add(nodes.Length);
          }
        }
      }
      return new Map { Nodes = nodes, DestNode = destNode, Paths = paths, CurPos = -1 };
    }
  }
}