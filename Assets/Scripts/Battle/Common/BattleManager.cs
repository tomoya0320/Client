using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle {
  public enum BattleState {
    None, // 退出循环的标记
    Loading,
    Running,
    Settle,
    Exit,
  }
  public class BattleManager {
    private BattleData BattleData;
    private BattleState BattleState = BattleState.None;
    public static BattleManager Instance { get; private set; }

    public static void Enter(BattleData battleData) {
      if(Instance != null) {
        Debug.LogError("上一场战斗未结束!");
        return;
      }

      // 战斗数据初始化
      Instance = new BattleManager {
        BattleData = battleData,
      };

      Instance.Run().Forget();
    }

    public async UniTaskVoid Run() {
      do {
        switch (BattleState) {
          case BattleState.None:
            BattleState = BattleState.Loading;
            break;
          case BattleState.Loading:
            await Load();
            BattleState = BattleState.Running;
            break;
          case BattleState.Running:
            await BeforeUpdate();
            await Update();
            await LateUpdate();
            break;
          case BattleState.Settle:
            await Settle();
            BattleState = BattleState.Exit;
            break;
          case BattleState.Exit:
            await Exit();
            BattleState = BattleState.None;
            break;
        }
      }
      while (BattleState != BattleState.None);
    }

    private async UniTask Load() {
      // Test
      await UniTask.Delay(1000);
      Debug.Log("战斗资源加载完毕");
    }

    private async UniTask Settle() {
      // Test
      await UniTask.Delay(1000);
      Debug.Log("战斗结算");
    }

    private async UniTask Exit(bool force = false) {
      // Test
      await UniTask.Delay(1000);
      Instance = null;
      Debug.Log("退出战斗");
    }

    private async UniTask BeforeUpdate() {
      // Test
      await UniTask.Delay(1000);
      Debug.Log("BeforeUpdate");
    }

    private async UniTask Update() {
      // Test
      await UniTask.Delay(1000);
      Debug.Log("Update");
    }

    private async UniTask LateUpdate() {
      // Test
      await UniTask.Delay(1000);
      Debug.Log("LateUpdate");
    }
  }
}