using UnityEngine;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public class BlockadeUnit : MonoBehaviour, IUnit
    {
        public string Name => gameObject.name;

        public ITransform Transform { get; private set; }

        public UnitFactions Faction => UnitFactions.Furniture;

        public void Awake()
        {
            Transform = new TransformDecorator(transform);
        }


    }
}
