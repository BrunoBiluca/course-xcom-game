﻿using System;
using UnityFoundation.Code;

namespace GameAssets
{
    public partial interface IActor<TFactory> where TFactory : IAPActionIntent
    {
        event Action OnCantExecuteAction;
        event Action OnActionFinished;

        void Execute();
        void Set(TFactory action);
        void UnsetAction();
    }
}