using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public class GridUnitValue
    {
        public List<ITransform> Transforms { get; }

        public GridUnitValue()
        {
            Transforms = new List<ITransform>();
        }

        public void Add(ITransform transform)
        {
            if(Transforms.Contains(transform))
                return;
            Transforms.Add(transform);
        }

        public void Remove(ITransform transform)
        {
            Transforms.Remove(transform);
        }

        public override string ToString()
        {
            if(Transforms.Count == 0)
                return string.Empty;

            return string.Join(",\n", Transforms.Select(t => t.Name).ToArray());
        }
    }
}
