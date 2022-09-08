using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public class GridUnitValue
    {
        public TextMeshPro Text { get; }
        public List<ITransform> Transforms { get; }

        public GridUnitValue(TextMeshPro text)
        {
            Text = text;
            Transforms = new List<ITransform>();
        }

        public void Add(ITransform transform)
        {
            if(Transforms.Contains(transform))
                return;
            Transforms.Add(transform);
            Text.text = ToString();
        }

        public void Remove(ITransform transform)
        {
            Transforms.Remove(transform);
            Text.text = ToString();
        }

        public override string ToString()
        {
            if(Transforms.Count == 0)
                return string.Empty;

            return string.Join(",\n", Transforms.Select(t => t.Name).ToArray());
        }
    }
}
