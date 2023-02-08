using System.Collections;
using System.Collections.Generic;

namespace GameAssets
{
    public enum UnitAnimationEvents
    {
        SHOT,
        MELEE
    }

    public class UnitAnimationEventProxy : AnimationEventProxy<UnitAnimationEvents>
    {
    }
}
