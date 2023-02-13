using UnityFoundation.Code.Grid;
using UnityFoundation.HealthSystem;

namespace GameAssets
{
    public class DamageableUnitGridValidation : IGridValidation<UnitValue>
    {
        private readonly DamageableLayer characterLayer;

        public DamageableUnitGridValidation(DamageableLayer characterLayer)
        {
            this.characterLayer = characterLayer;
        }

        public bool IsAvailable(GridCellXZ<UnitValue> cell)
        {
            if(Equals(cell.Value, default)) return false;
            if(cell.Value.Units.Count == 0) return false;

            var unit = cell.Value.Units[0];
            if(unit is not IDamageableUnit target)
                return false;

            return DamageableLayerManager.I.LayerCanDamage(characterLayer, target.Damageable.Layer);
        }
    }
}
