using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineHelper : SingletonMono<TimelineHelper> {
  private readonly Dictionary<PlayableDirector, Action<bool>> AnimCallbacks = new Dictionary<PlayableDirector, Action<bool>>();
  private readonly Dictionary<PlayableDirector, Action<bool>> CompletedAnimCallbacks = new Dictionary<PlayableDirector, Action<bool>>();

  public void PlayAnim(PlayableDirector playableDirector, Action<bool> callback) {
    if(AnimCallbacks.TryGetValue(playableDirector, out var beforeCallback)) {
      beforeCallback?.Invoke(false);
    }
    playableDirector.time = 0;
    AnimCallbacks[playableDirector] = callback;
  }

  private void Update() {
    foreach (var kv in AnimCallbacks) {
      kv.Key.time += Time.deltaTime;
      if(kv.Key.time >= kv.Key.duration) {
        kv.Key.time = kv.Key.duration;
        CompletedAnimCallbacks.Add(kv.Key, kv.Value);
      }
    }
  }

  private void LateUpdate() {
    if(CompletedAnimCallbacks.Count > 0) {
      foreach (var kv in CompletedAnimCallbacks) {
        kv.Value?.Invoke(true);
        AnimCallbacks.Remove(kv.Key);
      }
      CompletedAnimCallbacks.Clear();
    }
  }
}
