using Cysharp.Threading.Tasks;
using GameCore.MagicFuncs;
using GameCore.UI;
using System;
using UnityEngine;

namespace GameCore {
  public class Buff : IPoolObject {
    public int RuntimeId { get; private set; }
    public BuffComponent BuffComponent { get; private set; }
    public BuffTemplate BuffTemplate { get; private set; }
    public BuffContext BuffContext { get; private set; }
    public MagicFuncBase Magic => BuffTemplate.Magic?.Asset as MagicFuncBase;
    public Unit Source { get; private set; }
    public Unit Target { get; private set; }
    public int Turn { get; private set; }
    public UIBuff UIBuff { get; private set; }

    public void Init(int runtimeId, BuffComponent buffComponent, BuffTemplate buffTemplate, Unit source, Unit target) {
      RuntimeId = runtimeId;
      BuffComponent = buffComponent;
      BuffTemplate = buffTemplate;
      Source = source;
      Target = target;
      Turn = 0;
      BuffContext = Target.Battle.ObjectPool.Get<BuffContext>();
      BuffContext.Buff = this;
      UIBuff = Target.UIUnit.OnAddBuff(this);
    }

    public async UniTask<bool> Update(TickTime updateTime) {
      if (updateTime == BuffTemplate.UpdateTime) {
        var magic = ++Turn == BuffTemplate.Delay ? BuffTemplate.Magic?.Asset : BuffTemplate.IntervalMagic?.Asset;
        await Target.Battle.MagicManager.DoMagic(magic as MagicFuncBase, Source, Target, BuffContext);
        UIBuff.OnUpdate();
        return BuffTemplate.Duration < 0 || Turn < BuffTemplate.TotalDuration;
      }

      return true; // 只有符合更新时机才会去判断条件否则统一返回true避免magic里又触发一次buff更新逻辑导致只剩一回合的buff被提前回收
    }

    public string GetLeftTurnText() {
      int delayLeftTurn = Mathf.Max(BuffTemplate.Delay - Turn, 0);
      int leftTurn = BuffTemplate.Duration < 0 ? -1 : (delayLeftTurn > 0 ? BuffTemplate.Duration : BuffTemplate.TotalDuration - Turn);
      return $"{(leftTurn < 0 ? "∞" : leftTurn)}{(delayLeftTurn > 0 ? $"({delayLeftTurn})" : string.Empty)}";
    }

    public void Release() {
      Target.UIUnit.OnRemoveBuff(UIBuff);
      UIBuff = null;
      Target.Battle.ObjectPool.Release(BuffContext);
      BuffContext = null;
      BuffComponent = null;
      BuffTemplate = null;
      Source = null;
      Target = null;
      RuntimeId = 0;
      Turn = 0;
    }
  }
}