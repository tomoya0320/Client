using System.Collections.Generic;

namespace GameCore {
  public class UnitManager : TemplateManager<UnitTemplate> {
    private int IncId;
    private Dictionary<int, Unit> Units = new Dictionary<int, Unit>();
    public Dictionary<int, Unit>.ValueCollection AllUnits => Units.Values;

    public UnitManager(Battle battle) : base(battle) { }

    public Unit Create(Player player, UnitData unitData) {
      var unit = new Unit(Battle, ++IncId, player, unitData);
      Units.Add(unit.RuntimeId, unit);
      Battle.BuffManager.AddComponent(unit);
      return unit;
    }

    public Unit GetUnit(int runtimeId) {
      if (Units.TryGetValue(runtimeId, out Unit unit)) {
        return unit;
      }
      return null;
    }

    public void OnUnitDie(Unit unit) {
      if(!Units.ContainsKey(unit.RuntimeId)){
        return;
      }
      Battle.BuffManager.RemoveComponent(unit.RuntimeId);
      unit.Player.DeadUnitCount++;
    }
  }
}