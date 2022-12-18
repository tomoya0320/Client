using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace GameCore {
  [Serializable]
  public class Map {
    public const int WIDTH = 7;
    public const int HEIGHT = 3;
    public const int DEST_POS = WIDTH * HEIGHT;
    public const int NODE_COUNT = WIDTH * HEIGHT + 1;

    public int[] Nodes;
    public int CurPos;
    public Dictionary<int, List<int>> Paths;

    public void UpdateCurPos(int pos) {
      CurPos = pos;
      EventCenter.Broadcast(EventType.ON_MAP_CUR_POS_UPDATE);
    }

    public bool CheckNodeEnable(int pos) {
      int x = pos / HEIGHT;

      if (CurPos < 0) {
        return x == 0;
      }

      int curX = CurPos / HEIGHT;

      if (pos == WIDTH * HEIGHT) { // destination
        return curX == WIDTH - 1;
      }

      if (curX >= x || curX + 1 < x) {
        return false;
      }

      return Paths[CurPos].Contains(pos);
    }

    public static Map Generate() {
      List<int> tempList = new List<int>();
      int[] nodes = new int[NODE_COUNT];
      Dictionary<int, List<int>> paths = new Dictionary<int, List<int>>();
      for (int i = 0; i < WIDTH; i++) {
        for (int j = 0; j < HEIGHT; j++) {
          nodes[i * HEIGHT + j] = Random.Range(0, Game.Instance.MapNodeDatabase.MapNodeDatabaseItems.Count); // TEST
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
            next.Add(DEST_POS);
          }
        }
      }
      nodes[DEST_POS] = 0; // TEST жу╣Ц
      return new Map { Nodes = nodes, Paths = paths, CurPos = -1 };
    }
  }
}