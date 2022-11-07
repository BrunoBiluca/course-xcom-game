using System.Collections.Generic;
using System.Linq;
using UnityFoundation.Code;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public class GridUnitValue : IEmptyable
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
            transform.OnInvalidState += () => Remove(transform);
        }

        public void Remove(ITransform transform)
        {
            Transforms.Remove(transform);
        }

        public bool IsEmpty()
        {
            return Transforms.Count == 0;
        }

        public override string ToString()
        {
            if(Transforms.Count == 0)
                return string.Empty;

            return string.Join(",\n", Transforms.Select(t => t.Name).ToArray());
        }

        public void Clear()
        {
            Transforms.Clear();
        }
    }
}
