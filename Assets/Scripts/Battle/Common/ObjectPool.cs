using System;
using System.Collections.Generic;

namespace GameCore {
  public interface IPoolObject {
    void Release();
  }

  public class ObjectPool {
    private Dictionary<Type, Stack<IPoolObject>> PoolObjects = new Dictionary<Type, Stack<IPoolObject>>();

    public T Get<T>() where T : class, IPoolObject, new() {
      if (!PoolObjects.TryGetValue(typeof(T), out var stack)) {
        stack = new Stack<IPoolObject>();
        PoolObjects.Add(typeof(T), stack);
      }

      if (!stack.TryPop(out var obj)) {
        obj = new T();
      }
      return obj as T;
    }

    public void Release<T>(T obj) where T : class, IPoolObject {
      if (!PoolObjects.TryGetValue(typeof(T), out var stack)) {
        stack = new Stack<IPoolObject>();
        PoolObjects.Add(typeof(T), stack);
      }
      obj.Release();
      stack.Push(obj);
    }

    public void Release() {
      foreach (var stack in PoolObjects.Values) {
        stack.Clear();
      }
      PoolObjects.Clear();
    }
  }
}