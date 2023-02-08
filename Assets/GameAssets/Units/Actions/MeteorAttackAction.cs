using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityFoundation.CharacterSystem.ActorSystem;

namespace GameAssets
{
    public class MeteorAttackAction : IAction
    {
        public event Action OnCantExecuteAction;
        public event Action OnFinishAction;

        public void Execute()
        {
            
        }
    }
}
