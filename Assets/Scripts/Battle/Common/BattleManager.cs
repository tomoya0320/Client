using System.Threading.Tasks;
using UnityEngine;

namespace Battle {
  public enum BattleState {
    None,
    Loading,
    Running,
    Settle,
    Exit,
  }

  public class BattleManager {
    private BattleData BattleData;
    private BattleState BattleState = BattleState.None;

    public static BattleManager Enter(BattleData battleData) {
      BattleManager battleManager = new BattleManager {
        BattleData = battleData,
      };

      battleManager.Load();

      return battleManager;
    }

    private async void Load() {
      BattleState = BattleState.Loading;

      // Test
      await Task.Delay(1000);
      Debug.Log("战斗资源加载完毕");

      
      Run();
    }

    private async void Run() {
      BattleState = BattleState.Running;
      while (BattleState == BattleState.Running) {
        await BeforeUpdate();
        await Update();
        await LateUpdate();
      }
    }

    public async void Exit(bool force = false) {
      BattleState = BattleState.Exit;

      // Test
      await Task.Delay(1000);
      Debug.Log("退出战斗");
    }

    private async Task BeforeUpdate() {
      // Test
      await Task.Delay(1000);
      Debug.Log(nameof(BeforeUpdate));
    }

    private async Task Update() {
      // Test
      await Task.Delay(1000);
      Debug.Log(nameof(Update));
    }

    private async Task LateUpdate() {
      // Test
      await Task.Delay(1000);
      Debug.Log(nameof(LateUpdate));
    }
  }
}