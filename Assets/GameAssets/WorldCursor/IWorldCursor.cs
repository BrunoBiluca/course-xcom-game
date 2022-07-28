﻿using System;
using UnityEngine;
using UnityFoundation.Code;

namespace GameAssets
{
    public interface IWorldCursor
    {
        event Action OnClick;
        event Action OnSecondaryClick;

        Optional<Vector3> WorldPosition { get; }
        Optional<Vector2> ScreenPosition { get; }

        void Update();
    }
}