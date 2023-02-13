using System;
using UnityEngine;

namespace GameAssets
{
    public class GridIntentColor
    {
        public static Color Avaiable(IGridIntent intent)
        {
            return intent.IntentType switch {
                GridIntentType.Movement => Color.white,
                GridIntentType.Attack => Color.red,
                GridIntentType.Interact => Color.blue,
                GridIntentType.None => Color.white,
                _ => throw new NotImplementedException()
            };
        }

        public static Color Affected(IGridIntent intent)
        {
            return intent.IntentType switch {
                GridIntentType.Movement => Color.green,
                GridIntentType.Attack => Color.yellow,
                GridIntentType.Interact => Color.green,
                GridIntentType.None => Color.white,
                _ => throw new NotImplementedException()
            };
        }
    }
}
