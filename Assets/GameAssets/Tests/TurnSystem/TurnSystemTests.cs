using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAssets.Tests
{
    public class TurnSystemTests
    {

        [Test]
        public void Should_update_turn_count_when_turn_changed()
        {
            var turnSystem = new TurnSystem();

            turnSystem.EndTurn();

            Assert.That(turnSystem.CurrentTurn, Is.EqualTo(2));
        }

        [Test]
        public void Should_execute_action_when_turn_changed()
        {
            var turnSystem = new TurnSystem();

            var wasExecuted = false;
            turnSystem.OnTurnEnded += () => wasExecuted = true;

            turnSystem.EndTurn();

            Assert.That(wasExecuted, Is.True);
        }

    }
}