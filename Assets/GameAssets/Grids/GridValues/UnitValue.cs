using System.Collections.Generic;
using System.Linq;
using UnityFoundation.Code;

namespace GameAssets
{
    public class UnitValue : IEmptyable
    {
        public List<IUnit> Units { get; }

        public UnitValue()
        {
            Units = new List<IUnit>();
        }

        public void Add(IUnit unit)
        {
            if(Units.Contains(unit))
                return;

            Units.Add(unit);
        }

        public void Remove(IUnit unit)
        {
            Units.Remove(unit);
        }

        public bool IsEmpty()
        {
            return Units.Count == 0;
        }

        public void Clear()
        {
            Units.Clear();
        }

        public override string ToString()
        {
            if(Units.Count == 0)
                return string.Empty;

            return string.Join(",\n", Units.Select(t => t.Name).ToArray());
        }

    }
}
