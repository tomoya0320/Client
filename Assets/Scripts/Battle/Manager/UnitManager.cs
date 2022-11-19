using System.Collections.Generic;
using UnityEngine;

namespace GameCore {
  public class UnitManager : AssetManager<UnitTemplate> {
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

    public static bool CheckTargetCamp(PlayerCamp playerCamp, Unit owner, Unit mainTarget) {
      if (owner == null || mainTarget == null) {
        return false;
      }

      switch (playerCamp) {
        case PlayerCamp.NONE:
          return false;
        case PlayerCamp.ALL:
          return true;
        case PlayerCamp.ALLY:
          return owner.PlayerCamp == mainTarget.PlayerCamp;
        case PlayerCamp.ENEMY:
          return owner.PlayerCamp != mainTarget.PlayerCamp;
        default:
          return false;
      }
    }

    public void GetUnitList(PlayerCamp playerCamp, Unit owner, List<Unit> list) {
      list.Clear();
      foreach (var unit in AllUnits) {
        if (CheckTargetCamp(playerCamp, owner, unit)) {
          list.Add(unit);
        }
      }
    }

    public Unit GetNearestUnit(Vector3 pos, PlayerCamp playerCamp, Unit owner) {
      Unit result = null;
      float minDist = float.MaxValue;
      var unitList = TempList<Unit>.Get();
      GetUnitList(playerCamp, owner, unitList);
      foreach (var unit in unitList) {
        float dist = Vector3.Distance(pos, unit.UIUnit.transform.position);
        if (dist < minDist) {
          minDist = dist;
          result = unit;
        }
      }
      TempList<Unit>.Release(unitList);
      return result;
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
    }
  }
}