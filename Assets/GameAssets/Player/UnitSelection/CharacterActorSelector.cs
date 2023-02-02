using System;
using UnityFoundation.CharacterSystem.ActorSystem;

namespace GameAssets
{
    public sealed class CharacterActorSelector : IActorSelector<IAPActor>
    {
        private readonly IActorSelector<ICharacterUnit> characterSelector;

        public IAPActor CurrentUnit => characterSelector.CurrentUnit?.Actor;

        public event Action OnUnitSelected;
        public event Action OnUnitUnselected;

        public CharacterActorSelector(IActorSelector<ICharacterUnit> characterSelector)
        {
            this.characterSelector = characterSelector;

            characterSelector.OnUnitSelected += () => OnUnitSelected?.Invoke();
            characterSelector.OnUnitUnselected += () => OnUnitUnselected?.Invoke();
        }

        public void UnselectUnit()
        {
            characterSelector.UnselectUnit();
        }
    }

}