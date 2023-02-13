using Moq;
using NUnit.Framework;
using UnityEngine;
using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;

namespace GameAssets.Tests
{
    public class GridIntentSelectorTests
    {
        [Test]
        public void Should_return_no_available_cells_when_no_intent_was_selected()
        {
            var intentSelector = new Mock<IGridIntentSelector>();
            intentSelector.Setup(s => s.CurrentIntent).Returns(Optional<IGridIntent>.None());
            var gridManager = new UnitWorldGridManagerFakeBuilder().Build();

            var gridQuery = new GridIntentQuery(intentSelector.Object, gridManager);

            var cells = gridQuery.GetAvaiableCells();

            Assert.That(cells, Is.Empty);
        }

        [Test]
        public void Should_return_available_cells_when_move_intent_is_selected()
        {
            var intentSelector = new Mock<IGridIntentSelector>();
            intentSelector.Setup(s => s.CurrentIntent)
                .Returns(Optional<IGridIntent>.Some(new MoveIntentFake().Build()));
            var gridManager = new UnitWorldGridManagerFakeBuilder().Build();

            var gridIntentSelector = new GridIntentQuery(intentSelector.Object, gridManager);

            var cells = gridIntentSelector.GetAvaiableCells();

            Assert.That(cells.Count, Is.EqualTo(2));

            var affectedCells = gridIntentSelector.GetAffectedCells(Vector3.forward);

            Assert.That(affectedCells.Count, Is.EqualTo(1));
        }

        [Test]
        public void Should_return_available_cells_when_area_attack_intent_is_selected()
        {
            var intentSelector = new Mock<IGridIntentSelector>();
            intentSelector.Setup(s => s.CurrentIntent)
                .Returns(Optional<IGridIntent>.Some(new AreaAttackIntentFake().Build()));
            var gridManager = new UnitWorldGridManagerFakeBuilder().Build();

            var gridIntentSelector = new GridIntentQuery(intentSelector.Object, gridManager);

            var cells = gridIntentSelector.GetAvaiableCells();

            Assert.That(cells.Count, Is.EqualTo(3));

            var affectedCells = gridIntentSelector.GetAffectedCells(Vector3.forward);

            Assert.That(affectedCells.Count, Is.EqualTo(4));
        }

        [Test]
        public void Should_return_available_cells_when_shoot_intent_is_selected()
        {
            var gridManager = new UnitWorldGridManagerFakeBuilder()
                .AddUnit(UnitFactions.Enemy, Vector3.forward)
                .AddUnit(UnitFactions.Enemy, Vector3.right)
                .Build();

            var intentSelector = new Mock<IGridIntentSelector>();
            intentSelector.Setup(s => s.CurrentIntent)
                .Returns(Optional<IGridIntent>.Some(new ShootIntentFake().Build()));

            var gridIntentSelector = new GridIntentQuery(intentSelector.Object, gridManager);

            var cells = gridIntentSelector.GetAvaiableCells();

            Assert.That(cells.Count, Is.EqualTo(2));

            var affectedCells = gridIntentSelector.GetAffectedCells(Vector3.forward);

            Assert.That(affectedCells.Count, Is.EqualTo(1));
        }
    }
}
