using System;
using System.Collections.Generic;

namespace Broadcast {
  public static class EventCenter {
    private static Dictionary<EventType, Delegate> EventTable = new Dictionary<EventType, Delegate>();

    private static void OnListenerAdding(EventType eventType, Delegate callBack) {
      if (!EventTable.ContainsKey(eventType)) {
        EventTable.Add(eventType, null);
      }
      Delegate d = EventTable[eventType];
      if (d != null && d.GetType() != callBack.GetType()) {
        throw new Exception($"尝试为事件{eventType}添加不同类型的委托，当前事件所对应的委托是{d.GetType()}，要添加的委托类型为{callBack.GetType()}");
      }
    }

    private static void OnListenerRemoving(EventType eventType, Delegate callBack) {
      if (EventTable.ContainsKey(eventType)) {
        Delegate d = EventTable[eventType];
        if (d == null) {
          throw new Exception($"移除监听错误：事件{eventType}没有对应的委托");
        } else if (d.GetType() != callBack.GetType()) {
          throw new Exception($"移除监听错误：尝试为事件{eventType}移除不同类型的委托，当前委托类型为{d.GetType()}，要移除的委托类型为{callBack.GetType()}");
        }
      } else {
        throw new Exception($"移除监听错误：没有事件码{eventType}");
      }
    }

    private static void OnListenerRemoved(EventType eventType) {
      if (EventTable[eventType] == null) {
        EventTable.Remove(eventType);
      }
    }

    //no parameters
    public static void AddListener(EventType eventType, CallBack callBack) {
      OnListenerAdding(eventType, callBack);
      EventTable[eventType] = (CallBack)EventTable[eventType] + callBack;
    }

    //Single parameters
    public static void AddListener<T>(EventType eventType, CallBack<T> callBack) {
      OnListenerAdding(eventType, callBack);
      EventTable[eventType] = (CallBack<T>)EventTable[eventType] + callBack;
    }

    //two parameters
    public static void AddListener<T, X>(EventType eventType, CallBack<T, X> callBack) {
      OnListenerAdding(eventType, callBack);
      EventTable[eventType] = (CallBack<T, X>)EventTable[eventType] + callBack;
    }

    //three parameters
    public static void AddListener<T, X, Y>(EventType eventType, CallBack<T, X, Y> callBack) {
      OnListenerAdding(eventType, callBack);
      EventTable[eventType] = (CallBack<T, X, Y>)EventTable[eventType] + callBack;
    }

    //four parameters
    public static void AddListener<T, X, Y, Z>(EventType eventType, CallBack<T, X, Y, Z> callBack) {
      OnListenerAdding(eventType, callBack);
      EventTable[eventType] = (CallBack<T, X, Y, Z>)EventTable[eventType] + callBack;
    }

    //five parameters
    public static void AddListener<T, X, Y, Z, W>(EventType eventType, CallBack<T, X, Y, Z, W> callBack) {
      OnListenerAdding(eventType, callBack);
      EventTable[eventType] = (CallBack<T, X, Y, Z, W>)EventTable[eventType] + callBack;
    }

    //no parameters
    public static void RemoveListener(EventType eventType, CallBack callBack) {
      OnListenerRemoving(eventType, callBack);
      EventTable[eventType] = (CallBack)EventTable[eventType] - callBack;
      OnListenerRemoved(eventType);
    }

    //single parameters
    public static void RemoveListener<T>(EventType eventType, CallBack<T> callBack) {
      OnListenerRemoving(eventType, callBack);
      EventTable[eventType] = (CallBack<T>)EventTable[eventType] - callBack;
      OnListenerRemoved(eventType);
    }

    //two parameters
    public static void RemoveListener<T, X>(EventType eventType, CallBack<T, X> callBack) {
      OnListenerRemoving(eventType, callBack);
      EventTable[eventType] = (CallBack<T, X>)EventTable[eventType] - callBack;
      OnListenerRemoved(eventType);
    }

    //three parameters
    public static void RemoveListener<T, X, Y>(EventType eventType, CallBack<T, X, Y> callBack) {
      OnListenerRemoving(eventType, callBack);
      EventTable[eventType] = (CallBack<T, X, Y>)EventTable[eventType] - callBack;
      OnListenerRemoved(eventType);
    }

    //four parameters
    public static void RemoveListener<T, X, Y, Z>(EventType eventType, CallBack<T, X, Y, Z> callBack) {
      OnListenerRemoving(eventType, callBack);
      EventTable[eventType] = (CallBack<T, X, Y, Z>)EventTable[eventType] - callBack;
      OnListenerRemoved(eventType);
    }

    //five parameters
    public static void RemoveListener<T, X, Y, Z, W>(EventType eventType, CallBack<T, X, Y, Z, W> callBack) {
      OnListenerRemoving(eventType, callBack);
      EventTable[eventType] = (CallBack<T, X, Y, Z, W>)EventTable[eventType] - callBack;
      OnListenerRemoved(eventType);
    }

    //no parameters
    public static void Broadcast(EventType eventType) {
      if (EventTable.TryGetValue(eventType, out Delegate d)) {
        if (d is CallBack callBack) {
          callBack();
        } else {
          throw new Exception($"广播事件错误：事件{eventType}对应委托具有不同的类型");
        }
      }
    }

    //single parameters
    public static void Broadcast<T>(EventType eventType, T arg) {
      if (EventTable.TryGetValue(eventType, out Delegate d)) {
        if (d is CallBack<T> callBack) {
          callBack(arg);
        } else {
          throw new Exception($"广播事件错误：事件{eventType}对应委托具有不同的类型");
        }
      }
    }

    //two parameters
    public static void Broadcast<T, X>(EventType eventType, T arg1, X arg2) {
      if (EventTable.TryGetValue(eventType, out Delegate d)) {
        if (d is CallBack<T, X> callBack) {
          callBack(arg1, arg2);
        } else {
          throw new Exception($"广播事件错误：事件{eventType}对应委托具有不同的类型");
        }
      }
    }

    //three parameters
    public static void Broadcast<T, X, Y>(EventType eventType, T arg1, X arg2, Y arg3) {
      if (EventTable.TryGetValue(eventType, out Delegate d)) {
        if (d is CallBack<T, X, Y> callBack) {
          callBack(arg1, arg2, arg3);
        } else {
          throw new Exception($"广播事件错误：事件{eventType}对应委托具有不同的类型");
        }
      }
    }

    //four parameters
    public static void Broadcast<T, X, Y, Z>(EventType eventType, T arg1, X arg2, Y arg3, Z arg4) {
      if (EventTable.TryGetValue(eventType, out Delegate d)) {
        if (d is CallBack<T, X, Y, Z> callBack) {
          callBack(arg1, arg2, arg3, arg4);
        } else {
          throw new Exception($"广播事件错误：事件{eventType}对应委托具有不同的类型");
        }
      }
    }

    //five parameters
    public static void Broadcast<T, X, Y, Z, W>(EventType eventType, T arg1, X arg2, Y arg3, Z arg4, W arg5) {
      if (EventTable.TryGetValue(eventType, out Delegate d)) {
        if (d is CallBack<T, X, Y, Z, W> callBack) {
          callBack(arg1, arg2, arg3, arg4, arg5);
        } else {
          throw new Exception($"广播事件错误：事件{eventType}对应委托具有不同的类型");
        }
      }
    }
  }
}