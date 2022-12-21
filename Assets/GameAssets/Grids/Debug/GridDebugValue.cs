using TMPro;
using UnityEngine;

namespace GameAssets
{
    public class GridDebugValue
    {
        private readonly TextMeshPro text;

        public GridDebugValue(
            TextMeshPro text
        )
        {
            this.text = text;
        }

        public void SetText(string text)
        {
            this.text.text = text;
        }
    }
}
