using System.Collections.Generic;

namespace Battle {
  public class UnitManager : BattleBase {
    private Dictionary<int, Unit> RuntimeUnits = new Dictionary<int, Unit>();

    public UnitManager(BattleManager battleManager) : base(battleManager) {
      
    }

    public Unit GetUnit(int runtimeId) {
      RuntimeUnits.TryGetValue(runtimeId, out Unit unit);
      return unit;
    }
  }
}