using System;
using System.Collections.Generic;

namespace Battle {
  public interface IPoolObject {
    void Release();
  }

  public class ObjectPool {
    private Dictionary<Type, Stack<IPoolObject>> PoolObjects = new Dictionary<Type, Stack<IPoolObject>>();

    public T Get<T>() where T : class, IPoolObject, new() {
      if(!PoolObjects.TryGetValue(typeof(T), out var stack)) {
        stack = new Stack<IPoolObject>();
        PoolObjects.Add(typeof(T), stack);
      }
      T obj;
      if (stack.Count > 0) {
        obj = stack.Pop() as T;
      } else {
        obj = new();
      }
      return obj;
    }

    public void Release<T>(T obj) where T : class, IPoolObject, new() {
      if (!PoolObjects.TryGetValue(typeof(T), out var stack)) {
        stack = new Stack<IPoolObject>();
        PoolObjects.Add(typeof(T), stack);
      }
      obj.Release();
      stack.Push(obj);
    }
  }
}