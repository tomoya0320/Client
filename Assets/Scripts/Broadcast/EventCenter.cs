using System;
using System.Collections.Generic;

namespace GameCore.Broadcast {
  public enum EventType {
    ON_MAP_CUR_POS_UPDATE,
  }

  public delegate void Callback();
  public delegate void Callback<T>(T arg);
  public delegate void Callback<T, X>(T arg1, X arg2);
  public delegate void Callback<T, X, Y>(T arg1, X arg2, Y arg3);
  public delegate void Callback<T, X, Y, Z>(T arg1, X arg2, Y arg3, Z arg4);
  public delegate void Callback<T, X, Y, Z, W>(T arg1, X arg2, Y arg3, Z arg4, W arg5);

  public static class EventCenter {
    private static Dictionary<EventType, Delegate> EventTable = new Dictionary<EventType, Delegate>();

    private static void OnListenerAdding(EventType eventType, Delegate callback) {
      if (!EventTable.ContainsKey(eventType)) {
        EventTable.Add(eventType, null);
      }
      Delegate d = EventTable[eventType];
      if (d != null && d.GetType() != callback.GetType()) {
        throw new Exception($"尝试为事件{eventType}添加不同类型的委托，当前事件所对应的委托是{d.GetType()}，要添加的委托类型为{callback.GetType()}");
      }
    }

    private static void OnListenerRemoving(EventType eventType, Delegate callback) {
      if (EventTable.ContainsKey(eventType)) {
        Delegate d = EventTable[eventType];
        if (d == null) {
          throw new Exception($"移除监听错误：事件{eventType}没有对应的委托");
        } else if (d.GetType() != callback.GetType()) {
          throw new Exception($"移除监听错误：尝试为事件{eventType}移除不同类型的委托，当前委托类型为{d.GetType()}，要移除的委托类型为{callback.GetType()}");
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
    public static void AddListener(EventType eventType, Callback callback) {
      OnListenerAdding(eventType, callback);
      EventTable[eventType] = (Callback)EventTable[eventType] + callback;
    }

    //Single parameters
    public static void AddListener<T>(EventType eventType, Callback<T> callback) {
      OnListenerAdding(eventType, callback);
      EventTable[eventType] = (Callback<T>)EventTable[eventType] + callback;
    }

    //two parameters
    public static void AddListener<T, X>(EventType eventType, Callback<T, X> callback) {
      OnListenerAdding(eventType, callback);
      EventTable[eventType] = (Callback<T, X>)EventTable[eventType] + callback;
    }

    //three parameters
    public static void AddListener<T, X, Y>(EventType eventType, Callback<T, X, Y> callback) {
      OnListenerAdding(eventType, callback);
      EventTable[eventType] = (Callback<T, X, Y>)EventTable[eventType] + callback;
    }

    //four parameters
    public static void AddListener<T, X, Y, Z>(EventType eventType, Callback<T, X, Y, Z> callback) {
      OnListenerAdding(eventType, callback);
      EventTable[eventType] = (Callback<T, X, Y, Z>)EventTable[eventType] + callback;
    }

    //five parameters
    public static void AddListener<T, X, Y, Z, W>(EventType eventType, Callback<T, X, Y, Z, W> callback) {
      OnListenerAdding(eventType, callback);
      EventTable[eventType] = (Callback<T, X, Y, Z, W>)EventTable[eventType] + callback;
    }

    //no parameters
    public static void RemoveListener(EventType eventType, Callback callback) {
      OnListenerRemoving(eventType, callback);
      EventTable[eventType] = (Callback)EventTable[eventType] - callback;
      OnListenerRemoved(eventType);
    }

    //single parameters
    public static void RemoveListener<T>(EventType eventType, Callback<T> callback) {
      OnListenerRemoving(eventType, callback);
      EventTable[eventType] = (Callback<T>)EventTable[eventType] - callback;
      OnListenerRemoved(eventType);
    }

    //two parameters
    public static void RemoveListener<T, X>(EventType eventType, Callback<T, X> callback) {
      OnListenerRemoving(eventType, callback);
      EventTable[eventType] = (Callback<T, X>)EventTable[eventType] - callback;
      OnListenerRemoved(eventType);
    }

    //three parameters
    public static void RemoveListener<T, X, Y>(EventType eventType, Callback<T, X, Y> callback) {
      OnListenerRemoving(eventType, callback);
      EventTable[eventType] = (Callback<T, X, Y>)EventTable[eventType] - callback;
      OnListenerRemoved(eventType);
    }

    //four parameters
    public static void RemoveListener<T, X, Y, Z>(EventType eventType, Callback<T, X, Y, Z> callback) {
      OnListenerRemoving(eventType, callback);
      EventTable[eventType] = (Callback<T, X, Y, Z>)EventTable[eventType] - callback;
      OnListenerRemoved(eventType);
    }

    //five parameters
    public static void RemoveListener<T, X, Y, Z, W>(EventType eventType, Callback<T, X, Y, Z, W> callback) {
      OnListenerRemoving(eventType, callback);
      EventTable[eventType] = (Callback<T, X, Y, Z, W>)EventTable[eventType] - callback;
      OnListenerRemoved(eventType);
    }

    //no parameters
    public static void Broadcast(EventType eventType) {
      if (EventTable.TryGetValue(eventType, out Delegate d)) {
        if (d is Callback callback) {
          callback();
        } else {
          throw new Exception($"广播事件错误：事件{eventType}对应委托具有不同的类型");
        }
      }
    }

    //single parameters
    public static void Broadcast<T>(EventType eventType, T arg) {
      if (EventTable.TryGetValue(eventType, out Delegate d)) {
        if (d is Callback<T> callback) {
          callback(arg);
        } else {
          throw new Exception($"广播事件错误：事件{eventType}对应委托具有不同的类型");
        }
      }
    }

    //two parameters
    public static void Broadcast<T, X>(EventType eventType, T arg1, X arg2) {
      if (EventTable.TryGetValue(eventType, out Delegate d)) {
        if (d is Callback<T, X> callback) {
          callback(arg1, arg2);
        } else {
          throw new Exception($"广播事件错误：事件{eventType}对应委托具有不同的类型");
        }
      }
    }

    //three parameters
    public static void Broadcast<T, X, Y>(EventType eventType, T arg1, X arg2, Y arg3) {
      if (EventTable.TryGetValue(eventType, out Delegate d)) {
        if (d is Callback<T, X, Y> callback) {
          callback(arg1, arg2, arg3);
        } else {
          throw new Exception($"广播事件错误：事件{eventType}对应委托具有不同的类型");
        }
      }
    }

    //four parameters
    public static void Broadcast<T, X, Y, Z>(EventType eventType, T arg1, X arg2, Y arg3, Z arg4) {
      if (EventTable.TryGetValue(eventType, out Delegate d)) {
        if (d is Callback<T, X, Y, Z> callback) {
          callback(arg1, arg2, arg3, arg4);
        } else {
          throw new Exception($"广播事件错误：事件{eventType}对应委托具有不同的类型");
        }
      }
    }

    //five parameters
    public static void Broadcast<T, X, Y, Z, W>(EventType eventType, T arg1, X arg2, Y arg3, Z arg4, W arg5) {
      if (EventTable.TryGetValue(eventType, out Delegate d)) {
        if (d is Callback<T, X, Y, Z, W> callback) {
          callback(arg1, arg2, arg3, arg4, arg5);
        } else {
          throw new Exception($"广播事件错误：事件{eventType}对应委托具有不同的类型");
        }
      }
    }
  }
}