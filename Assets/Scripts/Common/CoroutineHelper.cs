using System;
using System.Collections;
using UnityEngine;

public class CoroutineHelper : SingletonMono<CoroutineHelper> {
  public Coroutine Run(IEnumerator enumerator) {
    return StartCoroutine(enumerator);
  }

  public Coroutine DelayInvoke(float time, Action callback) {
    return StartCoroutine(_DelayInvokeInternal(time, callback));
  }

  public void Stop(IEnumerator enumerator) {
    StopCoroutine(enumerator);
    // 嵌套的协程只能通过这种方式来终止
    while (enumerator.Current is IEnumerator current) {
      enumerator = current;
      StopCoroutine(enumerator);
    }
  }

  private IEnumerator _DelayInvokeInternal(float time, Action callback) {
    yield return new WaitForSeconds(time);
    callback?.Invoke();
  }
}