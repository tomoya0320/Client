using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

/// <summary>
/// Description为Tree的Type
/// 一种红点树类型代表了一个红点树的子类
/// </summary>
public enum RedDotTreeType {

}

public class RedDotNode {
  public string Name { get; private set; }
  /// <summary>
  /// 如果要把RedDotNode移除的话(仅限移除叶节点)记得将RedDotNode父节点的这个变量减一
  /// </summary>
  private int ChildCount;
  public int RedDotNum { get; private set; }
  public RedDotNode ParentNode { get; private set; }
  public bool IsLeaf => ChildCount == 0;
  private readonly RedDotTreeType RedDotTreeType;

  public RedDotNode(RedDotTreeType redDotTreeType, string name, RedDotNode parentNode) {
    Name = name;
    ParentNode = parentNode;
    RedDotTreeType = redDotTreeType;

    if (ParentNode != null) {
      ParentNode.ChildCount++;
    }
  }

  public void SetRedDotNum(int num) {
    if (num < 0) {
      Debug.LogError("RedDotNum不能小于0！");
    } else {
      int delta = num - RedDotNum;
      if (delta != 0) {
        RedDotNum = num;
        RedDotSystem.OnRedDotChanged(RedDotTreeType, Name, ParentNode.Name);
        ParentNode?.SetRedDotNum(ParentNode.RedDotNum + delta);
      }
    }
  }

  public void OnRemove() => ParentNode.ChildCount--;
}

public abstract class RedDotTree {
  protected Dictionary<string, RedDotNode> RedDotDict;
  protected abstract RedDotTreeType RedDotTreeType { get; }

  /// <summary>
  /// 每次被实例化时初始化具体的红点业务逻辑
  /// </summary>
  public abstract void InitData();

  /// <summary>
  /// 当不存在该节点对时返回0
  /// </summary>
  /// <param name="name"></param>
  /// <param name="parent"></param>
  /// <returns></returns>
  public int GetRedDotNum(string name, string parent) {
    if (RedDotDict.ContainsKey(name) && RedDotDict[name].ParentNode.Name == parent) {
      return RedDotDict[name].RedDotNum;
    }
    return 0;
  }

  protected void AddLeafNode(string name, string parent, int initRedDotNum = 1) {
    if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(parent)) {
      Debug.LogError("父子节点名均不能为空！");
      return;
    }

    if (RedDotDict.ContainsKey(name)) {
      Debug.LogError($"该子节点已添加！name:{name}");
    } else {
      if (RedDotDict.ContainsKey(parent)) {
        RedDotDict.Add(name, new RedDotNode(RedDotTreeType, name, RedDotDict[parent]));
        RedDotDict[name].SetRedDotNum(initRedDotNum);
      } else {
        Debug.LogError($"找不到父节点！parent:{parent}");
      }
    }
  }

  public void SetRedDotNum(string name, string parent, int redDotNum) {
    if (RedDotDict.ContainsKey(name)) {
      var redDotNode = RedDotDict[name];
      if (redDotNode.ParentNode.Name == parent) {
        if (redDotNode.IsLeaf) {
          redDotNode.SetRedDotNum(redDotNum);
          if (redDotNode.RedDotNum <= 0) {
            RedDotDict.Remove(name);
            redDotNode.OnRemove();
          }
        } else {
          Debug.LogError($"外部不允许直接对非叶节点进行设置！name:{name},parent:{parent}");
        }
      } else {
        Debug.LogError($"父节点不一致！name:{name},parent:{parent},correctParent:{redDotNode.ParentNode.Name}");
      }
    } else {
      AddLeafNode(name, parent, redDotNum);
    }
  }
}

public interface IUIRedDot {
  public string Name { get; set; } // 配合动态列表需要能够多次设置
  public string Parent { get; set; } // 配合动态列表需要能够多次设置
  public RedDotTreeType RedDotTreeType { get; }

  void OnRedDotChanged();
}

public static class RedDotSystem {
  private static Dictionary<RedDotTreeType, HashSet<IUIRedDot>> UIRedDots = new Dictionary<RedDotTreeType, HashSet<IUIRedDot>>();
  private static Dictionary<RedDotTreeType, RedDotTree> RedDotTrees = new Dictionary<RedDotTreeType, RedDotTree>();

  public static void Init() {
    foreach (RedDotTreeType redDotTreeType in Enum.GetValues(typeof(RedDotTreeType))) {
      UIRedDots.Add(redDotTreeType, new HashSet<IUIRedDot>());
      RedDotTrees.Add(redDotTreeType, AssemblyUtil.CreateInstance<RedDotTree>(redDotTreeType.GetDescription()));
    }
  }

  public static void InitData() {
    foreach (var redDotTree in RedDotTrees.Values) {
      redDotTree.InitData();
    }
  }

  public static void SetRedDotNum(RedDotTreeType redDotTreeType, string name, string parent, int redDotNum) {
    if (string.IsNullOrEmpty(name)) {
      return;
    }

    if (RedDotTrees.ContainsKey(redDotTreeType)) {
      RedDotTrees[redDotTreeType].SetRedDotNum(name, parent, redDotNum);
    } else {
      Debug.LogError($"该类型的Tree未注册！Type:{redDotTreeType}");
    }
  }

  public static int GetRedDotNum(RedDotTreeType redDotTreeType, string name, string parent) {
    if (string.IsNullOrEmpty(name)) {
      return 0;
    }

    if (RedDotTrees.ContainsKey(redDotTreeType)) {
      return RedDotTrees[redDotTreeType].GetRedDotNum(name, parent);
    } else {
      Debug.LogError($"该类型的Tree未注册！Type:{redDotTreeType}");
      return 0;
    }
  }

  public static void AddUIRedDot(IUIRedDot uiRedDot) {
    UIRedDots[uiRedDot.RedDotTreeType].Add(uiRedDot);
  }

  public static void RemoveUIRedDot(IUIRedDot uiRedDot) {
    UIRedDots[uiRedDot.RedDotTreeType].Remove(uiRedDot);
  }

  public static void OnRedDotChanged(RedDotTreeType redDotTreeType, string name, string parent) {
    foreach (var uiRedDot in UIRedDots[redDotTreeType]) {
      if(uiRedDot.Name == name && uiRedDot.Parent == parent) {
        uiRedDot.OnRedDotChanged();
      }
    }
  }
}