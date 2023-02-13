using Moq;
using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets.Tests
{
    public partial class UnitSelectionTests
    {
        public class TestCase
        {
            private readonly GameObject unit;
            private readonly Mock<IRaycastHandler> raycastHandler;

            public TestCase()
            {
                raycastHandler = new Mock<IRaycastHandler>();

                unit = new GameObject("unit");
                unit.AddComponent<SelectableMono>();
                unit.AddComponent<BoxCollider>();
            }

            public SelectableMono GetSelectableUnit()
            {
                return unit.GetComponent<SelectableMono>();
            }

            public TestCase FoundUnit()
            {
                raycastHandler
                    .Setup(rh => rh.GetObjectOf<ISelectable>(Vector2.zero, 0))
                    .Returns(unit.GetComponent<ISelectable>());

                return this;
            }

            public TestCase NotFoundUnit()
            {
                raycastHandler
                    .Setup(rh => rh.GetObjectOf<ISelectable>(Vector2.zero, 0))
                    .Returns((ISelectable)default);

                return this;
            }

            public IRaycastHandler GetRaycastHandler()
            {
                return raycastHandler.Object;
            }

            public void DestroyUnit()
            {
                UnityEngine.Object.DestroyImmediate(unit);
            }
        }

    }
}